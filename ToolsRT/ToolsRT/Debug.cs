using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools {
	/// <summary>
	/// デバッグ用メッセージを表示します
	/// </summary>
	public sealed class Debug {

		/// <summary>
		/// デバッグメッセージを表示します
		/// </summary>
		/// <param name="msg"><see cref="string"/>表示するメッセージ</param>
		public static void WriteLine(object msg) {
			System.Diagnostics.Debug.WriteLine(msg);
		}

		/// <summary>
		/// デバッグメッセージを表示します
		/// </summary>
		/// <param name="tag"><see cref="string"/>タグ</param>
		/// <param name="msg"><see cref="string"/>表示するメッセージ</param>
		public static void WriteLine(string tag,object msg) {
			System.Diagnostics.Debug.WriteLine($"{DateTime.Now} D/{tag}: {msg}");
		}

		/// <summary>
		/// エラーメッセージを表示します
		/// </summary>
		/// <param name="msg"><see cref="string"/>表示するメッセージ</param>
		public static void Fail(object msg) {
			Fail("",msg);
		}

		/// <summary>
		/// エラーメッセージを表示します
		/// </summary>
		/// <param name="tag"><see cref="string"/>タグ</param>
		/// <param name="msg"><see cref="string"/>表示するメッセージ</param>
		public static void Fail(string tag,object msg) {
			System.Diagnostics.Debug.Fail($"{DateTime.Now} F/{tag}: {msg}");
		}

	}
}
