using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed partial class ColorPicker:UserControl {
		/// <summary>
		/// 
		/// </summary>
		public ColorPicker() {
			InitializeComponent();
		}

		RowDefinition rd = new RowDefinition();
		Brush _brush = new SolidColorBrush();

		private void Rectangle_Tapped(object sender,TappedRoutedEventArgs e) {
			_brush = (((Rectangle)sender).Fill);
		}

		/// <summary>
		/// ColorPickerから色を取得します
		/// </summary>
		/// <returns>(<see cref="Brush"/>)</returns>
		public IAsyncOperation<Brush> getBrush() {
			return AsyncInfo.Run((token) => {
				return Task.Run(async () => {
					_brush = null;
					await Task.Run(async () => {
						while(_brush == null) {
							await Task.Delay(1000);
						}
					});
					return _brush;
				});
			});
		}

		/// <summary>
		/// <see cref="Brush"/> の32ビットの ARGB 値を取得します。
		/// </summary>
		/// <param name="brush"><see cref="Brush"/></param>
		/// <returns>(<see cref="int"/>)32ビット符号あり整数</returns>
		public static int ToArgb(Brush brush) {
			Color c = (brush as SolidColorBrush).Color;
			return int.Parse(c.ToString().Replace("#",""),NumberStyles.AllowHexSpecifier);
		}

		/// <summary>
		/// <see cref="Color"/> 構造体の32ビットの ARGB 値を取得します。
		/// </summary>
		/// <param name="color"><see cref="Color"/> 構造体</param>
		/// <returns>(<see cref="int"/>)32ビット符号あり整数</returns>
		[Windows.Foundation.Metadata.DefaultOverload()]
		public static int ToArgb(Color color) {
			return int.Parse(color.ToString().Replace("#",""),NumberStyles.AllowHexSpecifier);
		}

		/// <summary>
		/// 32ビットの ARGB 値から <see cref="Color"/> 構造体を作成します
		/// </summary>
		/// <param name="argb">32ビットの ARGB 値</param>
		/// <returns><see cref="Color"/> 構造体</returns>
		public static SolidColorBrush FromArgb(int argb) {
			byte[] b = new byte[4];
			var hex = argb.ToString("X8");
			b[0] = byte.Parse($"{hex[0].ToString()}{hex[1].ToString()}",NumberStyles.HexNumber);
			b[1] = byte.Parse($"{hex[2].ToString()}{hex[3].ToString()}",NumberStyles.HexNumber);
			b[2] = byte.Parse($"{hex[4].ToString()}{hex[5].ToString()}",NumberStyles.HexNumber);
			b[3] = byte.Parse($"{hex[6].ToString()}{hex[7].ToString()}",NumberStyles.HexNumber);
			return new SolidColorBrush(Color.FromArgb(b[0],b[1],b[2],b[3]));
		}

		/// <summary>
		/// 8ビットの ARGB 値から <see cref="SolidColorBrush"/> 構造体を作成します
		/// </summary>
		/// <param name="a">透明度</param>
		/// <param name="r">赤色</param>
		/// <param name="g">緑色</param>
		/// <param name="b">青色</param>
		/// <returns></returns>
		public static SolidColorBrush FromArgb(byte a,byte r,byte g,byte b) {
			return new SolidColorBrush(Color.FromArgb(a,r,g,b));
		}

		/// <summary>
		/// <see cref="Brush"/> から <see cref="Color"/> 構造体を作成します。
		/// </summary>
		/// <param name="brush"><see cref="Brush"/></param>
		/// <returns><see cref="Color"/> 構造体</returns>
		public static Color FromBrush(Brush brush) {
			return (brush as SolidColorBrush).Color;
		}

		static List<ColorName> colornames = (List<ColorName>)(new ColorNames().Items);

		/// <summary>
		/// <see cref="Brush"/> から 色の名前を取得します。
		/// </summary>
		/// <param name="brush"><see cref="Brush"/></param>
		/// <returns>色の名前</returns>
		public static string brush2name(Brush brush) {
			Color cl = FromBrush(brush);
			return colornames.Find(x => x.color == cl)?.Name ?? "";
		}

		/// <summary>
		/// 定義済みの色の指定した名前から <see cref="Color"/> 構造体を作成します。
		/// </summary>
		/// <param name="name"><see cref="string"/> 定義済みの色の名前を示す文字列。有効な名前は <see cref="Colors"/> 列挙体の要素と同じです。</param>
		/// <returns><see cref="Color"/> 構造体</returns>
		public static Color FromName(string name) {
			return colornames.Find(x => x.Name == name).color;
		}

		/// <summary>
		/// ユーザーが設定で選択しているアクセントカラーを取得します
		/// </summary>
		/// <returns><see cref="SolidColorBrush"/></returns>
		public static SolidColorBrush getUserColor() {
			return Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
		}

		/// <summary>
		/// システムカラーリソースの色を取得します。
		/// </summary>
		/// <param name="colorname"><see cref="string"/> 取得したいシステムカラーの名前</param>
		/// <returns></returns>
		public static SolidColorBrush getResourceColor(string colorname) {
			SolidColorBrush brush = new SolidColorBrush();
			try {
				brush = Application.Current.Resources[colorname] as SolidColorBrush;
			}
			catch(Exception ex) {
				throw new Exception("指定された色がありません。",ex);
			}
			return brush;
		}

		/// <summary>
		/// フォントサイズをまとめてintリストとして返します
		/// </summary>
		/// <returns><see cref="IReadOnlyList{Int32}"/></returns>
		public static IReadOnlyList<int> getFontSize() {
			return fontSize;
		}

		static int[] fontSize = {
			6,7,8,9,10,11,12,14,16,18,20,22,24,36,48,72,96
		};

		/// <summary>
		/// フォントの名前をstringリストでまとめて返します
		/// </summary>
		/// <returns><see cref="IReadOnlyList{String}"/></returns>
		public static IReadOnlyList<string> getFontNames() {
			return FontNames;
		}

		static string[] FontNames = {
			"Arial", "Calibri", "Cambria", "Cambria Math", "Comic Sans MS", "Courier New",
			"Ebrima", "Gadugi", "Georgia",
			"Javanese Text Regular Fallback font for Javanese script", "Leelawadee UI",
			"Lucida Console", "Malgun Gothic", "Microsoft Himalaya", "Microsoft JhengHei",
			"Microsoft JhengHei UI", "Microsoft New Tai Lue", "Microsoft PhagsPa",
			"Microsoft Tai Le", "Microsoft YaHei", "Microsoft YaHei UI",
			"Microsoft Yi Baiti", "Mongolian Baiti", "MV Boli", "Myanmar Text",
			"Nirmala UI", "Segoe MDL2 Assets", "Segoe Print", "Segoe UI", "Segoe UI Emoji",
			"Segoe UI Historic", "Segoe UI Symbol", "SimSun", "Times New Roman",
			"Trebuchet MS", "Verdana", "Webdings", "Wingdings", "Yu Gothic",
			"Yu Gothic UI"
		};

	}
}
