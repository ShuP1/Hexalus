using MyCommon;

namespace Common.Protocol
{
	  public enum ResultStatus { Error, OK };
	  public enum ResultErrors { UnknownException, UnknownType, Disconnected, WIP };

	  /// <summary>
	  /// Server to Client Result from RequestData
	  /// </summary>
	  public class ResultData : Data
	  {
			public int id; //Client Side Autoindent
			public ResultStatus status;
			public string[] result;

			public ResultData(int Id, ResultStatus Status, string[] Result = null)
			{
				  dtype = DataTypes.Result;
				  id = Id;
				  status = Status;
				  result = Result;
			}

			public ResultData(int Id, RequestResult Result)
			{
				  id = Id;
				  status = Result.status;
				  result = Result.result;
			}

			public ResultData(ref byte[] bytes)
			{
				  if(!Binary.TryToInt(ref bytes, out id))
						return;

				  int istatus;
				  if(!Binary.TryToInt(ref bytes, out istatus))
						return;

				  status = (ResultStatus)istatus;

				  if(!Binary.TryToStringArray(ref bytes, out result))
						return;
			}

			public override byte[] ToBytes()
			{
				  return Binary.AddBytes(
					  Binary.FromInt((int)dtype),
					  Binary.FromInt(id),
					  Binary.FromInt((int)status),
					  Binary.FromStringArray(result)
					  );
			}
	  }
}