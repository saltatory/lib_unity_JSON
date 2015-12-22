using System;

namespace MWJSON.LitJson
{
//// Must match namespace of Json.cs
//// Wraps Json.cs's implementation-specific exception
//// -Curi
    public class JsonParseException : ApplicationException
    {
        public JsonParseException() : base()
        {
        }

        public JsonParseException( string message ) : base(message)
        {
        }

        public JsonParseException( string message, Exception inner_exception ) : base (message, inner_exception)
        {
        }
    }
}
