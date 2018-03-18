using NUnit.Framework;
using MPCIC.Core;
using System.Collections.Generic;

namespace MPCIC.Tests.Core
{
    [TestFixture]
    public class SampleTests
    {
        public class SampleEquivalence
        {
            public SampleEquivalence(Note.IParser parser, string filePath, Sample sample)
            {
                Parser = parser;
                FilePath = filePath;
                Sample = sample;
            }

            public Note.IParser Parser;

            public string FilePath;

            public Sample Sample;

            public override string ToString()
            {
                return string.Format("{0} ({1})", FilePath, Parser);
            }
        }

        public static SampleEquivalence[] Equivalences =
        {
            new SampleEquivalence(Note.MidiNoteParser.Instance, "aa/Label12.wav", new Sample("Label", new Note(12))),
            new SampleEquivalence(Note.MidiNoteParser.Instance, "aa/Label_12.wav", new Sample("Label", new Note(12))),
            new SampleEquivalence(Note.LetterNoteParser.Instance, "bb/LabelC#2.wav", new Sample("Label", new Note(Key.Db, 2))),
            new SampleEquivalence(Note.LetterNoteParser.Instance, "bb/LabelC2.wav", new Sample("Label", new Note(Key.C, 2))),
            new SampleEquivalence(Note.LetterNoteParser.Instance, "bb/LabelC#2.WAV", new Sample("Label", new Note(Key.Db, 2))),
            new SampleEquivalence(Note.LetterNoteParser.Instance, "bb/LabelC2.WAV", new Sample("Label", new Note(Key.C, 2))),
            new SampleEquivalence(Note.LetterNoteParser.Instance, "bb/Label_C2.WAV", new Sample("Label", new Note(Key.C, 2))),
        };

        [Test]
        [TestCaseSource("Equivalences")]
        public static void ConstructorTests(SampleEquivalence equivalence)
        {
            
            Sample sample = Sample.FromFilePath(equivalence.FilePath);

            Assert.That(sample, Is.Not.Null);
            Assert.That(sample.RootNote, Is.EqualTo(equivalence.Sample.RootNote));
            Assert.That(sample.Label, Is.EqualTo(equivalence.Sample.Label));
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void ParserCandidatesTests(SampleEquivalence equivalence)
        {
            // When getting all the candidates for a file
            Dictionary<Note.IParser, Sample> candidates = Sample.CandidatesFromFilePath(equivalence.FilePath);

            // Then the corresponding parser caught the file
            Assert.That(candidates, Contains.Key(equivalence.Parser));

            // And its note and label were correctly assigned
            Sample candidate = candidates[equivalence.Parser];
            Assert.That(candidate.RootNote, Is.EqualTo(equivalence.Sample.RootNote));
            Assert.That(candidate.Label, Is.EqualTo(equivalence.Sample.Label));
        }

        [Test]
        [TestCase("Label12.mp3")]
        [TestCase("kick.wav")]
        public static void IsNotSampleTest(string invalidFile)
        {
            Sample sample = Sample.FromFilePath(invalidFile);
            
            Assert.That(sample, Is.Null);
        }
    }
}