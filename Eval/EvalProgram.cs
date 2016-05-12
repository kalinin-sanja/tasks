using System;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
//using Microsoft.JScript;
//using Microsoft.JScript.Vsa;
using NUnit.Framework;
using NCalc;

namespace EvalTask
{
    class EvalProgram
    {
        static string EvalExpression(string exprStr)
        {
            /*var regex = new Regex(@"(^|[^\.])(\d+\/\d+)([^\.]|$)");
            foreach (var match in regex.Matches(exprStr))
            {
                var expRes = new NCalc.Expression(match.ToString()).Evaluate();
                long res = Convert.ToInt64(expRes);
                //bool can = long.TryParse(expRes, out res);
                //if (can)
                exprStr = exprStr.Replace(match.ToString(), res.ToString());
            }*/
            var expr = new NCalc.Expression(exprStr);

            expr.EvaluateFunction += delegate (string name, FunctionArgs args)
            {
                if (String.Equals(name, "sqrt", StringComparison.CurrentCultureIgnoreCase))
                {
                    var value = System.Convert.ToDouble(args.EvaluateParameters()[0]);
                    args.HasResult = true;
                    args.Result = Math.Sqrt(value);
                }
            };
            var x = expr.Evaluate().ToString();
            x = x.Replace("∞", "Infinity").Replace("бесконечность", "Infinity");
            return x.Replace(",", ".");
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
    }
}
