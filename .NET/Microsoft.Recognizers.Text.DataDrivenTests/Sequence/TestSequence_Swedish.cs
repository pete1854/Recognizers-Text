﻿using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequence_Swedish : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void PhoneNumberModel(TestModel testSpec)
        {
            TestPhoneNumber(testSpec);
        }
    }
}