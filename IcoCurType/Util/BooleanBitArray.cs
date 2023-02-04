using System;

namespace IcoCurType.Util;

internal class BooleanBitArray {
	private int m_bitSize;
	private byte[] m_data;

	public bool this[int index] {
		get {
			return Get(index);
		}
		set {
			Set(m_data, index, value);
		}
	}

	public BooleanBitArray(int sizeInBits) {
		m_bitSize = sizeInBits;
		int num = m_bitSize / 8 + 1;
		if (m_bitSize % 8 == 0) {
			num = m_bitSize / 8;
		}
		m_data = new byte[num];
		if (m_data == null) {
			throw new OutOfMemoryException($"Could not allocate {num} bytes of memory for boolean bit array");
		}
		Array.Clear(m_data, 0, m_data.Length);
	}

	public bool Get(int bitIndex) {
		byte b = (byte)(1 << bitIndex % 8);
		return (m_data[bitIndex / 8] & b) != 0;
	}

	public unsafe static bool Get(byte* data, int BitIndex) {
		byte b = (byte)(1 << BitIndex % 8);
		return (data[BitIndex / 8] & b) != 0;
	}

	public static bool Get(byte[] data, int bitIndex) {
		byte b = (byte)(1 << bitIndex % 8);
		return (data[bitIndex / 8] & b) != 0;
	}

	public unsafe static void Set(byte* data, int BitIndex, bool value) {
		if (value) {
			byte b = (byte)(1 << BitIndex % 8);
			byte* ptr = data + BitIndex / 8;
			*ptr = (byte)(*ptr | b);
		} else {
			byte b2 = (byte)(~(byte)(1 << BitIndex % 8));
			byte* ptr2 = data + BitIndex / 8;
			*ptr2 = (byte)(*ptr2 & b2);
		}
	}

	public static void Set(byte[] data, int BitIndex, bool value) {
		if (value) {
			byte b = (byte)(1 << BitIndex % 8);
			int num = BitIndex / 8;
			data[num] |= b;
		} else {
			byte b2 = (byte)(~(byte)(1 << BitIndex % 8));
			int num2 = BitIndex / 8;
			data[num2] &= b2;
		}
	}

	public static void SetMSbFirst(byte[] data, int BitIndex, bool value) {
		if (value) {
			byte b = (byte)(1 << 7 - BitIndex % 8);
			int num = BitIndex / 8;
			data[num] |= b;
		} else {
			byte b2 = (byte)(~(byte)(1 << 7 - BitIndex % 8));
			int num2 = BitIndex / 8;
			data[num2] &= b2;
		}
	}
}
