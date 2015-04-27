using System;
using System.Runtime.CompilerServices;
using QUnit;

namespace CoreLib.TestScript.SimpleTypes {
	[TestFixture]
	public class Int64Tests {
		[Test]
		public void TypePropertiesAreCorrect() {
			Assert.IsTrue((object)(long)0 is long);
			Assert.IsFalse((object)0.5 is long);
			Assert.AreEqual(typeof(long).FullName, "ss.Int64");
			Assert.IsFalse(typeof(long).IsClass);
			Assert.IsTrue(typeof(IComparable<long>).IsAssignableFrom(typeof(long)));
			Assert.IsTrue(typeof(IEquatable<long>).IsAssignableFrom(typeof(long)));
			Assert.IsTrue(typeof(IFormattable).IsAssignableFrom(typeof(long)));
			object l = (long)0;
			Assert.IsTrue(l is long);
			Assert.IsTrue(l is IComparable<long>);
			Assert.IsTrue(l is IEquatable<long>);
			Assert.IsTrue(l is IFormattable);

			var interfaces = typeof(long).GetInterfaces();
			Assert.AreEqual(interfaces.Length, 3);
			Assert.IsTrue(interfaces.Contains(typeof(IComparable<long>)));
			Assert.IsTrue(interfaces.Contains(typeof(IEquatable<long>)));
			Assert.IsTrue(interfaces.Contains(typeof(IFormattable)));
		}

		[IncludeGenericArguments]
		private T GetDefaultValue<T>() {
			return default(T);
		}

		[Test]
		public void DefaultValueIs0() {
			Assert.AreStrictEqual(GetDefaultValue<long>(), Int64.Zero);
		}

		[Test]
		public void DefaultConstructorReturnsZero() {
			Assert.AreStrictEqual(new long(), Int64.Zero);
		}

		[Test]
		public void CreatingInstanceReturnsZero() {
			Assert.AreStrictEqual(Activator.CreateInstance<long>(), Int64.Zero);
		}

		[Test]
		public void FormatWorks() {
			Assert.AreEqual(((long) 0x123).Format("x"), "123");
		}

		[Test]
		public void IFormattableToStringWorks() {
			Assert.AreEqual(((long)0x123).ToString("x"), "123");
			Assert.AreEqual(((IFormattable)((long)0x123)).ToString("x"), "123");
		}

		[Test]
		public void LocaleFormatWorks() {
			Assert.AreEqual(((long)0x123).LocaleFormat("x"), "123");
		}

		[Test]
		public void ParseWithoutRadixWorks() {
			Assert.AreEqual(long.Parse("234"), 234);
		}

		[Test]
		public void ParseWithRadixWorks() {
			Assert.AreEqual(long.Parse("234", 16), 0x234);
		}

		[Test]
		public void TryParseWorks() {
			long numberResult;
			bool result = long.TryParse("57574", out numberResult);
			Assert.IsTrue(result);
			Assert.AreEqual(numberResult, 57574);

			result = long.TryParse("-14", out numberResult);
			Assert.IsTrue(result);
			Assert.AreEqual(numberResult, -14);

			result = long.TryParse("", out numberResult);
			Assert.IsFalse(result);
			Assert.AreEqual(numberResult, 0);

			result = long.TryParse(null, out numberResult);
			Assert.IsFalse(result);
			Assert.AreEqual(numberResult, 0);

			result = long.TryParse("notanumber", out numberResult);
			Assert.IsFalse(result);
			Assert.AreEqual(numberResult, 0);

			result = long.TryParse("2.5", out numberResult);
			Assert.IsFalse(result);
			Assert.AreEqual(numberResult, 0);
		}

		[Test]
		public void CastingOfLargeDoublesToInt64Works() {
			double d1 = 5e9 + 0.5, d2 = -d1;
			Assert.AreEqual((long)d1, 5000000000, "Positive");
			Assert.AreEqual((long)d2, -5000000000, "Negative");
		}
		
