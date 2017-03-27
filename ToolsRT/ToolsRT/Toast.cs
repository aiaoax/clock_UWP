using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class Toast {


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1) {
			return Create(ttt,text1,"","",null,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,Uri uri) {
			return Create(ttt,text1,"","",uri,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,IReadOnlyList<ToastAction> tas) {
			return Create(ttt,text1,"","",null,tas);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2) {
			return Create(ttt,text1,text2,"",null,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,Uri uri,IReadOnlyList<ToastAction> tas) {
			return Create(ttt,text1,"","",uri,tas);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,IReadOnlyList<ToastAction> tas) {
			return Create(ttt,text1,text2,"",null,tas);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,Uri uri) {
			return Create(ttt,text1,text2,"",uri,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,string text3) {
			return Create(ttt,text1,text2,text3,null,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,Uri uri,IReadOnlyList<ToastAction> tas) {
			return Create(ttt,text1,text2,"",uri,tas);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,string text3,IReadOnlyList<ToastAction> tas) {
			return Create(ttt,text1,text2,text3,null,tas);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,string text3,Uri uri) {
			return Create(ttt,text1,text2,text3,uri,null);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static ToastNotification Create(ToastTemplateType ttt,string text1,string text2,string text3,Uri uri,IReadOnlyList<ToastAction> tas) {
			ToastNotification tn = null;
			try {
				XmlDocument doc = new XmlDocument();
				string xml = $@"<toast>
	<visual>
		<binding template=""{ttt.ToString()}"">" + Environment.NewLine;
				if(uri != null) {
					xml += $@"			<image id=""1"" src=""{uri?.AbsoluteUri}"" alt=""image1""/>" + Environment.NewLine;
				}
				xml += $@"			<text id=""1"">{text1}</text>
			<text id=""2"">{text2}</text>
			<text id=""3"">{text3}</text>
		</binding>
	</visual>" + Environment.NewLine;
				if(tas?.Count > 0) {
					xml += @"	<actions>" + Environment.NewLine;
					for(int i = 0;i < tas.Count;i++) {
						if(i > 4) break;
						xml += $@"		<action activationType=""{tas[i].activationType}"" content=""{tas[i].content}"" arguments=""{tas[i].arguments}""/>" + Environment.NewLine;
					}
					xml += @"	</actions>" + Environment.NewLine;
				}
				xml += "</toast>";
				Debug.WriteLine(xml);
				doc.LoadXml(xml);

				tn = new ToastNotification(doc);
			}
			catch(Exception ex) {
				Debug.Fail("Toast.Create",ex.Message);
			}
			return tn;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,"","",null,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,Uri uri) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,"","",uri,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,"","",null,tas));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static void Show(ToastTemplateType ttt,string text1,string text2) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,"",null,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,Uri uri,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,"","",uri,tas));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,string text2,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,"",null,tas));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,string text2,Uri uri) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,"",uri,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static void Show(ToastTemplateType ttt,string text1,string text2,string text3) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,text3,null,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,string text2,Uri uri,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,"",uri,tas));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,string text2,string text3,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,text3,null,tas));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		[DefaultOverload()]
		public static void Show(ToastTemplateType ttt,string text1,string text2,string text3,Uri uri) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,text3,uri,null));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ttt"></param>
		/// <param name="text1"></param>
		/// <param name="text2"></param>
		/// <param name="text3"></param>
		/// <param name="uri"></param>
		/// <param name="tas"></param>
		/// <returns></returns>
		public static void Show(ToastTemplateType ttt,string text1,string text2,string text3,Uri uri,IReadOnlyList<ToastAction> tas) {
			ToastNotificationManager.CreateToastNotifier().Show(Create(ttt,text1,text2,text3,uri,tas));
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ToastActivationType {
		/// <summary>
		/// 
		/// </summary>
		foreground,

		/// <summary>
		/// 
		/// </summary>
		background,

		/// <summary>
		/// 
		/// </summary>
		protocol,

		/// <summary>
		/// 
		/// </summary>
		system
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ToastAction {



		/// <summary>
		/// 
		/// </summary>
		public ToastAction() {

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="arguments"></param>
		public ToastAction(string content,string arguments) {
			activationType = ToastActivationType.foreground;
			this.content = content;
			this.arguments = arguments;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="activationType"></param>
		/// <param name="content"></param>
		/// <param name="arguments"></param>
		
		public ToastAction(ToastActivationType activationType,string content,string arguments) {
			this.activationType = activationType;
			this.content = content;
			this.arguments = arguments;
		}

		/// <summary>
		/// 
		/// </summary>
		public ToastActivationType activationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string content { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string arguments { get; set; }

	}
}
