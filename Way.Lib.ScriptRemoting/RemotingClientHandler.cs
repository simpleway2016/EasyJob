﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    class RemotingClientHandler
    {
        public enum RemotingStreamType
        {
            Text,
            Bytes
        }

        public RemotingStreamType StreamType
        {
            get;
            private set;
        }
        static Type RemotingPageType = typeof(RemotingController);
        internal static ArrayList KeepAliveHandlers = ArrayList.Synchronized(new ArrayList());
        static ConcurrentDictionary<string, TypeDefine> ExistTypes = new ConcurrentDictionary<string, TypeDefine>();
        public delegate void SendDataHandler(string data);
        internal SendDataHandler mSendDataFunc;
        internal Action mCloseStreamHandler;
        SessionState Session;
        internal string GroupName;
        IUploadFileHandler mUploadFileHandler;
        int mFileGettedSize = 0;
        MessageBag mCurrentBag;
        string mClientIP;
        internal object Tag1;
        internal object Tag2;
        internal DateTime _heartTime;

        public RemotingClientHandler(SendDataHandler sendFunc , Action closeStreamHandler,string clientIP)
        {
            mCloseStreamHandler = closeStreamHandler;
            mSendDataFunc = sendFunc;
            this.StreamType = RemotingStreamType.Text;
            mClientIP = clientIP;
            _heartTime = DateTime.Now;
        }
        public virtual void OnReceived(string data)
        {
            try
            {
                MessageBag msgBag = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageBag>(data);
                mCurrentBag = msgBag;
                if (this.Session == null)
                {
                    if (msgBag.SessionID.IsNullOrEmpty())
                    {
                        this.Session = SessionState.GetSession(Guid.NewGuid().ToString(), this.mClientIP);
                    }
                    else
                    {
                        this.Session = SessionState.GetSession(msgBag.SessionID, this.mClientIP);
                        if (this.Session == null)
                        {
                            this.Session = SessionState.GetSession(Guid.NewGuid().ToString(), this.mClientIP);
                        }
                    }
                }

                if (msgBag.Action == "init")
                {
                    handleInit(msgBag);
                }
                else if (msgBag.Action == "exit")
                {
                    mCloseStreamHandler();
                }
                else if (msgBag.Action == "UploadFile")
                {
                    this.StreamType = RemotingStreamType.Bytes;
                    handleUploadFile(msgBag);
                }
                else if (msgBag.MethodName.IsNullOrEmpty() == false)
                {
                   
                    handleMethodInvoke(msgBag);
                }
                else if (msgBag.GroupName.IsNullOrEmpty() == false)
                {
                    this.GroupName = msgBag.GroupName;
                    if(RemotingController.MessageReceiverConnect(this.Session , this.GroupName) == false)
                    {
                        throw new Exception("服务器拒绝你接收信息");
                    }
                    KeepAliveHandlers.Add(this);
                    return;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                var thread = System.Threading.Thread.CurrentThread;
                if (SessionState.ThreadSessions.ContainsKey(thread))
                {
                    SessionState.ThreadSessions.Remove(thread);
                }
            }
            
        }

        public virtual void OnReceived(byte[] data)
        {
            try
            {
                mFileGettedSize += data.Length;
                if (mUploadFileHandler != null)
                {
                    mUploadFileHandler.OnGettingFileData(data);
                }
                SendData(MessageType.Result, mFileGettedSize);

                if (mFileGettedSize >= mCurrentBag.FileSize)
                {
                    if (mUploadFileHandler != null)
                    {
                        mUploadFileHandler.OnUploadFileCompleted();
                    }
                    mUploadFileHandler = null;
                    //重新接收text
                    this.StreamType = RemotingStreamType.Text;
                    SendData(MessageType.Result, "ok");
                }
            }
            catch (Exception ex)
            {
                this.StreamType = RemotingStreamType.Text;
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }
        public virtual void OnDisconnected()
        {
            if (mUploadFileHandler != null)
            {
                try
                {
                    mUploadFileHandler.OnUploadFileError();
                }
                catch
                {
                }
                mUploadFileHandler = null;
            }
            try
            {
                if (this.GroupName.IsNullOrEmpty() == false)
                {
                    KeepAliveHandlers.Remove(this);
                }
            }
            catch
            {
            }
        }
        
        TypeDefine checkRemotingName(string remoteName)
        {
            
            TypeDefine pageDefine = null;
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
                        pageDefine = new ScriptRemoting.TypeDefine();
                        pageDefine.ControllerType = type;
                        break;
                    }
                }
                if (pageDefine == null)
                {
                    throw new Exception("无法找到" + remoteName + "类定义");
                }
                if (pageDefine.ControllerType != RemotingPageType && pageDefine.ControllerType.GetTypeInfo().IsSubclassOf(RemotingPageType) == false)
                {
                    throw new Exception(remoteName + "不是RemotingPage的子类");
                }

                MethodInfo[] methods = pageDefine.ControllerType.GetMethods();
                foreach (MethodInfo m in methods)
                {
                    if (m.GetCustomAttribute<RemotingMethodAttribute>() != null)
                    {
                        pageDefine.Methods.Add(m);
                    }
                }
                try
                {
                    ExistTypes[remoteName] = pageDefine;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return pageDefine;
        }

        void handleInit(MessageBag msgBag)
        {
            try
            {
                string remoteName = msgBag.ClassFullName;
                TypeDefine pageDefine = checkRemotingName(remoteName);


                StringBuilder methodOutput = new StringBuilder();
                methodOutput.Append(@"(function (_super) {
    __extends(func, _super);
    function func() {
        _super.apply(this, arguments);
    }
");
                foreach (MethodInfo method in pageDefine.Methods)
                {
                    methodOutput.Append($"func.prototype." + method.Name + " = function (");
                    var parameters = method.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodOutput.Append(parameters[i].Name);
                        methodOutput.Append(',');
                    }
                    methodOutput.AppendLine("callback){");
                    methodOutput.Append("_super.prototype.pageInvoke.call(this,'" + method.Name + "',[");
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodOutput.Append(parameters[i].Name);
                        if (i < parameters.Length - 1)
                        {
                            methodOutput.Append(',');
                        }
                    }
                    methodOutput.AppendLine("] , callback );");
                    methodOutput.AppendLine("};");
                }

                methodOutput.Append(@"
    return func;
}(WayScriptRemoting));
");
                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();

                mSendDataFunc(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    text = methodOutput.ToString(),
                    SessionID = this.Session.SessionID
                }));
            }
            catch (Exception ex)
            {
                mSendDataFunc(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    err = ex.Message,
                }));
            }
        }

        void handleUploadFile(MessageBag msgBag)
        {
            mFileGettedSize = 0;
            try
            {
                string remoteName = msgBag.ClassFullName;
                var pageDefine = checkRemotingName(remoteName);

                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();
                mFileGettedSize = msgBag.Offset;
                mUploadFileHandler = currentPage.OnBeginUploadFile(msgBag.FileName, msgBag.FileSize , msgBag.Offset);
                SendData(MessageType.UploadFileBegined,"ok");
            }
            catch (Exception ex)
            {
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }

       void handleMethodInvoke(MessageBag msgBag)
        {
            try
            {
                string remoteName = msgBag.ClassFullName;
                var pageDefine = checkRemotingName(remoteName);

                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();

                 
                MethodInfo methodinfo = pageDefine.Methods.Single(m=>m.Name == msgBag.MethodName);
                var pInfos = methodinfo.GetParameters();
                if (pInfos.Length != msgBag.Parameters.Length)
                    throw new Exception($"{msgBag.MethodName}参数个数不相符");
                object[] parameters = new object[pInfos.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type pType = pInfos[i].ParameterType;

                    if (pType.GetTypeInfo().IsValueType)
                    {
                        parameters[i] = Convert.ChangeType(msgBag.Parameters[i], pType);
                    }
                    else
                    {
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(msgBag.Parameters[i], pType);
                    }
                }
                var result = methodinfo.Invoke(currentPage, parameters);
                SendData(MessageType.Result, result);
            }
            catch (Exception ex)
            {
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }

        public void SendData(MessageType msgType, object resultObj)
        {
            try
            {
                lock (this)
                {
                    string objstr;
                    if (resultObj is Dictionary<string, object>[])
                    {

                        objstr = ResultHelper.ToJson((Dictionary<string, object>[])resultObj);
                    }
                    else
                    {
                        objstr = Newtonsoft.Json.JsonConvert.SerializeObject(resultObj);
                    }
                    var dataStr = "{\"result\":" + objstr + ",\"type\":" + ((int)msgType) + "}";
                    mSendDataFunc(dataStr);
                }
            }
            catch (Exception ex)
            {
                mCloseStreamHandler();
                throw ex;
            }
        }
        public void SendClientMessage(string msg)
        {
            try
            {
                lock (this)
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        msg = msg
                    });
                    mSendDataFunc(data);
                }
            }
            catch (Exception ex)
            {
                mCloseStreamHandler();
                throw ex;
            }
        }

    }
    class TypeDefine
    {
        public Type ControllerType;
        public List<MethodInfo> Methods = new List<MethodInfo>();
    }
    class MessageBag
    {
        public string SessionID;
        public string GroupName;
        public string ClassFullName;
        public string Action;
        public string MethodName;
        public string FileName;
        public int FileSize;
        public int Offset;
        public string[] Parameters;
    }

    enum MessageType
    {
        Result = 1,
        Notify = 2,
        SendSessionID = 3,
        InvokeError = 4,
        UploadFileBegined = 5
    }

}
