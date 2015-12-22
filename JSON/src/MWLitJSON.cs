/// <summary>
/// MWLitJSON : LitJSON implementation for IMWJSON interface 
/// </summary>
/// 
/// Created by Steve Chang on 11/30/2015
/// Last Modified 12/1/2015
/// 
/// Copyright : MobilityWare 2015-2016
/// 
/// 
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using MWJSON.LitJson;

namespace MWJSON
{
	public delegate void JSONErrorEventHandler(object sender, EventArgs eventArgs);
	    
	public class MWLitJSON : IMWJSON {

		#region - Public fields -
/// <summary>
/// Lasy Singleton
/// </summary>
		public static MWLitJSON Instance {
			get {
				if( _Instance == null )	{
					_Instance = new MWLitJSON();
				}
				return _Instance;
			}
		}

		public static event JSONErrorEventHandler ErrorEvent = delegate { };        
		#endregion

		#region - Private fields -
		static MWLitJSON _Instance  = null;
		static Version _ver = new Version(1,0,0,0);
		#endregion


		#region - Default Contructor/Destructor -
		static MWLitJSON () {
			JsonMapper.RegisterExporter<UnityEngine.Vector3> (Vector3Exporter);
			JsonMapper.RegisterExporter<UnityEngine.Vector2> (Vector2Exporter);
			JsonMapper.RegisterExporter<UnityEngine.Vector4> (Vector4Exporter);
			JsonMapper.RegisterExporter<Single> (FloatExporter);
			JsonMapper.RegisterExporter<UnityEngine.Quaternion> (QuaternionExporter);
			JsonMapper.RegisterExporter<UnityEngine.Color> (ColorExporter);
			JsonMapper.RegisterExporter<UnityEngine.Matrix4x4> (Matrix4x4Exporter);

			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Vector2> (Vector2Importer);
			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Vector3> (Vector3Importer);
			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Vector4> (Vector4Importer);
			JsonMapper.RegisterImporter<double, Single> (FloatImporter);
			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Quaternion> (QuaternionImporter);
			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Color> (ColorImporter);
			JsonMapper.RegisterImporter<Dictionary<string,float>, UnityEngine.Matrix4x4> (Matrix4x4Importer); // doesn't work????
		}
		#endregion

		#region - Interface implemenation -
		public Version GetVersion()	{
			return _ver;
		}

