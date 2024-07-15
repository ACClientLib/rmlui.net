using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RmlUiNet;

namespace RmlUi.Net.TestApp
{
    public class BaseFileInterface : FileInterface
    {
        private readonly string BasePath = "rml";
        private List<FileStream> m_Streams = new List<FileStream>();

        public override void Close(ulong file)
        {
            if (m_Streams[(int)file] == null)
            {
                Console.WriteLine("ERROR: Invalid FileHandle!");
            }

            m_Streams[(int)file].Dispose();
            m_Streams[(int)file] = null;
        }

        public override ulong Length(ulong file)
        {
            if (m_Streams[(int)file] == null)
            {
                Console.WriteLine("ERROR: Invalid FileHandle!");
            }

            return (ulong)m_Streams[(int)file].Length;
        }

        public override string LoadFile(string path)
        {
            if (!File.Exists(Path.Combine(BasePath, path)))
            {
                Console.WriteLine($"ERR: File {path} doesn't exist within RmlUi files!");
                return "";
            }

            return File.ReadAllText(Path.Combine(BasePath, path));
        }

        public override ulong Open(string path)
        {
            Console.WriteLine(path);

            if (!File.Exists(Path.Combine(BasePath, path)))
            {
                Console.WriteLine($"ERR: File {path} doesn't exist within RmlUi files!");
                return 0;
            }

            // check if we have an open position within the list
            var openIndex = m_Streams.FindIndex((e) => e == null);
            if (openIndex != -1)
            {
                m_Streams[openIndex] = File.Open(Path.Combine(BasePath, path), FileMode.Open, FileAccess.Read);
                return (ulong)openIndex;
            }
            else
            {
                m_Streams.Add(File.Open(Path.Combine(BasePath, path), FileMode.Open, FileAccess.Read));
                return (ulong)m_Streams.Count - 1;
            }
        }

        public override ulong Read(out byte[] buffer, ulong size, ulong file)
        {
            if (m_Streams[(int)file] == null)
            {
                Console.WriteLine("ERROR: Invalid FileHandle!");
            }

            buffer = new byte[size];

            return (ulong)m_Streams[(int)file].Read(buffer, (int)m_Streams[(int)file].Position, (int)size);
        }

        public override bool Seek(ulong file, long offset, int origin)
        {
            if (m_Streams[(int)file] == null)
            {
                Console.WriteLine("ERROR: Invalid FileHandle!");
            }

            m_Streams[(int)file].Seek(offset, (SeekOrigin)origin);

            return true; // we can't really tell if it was successful or not?
        }

        public override ulong Tell(ulong file)
        {
            if (m_Streams[(int)file] == null)
            {
                Console.WriteLine("ERROR: Invalid FileHandle!");
            }

            return (ulong)m_Streams[(int)file].Position;
        }
    }
}
