using System.Collections.Generic;
using System.IO;

namespace IcoCurType.Util;

internal class EvanRIFFFormat {
	public class Chunk {
		public int ID;
		public int Size;
		public int HeaderID;
		public long StreamOffset;
		public List<Chunk> Children;

		public Chunk() {
			ID = 0;
			Size = 0;
			HeaderID = 0;
			StreamOffset = 0L;
		}

		public long DataOffset() {
			return StreamOffset + 8;
		}

		public string GetStringID() {
			return IntToString(ID);
		}

		public string GetStringHeaderID() {
			if (HeaderID == 0) {
				return null;
			}
			return IntToString(HeaderID);
		}

		private string IntToString(int i) {
			return new string(new char[4]
			{
				(char)((uint)i & 0xFFu),
				(char)((uint)(i >> 8) & 0xFFu),
				(char)((uint)(i >> 16) & 0xFFu),
				(char)((uint)(i >> 24) & 0xFFu)
			});
		}
	}

	public Chunk MasterChunk;

	public EvanRIFFFormat() {
		MasterChunk = null;
	}

	private void AddAllChunks(List<Chunk> list, Chunk chunk, int ChunkID) {
		if (chunk.Children == null) {
			return;
		}
		for (int i = 0; i < chunk.Children.Count; i++) {
			if (chunk.Children[i].ID == ChunkID) {
				list.Add(chunk.Children[i]);
			}
			AddAllChunks(list, chunk.Children[i], ChunkID);
		}
	}

	public List<Chunk> FindAllChunks(int ChunkID) {
		List<Chunk> list = new List<Chunk>();
		AddAllChunks(list, MasterChunk, ChunkID);
		return list;
	}

	public Chunk FindFirstChunk(int ChunkID) {
		if (MasterChunk == null) {
			return null;
		}
		return FindInChunk(MasterChunk, ChunkID);
	}

	private Chunk FindInChunk(Chunk chunk, int ChunkID) {
		if (chunk.Children == null) {
			return null;
		}
		for (int i = 0; i < chunk.Children.Count; i++) {
			if (chunk.Children[i].ID == ChunkID) {
				return chunk.Children[i];
			}
			Chunk chunk2 = FindInChunk(chunk.Children[i], ChunkID);
			if (chunk2 != null) {
				return chunk2;
			}
		}
		return null;
	}

	private void GetLISTChildChunks(Chunk chnk, Stream s) {
		long position = s.Position;
		s.Seek(chnk.StreamOffset + 12, SeekOrigin.Begin);
		chnk.Children = new List<Chunk>();
		while (s.Position < chnk.StreamOffset + chnk.Size + 8) {
			Chunk chunk = new Chunk();
			chunk.StreamOffset = s.Position;
			chunk.ID = EOStreamUtility.ReadInt(s);
			chunk.Size = EOStreamUtility.ReadInt(s);
			chnk.Children.Add(chunk);
			s.Seek(chunk.Size, SeekOrigin.Current);
		}
		s.Seek(position, SeekOrigin.Begin);
	}

	public int InitFromStream(Stream s) {
		long position = s.Position;
		int num = 0x46464952;
		int num2 = EOStreamUtility.ReadInt(s);
		if (num2 != num) {
			return -1;
		}
		MasterChunk = new Chunk();
		MasterChunk.ID = num2;
		MasterChunk.StreamOffset = position;
		MasterChunk.Size = EOStreamUtility.ReadInt(s);
		MasterChunk.HeaderID = EOStreamUtility.ReadInt(s);
		MasterChunk.Children = new List<Chunk>();
		int num3 = 0x5453494c;
		while (s.Position < position + MasterChunk.Size + 8 && s.Position < s.Length) {
			Chunk chunk = new Chunk();
			chunk.StreamOffset = s.Position;
			chunk.ID = EOStreamUtility.ReadInt(s);
			chunk.Size = EOStreamUtility.ReadInt(s);
			if (chunk.ID == num3) {
				chunk.HeaderID = EOStreamUtility.ReadInt(s);
				GetLISTChildChunks(chunk, s);
			}
			MasterChunk.Children.Add(chunk);
			if (chunk.ID == num3) {
				s.Seek(chunk.Size - 4, SeekOrigin.Current);
			} else {
				s.Seek(chunk.Size, SeekOrigin.Current);
			}
		}
		return 0;
	}
}