		/// <summary>
		/// Serializes an object to a json string
		/// </summary>
		/// <param name='data'>
		/// The object to serialize into json
		/// </param>
		/// <returns>
		/// String representing resultant json data
		/// </returns> 
		public string SerializeToJson (object data, bool usePrintableFormatting = false) {
			string result = "";
			JsonWriter writer = new JsonWriter ();
			writer.PrettyPrint = usePrintableFormatting;
			
			try {
				JsonMapper.ToJson (data, writer);
				result = writer.ToString ();
			} catch (JsonException exception) {
				Debug.LogError ("Error serializing data to JSON. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}
		
		public object DeserializeFromJson(string jsonstring)
		{
			return GetDictionaryForJson (jsonstring);
		}
		
		public T DeserializeFromJson<T>(string jsonstring)
		{
			return GetTemplatedTypeForJson<T> (jsonstring);
		}
		#endregion

		#region - Private Methods -
		private static void OnErrorEvent (EventArgs eventArgs) {
			if (ErrorEvent != null) {
				ErrorEvent (null, eventArgs);   
			}
		}		
		#endregion

		#region  - Deserialization Functions -      
		/// <summary>
		/// Populates and returns IDictionary with data from a json string
		/// </summary>
		/// <param name='json'>
		/// The string that stores a json document
		/// </param>
		/// <returns>
		/// The resulting IDictionary storing json data
		/// </returns>
		public static IDictionary GetDictionaryForJson (string json) {    
			IDictionary result = null;
			try {
				result = JsonMapper.ToObject<Dictionary<string, object> > (json); 
			} catch (JsonException exception) {
				Debug.LogError ("Error getting Dictionary for JSON string. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}
	    
		/// <summary>
		/// Populates and returns IDictionary with data from a json document
		/// </summary>
		/// <param name='reader'>
		/// StreamReader that has opened a json document
		/// </param>
		/// <returns>
		/// The resulting IDictionary storing json data
		/// </returns>        
		public static IDictionary GetDictionaryForJson (StreamReader reader) {
			IDictionary result = null;
			try {
				result = JsonMapper.ToObject<Dictionary<string, object> > (reader); 
			} catch (JsonException exception) {
				Debug.LogError ("Error getting Dictionary for JSON document. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}

		/// <summary>
		/// Populates and returns IList with data from a json document
		/// </summary>
		/// <param name='json'>
		/// She string that stores a json document
		/// </param>
		/// <returns>
		/// The resulting IList storing json data
		/// </returns>         
		public static IList GetListForJson (string json) {
			IList result = null;
			try {
				result = JsonMapper.ToObject<List<object> > (json);
			} catch (JsonException exception) {
				Debug.LogError ("Error getting List for JSON string. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}
	    
		/// <summary>
		/// Populates and returns IList with data from a json document
		/// </summary>
		/// <param name='reader'>
		/// StreamReader that has opened a json document
		/// </param>
		/// <returns>
		/// The resulting IList storing json data
		/// </returns>         
		public static IList GetListForJson (StreamReader reader) {
			IList result = null;
			try {
				result = JsonMapper.ToObject<List<object> > (reader); 
			} catch (JsonException exception) {
				Debug.LogError ("Error getting List for JSON document. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}     
	    
		/// <summary>
		/// Populates and returns templated object with data from a json string
		/// </summary>
		/// <param name='json'>
		/// She string that stores a json document
		/// </param>
		/// <returns>
		/// The resulting templated object storing json data
		/// </returns> 
		public static T GetTemplatedTypeForJson<T> (string json) {
			T result = default(T);
			try {
				result = JsonMapper.ToObject<T> (json);
			} catch (JsonException exception) {
				Debug.LogError ("Error getting templated object for JSON string. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}
	    
		/// <summary>
		/// Populates and returns templated object with data from a json string
		/// </summary>
		/// <param name='reader'>
		/// StreamReader that has opened a json document
		/// </param>
		/// <returns>
		/// The resulting templated object storing json data
		/// </returns> 
		public static T GetTemplatedTypeForJson<T> (StreamReader reader) {
			T result = default(T);
			try {
				result = JsonMapper.ToObject<T> (reader);
			} catch (JsonException exception) {
				Debug.LogError ("Error getting templated object for JSON document. " + exception.ToString ());
				OnErrorEvent (EventArgs.Empty);
			}
			return result;
		}        
		#endregion 
	    
		#region - Vector -
		/// <summary>
		/// Converts a float to a doauble, needed as LitJson reads numbers as double, and unity uses floats
		/// </summary>
		/// <param name="value">double Value.</param>
		public static void FloatExporter (System.Single value, JsonWriter writer) {
			writer.Write (value);
		}
	    
		/// <summary>
		/// Converts a double to a float (Single), needed as LitJson reads numbers as double, and Vector3s are floats
		/// </summary>
		/// <param name="value">double Value.</param>
		public static System.Single FloatImporter (double value) {
			return (System.Single)value;
		}
		#endregion 

		#region - Vector -
		/// <summary>
		/// uses the writer to convert the vector3 to a string
		/// </summary>
		/// <param name="vector">input vector.</param>
		/// <param name="writer">Writer.</param>
		public static void Vector2Exporter (UnityEngine.Vector2 vector, JsonWriter writer) {
			writer.WriteObjectStart ();
			writer.WritePropertyName ("x");
			writer.Write (vector.x);
			writer.WritePropertyName ("y");
			writer.Write (vector.y);
			writer.WriteObjectEnd ();
		}


		/// <summary>
		/// imports a vector3, assumes format was using Vector3Exporter
		/// </summary>
		/// <returns>a Vector3.</returns>
		/// <param name="floatDict">string to float dictionary.</param>
		public static UnityEngine.Vector2 Vector2Importer (Dictionary<string,float> floatDict) {
			float x = floatDict ["x"];
			float y = floatDict ["y"];
			UnityEngine.Vector2 vector = new UnityEngine.Vector2 (x, y);
			return vector;
		}

		/// <summary>
		/// uses the writer to convert the vector3 to a string
		/// </summary>
		/// <param name="vector">input vector.</param>
		/// <param name="writer">Writer.</param>
		public static void Vector3Exporter (UnityEngine.Vector3 vector, JsonWriter writer) {
			writer.WriteObjectStart ();
			writer.WritePropertyName ("x");
			writer.Write (vector.x);
			writer.WritePropertyName ("y");
			writer.Write (vector.y);
			writer.WritePropertyName ("z");
			writer.Write (vector.z);
			writer.WriteObjectEnd ();
		}
	    
	    
		/// <summary>
		/// imports a vector3, assumes format was using Vector3Exporter
		/// </summary>
		/// <returns>a Vector3.</returns>
		/// <param name="floatDict">string to float dictionary.</param>
		public static UnityEngine.Vector3 Vector3Importer (Dictionary<string,float> floatDict) {
			float x = floatDict ["x"];
			float y = floatDict ["y"];
			float z = floatDict ["z"];
			UnityEngine.Vector3 vector = new UnityEngine.Vector3 (x, y, z);
			return vector;
		}

		/// <summary>
		/// uses the writer to convert the vector3 to a string
		/// </summary>
		/// <param name="vector">input vector.</param>
		/// <param name="writer">Writer.</param>
		public static void Vector4Exporter (UnityEngine.Vector4 vector, JsonWriter writer) {
			writer.WriteObjectStart ();
			writer.WritePropertyName ("x");
			writer.Write (vector.x);
			writer.WritePropertyName ("y");
			writer.Write (vector.y);
			writer.WritePropertyName ("z");
			writer.Write (vector.z);
			writer.WritePropertyName ("w");
			writer.Write (vector.w);
			writer.WriteObjectEnd ();
		}

		public static void Matrix4x4Exporter (UnityEngine.Matrix4x4 matrix, JsonWriter writer) {
			writer.WriteObjectStart ();
			for (int i = 0; i < 16; i++) {
				writer.WritePropertyName ("v" + i.ToString ());
				writer.Write (matrix [i]);
			}
			writer.WriteObjectEnd ();
		}
	    
		/// <summary>
		/// imports a vector3, assumes format was using Vector3Exporter
		/// </summary>
		/// <returns>a Vector3.</returns>
		/// <param name="floatDict">string to float dictionary.</param>
		public static UnityEngine.Vector4 Vector4Importer (Dictionary<string,float> floatDict) {
			float x = floatDict ["x"];
			float y = floatDict ["y"];
			float z = floatDict ["z"];
			float w = floatDict ["w"];
			UnityEngine.Vector4 vector = new UnityEngine.Vector4 (x, y, z, w);
			return vector;
		}


		public static UnityEngine.Matrix4x4 Matrix4x4Importer (Dictionary<string,float> floatDict) {
			UnityEngine.Matrix4x4 matrix = new UnityEngine.Matrix4x4 ();
			for (int i = 0; i< 16; i++) {
				matrix [i % 4, i / 4] = floatDict ["v" + i.ToString ()];
			}
	//          Debug.Error(null, "M = " + matrix.ToString());
			return matrix;
		}
		#endregion - Vector -

	    #region - Color -
		/// <summary>
		/// uses the writer to convert the Color to a string
		/// </summary>
		/// <param name="quaternion">input quaternion.</param>
		/// <param name="writer">Writer.</param>
		public static void ColorExporter (UnityEngine.Color color, JsonWriter writer) {
			writer.WriteObjectStart ();
			writer.WritePropertyName ("r");
			writer.Write (color.r);
			writer.WritePropertyName ("g");
			writer.Write (color.g);
			writer.WritePropertyName ("b");
			writer.Write (color.b);
			writer.WritePropertyName ("a");
			writer.Write (color.a);
			writer.WriteObjectEnd ();
		}
	    
	    
		/// <summary>
		/// imports a quaternion, assumes format was using QuaternionExporter
		/// </summary>
		/// <returns>a quaternion.</returns>
		/// <param name="floatDict">string to float dictionary.</param>
		public static UnityEngine.Color ColorImporter (Dictionary<string,float> floatDict) {
			float r = floatDict ["r"];
			float g = floatDict ["g"];
			float b = floatDict ["b"];
			float a = floatDict ["a"];
			UnityEngine.Color color = new UnityEngine.Color (r, g, b, a);
			return color;
		}
	    #endregion - Color -

		#region - Quaternion -
		/// <summary>
		/// uses the writer to convert the quaternion to a string
		/// </summary>
		/// <param name="quaternion">input quaternion.</param>
		/// <param name="writer">Writer.</param>
		public static void QuaternionExporter (UnityEngine.Quaternion quaternion, JsonWriter writer) {
			writer.WriteObjectStart ();
			writer.WritePropertyName ("x");
			writer.Write (quaternion.x);
			writer.WritePropertyName ("y");
			writer.Write (quaternion.y);
			writer.WritePropertyName ("z");
			writer.Write (quaternion.z);
			writer.WritePropertyName ("w");
			writer.Write (quaternion.w);
			writer.WriteObjectEnd ();
		}
	    
	    
		/// <summary>
		/// imports a quaternion, assumes format was using QuaternionExporter
		/// </summary>
		/// <returns>a quaternion.</returns>
		/// <param name="floatDict">string to float dictionary.</param>
		public static UnityEngine.Quaternion QuaternionImporter (Dictionary<string,float> floatDict) {
			float x = floatDict ["x"];
			float y = floatDict ["y"];
			float z = floatDict ["z"];
			float w = floatDict ["w"];
			UnityEngine.Quaternion quaternion = new UnityEngine.Quaternion (x, y, z, w);
			return quaternion;
		}
		#endregion  - Quaternion -
	}
}