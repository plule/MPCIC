using NUnit.Framework;
using MPCIC.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MPCIC.Tests.Core
{
    [TestFixture]
    public class NoteTests
    {
        public class NoteEquivalence
        {
            public NoteEquivalence(int midiNumber, Key key, int octave, string midiRepresentation, params string[] letterRepresentations)
            {
                MidiNumber = midiNumber;
                Key = key;
                Octave = octave;
                MidiRepresentation = midiRepresentation;
                LetterRepresentations = new List<string>(letterRepresentations);
                Representations = new List<string>(letterRepresentations);
                Representations.Add(MidiRepresentation);
            }

            public int MidiNumber { get; private set; }
            public Key Key { get; private set; }
            public int Octave { get; private set; }

            public string MidiRepresentation;

            public List<string> LetterRepresentations;

            public List<string> Representations;

            public override string ToString()
            {
                return string.Format("{0} {1}{2} ({3})", MidiNumber, Key, Octave, string.Join(", ", Representations));
            }
        }

        public static NoteEquivalence[] Equivalences =
        {
            new NoteEquivalence(0, Key.C, 0, "0", "C0"),
            new NoteEquivalence(25, Key.Db, 2, "25", "C#2", "Db2"),
            new NoteEquivalence(102, Key.Gb, 8, "102", "F#8", "Gb8"),
            new NoteEquivalence(127, Key.G, 10, "127", "G10")
        };

        [Test]
        [TestCaseSource("Equivalences")]
        public static void ConstructorTests(NoteEquivalence equivalence)
        {
            Assert.That(new Note(equivalence.MidiNumber), Is.EqualTo(new Note(equivalence.Key, equivalence.Octave)));
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void MidiNumberToKeyTest(NoteEquivalence equivalence)
        {
            Key key = Note.MidiNumberToKey(equivalence.MidiNumber);
            Assert.That(key, Is.EqualTo(equivalence.Key));
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void MidiNumberToOctaveTest(NoteEquivalence equivalence)
        {
            int octave = Note.MidiNumberToOctave(equivalence.MidiNumber);
            Assert.That(octave, Is.EqualTo(equivalence.Octave));
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void KeyOctaveToMidiNumberTest(NoteEquivalence equivalence)
        {
            int midiNumber = Note.KeyOctaveToMidiNumber(equivalence.Key, equivalence.Octave);
            Assert.That(midiNumber, Is.EqualTo(equivalence.MidiNumber));
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void CanBeConvertedFromStringForAllRepresentations(NoteEquivalence equivalence)
        {
            foreach(string representation in equivalence.Representations)
            {
                Note note = Note.FromString(representation);
                Assert.That(note.MidiNumber, Is.EqualTo(equivalence.MidiNumber));
            }
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void GlobalPatternMatchesTheValidPatterns(NoteEquivalence equivalence)
        {
            Regex r = new Regex(Note.Pattern);

            foreach(string representation in equivalence.Representations)
            {
                Assert.That(r.IsMatch(representation), Is.True);
            }
        }

        [Test]
        [TestCaseSource("Equivalences")]
        public static void ParsersSuccessfullyParseTheirNotes(NoteEquivalence equivalence)
        {
            Note midiParsed;

            Assert.That(Regex.IsMatch(equivalence.MidiRepresentation, Note.MidiNoteParser.Instance.Pattern));
            Assert.That(Note.MidiNoteParser.Instance.TryParse(equivalence.MidiRepresentation, out midiParsed), Is.True);
            Assert.That(midiParsed.MidiNumber, Is.EqualTo(equivalence.MidiNumber));

            foreach(string representation in equivalence.LetterRepresentations)
            {
                Note letterParsed;

                Assert.That(Regex.IsMatch(representation, Note.LetterNoteParser.Instance.Pattern));
                Assert.That(Note.LetterNoteParser.Instance.TryParse(representation, out letterParsed), Is.True);
                Assert.That(letterParsed.MidiNumber, Is.EqualTo(equivalence.MidiNumber));
            }
        }

        [Test]
        [TestCase("12A")]
        [TestCase("A#")]
        [TestCase("1000")]
        public static void GlobalPatternDoesNotMatchInvalidPatterns(string invalidPattern)
        {
            Regex r = new Regex(@"\b" + Note.Pattern + @"\b");

            Assert.That(r.IsMatch(invalidPattern), Is.False);
        }
    }
}