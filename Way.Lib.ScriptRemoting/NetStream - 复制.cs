﻿//using System;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//namespace Way.Lib.ScriptRemoting
//{
//    public class NetStreamEventArgs 
//    {
//        public bool HasError
//        {
//            get;
//            set;
//        }
//        public byte[] Data
//        {
//            get;
//            set;
//        }
//    }
//    /// <summary>
//    /// NetStream 的摘要说明。
//    /// </summary>
//    public class NetStream : Stream
//    {
//        class ReceiveState
//        {
//            public int Length;
//            public int Offset;
//            public byte[] Data;
//            public ReceivedHandler Callback;
//        }
//        /// <summary>
//        /// 预读取缓冲区
//        /// </summary>
//        List<byte> preBuffer = new List<byte>();

//        private AddressFamily m_Family;
//        public AddressFamily Family
//        {
//            get
//            {
//                return m_Family;
//            }
//        }
//        private Socket m_ClientSocket;
//        public Socket Socket
//        {
//            get
//            {
//                return m_ClientSocket;
//            }
//            private set
//            {
//                m_ClientSocket = value;
//            }
//        }
//        private bool m_Active;
//        private System.Text.Encoding code = System.Text.Encoding.UTF8;
       

//        private const int dataBuffer = 1024;

//        public System.Text.Encoding Encoding
//        {
//            get
//            {
//                return code;
//            }
//            set
//            {
//                code = value;
//            }
//        }

//        private System.Text.Encoding _ErrorEncoding = System.Text.Encoding.UTF8;
//        public System.Text.Encoding ErrorEncoding
//        {
//            get
//            {
//                return _ErrorEncoding;
//            }
//            set
//            {
//                _ErrorEncoding = value;
//            }
//        }

//        protected override void Dispose(bool disposing)
//        {
//            this.Close();
//            base.Dispose(disposing);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Address">主机名或者ip地址</param>
//        /// <param name="port">端口</param>
//        public NetStream(string Address, int port)
//        {
//            IPAddress[] hostAddresses = Dns.GetHostAddresses(Address);
//            Exception exception = null;
//            Socket socket = null;
//            Socket socket2 = null;
//            try
//            {

//                if (Socket.SupportsIPv4)
//                {
//                    socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                    socket2.SendTimeout = 6000;
//                    socket2.ReceiveTimeout = 6000;
//                }
//                if (Socket.OSSupportsIPv6)
//                {
//                    socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
//                    socket.SendTimeout = 6000;
//                    socket.ReceiveTimeout = 6000;
//                }

//                foreach (IPAddress address in hostAddresses)
//                {
//                    try
//                    {

//                        if ((address.AddressFamily == AddressFamily.InterNetwork) && (socket2 != null))
//                        {
//                            socket2.Connect(address, port);
//                            this.m_ClientSocket = socket2;
//                            if (socket != null)
//                            {
//                                socket.Close();
//                            }
//                        }
//                        else if (socket != null)
//                        {

//                            socket.Connect(address, port);
//                            this.m_ClientSocket = socket;
//                            if (socket2 != null)
//                            {
//                                socket2.Close();
//                            }
//                        }
//                        this.m_Family = address.AddressFamily;
//                        this.m_Active = true;

//                    }
//                    catch (Exception exception2)
//                    {
//                        if (((exception2 is ThreadAbortException) || (exception2 is StackOverflowException)) || (exception2 is OutOfMemoryException))
//                        {
//                            throw;
//                        }
//                        exception = exception2;
//                    }
//                }
//            }
//            catch (Exception exception3)
//            {
//                if (((exception3 is ThreadAbortException) || (exception3 is StackOverflowException)) || (exception3 is OutOfMemoryException))
//                {
//                    throw;
//                }
//                exception = exception3;
//            }
//            finally
//            {
//                if (!this.m_Active)
//                {
//                    if (socket != null)
//                    {
//                        socket.Close();
//                    }
//                    if (socket2 != null)
//                    {
//                        socket2.Close();
//                    }
//                    if (exception != null)
//                    {
//                        throw exception;
//                    }
//                    throw new SocketException((int)SocketError.NotConnected);
//                }
//            }


