using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace clock_UWP {
	/// <summary>
	/// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
	/// </summary>
	public sealed partial class MainPage:Page {
		public MainPage() {
			InitializeComponent();
			timer.Tick += Timer_Tick;
			timer.Interval = new TimeSpan(0,0,0,0,100);
			ApplicationSetting.rootPage = this;
			timeSetAsync();
			timer.Start();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			try {
				ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
			}
			catch(Exception ex) {
				await MessageBox.ShowAsync(ex.Message);
				Debug.WriteLine(ex.Message);
			}
			timeSetAsync();
		}

		private void Timer_Tick(object sender,object e) {
			dt = DateTime.Now.Add(ts);
			if(dt.Minute == 0 && dt.Second < 1) {
				timeSetAsync();
			}
			R_time.Text = $"{dt:HH\\:mm\\:ss}";
			R_date.Text = $"{dt:yyyy/MM/dd (ddd)}";

		}

		DateTime dt;
		TimeSpan ts;

		DispatcherTimer timer = new DispatcherTimer();


		async void timeSetAsync() {
			try {
				HttpClient hc = new HttpClient();
				var html = hc.GetStringAsync("https://ntp-a1.nict.go.jp/cgi-bin/ntp").Result;
				html = Regex.Replace(html,"<.*>","",RegexOptions.Multiline).Trim();
				var ntp = (double.Parse(html));
				dt = new DateTime(1900,1,1,9,0,0).AddSeconds(ntp);
				ts = dt - DateTime.Now;
				//R_time.Text = $"{dt:HH\\:mm\\:ss}";
				//R_date.Text = $"{dt:yyyy/MM/dd (ddd)}";
			}
			catch(Exception ex) {
				await MessageBox.ShowAsync(ex.Message);
				Debug.WriteLine(ex.Message);
				dt = DateTime.Now;
			}
		}

		private void Grid_Tapped(object sender,TappedRoutedEventArgs e) {
			if(ApplicationView.GetForCurrentView().IsFullScreenMode) {
				ApplicationView.GetForCurrentView().ExitFullScreenMode();
			}
			else {
				ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
			}
			timeSetAsync();
		}
	}
}
