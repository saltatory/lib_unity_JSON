using System.Collections.Generic;
using LitJson;

    public class Json
    {
        /**
         * @throw JsonParseException
         */
        public static object parse( string jsonInput )
        {
            try
            {
                return JsonMapper.ToObjectSimple(jsonInput);
            } catch ( LitJson.JsonException ex )
            {
                throw new JsonParseException("Internal JSON parse error:", ex);
            }
        }

        public static string serialize( object input )
        {
            try
            {
                return JsonMapper.ToJson(input);
            } catch ( LitJson.JsonException )
            {
                return null;
                //throw new JsonSerializeException("Internal JSON parse error:", ex);
            }
        }
    }
