using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace KCPNet
{
    public partial class Utility
    {
        public class Bytes
        {
            public static byte[] Serialize<T>(T msg)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(ms, msg);
                        ms.Seek(0, SeekOrigin.Begin);
                        return ms.ToArray();
                    }
                    catch (SerializationException e)
                    {
                        Utility.Debug.Error($"Serialize Failed:{e.Message}");
                        throw;
                    }
                }
            }
            public static T Deserialize<T>(byte[] bytes)
            {
                using(MemoryStream ms=new MemoryStream(bytes))
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        T msg = (T)bf.Deserialize(ms);
                        return msg;
                    }
                    catch(SerializationException e)
                    {
                        Utility.Debug.Error($"Deserialize Failed:{e.Message}");
                        throw;
                    }
                }
            }
            public static byte[] Compress(byte[] bytes)
            {
                using (MemoryStream outMs=new MemoryStream())
                {
                    using (GZipStream gz = new GZipStream(outMs, CompressionMode.Compress, true))
                    {
                        //todo 可能出现问题
                        gz.Write(bytes,0,bytes.Length);
                        gz.Close();
                        return outMs.ToArray();
                    }
                }
            }
            public static byte[] Decompress(byte[] bytes)
            {
                using (MemoryStream outMs=new MemoryStream())
                {
                    using (MemoryStream inputMs=new MemoryStream(bytes))
                    {
                        using (GZipStream gz = new GZipStream(inputMs, CompressionMode.Decompress))
                        {
                            byte[] data = new byte[1024];
                            int len = 0;
                            while ((len = gz.Read(data, 0, data.Length)) > 0)
                            {
                                outMs.Write(data, 0, len);
                            }
                            gz.Close();
                            return outMs.ToArray();
                        }
                    }
                }
            }
        }
    }
}
