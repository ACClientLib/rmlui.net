using System;
using System.IO;
using UnityEngine;
using RmlUiNet;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace URmlUi
{
	public class UnityFileInterface : FileInterface
	{
		private readonly string BasePath = Path.Combine(Application.streamingAssetsPath, "rml");
		private List<FileStream> m_Streams = new List<FileStream>();

		public override void Close(ulong file)
		{
			Assert.IsTrue((int)file > 0, "Invalid FileHandle!");
			Assert.IsNotNull(m_Streams[(int)file - 1], "Invalid FileHandle!");

			m_Streams[(int)file - 1].Dispose();
			m_Streams[(int)file - 1] = null;
		}

		public override ulong Length(ulong file)
		{
			Assert.IsTrue((int)file > 0, "Invalid FileHandle!");
			Assert.IsNotNull(m_Streams[(int)file - 1], "Invalid FileHandle!");

			return (ulong)m_Streams[(int)file - 1].Length;
		}

		public override string LoadFile(string path)
		{
			// Swap separators if we determine that we're using the wrong ones
			if (!path.Contains(Path.DirectorySeparatorChar) && path.Contains(Path.AltDirectorySeparatorChar))
				path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			if (!File.Exists(Path.Combine(BasePath, path)))
			{
				Debug.LogError($"File {path} doesn't exist within RmlUi files!");
				return "";
			}

			return File.ReadAllText(Path.Combine(BasePath, path));
		}

		public override ulong Open(string path)
		{
			// Swap separators if we determine that we're using the wrong ones
			if (!path.Contains(Path.DirectorySeparatorChar) && path.Contains(Path.AltDirectorySeparatorChar))
				path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			if (!File.Exists(Path.Combine(BasePath, path)))
			{
				Debug.LogError($"File {path} doesn't exist within RmlUi files!");
				return 0;
			}

			// check if we have an open position within the list
			var openIndex = m_Streams.FindIndex((e) => e == null);
			if (openIndex != -1)
			{
				m_Streams[openIndex] = File.Open(Path.Combine(BasePath, path), FileMode.Open, FileAccess.Read);
				return (ulong)openIndex + 1; // this skips over zero
			}
			else
			{
				m_Streams.Add(File.Open(Path.Combine(BasePath, path), FileMode.Open, FileAccess.Read));
				return (ulong)m_Streams.Count;
			}	
		}

		public override ulong Read(byte[] buffer, ulong size, ulong file)
		{
			Assert.IsTrue((int)file > 0, "Invalid FileHandle!");
			Assert.IsNotNull(m_Streams[(int)file - 1], "Invalid FileHandle!");

			return (ulong)m_Streams[(int)file - 1].Read(buffer, (int)m_Streams[(int)file - 1].Position, (int)size);
		}

		public override bool Seek(ulong file, long offset, int origin)
		{
			Assert.IsTrue((int)file > 0, "Invalid FileHandle!");
			Assert.IsNotNull(m_Streams[(int)file - 1], "Invalid FileHandle!");

			m_Streams[(int)file - 1].Seek(offset, (SeekOrigin)origin);

			return true; // we can't really tell if it was successful or not?
		}

		public override ulong Tell(ulong file)
		{
			Assert.IsTrue((int)file > 0, "Invalid FileHandle!");
			Assert.IsNotNull(m_Streams[(int)file - 1], "Invalid FileHandle!");

			return (ulong)m_Streams[(int)file - 1].Position;
		}
	}
}
