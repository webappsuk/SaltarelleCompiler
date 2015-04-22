using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Saltarelle.Compiler.JSModel.Expressions;

namespace Saltarelle.Compiler.Tests {
	public class MockRuntimeLibrary : IRuntimeLibrary {
		private enum TypeContext {
			GenericArgument,
			TypeOf,
			CastTarget,
			GetDefaultValue,
			UseStaticMember,
			BindBaseCall,
		}

		private string GetTypeContextShortName(TypeContext c) {
			switch (c) {
				case TypeContext.GenericArgument: return "ga";
				case TypeContext.TypeOf:          return "to";
				case TypeContext.UseStaticMember: return "sm";
				case TypeContext.CastTarget:      return "ct";
				case TypeContext.GetDefaultValue: return "def";
				case TypeContext.BindBaseCall:    return "bind";
				default: throw new ArgumentException("c");
			}
		}

		public MockRuntimeLibrary() {
			GetTypeOf                                       = (t, c)             => GetScriptType(t, TypeContext.TypeOf, c.ResolveTypeParameter);
			InstantiateType                                 = (t, c)             => GetScriptType(t, TypeContext.UseStaticMember, c.ResolveTypeParameter);
			InstantiateTypeForUseAsTypeArgumentInInlineCode = (t, c)             => GetScriptType(t, TypeContext.GenericArgument, c.ResolveTypeParameter);
			TypeIs                                          = (e, s, t, c)       => JsExpression.Invocation(JsExpression.Identifier("$TypeIs"), e, GetScriptType(t, TypeContext.CastTarget, c.ResolveTypeParameter));
			TryDowncast                                     = (e, s, d, c)       => JsExpression.Invocation(JsExpression.Identifier("$TryCast"), e, GetScriptType(d, TypeContext.CastTarget, c.ResolveTypeParameter));
			Downcast                                        = (e, s, d, c)       => JsExpression.Invocation(JsExpression.Identifier("$Cast"), e, GetScriptType(d, TypeContext.CastTarget, c.ResolveTypeParameter));
			Upcast                                          = (e, s, d, c)       => JsExpression.Invocation(JsExpression.Identifier("$Upcast"), e, GetScriptType(d, TypeContext.CastTarget, c.ResolveTypeParameter));
			ReferenceEquals                                 = (a, b, c)          => JsExpression.Invocation(JsExpression.Identifier("$ReferenceEquals"), a, b);
			ReferenceNotEquals                              = (a, b, c)          => JsExpression.Invocation(JsExpression.Identifier("$ReferenceNotEquals"), a, b);
			InstantiateGenericMethod                        = (m, a, c)          => JsExpression.Invocation(JsExpression.Identifier("$InstantiateGenericMethod"), new[] { m }.Concat(a.Select(x => GetScriptType(x, TypeContext.GenericArgument, c.ResolveTypeParameter))));
			MakeException                                   = (e, c)             => JsExpression.Invocation(JsExpression.Identifier("$MakeException"), e);
			IntegerDivision                                 = (n, d, c)          => JsExpression.Invocation(JsExpression.Identifier("$IntDiv"), n, d);
			UInt64Addition = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Addition"), n, d);
			Int64Addition = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Addition"), n, d);
			UInt64Subtraction = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Subtraction"), n, d);
			Int64Subtraction = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Subtraction"), n, d);
			UInt64Multiplication = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Multiplicatio"), n, d);
			Int64Multiplication = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Multiplication"), n, d);
			UInt64Division = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Division"), n, d);
			Int64Division = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Division"), n, d);
			UInt64Modulus = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Modulus"), n, d);
			Int64Modulus = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Modulus"), n, d);
			UInt64BitwiseAnd = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64BitwiseAnd"), n, d);
			Int64BitwiseAnd = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64BitwiseAnd"), n, d);
			UInt64BitwiseOr = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64BitwiseOr"), n, d);
			Int64BitwiseOr = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64BitwiseOr"), n, d);
			UInt64ExclusiveOr = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ExclusiveOr"), n, d);
			Int64ExclusiveOr = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ExclusiveOr"), n, d);
			UInt64LeftShift = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64LeftShift"), n, d);
			Int64LeftShift = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64LeftShift"), n, d);
			UInt64RightShift = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64RightShift"), n, d);
			Int64RightShift = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64RightShift"), n, d);
			UInt64Equality = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Equality"), n, d);
			Int64Equality = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Equality"), n, d);
			UInt64Inequality = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Inequality"), n, d);
			Int64Inequality = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Inequality"), n, d);
			UInt64LessThanOrEqual = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64LessThanOrEqual"), n, d);
			Int64LessThanOrEqual = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64LessThanOrEqual"), n, d);
			UInt64GreaterThanOrEqual = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64GreaterThanOrEqual"), n, d);
			Int64GreaterThanOrEqual = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64GreaterThanOrEqual"), n, d);
			UInt64LessThan = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64LessThan"), n, d);
			Int64LessThan = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64LessThan"), n, d);
			UInt64GreaterThan = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64GreaterThan"), n, d);
			Int64GreaterThan = (n, d, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64GreaterThan"), n, d);
			UInt64Negation = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Negation"), n);
			Int64Negation = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Negation"), n);
			UInt64OnesComplement = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64OnesComplement"), n);
			Int64OnesComplement = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64OnesComplement"), n);
			UInt64Increment = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Increment"), n);
			Int64Increment = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Increment"), n);
			UInt64Decrement = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64Decrement"), n);
			Int64Decrement = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64Decrement"), n);
			FloatToInt = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$FloatToInt"), n);
			Int64FromUInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromUInt32"), n);
			Int64FromInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromInt32"), n);
			Int64FromUInt64 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromUInt64"), n);
			Int64FromDouble = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromDouble"), n);
			Int64FromSingle = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromSingle"), n);
			Int64FromDecimal = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64FromDecimal"), n);
			Int64ToUInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ToUInt32"), n);
			Int64ToInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ToInt32"), n);
			Int64ToDouble = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ToDouble"), n);
			Int64ToSingle = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ToSingle"), n);
			Int64ToDecimal = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$Int64ToDecimal"), n);
			UInt64FromUInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromUInt32"), n);
			UInt64FromInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromInt32"), n);
			UInt64FromInt64 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromInt64"), n);
			UInt64FromDouble = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromDouble"), n);
			UInt64FromSingle = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromSingle"), n);
			UInt64FromDecimal = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64FromDecimal"), n);
			UInt64ToUInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ToUInt32"), n);
			UInt64ToInt32 = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ToInt32"), n);
			UInt64ToDouble = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ToDouble"), n);
			UInt64ToSingle = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ToSingle"), n);
			UInt64ToDecimal = (n, c) => JsExpression.Invocation(JsExpression.Identifier("$UInt64ToDecimal"), n);
			FloatToInt                                      = (e, c)             => JsExpression.Invocation(JsExpression.Identifier("$Truncate"), e);
			Coalesce                                        = (a, b, c)          => JsExpression.Invocation(JsExpression.Identifier("$Coalesce"), a, b);
			Lift                                            = (e, c)             => JsExpression.Invocation(JsExpression.Identifier("$Lift"), e);
			FromNullable                                    = (e, c)             => JsExpression.Invocation(JsExpression.Identifier("$FromNullable"), e);
			LiftedBooleanAnd                                = (a, b, c)          => JsExpression.Invocation(JsExpression.Identifier("$LiftedBooleanAnd"), a, b);
			LiftedBooleanOr                                 = (a, b, c)          => JsExpression.Invocation(JsExpression.Identifier("$LiftedBooleanOr"), a, b);
			Bind                                            = (f, t, c)          => JsExpression.Invocation(JsExpression.Identifier("$Bind"), f, t);
			BindFirstParameterToThis                        = (f, c)             => JsExpression.Invocation(JsExpression.Identifier("$BindFirstParameterToThis"), f);
			Default                                         = (t, c)             => t.Kind == TypeKind.Dynamic ? (JsExpression)JsExpression.Identifier("$DefaultDynamic") : JsExpression.Invocation(JsExpression.Identifier("$Default"), GetScriptType(t, TypeContext.GetDefaultValue, c.ResolveTypeParameter));
			CreateArray                                     = (t, dim, c)        => JsExpression.Invocation(JsExpression.Identifier("$CreateArray"), new[] { GetScriptType(t, TypeContext.GetDefaultValue, c.ResolveTypeParameter) }.Concat(dim));
			CloneDelegate                                   = (e, s, t, c)       => JsExpression.Invocation(JsExpression.Identifier("$CloneDelegate"), e);
			CallBase                                        = (m, a, c)          => JsExpression.Invocation(JsExpression.Identifier("$CallBase"), new[] { GetScriptType(m.DeclaringType, TypeContext.BindBaseCall, c.ResolveTypeParameter), JsExpression.String("$" + m.Name), JsExpression.ArrayLiteral(m is SpecializedMethod ? ((SpecializedMethod)m).TypeArguments.Select(x => GetScriptType(x, TypeContext.GenericArgument, c.ResolveTypeParameter)) : new JsExpression[0]), JsExpression.ArrayLiteral(a) });
			BindBaseCall                                    = (m, a, c)          => JsExpression.Invocation(JsExpression.Identifier("$BindBaseCall"), new[] { GetScriptType(m.DeclaringType, TypeContext.BindBaseCall, c.ResolveTypeParameter), JsExpression.String("$" + m.Name), JsExpression.ArrayLiteral(m is SpecializedMethod ? ((SpecializedMethod)m).TypeArguments.Select(x => GetScriptType(x, TypeContext.GenericArgument, c.ResolveTypeParameter)) : new JsExpression[0]), a });
			MakeEnumerator                                  = (yt, mn, gc, d, c) => JsExpression.Invocation(JsExpression.Identifier("$MakeEnumerator"), new[] { GetScriptType(yt, TypeContext.GenericArgument, c.ResolveTypeParameter), mn, gc, d ?? JsExpression.Null });
			MakeEnumerable                                  = (yt, ge, c)        => JsExpression.Invocation(JsExpression.Identifier("$MakeEnumerable"), new[] { GetScriptType(yt, TypeContext.GenericArgument, c.ResolveTypeParameter), ge });
			GetMultiDimensionalArrayValue                   = (a, i, c)          => JsExpression.Invocation(JsExpression.Identifier("$MultidimArrayGet"), new[] { a }.Concat(i));
			SetMultiDimensionalArrayValue                   = (a, i, v, c)       => JsExpression.Invocation(JsExpression.Identifier("$MultidimArraySet"), new[] { a }.Concat(i).Concat(new[] { v }));
			CreateTaskCompletionSource                      = (t, c)             => JsExpression.Invocation(JsExpression.Identifier("$CreateTaskCompletionSource"), t != null ? GetScriptType(t, TypeContext.GenericArgument, c.ResolveTypeParameter) : JsExpression.String("non-generic"));
			SetAsyncResult                                  = (t, v, c)          => JsExpression.Invocation(JsExpression.Identifier("$SetAsyncResult"), t, v ?? JsExpression.String("<<null>>"));
			SetAsyncException                               = (t, e, c)          => JsExpression.Invocation(JsExpression.Identifier("$SetAsyncException"), t, e);
			GetTaskFromTaskCompletionSource                 = (t, c)             => JsExpression.Invocation(JsExpression.Identifier("$GetTask"), t);
			ApplyConstructor                                = (c, a, x)          => JsExpression.Invocation(JsExpression.Identifier("$ApplyConstructor"), c, a);
			ShallowCopy                                     = (s, t, c)          => JsExpression.Invocation(JsExpression.Identifier("$ShallowCopy"), s, t);
			GetMember                                       = (m, c)             => JsExpression.Invocation(JsExpression.Identifier("$GetMember"), GetScriptType(m.DeclaringType, TypeContext.TypeOf, c.ResolveTypeParameter), JsExpression.String(m.Name));
			GetExpressionForLocal                           = (n, a, t, c)       => JsExpression.Invocation(JsExpression.Identifier("$Local"), JsExpression.String(n), GetScriptType(t, TypeContext.TypeOf, c.ResolveTypeParameter), a);
			CloneValueType                                  = (v, t, c)          => JsExpression.Invocation(JsExpression.Identifier("$Clone"), v, GetScriptType(t, TypeContext.TypeOf, c.ResolveTypeParameter));
			InitializeField                                 = (t, n, m, v, c)    => JsExpression.Invocation(JsExpression.Identifier("$Init"), t, JsExpression.String(n), v);
		}

		public Func<IType, IRuntimeContext, JsExpression> GetTypeOf { get; set; }
		public Func<IType, IRuntimeContext, JsExpression> InstantiateType { get; set; }
		public Func<IType, IRuntimeContext, JsExpression> InstantiateTypeForUseAsTypeArgumentInInlineCode { get; set; }
		public Func<JsExpression, IType, IType, IRuntimeContext, JsExpression> TypeIs { get; set; }
		public Func<JsExpression, IType, IType, IRuntimeContext, JsExpression> TryDowncast { get; set; }
		public Func<JsExpression, IType, IType, IRuntimeContext, JsExpression> Downcast { get; set; }
		public Func<JsExpression, IType, IType, IRuntimeContext, JsExpression> Upcast { get; set; }
		public Func<JsExpression, IEnumerable<IType>, IRuntimeContext, JsExpression> InstantiateGenericMethod { get; set; }
		new public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> ReferenceEquals { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> ReferenceNotEquals { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> MakeException { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> IntegerDivision { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Addition { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Addition { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Subtraction { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Subtraction { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Multiplication { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Multiplication { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Division { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Division { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Modulus { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Modulus { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64BitwiseAnd { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64BitwiseAnd { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64BitwiseOr { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64BitwiseOr { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64ExclusiveOr { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64ExclusiveOr { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64LeftShift { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64LeftShift { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64RightShift { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64RightShift { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Equality { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Equality { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64Inequality { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64Inequality { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64LessThanOrEqual { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64LessThanOrEqual { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64GreaterThanOrEqual { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64GreaterThanOrEqual { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64LessThan { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64LessThan { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> UInt64GreaterThan { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Int64GreaterThan { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64Negation { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64Negation { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64OnesComplement { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64OnesComplement { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64Increment { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64Increment { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64Decrement { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64Decrement { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> FloatToInt { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromUInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromUInt64 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromDouble { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromSingle { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64FromDecimal { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64ToUInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64ToInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64ToDouble { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64ToSingle { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Int64ToDecimal { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromUInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromInt64 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromDouble { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromSingle { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64FromDecimal { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64ToUInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64ToInt32 { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64ToDouble { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64ToSingle { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> UInt64ToDecimal { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Coalesce { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> Lift { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> FromNullable { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> LiftedBooleanAnd { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> LiftedBooleanOr { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> Bind { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> BindFirstParameterToThis { get; set; }
		public Func<IType, IRuntimeContext, JsExpression> Default { get; set; }
		public Func<IType, IEnumerable<JsExpression>, IRuntimeContext, JsExpression> CreateArray { get; set; }
		public Func<JsExpression, IType, IType, IRuntimeContext, JsExpression> CloneDelegate { get; set; }
		public Func<IMethod, IEnumerable<JsExpression>, IRuntimeContext, JsExpression> CallBase { get; set; }
		public Func<IMethod, JsExpression, IRuntimeContext, JsExpression> BindBaseCall { get; set; }
		public Func<IType, JsExpression, JsExpression, JsExpression, IRuntimeContext, JsExpression> MakeEnumerator { get; set; }
		public Func<IType, JsExpression, IRuntimeContext, JsExpression> MakeEnumerable { get; set; }
		public Func<JsExpression, IEnumerable<JsExpression>, IRuntimeContext, JsExpression> GetMultiDimensionalArrayValue { get; set; }
		public Func<JsExpression, IEnumerable<JsExpression>, JsExpression, IRuntimeContext, JsExpression> SetMultiDimensionalArrayValue { get; set; }
		public Func<IType, IRuntimeContext, JsExpression> CreateTaskCompletionSource { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> SetAsyncResult { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> SetAsyncException { get; set; }
		public Func<JsExpression, IRuntimeContext, JsExpression> GetTaskFromTaskCompletionSource { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> ApplyConstructor { get; set; }
		public Func<JsExpression, JsExpression, IRuntimeContext, JsExpression> ShallowCopy { get; set; }
		public Func<IMember, IRuntimeContext, JsExpression> GetMember { get; set; }
		public Func<string, JsExpression, IType, IRuntimeContext, JsExpression> GetExpressionForLocal { get; set; }
		public Func<JsExpression, IType, IRuntimeContext, JsExpression> CloneValueType { get; set; }
		public Func<JsExpression, string, IMember, JsExpression, IRuntimeContext, JsExpression> InitializeField { get; set; }

		private JsExpression GetScriptType(IType type, TypeContext context, Func<ITypeParameter, JsExpression> resolveTypeParameter) {
			string contextName = GetTypeContextShortName(context);
			if (type is ParameterizedType) {
				var pt = (ParameterizedType)type;
				return JsExpression.Invocation(JsExpression.Identifier(contextName + "_$InstantiateGenericType"), new[] { new JsTypeReferenceExpression(Common.CreateMockTypeDefinition(type.Name, Common.CreateMockAssembly())) }.Concat(pt.TypeArguments.Select(a => GetScriptType(a, TypeContext.GenericArgument, resolveTypeParameter))));
			}
			else if (type.TypeParameterCount > 0) {
				// This handles open generic types ( typeof(C<,>) )
				return new JsTypeReferenceExpression(Common.CreateMockTypeDefinition(contextName + "_" + type.GetDefinition().Name, Common.CreateMockAssembly()));
			}
			else if (type.Kind == TypeKind.Array) {
				return JsExpression.Invocation(JsExpression.Identifier(contextName + "_$Array"), GetScriptType(((ArrayType)type).ElementType, TypeContext.GenericArgument, resolveTypeParameter));
			}
			else if (type.Kind == TypeKind.Anonymous) {
				return JsExpression.Identifier(contextName + "_$Anonymous");
			}
			else if (type is ITypeDefinition) {
				return new JsTypeReferenceExpression(Common.CreateMockTypeDefinition(contextName + "_" + type.Name, Common.CreateMockAssembly()));
			}
			else if (type is ITypeParameter) {
				return resolveTypeParameter((ITypeParameter)type);
			}
			else {
				throw new ArgumentException("Unsupported type + " + type);
			}
		}

		JsExpression IRuntimeLibrary.TypeOf(IType type, IRuntimeContext context) {
			return GetTypeOf(type, context);
		}

		JsExpression IRuntimeLibrary.InstantiateType(IType type, IRuntimeContext context) {
			return InstantiateType(type, context);
		}

		JsExpression IRuntimeLibrary.InstantiateTypeForUseAsTypeArgumentInInlineCode(IType type, IRuntimeContext context) {
			return InstantiateTypeForUseAsTypeArgumentInInlineCode(type, context);
		}

		JsExpression IRuntimeLibrary.TypeIs(JsExpression expression, IType sourceType, IType targetType, IRuntimeContext context) {
			return TypeIs(expression, sourceType, targetType, context);
		}

		JsExpression IRuntimeLibrary.TryDowncast(JsExpression expression, IType sourceType, IType targetType, IRuntimeContext context) {
			return TryDowncast(expression, sourceType, targetType, context);
		}

		JsExpression IRuntimeLibrary.Downcast(JsExpression expression, IType sourceType, IType targetType, IRuntimeContext context) {
			return Downcast(expression, sourceType, targetType, context);
		}

		JsExpression IRuntimeLibrary.Upcast(JsExpression expression, IType sourceType, IType targetType, IRuntimeContext context) {
			return Upcast(expression, sourceType, targetType, context);
		}

		JsExpression IRuntimeLibrary.ReferenceEquals(JsExpression a, JsExpression b, IRuntimeContext context) {
			return ReferenceEquals(a, b, context);
		}

		JsExpression IRuntimeLibrary.ReferenceNotEquals(JsExpression a, JsExpression b, IRuntimeContext context) {
			return ReferenceNotEquals(a, b, context);
		}

		JsExpression IRuntimeLibrary.InstantiateGenericMethod(JsExpression type, IEnumerable<IType> typeArguments, IRuntimeContext context) {
			return InstantiateGenericMethod(type, typeArguments, context);
		}

		JsExpression IRuntimeLibrary.MakeException(JsExpression operand, IRuntimeContext context) {
			return MakeException(operand, context);
		}

		JsExpression IRuntimeLibrary.IntegerDivision(JsExpression numerator, JsExpression denominator, IRuntimeContext context) {
			return IntegerDivision(numerator, denominator, context);
		}

		JsExpression IRuntimeLibrary.UInt64Addition(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64Addition(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64Addition(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64Addition(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Subtraction(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64Subtraction(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64Subtraction(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64Subtraction(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Multiplication(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64Multiplication(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64Multiplication(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64Multiplication(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Division(JsExpression numerator, JsExpression denominator, IRuntimeContext context)
		{
			return UInt64Division(numerator, denominator, context);
		}

		JsExpression IRuntimeLibrary.Int64Division(JsExpression numerator, JsExpression denominator, IRuntimeContext context)
		{
			return Int64Division(numerator, denominator, context);
		}

		JsExpression IRuntimeLibrary.UInt64Modulus(JsExpression numerator, JsExpression denominator, IRuntimeContext context)
		{
			return UInt64Modulus(numerator, denominator, context);
		}
		
		JsExpression IRuntimeLibrary.Int64Modulus(JsExpression numerator, JsExpression denominator, IRuntimeContext context)
		{
			return Int64Division(numerator, denominator, context);
		}
		
		JsExpression IRuntimeLibrary.UInt64BitwiseAnd(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64BitwiseAnd(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64BitwiseAnd(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64BitwiseAnd(operand, operand2, context);
		}
		
		JsExpression IRuntimeLibrary.UInt64BitwiseOr(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64BitwiseOr(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64BitwiseOr(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64BitwiseOr(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64ExclusiveOr(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64ExclusiveOr(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64ExclusiveOr(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64ExclusiveOr(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64LeftShift(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64LeftShift(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64LeftShift(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64LeftShift(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64RightShift(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64RightShift(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64RightShift(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64RightShift(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Equality(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64Equality(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64Equality(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64Equality(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Inequality(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64Inequality(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64Inequality(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64Inequality(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64LessThanOrEqual(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64LessThanOrEqual(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64LessThanOrEqual(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64LessThanOrEqual(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64GreaterThanOrEqual(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64GreaterThanOrEqual(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64GreaterThanOrEqual(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64GreaterThanOrEqual(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64LessThan(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64LessThan(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64LessThan(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64LessThan(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64GreaterThan(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return UInt64GreaterThan(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.Int64GreaterThan(JsExpression operand, JsExpression operand2, IRuntimeContext context)
		{
			return Int64GreaterThan(operand, operand2, context);
		}

		JsExpression IRuntimeLibrary.UInt64Negation(JsExpression operand, IRuntimeContext context)
		{
			return UInt64Negation(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64Negation(JsExpression operand, IRuntimeContext context)
		{
			return Int64Negation(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64OnesComplement(JsExpression operand, IRuntimeContext context)
		{
			return UInt64OnesComplement(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64OnesComplement(JsExpression operand, IRuntimeContext context)
		{
			return Int64OnesComplement(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64Increment(JsExpression operand, IRuntimeContext context)
		{
			return UInt64Increment(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64Increment(JsExpression operand, IRuntimeContext context)
		{
			return Int64Increment(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64Decrement(JsExpression operand, IRuntimeContext context)
		{
			return UInt64Decrement(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64Decrement(JsExpression operand, IRuntimeContext context)
		{
			return Int64Decrement(operand, context);
		}

		JsExpression IRuntimeLibrary.FloatToInt(JsExpression operand, IRuntimeContext context) {
			return FloatToInt(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromUInt32(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromUInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromUInt64(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromUInt64(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromInt32(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromDouble(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromDouble(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromSingle(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromSingle(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64FromDecimal(JsExpression operand, IRuntimeContext context)
		{
			return Int64FromDecimal(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64ToUInt32(JsExpression operand, IRuntimeContext context)
		{
			return Int64ToUInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64ToInt32(JsExpression operand, IRuntimeContext context)
		{
			return Int64ToInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64ToDouble(JsExpression operand, IRuntimeContext context)
		{
			return Int64ToDouble(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64ToSingle(JsExpression operand, IRuntimeContext context)
		{
			return Int64ToSingle(operand, context);
		}

		JsExpression IRuntimeLibrary.Int64ToDecimal(JsExpression operand, IRuntimeContext context)
		{
			return Int64ToDecimal(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromUInt32(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromUInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromInt64(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromInt64(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromInt32(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromDouble(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromDouble(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromSingle(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromSingle(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64FromDecimal(JsExpression operand, IRuntimeContext context)
		{
			return UInt64FromDecimal(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64ToUInt32(JsExpression operand, IRuntimeContext context)
		{
			return UInt64ToUInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64ToInt32(JsExpression operand, IRuntimeContext context)
		{
			return UInt64ToInt32(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64ToDouble(JsExpression operand, IRuntimeContext context)
		{
			return UInt64ToDouble(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64ToSingle(JsExpression operand, IRuntimeContext context)
		{
			return UInt64ToSingle(operand, context);
		}

		JsExpression IRuntimeLibrary.UInt64ToDecimal(JsExpression operand, IRuntimeContext context)
		{
			return UInt64ToDecimal(operand, context);
		}

		JsExpression IRuntimeLibrary.Coalesce(JsExpression a, JsExpression b, IRuntimeContext context) {
			return Coalesce(a, b, context);
		}

		JsExpression IRuntimeLibrary.Lift(JsExpression expression, IRuntimeContext context) {
			return Lift(expression, context);
		}

		JsExpression IRuntimeLibrary.FromNullable(JsExpression expression, IRuntimeContext context) {
			return FromNullable(expression, context);
		}

		JsExpression IRuntimeLibrary.LiftedBooleanAnd(JsExpression a, JsExpression b, IRuntimeContext context) {
			return LiftedBooleanAnd(a, b, context);
		}

		JsExpression IRuntimeLibrary.LiftedBooleanOr(JsExpression a, JsExpression b, IRuntimeContext context) {
			return LiftedBooleanOr(a, b, context);
		}

		JsExpression IRuntimeLibrary.Bind(JsExpression function, JsExpression target, IRuntimeContext context) {
			return Bind(function, target, context);
		}

		JsExpression IRuntimeLibrary.BindFirstParameterToThis(JsExpression function, IRuntimeContext context) {
			return BindFirstParameterToThis(function, context);
		}

		JsExpression IRuntimeLibrary.Default(IType type, IRuntimeContext context) {
			return Default(type, context);
		}

		JsExpression IRuntimeLibrary.CreateArray(IType elementType, IEnumerable<JsExpression> size, IRuntimeContext context) {
			return CreateArray(elementType, size, context);
		}

		JsExpression IRuntimeLibrary.CloneDelegate(JsExpression source, IType sourceType, IType targetType, IRuntimeContext context) {
			return CloneDelegate(source, sourceType, targetType, context);
		}

		JsExpression IRuntimeLibrary.CallBase(IMethod method, IEnumerable<JsExpression> thisAndArguments, IRuntimeContext context) {
			return CallBase(method, thisAndArguments, context);
		}

		JsExpression IRuntimeLibrary.BindBaseCall(IMethod method, JsExpression @this, IRuntimeContext context) {
			return BindBaseCall(method, @this, context);
		}

		JsExpression IRuntimeLibrary.MakeEnumerator(IType yieldType, JsExpression moveNext, JsExpression getCurrent, JsExpression dispose, IRuntimeContext context) {
			return MakeEnumerator(yieldType, moveNext, getCurrent, dispose, context);
		}

		JsExpression IRuntimeLibrary.MakeEnumerable(IType yieldType, JsExpression getEnumerator, IRuntimeContext context) {
			return MakeEnumerable(yieldType, getEnumerator, context);
		}

		JsExpression IRuntimeLibrary.GetMultiDimensionalArrayValue(JsExpression array, IEnumerable<JsExpression> indices, IRuntimeContext context) {
			return GetMultiDimensionalArrayValue(array, indices, context);
		}

		JsExpression IRuntimeLibrary.SetMultiDimensionalArrayValue(JsExpression array, IEnumerable<JsExpression> indices, JsExpression value, IRuntimeContext context) {
			return SetMultiDimensionalArrayValue(array, indices, value, context);
		}

		JsExpression IRuntimeLibrary.CreateTaskCompletionSource(IType taskGenericArgument, IRuntimeContext context) {
			return CreateTaskCompletionSource(taskGenericArgument, context);
		}

		JsExpression IRuntimeLibrary.SetAsyncResult(JsExpression taskCompletionSource, JsExpression value, IRuntimeContext context) {
			return SetAsyncResult(taskCompletionSource, value, context);
		}

		JsExpression IRuntimeLibrary.SetAsyncException(JsExpression taskCompletionSource, JsExpression exception, IRuntimeContext context) {
			return SetAsyncException(taskCompletionSource, exception, context);
		}

		JsExpression IRuntimeLibrary.GetTaskFromTaskCompletionSource(JsExpression taskCompletionSource, IRuntimeContext context) {
			return GetTaskFromTaskCompletionSource(taskCompletionSource, context);
		}

		JsExpression IRuntimeLibrary.ApplyConstructor(JsExpression constructor, JsExpression argumentsArray, IRuntimeContext context) {
			return ApplyConstructor(constructor, argumentsArray, context);
		}

		JsExpression IRuntimeLibrary.ShallowCopy(JsExpression source, JsExpression target, IRuntimeContext context) {
			return ShallowCopy(source, target, context);
		}

		JsExpression IRuntimeLibrary.GetMember(IMember member, IRuntimeContext context) {
			return GetMember(member, context);
		}

		JsExpression IRuntimeLibrary.GetExpressionForLocal(string name, JsExpression accessor, IType type, IRuntimeContext context) {
			return GetExpressionForLocal(name, accessor, type, context);
		}

		JsExpression IRuntimeLibrary.CloneValueType(JsExpression value, IType type, IRuntimeContext context) {
			return CloneValueType(value, type, context);
		}

		JsExpression IRuntimeLibrary.InitializeField(JsExpression jsMember, string scriptName, IMember member, JsExpression initialValue, IRuntimeContext context) {
			return InitializeField(jsMember, scriptName, member, initialValue, context);
		}
	}
}