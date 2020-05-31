using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPCIC.Core
{
    /// <summary>
    /// A sample file with a label and a root note.
    /// </summary>
    public class Sample
    {
        public static string Separators = "-_";

        /// <summary>
        /// Path of the sample file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Name of the sample file.
        /// </summary>
        /// <returns></returns>
        public string FileName { get { return Path.GetFileName(FilePath); } }

        /// <summary>
        /// Path of the directory of the sample file.
        /// </summary>
        /// <returns></returns>
        public string DirectoryPath { get { return Path.GetDirectoryName(FilePath); } }

        /// <summary>
        /// Name of the directory of the sample file.
        /// </summary>
        /// <returns></returns>
        public string DirectoryName { get { return Path.GetFileName(DirectoryPath); } }

        /// <summary>
        /// Root note of the sample.
        /// </summary>
        public Note RootNote { get; set; }

        /// <summary>
        /// Lowest applicable note for this sample.
        /// </summary>
        public Note LowNote { get; set; }

        /// <summary>
        /// Highest applicable note for this sample.
        /// </summary>
        public Note HighNote { get; set; }

        /// <summary>
        /// Label of the sample.
        /// </summary>
        public string Label {get; set;}

        public Sample()
        {
            LowNote = new Note(Key.C, 1);
            RootNote = new Note(Key.C, 2);
            HighNote = new Note(Key.C, 3);
            FilePath = "Note.wav";
        }

        public Sample(string label, Note rootNote)
        {
            Label = label;
            RootNote = rootNote;
        }

        public Sample(string label, Note rootNote, string filePath)
            : this(label, rootNote)
        {
            FilePath = filePath;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Sample other = (Sample)obj;
            return FilePath.Equals(other.FilePath) && RootNote.Equals(other.RootNote);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + FilePath.GetHashCode();
                hash = hash * 31 + RootNote.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", FileName, RootNote);
        }

        /// <summary>
        /// For all the note parser successfully parsing the path, return a sample build with it.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<Note.IParser, Sample> CandidatesFromFilePath(string filePath)
        {
            Dictionary<Note.IParser, Sample> candidates = new Dictionary<Note.IParser, Sample>();
            foreach(Note.IParser parser in Note.Parsers)
            {
                string fileName = Path.GetFileName(filePath);

                string pattern = string.Format("({0}).wav", parser.Pattern);
                Match match = Regex.Match(fileName, pattern, RegexOptions.IgnoreCase);

                if(!match.Success)
                    continue;

                string noteStr = match.Groups[1].Value;
                
                if(parser.TryParse(noteStr, out Note note))
                {
                    Sample sample = new Sample();

                    int suffixLength = noteStr.Length + ".wav".Length;

                    if(fileName.Length < suffixLength)
                        continue;

                    sample.FilePath = filePath;

                    sample.Label = fileName.Substring(0, fileName.Length - (".wav".Length + noteStr.Length))
                        .Trim()
                        .Trim(Separators.ToCharArray());

                    sample.RootNote = note;

                    candidates.Add(parser, sample);
                }
            }
            
            return candidates;
        }

        /// <summary>
        /// Build a Sample from a file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>null if the file could not be analyzed as a sample.</returns>
        public static Sample FromFilePath(string filePath)
        {
            Sample sample = new Sample();
            string fileName = Path.GetFileName(filePath);
            Match match = Regex.Match(fileName, Note.Pattern, RegexOptions.IgnoreCase);

            if(!match.Success)
            {
                return null;
            }

            string noteStr = match.Groups[1].Value;

            int suffixLength = noteStr.Length + ".wav".Length;

            if(fileName.Length < suffixLength)
                return null;

            if(!(Path.GetExtension(fileName).ToLower() == ".wav"))
                return null;

            sample.FilePath = filePath;

            sample.Label = fileName.Substring(0, fileName.Length - (".wav".Length + noteStr.Length))
                .Trim()
                .Trim(Separators.ToCharArray());

            sample.RootNote = Note.FromString(noteStr);

            return sample;
        }
    }
}