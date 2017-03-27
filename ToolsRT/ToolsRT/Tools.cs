//Ver 1.1.0.0
/* 更新履歴
 * V1.0.0.0 ToolsRT作成
 * V1.1.0.0 VS2017専用に変更
*/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class PageInfo {
		/// <summary>
		/// 
		/// </summary>
		public static void backButton(Frame frame) {
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility
				= frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

		}
		/* 
	 
	SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
	
	protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
		base.OnNavigatingFrom(e);
		SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
	}

	private void MainPage_BackRequested(object sender,BackRequestedEventArgs e) {
		if(Frame.CanGoBack) {
			Frame.GoBack();
			e.Handled = true;
		}
	}
	 */

	}


	/// <summary>
	/// 
	/// </summary>
	public sealed class ApplicationSetting {
		/// <summary>
		/// この中に <see cref="Page"/> を入れると、
		/// <see cref="MessageBox.rootPage"/> と <see cref="Screens.rootPage"/> と <see cref="Sounds.rootPage"/>
		///  に代入されます。
		/// </summary>
		public static Page rootPage { get; set; }
	}

	/// <summary>
	/// デバイスの情報を取得します。
	/// </summary>
	public sealed class DeviceSetting {

		/// <summary>
		/// OSがWindows 10 mobileかどうかを判定します。
		/// </summary>
		public static bool isMobile { get { return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile"; } }

		/// <summary>
		/// OSがWindows 10 Desktop かどうかを判定します。
		/// </summary>
		public static bool isDesktop { get { return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop"; } }

		/// <summary>
		/// デバイス名を取得します。
		/// </summary>
		public static string deviceName { get { return (new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation()).FriendlyName; } }

		/// <summary>
		/// OS名を取得します。
		/// </summary>
		public static string osName { get { return (new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation()).OperatingSystem; } }

		/// <summary>
		/// 機種名を取得します
		/// </summary>
		public static string productName { get { return (new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation()).SystemProductName; } }
	}

	/// <summary>
	/// テーマ
	/// </summary>
	public enum selectTheme {
		/// <summary>
		/// デフォルト
		/// </summary>
		Default,
		/// <summary>
		/// 明るい
		/// </summary>
		Light,
		/// <summary>
		/// 暗い
		/// </summary>
		Dark
	}

	/// <summary>
	/// 
	/// </summary>
	public enum selectOrientation {
		/// <summary>
		/// 
		/// </summary>
		Landscape,

		/// <summary>
		/// 
		/// </summary>
		Portrait
	}

	/// <summary>
	/// 時間を計測します
	/// </summary>
	public static class Times {
		/// <summary>
		/// タイマーをスタートします
		/// </summary>
		public static void Start() {
			Data.SaveTemp("Tools_time_tmp",DateTime.Now.Ticks);
		}

		/// <summary>
		/// タイマーをストップします
		/// </summary>
		public static async void Stop() {
			long time = long.Parse($"{await Data.LoadTempAsync("Tools_time_tmp")}");
			time = DateTime.Now.Ticks - time;
			Debug.WriteLine("");
			Debug.WriteLine(time);
			Debug.WriteLine(new TimeSpan(time));
			Debug.WriteLine("");
		}
	}

}