//            //IPHostEntry host = Dns.GetHostEntry(Address);
//            //ip = new IPEndPoint(host.AddressList[0], port);
//            //Socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//            //Socket.SendTimeout = 0;
//            //Socket.ReceiveTimeout = 0;
//            //Socket.Connect(ip.Address, port);

//            //int keepAlive = -1744830460; // SIO_KEEPALIVE_VALS
//            //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x10, 0x27, 0, 0, 0xe8, 0x03, 0, 0 }; // True, 10秒, 1 秒

//            //this.Socket.IOControl(keepAlive, inValue, null);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public byte[] Serialize(object obj)
//        {

//            IFormatter formatter = new BinaryFormatter();
//            MemoryStream ms = new MemoryStream();
//            formatter.Serialize(ms, obj);
//            byte[] bs = new byte[ms.Length];
//            ms.Position = 0;
//            ms.Read(bs, 0, bs.Length);
//            ms.Close();
//            ms.Dispose();
//            return bs;


//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Bytes"></param>
//        /// <returns></returns>
//        public object Deserialize(byte[] Bytes)
//        {

//            IFormatter formatter = new BinaryFormatter();

//            MemoryStream stream = new MemoryStream();
//            stream.Write(Bytes, 0, Bytes.Length);
//            stream.Position = 0;
//            object obj = formatter.Deserialize(stream);
//            stream.Close();
//            stream.Dispose();
//            return obj;

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="SocketClient"></param>
//        public NetStream(Socket SocketClient)
//        {
//            this.m_ClientSocket = SocketClient;
//            this.Socket.SendTimeout = 6000;
//            this.Socket.ReceiveTimeout = 6000;


//            try
//            {
//                //经典代码,再也不用写什么心跳包了，接收数据必须采用BeginReceive
//                byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 }; //True, 20 秒, 2 秒
//                this.Socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
//            }
//            catch { return; }

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Connect()
//        {

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Close()
//        {
//            try
//            {
//                Socket.Close();
                
//            }
//            catch
//            {
//            }

//            try
//            {
//                Socket.Dispose();
//            }
//            catch
//            {
//            }
//            Socket = null;
//        }

//        private void callback(IAsyncResult result)
//        {
//            Socket.EndDisconnect(result);
//            Socket.Close(10);
//        }

//        public bool Connected
//        {
//            get
//            {
//                return Socket.Connected;
//            }
//        }

//        ///// <summary>
//        ///// 读取数据流中所有数据
//        ///// </summary>
//        ///// <returns></returns>
//        //public string ReadToEnd()
//        //{
//        //    if (client.Available > 0)
//        //    {
//        //        byte[] b = new byte[client.Available];
//        //        client.Receive(b, 0, b.Length, SocketFlags.None);
//        //        return code.GetString(b);
//        //    }
//        //    return "";
//        //}

//        public void WriteHtmlFile(string filePath)
//        {
//            StreamReader reader = new System.IO.StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), this.Encoding);
//            while (true)
//            {
//                string line = reader.ReadLine();
//                if (line == null)
//                    break;
//                WriteLine(line);
//            }
//            reader.Close();
//        }

//        public delegate void ReceivedHandler(object sender ,NetStreamEventArgs e );
//        public void BeginReceiveDatas(byte[] buffer, int offset, int size , ReceivedHandler callback)
//        {
//            if (preBuffer.Count >= size)
//            {
//                for (int i = 0; i < size; i++)
//                {
//                    buffer[offset + i] = preBuffer[i];
//                }
//                preBuffer.RemoveRange(0, size);
//                NetStreamEventArgs arg = new NetStreamEventArgs();
//                arg.Data = buffer;
//                callback.Invoke(this, arg);
//                return;
//            }