		[Test]
		public void AdditionOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 3;
			Assert.AreEqual(v1 + v3, 50000000003, "Positive");
			Assert.AreEqual(v2 + v3, -49999999997, "Negative");
		}

		[Test]
		public void AdditionAssignOfLargeInt64Works()
		{
			long v1 = 50000000000L, v3 = 3;
			v1 += v3;
			Assert.AreEqual(v1, 50000000003, "Positive");
		}

		[Test]
		public void SubtractionOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 3;
			Assert.AreEqual(v1 - v3, 49999999997, "Positive");
			Assert.AreEqual(v2 - v3, -50000000003, "Negative");
		}

		[Test]
		public void MultiplicationOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 3;
			Assert.AreEqual(v1 * v3, 150000000000, "Positive");
			Assert.AreEqual(v2 * v3, -150000000000, "Negative");
		}

		[Test]
		public void DivisionOfLargeInt64Works() {
			long v1 = 50000000000L, v2 = -v1, v3 = 3;
			Assert.AreEqual(v1 / v3,  16666666666, "Positive");
			Assert.AreEqual(v2 / v3, -16666666666, "Negative");
		}

		[Test]
		public void ModulusOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 25000000000L;
			Assert.AreEqual(v1 % v3, Int64.Zero, "Positive");
			Assert.AreEqual(v2 % v3, Int64.Zero, "Negative");
		}

		[Test]
		public void AndOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 25000000000L;
			Assert.AreEqual(v1 & v3, 6444101632, "Positive");
			Assert.AreEqual(v2 & v3, 18555897856, "Negative");
		}
		
		[Test]
		public void OrOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 25000000000L;
			Assert.AreEqual(v1 | v3, 68555898368, "Positive");
			Assert.AreEqual(v2 | v3, -43555897856, "Negatie");
		}

		[Test]
		public void XOrOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1, v3 = 25000000000L;
			Assert.AreEqual(v1 ^ v3, 62111796736, "Positive");
			Assert.AreEqual(v2 ^ v3, -62111795712, "Negative");
		}

		[Test]
		public void NotOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(~v1, -50000000001L, "Positive");
			Assert.AreEqual(~v2, 49999999999L, "Negative");
		}
		
		[Test]
		public void LeftShiftOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 << 13, 409600000000000, "Positive");
			Assert.AreEqual(v2 << 13, -409600000000000, "Negative");
		}

		[Test]
		public void RightShiftOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 >> 13, 6103515L, "Positive");
			Assert.AreEqual(v2 >> 13, -6103516L, "Negative");
		}
		
		[Test]
		public void EqualityOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 == 13, false, "Positive");
			Assert.AreEqual(v2 == 13, false, "Negative");
		}

		[Test]
		public void InequalityOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 != 13, true, "Positive");
			Assert.AreEqual(v2 != 13, true, "Negative");
		}
		
		[Test]
		public void LessThanOrEqualOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 <= 13, false, "Positive");
			Assert.AreEqual(v2 <= 13, true, "Negative");
		}

		[Test]
		public void LessThanOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 < 13, false, "Positive");
			Assert.AreEqual(v2 < 13, true, "Negative");
		}
		
		[Test]
		public void GreaterThanOrEqualOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 >= 13, true, "Positive");
			Assert.AreEqual(v2 >= 13, false, "Negative");
		}

		[Test]
		public void GreaterThanOfLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1 > 13, true, "Positive");
			Assert.AreEqual(v2 > 13, false, "Negative");
		}

		[Test]
		public void PreIncrementLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(++v1, 50000000001L, "Positive");
			Assert.AreEqual(++v2, -49999999999L, "Negative");
		}

		[Test]
		public void PreDecrementLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(--v1, 50000000001L, "Positive");
			Assert.AreEqual(--v2, -49999999999L, "Negative");
		}

		[Test]
		public void PostIncrementLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1++, 50000000000L, "Positive");
			Assert.AreEqual(v2++, -50000000000L, "Negative");
		}

		[Test]
		public void PostIncrementAssignLargeInt64Works() {
			long v1 = 50000000000L;
			v1 = v1++;
			Assert.AreEqual(v1, 50000000000L, "Positive");
		}

		[Test]
		public void PostDecrementLargeInt64Works()
		{
			long v1 = 50000000000L, v2 = -v1;
			Assert.AreEqual(v1--, 50000000000L, "Positive");
			Assert.AreEqual(v2--, -50000000000L, "Negative");
		}
		[Test]
		public void ToStringWithoutRadixWorks() {
			int test = 123;
			long test2 = (long)test;
            Assert.AreEqual((test2).ToString(), "123");
			long a = 9007199254740993;
            Assert.AreEqual((a).ToString(), "9007199254740993");
		}

		[Test]
		public void ToStringWithRadixWorks() {
			Assert.AreEqual(((long)123).ToString(10), "123");
			Assert.AreEqual(((long)0x123).ToString(16), "123");
		}
		
		[Test]
		public void DoYouEvenLiftBro() {
			long lol = 5L;
			long? weight = lol;
			var a = ~weight;
			var b = +weight;
			var c = -weight;
			Assert.AreEqual(a, -6L);
			Assert.AreEqual(b, 5L);
			Assert.AreEqual(c, -5L);
		}

		//[Test]
		//public void CompilerNoErrorTests() {
		//	public long P1 { get; set; }
		//	public long P2 { get; set; }
		//	public void M() {
		//	long i = 0;
	
		//	P1 = P2 = i;
		//}

		[Test]
		public void GetHashCodeWorks() {
			Assert.AreEqual   (((long)0).GetHashCode(), ((long)0).GetHashCode());
			Assert.AreEqual   (((long)1).GetHashCode(), ((long)1).GetHashCode());
			Assert.AreNotEqual(((long)0).GetHashCode(), ((long)1).GetHashCode());
			Assert.IsTrue((long)0x100000000L.GetHashCode() <= 0xffffffffL);
		}

		[Test]
		public void EqualsWorks() {
			Assert.IsTrue (((long)0).Equals((object)(long)0));
			Assert.IsFalse(((long)1).Equals((object)(long)0));
			Assert.IsFalse(((long)0).Equals((object)(long)1));
			Assert.IsTrue (((long)1).Equals((object)(long)1));
		}

		[Test]
		public void IEquatableEqualsWorks() {
			Assert.IsTrue (((long)0).Equals((long)0));
			Assert.IsFalse(((long)1).Equals((long)0));
			Assert.IsFalse(((long)0).Equals((long)1));
			Assert.IsTrue (((long)1).Equals((long)1));

			Assert.IsTrue (((IEquatable<long>)((long)0)).Equals((long)0));
			Assert.IsFalse(((IEquatable<long>)((long)1)).Equals((long)0));
			Assert.IsFalse(((IEquatable<long>)((long)0)).Equals((long)1));
			Assert.IsTrue (((IEquatable<long>)((long)1)).Equals((long)1));
		}

		[Test]
		public void CompareToWorks() {
			Assert.IsTrue(((long)0).CompareTo((long)0) == 0);
			Assert.IsTrue(((long)1).CompareTo((long)0) > 0);
			Assert.IsTrue(((long)0).CompareTo((long)1) < 0);
		}

		[Test]
		public void IComparableCompareToWorks() {
			Assert.IsTrue(((IComparable<long>)((long)0)).CompareTo((long)0) == 0);
			Assert.IsTrue(((IComparable<long>)((long)1)).CompareTo((long)0) > 0);
			Assert.IsTrue(((IComparable<long>)((long)0)).CompareTo((long)1) < 0);
		}
	}
}
