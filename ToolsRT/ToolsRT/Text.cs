using Newtonsoft.Json;
using System;

using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;

namespace Tools {
	/// <summary>
	/// テキストファイルを扱うメソッドです。
	/// </summary>
	public sealed class Text {

		/// <summary>
		/// テキストファイルを読み込みます。
		/// </summary>
		/// <param name="filename">(<see cref="string"/>)ファイル名</param>
		/// <returns>(<see cref="string"/>)</returns>
		public static IAsyncOperation<string> ReadAsync(string filename) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					string return_str;
					StorageFolder isf = Package.Current.InstalledLocation;
					var sf = await isf.GetFileAsync(filename);
					using(Stream st = (await sf.OpenReadAsync()).AsStream())
					using(TextReader tr = new StreamReader(st)) {
						return_str = await tr.ReadToEndAsync();
					}
					return return_str;
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static IAsyncOperation<string> ReadFileAsync(string filename) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var sf = await StorageFile.GetFileFromPathAsync(filename);
					return await ReadFileAsync(sf);
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static IAsyncOperation<string> ReadFileAsync(StorageFile sf) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					string return_str;
					using(Stream st = await sf.OpenStreamForReadAsync())
					using(TextReader tr = new StreamReader(st)) {
						return_str = await tr.ReadToEndAsync();
					}
					return return_str;
				});
			});

		}

		/// <summary>
		/// 長い文字列を短く短縮します
		/// </summary>
		/// <param name="str">長い文字列</param>
		/// <param name="length">制限する長さ</param>
		/// <returns></returns>
		public static string shortString(string str,int length) {
			string ret = "";
			for(int i = 0;i < str.Length;i++) {
				ret += str[i];
				if(i > length) {
					ret += "…";
					break;
				}
			}
			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string jsonType(string txt) {
			string str = "", type;
			object json = new object();
			json = JsonConvert.DeserializeObject(txt);
			if(json is Newtonsoft.Json.Linq.JArray) {
				foreach(var item in json as Newtonsoft.Json.Linq.JArray) {
					foreach(var item1 in item as Newtonsoft.Json.Linq.JObject) {
						switch(item1.Value.Type) {
							case Newtonsoft.Json.Linq.JTokenType.Object:
								type = "object";
								break;
							case Newtonsoft.Json.Linq.JTokenType.Integer:
								type = "int";
								break;
							case Newtonsoft.Json.Linq.JTokenType.Float:
								type = "float";
								break;
							case Newtonsoft.Json.Linq.JTokenType.String:
								type = "string";
								break;
							case Newtonsoft.Json.Linq.JTokenType.Boolean:
								type = "bool";
								break;
							case Newtonsoft.Json.Linq.JTokenType.Date:
								type = "DateTime";
								break;
							default:
								type = item1.Value.Type.ToString();
								break;
						}
						str += $"public {type} {item1.Key}{{get;set;}}{Environment.NewLine}";
					}
					str += Environment.NewLine;
				}

			}
			else if(json is Newtonsoft.Json.Linq.JObject) {
				foreach(var item in json as Newtonsoft.Json.Linq.JObject) {
					switch(item.Value.Type) {
						case Newtonsoft.Json.Linq.JTokenType.Object:
							type = "object";
							break;
						case Newtonsoft.Json.Linq.JTokenType.Integer:
							type = "int";
							break;
						case Newtonsoft.Json.Linq.JTokenType.Float:
							type = "float";
							break;
						case Newtonsoft.Json.Linq.JTokenType.String:
							type = "string";
							break;
						case Newtonsoft.Json.Linq.JTokenType.Boolean:
							type = "bool";
							break;
						case Newtonsoft.Json.Linq.JTokenType.Date:
							type = "DateTime";
							break;
						default:
							type = item.Value.Type.ToString();
							break;
					}
					str += $"public {type} {item.Key}{{get;set;}}{Environment.NewLine}";

				}

			}
			return str;
		}
	}
}
