using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace IcoCurType.Util;

internal class EOIcoCurLoader {
	public enum Type {
		Type_Icon = 1,
		Type_Cursor
	}

	private struct IcoHeader {
		public ushort Reserved;
		public ushort Type;
		public ushort Count;

		public IcoHeader(Type type) {
			Reserved = 0;
			Type = (ushort)type;
			Count = 0;
		}

		public bool IsValid() {
			return Reserved == 0 && (Type == 1 || Type == 2);
		}

		public unsafe static int GetStructSize() {
			return sizeof(IcoHeader);
		}

		public void ReadFromStream(Stream InStream) {
			Reserved = EOStreamUtility.Read_ushort(InStream);
			Type = EOStreamUtility.Read_ushort(InStream);
			Count = EOStreamUtility.Read_ushort(InStream);
		}

		public void WriteToStream(Stream outS) {
			EOStreamUtility.Write_ushort(outS, Reserved);
			EOStreamUtility.Write_ushort(outS, Type);
			EOStreamUtility.Write_ushort(outS, Count);
		}
	}

	private struct DirectoryEntry {
		public byte bWidth;
		public byte bHeight;
		public byte bColorCount;
		public byte bReserved;
		public ushort Planes_XHotspot;
		public ushort BitCount_YHotspot;
		public uint dwBytesInRes;
		public uint dwImageOffset;

		public unsafe static int GetStructSize() {
			return sizeof(DirectoryEntry);
		}

		public void ReadFromStream(Stream InStream) {
			bWidth = (byte)InStream.ReadByte();
			bHeight = (byte)InStream.ReadByte();
			bColorCount = (byte)InStream.ReadByte();
			bReserved = (byte)InStream.ReadByte();
			Planes_XHotspot = EOStreamUtility.Read_ushort(InStream);
			BitCount_YHotspot = EOStreamUtility.Read_ushort(InStream);
			dwBytesInRes = EOStreamUtility.Read_uint(InStream);
			dwImageOffset = EOStreamUtility.Read_uint(InStream);
		}

		public void WriteToStream(Stream outS) {
			outS.WriteByte(bWidth);
			outS.WriteByte(bHeight);
			outS.WriteByte(bColorCount);
			outS.WriteByte(bReserved);
			EOStreamUtility.Write_ushort(outS, Planes_XHotspot);
			EOStreamUtility.Write_ushort(outS, BitCount_YHotspot);
			EOStreamUtility.Write_uint(outS, dwBytesInRes);
			EOStreamUtility.Write_uint(outS, dwImageOffset);
		}
	}

	private struct BITMAPINFOHEADER {
		public uint StructSize;
		public int Width;
		public int Height;
		public ushort Planes;
		public ushort BitCount;
		public uint biCompression;
		public uint biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public uint biClrUsed;
		public uint biClrImportant;

		public unsafe BITMAPINFOHEADER(int width, int height, int bpp) {
			StructSize = (uint)sizeof(BITMAPINFOHEADER);
			Width = width;
			Height = height;
			Planes = 1;
			BitCount = (ushort)bpp;
			biCompression = 0u;
			biSizeImage = (uint)(width * height * bpp / 8);
			biXPelsPerMeter = 0;
			biYPelsPerMeter = 0;
			biClrUsed = 0u;
			biClrImportant = 0u;
		}

		public unsafe static int GetStructSize() {
			return sizeof(BITMAPINFOHEADER);
		}

		public void ReadFromStream(Stream inS) {
			StructSize = EOStreamUtility.Read_uint(inS);
			Width = EOStreamUtility.ReadInt(inS);
			Height = EOStreamUtility.ReadInt(inS);
			Planes = EOStreamUtility.Read_ushort(inS);
			BitCount = EOStreamUtility.Read_ushort(inS);
			biCompression = EOStreamUtility.Read_uint(inS);
			biSizeImage = EOStreamUtility.Read_uint(inS);
			biXPelsPerMeter = EOStreamUtility.ReadInt(inS);
			biYPelsPerMeter = EOStreamUtility.ReadInt(inS);
			biClrUsed = EOStreamUtility.Read_uint(inS);
			biClrImportant = EOStreamUtility.Read_uint(inS);
		}

		public void WriteToStream(Stream outS) {
			EOStreamUtility.Write_uint(outS, StructSize);
			EOStreamUtility.Write_uint(outS, (uint)Width);
			EOStreamUtility.Write_uint(outS, (uint)Height);
			EOStreamUtility.Write_ushort(outS, Planes);
			EOStreamUtility.Write_ushort(outS, BitCount);
			EOStreamUtility.Write_uint(outS, biCompression);
			EOStreamUtility.Write_uint(outS, biSizeImage);
			EOStreamUtility.Write_uint(outS, (uint)biXPelsPerMeter);
			EOStreamUtility.Write_uint(outS, (uint)biYPelsPerMeter);
			EOStreamUtility.Write_uint(outS, biClrUsed);
			EOStreamUtility.Write_uint(outS, biClrImportant);
		}
	}

