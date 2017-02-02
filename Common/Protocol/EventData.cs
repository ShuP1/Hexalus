using MyCommon;

namespace Common.Protocol
{
	  public enum EventTypes
	  {
			None,
			ServerClose,
			ChatMessage
	  };

	  public class EventData : Data
	  {
			public EventTypes type;
			public string[] data;

			public EventData(EventTypes Type, string[] Data = null)
			{
				  dtype = DataTypes.Event;
				  type = Type;
				  data = Data;
			}

			public EventData(ref byte[] bytes)
			{
				  int itype;
				  if(!Binary.TryToInt(ref bytes, out itype))
						return;

				  type = (EventTypes)itype;

				  if(!Binary.TryToStringArray(ref bytes, out data))
						return;
			}

			public override byte[] ToBytes()
			{
				  return Binary.AddBytes(
					  Binary.FromInt((int)dtype),
					  Binary.FromInt((int)type),
					  Binary.FromStringArray(data)
					  );
			}
	  }
}