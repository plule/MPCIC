using NUnit.Framework;
using MPCIC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using MPCIC.Tests.TestData;

namespace MPCIC.Tests.Core
{
    [TestFixture]
    public class SampleCollectionTests
    {
        [Test]
        [TestCaseSource(typeof(TestLibrary), "Collections")]
        public static void TestLibrariesAreCorrectlyGuessed(TestLibrary library)
        {
            SampleCollection reference = library.Collection;
            SampleCollection collection = SampleCollection.FromFilesInDirectory(library.FilePaths);

            Assert.That(collection.Name, Is.EqualTo(reference.Name));
            Assert.That(collection.Samples, Is.EquivalentTo(reference.Samples));
        }
    }
}