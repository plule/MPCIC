using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MPCIC.Core;

namespace MPCIC.Tests.TestData
{
    public class TestLibrary
    {
        public List<string> FilePaths;

        public SampleCollection Collection;

        public TestLibrary(string directory, IEnumerable<string> fileNames, SampleCollection collection)
        {
            FilePaths = fileNames.Select(fileName => Path.Combine(RootDirectory, directory, fileName)).ToList();

            if(FilePaths.Count != collection.Samples.Count)
                throw new ArgumentException("Bad TestLibrary: not the same number of files and samples.");

            Collection = collection;
            Collection.DirectoryPath = Path.Combine(RootDirectory, directory);

            for(int i = 0; i < FilePaths.Count; ++i)
            {
                Collection.Samples[i].FilePath = FilePaths[i];
            }
        }

        public static string RootDirectory
        {
            get
            {
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                return Path.Combine(assemblyPath, "..", "..", "..", "..", "TestData");
            }
        }

        public static List<TestLibrary> Collections
        {
            get
            {
                return new List<TestLibrary>
                {
                    /*Bass, */Synth, MellotronCello, MellotronFlute
                };
            }
        }

        public static TestLibrary Bass
        {
            get
            {
                return new TestLibrary("Bass", new string[]{"THMB40.WAV", "THMB43.WAV", "THMB48.WAV", "THMB52.WAV", "THMB55.WAV", "THMB60.WAV"}, new SampleCollection
                {
                    Name = "Bass",
                    Samples = new List<Sample>
                    {
                        new Sample("THMB", new Note(40)),
                        new Sample("THMB", new Note(43)),
                        new Sample("THMB", new Note(48)),
                        new Sample("THMB", new Note(52)),
                        new Sample("THMB", new Note(55)),
                        new Sample("THMB", new Note(60)),
                    }
                });
            }
        }

        public static TestLibrary Synth
        {
            get
            {
                return new TestLibrary("Synth", new string[]{"A1.wav", "A2.wav", "A3.wav", "A4.wav", "A5.wav", "A6.wav"}, new SampleCollection
                {
                    Name = "Synth",
                    Samples = new List<Sample>
                    {
                        new Sample("", new Note(Key.A, 1)),
                        new Sample("", new Note(Key.A, 2)),
                        new Sample("", new Note(Key.A, 3)),
                        new Sample("", new Note(Key.A, 4)),
                        new Sample("", new Note(Key.A, 5)),
                        new Sample("", new Note(Key.A, 6))
                    }
                });
            }
        }

        public static TestLibrary MellotronCello
        {
            get
            {
                return new TestLibrary("MellotronCello", new string[]{"MellotronCello-A2.wav", "MellotronCello-A3.wav", "MellotronCello-A4.wav", "MellotronCello-D3.wav", "MellotronCello-D4.wav", "MellotronCello-D5.wav"}, new SampleCollection
                {
                    Name = "MellotronCello",
                    Samples = new List<Sample>
                    {
                        new Sample("MellotronCello", new Note(Key.A, 2)),
                        new Sample("MellotronCello", new Note(Key.A, 3)),
                        new Sample("MellotronCello", new Note(Key.A, 4)),
                        new Sample("MellotronCello", new Note(Key.D, 3)),
                        new Sample("MellotronCello", new Note(Key.D, 4)),
                        new Sample("MellotronCello", new Note(Key.D, 5))
                    }
                });
            }
        }

        public static TestLibrary MellotronFlute
        {
            get
            {
                return new TestLibrary("MellotronFlute", new string[]{"MellotronFlute-A2.wav", "MellotronFlute-A3.wav", "MellotronFlute-A4.wav", "MellotronFlute-D3.wav", "MellotronFlute-D4.wav", "MellotronFlute-D5.wav"}, new SampleCollection
                {
                    Name = "MellotronFlute",
                    Samples = new List<Sample>
                    {
                        new Sample("MellotronFlute", new Note(Key.A, 2)),
                        new Sample("MellotronFlute", new Note(Key.A, 3)),
                        new Sample("MellotronFlute", new Note(Key.A, 4)),
                        new Sample("MellotronFlute", new Note(Key.D, 3)),
                        new Sample("MellotronFlute", new Note(Key.D, 4)),
                        new Sample("MellotronFlute", new Note(Key.D, 5))
                    }
                });
            }
        }
    }
}