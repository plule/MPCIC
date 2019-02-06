using NUnit.Framework;
using MPCIC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using MPCIC.Tests.TestData;
using System.IO;

namespace MPCIC.Tests.Core
{
    [TestFixture]
    public class SampleCollectionTests
    {
        [Test]
        [TestCaseSource(typeof(TestLibrary), "Collections")]
        public static void TestLibrariesAreCorrectlyGuessedFromFiles(TestLibrary library)
        {
            SampleCollection reference = library.Collection;
            SampleCollection collection = SampleCollection.FromFilesInDirectory(library.FilePaths);

            Assert.That(collection.Name, Is.EqualTo(reference.Name));
            Assert.That(collection.Samples, Is.EquivalentTo(reference.Samples));
        }

        [Test]
        [TestCaseSource(typeof(TestLibrary), "Collections")]
        public static void TestLibrariesAreCorrectlyGuessedFromDirectory(TestLibrary library)
        {
            SampleCollection reference = library.Collection;
            SampleCollection collection = SampleCollection.FromDirectory(library.DirectoryPath);

            Assert.That(collection.Name, Is.EqualTo(reference.Name));
            Assert.That(collection.Samples, Is.EquivalentTo(reference.Samples));
        }

        [Test]
        [Ignore("For recreating test data")]
        [TestCaseSource(typeof(TestLibrary), "Collections")]
        public static void CreateTestLibrariesFile(TestLibrary library)
        {
            foreach(string path in library.FilePaths)
            {
                string directory = Path.GetDirectoryName(path);
                Directory.CreateDirectory(directory);
                using(File.AppendText(path)){}
            }
        }
    }
}