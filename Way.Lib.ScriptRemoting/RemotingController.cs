﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public interface IUploadFileHandler
    {
        void OnGettingFileData(byte[] data);
        void OnUploadFileCompleted();
        void OnUploadFileError();
    }
    /// <summary>
    /// 分页信息定义
    /// </summary>
    public class PagerInfo
    {
        public int PageIndex;
        public int PageSize;
    }

    public class DatasourceDefine
    {
        /// <summary>
        /// 数据源所属类型
        /// </summary>
        public Type TargetType { get; set; }
        /// <summary>
        /// 属性名称或者方法名称
        /// </summary>
        public string PropertyOrMethodName { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class RemotingController
    {
        internal class ParseHtmlInfo
        {
            public string Url;
            public string Controller;
            public DateTime LastModified;
            public List<string> Datasources = new List<string>();
            public List<string> AllowEditDatasources = new List<string>();
        }
        public class RequestHeaderCollection:Dictionary<string,string>
        {
            RemotingClientHandler.GetHeaderValueHandler _GetHeaderValueHandler;
            internal RequestHeaderCollection(RemotingClientHandler.GetHeaderValueHandler getHeaderValueHandler)
            {
                _GetHeaderValueHandler = getHeaderValueHandler;
            }
            public string this[string key]
            {
                get
                {
                    if (_GetHeaderValueHandler == null)
                        return null;
                    return _GetHeaderValueHandler(key);
                }
            }
        }

        static List<string> SafeDomains = new List<string>();
        internal string SocketID;
        public SessionState Session
        {
            get;
            internal set;
        }
        public delegate bool OnMessageReceiverConnectHandler(SessionState session, string groupName);
        /// <summary>
        /// 当有客户端试图连接，接收group信息时触发，您可以return false，或者throw Exception，拒绝对方连接
        /// </summary>
        public static event OnMessageReceiverConnectHandler OnMessageReceiverConnect;
        internal static bool MessageReceiverConnect(SessionState session, string groupName)
        {
            if (OnMessageReceiverConnect == null)
                return true;
            return OnMessageReceiverConnect(session, groupName);
        }


        /// <summary>
        /// 获取web所在文件夹
        /// </summary>
        public static string WebRoot
        {
            get
            {
                return ScriptRemotingServer.Root;
            }
        }
        public RequestHeaderCollection RequestHeaders
        {
            get;
            internal set;
        }

        static void CheckHtmlFile(ParseHtmlInfo info,List<HtmlUtil.HtmlNode> nodes,string webroot, string weburl)
        {
            try
            {
                foreach (var node in nodes)
                {

                    if (node.Name == null)
                    {
                        if (node is HtmlUtil.HtmlTextBlock && ((HtmlUtil.HtmlTextBlock)node).Text.IsNullOrEmpty() == false)
                        {
                            if (string.Equals(node.Parent.Name, "script", StringComparison.CurrentCultureIgnoreCase))
                            {
                                //继续解析外嵌的html
                                var matches = Regex.Matches(((HtmlUtil.HtmlTextBlock)node).Text, @"WayHelper.writePage\((?<g1>(\w|\.|\/|\:| |\""|\')+)\)");
                                foreach (Match m in matches)
                                {
                                    string url = m.Groups["g1"].Value.Trim();
                                    url = url.Substring(1, url.Length - 2);
                                    if (url.StartsWith("/"))
                                    {
                                        url = webroot + url.Substring(1);
                                    }
                                    else
                                    {
                                        url = weburl + url;
                                    }

                                    try
                                    {
                                        HttpClient client = new HttpClient();
                                        var task = client.GetStreamAsync(url);
                                        task.Wait();
                                        Way.Lib.HtmlUtil.HtmlParser parser = new HtmlUtil.HtmlParser();
                                        var stream = new System.IO.StreamReader(task.Result);
                                        parser.Parse(stream);
                                        stream.Dispose();
                                        CheckHtmlFile(info, parser.Nodes, webroot, weburl);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            else
                            {
                                var matches = Regex.Matches(((HtmlUtil.HtmlTextBlock)node).Text, @"\{\@(\w|\.)+\:(?<g1>(\w|\.)+)\:(\w|\.)+\}");
                                foreach (Match m in matches)
                                {
                                    string fullname = m.Groups["g1"].Value;
                                    fullname = fullname.Substring(0, fullname.LastIndexOf("."));
                                    if(info.Datasources.Contains(fullname) == false && fullname.StartsWith("[") == false)
                                        info.Datasources.Add(fullname);
                                }
                            }
                        }
                        continue;
                    }

                    else if (info.Controller.IsNullOrEmpty() && String.Equals(node.Name, "body", StringComparison.CurrentCultureIgnoreCase))
                    {
                        info.Controller = (from m in node.Attributes where m.Name == "controller" select m.Value).FirstOrDefault();
                        if (info.Controller == null)
                            info.Controller = "";
                    }
                    else
                    {
                        var _datasource = (from m in node.Attributes where m.Name == "datasource" select m.Value).FirstOrDefault();
                        if (_datasource != null && _datasource.StartsWith("[") == false)
                        {
                            if (info.Datasources.Contains(_datasource) == false)
                                info.Datasources.Add(_datasource);
                            if (node.Attributes.Any(m => m.Name == "allowedit" && m.Value == "true"))
                            {
                                if (info.AllowEditDatasources.Contains(_datasource) == false)
                                    info.AllowEditDatasources.Add(_datasource);
                            }
                        }
                    }
                    CheckHtmlFile(info, node.Nodes, webroot,weburl);
                }
            }
            catch(Exception ex)
            {

            }
        }
        internal void onLoad()
        {
            this.OnLoad();
        }
        protected virtual void OnLoad()
        {
        }

        internal void _OnBeforeInvokeMethod(MethodInfo method)
        {
            OnBeforeInvokeMethod(method);
        }
        protected virtual void OnBeforeInvokeMethod(MethodInfo method)
        { }

        Type getType(string fullname)
        {
            Assembly currentAssembly = this.GetType().GetTypeInfo().Assembly;
            Type type = null;
            if (DataSourceTypes.ContainsKey(fullname))
            {
                type = DataSourceTypes[fullname];
            }
            else
            {
                type = currentAssembly.GetType(fullname);
                if (type == null)
                {
                    Assembly[] assemblies = PlatformHelper.GetAppAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        if (assembly != currentAssembly)
                        {
                            type = assembly.GetType(fullname);
                            if (type != null)
                                break;
                        }
                    }
                }
                if (type == null)
                {
                    throw new Exception("无法找到类" + fullname);
                }
                //if (type.GetTypeInfo().GetCustomAttribute(typeof(RemotingMethodAttribute)) == null)
                //{
                //    throw new Exception("类" + fullname + "没有标记RemotingMethod");
                //}
                try
                {
                    DataSourceTypes.Add(fullname, type);
                }
                catch
                {
                }
            }
            return type;
        }

        static void getSearchingDictionary(Dictionary<string, object> dic,string root , Newtonsoft.Json.Linq.JObject searchModel)
        {
            foreach (var jobj in searchModel)
            {
                string name = jobj.Key;
                var valueObj = jobj.Value;
                if (valueObj is Newtonsoft.Json.Linq.JValue)
                {
                    var value = ((Newtonsoft.Json.Linq.JValue)valueObj).Value;
                    if (value != null)
                    {
                        if (value is string)
                        {
                            string strvalue = value.ToString().Trim();
                            if (strvalue.Length > 0)
                            {
                                dic.Add(root + name, strvalue);
                            }
                        }
                        else
                        {
                            dic.Add(root + name, value);
                        }
                    }
                }
                else if (valueObj is Newtonsoft.Json.Linq.JObject)
                {
                    getSearchingDictionary(dic, name + ".", valueObj as Newtonsoft.Json.Linq.JObject);
                }
            }
        }

        static object search(object result, Type dataItemType, Newtonsoft.Json.Linq.JObject searchModel)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            getSearchingDictionary(dic,"", searchModel);
            if (dic.Count == 0)
                return result;

            ParameterExpression param = Expression.Parameter(dataItemType, "n");
            Expression totalExpression = null;
            foreach (var searchKeyPair in dic)
            {

                PropertyInfo pinfo;
                Expression paramExpression = ResultHelper.GetPropertyExpression(param, dataItemType, searchKeyPair.Key, out pinfo);
                Type ptype = pinfo.PropertyType;

                if (ptype.GetTypeInfo().IsGenericType)
                {
                    ptype = ptype.GetGenericArguments()[0];
                    paramExpression = Expression.Convert(paramExpression, ptype);
                }
                List<Expression> doneExpressions = new List<Expression>();
                if (ptype == typeof(string))
                {
                    if (searchKeyPair.Value.ToSafeString().StartsWith("equal:"))
                    {
                        Expression right = Expression.Constant(searchKeyPair.Value.ToString().Substring(6));
                        doneExpressions.Add(Expression.Equal(paramExpression, right));
                    }
                    else
                    {
                        doneExpressions.Add(Expression.Call(paramExpression,
                            typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                            Expression.Constant(searchKeyPair.Value)));
                    }
                }
                else if (ptype == typeof(int) || ptype == typeof(double) || ptype == typeof(decimal) || ptype == typeof(float) || ptype == typeof(short) || ptype == typeof(long))
                {
                    string[] pairValues = searchKeyPair.Value.ToString().Split(' ');
                    for(int i = 0; i < pairValues.Length; i ++)
                    {
                        string value = pairValues[i];
                        if (value.Length == 0)
                            continue;
                        if (value.StartsWith(">="))
                        {
                            value = value.Replace(">=", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            doneExpressions.Add(Expression.GreaterThanOrEqual(paramExpression, right));
                        }
                        else if (value.StartsWith(">"))
                        {
                            value = value.Replace(">", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            doneExpressions.Add(Expression.GreaterThan(paramExpression, right));
                        }
                        else if (value.StartsWith("<="))
                        {
                            value = value.Replace("<=", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            doneExpressions.Add(Expression.LessThanOrEqual(paramExpression, right));
                        }
                        else if (value.StartsWith("<"))
                        {
                            value = value.Replace("<", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            doneExpressions.Add(Expression.LessThan(paramExpression, right));
                        }
                        else
                        {
                            //等式右边的值
                            try
                            {
                                value = value.Replace("=", "");
                                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                                doneExpressions.Add(Expression.Equal(paramExpression, right));
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    
                }
                //else if (control is System.Web.UI.WebControls.ListControl || control is TextBoxList || control is ParentToChildSelector)
                //{
                //    if (ptype.IsEnum)
                //    {
                //        //等式右边的值
                //        Expression right = Expression.Constant(Enum.Parse(ptype, value));
                //        left = Expression.Equal(left, right);
                //    }
                //    else
                //    {
                //        //等式右边的值
                //        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //        left = Expression.Equal(left, right);
                //    }


                //}
                //else if (control.Attributes["_IsDate"] == "1")
                //{
                //    if (control.Attributes["_IsFrom"] == "1")
                //    {
                //        if (control.Attributes["_IsTo"] == "1")
                //        {
                //            //应该是指定月份，或者年份
                //            var ms = System.Text.RegularExpressions.Regex.Matches(value, @"[0-9]+");
                //            if (ms.Count == 1)
                //            {
                //                //只有年份
                //                Expression expression1;

                //                //等式右边的值
                //                value = string.Format("{0}-1-1", ms[0]);
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = string.Format("{0}-1-1", ms[0].Value.ToInt() + 1);
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.And(expression1, right);
                //            }
                //            else if (ms.Count == 2)
                //            {
                //                //有月份
                //                DateTime monthdate = Convert.ToDateTime(value);

                //                Expression expression1;
                //                //等式右边的值
                //                value = monthdate.ToString("yyyy-MM-01");
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = monthdate.AddMonths(1).ToString("yyyy-MM-01");
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.And(expression1, right);
                //            }
                //            else if (ms.Count == 3)
                //            {
                //                //有日
                //                DateTime day = Convert.ToDateTime(value);
                //                Expression expression1;
                //                //等式右边的值
                //                value = day.ToString("yyyy-MM-dd");
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = day.AddDays(1).ToString("yyyy-MM-dd");
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.AndAlso(expression1, right);
                //            }
                //        }
                //        else
                //        {
                //            //等式右边的值
                //            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //            left = Expression.GreaterThanOrEqual(left, right);
                //        }
                //    }
                //    else
                //    {
                //        if (value.Contains(":"))
                //        {
                //            value = value.ToDateTime().AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                //        }
                //        else
                //        {
                //            value = value.ToDateTime().ToString("yyyy-MM-dd").ToDateTime().AddDays(1).ToString("yyyy-MM-dd");
                //        }
                //        //等式右边的值
                //        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //        left = Expression.LessThan(left, right);
                //    }
                //}

               foreach( var itemexpression in doneExpressions )
                {
                    //left = Expression.Lambda(left, param);
                    if (totalExpression == null)
                        totalExpression = itemexpression;
                    else
                        totalExpression = Expression.AndAlso(totalExpression, itemexpression);
                }
            }

            Type queryableType = (result is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            if (totalExpression != null)
            {
                totalExpression = Expression.Lambda(totalExpression, param);
                var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Where");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    try
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataItemType);
                        result = mmm.Invoke(null, new object[] { result, totalExpression });
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return result;
        }

        static Dictionary<string, Type> DataSourceTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 根据数据源名称，返回数据源路径
        /// </summary>
        /// <param name="datasourceName">数据源名称</param>
        /// <param name="isForEditing">此数据源是否要被编辑</param>
        /// <returns></returns>
        protected virtual DatasourceDefine OnGetDataSourcePath(string datasourceName,bool isForEditing)
        {
            string fullname = this.GetType().FullName;

            //if (isForEditing)
            //{
               
            //    if (ParsedHtmls.Any(m=> m.Controller == fullname && m.AllowEditDatasources.Contains(datasourceName)) == false)
            //        throw new Exception("无权编辑" + datasourceName);
            //}
            //else
            //{
            //    if (ParsedHtmls.Any(m => m.Controller == fullname && m.Datasources.Contains(datasourceName)) == false)
            //        throw new Exception("此html没有在任何地方定义使用" + datasourceName);
            //}
           
            int index = datasourceName.LastIndexOf(".");
            fullname = datasourceName.Substring(0 , index);
            string propertyName = datasourceName.Substring(index + 1);
            return new DatasourceDefine()
            {
                TargetType = getTypeDefine(fullname),
                PropertyOrMethodName = propertyName,
            };
        }

        static Dictionary<string, Type> ExistTypes = new Dictionary<string, Type>();
        static Type getTypeDefine(string remoteName)
        {
            Type pageDefine = null;
            try
            {
                if (ExistTypes.ContainsKey(remoteName))
                {
                    pageDefine = ExistTypes[remoteName];
                }
                else
                {
                    Assembly[] assemblies = PlatformHelper.GetAppAssemblies();

                    for (int i = 0; i < assemblies.Length; i++)
                    {
                        var type = assemblies[i].GetType(remoteName);
                        if (type != null)
                        {
                            pageDefine = type;
                            break;
                        }
                    }
                }
            }
            catch
            {
                return ExistTypes[remoteName];
            }
            return pageDefine;
        }

        object GetDataSource(DatasourceDefine datasourceDefine, Dictionary<Type, object> activeObjs)
        {
            object obj = null;
            if (activeObjs == null || activeObjs.ContainsKey(datasourceDefine.TargetType) == false)
            {
                obj = Activator.CreateInstance(datasourceDefine.TargetType);
                if (activeObjs != null)
                {
                    activeObjs[datasourceDefine.TargetType] = obj;
                }
            }
            else
            {
                obj = activeObjs[datasourceDefine.TargetType];
            }

            var pro = datasourceDefine.TargetType.GetProperty(datasourceDefine.PropertyOrMethodName);
            if (pro != null)
            {
                return pro.GetValue(obj);
            }
            else
            {
                var method = datasourceDefine.TargetType.GetMethod(datasourceDefine.PropertyOrMethodName);

                return method.Invoke(obj, null);
            }
            
        }

        /// <summary>
        /// 通用获取数据的函数
        /// </summary>
        /// <param name="pagerInfo">分页信息</param>
        /// <param name="target">数据源，如：Project1.DBContext1.Datas，表示把Project1.DBContext1的Datas属性作为数据源</param>
        /// <param name="fields">需要绑定的字段</param>
        /// <param name="searchJsonStr">搜索条件的model</param>
        /// <returns></returns>
        protected object GetDataSource( PagerInfo pagerInfo ,  string target, string[] fields,string searchJsonStr)
        {
            List<string> changeNewNames = new List<string>(fields.Length);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Contains("->"))
                {
                    string newName = fields[i].Substring(fields[i].IndexOf("->") + 2);
                    changeNewNames.Add(newName);
                }
            }
            if (changeNewNames.Count > 0)
            {
                fields = (from m in fields where changeNewNames.Contains(m) == false select m).ToArray();
            }
            var datasourceDefine = this.OnGetDataSourcePath(target,false);
         
            string pkid = null;//主键值

            Dictionary<Type, object> activeObjs = new Dictionary<Type, object>();
            try
            {

                object[] arrResult = null;
                object result = GetDataSource(datasourceDefine, activeObjs);

                Type dataItemType = result.GetType();
                if (dataItemType.IsArray)
                {
                    dataItemType = dataItemType.GetElementType();
                }
                else if (dataItemType.GetTypeInfo().IsGenericType)
                {
                    dataItemType = dataItemType.GetGenericArguments()[0];
                }

                if (pagerInfo.PageIndex == 0 || (result is IQueryable && !(result is IOrderedQueryable) ))
                {
                    //获取主键名
                    if (dataItemType.GetTypeInfo().IsSubclassOf(typeof(EntityDB.DataItem)))
                    {
                        EntityDB.Attributes.Table tableatt = dataItemType.GetTypeInfo().GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
                        if (tableatt != null)
                        {
                            pkid = tableatt.KeyName;
                            if (fields.Contains(pkid) == false)
                            {
                                var list = new List<string>(fields);
                                list.Add(pkid);
                                fields = list.ToArray();
                            }

                            if (result is IQueryable && !(result is IOrderedQueryable))
                            {
                                result = ResultHelper.GetQueryForOrderBy(result, pkid);
                            }
                        }
                    }
                }
                if (searchJsonStr.IsNullOrEmpty() == false)
                {
                    var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                    result = search(result, dataItemType, searchModel);
                }
                if (result is IQueryable)
                {
                    result = ResultHelper.InvokeSelect(result, fields);
                }
                if (pagerInfo.PageSize > 0)
                {
                    result = ResultHelper.InvokeSkip(result, pagerInfo.PageIndex * pagerInfo.PageSize);
                    result = ResultHelper.InvokeTake(result, pagerInfo.PageSize);
                }
                arrResult = (object[])ResultHelper.InvokeToArray(result);

                List<Dictionary<string, object>> finalResult = new List<Dictionary<string, object>>();
                foreach (var dataitem in arrResult)
                {
                    Dictionary<string, object> jData = new Dictionary<string, object>();
                    foreach (string _field in fields)
                    {
                        string field = _field;
                        int flag1 = field.IndexOf("->");
                        if (flag1>0)
                        {
                            field = field.Substring(0 , flag1);
                        }
                        if (field.Contains(":"))
                        {
                            string[] fieldInfo = field.Split(':');

                            object fieldValue = ResultHelper.GetPropertyValue(dataitem, fieldInfo[0]);
                            if (fieldValue == null)
                            {
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".value", "");
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", "");
                                continue;
                            }

                            ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".value", fieldValue);

                            string otherTarget = fieldInfo[1].Substring(0, fieldInfo[1].LastIndexOf("."));

                            object query = GetDataSource(OnGetDataSourcePath(otherTarget,false), activeObjs);

                            string compareProName = fieldInfo[1].Substring(fieldInfo[1].LastIndexOf(".") + 1);

                            object findedDataItem = ResultHelper.InvokeWhereEquals(query, compareProName, fieldValue);
                            findedDataItem = ResultHelper.InvokeSelect(findedDataItem , fieldInfo[2]);
                           
                            if (findedDataItem != null)
                            {
                                fieldValue = ResultHelper.InvokeFirstOrDefault(findedDataItem);
                                if (fieldValue == null)
                                    fieldValue = "";
                                fieldValue = fieldValue is Enum ? fieldValue.ToString() : fieldValue;
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", fieldValue);
                            }
                            else
                            {
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", "");
                            }
                        }
                        else
                        {
                            object fieldValue = ResultHelper.GetPropertyValue(dataitem, field);
                            if (fieldValue == null)
                                fieldValue = "";

                            if(fieldValue is Enum)
                            {
                                ResultHelper.SetJsonDataValue(jData, field + ".value", (int)fieldValue);
                                ResultHelper.SetJsonDataValue(jData, field + ".caption", fieldValue.ToString());
                            }
                            else
                            {
                                ResultHelper.SetJsonDataValue(jData, field, fieldValue);
                            }
                            
                        }
                    }
                    finalResult.Add(jData);
                }

                foreach (string _field in fields)
                {
                    int flag1 = _field.IndexOf("->");
                    if (flag1 > 0)
                    {
                        string originalName = _field.Substring(0, flag1).Split(':')[0];
                        string changeName = _field.Substring(flag1 + 2);
                        foreach (Dictionary<string, object> dataitem in finalResult)
                        {
                            ResultHelper.ChangePropertyName(dataitem, originalName, changeName);
                        }
                    }
                }

                var pkvalueDic = new Dictionary<string, object>();
                pkvalueDic["pkid"] = pkid == null ? "" : pkid;
                finalResult.Add(pkvalueDic);
                return   finalResult.ToArray();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach( KeyValuePair<Type,object> item in activeObjs )
                {
                    if(item.Value is IDisposable)
                    {
                        ((IDisposable)item.Value).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 计算总记录数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="searchJsonStr">查询条件model</param>
        /// <returns></returns>
        [RemotingMethod]
        public int Count(string target,string searchJsonStr)
        {
            var datasourceDefine = this.OnGetDataSourcePath(target,false);
            Dictionary<Type, object> dbcontexts = new Dictionary<Type, object>();
            object result = GetDataSource(datasourceDefine, dbcontexts);
            try
            {
                Type dataItemType = result.GetType();
                if (dataItemType.IsArray)
                {
                    dataItemType = dataItemType.GetElementType();
                }
                else if (dataItemType.GetTypeInfo().IsGenericType)
                {
                    dataItemType = dataItemType.GetGenericArguments()[0];
                }

                if (searchJsonStr.IsNullOrEmpty() == false)
                {
                    var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                    result = search(result, dataItemType, searchModel);
                }

                return Convert.ToInt32(ResultHelper.InvokeCount(result));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (var obj in dbcontexts)
                {
                    if (obj is IDisposable)
                    {
                        ((IDisposable)result).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 合计指定字段的
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fields"></param>
        /// <param name="searchJsonStr"></param>
        /// <returns></returns>
        [RemotingMethod]
        public object Sum(string target, string[] fields, string searchJsonStr)
        {
            var datasourceDefine = OnGetDataSourcePath(target,false);
            var dbcontexts = new Dictionary<Type, object>();
            object result = GetDataSource(datasourceDefine, dbcontexts);
            try
            {
                Type dataItemType = result.GetType();
                if (dataItemType.IsArray)
                {
                    dataItemType = dataItemType.GetElementType();
                }
                else if (dataItemType.GetTypeInfo().IsGenericType)
                {
                    dataItemType = dataItemType.GetGenericArguments()[0];
                }

                if (searchJsonStr.IsNullOrEmpty() == false)
                {
                    var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                    result = search(result, dataItemType, searchModel);
                }

                Dictionary<string, object> finallResult = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    if (field.Length == 0)
                        continue;

                    var query = ResultHelper.InvokeSelect(result, field);
                    var value = ResultHelper.InvokeSum(query);
                    finallResult[field] = value;
                }
                
                return finallResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (var obj in dbcontexts)
                {
                    if (obj is IDisposable)
                    {
                        ((IDisposable)result).Dispose();
                    }
                }
            }
        }

        protected virtual void OnBeforeSavingData(object dataitem)
        {
        }

        /// <summary>
        /// 把对象保存到数据库
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dataJson"></param>
        /// <returns>返回主键值</returns>
        [RemotingMethod]
        public object SaveData(string target,string dataJson)
        {
            var datasourceDefine = OnGetDataSourcePath(target,true);
           
            var dbContext = Activator.CreateInstance(datasourceDefine.TargetType) as EntityDB.DBContext;
            if (dbContext == null)
            {
                throw new Exception(datasourceDefine.TargetType.FullName + " is not " + typeof(EntityDB.DBContext).FullName);
            }
            try
            {
                var tableType = datasourceDefine.TargetType.GetProperty(datasourceDefine.PropertyOrMethodName).PropertyType.GetGenericArguments()[0];
                EntityDB.Attributes.Table myTableAttr = tableType.GetTypeInfo().GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
                var dataitem = Newtonsoft.Json.JsonConvert.DeserializeObject(dataJson, tableType) as EntityDB.DataItem;
                if (dataitem == null)
                {
                    throw new Exception("dataitem is not EntityDB.DataItem");
                }
                this.OnBeforeSavingData(dataitem);
                dbContext.Update(dataitem);
                if (myTableAttr.KeyName.IsNullOrEmpty())
                    return null;
                return dataitem.GetValue(myTableAttr.KeyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbContext.Dispose();
            }
        }
        /// <summary>
        /// 文件开始传输
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="state">客户端设置的附加信息</param>
        /// <param name="fileSize"></param>
        /// <param name="offset">文件偏移</param>
        /// <returns>返回一个IUploadFileHandler对象，处理上传的文件数据</returns>
        public virtual IUploadFileHandler OnBeginUploadFile(string fileName,string state, int fileSize,int offset)
        {
            return null;
        }

        internal static ConcurrentDictionary<string, object> HttpUploadHandlers = new ConcurrentDictionary<string, object>();
        [RemotingMethod]
        internal string UploadFileWithHTTP(string fileName, string state, int fileSize, int offset)
        {
            try
            {
                while (true)
                {
                    bool _hasDel = false;
                    foreach (var kv in HttpUploadHandlers)
                    {
                        var obj = (object[])kv.Value;
                        var time = (DateTime)obj[0];
                        if ((DateTime.Now - time).TotalMinutes > 20)
                        {
                            var h = (IUploadFileHandler)obj[3];
                            h.OnUploadFileError();
                            object delobj;
                            HttpUploadHandlers.TryRemove(kv.Key,out delobj);
                            _hasDel = true;
                            break;
                        }
                    }
                    if (!_hasDel)
                        break;
                }
            }
            catch
            {
            }

            var handler = this.OnBeginUploadFile(fileName, state, fileSize, offset);
            string tranid = Guid.NewGuid().ToString();
            HttpUploadHandlers[tranid] = new object[] { DateTime.Now, offset, fileSize, handler };
            return tranid;
        }
        [RemotingMethod]
        internal object GettingFileDataWithHttp(string tranid,string data )
        {
            if (HttpUploadHandlers.ContainsKey(tranid) == false)
                throw new Exception("tranid not exist");
            try
            {
                var obj = (object[])HttpUploadHandlers[tranid];

                int uploaded = (int)obj[1];
                int filesize = (int)obj[2];
                var handler = (IUploadFileHandler)obj[3];

                string[] strArr = data.Split('%');
                byte[] bs = new byte[strArr.Length - 1];
                for (int i = 1; i < strArr.Length; i++)
                {
                    bs[i - 1] = (byte)Convert.ToInt32(strArr[i], 16);
                }

                uploaded += bs.Length;
                handler.OnGettingFileData(bs);
                obj[1] = uploaded;
                obj[0] = DateTime.Now;
                if (uploaded == filesize)
                {
                    handler.OnUploadFileCompleted();
                    object delobj;
                    HttpUploadHandlers.TryRemove(tranid, out delobj);
                }

                return new { size = filesize, offset = uploaded };
            }
            catch (Exception ex)
            {
                object delobj;
                HttpUploadHandlers.TryRemove(tranid, out delobj);
                throw ex;
            }

        }
        /// <summary>
        /// 把消息发送到客户端，客户端对象的.onmessage会接收到
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="msg"></param>
        public static void SendGroupMessage(string groupName, string msg)
        {
            if (groupName.IsNullOrEmpty())
                throw new Exception("groupName can not be empty");
            for(int i = 0; i < RemotingClientHandler.KeepAliveHandlers.Count; i ++)
            {
                var handler = (RemotingClientHandler)RemotingClientHandler.KeepAliveHandlers[i];
                if (handler!=null && handler.GroupName == groupName)
                {
                    Task.Run(()=>
                    {
                        try
                        {
                            handler.SendClientMessage(msg);
                        }
                        catch
                        {
                        }
                    });
                   
                }
            }
           
        }

       
    }
}
