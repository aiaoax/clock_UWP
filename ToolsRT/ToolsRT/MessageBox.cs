using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class MessageBox {

		/// <summary>
		/// <see cref="MessageBox"/> を使用する前に表示するページを入れてください。(通常は this)
		/// </summary>
		/// <example> 
		/// このコードを <see cref="MessageBox"/> を表示する前に入れてください。
		/// <code>
		/// MessageBox.rootPage = this;
		/// </code>
		/// </example>
		public static Page rootPage { get; set; }

		/// <summary>
		/// 指定したテキストを表示するメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <returns><see cref="DialogResult"/> 値のいずれか 1 つ。</returns>
		public static IAsyncOperation<DialogResult> ShowAsync(string text) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await ShowAsync(text,"",MessageBoxButtons.OK,MessageBoxIcon.None);
				});
			});

		}

		/// <summary>
		/// 指定したテキストとキャプションを表示するメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="caption">メッセージ ボックスのタイトル バーに表示するテキスト。</param>
		/// <returns><see cref="DialogResult"/> 値のいずれか 1 つ。</returns>
		public static IAsyncOperation<DialogResult> ShowAsync(string text,string caption) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await ShowAsync(text,caption,MessageBoxButtons.OK,MessageBoxIcon.None);
				});
			});
		}

		/// <summary>
		/// 指定したテキストとボタンを表示するメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="buttons">メッセージ ボックスに表示するボタンを指定する <see cref="MessageBoxButtons"/> 値の 1 つ。</param>
		/// <returns><see cref="DialogResult"/> 値のいずれか 1 つ。</returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static IAsyncOperation<DialogResult> ShowAsync(string text,MessageBoxButtons buttons) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await ShowAsync(text,"",buttons,MessageBoxIcon.None);
				});
			});
		}

		/// <summary>
		/// 指定したテキスト、キャプション、およびボタンを表示するメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="caption">メッセージ ボックスのタイトル バーに表示するテキスト。</param>
		/// <param name="buttons">メッセージ ボックスに表示するボタンを指定する <see cref="MessageBoxButtons"/> 値の 1 つ。</param>
		/// <returns><see cref="DialogResult"/> 値のいずれか 1 つ。</returns>
		public static IAsyncOperation<DialogResult> ShowAsync(string text,string caption,MessageBoxButtons buttons) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					return await ShowAsync(text,caption,buttons,MessageBoxIcon.None);
				});
			});
		}

		/// <summary>
		/// 指定したテキスト、キャプション、ボタン、およびアイコンを表示するメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="caption">メッセージ ボックスのタイトル バーに表示するテキスト。</param>
		/// <param name="buttons">メッセージ ボックスに表示するボタンを指定する <see cref="MessageBoxButtons"/> 値の 1 つ。</param>
		/// <param name="icon">メッセージ ボックスに表示するアイコンを指定する <see cref="MessageBoxIcon"/> 値の 1 つ。互換性のために残しているもので実際には意味はありません。</param>
		/// <returns><see cref="DialogResult"/> 値のいずれか 1 つ。</returns>
		public static IAsyncOperation<DialogResult> ShowAsync(string text,string caption,MessageBoxButtons buttons,MessageBoxIcon icon) {
			if(rootPage == null) {

				if(ApplicationSetting.rootPage != null) {
					rootPage = ApplicationSetting.rootPage;
				}
				else {
					throw new NullReferenceException("Tools.MessageBox.rootPage に 表示するページ(通常は this)を入れてください。");
				}
			}
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					DialogResult dr = DialogResult.Null;
					await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () => {
						var msg = new MessageDialog(text,caption);
						switch(buttons) {
							case MessageBoxButtons.OKCancel:
								msg.Commands.Add(new UICommand("OK",null,DialogResult.OK));
								msg.Commands.Add(new UICommand("キャンセル",null,DialogResult.Cancel));
								msg.DefaultCommandIndex = 0;
								msg.CancelCommandIndex = 1;
								break;

							case MessageBoxButtons.AbortRetryIgnore:
								msg.Commands.Add(new UICommand("中止",null,DialogResult.Abort));
								msg.Commands.Add(new UICommand("再試行",null,DialogResult.Retry));
								msg.Commands.Add(new UICommand("無視",null,DialogResult.Ignore));
								msg.DefaultCommandIndex = 1;
								msg.CancelCommandIndex = 0;
								break;

							case MessageBoxButtons.YesNoCancel:
								msg.Commands.Add(new UICommand("はい",null,DialogResult.Yes));
								msg.Commands.Add(new UICommand("いいえ",null,DialogResult.No));
								msg.Commands.Add(new UICommand("キャンセル",null,DialogResult.Cancel));
								msg.DefaultCommandIndex = 0;
								msg.CancelCommandIndex = 2;
								break;

							case MessageBoxButtons.YesNo:
								msg.Commands.Add(new UICommand("はい",null,DialogResult.Yes));
								msg.Commands.Add(new UICommand("いいえ",null,DialogResult.No));
								msg.DefaultCommandIndex = 0;
								msg.CancelCommandIndex = 1;
								break;

							case MessageBoxButtons.RetryCancel:
								msg.Commands.Add(new UICommand("再試行",null,DialogResult.Retry));
								msg.Commands.Add(new UICommand("キャンセル",null,DialogResult.Cancel));
								msg.DefaultCommandIndex = 0;
								msg.CancelCommandIndex = 1;
								break;

							default:
								msg.Commands.Add(new UICommand("OK",null,DialogResult.OK));
								msg.DefaultCommandIndex = 0;
								break;
						}

						var res = await msg.ShowAsync();
						dr = (DialogResult)res.Id;
					});

					while(dr == DialogResult.Null) {
						Task.WaitAll(Task.Delay(1000));
					}
					return dr;
				});

			});
		}

	}

	/// <summary>
	/// ダイアログ結果を取得または設定します。
	/// </summary>
	public enum DialogResult {
		/// <summary>
		/// ダイアログ ボックスの戻り値がない場合は Null です。通常はエラーです。
		/// </summary>
		Null = -1,

		/// <summary>
		/// ダイアログ ボックスから Nothing が返されます。つまり、モーダル ダイアログ ボックスの実行が継続します。
		/// </summary>
		None = 0,

		/// <summary>
		/// ダイアログ ボックスの戻り値は OK です (通常は "OK" というラベルが指定されたボタンから送られます)。
		/// </summary>
		OK = 1,

		/// <summary>
		/// ダイアログ ボックスの戻り値は Cancel です (通常は "キャンセル" というラベルが指定されたボタンから送られます)。
		/// </summary>
		Cancel = 2,

		/// <summary>
		/// ダイアログ ボックスの戻り値は Abort です (通常は "中止" というラベルが指定されたボタンから送られます)。
		/// </summary>
		Abort = 3,

		/// <summary>
		/// ダイアログ ボックスの戻り値は Retry です (通常は "再試行" というラベルが指定されたボタンから送られます)。
		/// </summary>
		Retry = 4,

		/// <summary>
		/// ダイアログ ボックスの戻り値は Ignore です (通常は "無視" というラベルが指定されたボタンから送られます)。
		/// </summary>
		Ignore = 5,

		/// <summary>
		/// ダイアログ ボックスの戻り値は Yes です (通常は "はい" というラベルが指定されたボタンから送られます)。
		/// </summary>
		Yes = 6,

		/// <summary>
		/// ダイアログ ボックスの戻り値は No です (通常は "いいえ" というラベルが指定されたボタンから送られます)。
		/// </summary>
		No = 7
	}

	/// <summary>
	/// <see cref="MessageBox"/> に表示するボタンを定義する定数を指定します。
	/// </summary>
	public enum MessageBoxButtons {
		/// <summary>
		/// メッセージ ボックスに [OK] ボタンを含めます。
		/// </summary>
		OK = 0,

		/// <summary>
		/// メッセージ ボックスに [OK] ボタンと [キャンセル] ボタンを含めます。
		/// </summary>
		OKCancel = 1,

		/// <summary>
		/// メッセージ ボックスに [中止]、[再試行]、および [無視] の各ボタンを含めます。
		/// </summary>
		AbortRetryIgnore = 2,

		/// <summary>
		/// メッセージ ボックスに [はい]、[いいえ]、および [キャンセル] の各ボタンを含めます。
		/// </summary>
		YesNoCancel = 3,

		/// <summary>
		/// メッセージ ボックスに [はい] ボタンと [いいえ] ボタンを含めます。
		/// </summary>
		YesNo = 4,

		/// <summary>
		/// メッセージ ボックスに [再試行] ボタンと [キャンセル] ボタンを含めます。
		/// </summary>
		RetryCancel = 5
	}

	/// <summary>
	/// 表示する情報を定義する定数を指定します。互換性のために残しているもので実際には意味はありません。
	/// </summary>
	public enum MessageBoxIcon {
		/// <summary>
		/// メッセージ ボックスに記号を表示しません。
		/// </summary>
		None = 0,

		/// <summary>
		/// メッセージ ボックスに、背景が赤の円で囲まれた白い X から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Hand = 16,

		/// <summary>
		/// メッセージ ボックスに、背景が赤の円で囲まれた白い X から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Error = 16,

		/// <summary>
		/// メッセージ ボックスに、背景が赤の円で囲まれた白い X から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Stop = 16,

		/// <summary>
		/// メッセージ ボックスに、円で囲まれた疑問符から成る記号を表示します。この疑問符のメッセージ アイコンは、特定の種類のメッセージを明確に表しておらず、メッセージを疑問符で表現すると、どのような種類のメッセージにも当てはまる可能性があるため、現在推奨されていません。
		/// また、ユーザーがメッセージ記号の疑問符をヘルプ情報と混同する可能性があります。そのため、この疑問符のメッセージ記号をメッセージボックスで使用しないでください。この記号の使用は、下位互換性を維持するためにのみサポートされています。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Question = 32,

		/// <summary>
		/// メッセージ ボックスに、背景が黄色の三角形で囲まれた感嘆符から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Exclamation = 48,

		/// <summary>
		/// メッセージ ボックスに、背景が黄色の三角形で囲まれた感嘆符から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Warning = 48,

		/// <summary>
		/// メッセージ ボックスに、円で囲まれた小文字の i から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Asterisk = 64,

		/// <summary>
		/// メッセージ ボックスに、円で囲まれた小文字の i から成る記号を表示します。
		/// 互換性のために残しているもので実際には意味はありません。
		/// </summary>
		Information = 64
	}

}
