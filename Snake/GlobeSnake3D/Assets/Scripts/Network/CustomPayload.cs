using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class CustomPayload
{
    public static byte code = 0x33;
    public QuaternionSerializer quat;
    public float degsPerSecond;
    public double time;

    public CustomPayload(Quaternion quat, float degsPerSecond)
    {
        this.quat = quat;
        this.degsPerSecond = degsPerSecond;
        time = PhotonNetwork.time;
    }

    public CustomPayload(Quaternion quat, float degsPerSecond, double time)
    {
        this.quat = quat;
        this.degsPerSecond = degsPerSecond;
        this.time = time;
    }

    public static object Deserialize(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

    public static byte[] Serialize(object customType)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, customType);
            return ms.ToArray();
        }
    }
}
