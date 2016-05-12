using System;
using System.CodeDom.Compiler;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
//using Microsoft.JScript;
//using Microsoft.JScript.Vsa;
using NUnit.Framework;
using NCalc;
using NCalc.Domain;

namespace EvalTask
{
    class EvalProgram
    {
        private static string ProcentEvaluator(Match input)
        {
            var pre = input.Value.Replace("%", "");
            var value = double.Parse(pre, CultureInfo.InvariantCulture) / 100;
            return System.Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        static string EvalExpression(string exprStr)
        {
            exprStr = exprStr.Replace("sqrt(", "Math.Sqrt((double)");
            exprStr = Regex.Replace(exprStr, @"\d+(?:\.?)(?:\d)*\s*%", new MatchEvaluator(ProcentEvaluator));
            MethodInfo function = CreateFunction(exprStr);
            string result = function.Invoke(null, null).ToString();
            
            //exprStr = Regex.Replace(exprStr, "\\d+(?:\\.?)(?:\\d*)%", new MatchEvaluator(ProcentEvaluator));
            //var expr = new NCalc.Expression(exprStr);
            //expr.EvaluateFunction += delegate (string name, FunctionArgs args)
            //{
            //    if (String.Equals(name, "sqrt", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        var value = System.Convert.ToDouble(args.EvaluateParameters()[0]);
            //        args.HasResult = true;
            //        args.Result = Math.Sqrt(value);
            //    }
            //};

            //var x = expr.Evaluate().ToString();
            //x = x.Replace("∞", "Infinity").Replace("бесконечность", "Infinity");
            //return x.Replace(",", ".");
            
            result = result.Replace("∞", "Infinity").Replace("бесконечность", "Infinity").Replace(",", ".");
            return result.Replace(",", ".");
        }

        public static MethodInfo CreateFunction(string function)
        {
            string code = @"
        using System;
            
        namespace UserFunctions
        {                
            public class BinaryFunction
            {                
                public static double Function()
                {
                    return ((double)(func_xy));
                }
            }
        }
    ";

            string finalCode = code.Replace("func_xy", function);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), finalCode);

            Type binaryFunction = results.CompiledAssembly.GetType("UserFunctions.BinaryFunction");
            return binaryFunction.GetMethod("Function");
        }



        static void Main(string[] args)
        {
            string input = Console.In.ReadToEnd();
            string output = EvalExpression(input);
            Console.WriteLine(output);
        }
        [Test]
        public void Tests()
        {
            Assert.AreEqual("0", EvalExpression("1/2"));
        }
        [Test]
        public void TestEvaluator()
        {
            Assert.AreEqual("5.8", EvalExpression("2.8+3"));
        }

        [Test]
        public void Integers_Test()
        {
            Assert.AreEqual("12", EvalExpression("7+4+1"));
        }

        [Test]
        public void Unary_Minus_Test()
        {
            Assert.AreEqual("0.2", EvalExpression("-2.8+3"));
        }

        [Test]
        public void OneArg_Test()
        {
            Assert.AreEqual("0.2", EvalExpression("0.2"));
        }
        [Test]
        public void Drobi()
        {
            Assert.AreEqual("4", EvalExpression("4+2/3"));
        }
        [Test]
        public void ZeroDiv_Test()
        {
            Assert.AreEqual("NaN", EvalExpression("0.0/0"));
        }
        [Test]
        public void Inf_Test()
        {
            Assert.AreEqual("Infinity", EvalExpression("1.0/0"));
        }
        [Test]
        public void Sqrt_Test()
        {
            Assert.AreEqual("1", EvalExpression("sqrt(1)"));
        }


        [TestCase("2+sqrt(25)*sqrt(4)", Result = "12", TestName = "SQRTHardTest")]
        [TestCase("sqrt(25)", Result = "5", TestName = "SQRTTest")]
        [TestCase("sqrt(25+1-1)", Result = "5", TestName = "SQRTTest2")]
        [TestCase("12.1%", Result = "0.121", TestName = "%Test1")]
        [TestCase("12.%", Result = "0.12", TestName = "%Test2")]
        [TestCase("12%", Result = "0.12", TestName = "%Test3")]
        public string TestEngine(string input)
        {
            return EvalExpression(input);
        }
    }
}
