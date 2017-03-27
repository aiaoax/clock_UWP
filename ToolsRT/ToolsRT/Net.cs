using System;
using System.Collections.Generic;

using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Connectivity;
using Windows.Storage.Pickers;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class Net {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static IAsyncOperation<string> DownloadAsync(string url) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var client = new HttpClient();
					var response = await client.GetAsync(url);
					return await response.Content.ReadAsStringAsync();
				});
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static IAsyncOperation<IReadOnlyList<string>> IpaddrListAsync() {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var ret = new List<string>();
					var lip = NetworkInformation.GetInternetConnectionProfile();
					if(lip != null && lip.NetworkAdapter != null) {
						await Task.Run(() => {
							var hostnames = NetworkInformation.GetHostNames();
							foreach(var item in hostnames) {
								ret.Add(item.RawName);
							}
						});
					}
					return (IReadOnlyList<string>)ret;
				});
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static IAsyncOperation<string> IpaddrStringAsync() {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var ret = "";
					var lip = NetworkInformation.GetInternetConnectionProfile();
					if(lip != null && lip.NetworkAdapter != null) {
						await Task.Run(() => {
							var hostnames = NetworkInformation.GetHostNames();
							foreach(var item in hostnames) {
								ret += item.RawName + Environment.NewLine;
							}
						});
					}
					return ret;
				});
			});
		}

	}
}
