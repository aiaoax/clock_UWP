using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tools {
	/// <summary>
	/// 
	/// </summary>
	public sealed class Bool2VisibilityConverter:IValueConverter {
		/// <summary>
		/// 
		/// </summary>
		public bool IsReversed { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value,Type targetType,object parameter,string language) {
			var val = System.Convert.ToBoolean(value);
			if(IsReversed) {
				val = !val;
			}
			if(val) {
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value,Type targetType,object parameter,string language) {
			return false;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class NotBool2VisibilityConverter:IValueConverter {
		/// <summary>
		/// 
		/// </summary>
		public bool IsReversed { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value,Type targetType,object parameter,string language) {
			var val = System.Convert.ToBoolean(value);
			if(IsReversed) {
				val = !val;
			}
			if(val) {
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value,Type targetType,object parameter,string language) {
			return false;
		}
	}
}
