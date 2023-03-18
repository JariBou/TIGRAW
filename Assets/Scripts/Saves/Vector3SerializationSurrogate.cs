using System.Numerics;
using System.Runtime.Serialization;

namespace Saves
{
    public class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 vector = (Vector3)obj;
            info.AddValue("x", vector.X);
            info.AddValue("y", vector.Y);
            info.AddValue("z", vector.Z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 vector = (Vector3)obj;
            vector.X = (float)info.GetValue("x", typeof(float));
            vector.Y = (float)info.GetValue("y", typeof(float));
            vector.Z = (float)info.GetValue("z", typeof(float));
            obj = vector;
            return obj;
        }
    }
}