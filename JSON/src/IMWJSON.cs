namespace Mobilityware.JSON
{
	public interface IJSON {
		string SerializeToJson (object data, bool usePrintableFormatting = false);
		object DeserializeFromJson (string jsonString);
		T DeserializeFromJson<T> (string jsonString);
	}
}