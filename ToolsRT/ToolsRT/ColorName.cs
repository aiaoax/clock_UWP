using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Tools {

	/// <summary>
	/// 
	/// </summary>
	public sealed class ColorName {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="_color"></param>
		public ColorName(string _name,Color _color) {
			Name = _name;
			color = _color;
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Color color { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ColorNames:IList<ColorName> {
		/// <summary>
		/// 
		/// </summary>
		IList<ColorName> cls = new List<ColorName>();
		
		/// <summary>
		/// 
		/// </summary>
		public ColorNames() {
			foreach(var item in typeof(Colors).GetRuntimeProperties()) {
				cls.Add(new ColorName(item.Name,(Color)item.GetValue(null)));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ColorName this[int index] {
			get {
				return cls[index];
			}

			set {
				cls[index] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count {
			get {
				return cls.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly {
			get {
				return cls.IsReadOnly;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Add(ColorName item) {
			cls.Add(item);
		}

		/// <summary>
		/// 
		/// </summary>
		public IList<ColorName> Items {
			get { return cls; }
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear() {
			cls.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(ColorName item) {
			return cls.Contains(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(ColorName[] array,int arrayIndex) {
			cls.CopyTo(array,arrayIndex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ColorName> GetEnumerator() {
			return cls.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(ColorName item) {
			return cls.IndexOf(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index,ColorName item) {
			cls.Insert(index,item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(ColorName item) {
			return cls.Remove(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) {
			cls.RemoveAt(index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return cls.GetEnumerator();
		}
	}
}
