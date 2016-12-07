﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityDB
{
    public enum DatabaseType:int 
    {


        /// <summary>
        /// 
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// 
        /// </summary>

        Sqlite = 2,

        /// <summary>
        /// 
        /// </summary>

        MySql = 3,
    }
    public class DatabaseModifyEventArg
    {
        public object DataItem
        {
            get;
            internal set;
        }
        public object Parameters
        {
            get;
            internal set;
        }
    }
   
    public class DBContext : Microsoft.EntityFrameworkCore.DbContext
    {

        #region 事件
        public delegate void DatabaseEventHandler(object sender, DatabaseModifyEventArg e);
        public static event DatabaseEventHandler BeforeDelete;
        public static event DatabaseEventHandler BeforeInsert;
        public static event DatabaseEventHandler BeforeUpdate;
        public static event DatabaseEventHandler AfterDelete;
        public static event DatabaseEventHandler AfterInsert;
        public static event DatabaseEventHandler AfterUpdate;
        #endregion

        #region 静态变量
        static bool SetConfigurationed = false;
        static List<IActionCapture> TypeCaptures = new List<IActionCapture>();

        static Dictionary<EntityDB.DatabaseType, Type> _DatabaseServiceTypes;
        static Dictionary<EntityDB.DatabaseType, Type> DatabaseServiceTypes
        {
            get
            {
                if (_DatabaseServiceTypes == null)
                {
                    var compareType = typeof(IDatabaseService);
                    _DatabaseServiceTypes = new Dictionary<DatabaseType, Type>();
                    var types = typeof(DBContext).Assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.GetInterfaces().Any(m => m == compareType))
                        {
                            var attrs = type.GetCustomAttributes(typeof(EntityDB.Attributes.DatabaseTypeAttribute), false);
                            if (attrs.Length > 0)
                            {
                                EntityDB.Attributes.DatabaseTypeAttribute att = (EntityDB.Attributes.DatabaseTypeAttribute)attrs[0];
                                _DatabaseServiceTypes[att.DBType] = type;
                            }
                        }
                    }
                }
                return _DatabaseServiceTypes;
            }
        }

        #endregion

        #region 属性
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; private set; }

        IDatabaseService _databaseService;
        public new IDatabaseService Database
        {
            get
            {
                return _databaseService;
            }
        }

        #endregion

        static DBContext()
        {
            if (SetConfigurationed == false)
            {
                SetConfigurationed = true;
                //防止有些dll版本不对，无法加载
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                BeforeDelete += Database_BeforeDelete;
                BeforeInsert += Database_BeforeInsert;
                BeforeUpdate += Database_BeforeUpdate;

                AfterDelete += Database_AfterDelete;
                AfterInsert += Database_AfterInsert;
                AfterUpdate += Database_AfterUpdate;
            }
        }
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Split(',')[0].Trim();
            if (name.StartsWith("Microsoft.VisualStudio.Web.PageInspector.Tracing.resources"))
                return null;
            var assembly = Assembly.Load(name);
            return assembly;
        }
        /// <summary>
        /// 添加触发器
        /// </summary>
        /// <param name="actionCapture"></param>
        public static void AddActionCapture(IActionCapture actionCapture)
        {
            TypeCaptures.Add(actionCapture);
        }

        #region 关联事件
        static void Database_AfterUpdate(object sender, DatabaseModifyEventArg e)
        {
            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.AfterUpdate(  sender, e);
                    }
                }
            }
        }

        static void Database_AfterInsert(object sender, DatabaseModifyEventArg e)
        {
            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.AfterInsert(sender, e);
                    }
                }
            }
        }

        static void Database_AfterDelete(object sender, DatabaseModifyEventArg e)
        {
            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.AfterDelete( sender, e);
                    }
                }
            }
        }

        static void Database_BeforeUpdate(object sender, DatabaseModifyEventArg e)
        {
            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.BeforeUpdate(sender, e);
                    }
                }
            }
        }

        static void Database_BeforeInsert(object sender, DatabaseModifyEventArg e)
        {
            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.BeforeInsert(sender, e);
                    }
                }
            }
        }

        static void Database_BeforeDelete(object sender, DatabaseModifyEventArg e)
        {

            if (e.DataItem != null)
            {
                Type dataitemType = e.DataItem.GetType();
                foreach (var capture in TypeCaptures)
                {
                    if (dataitemType.FullName == capture.DataItemType.FullName)
                    {
                        capture.BeforeDelete(sender, e);
                    }
                }
            }
        }
        #endregion

        #region 动态query

        public static object GetQueryByString(object linqQuery, string stringQuery)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            Type dynamicQueryableType = typeof(DynamicQueryable);
            var methods = dynamicQueryableType.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Where" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                return mmm.Invoke(null, new object[] { linqQuery, stringQuery, null });

            }
            return linqQuery;
        }

        public static bool InvokeAny(object linqQuery, string propertyName, object value)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            System.Reflection.PropertyInfo pinfo;
            System.Linq.Expressions.Expression left, right;

            left = GetPropertyExpression(param, dataType, propertyName, out pinfo);
            if (pinfo.PropertyType.IsGenericType)
            {
                Type ptype = pinfo.PropertyType.GetGenericArguments()[0];
                left = System.Linq.Expressions.Expression.Convert(left, ptype);
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, ptype));
            }
            else
            {
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, pinfo.PropertyType));
            }

            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Equal(left, right);
            expression = System.Linq.Expressions.Expression.Lambda(expression, param);

            Type queryableType = typeof(System.Linq.Queryable);
            var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name == "Any" && method.IsGenericMethod)
                {
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                    return (bool)mmm.Invoke(null, new object[] { linqQuery, expression });
                }
            }
            return false;
        }

        public static object InvokeWhereEquals(object linqQuery, string propertyName, object value)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            System.Reflection.PropertyInfo pinfo;
            System.Linq.Expressions.Expression left, right;

            left = GetPropertyExpression(param, dataType, propertyName, out pinfo);
            if (pinfo.PropertyType.IsGenericType)
            {
                Type ptype = pinfo.PropertyType.GetGenericArguments()[0];
                left = System.Linq.Expressions.Expression.Convert(left, ptype);
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, ptype));
            }
            else
            {
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, pinfo.PropertyType));
            }

            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Equal(left, right);
            expression = System.Linq.Expressions.Expression.Lambda(expression, param);

            Type queryableType = typeof(System.Linq.Queryable);
            var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name == "Where" && method.IsGenericMethod)
                {
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                    return mmm.Invoke(null, new object[] { linqQuery, expression });
                }
            }
            return null;
        }

        public static object InvokeWhereWithMethod(object linqQuery,MethodInfo fieldMethod, string propertyName, object value)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            System.Reflection.PropertyInfo pinfo;
            System.Linq.Expressions.Expression left, right;

            left = GetPropertyExpression(param, dataType, propertyName, out pinfo);

            if (pinfo.PropertyType.IsGenericType)
            {
                Type ptype = pinfo.PropertyType.GetGenericArguments()[0];
                left = System.Linq.Expressions.Expression.Convert(left, ptype);
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, ptype));
            }
            else
            {
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, pinfo.PropertyType));
            }

            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Call(left,fieldMethod, right);
            expression = System.Linq.Expressions.Expression.Lambda(expression, param);

            Type queryableType = typeof(System.Linq.Queryable);
            var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name == "Where" && method.IsGenericMethod)
                {
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                    return mmm.Invoke(null, new object[] { linqQuery, expression });
                }
            }
            return null;
        }
        internal static object InvokeToList(object linqQuery)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            Type queryType = typeof(System.Linq.Enumerable);
            var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "ToList");
            foreach (System.Reflection.MethodInfo method in methods)
            {
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                if (mmm != null)
                {
                    return mmm.Invoke(null, new object[] { linqQuery });
                }
            }
            return null;
        }

        static MethodInfo ToArrayMethod;
        public static object InvokeToArray(object linqQuery)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            if (ToArrayMethod == null)
            {
                Type queryType = typeof(System.Linq.Enumerable);
                var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "ToArray");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.IsGenericMethod)
                    {
                        ToArrayMethod = method;
                    }
                }
            }
            if (ToArrayMethod == null)
                throw new Exception("找不到泛型ToArray方法");

            System.Reflection.MethodInfo mmm = ToArrayMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { linqQuery });
            }
            else
                throw new Exception(ToArrayMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }

        static MethodInfo TakeMethod;
        public static object InvokeTake(object linqQuery, int takeSize)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            if (TakeMethod == null)
            {
                Type queryType = typeof(System.Linq.Queryable);
                var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Take");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.IsGenericMethod)
                    {
                        TakeMethod = method;
                        break;
                    }
                    
                }
            }
            if (TakeMethod == null)
                throw new Exception("找不到泛型Take方法");

            System.Reflection.MethodInfo mmm = TakeMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { linqQuery, takeSize });
            }
            else
                throw new Exception(TakeMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }

        static MethodInfo SkipMethod;
        public static object InvokeSkip(object linqQuery, int skip)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            if (SkipMethod == null)
            {
                Type queryType = typeof(System.Linq.Queryable);
                var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Skip");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.IsGenericMethod)
                    {
                        SkipMethod = method;
                        break;
                    }

                }
            }
            if (SkipMethod == null)
                throw new Exception("找不到泛型Skip方法");

            System.Reflection.MethodInfo mmm = SkipMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { linqQuery, skip });
            }
            else
                throw new Exception(SkipMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }
        /// <summary>
        /// 获取属性对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">属性名，可以是a.b的形式</param>
        /// <returns></returns>
        public static object GetPropertyValue(object data, string propertyName)
        {
            if (data == null)
                return null;
            string[] dataFieldArr = propertyName.Split('.');
            Type currentObjType = data.GetType();
            PropertyInfo propertyInfo = null;
            object result = data;
            for (int i = 0; i < dataFieldArr.Length; i++)
            {
                propertyInfo = currentObjType.GetProperty(dataFieldArr[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new Exception("属性" + dataFieldArr[i] + "无效");

                result = propertyInfo.GetValue(result);
                if (result == null)
                    return null;
                if (i < dataFieldArr.Length - 1)
                {
                    currentObjType = propertyInfo.PropertyType;
                }
            }
            return result;
        }
        public static System.Linq.Expressions.Expression GetPropertyExpression(ParameterExpression param, Type dataType, string propertyName, out PropertyInfo propertyInfo)
        {
            System.Linq.Expressions.Expression left = null;
            string[] dataFieldArr = propertyName.Split('.');
            System.Linq.Expressions.Expression lastObjectExpress = param;
            Type currentObjType = dataType;
            propertyInfo = null;
            for (int i = 0; i < dataFieldArr.Length; i++)
            {
                propertyInfo = currentObjType.GetProperty(dataFieldArr[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new Exception("属性" + dataFieldArr[i] + "无效");
                left = System.Linq.Expressions.Expression.Property(lastObjectExpress, propertyInfo);
                if (i < dataFieldArr.Length - 1)
                {
                    currentObjType = propertyInfo.PropertyType;
                    lastObjectExpress = left;
                }
            }
            return left;
        }

        public static object InvokeSelect(object linqQuery, string propertyName)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            PropertyInfo pinfo;
            System.Linq.Expressions.Expression left = GetPropertyExpression(param, dataType, propertyName, out pinfo);


            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Lambda(left, param);

            Type myType = typeof(System.Linq.Queryable);
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Select" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType, pinfo.PropertyType);
                return mmm.Invoke(null, new object[] { linqQuery, expression });

            }
            return null;
        }
        public static object InvokeSum(object linqQuery)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            Type myType = typeof(System.Linq.Queryable);
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Sum" || method.IsGenericMethod || method.ReturnType != dataType)
                    continue;
                if (method.GetParameters().Length != 1)
                    continue;

                return method.Invoke(null, new object[] { linqQuery });

            }
            return null;
        }
        public static object InvokeFirstOrDefault(object linqQuery)
        {
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            Type myType = typeof(System.Linq.Queryable);
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "FirstOrDefault" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                return mmm.Invoke(null, new object[] { linqQuery });

            }
            return null;
        }
        public static object GetQueryForOrderBy(object linqQuery, string stringOrder)
        {
            Type myType = typeof(System.Linq.Queryable);
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            bool isThenBy = false;
            string[] orders = stringOrder.Split(',');
            foreach (string order in orders)
            {
                if (order.Trim().Length == 0)
                    continue;
                bool desc = order.Trim().ToLower().Contains(" desc");
                string methodName;
                if (isThenBy == false)
                {
                    isThenBy = true;
                    methodName = desc ? "OrderByDescending" : "OrderBy";
                }
                else
                {
                    methodName = desc ? "ThenByDescending" : "ThenBy";
                }
                string itemProperty = order.Trim().Split(' ')[0];
                if (itemProperty.StartsWith("[") && itemProperty.EndsWith("]"))
                    itemProperty = itemProperty.Substring(1, itemProperty.Length - 2);

                System.Reflection.PropertyInfo pinfo;
                System.Linq.Expressions.Expression left = GetPropertyExpression(param, dataType, itemProperty, out pinfo);
                System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Lambda(left, param);


                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.Name != methodName || method.IsGenericMethod == false)
                        continue;
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType, pinfo.PropertyType);
                    linqQuery = mmm.Invoke(null, new object[] { linqQuery, expression });
                    break;
                }
            }
            return linqQuery;
        }
        public static object GetQueryForThenBy(object linqQuery, string stringOrder)
        {
            Type myType = typeof(System.Linq.Queryable);
            Type dataType = linqQuery.GetType().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            string[] orders = stringOrder.Split(',');
            foreach (string order in orders)
            {
                if (order.Trim().Length == 0)
                    continue;
                bool desc = order.Trim().ToLower().Contains(" desc");
                string methodName = desc ? "ThenByDescending" : "ThenBy";
                string itemProperty = order.Trim().Split(' ')[0];
                if (itemProperty.StartsWith("[") && itemProperty.EndsWith("]"))
                    itemProperty = itemProperty.Substring(1, itemProperty.Length - 2);

                System.Reflection.PropertyInfo pinfo;
                System.Linq.Expressions.Expression left = GetPropertyExpression(param, dataType, itemProperty, out pinfo);
                System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Lambda(left, param);


                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.Name != methodName || method.IsGenericMethod == false)
                        continue;
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType, pinfo.PropertyType);
                    linqQuery = mmm.Invoke(null, new object[] { linqQuery, expression });
                    break;
                }
            }
            return linqQuery;
        }
        #endregion


       

        //public static void Init(System.Reflection.Assembly mainAssembly)
        //{
        //    Helper.Init(mainAssembly);
        //}

        static System.Data.Common.DbConnection CreateConnection(string connectionString, DatabaseType dbType)
        {
            Type type = DatabaseServiceTypes[dbType];
            IDatabaseService service = (IDatabaseService)Activator.CreateInstance(type);
            return service.CreateConnection(connectionString);
        }

        public static IDatabaseService CreateDatabaseService(string connectionString, DatabaseType dbType)
        {
            DBContext context = new DBContext(connectionString, dbType);
            return context.Database;
        }

        public DBContext(string connectionString, DatabaseType dbType = DatabaseType.SqlServer)
        {
            this.DatabaseType = dbType;
            this.ConnectionString = connectionString;
            Type type = DatabaseServiceTypes[dbType];
            _databaseService = (IDatabaseService)Activator.CreateInstance(type, new object[] { this });

            this.ChangeTracker.AutoDetectChangesEnabled = false;
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            if (this.DatabaseType == DatabaseType.SqlServer)
            {
                optionsBuilder.UseSqlServer(this.ConnectionString);
            }
            else if (this.DatabaseType == DatabaseType.Sqlite)
            {
                optionsBuilder.UseSqlite(this.ConnectionString);
            }
            else if (this.DatabaseType == DatabaseType.MySql)
            {
                optionsBuilder.UseMySql(this.ConnectionString);
            }
        }


        #region 数据更新、添加、删除操作
        /// <summary>
        /// 更新对象数据到数据库
        /// </summary>
        /// <param name="dataitem"></param>
        public virtual void Update(DataItem dataitem)
        {
            string pkid = dataitem.PKIDField;
            object pkvalue = dataitem.PKValue;
            if (pkvalue == null && pkid != null)
            {
                Insert(dataitem);
                return;
            }

            if (BeforeUpdate != null)
            {
                BeforeUpdate(this, new DatabaseModifyEventArg()
                {
                    DataItem = dataitem,
                });
            }

            bool needCloseConnection = false;
            if (this.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                needCloseConnection = true;
                this.Database.Connection.Open();
            }

            try
            {
                _databaseService.Update(dataitem);
                if (AfterUpdate != null)
                {
                    AfterUpdate(this, new DatabaseModifyEventArg()
                    {
                        DataItem = dataitem,
                    });
                }
                dataitem.ChangedProperties.Clear();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (needCloseConnection)
                    this.Database.Connection.Close();
            }
        }
        /// <summary>
        /// 添加对象数据到数据库
        /// </summary>
        /// <param name="dataitem"></param>
        public virtual void Insert(DataItem dataitem)
        {
            if (BeforeInsert != null)
            {
                BeforeInsert(this, new DatabaseModifyEventArg()
                    {
                        DataItem = dataitem,
                    });
            }

            bool needCloseConnection = false;
            if (this.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                needCloseConnection = true;
                this.Database.Connection.Open();
            }

            try
            {
                _databaseService.Insert(dataitem);
                if (AfterInsert != null)
                {
                    AfterInsert(this, new DatabaseModifyEventArg()
                    {
                        DataItem = dataitem,
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (needCloseConnection)
                    this.Database.Connection.Close();
            }
        }
        /// <summary>
        /// 在数据库删除此对象数据
        /// </summary>
        /// <param name="dataitem"></param>
        public virtual void Delete(DataItem dataitem)
        {
            if (BeforeDelete != null)
            {
                BeforeDelete(this, new DatabaseModifyEventArg()
                {
                    DataItem = dataitem,
                });
            }

            bool needCloseConnection = false;
            if (this.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                needCloseConnection = true;
                this.Database.Connection.Open();
            }

            try
            {
                _databaseService.Delete(dataitem);
                if (AfterDelete != null)
                {
                    AfterDelete(this, new DatabaseModifyEventArg()
                    {
                        DataItem = dataitem,
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (needCloseConnection)
                    this.Database.Connection.Close();
            }
           
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="items"></param>
        public void Delete(System.Linq.IQueryable items)
        {
            Type dataType = items.GetType().GetGenericArguments()[0];

            string pkid = null;
            if (dataType != null)
            {
                try
                {
                    object[] atts = dataType.GetCustomAttributes(typeof(Attributes.Table), true);
                    pkid = ((Attributes.Table)atts[0]).IDField;
                }
                catch
                {
                }
            }
            if (pkid == null)
                throw new Exception(dataType.Name + "没有定义主键");


            bool needCloseConnection = false;
            if (this.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                needCloseConnection = true;
                this.Database.Connection.Open();
            }

            try
            {
                var query = InvokeSelect(items, pkid);
                while (true)
                {
                    var data1 = InvokeTake(query, 100);
                    var dataitems = (System.Collections.IList)InvokeToList(data1);
                    if (dataitems.Count == 0)
                        break;
                    foreach (var idvalue in dataitems)
                    {
                        var deldataItem = (DataItem)Activator.CreateInstance(dataType);
                        deldataItem.SetValue(pkid, idvalue);
                        Delete(deldataItem);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (needCloseConnection)
                    this.Database.Connection.Close();
            }

        }
        #endregion



       
        /// <summary>
        /// 开始事务
        /// </summary>
        public virtual void BeginTransaction()
        {
            ((Microsoft.EntityFrameworkCore.DbContext)this).Database.BeginTransaction();
        }

        
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="IsolationLevel"></param>
        public virtual void BeginTransaction(System.Data.IsolationLevel IsolationLevel)
        {
            ((Microsoft.EntityFrameworkCore.DbContext)this).Database.BeginTransaction(IsolationLevel);
        }

     
        public void CommitTransaction()
        {
            ((Microsoft.EntityFrameworkCore.DbContext)this).Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            ((Microsoft.EntityFrameworkCore.DbContext)this).Database.RollbackTransaction();
        }
    }
}