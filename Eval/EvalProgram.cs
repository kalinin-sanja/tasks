﻿using System;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using NUnit.Framework;

namespace EvalTask
{
	class EvalProgram
	{
        static double Evaluate(string expression)
        {
            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);
            return (double)(loDataTable.Rows[0]["Eval"]);
        }
        static string EvalExpression(string exprStr)
        {
            return Evaluate(exprStr).ToString(CultureInfo.InvariantCulture);

        }
        static void Main(string[] args)
		{
			string input = Console.In.ReadToEnd();
			string output = "TODO";
			Console.WriteLine(output);
		}

	    [Test]
	    public void TestEvaluator()
	    {
	        Assert.AreEqual("5.8", EvalExpression("2.8+3"));
	    }

	    [Test]
	    public void Unary_Minus_Test()
	    {
            Assert.AreEqual("0.2", EvalExpression("-2.8+3"));
        }
	}
}
