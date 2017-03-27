using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Tools {
	/// <summary>
	/// 画面表示に関するメソッドです。
	/// </summary>
	public sealed class Screens {

		/// <summary>
		/// <see cref="Screens"/> を使用する前に表示するページを入れてください。(通常は this)
		/// </summary>
		/// <example> 
		/// このコードを <see cref="Screens"/> を表示する前に入れてください。
		/// <code>
		/// <see cref="rootPage"/> = this;
		/// </code>
		/// </example>
		public static Page rootPage { get; set; }

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <example> 
		/// <code>
		/// <see cref="Message"/>("表示したいテキスト");
		/// </code>
		/// </example>
		public static void Message(string content) {
			Message(content,"");
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="title">(<see cref="string"/>)表示したいタイトル</param>
		public static async void Message(string content,string title) {
			await new MessageDialog(content,title).ShowAsync();
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="uics">(<see cref="IList{UICommand}"/>)追加したいコマンド</param>
		/// <returns>(<see cref="int"/>)ユーザーが選択したボタンのコマンドインデックス</returns>
		public static IAsyncOperation<int> MessageAsync(string content,IList<UICommand> uics) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await MessageAsync(content,"",uics,0);
				});
			});
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="uics">(<see cref="IList{UICommand}"/>)追加したいコマンド</param>
		/// <param name="cmdindex">(<see cref="uint"/>)デフォルトのコマンドインデックス</param>
		/// <returns>(<see cref="int"/>)ユーザーが選択したボタンのコマンドインデックス</returns>
		public static IAsyncOperation<int> MessageAsync(string content,IList<UICommand> uics,uint cmdindex) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await MessageAsync(content,"",uics,cmdindex);
				});
			});
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="title">(<see cref="string"/>)表示したいタイトル</param>
		/// <param name="commands">(<see cref="string"/>)ボタンのテキスト</param>
		/// <returns>(<see cref="int"/>)選択されたボタン(左から0,1,2...)</returns>
		public static IAsyncOperation<int> MessageAsync(string content,string title,params string[] commands) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					List<UICommand> uics = new List<UICommand>();
					foreach(var item in commands) {
						uics.Add(new UICommand(item));
					}
					return await MessageAsync(content,title,uics,0);
				});
			});
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="title">(<see cref="string"/>)表示したいタイトル</param>
		/// <param name="uics">(<see cref="IList{UICommand}"/>)追加したいコマンド</param>
		/// <returns>(<see cref="int"/>)選択されたボタン(左から0,1,2...)</returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static IAsyncOperation<int> MessageAsync(string content,string title,IList<UICommand> uics) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await MessageAsync(content,title,uics,0);
				});
			});
		}

		/// <summary>
		/// 画面の上にメッセージを表示します。
		/// </summary>
		/// <param name="content">(<see cref="string"/>)表示したいテキスト</param>
		/// <param name="title">(<see cref="string"/>)表示したいタイトル</param>
		/// <param name="uics">(<see cref="IList{UICommand}"/>)追加したいコマンド</param>
		/// <param name="cmdindex">(<see cref="uint"/>)デフォルトのコマンドインデックス</param>
		/// <returns>(<see cref="int"/>)ユーザーが選択したボタンのコマンドインデックス</returns>
		public static IAsyncOperation<int> MessageAsync(string content,string title,IList<UICommand> uics,uint cmdindex) {
			if(rootPage == null) {
				if(ApplicationSetting.rootPage != null) {
					rootPage = ApplicationSetting.rootPage;
				}
				else {
					throw new NullReferenceException("Tools.Screens.rootPage に 表示するページ(通常は this)を入れてください。");
				}
			}
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					IUICommand result = null;
					try {
						await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () => {
							MessageDialog md = new MessageDialog(content,title);
							int i = 0;
							foreach(var item in uics) {
								if(item.Id == null) item.Id = i;
								i++;
								md.Commands.Add(item);
								if(i > 2) {
									break;
								}
								else if(DeviceSetting.isMobile && i > 1) {
									break;
								}
							}
							md.DefaultCommandIndex = cmdindex;
							result = await md.ShowAsync();
						});
						while(result?.Id == null || result == null) {
							Task.WaitAll(Task.Delay(1000));
						}
					}
					catch(Exception ex) {
						Message(ex.Message,"エラー");
						return -1;
					}
					return (int)result.Id;
				});
			});
		}

#if WINDOWS_UWP
		/// <summary>
		/// UWPのタイトルバーを変更します
		/// </summary>
		/// <param name="title">(<see cref="string"/>)タイトルバーの文字</param>
		public static void TitleBarChange(string title) {
			if(title != "") {
				ApplicationView.GetForCurrentView().Title = title;
			}
		}

		/// <summary>
		/// UWPのタイトルバーを変更します
		/// </summary>
		/// <param name="backColor">(<see cref="Color"/>)ウィンドウがアクティブの時の背景色</param>
		/// <param name="foreColor">(<see cref="Color"/>)ウィンドウがアクティブの時の文字色</param>
		/// <param name="title">(<see cref="string"/>)タイトルバーの文字</param>
		public static void TitleBarChange(Color backColor,Color foreColor,params string[] title) {
			var titlebar = ApplicationView.GetForCurrentView().TitleBar;
			titlebar.BackgroundColor = titlebar.InactiveForegroundColor
				= titlebar.ButtonInactiveForegroundColor = titlebar.ButtonBackgroundColor = backColor;
			titlebar.ForegroundColor = titlebar.InactiveBackgroundColor
				= titlebar.ButtonInactiveBackgroundColor = titlebar.ButtonForegroundColor = foreColor;
			if(title.Length > 0) {
				TitleBarChange(title[0]);
			}

		}

		/// <summary>
		/// UWPのタイトルバーを変更します
		/// </summary>
		/// <param name="backColor">(<see cref="Color"/>)ウィンドウがアクティブの時の背景色</param>
		/// <param name="foreColor">(<see cref="Color"/>)ウィンドウがアクティブの時の文字色</param>
		/// <param name="back_backColor">(<see cref="Color"/>)ウィンドウが非アクティブの時の背景色</param>
		/// <param name="back_foreColor">(<see cref="Color"/>)ウィンドウが非アクティブの時の文字色</param>
		/// <param name="title">(<see cref="string"/>)タイトルバーの文字</param>
		public static void TitleBarChange(Color backColor,Color foreColor,Color back_backColor,Color back_foreColor,params string[] title) {
			var titlebar = ApplicationView.GetForCurrentView().TitleBar;
			titlebar.BackgroundColor = titlebar.ButtonBackgroundColor = backColor;
			titlebar.ForegroundColor = titlebar.ButtonForegroundColor = foreColor;
			titlebar.InactiveForegroundColor = titlebar.ButtonInactiveForegroundColor = back_foreColor;
			titlebar.InactiveBackgroundColor = titlebar.ButtonInactiveBackgroundColor = back_backColor;
			if(title.Length > 0) {
				TitleBarChange(title[0]);
			}
		}
	}
#endif
}
