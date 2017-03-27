using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Tools {
	/// <summary>
	/// 音に関するメソッドです。
	/// </summary>
	public sealed class Sounds {

		/// <summary>
		/// <see cref="PlayAsync(string, AudioCategory)"/> を使用する前に表示するページを入れてください。(通常は this)
		/// </summary>
		/// <example> 
		/// このコードを <see cref="PlayAsync(string, AudioCategory)"/> を使用する前に入れてください。
		/// <code>
		/// Tools.Sounds.rootPage = this;
		/// </code>
		/// </example>
		public static Page rootPage { get; set; }

		static Windows.Storage.Streams.IRandomAccessStream stream;
		/// <summary>
		/// 音を再生します。
		/// </summary>
		/// <param name="filename">(<see cref="string"/>)ファイル名</param>
		/// <param name="audiocategory">(<see cref="AudioCategory"/>)オーディオ情報の目的</param>
		public static IAsyncInfo PlayAsync(string filename,AudioCategory audiocategory) {
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
					try {
						await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () => {
							var uri = new Uri("ms-appx:///" + filename,UriKind.Absolute);
							var file = await StorageFile.GetFileFromApplicationUriAsync(uri);

							stream = await file.OpenAsync(FileAccessMode.Read);
							MediaElement me = new MediaElement();
							me.SetSource(stream,file.ContentType);
							me.AutoPlay = false;
							me.AreTransportControlsEnabled = true;
							me.AudioCategory = audiocategory;
							me.Play();

						});

					}
					catch(Exception ex) {
						Debug.Fail("Sounds.PlayAsync",ex.Message);

					}
				});
			});

		}

		/// <summary>
		/// 音声合成でテキストを読み上げます
		/// </summary>
		/// <param name="text">(<see cref="string"/>)読み上げるテキスト</param>
		public static void Speak(string text) {
			MediaElement me1 = new MediaElement();
			Speak(text,me1);
		}

		/// <summary>
		/// 音声合成でテキストを読み上げます
		/// </summary>
		/// <param name="text">(<see cref="string"/>)読み上げるテキスト</param>
		/// <param name="me1">(<see cref="MediaElement"/>)MediaElementを指定</param>
		public static void Speak(string text,MediaElement me1) {
			var voices = SpeechSynthesizer.AllVoices;
			VoiceInformation onsei = voices.FirstOrDefault();
			Speak(text,onsei,me1);
		}

		/// <summary>
		/// 音声合成でテキストを読み上げます
		/// </summary>
		/// <param name="text">(<see cref="string"/>)読み上げるテキスト</param>
		/// <param name="voice">(<see cref="string"/>)声</param>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static void Speak(string text,string voice) {
			MediaElement me1 = new MediaElement();
			Speak(text,voice,me1);
		}

		/// <summary>
		/// 音声合成でテキストを読み上げます
		/// </summary>
		/// <param name="text">(<see cref="string"/>)読み上げるテキスト</param>
		/// <param name="voice">(<see cref="string"/>)声</param>
		/// <param name="me1">(<see cref="MediaElement"/>)MediaElementを指定</param>
		public static void Speak(string text,string voice,MediaElement me1) {
			var voices = SpeechSynthesizer.AllVoices;
			VoiceInformation onsei = voices.FirstOrDefault(v => v.DisplayName.Contains(voice));
			Speak(text,onsei,me1);

		}

		/// <summary>
		/// 音声合成でテキストを読み上げます
		/// </summary>
		/// <param name="text">(<see cref="string"/>)読み上げるテキスト</param>
		/// <param name="onsei">(<see cref="VoiceInformation"/>)VoiceInformationを指定</param>
		/// <param name="me1">(<see cref="MediaElement"/>)MediaElementを指定</param>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static async void Speak(string text,VoiceInformation onsei,MediaElement me1) {
			SpeechSynthesisStream sss;
			using(var ss = new SpeechSynthesizer()) {
				ss.Voice = onsei;
				sss = await ss.SynthesizeTextToStreamAsync(text);
			}
			me1.SetSource(sss,sss.ContentType);
			me1.Play();

		}

		/// <summary>
		/// インストールされている合成音声を(<see cref="string"/>)配列で格納します。
		/// </summary>
		/// <param name="lang">(<see cref="string"/>)絞り込みたい言語。すべての場合、空の文字列を指定してください</param>
		/// <returns><see cref="string"/>インストールされている音声</returns>
		public static string[] ReadVoice(string lang) {
			List<string> str = new List<string>();
			try {
				var allvoices = SpeechSynthesizer.AllVoices;
				str.Clear();
				if(lang != "") {
					var voices = allvoices.Where(v => string.Equals(v.Language,lang,StringComparison.OrdinalIgnoreCase));
					foreach(var item in voices) {
						str.Add(item.DisplayName);
					}
				}
				else {
					foreach(var item in allvoices) {
						str.Add(item.DisplayName);
					}
				}
			}
			catch(Exception ex) {
				Debug.WriteLine("[" + ex.Message + "]");
				str.Clear();
			}
			return str.ToArray();
		}

		/// <summary>
		/// 音声認識を開始し、聞き取った文章を(<see cref="string"/>)で返します。
		/// </summary>
		/// <returns>(<see cref="string"/>)音声認識の結果</returns>
		public static IAsyncOperation<string> ReadSpeechAsync() {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					var speechRecognizer = new SpeechRecognizer();

					await speechRecognizer.CompileConstraintsAsync();

					SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();

					return speechRecognitionResult.Text;
				});
			});


		}
	}
}
