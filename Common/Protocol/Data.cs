using MyCommon;

namespace Common.Protocol
{
    public enum DataTypes { Request, Result, Event };

    public class Data
    {
        public DataTypes dtype;

        /// <summary>
        /// Create Packet from bytes
        /// </summary>
        /// <param name="bytes">row bytes (remove used bytes)</param>
        public static Data FromBytes(ref byte[] bytes)
        {
            int itype;
            if (!Binary.TryToInt(ref bytes, out itype))
                return null;

            DataTypes dtype = (DataTypes)itype;

            switch (dtype)
            {
                case DataTypes.Request:
                    return new RequestData(ref bytes);

                case DataTypes.Result:
                    return new ResultData(ref bytes);

                case DataTypes.Event:
                    return new EventData(ref bytes);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Generate bytes to send
        /// </summary>
        public virtual byte[] ToBytes() { return new byte[0]; }
    }
}