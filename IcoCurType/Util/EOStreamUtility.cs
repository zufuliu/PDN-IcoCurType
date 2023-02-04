using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace IcoCurType.Util;

internal static class EOStreamUtility {
	public unsafe static void LoadStreamDataToAllocatedBlock(Stream s, void* mem) {
		byte[] array = new byte[s.Length];
		s.Read(array, 0, (int)s.Length);
		int num = (int)s.Length;
		for (int i = 0; i < num; i++) {
			((byte*)mem)[i] = array[i];
		}
	}

	public unsafe static void ReadRaw(Stream s, void* mem, int ReadSize) {
		byte[] array = new byte[ReadSize];
		s.Read(array, 0, ReadSize);
		Marshal.Copy(array, 0, new IntPtr(mem), ReadSize);
	}

	public static int ReadInt(Stream inS) {
		byte[] array = new byte[4];
		inS.Read(array, 0, 4);
		return array[0] | (array[1] << 8) | (array[2] << 16) | (array[3] << 24);
	}

	public static uint Read_uint(Stream inS) {
		int num = inS.ReadByte();
		int num2 = inS.ReadByte();
		int num3 = inS.ReadByte();
		int num4 = inS.ReadByte();
		if (num == -1) {
			num2 = (num = (num3 = (num4 = 0)));
		} else if (num2 == -1) {
			num3 = (num2 = (num4 = 0));
		} else if (num3 == -1) {
			num4 = (num3 = 0);
		} else if (num4 == -1) {
			num4 = 0;
		}
		return (uint)(num | (num2 << 8) | (num3 << 16) | (num4 << 24));
	}

	public static uint[] ReadUInts(Stream s, int Count) {
		byte[] array = new byte[Count * 4];
		uint[] array2 = new uint[Count];
		s.Read(array, 0, Count * 4);
		for (int i = 0; i < Count; i++) {
			array2[i] = (uint)(array[i * 4] | array[i * 4 + 1] | array[i * 4 + 2] | array[i * 4 + 3]);
		}
		return array2;
	}

	public static ushort Read_ushort(Stream inS) {
		int num = inS.ReadByte();
		int num2 = inS.ReadByte();
		return (ushort)(num | (num2 << 8));
	}

	public static void WriteBGRAColor(Stream outS, Color BGRA) {
		outS.WriteByte(BGRA.B);
		outS.WriteByte(BGRA.G);
		outS.WriteByte(BGRA.R);
		outS.WriteByte(BGRA.A);
	}

	public unsafe static bool WriteRaw(Stream outS, void* Data, int Size) {
		try {
			for (int i = 0; i < Size; i++) {
				outS.WriteByte(((byte*)Data)[i]);
			}
		} catch (Exception) {
			return false;
		}
		return true;
	}

	public unsafe static bool WriteRaw(Stream outS, void* Data, uint Size) {
		try {
			for (uint num = 0u; num < Size; num++) {
				outS.WriteByte(((byte*)Data)[(int)num]);
			}
		} catch (Exception) {
			return false;
		}
		return true;
	}

	public static bool WriteString(Stream outS, string s, bool includeNullTerminator) {
		try {
			int length = s.Length;
			char[] array = s.ToCharArray();
			for (int i = 0; i < length; i++) {
				outS.WriteByte((byte)array[i]);
			}
			if (includeNullTerminator) {
				outS.WriteByte(0);
			}
		} catch (Exception) {
			return false;
		}
		return true;
	}

	public static void Write_ushort(Stream outS, ushort word) {
		outS.WriteByte((byte)(word & 0xFFu));
		outS.WriteByte((byte)(word >> 8));
	}

	public static void Write_uint(Stream outS, uint dword) {
		byte[] buffer = new byte[4]
		{
			(byte)dword,
			(byte)(dword >> 8),
			(byte)(dword >> 16),
			(byte)(dword >> 24)
		};
		outS.Write(buffer, 0, 4);
	}
}
