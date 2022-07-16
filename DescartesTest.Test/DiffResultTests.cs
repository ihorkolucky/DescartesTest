using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescartesTest.Test
{

    /// <summary>
    /// Unit Tests for DiffResult class
    /// </summary>
    [TestClass()]
    public class DiffResultTests
    {
        [TestMethod()]
        public void GetResult_Test_Equals()
        {
            // arrange

            string val1 = "val1";
            string val2 = "val1";

            var expected = "Equals";

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            DiffResult diffResult = new DiffResult(diff);

            var actualResult = diffResult.GetResult();

            var actual = actualResult.GetValue("diffResultType");

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetResult_Test_SizeDoNotMatch()
        {
            // arrange

            string val1 = "val1";
            string val2 = "val12";

            var expected = "SizeDoNotMatch";

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            DiffResult diffResult = new DiffResult(diff);

            var actualResult = diffResult.GetResult();

            var actual = actualResult.GetValue("diffResultType");

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetResult_Test_ContentDoNotMatch()
        {
            // arrange

            string val1 = "val1";
            string val2 = "val2";

            var expected = "ContentDoNotMatch";

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            DiffResult diffResult = new DiffResult(diff);

            var actualResult = diffResult.GetResult();

            var actual = actualResult.GetValue("diffResultType");

            // assert
            Assert.AreEqual(expected, actual);
        }

    }
}
