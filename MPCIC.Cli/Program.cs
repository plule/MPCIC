using System;
using MPCIC.Core;

namespace MPCIC.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SampleCollection collection = SampleCollection.FromFilesInDirectory(args);
                foreach(Sample sample in collection.Samples)
                {
                    Console.WriteLine(string.Format("{0}:\t{1}\t{2}\t{3}", sample.FileName, sample.LowNote, sample.RootNote, sample.HighNote));
                }

                collection.DoIt();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.WriteLine("Press any key.");
            Console.Read();
        }
    }
}
