using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

//// Implementation-specific. Changed LitJson file JsonMapper.cs by adding "partial" to class declaration.
//// All other changes are encapsulated in this partial class file
//// This is a dumbed-down version of the parser.
//// Everything is int, long, double, bool, string, array (IList) or object (IDictionary)
//// -Curi
namespace LitJson
{
    public partial class JsonMapper
    {
        public static object ToObjectSimple( string json )
        {
            JsonReader reader = new JsonReader (json);

            return ReadValueSimple(reader);
        }


        private static object ReadValueSimple( JsonReader reader )
        {
            reader.Read ();

            if( reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null ) 
            {
                return null;
            }

            if( reader.Token == JsonToken.Double ||
                reader.Token == JsonToken.Int ||
                reader.Token == JsonToken.Long ||
                reader.Token == JsonToken.String ||
                reader.Token == JsonToken.Boolean )
            {
                return reader.Value;
            }


            if( reader.Token == JsonToken.ArrayStart )
            {
                IList newArray = new ArrayList();

                while( true )
                {
                    object item = ReadValueSimple(reader);
                    if( item == null && reader.Token == JsonToken.ArrayEnd )
                    {
                        break;
                    }

                    newArray.Add(item);
                }

                return newArray;
            }
            else if( reader.Token == JsonToken.ObjectStart )
            {
                IDictionary<string,object> newObject = new Dictionary<string,object>();

                while( true )
                {
                    reader.Read();

                    if( reader.Token == JsonToken.ObjectEnd )
                    {
                        break;
                    }

                    string property = (string)reader.Value;

                    newObject[property] = ReadValueSimple(reader);
                }

                return newObject;
            }

            throw new JsonException(String.Format("Unknown JSON type while parsing. Not a double, int, long, string, boolean, array, or object"));
        }


    } /* partial class JsonMapper -- Glendale */
} /* namespace */
