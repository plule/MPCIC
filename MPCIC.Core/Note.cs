using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MPCIC.Core
{
    /// <summary>
    /// Musical note.
    /// </summary>
    public class Note
    {
        // 2, 5 (midi note) or A, A#7
        public const string Pattern = @"((1[0-2]\d|\d\d|\d)|(([A-G])#?b?(10|[0-9])))";

        public Note(int midiNumber)
        {
            MidiNumber = midiNumber;
        }

        public Note(Key key, int octave)
        {
            MidiNumber = KeyOctaveToMidiNumber(key, octave);
        }

        public Note()
        {
            MidiNumber = 0;
        }

        public static implicit operator int(Note n)
        {
            return n.MidiNumber;
        }

        public static implicit operator Note(int n)
        {
            return new Note(n);
        }

        #region Parsers

        public static List<IParser> Parsers = new List<IParser> { MidiNoteParser.Instance, LetterNoteParser.Instance};

        public interface IParser
        {
            bool TryParse(string noteStr, out Note note);

            string Pattern { get; }
        }

        public class MidiNoteParser : IParser
        {
            public static readonly MidiNoteParser Instance = new MidiNoteParser();

            public string Pattern { get { return @"1[0-2]\d|\d\d|\d"; } }

            public bool TryParse(string noteStr, out Note note)
            {
                note = null;
                int midiNote;
                if(!int.TryParse(noteStr, out midiNote))
                    return false;

                if(midiNote < 0 || midiNote > 127)
                    return false;
                
                note = new Note(midiNote);
                return true;
            }
        }

        public class LetterNoteParser : IParser
        {
            public static readonly LetterNoteParser Instance = new LetterNoteParser();
            public string Pattern { get { return @"([A-G])(#?b?)(10|[0-9])"; } }

            public bool TryParse(string noteStr, out Note note)
            {
                Match match = Regex.Match(noteStr, Pattern);
                note = null;

                if(!match.Success)
                    return false;

                string letterStr = match.Groups[1].Value;
                string modifierStr = match.Groups[2].Value;
                string octaveStr = match.Groups[3].Value;

                Key key;
                if(!Enum.TryParse(letterStr, out key))
                    return false;
                
                switch(modifierStr)
                {
                    case "#":
                        key = key + 1;
                        break;
                    case "b":
                        key = key - 1;
                        break;
                }

                int octave;
                if(!int.TryParse(octaveStr, out octave) || octave > 10)
                    return false;

                note = new Note(key, octave);
                return true;
            }
        }

        #endregion

        /// <summary>
        /// Build from a string. The string must be a valid note.
        /// </summary>
        /// <param name="noteStr"></param>
        /// <returns></returns>
        public static Note FromString(string noteStr)
        {
            foreach(IParser parser in Parsers)
            {
                if(parser.TryParse(noteStr, out Note note))
                    return note;
            }

            return null;
        }

        public int MidiNumber { get; set; }

        public Key KeyValue
        {
            get
            {
                return MidiNumberToKey(MidiNumber);
            }
        }

        public int Octave
        {
            get
            {
                return MidiNumberToOctave(MidiNumber);
            }
        }

        public static int KeyOctaveToMidiNumber(Key key, int octave)
        {
            return octave * _octaveSize + (int)key;
        }

        public static Key MidiNumberToKey(int midiNumber)
        {
            return (Key) (midiNumber % _octaveSize);
        }

        public static int MidiNumberToOctave(int midiNumber)
        {
            return midiNumber / _octaveSize;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Note;

            if (item == null)
            {
                return false;
            }

            return MidiNumber.Equals(item.MidiNumber);
        }

        public override int GetHashCode()
        {
            return MidiNumber.GetHashCode();
        }

        public override string ToString()
        {
            //return MidiNumber.ToString();
            return string.Format("{0}{1}", KeyValue, Octave);
        }

        private const int _octaveSize = 12;
    }
}
