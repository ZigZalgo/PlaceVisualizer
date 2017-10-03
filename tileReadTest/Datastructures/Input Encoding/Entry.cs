using Newtonsoft.Json;
using System;
using System.IO;


namespace tileRead.Datastructures
{
    /// <summary>
    /// Although most other things a generic, Entry is not due to the fact that I am only working with a 
    /// singular data set. I admit I could make this general case for anything though, but that is more
    /// work than is worth for me right now. I may come back in the future and add that in though
    /// </summary>
    public class Entry
    {
        public long time;
        public string hash;
        public int x;
        public int y;
        public byte colourID;

        

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public Entry(long t, string user, int xc, int yc, byte c)
        {
            time = t;
            hash = user;
            x = xc;
            y = yc;
            colourID = c;
        }

        public void writeEntryToFile(BinaryWriter writer)
        {
            writer.Write(time);
            writer.Write(hash);
            writer.Write(x);
            writer.Write(y);
            writer.Write(colourID);
        }

        public static Entry readEntryFromFile(BinaryReader reader)
        {
            long ts = reader.ReadInt64();
            string hash = reader.ReadString();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            byte c = reader.ReadByte();
            return new Entry(ts, hash, x, y, c);
        }

        public static Entry[] readEntriesFromFile(string filePath)
        {
            if(Path.GetExtension(filePath) != ".bin")
            {
                throw new IOException("Cannot read file of type " + Path.GetExtension(filePath));
            }
            FileStream stream = File.OpenRead(filePath);
            BinaryReader reader = new BinaryReader(stream);
            int numToRead = reader.ReadInt32();
            Entry[] entries = new Entry[numToRead];
            for(int i = 0; i < numToRead; i++)
            {
                entries[i] = Entry.readEntryFromFile(reader);
            }
            return entries;
        } 
    }
}
