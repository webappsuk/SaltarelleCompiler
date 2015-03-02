using System;
using System.Collections.Generic;
using QUnit;

namespace CoreLib.TestScript.Collections.Generic {
	[TestFixture]
	public class GenericJsDictionaryTests {
		[Test]
		public void TypePropertiesAreCorrect() {
			Assert.AreEqual(typeof(JsDictionary<string, object>).FullName, "Object");
			Assert.IsTrue(typeof(JsDictionary<string, object>).IsClass);
		}

		[Test]
		public void DefaultConstructorWorks() {
			var d = new JsDictionary<string, object>();
			Assert.IsTrue(d != null);
			Assert.AreEqual(d.Count, 0);
		}

		[Test]
		public void NameValuePairsConstructorWorks() {
			var d = new JsDictionary<string, object>("a", "valueA", "b", 134);
			Assert.AreEqual(d.Count, 2);
			Assert.AreEqual(d["a"], "valueA");
			Assert.AreEqual(d["b"], 134);
		}

		[Test]
		public void KeysWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			var keys = d.Keys;
			Assert.IsTrue(keys.Contains("a"));
			Assert.IsTrue(keys.Contains("b"));
		}

		[Test]
		public void IndexingWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			Assert.AreEqual(d["a"], "valueA");
			Assert.AreEqual(d["b"], 134);
		}

		[Test]
		public void ClearWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			d.Clear();
			Assert.AreEqual(d.Count, 0);
		}

		[Test]
		public void ContainsKeyWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			Assert.IsTrue(d.ContainsKey("a"));
			Assert.IsFalse(d.ContainsKey("c"));
		}

		[Test]
		public void GetDictionaryWorks() {
			var obj = new { a = "valueA", b = 134 };
			var d = JsDictionary<string, object>.GetDictionary(obj);
			Assert.AreStrictEqual(d, obj);
			Assert.AreEqual(2, d.Keys.Count);
			Assert.AreEqual(d["a"], "valueA");
			Assert.AreEqual(d["b"], 134);
		}

		[Test]
		public void GetEnumeratorWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			var d2 = new JsDictionary<string, object>();
			foreach (var kvp in d) {
				d2[kvp.Key] = kvp.Value;
			}
			Assert.AreEqual(d, d2);
		}

		[Test]
		public void RemoveWorks() {
			var d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			d.Remove("a");
			Assert.AreEqual(d.Keys, new[] { "b" });
		}

		[Test]
		public void CastToDictionaryWorks()
		{
			JsDictionary<string, object> d = JsDictionary<string, object>.GetDictionary(new { a = "valueA", b = 134 });
			Dictionary<string, object> d2 = (Dictionary<string, object>) d;
			
			// Check all keys and values copied over ok
			foreach (var key in d.Keys) {
				Assert.AreEqual(d[key], d2[key]);
			}

			// Use a dictionary-specific method as additional confirmation that the conversion went according to plan
			object val;
			Assert.IsFalse(d2.TryGetValue("c", out val));
			Assert.IsNull(val);
		}

		[Test]
		public void CastFromDictionaryWorks()
		{
			Dictionary<string, object> d = new Dictionary<string, object>() {{"a", "valueA"}, {"b", 134}};
			JsDictionary<string, object> d2 = (JsDictionary<string, object>)d;

			// Check all keys and values copied over ok
			foreach (var key in d.Keys)
			{
				Assert.AreEqual(d[key], d2[key]);
			}

			// Verify our jsdictionary only has its own keys
			Assert.AreEqual(Keys(d2), new[] {"a", "b"});
		}

		[Test]
		public void AddKvpWorks()
		{
			var d = new JsDictionary<string, object>();
			d.Add(new KeyValuePair<string, object>("a", 55));
			d.Add(new KeyValuePair<string, object>("b", "valueB"));
			Assert.AreEqual(d["a"], 55);
			Assert.AreEqual(d["b"], "valueB");
		}

		[Test]
		public void AddWorks()
		{
			var d = new JsDictionary<string, object>();
			d.Add("a", 55);
			d.Add("b", "valueB");
			Assert.AreEqual(d["a"], 55);
			Assert.AreEqual(d["b"], "valueB");
		}

		[Test]
		public void CollectionInitializerWorks()
		{
			var d = new JsDictionary<string, object>() {{"a", 55}, {"b", "valueB"}};
			Assert.AreEqual(d["a"], 55);
			Assert.AreEqual(d["b"], "valueB");
		}

		[Test]
		public void ContainsWorks()
		{
			ICollection<KeyValuePair<string,object>> d = new JsDictionary<string, object>() { { "a", 55 }, { "b", "valueB" } };
			Assert.IsTrue(d.Contains(new KeyValuePair<string, object>("b", "valueB")));
			Assert.IsFalse(d.Contains(new KeyValuePair<string, object>("b", "valueA")));
		}

		[Test]
		public void ThisIsBroked()
		{
			IX x = new X();
			x.Do();
			((X) x).Do();
		}
	}
}