//            ReceiveState state = new ReceiveState();
//            state.Data = buffer;
//            state.Offset = offset;
//            state.Length = size;
//            state.Callback = callback;
//            try
//            {
//                Socket.BeginReceive(buffer, offset, size, SocketFlags.None, new AsyncCallback(receiveDataCallBack), state);
//            }
//            catch
//            {
//                try
//                {
//                    Socket.Dispose();
//                }
//                catch
//                {
//                }
//                Socket = null;
//                NetStreamEventArgs arg = new NetStreamEventArgs();
//                arg.HasError = true;
//                state.Callback.Invoke(this, arg);
//            }
//        }
//        private void receiveDataCallBack(IAsyncResult ar)
//        {
//            ReceiveState state = (ReceiveState)ar.AsyncState;
//            int length = 0;
//            try
//            {
//                length = Socket.EndReceive(ar);
//            }
//            catch
//            {
//            }
//            if (length == 0 )
//            {
//                try
//                {
//                    Socket.Dispose();
//                }
//                catch
//                {
//                }
//                Socket = null;
//                NetStreamEventArgs arg = new NetStreamEventArgs();
//                arg.HasError = true;
//                state.Callback.Invoke(this, arg);
//                return;
//            }

           
//            state.Offset += length;
//            if (state.Offset < state.Length)
//            {
//                Socket.BeginReceive(state.Data, state.Offset, state.Length - state.Offset, SocketFlags.None, new AsyncCallback(receiveDataCallBack), state);
//            }
//            else
//            {
//                NetStreamEventArgs arg = new NetStreamEventArgs();
//                arg.Data = state.Data;
//                state.Callback.Invoke(this, arg);
//            }
//        }

//        public byte[] ReceiveDatas(int length)
//        {
//            int offset = 0;
//            byte[] buffer = new byte[length];
//            if (preBuffer.Count > 0)
//            {
//                int copylen = Math.Min(length, preBuffer.Count);
//                offset += copylen;
//                preBuffer.CopyTo(0, buffer, 0, copylen);
//                preBuffer.RemoveRange(0, copylen);
//                if (offset == length)
//                    return buffer;
//            }
//            while (true)
//            {
//                var result = Socket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, null, null);
//                result.AsyncWaitHandle.WaitOne();

//                length = Socket.EndReceive(result);
//                if (length == 0)
//                {
//                    try
//                    {
//                        Socket.Dispose();
//                    }
//                    catch
//                    {
//                    }
//                    Socket = null;
//                    throw new Exception("Socket已经断开");
//                }
//                else
//                {
//                    offset += length;
//                    if (offset >= buffer.Length)
//                        break;
//                }
//            }

//            return buffer;
//        }


//        /// <summary>
//        /// 读取一行，或者读取到某个字符则返回
//        /// </summary>
//        /// <param name="encoding"></param>
//        /// <param name="finallyReaded">读取了几个字节</param>
//        /// <param name="_char">指定字符</param>
//        /// <returns></returns>
//        public string ReadLineOREndWithOtherChar(System.Text.Encoding encoding, ref int finallyReaded, char _char)
//        {
//            List<byte> datas = new List<byte>();

//            while (true)
//            {
//                byte[] bs = new byte[1];
//                int flag = Socket.Receive(bs, SocketFlags.None);
//                finallyReaded += flag;
//                if (flag > 0)
//                {
//                    byte b = bs[0];
//                    if (b == _char)
//                    {
//                        return encoding.GetString(datas.ToArray());
//                    }
//                    if (b == 0xa || b == 0xd)
//                    {
//                        if (b == 0xa)
//                            return encoding.GetString(datas.ToArray());
//                    }
//                    else
//                    {
//                        datas.Add(bs[0]);
//                    }
//                }
//            }

//        }



      
//        public string ReadLine()
//        { 
//            int lineEndIndex = -1;
//            if (preBuffer.Count > 0)
//            {
//                for (int i = 0; i < preBuffer.Count; i++)
//                {
//                    byte b = preBuffer[i];
//                    if (b == 0xa  )
//                    {
//                        lineEndIndex = i;
//                        break;
//                    }
//                }

