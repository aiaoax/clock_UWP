using System;
using System.Collections.Generic;

using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Tools {
	/// <summary>
	/// データの保存、読み込みをするメソッドです。
	/// </summary>
	public sealed class Data {
		/// <summary>
		/// <see cref="buffer2BitmapImage"/> を使用する前に表示するページを入れてください。(通常は this)
		/// </summary>
		/// <example> 
		/// このコードを <see cref="buffer2BitmapImage"/> を使用する前に入れてください。
		/// <code>
		/// Data.rootPage = this;
		/// </code>
		/// </example>
		public static Page rootPage { get; set; }

		/// <summary>
		/// ローカルデータを保存します。
		/// </summary>
		/// <param name="key">(<see cref="string"/>)キー</param>
		/// <param name="value"><see cref="object"/>値</param>
		public static void Save(string key,object value) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
				container.Values[key] = value;
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.Save",ex.Message);
#endif
			}
		}

		/// <summary>
		/// ローカルデータを読み込みます。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="object"/>読み込みに成功したときに、このメソッドが返される時 object 型で値を格納します。読み込みに失敗したときは null を格納します。</returns>
		public static object Load(string key) {
			object val;
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

				if(container.Values.ContainsKey(key)) {
					val = container.Values[key];
					return val;
				}
				else {
					val = null;
					return val;
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.Load",ex.Message);
#endif
				val = null;
				return val;
			}
		}

		/// <summary>
		/// ローカルデータを読み込みます。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <param name="val"><see cref="object"/>読み込みに成功したときに、このメソッドが返される時 object 型で値を格納します。読み込みに失敗したときは null を格納します。</param>
		/// <returns><see cref="bool"/>読み込みが成功した場合 true 、失敗した時は false を返します。</returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static bool Load(string key,out object val) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
				if(container.Values.ContainsKey(key)) {
					val = container.Values[key];
					return true;
				}
				else {
					val = null;
					return false;
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.Load",ex.Message);
#endif
				val = null;
				return false;
			}
		}

		/// <summary>
		/// ローカルデータを読み込み出来るかどうかを判定します。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="bool"/>読み込みが成功した場合 true 、失敗した時は false を返します。</returns>
		public static bool IsLoad(string key) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
				if(container.Values.ContainsKey(key)) {
					return true;
				}
				else {
					return false;
				}
			}
			catch {
				return false;
			}
		}

		/// <summary>
		/// ローカルデータの中を一覧で取得します
		/// </summary>
		/// <returns><see cref="IReadOnlyList{String}"/></returns>
		public static IReadOnlyList<string> LoadAllKeys() {
			List<string> keys = new List<string>();
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
				foreach(var item in container.Values) {
					keys.Add(item.Key);
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.LoadAllKeys",ex.Message);
#endif
				return new List<string>();
			}
			return keys;
		}

		/// <summary>
		/// ローカルデータを削除します。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="bool"/>削除が成功した場合 true 、失敗した時は false を返します。</returns>
		public static bool Delete(string key) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
				container.Values.Remove(key);
				return true;
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.Delete",ex.Message);
#endif
			}
			return false;
		}

		/// <summary>
		/// ローミングデータを保存します
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <param name="value"><see cref="string"/>値</param>
		public static void SaveRoaming(string key,object value) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				container.Values[key] = value;
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.SaveRoaming",ex.Message);
#endif
			}
		}

		/// <summary>
		/// ローミングデータを読み込みます。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="object"/>読み込みに成功したときに、このメソッドが返される時 object 型で値を格納します。読み込みに失敗したときは null を格納します。</returns>
		public static object LoadRoaming(string key) {
			object val;
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				if(container.Values.ContainsKey(key)) {
					val = container.Values[key];
				}
				else {
					val = null;
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.LoadRoaming",ex.Message);
#endif
				val = null;

			}
			return val;
		}

		/// <summary>
		/// ローミングデータを読み込みます。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <param name="val"><see cref="object"/>読み込みに成功したときに、このメソッドが返される時 object 型で値を格納します。読み込みに失敗したときは null を格納します。</param>
		/// <returns><see cref="bool"/>読み込みが成功した場合 true 、失敗した時は false を返します。</returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static bool LoadRoaming(string key,out object val) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				if(container.Values.ContainsKey(key)) {
					val = container.Values[key];
					return true;
				}
				else {
					val = null;
					return false;
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.LoadRoaming",ex.Message);
#endif
				val = null;
				return false;
			}
		}

		/// <summary>
		/// ローミングデータを読み込み出来るかどうかを判定します。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="bool"/>読み込みが成功した場合 true 、失敗した時は false を返します。</returns>
		public static bool IsLoadRoaming(string key) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				if(container.Values.ContainsKey(key)) {
					return true;
				}
				else {
					return false;
				}
			}
			catch {
				return false;
			}
		}

		/// <summary>
		/// ローミングデータの中を一覧で取得します
		/// </summary>
		/// <returns><see cref="IReadOnlyList{String}"/></returns>
		public static IReadOnlyList<string> LoadRoamingAllKeys() {
			List<string> keys = new List<string>();
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				foreach(var item in container.Values) {
					keys.Add(item.Key);
				}
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.LoadRoamingAllKeys",ex.Message);
#endif
				return new List<string>();
			}
			return keys;
		}

		/// <summary>
		/// ローミングデータを削除します。
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="bool"/>削除が成功した場合 true 、失敗した時は false を返します。</returns>
		public static bool DeleteRoaming(string key) {
			try {
				ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
				container.Values.Remove(key);
				return true;
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.DeleteRoaming",ex.Message);
#endif
			}
			return false;
		}

		/// <summary>
		/// 一時データを保存します
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <param name="value"><see cref="string"/>値</param>
		public static async void SaveTemp(string key,object value) {
			try {
				StorageFolder folder = ApplicationData.Current.TemporaryFolder;
				StorageFile file = await folder.CreateFileAsync(key,CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteTextAsync(file,$"{value}");
			}
			catch(Exception ex) {
#if DEBUG
				Screens.Message(ex.Message);
				Debug.Fail("Data.SaveTemp",ex.Message);
#endif
			}
		}

		/// <summary>
		/// 一時データを読み込みます
		/// </summary>
		/// <param name="key"><see cref="string"/>キー</param>
		/// <returns><see cref="string"/>文字列の値。存在しないときは string.Empty です</returns>
		public static IAsyncOperation<string> LoadTempAsync(string key) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					try {
						StorageFolder folder = ApplicationData.Current.TemporaryFolder;
						StorageFile file = await folder.GetFileAsync(key);
						if(file != null) {
							return await FileIO.ReadTextAsync(file);
						}
						else {
							return string.Empty;
						}
					}
					catch(Exception ex) {
#if DEBUG
						Screens.Message(ex.Message);
						Debug.Fail("Data.LoadTempAsync",ex.Message);
#endif
						return string.Empty;
					}
				});
			});

		}

		/// <summary>
		/// ローカルデータの中を一覧で取得します
		/// </summary>
		/// <returns><see cref="IReadOnlyList{String}"/></returns>
		public static IAsyncOperation<IReadOnlyList<string>> LoadTempAllKeysAsync() {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					List<string> keys = new List<string>();
					try {
						StorageFolder folder = ApplicationData.Current.TemporaryFolder;
						var files = await folder.GetFilesAsync();
						if(files != null) {
							foreach(var item in files) {
								keys.Add(item.Name);
							}
						}
						else {
							keys = new List<string>();
						}
					}
					catch(Exception ex) {
#if DEBUG
						Screens.Message(ex.Message);
						Debug.Fail("Data.LoadTempAllKeysAsync",ex.Message);
#endif
						keys = new List<string>();
					}
					return (IReadOnlyList<string>)keys;
				});
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		public static IAsyncOperation<BitmapImage> buffer2BitmapImage(IBuffer img) {
			if(rootPage == null) {
				if(ApplicationSetting.rootPage != null) {
					rootPage = ApplicationSetting.rootPage;
				}
				else {
					throw new NullReferenceException("Tools.Data.rootPage に 表示するページ(通常は this)を入れてください。");
				}
			}
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					try {
						BitmapImage bi = null;
						await rootPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,async () => {
							bi = new BitmapImage();
							MemoryStream ms = new MemoryStream(img.ToArray());
							await bi.SetSourceAsync(ms.AsRandomAccessStream());
						});
						//ms.Dispose();
						while(bi == null) {
							Task.WaitAll(Task.Delay(1000));
						}
						return bi;
					}
					catch {
						return null;
					}
				});
			});

		}

	}
}
