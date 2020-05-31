using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace MPCIC.Core
{
    /// <summary>
    /// A collection of samples to be converted to an instrument.
    /// </summary>
    public class SampleCollection
    {
        public string Name { get; set; }

        public List<Sample> Samples;

        public string DirectoryPath { get; set; }

        public SampleCollection()
        {
            Samples = new List<Sample>();
        }

        public SampleCollection(string name, List<Sample> samples)
        {
            if(samples.Count == 0)
                throw new ArgumentException("Empty list of samples.", "samples");

            bool moreThanOneDirectory = samples
                .GroupBy(sample => sample.DirectoryPath)
                .Count() > 1;
            
            if(moreThanOneDirectory)
            {
                throw new ArgumentException("All the samples of a collection must be in the same directory.", "samples");
            }

            Name = name;
            Samples = samples.OrderBy(sample => sample.RootNote.MidiNumber).ToList();
            DirectoryPath = Samples.First().DirectoryPath;
        }

        public SampleCollection(List<Sample> samples)
            : this("", samples)
        {
            bool allSameLabel = Samples
                .GroupBy(sample => sample.Label)
                .Count() == 1 && !string.IsNullOrWhiteSpace(Samples.First().Label);

            Name = allSameLabel ? Samples.First().Label : Samples.First().DirectoryName;
            AssignRanges();
        }

        /// <summary>
        /// Assign the range of each sample.
        /// </summary>
        public void AssignRanges()
        {
            if(Samples.Count == 0)
                return;

            Sample previous = null;
            foreach(Sample sample in Samples.OrderBy(sample => sample.RootNote.MidiNumber))
            {
                if(previous != null)
                {
                    int middle = (previous.RootNote + sample.RootNote) / 2;

                    previous.HighNote = middle;
                    sample.LowNote = middle + 1;
                }

                previous = sample;
            }

            // Adjust lower LowNote and higher HighNote
            if(Samples.Count > 2)
            {
                Sample reference = Samples[1];
                Sample first = Samples[0];
                Sample last = Samples.Last();
                int referenceRange = reference.HighNote - reference.LowNote;
                first.LowNote = first.RootNote - referenceRange / 2;
                last.HighNote = last.RootNote + referenceRange / 2;
            }
            else
            {
                Samples.First().LowNote = 0;
                Samples.Last().HighNote = 127;
            }
        }

        public void DoIt()
        {
            XmlDocument xprogramDoc = XPMReference.GetXPMReference();

            XmlNode xprogram = xprogramDoc["MPCVObject"]["Program"];

            xprogram["ProgramName"].InnerText = Name;
            xprogram["KeygroupNumKeygroups"].InnerText = Samples.Count.ToString();

            XmlNode xkeygroups = xprogram["Instruments"];
            XmlNode xreferencekeygroup = xkeygroups.FirstChild.Clone();
            bool isFirst = true;
            int kgNumber = 1;
            foreach(Sample sample in Samples)
            {
                XmlNode xkeyGroup;
                // In the reference, there is only one keygroup.
                if(isFirst)
                {
                    xkeyGroup = xkeygroups.FirstChild;
                    isFirst = false;
                }
                else
                {
                    xkeyGroup = xreferencekeygroup.Clone();
                    xkeygroups.AppendChild(xkeyGroup);
                }

                xkeyGroup["LowNote"].InnerText = (sample.LowNote.MidiNumber + 24).ToString();
                xkeyGroup["HighNote"].InnerText = (sample.HighNote.MidiNumber + 24).ToString();
                xkeyGroup.Attributes["number"].InnerText = kgNumber.ToString();

                XmlNode xlayer = xkeyGroup["Layers"].ChildNodes[0];
                xlayer["RootNote"].InnerText = (sample.RootNote.MidiNumber + 25).ToString();
                xlayer["SampleName"].InnerText = Path.GetFileNameWithoutExtension(sample.FileName);
                xlayer["SampleFile"].InnerText = sample.FileName;

                kgNumber++;
            }

            string xprogramFileName = Path.ChangeExtension(Name, "xpm");
            string xprogramFilePath = Path.Combine(DirectoryPath, xprogramFileName);

            xprogramDoc.Save(xprogramFilePath);
            
            
            // xprogram["MPCVObject"]["Program"]["Instruments"].ChildNodes[0]["Layers"].ChildNodes[0]["SampleName"].InnerText = "";

        }

        /// <summary>
        /// Create collections from a set of files in the same directory.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public static SampleCollection FromFilesInDirectory(IEnumerable<string> filePaths)
        {
            if(!filePaths.Any())
                throw new ArgumentException("No files given.", "filePaths");

            IEnumerable<Dictionary<Note.IParser, Sample>> parsedPath = filePaths
                .Select(filePath => Sample.CandidatesFromFilePath(filePath));

            Dictionary<Note.IParser, List<Sample>> dict = new Dictionary<Note.IParser, List<Sample>>();

            foreach(var dic in parsedPath)
            {
                foreach(var kvp in dic)
                {
                    if(!dict.ContainsKey(kvp.Key))
                        dict.Add(kvp.Key, new List<Sample>());

                    dict[kvp.Key].Add(kvp.Value);
                }
            }

            var fullParsers = dict.Where(kvp => kvp.Value.Count == filePaths.Count());

            // No parser manage to parse all the entries. Fail.
            if(fullParsers.Count() == 0)
            {
                return null;
            }

            // Only one parser managed to parse all the entries.
            if(fullParsers.Count() == 1)
            {
                return new SampleCollection(fullParsers.First().Value);
            }

            // More than one parser managed to parse all the entries.
            // Take the one with the shorter label
            int minLength = int.MaxValue;
            List<Sample> samples = null;
            foreach(var kvp in fullParsers)
            {
                if(kvp.Value.First().Label.Length < minLength)
                {
                    minLength = kvp.Value.First().Label.Length;
                    samples = kvp.Value;
                }
            }

            return new SampleCollection(samples);
        }

        public static SampleCollection FromDirectory(string directory)
        {
            return FromFilesInDirectory(
                Directory.EnumerateFiles(directory).Where(p => Path.GetExtension(p).ToLower() == ".wav")

            );
        }
    }
}