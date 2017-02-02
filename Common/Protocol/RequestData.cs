using MyCommon;

namespace Common.Protocol
{
	  public enum RequestTypes
	  {
			Null,
			IdentificationStart,
			IdentificationEnd,
			Leave
	  };

	  /// <summary>
	  /// Client to Server Data request packet 'allways' return ResultData
	  /// </summary>
	  public class RequestData : Data
	  {
			public int id; //Client size autoindent
			public RequestTypes type;
			public string data;

			public RequestData(int Id, RequestTypes Type, string Data)
			{
				  dtype = DataTypes.Request;
				  id = Id;
				  type = Type;
				  data = Data;
			}

			public RequestData(ref byte[] bytes)
			{
				  if(!Binary.TryToInt(ref bytes, out id))
						return;

				  int itype;
				  if(!Binary.TryToInt(ref bytes, out itype))
						return;

				  type = (RequestTypes)itype;

				  if(!Binary.TryToString(ref bytes, out data))
						return;
			}

			public override byte[] ToBytes()
			{
				  return Binary.AddBytes(
					  Binary.FromInt((int)dtype),
					  Binary.FromInt(id),
					  Binary.FromInt((int)type),
					  Binary.FromString(data)
					  );
			}
	  }
}