//                if (lineEndIndex > 0)
//                    goto _return;
//            }

//            byte[] buffer = new byte[1024];
//            _doagain:
//            var result = Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,null, null);
//            result.AsyncWaitHandle.WaitOne();

//            int length = Socket.EndReceive(result);

//            if (length == 0)
//            {
//                try
//                {
//                    Socket.Dispose();
//                }
//                catch
//                {
//                }
//                Socket = null;
//                throw new Exception("Socket已经断开");
//            }
           
//            for (int i = 0; i < length; i++)
//            {
//                byte b = buffer[i];
//                preBuffer.Add(b);
//                if (b == 0xa && lineEndIndex < 0)
//                {
//                    lineEndIndex = i;
//                }
//            }
//            if (lineEndIndex < 0)
//                goto _doagain;

//            _return:
//            byte[] lineBytes = new byte[lineEndIndex - 1];
//            preBuffer.CopyTo(0 , lineBytes , 0 , lineBytes.Length);
//            preBuffer.RemoveRange(0, lineEndIndex + 1);

//            return this.code.GetString(lineBytes);
//        }
       

//        public void WriteLine(string text)
//        {
//            byte[] buffer = this.Encoding.GetBytes(text + "\r\n");
//            _write(buffer, 0, buffer.Length);
//        }

//        public void CheckIsActive()
//        {
//            return;

//            //if (this.Socket.Poll(-1, SelectMode.SelectRead) == false)
//            //{
//            //    throw (new System.Net.Sockets.SocketException(10054));
//            //}

//            if (this.Socket.Receive(new byte[1], SocketFlags.Peek) == 0)
//                throw (new System.Net.Sockets.SocketException(10054));
//            //this.Socket.Receive(new byte[0]);
//        }



//        public void Write(byte[] buffer)
//        {
//            Write(buffer, 0, buffer.Length);
//        }

//        public void Write(byte[] buffer, int size)
//        {
//            Write(buffer, 0, size);
//        }

//        public void WriteByEachSize(byte[] buffer, int offset, int count, int eachSize)
//        {
//            if (buffer.Length == 0)
//                return;
//            try
//            {
//                int writed = offset;

//                while (count > 0)
//                {
//                    int f = Socket.Send(buffer, writed, Math.Min(count, eachSize), SocketFlags.None);
//                    writed += f;
//                    count -= f;
//                    if (f == 0)
//                    {
//                        Thread.Sleep(10);
//                    }
//                }
//            }
//            catch
//            {
//            }

//        }

//        /// <summary>
//        /// 按块大小发送数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="eachSize">每次发送多少字节</param>
//        public void WriteByEachSize(byte[] buffer, int eachSize)
//        {
//            WriteByEachSize(buffer, 0, buffer.Length, eachSize);
//        }
//        public int _write(byte[] buffer)
//        {

//            return _write(buffer, 0, buffer.Length);

//        }
//        public int _write(byte[] buffer, int offset, int count)
//        {
//            if (buffer.Length == 0)
//                return 0;

//            if (count > 10240)
//            {
//                WriteByEachSize(buffer, offset, count, 10240);
//                return count;
//            }
//            try
//            {
//                int writed = offset;


//                while (count > 0)
//                {
//                    int f = Socket.Send(buffer, writed, count, SocketFlags.None);
//                    writed += f;
//                    count -= f;
//                    if (f == 0)
//                    {
//                        Thread.Sleep(10);
//                    }
//                }

//                return count;
//            }
//            catch
//            {
//            }
//            return 0;
//        }



//        public override bool CanRead
//        {
//            get { return true; }
//        }

//        public override bool CanSeek
//        {
//            get { return false; }
//        }

//        public override bool CanWrite
//        {
//            get { return true; }
//        }

//        public override void Flush()
//        {

//        }

//        public override long Length
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public override long Position
//        {
//            get
//            {
//                throw new Exception("The method or operation is not implemented.");
//            }
//            set
//            {
//                throw new Exception("The method or operation is not implemented.");
//            }
//        }

