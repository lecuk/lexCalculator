using lexCalculator.Types;
using System;
using System.Collections.Generic;

namespace lexCalculator.Calculation
{
	static class OperationImplementations
	{
		public static readonly IReadOnlyDictionary<UnaryOperation, Func<double, double>> UnaryFunctions 
			= new Dictionary<UnaryOperation, Func<double, double>>()
		{
			{ UnaryOperation.Negative,				MoreMath.Negative },
			{ UnaryOperation.Sign,					MoreMath.FSign },
			{ UnaryOperation.Sine,					Math.Sin},
			{ UnaryOperation.Cosine,				Math.Cos},
			{ UnaryOperation.Tangent,				Math.Tan},
			{ UnaryOperation.Cotangent,				MoreMath.Cot },
			{ UnaryOperation.Secant,				MoreMath.Sec },
			{ UnaryOperation.Cosecant,				MoreMath.Csc },
			{ UnaryOperation.ArcSine,				Math.Asin },
			{ UnaryOperation.ArcCosine,				Math.Acos },
			{ UnaryOperation.ArcTangent,			Math.Atan },
			{ UnaryOperation.ArcCotangent,			MoreMath.Acot },
			{ UnaryOperation.ArcSecant,				MoreMath.Asec },
			{ UnaryOperation.ArcCosecant,			MoreMath.Acsc },
			{ UnaryOperation.SineHyperbolic,		Math.Sinh },
			{ UnaryOperation.CosineHyperbolic,		Math.Cosh },
			{ UnaryOperation.TangentHyperbolic,		Math.Tanh },
			{ UnaryOperation.CotangentHyperbolic,	MoreMath.Coth },
			{ UnaryOperation.SecantHyperbolic,		MoreMath.Sech },
			{ UnaryOperation.CosecantHyperbolic,	MoreMath.Csch },
			{ UnaryOperation.Exponent,				Math.Exp },
			{ UnaryOperation.NaturalLogarithm,		Math.Log },
			{ UnaryOperation.Square,				MoreMath.Square },
			{ UnaryOperation.Cube,                  MoreMath.Cube },
			{ UnaryOperation.SquareRoot,			Math.Sqrt },
			{ UnaryOperation.CubeRoot,				MoreMath.Cbrt },
			{ UnaryOperation.Floor,					Math.Floor },
			{ UnaryOperation.Ceiling,				Math.Ceiling },
			{ UnaryOperation.AbsoluteValue,			Math.Abs },
			{ UnaryOperation.Factorial,				MoreMath.Factorial }
		};

		public static readonly IReadOnlyDictionary<UnaryOperation, Action<double[], double[]>> UnaryArrayFunctions 
			= new Dictionary<UnaryOperation, Action<double[], double[]>>()
		{
			{ UnaryOperation.Negative,              (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = -x[i];							} },
			{ UnaryOperation.Sign,                  (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Sign(x[i]);					} },
			{ UnaryOperation.Sine,                  (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Sin(x[i]);					} },
			{ UnaryOperation.Cosine,                (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Cos(x[i]);					} },
			{ UnaryOperation.Tangent,               (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Tan(x[i]);					} },
			{ UnaryOperation.Cotangent,             (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Tan(x[i]);				} },
			{ UnaryOperation.Secant,                (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Cos(x[i]);				} },
			{ UnaryOperation.Cosecant,              (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Sin(x[i]);				} },
			{ UnaryOperation.ArcSine,               (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Asin(x[i]);					} },
			{ UnaryOperation.ArcCosine,             (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Acos(x[i]);					} },
			{ UnaryOperation.ArcTangent,            (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Atan(x[i]);					} },
			{ UnaryOperation.ArcCotangent,          (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Atan(1.0 / x[i]);			} },
			{ UnaryOperation.ArcSecant,             (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Acos(1.0 / x[i]);			} },
			{ UnaryOperation.ArcCosecant,           (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Asin(1.0 / x[i]);			} },
			{ UnaryOperation.SineHyperbolic,        (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Sinh(x[i]);					} },
			{ UnaryOperation.CosineHyperbolic,      (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Cosh(x[i]);					} },
			{ UnaryOperation.TangentHyperbolic,     (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Tanh(x[i]);					} },
			{ UnaryOperation.CotangentHyperbolic,   (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Tanh(x[i]);			} },
			{ UnaryOperation.SecantHyperbolic,      (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Cosh(x[i]);			} },
			{ UnaryOperation.CosecantHyperbolic,    (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = 1.0 / Math.Sinh(x[i]);			} },
			{ UnaryOperation.Exponent,              (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Exp(x[i]);					} },
			{ UnaryOperation.NaturalLogarithm,      (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Log(x[i]);					} },
			{ UnaryOperation.SquareRoot,            (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Sqrt(x[i]);					} },
			{ UnaryOperation.Square,				(x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = x[i] * x[i];						} },
			{ UnaryOperation.Cube,					(x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = x[i] * x[i] * x[i];				} },
			{ UnaryOperation.CubeRoot,              (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Pow(x[i], 0.3333333333333);	} },
			{ UnaryOperation.Floor,                 (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Floor(x[i]);					} },
			{ UnaryOperation.Ceiling,               (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Ceiling(x[i]);				} },
			{ UnaryOperation.AbsoluteValue,         (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Abs(x[i]);					} },
			{ UnaryOperation.Factorial,             (x, r) => { for (int i = 0; i < r.Length; ++i) r[i] = MoreMath.Factorial(x[i]);			} }
		};