	private Stream m_stream;
	private long m_initialStreamPos;
	public string ErrorMsg;
	private Point HotSpot;

	public EOIcoCurLoader(Stream icoCurStream) {
		if (!icoCurStream.CanRead) {
			throw new ArgumentException("Cannot initialize EOIcoCurLoader with a stream that doesn't support reading");
		}
		m_stream = icoCurStream;
		m_initialStreamPos = m_stream.Position;
		ErrorMsg = "An unspecified error has occured";
	}

	public int CountImages() {
		long position = m_stream.Position;
		m_stream.Position = m_initialStreamPos;
		byte[] array = new byte[6];
		try {
			m_stream.Read(array, 0, 6);
		} catch (Exception ex) {
			ErrorMsg = "Could not get 6 bytes from the beginning of the stream. The following exception was generated:\r\n" + ex.ToString();
			return -1;
		}
		if (array[0] != 0 || array[1] != 0 || (array[2] != 1 && array[2] != 2) || array[3] != 0) {
			return -2;
		}
		int result = array[4] + array[5] * 256;
		m_stream.Seek(position, SeekOrigin.Begin);
		return result;
	}

	public unsafe Bitmap GetImage(uint ImageIndex) {
		Bitmap bitmap = null;
		m_stream.Position = m_initialStreamPos;
		IcoHeader icoHeader = default(IcoHeader);
		icoHeader.ReadFromStream(m_stream);
		if (ImageIndex >= icoHeader.Count) {
			ErrorMsg = $"Invalid image index of {ImageIndex} was passed to GetImage";
			return null;
		}
		DirectoryEntry[] array = new DirectoryEntry[icoHeader.Count];
		for (int i = 0; i < icoHeader.Count; i++) {
			array[i].ReadFromStream(m_stream);
		}
		HotSpot = new Point(array[(uint)(UIntPtr)ImageIndex].Planes_XHotspot, array[(uint)(UIntPtr)ImageIndex].BitCount_YHotspot);
		if (m_initialStreamPos + array[(uint)(UIntPtr)ImageIndex].dwImageOffset > m_stream.Length) {
			throw new InvalidDataException("Directory entry is invalid. Image offset is outside of the bounds of the input stream.");
		}
		m_stream.Position = m_initialStreamPos + array[(uint)(UIntPtr)ImageIndex].dwImageOffset;
		uint num = EOStreamUtility.Read_uint(m_stream);
		if (0x474e5089 == num) {
			m_stream.Seek(-4L, SeekOrigin.Current);
			EOOffsetStream stream = new EOOffsetStream(m_stream);
			try {
				bitmap = new Bitmap(stream);
				bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
			} catch (ArgumentException) {
				return null;
			}
			return bitmap;
		}
		m_stream.Seek(-4L, SeekOrigin.Current);
		uint out_Width = 0u;
		uint out_Height = 0u;
		uint out_bpp = 0u;
		GetImageDimensions(ImageIndex, ref out_Width, ref out_Height, ref out_bpp);
		default(BITMAPINFOHEADER).ReadFromStream(m_stream);
		uint* ptr = stackalloc uint[256];
		if (out_bpp <= 8) {
			int num2 = 1 << (int)out_bpp;
			for (int j = 0; j < num2; j++) {
				ptr[j] = EOStreamUtility.Read_uint(m_stream);
			}
		}
		uint num3 = SizeComp(out_Width, out_Height, out_bpp);
		byte[] array2 = new byte[num3];
		m_stream.Read(array2, 0, (int)num3);
		fixed (byte* bits = array2) {
			bitmap = EvanBitmap.FromRawBitsbpp(bits, ptr, (int)out_Width, (int)out_Height, (int)out_bpp, PaddedTo32Bit: true);
		}
		if (bitmap != null && out_bpp != 32) {
			void* ptr2 = Marshal.AllocHGlobal((int)SizeComp(out_Width, out_Height, 1u)).ToPointer();
			EOStreamUtility.ReadRaw(m_stream, ptr2, (int)SizeComp(out_Width, out_Height, 1u));
			Bitmap bWMask = EvanBitmap.FromRawBitsBinary(ptr2, (int)out_Width, (int)out_Height, PaddedTo32Bit: true);
			Marshal.FreeHGlobal(new IntPtr(ptr2));
			EvanBitmap.MaskToAlpha(bitmap, bWMask);
		}
		return bitmap;
	}

