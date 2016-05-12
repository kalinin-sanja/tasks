using System.Linq;
using NUnit.Framework;

namespace SimQLTask
{
	[TestFixture]
	public class SimQLProgram_Should
	{
		[Test]
		public void SumEmptyDataToZero()
		{
			var results = SimQLProgram.ExecuteQueries(
				"{" +
				"'data': [], " +
				"'queries': ['sum(item.cost)', 'sum(itemsCount)']}");
			Assert.AreEqual(new[] { "sum(item.cost) = 0", "sum(itemsCount) = 0" }, results);
		}

		[Test]
		public void SumSingleItem()
		{
			var results = SimQLProgram.ExecuteQueries(
				"{" +
				"'data': [{'itemsCount':42}, {'foo':'bar'}], " +
				"'queries': ['sum(itemsCount)']}");
			Assert.AreEqual(new[] { "sum(itemsCount) = 42" }, results);
		}

		[Test]
		public void DoSomething_WhenSomething()
		{
			var results =
				SimQLProgram.ExecuteQueries("{\"data\":{\"empty\":[],\"x\":[0.1,0.2,0.3],\"a\":[{\"b\":10,\"c\":[1,2,3]},{\"b\":30,\"c\":[4]},{\"d\":500}]},\"queries\":[\"sum(empty)\",\"sum(a.b)\",\"sum(a.c)\",\"sum(a.d)\",\"sum(x)\"]}").ToList();
			Assert.Contains("sum(x) = 0.6",results.ToList());
			Assert.Contains("sum(empty) = 0", results.ToList());

		}

		[Test]
		public void AdvancedSelectorsWorks()
		{
			var results = SimQLProgram.ExecuteQueries("{\"data\":{\"empty\":[],\"x\":[0.1,0.2,0.3],\"a\":[{\"b\":10,\"c\":[1,2,3]},{\"b\":30,\"c\":[4]},{\"b\":11,\"c\":[-1,-2,9]},{\"b\":22,\"c\":[-1,-2,9]},{\"d\":500}]},\"queries\":[\"sum(a.b from 12 top 2)\",\"sum(a.b from -100 top 1)\",\"sum(a.b from -100.1 top 0)\",\"sum(a.b from 1000 top 10)\",\"sum(a.b from 30.0 top 10)\",\"sum(a.b from -100 top 100)\"]}").ToList();
			Assert.Contains("sum(a.b from 12 top 2) = 52",results);

		}

	}
}