		public static readonly IReadOnlyDictionary<BinaryOperation, Func<double, double, double>> BinaryFunctions 
			= new Dictionary<BinaryOperation, Func<double, double, double>>()
		{
			{ BinaryOperation.Addition,			MoreMath.Sum },
			{ BinaryOperation.Substraction,		MoreMath.Sub },
			{ BinaryOperation.Multiplication,	MoreMath.Mul },
			{ BinaryOperation.Division,			MoreMath.Div },
			{ BinaryOperation.Power,			Math.Pow },
			{ BinaryOperation.Remainder,		Math.IEEERemainder },
			{ BinaryOperation.Logarithm,		MoreMath.Log },
			{ BinaryOperation.NRoot,			MoreMath.Nrt }
		};

		public static readonly IReadOnlyDictionary<BinaryOperation, Action<double[], double[], double[]>> BinaryArrayFunctions 
			= new Dictionary<BinaryOperation, Action<double[], double[], double[]>>()
		{
			{ BinaryOperation.Addition,         (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] + b[i];						} },
			{ BinaryOperation.Substraction,     (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] - b[i];						} },
			{ BinaryOperation.Multiplication,   (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] * b[i];						} },
			{ BinaryOperation.Division,         (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] / b[i];						} },
			{ BinaryOperation.Power,            (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Pow(a[i], b[i]);				} },
			{ BinaryOperation.Remainder,        (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.IEEERemainder(a[i], b[i]);	} },
			{ BinaryOperation.Logarithm,        (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Log(b[i]) / Math.Log(a[i]);	} },
			{ BinaryOperation.NRoot,            (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Pow(a[i], 1.0 / b[i]);		} }
		};

		public static readonly IReadOnlyDictionary<BinaryOperation, Action<double[], double[], double[]>> BinaryArrayFunctionsWithGPU
			= new Dictionary<BinaryOperation, Action<double[], double[], double[]>>()
		{
			{ BinaryOperation.Addition,         (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] + b[i];						} },
			{ BinaryOperation.Substraction,     (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] - b[i];                       } },
			{ BinaryOperation.Multiplication,   (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] * b[i];                       } },
			{ BinaryOperation.Division,         (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = a[i] / b[i];                       } },
			{ BinaryOperation.Power,            (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Pow(a[i], b[i]);              } },
			{ BinaryOperation.Remainder,        (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.IEEERemainder(a[i], b[i]);    } },
			{ BinaryOperation.Logarithm,        (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Log(b[i]) / Math.Log(a[i]);   } },
			{ BinaryOperation.NRoot,            (a, b, r) => { for (int i = 0; i < r.Length; ++i) r[i] = Math.Pow(a[i], 1.0 / b[i]);        } }
		};
	}
}
