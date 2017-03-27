using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class LyricData {

		/// <summary>
		/// 
		/// </summary>
		public static Page rootPage { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public LyricData() {
			time = new TimeSpan(0,0,0,0,1);
			lyric = yomi = "";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_time"></param>
		/// <param name="_lyric"></param>
		public LyricData(TimeSpan _time,string _lyric) {
			time = _time;
			lyric = _lyric.Trim();
			yomi = getYomiAsync(lyric).GetResults().Trim();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_time"></param>
		/// <param name="_lyric"></param>
		/// <param name="_yomi"></param>
		public LyricData(TimeSpan _time,string _lyric,string _yomi) {
			time = _time;
			lyric = _lyric.Trim();
			yomi = _yomi == "" ? getYomiAsync(lyric).GetResults().Trim() : _yomi;
		}

		/// <summary>
		/// 
		/// </summary>
		public TimeSpan time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string lyric { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string yomi { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Color color { get; set; } = Colors.Black;

		/// <summary>
		/// 
		/// </summary>
		public Color back { get; set; } = Colors.White;

		static Color ControlText = Colors.Black;
		static Color ControlLightLight = Colors.White;

		/// <summary>
		/// 
		/// </summary>
		public FontFamily font { get; set; } = new FontFamily("Yu Gothic UI");

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return lyric;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lyrictxt"></param>
		/// <param name="yomi"></param>
		/// <param name="print"></param>
		/// <returns></returns>
		public static IAsyncOperation<IReadOnlyList<LyricData>> ToListAsync(string lyrictxt,string yomi,bool print) {
			if(rootPage == null) {
				if(ApplicationSetting.rootPage != null) {
					rootPage = ApplicationSetting.rootPage;
				}
				else {
					throw new NullReferenceException("Tools.LyricData.rootPage に 表示するページ(通常は this)を入れてください。");
				}
			}
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					List<LyricData> lyrs = null;
					await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () => {
						List<LyricData> temp = new List<LyricData>();
						string st = "";
						try {
							if(yomi == "") {
								print = false;
							}
							try {
								bool b = false;
								var txts = lyrictxt.Split('\n');
								string[] yomis;
								if(yomi != "") {
									yomis = yomi.Split('\n');
									b = true;
								}
								else {
									yomis = new string[1];
								}
								int i = -1;
								FontFamily ff = null;
								try {
									ff = new FontFamily("Yu Gothic UI");
								}
								catch(Exception ex) {
									Debug.Fail("LyricData.ToListAsync",ex.Message);
								}

								Color c = ControlLightLight, backc = ControlText;
								foreach(var item in txts) {
									try {
										i++;

										LyricData ly = new LyricData();

										var lydata = item.Split(']');
										lydata[0] = lydata[0].Replace("[","");
										if(lydata.Length > 1) {
											lydata[1] = lydata[1].Replace("\r","");
											lydata[1] = lydata[1].Replace("　"," ");
											lydata[1] = zenkaku2hankaku(lydata[1]);
											ly.lyric = lydata[1];

											if(b) {
												yomis[i] = yomis[i].Replace("\r","");
												if(yomis[i] != "") {
													var str = yomis[i];
													if(str.IndexOf("#") >= 0) {
														var strs = str.Split('#');
														int ix = -1;
														foreach(var cmds in strs) {
															ix++;
															if(ix < 1) {
																ly.yomi = cmds;
																continue;
															}
															var cmd = cmds.Split(':');
															switch(cmd[0].ToLower()) {
																case "color":
																	try {
																		var args = cmd[1].Split(',');
																		if(args.Length != 3) {
																			c = ControlLightLight;
																		}
																		else {
																			c = Color.FromArgb(255,byte.Parse(args[0]),byte.Parse(args[1]),byte.Parse(args[2]));
																		}
																	}
																	catch {
																		var err = "";
																		foreach(var txt in strs) {
																			err += $"{txt} ";
																		}
																		Debug.WriteLine(err);
																	}

																	break;

																case "back":
																	try {
																		var args = cmd[1].Split(',');
																		if(args.Length != 3) {
																			backc = ControlLightLight;
																		}
																		else {
																			backc = Color.FromArgb(255,byte.Parse(args[0]),byte.Parse(args[1]),byte.Parse(args[2]));
																		}
																	}
																	catch {
																		var err = "";
																		foreach(var txt in strs) {
																			err += $"{txt} ";
																		}
																		Debug.WriteLine(err);
																	}

																	break;
																case "font":
																	try {
																		if(cmd[1].Trim() != "")
																			ff = new FontFamily(cmd[1].Trim());
																	}
																	catch {
																		var err = "";
																		foreach(var txt in strs) {
																			err += $"{txt} ";
																		}
																		Debug.WriteLine(err);
																	}
																	break;
																default:
																	break;
															}

														}
													}
													else {
														ly.yomi = str;
													}
													ly.back = backc;
													ly.color = c;
													ly.font = ff;
												}
												else {
													ly.yomi = await getYomiAsync(ly.lyric);
													ly.back = backc;
													ly.color = c;
													ly.font = ff;
												}
											}
											else {
												ly.yomi = await getYomiAsync(ly.lyric);
												ly.back = backc;
												ly.color = c;
												ly.font = ff;
											}
										}
										else {
											ly.lyric = ly.yomi = "";
										}
										var times = lydata[0].Split(':');
										string[] sec = new string[2];
										if(times[1].IndexOf('.') > 0)
											sec = times[1].Split('.');
										else if(times[1].IndexOf(':') > 0)
											sec = times[1].Split(':');
										else
											sec = new string[] { "0","0" };
										ly.time = new TimeSpan(0,0,int.Parse(times[0]),int.Parse(sec[0]),int.Parse(sec[1]) * 10);
										temp.Add(ly);
										if(print) {

											st += ly.yomi + "\r\n";

										}
									}
									catch(Exception ex) {
										Debug.Fail("LyricData.ToListAsync",ex.Message);
										Debug.WriteLine($"{temp.Count + 1}行目");
									}
								}

							}
							catch(Exception ex) {
								Debug.Fail("LyricData.ToListAsync",ex.Message);
								Debug.WriteLine($"{temp.Count + 1}行目");
								temp = new List<LyricData>(1);
							}

						}
						catch(Exception ex) {
							Debug.Fail("LyricData.ToListAsync",ex.Message);
							Debug.WriteLine($"{temp.Count + 1}行目");
							temp = new List<LyricData>(1);
						}
						lyrs = temp;
					});
					while(lyrs == null) {
						Task.WaitAll(Task.Delay(100));
					}
					return (IReadOnlyList<LyricData>)lyrs;
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="musicfile"></param>
		/// <returns></returns>
		public static IAsyncOperation<IReadOnlyList<LyricData>> ToListAsync(StorageFile musicfile) {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					List<LyricData> lyrs = new List<LyricData>();
					var ly_path = musicfile.Path;
					string lrc = "", yomi = "";
					
					//歌詞ファイルがあるか確認
					ly_path = Regex.Replace(ly_path,@"\.[a-z0-9]{3,4}$",".lrc");
					var yomifile = ly_path + "x";
					try {

						var ly_sf = await StorageFile.GetFileFromPathAsync(ly_path);
						lrc = await Text.ReadFileAsync(ly_sf);
						try {
							var yomi_sf = await StorageFile.GetFileFromPathAsync(yomifile);
							yomi = await Text.ReadFileAsync(yomi_sf);
						}
						catch(Exception ex) {
							Debug.Fail("LyricData.ToListAsync",ex.Message);
							yomi = "";
						}

					}
					catch { }

					return await ToListAsync(lrc,yomi,false);
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lds"></param>
		/// <returns></returns>
		public static IAsyncOperation<string> ToTextAsync(IReadOnlyList<LyricData> lds) {
			return AsyncInfo.Run((token) => {
				return Task.Run(() => {
					string ret = "";
					string tmp = "";
					foreach(var item in lds) {
						tmp += $"[{item.time.ToString(@"mm\:ss\.ff")}]{item.lyric}";
						ret += tmp;
						tmp = Environment.NewLine;
					}
					return ret;
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lds"></param>
		/// <returns></returns>
		public static IAsyncOperation<string> ToText_yomiAsync(IReadOnlyList<LyricData> lds) {
			return AsyncInfo.Run((token) => {
				return Task.Run(() => {
					string ret = "", tmp = "";
					FontFamily ff = new FontFamily("Yu Gothic UI");
					Color c = ControlLightLight;
					foreach(var item in lds) {
						tmp += item.yomi;
						if(ff != item.font) {
							ff = item.font;
							tmp += $"#font:{ff.Source}";
						}
						if(c != item.color) {
							c = item.color;
							if(c == ControlLightLight) {
								tmp += $"#color:0,0,0,0";
							}
							else {
								tmp += $"#color:{c.R},{c.G},{c.B}";
							}
						}
						ret += tmp;
						tmp = Environment.NewLine;
					}
					return ret;
				});
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		static IAsyncOperation<string> getYomiAsync(string text) {
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
					string ret = "";
					await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() => {
						text = text.Replace("♪","").Replace("☆","").Replace("★","").Replace("☆彡","").Replace("♡","").Trim();
						var list = JapanesePhoneticAnalyzer.GetWords(text);

						foreach(var item in list) {
							ret += item.YomiText + " ";
						}

						ret = zenkaku2hankaku(ret).Trim();
						ret = ret.Replace("　"," ");
						ret = Regex.Replace(ret,@"  +"," ");
					});
					return ret;
				});
			});

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		static string zenkaku2hankaku(string text) {
			Dictionary<char,char> conv = new Dictionary<char,char>() {
				{ '１','1'},{'２','2'},{'３','3'},{'４','4'},{'５','5'},
				{ '６','6'},{'７','7'},{'８','8'},{'９','9'},{'０','0'},
				{'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
				{'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
				{'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
				{'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
				{'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
				{'Ｚ','Z'},
				{'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
				{'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
				{'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
				{'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
				{'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
				{'ｚ','z'},
				{'　',' '},{'？','?' },{'！','!' },{'．','.' },{'，',',' },
				{'’','\'' },{'”','"' },{'￥','\\' }
			};
			text = text.Replace(" っ","っ").Replace(" ゃ","ゃ").Replace(" ゅ","ゅ").Replace(" ょ","ょ").Replace("・・・","…");
			text = text.Replace(" ～ ","～").Replace(" ・ ","・").Replace("だ ","だ").Replace("み ","み").Replace("し ","し").Replace(" ん","ん").Replace(" る","る");
			return new string(text.Select(n => (conv.ContainsKey(n) ? conv[n] : n)).ToArray());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="musicpath"></param>
		/// <returns></returns>
		public static string getKasiFilePath(string musicpath) {
			return Regex.Replace(musicpath,@"\.[a-z0-9]{3,4}$",".lrc");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="musicpath"></param>
		/// <returns></returns>
		public static string getYomiFilePath(string musicpath) {
			return Regex.Replace(musicpath,@"\.[a-z0-9]{3,4}$",".lrcx");
		}

	}

}
