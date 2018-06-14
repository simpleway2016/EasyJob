﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib
{
    public class RSADecrptException : Exception
    {
    }
    public enum RSAKeyType
    {
        PKCS1 = 0,
        PKCS8 = 1
    }
    public class RSA : IDisposable
    {
        System.Security.Cryptography.RSA _rsa;
        System.Security.Cryptography.RSAParameters _parameter;
        string _KeyExponent;
        public string KeyExponent
        {
            get { return _KeyExponent; }
        }

        string _KeyModulus;
        public string KeyModulus
        {
            get { return _KeyModulus; }
        }
        public byte[] D
        {
            get
            {
                return _parameter.D;
            }
        }
        public byte[] Exponent
        {
            get
            {
                return _parameter.Exponent;
            }
        }
        public byte[] Modulus
        {
            get
            {
                return _parameter.Modulus;
            }
        }
        const int MAXLENGTH = 110;
        public RSA():this(1024)
        {
           
        }
        public RSA(int keysize)
        {
            _rsa = System.Security.Cryptography.RSA.Create();
            _rsa.KeySize = keysize;//默认是2048，也就是_parameter.Modulus是256字节，但是js那边的算法会卡死
            //把公钥适当转换，准备发往客户端
            _parameter = _rsa.ExportParameters(true);
            _KeyExponent = BytesToHexString(_parameter.Exponent);
            _KeyModulus = BytesToHexString(_parameter.Modulus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">内容建议使用System.Net.WebUtility.UrlEncode编码一次，避免中文乱码</param>
        /// <param name="exponent"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public static string EncryptByKey(string content , byte[] exponent,byte[] modulus)
        {
            RSAParameters rp = new RSAParameters();
            rp.Exponent = exponent;
            rp.Modulus = modulus;

            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(rp);

            if (content.Length <= MAXLENGTH)
            {
                var data = rsa.Encrypt(System.Text.Encoding.ASCII.GetBytes(content), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                return BytesToHexString(data);
            }
            else
            {
                var result = new StringBuilder();
                var total = content.Length;
                for (var i = 0; i < content.Length; i += MAXLENGTH)
                {
                    var text = content.Substring(i, Math.Min(MAXLENGTH, total));
                    total -= text.Length;
                    var data = rsa.Encrypt(System.Text.Encoding.ASCII.GetBytes(text), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                    result.Append( BytesToHexString(data));
                }
                return result.ToString();
            }

          
        }

      
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        private static byte[] pkcs8ToPkcs1(byte[] pkcs8)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading    
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes    
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);        //read the Sequence OID    
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct    
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)    //expect an Octet string    
                    return null;

                bt = binr.ReadByte();        //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count    
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key    

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                return rsaprivkey;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        public static System.Security.Cryptography.RSA CreateRsaFromPrivateKey(string privateKey , RSAKeyType keytype = RSAKeyType.PKCS1)
        {
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            if(keytype == RSAKeyType.PKCS8)
            {
                privateKeyBits = pkcs8ToPkcs1(privateKeyBits);
            }
            var rsa = System.Security.Cryptography.RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

      

        public static System.Security.Cryptography.RSA CreateRsaFromPublicKey(string publicKeyString)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            x509key = Convert.FromBase64String(publicKeyString);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = System.Security.Cryptography.RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        /// <summary>
        /// 根据字符串类型的private Key获取RSAParameters，如果是public key，可以使用Chilkat
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static RSAParameters GetRSAParameters(string  privateKey)
        {
            return getRSAPrivateKey( Convert.FromBase64String(privateKey) );
        }
        static RSAParameters getRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    throw new Exception("转换私钥失败");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    throw new Exception("转换私钥失败");
                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("转换私钥失败");


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);
                
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                return RSAparams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                binr.Dispose();
            }
        }

        /// <summary>
        /// 解开Exponent加密的内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] content)
        {
            return _rsa.Decrypt(content, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
        }

            /// <summary>
            /// 解开Exponent加密的内容
            /// </summary>
            /// <param name="content"></param>
            /// <returns></returns>
        public string Decrypt(string content)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < content.Length; i += 256)
            {
                byte[] bs = null;
                try
                {
                    bs = _rsa.Decrypt(HexStringToBytes(content, i, 256), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                }
                catch
                {
                    throw new RSADecrptException();
                }
                string str = System.Text.Encoding.ASCII.GetString(bs);
                result.Append(str);
            }

            return result.ToString();
        }

        /// <summary>
        /// 利用D进行加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EncryptByD(string data)
        {
            BigInteger d = new BigInteger(_parameter.D);
            BigInteger n = new BigInteger(_parameter.Modulus);

            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += MAXLENGTH)
            {
                string content = data.Substring(j, Math.Min(MAXLENGTH, data.Length - j));
                byte[] source = System.Text.Encoding.ASCII.GetBytes(content);

                BigInteger biText = new BigInteger(source);
                BigInteger biEnText = biText.modPow(d, n);

                byte[] b = biEnText.getBytes();
                result.Append(BytesToHexString(b));
             }
            return result.ToString();
        }
        /// <summary>
        /// 解开D加密的内容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecryptContentFromDEncrypt(string data)
        {
            return DecryptContentFromDEncrypt(data , _parameter.Exponent, _parameter.Modulus);


        }
        /// <summary>
        ///  解开D加密的内容
        /// </summary>
        /// <param name="data"></param>
        /// <param name="exponent"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public static string DecryptContentFromDEncrypt(string data,byte[] exponent, byte[] modulus)
        {
            BigInteger e = new BigInteger(exponent);
            BigInteger n = new BigInteger(modulus);

            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += 256)
            {
                byte[] source = HexStringToBytes(data, j, 256);
                BigInteger biText = new BigInteger(source);
                BigInteger biEnText = biText.modPow(e, n);

                byte[] b = biEnText.getBytes();
                result.Append(System.Text.Encoding.ASCII.GetString(b));
            }

            return result.ToString();


        }
        public static byte[] HexStringToBytes(string hex)
        {
            return HexStringToBytes(hex ,0 , hex.Length);
        }
        static byte[] HexStringToBytes(string hex, int index, int len)
        {
            if (len == 0)
            {
                return new byte[0];
            }



            byte[] result = new byte[len / 2];
            int myindex = 0;
            int endindex = index + len;
            for (int i = index; i < endindex; i += 2)
            {
                result[myindex] = byte.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                myindex++;
            }

            return result;
        }
        public static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        /// <summary>
        /// 加密内容成base64字符串，支持超长字符串
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="content"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static string EncryptToBase64(System.Security.Cryptography.RSA rsa, byte[] content, RSAEncryptionPadding padding)
        {
            int KEYBIT = rsa.KeySize;
            int RESERVEBYTES = 11;// 加密block需要预留11字节
            int encryptBlockSize = KEYBIT / 8 - RESERVEBYTES;
            List<byte> result = new List<byte>();
            int total = content.Length;
            int index = 0;
            while (total > 0)
            {
                int read = Math.Min(encryptBlockSize, total);
                total -= read;
                byte[] bs = new byte[read];
                Array.Copy(content, index, bs, 0, read);
                bs = rsa.Encrypt(bs, padding);
                result.AddRange(bs);

                index += read;
            }
            return Convert.ToBase64String(result.ToArray());
        }
        public static byte[] DecryptFromBase64(System.Security.Cryptography.RSA rsa, string content, RSAEncryptionPadding padding)
        {
            var data = Convert.FromBase64String(content);
            int KEYBIT = rsa.KeySize;
            int decryptBlockSize = KEYBIT / 8;
            List<byte> result = new List<byte>();
            int total = data.Length;
            int index = 0;
            while (total > 0)
            {
                int read = Math.Min(decryptBlockSize, total);
                total -= read;
                byte[] bs = new byte[read];
                Array.Copy(data, index, bs, 0, read);
                bs = rsa.Decrypt(bs, padding);
                result.AddRange(bs);

                index += read;
            }
            return result.ToArray();
        }

        public void Dispose()
        {
            if(_rsa != null)
            {
                _rsa.Dispose();
                _rsa = null;
            }
        }
    }
}
