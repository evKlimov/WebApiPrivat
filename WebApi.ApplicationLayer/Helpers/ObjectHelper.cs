using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebApi.ApplicationLayer.Helpers
{
    public static class ObjectHelper
    {
        public static byte[] ToBytes(this object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object FromBytes(this byte[] array)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(array, 0, array.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return binForm.Deserialize(memStream);
            }
        }
    }
}
