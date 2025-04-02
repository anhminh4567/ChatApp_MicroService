using ThreadLike.Common.Domain;

namespace ThreadLike.User.Unittest
{
	public class UnitTest1
	{
		private const string TEST_STRING = "asdf";
		[Fact]
		public void Test1()
		{
			var result1 = testFunction();
			var result2 = testFunction2();
			Assert.True(result1.Value == TEST_STRING);
			Assert.True(result2.Value == TEST_STRING);

		}

		public Result<string> testFunction()
		{
			return Result.Ok(TEST_STRING);
		}
		public Result<string> testFunction2()
		{
			return TEST_STRING;
		}

	}
}