	public bool GetImageDimensions(uint ImageIndex, ref uint out_Width, ref uint out_Height, ref uint out_bpp) {
		long position = m_stream.Position;
		m_stream.Position = m_initialStreamPos;
		IcoHeader icoHeader = default(IcoHeader);
		icoHeader.ReadFromStream(m_stream);
		if (ImageIndex >= icoHeader.Count) {
			ErrorMsg = $"Invalid image index passed to GetImageDimensions.\r\nImage index: {ImageIndex}\r\nAvailable image count: {icoHeader.Count}";
			return false;
		}
		DirectoryEntry[] array = new DirectoryEntry[icoHeader.Count];
		for (int i = 0; i < icoHeader.Count; i++) {
			array[i].ReadFromStream(m_stream);
		}
		long num = m_initialStreamPos + array[(uint)(UIntPtr)ImageIndex].dwImageOffset;
		try {
			m_stream.Seek(num, SeekOrigin.Begin);
		} catch (Exception) {
			ErrorMsg = $"Could not seek to appropriate position in icon stream data.\r\nThe file data may be truncated, inaccessible or invalid.\r\nAttempted seek position: {num}";
			m_stream.Seek(position, SeekOrigin.Begin);
			return false;
		}
		uint num2 = EOStreamUtility.Read_uint(m_stream);
		if (0x474e5089 == num2) {
			m_stream.Seek(-4L, SeekOrigin.Current);
			EOOffsetStream stream = new EOOffsetStream(m_stream);
			try {
				using Bitmap bitmap = new Bitmap(stream);
				out_Width = (uint)bitmap.Width;
				out_Height = (uint)bitmap.Height;
				out_bpp = PixelFormatTobpp(bitmap.PixelFormat);
			} catch (ArgumentException) {
				return false;
			}
			return true;
		}
		m_stream.Seek(-4L, SeekOrigin.Current);
		BITMAPINFOHEADER bITMAPINFOHEADER = default(BITMAPINFOHEADER);
		bITMAPINFOHEADER.ReadFromStream(m_stream);
		out_bpp = bITMAPINFOHEADER.BitCount;
		out_Width = array[(uint)(UIntPtr)ImageIndex].bWidth;
		out_Height = array[(uint)(UIntPtr)ImageIndex].bHeight;
		uint num3 = SizeComp(out_Width, out_Height, out_bpp) + SizeComp(out_Width, out_Height, 1u);
		if (out_Width == 0 && out_Height == 0) {
			out_Width = (uint)bITMAPINFOHEADER.Width;
			out_Height = (uint)(bITMAPINFOHEADER.Height / 2);
		} else if (num3 != array[(uint)(UIntPtr)ImageIndex].dwBytesInRes) {
			if (out_Width == 255) {
				out_Width = 256u;
			}
			if (out_Height == 255) {
				out_Height = 256u;
			}
		}
		if (out_Width == 0) {
			out_Width = 256u;
		}
		if (out_Height == 0) {
			out_Height = 256u;
		}
		m_stream.Seek(position, SeekOrigin.Begin);
		return true;
	}

	private PixelFormat PixelFormatFrombpp(uint bpp) {
		return bpp switch {
			1u => PixelFormat.Format1bppIndexed,
			4u => PixelFormat.Format4bppIndexed,
			8u => PixelFormat.Format8bppIndexed,
			15u => PixelFormat.Format16bppRgb555,
			16u => PixelFormat.Format16bppRgb565,
			24u => PixelFormat.Format24bppRgb,
			32u => PixelFormat.Format32bppArgb,
			_ => PixelFormat.Undefined,
		};
	}

	private uint PixelFormatTobpp(PixelFormat pf) {
		return pf switch {
			PixelFormat.Format16bppRgb555 => 15u,
			PixelFormat.Format16bppRgb565 => 16u,
			PixelFormat.Format24bppRgb => 24u,
			PixelFormat.Format1bppIndexed => 1u,
			PixelFormat.Format4bppIndexed => 4u,
			PixelFormat.Format8bppIndexed => 8u,
			PixelFormat.Format32bppArgb => 32u,
			_ => 32u,
		};
	}

	private int SizeComp(int w, int h, int bpp) {
		int num = w * bpp / 8;
		if (num % 4 != 0) {
			num += 4 - num % 4;
		}
		return h * num;
	}

	private uint SizeComp(uint w, uint h, uint bpp) {
		uint num = w * bpp / 8u;
		if (num % 4u != 0) {
			num += 4 - num % 4u;
		}
		return h * num;
	}
}
