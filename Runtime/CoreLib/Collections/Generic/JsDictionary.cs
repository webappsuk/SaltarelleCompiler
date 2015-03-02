// Dictionary.cs
// Script#/Libraries/CoreLib
// This source code is subject to terms and conditions of the Apache License, Version 2.0.
//

using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// The JsDictionary data type which is mapped to the Object type in Javascript.
	/// </summary>
	[Imported]
	[IgnoreNamespace]
	[IncludeGenericArguments(false)]
	[ScriptName("Object")]
	public sealed class JsDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>> {
		[InlineCode("{{}}")]
		public JsDictionary() {}

		public JsDictionary(params object[] nameValuePairs) {}

		public int Count {
			[InlineCode("{$System.Script}.getKeyCount({this})")] get { return 0; }
		}

		public new ICollection<TKey> Keys {
			[InlineCode("{$System.Object}.keys({this})")] get { return null; }
		}

		[IntrinsicProperty]
		public TValue this[TKey key] {
			get { return default(TValue); }
			set { }
		}

		[InlineCode("{$System.Script}.clearKeys({this})")]
		public void Clear() {}

		[InlineCode("{$System.Script}.keyExists({this}, {key})")]
		public bool ContainsKey(TKey key) {
			return false;
		}

		[ScriptSkip]
		public static JsDictionary<TKey, TValue> GetDictionary(object o) {
			return null;
		}

		[InlineCode("new {$System.ObjectEnumerator`2}({this})")]
		public ObjectEnumerator<TKey, TValue> GetEnumerator() {
			return null;
		}

		[InlineCode("delete {this}[{key}]")]
		public void Remove(TKey key) {}

		[InlineCode("new ({$System.Script}.makeGenericType({$System.Collections.Generic.Dictionary`2}, [{TKey}, {TValue}]))({d})")]
		public static explicit operator Dictionary<TKey, TValue>(JsDictionary<TKey, TValue> d) {
			return null;
		}

		[InlineCode("(function(){{var $t1={{}},$t2={d}.getEnumerator(),$t3;try{{while($t2.moveNext()){{$t3=$t2.current();$t1[$t3.key]=$t3.value;}}}}finally{{$t2.dispose();}} return $t1;}})()")]
		public static explicit operator JsDictionary<TKey, TValue>(Dictionary<TKey, TValue> d)
		{
			return null;
		}

		[InlineCode("{this}[{item}.key]={item}.value")]
		public void Add(KeyValuePair<TKey, TValue> item) {}


		[InlineCode("{this}[{key}]={value}")]
		public void Add(TKey key, TValue value) { }

		[InlineCode("{$System.Script}.equals({this}[{item}.key], {item}.value)")]
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
			return false;
		}

		// TODO: If value at key equals supplied value then remove the key
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public class X : IX {
		[InlineCode("{this}.notDo()")]
		public string Do() {
			return "a";
		}
	}

	public interface IX {
		string Do();
	}
}