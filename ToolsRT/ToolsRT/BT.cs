using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class BT {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid">(<see cref="string"/>)</param>
		/// <returns>(<see cref="DeviceInformationCollection"/>)</returns>
		public static IAsyncOperation<DeviceInformationCollection> getDeviceInfo(string guid) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await getDeviceInfo(new Guid(guid));
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static IAsyncOperation<DeviceInformationCollection> getDeviceInfo(Guid guid) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var dev = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(guid));
					return await DeviceInformation.FindAllAsync(dev);
				});
			});

		}

	}
}
