using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tileRead.Datastructures;
using tileRead.Tools;

namespace tileRead.Tools
{
    public class Parser
    {
        public static void Main(string[] args)
        {
            DebugLog.resetLog();
            DebugLog.setDebug(true);
            Entry[] entries = Parser.Parse(args[0]);
        }
        public static Entry[] Parse(string filePath)
        {
            FileStructure.addFileToDirectory(filePath);
            FileStream stream = File.OpenRead(filePath);
            StreamReader reader = new StreamReader(stream);
            string line = null;
            //removing the headers
            reader.ReadLine();
            //reading the file line at a time
            List<Entry> entries = new List<Entry>();
            HashSet<string> hashes = new HashSet<string>();
            line = reader.ReadLine();
            DebugLog.LogConsole("Parsing...");
            while( line != null && line!= "")
            {   
                string[] values = line.Split(',');
                //no need to dynamically check values length because there is only
                //one data set we are using this on

                //Also, we can assume that all values will parse properly
                //so no need to do any exception handling
                long timeStamp = long.Parse(values[0]);
                string hash = values[1];
                int xCoord = Int32.Parse(values[2]);
                int yCoord = Int32.Parse(values[3]);
                byte colourID = byte.Parse(values[4]);
                Entry e = new Entry(timeStamp, hash, xCoord, yCoord, colourID);
                entries.Add(e);
                hashes.Add(hash);
                line = reader.ReadLine();
            }
            DebugLog.LogConsole("Input parsed. " + entries.Count + " entries read.");
            DebugLog.LogConsole("Sorting...");
            entries.Sort(delegate (Entry t1, Entry t2) { return t1.time.CompareTo(t2.time); });
            DebugLog.LogConsole("Sorting finished.");
            //Now to write our binary to a file
            string outDir = FileStructure.parseDirectory;
            string outPath = Path.Combine(outDir, Path.GetFileNameWithoutExtension(filePath));
            DebugLog.LogConsole("Output path : " + outPath);
            FileStream outStream = File.Create(outPath+".bin");
            BinaryWriter outWriter = new BinaryWriter(outStream);

            Console.Out.WriteLine(entries.Count);
            outWriter.Write(entries.Count);
            for(int i = 0; i < entries.Count; i++)
            {
                entries[i].writeEntryToFile(outWriter);
            }
            List<String> hashls = hashes.ToList();
            DebugLog.LogConsole("File creating complete.");
            reader.Close();
            outWriter.Close();
            stream.Close();
            outStream.Close();
            return entries.ToArray() ;
        }
    }
}
