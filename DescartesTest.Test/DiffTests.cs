using Microsoft.VisualStudio.TestTools.UnitTesting;
using DescartesTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescartesTest.Tests
{
    /// <summary>
    /// Unit Tests for Diff class
    /// </summary>
    [TestClass()]
    public class DiffTests
    {
        [TestMethod()]
        public void IsFilled_Test()
        {
            // arrange

            string val1 = "valFirst";
            string val2 = "valSecond";

            bool expected = true;

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            bool actual = diff.IsFilled;

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsEqual_Test()
        {
            // arrange

            string val1 = "12345";
            string val2 = "12345";

            bool expected = true;

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            bool actual = diff.IsEqual;

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void IsEqualSize_Test()
        {
            // arrange

            string val1 = "12345";
            string val2 = "67890";

            bool expected = true;

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            bool actual = diff.IsEqualSize;

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsContentDoNotMatch_Test()
        {
            // arrange

            string val1 = "val1";
            string val2 = "val2";

            bool expected = true;

            // act

            Diff diff = new Diff { Left = val1, Right = val2 };

            bool actual = diff.IsContentDoNotMatch;

            // assert
            Assert.AreEqual(expected, actual);
        }

    }
}