//        public override int Read(byte[] buffer, int offset, int count)
//        {
//            int copylen = 0;
//            if (preBuffer.Count > 0)
//            {
//                copylen = Math.Min(count, preBuffer.Count);
                
//                preBuffer.CopyTo(0, buffer, offset, copylen);
//                preBuffer.RemoveRange(0, copylen);

//                offset += copylen;
//                if (copylen == count)
//                    return count;
//                else
//                {
//                    count -= copylen;
//                }
//            }

//            int read = Socket.Receive(buffer, offset, count, SocketFlags.None);
//            if (read == 0)
//                this.CheckIsActive();
//            return read + copylen;
//        }

//        public override long Seek(long offset, SeekOrigin origin)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }

//        public override void SetLength(long value)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//        public delegate void BeforeWriteStringHandler(ref string content);
//        public event BeforeWriteStringHandler BeforeWriteString;
//        public override void Write(byte[] buffer, int offset, int count)
//        {

//            _write(buffer, offset, count);
//        }

//        public override void WriteByte(byte value)
//        {
//            this.Write(new byte[1] { value });
//        }

//        public override int ReadByte()
//        {
//            byte[] bs = this.ReceiveDatas(1);
//            return (int)bs[0];
//        }

//        public void Write(char _char)
//        {
//            this.Write(new byte[1] { (byte)_char });
//        }

//        /// <summary>
//        /// 此方法会写入\0字符，不需要写入\0字符请调用WriteString
//        /// </summary>
//        /// <param name="text"></param>
//        public void Write(string text)
//        {
//            byte[] bs = this.code.GetBytes(text);
//            byte[] newbs = new byte[bs.Length + 1];
//            Array.Copy(bs, newbs, bs.Length);
//            newbs[bs.Length] = 0;
//            this.Write(newbs);

//        }

//        public void WriteString(string text)
//        {
//            if (text != null)
//            {
//                if (BeforeWriteString != null)
//                {
//                    BeforeWriteString(ref text);
//                }

//                byte[] bs = this.code.GetBytes(text);
//                this.Write(bs);
//            }
//        }

//        public void Write(int _int)
//        {
//            byte[] bs = BitConverter.GetBytes(_int);
//            this.Write(bs);
//        }

//        public int ReadInt()
//        {
//            byte[] bs = this.ReceiveDatas(4);
//            return BitConverter.ToInt32(bs, 0);
//        }

//        public double ReadDouble()
//        {
//            byte[] bs = this.ReceiveDatas(8);
//            return BitConverter.ToDouble(bs, 0);
//        }
//        public float ReadFloat()
//        {
//            byte[] bs = this.ReceiveDatas(4);
//            return BitConverter.ToSingle(bs, 0);
//        }
//        public long ReadLong()
//        {
//            byte[] bs = this.ReceiveDatas(8);
//            return BitConverter.ToInt64(bs, 0);
//        }

//        public short ReadShort()
//        {
//            byte[] bs = this.ReceiveDatas(2);
//            return BitConverter.ToInt16(bs, 0);
//        }

//        public bool ReadBoolean()
//        {
//            byte[] bs = this.ReceiveDatas(1);
//            return BitConverter.ToBoolean(bs, 0);
//        }

//        public void Write(bool _int)
//        {
//            byte[] bs = BitConverter.GetBytes(_int);
//            this.Write(bs);
//        }

//        public void Write(short _int)
//        {
//            byte[] bs = BitConverter.GetBytes(_int);
//            this.Write(bs);
//        }

//        public void Write(long _long)
//        {
//            byte[] bs = BitConverter.GetBytes(_long);
//            this.Write(bs);
//        }

//        public void Write(float _float)
//        {
//            byte[] bs = BitConverter.GetBytes(_float);
//            this.Write(bs);
//        }

//        public void Write(double _double)
//        {
//            byte[] bs = BitConverter.GetBytes(_double);
//            this.Write(bs);
//        }
//    }


//}
