using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IcoCurType.Util;

internal class EOIcoCurWriter {
	public enum IcoCurType {
		Icon = 1,
		Cursor
	}

	private struct IcoHeader {
		public ushort Reserved;
		public ushort Type;
		public ushort Count;

		public IcoHeader(IcoCurType type) {
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

		public void WriteToStream(Stream m_outS) {
			EOStreamUtility.Write_ushort(m_outS, Reserved);
			EOStreamUtility.Write_ushort(m_outS, Type);
			EOStreamUtility.Write_ushort(m_outS, Count);
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

		public void WriteToStream(Stream m_outS) {
			m_outS.WriteByte(bWidth);
			m_outS.WriteByte(bHeight);
			m_outS.WriteByte(bColorCount);
			m_outS.WriteByte(bReserved);
			EOStreamUtility.Write_ushort(m_outS, Planes_XHotspot);
			EOStreamUtility.Write_ushort(m_outS, BitCount_YHotspot);
			EOStreamUtility.Write_uint(m_outS, dwBytesInRes);
			EOStreamUtility.Write_uint(m_outS, dwImageOffset);
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

		public void WriteToStream(Stream m_outS) {
			EOStreamUtility.Write_uint(m_outS, StructSize);
			EOStreamUtility.Write_uint(m_outS, (uint)Width);
			EOStreamUtility.Write_uint(m_outS, (uint)Height);
			EOStreamUtility.Write_ushort(m_outS, Planes);
			EOStreamUtility.Write_ushort(m_outS, BitCount);
			EOStreamUtility.Write_uint(m_outS, biCompression);
			EOStreamUtility.Write_uint(m_outS, biSizeImage);
			EOStreamUtility.Write_uint(m_outS, (uint)biXPelsPerMeter);
			EOStreamUtility.Write_uint(m_outS, (uint)biYPelsPerMeter);
			EOStreamUtility.Write_uint(m_outS, biClrUsed);
			EOStreamUtility.Write_uint(m_outS, biClrImportant);
		}
	}

	private long m_StreamStart;
	private int m_NumWritten;
	private DirectoryEntry[] m_Entries;
	private Stream m_outS;
	private IcoCurType m_type;
	public string ErrorMsg;

	public EOIcoCurWriter(Stream outputStream, int imageCount, IcoCurType type) {
		if (!outputStream.CanSeek) {
			throw new ArgumentException("Icon/cursor output stream does not support seeking. A stream that supports seeking is required to write icon and cursor data.");
		}
		m_StreamStart = outputStream.Position;
		m_outS = outputStream;
		m_type = type;
		IcoHeader icoHeader = new IcoHeader(type);
		icoHeader.Count = (ushort)imageCount;
		icoHeader.WriteToStream(m_outS);
		m_NumWritten = 0;
		m_Entries = new DirectoryEntry[imageCount];
	}

	private unsafe void MakeMask(Bitmap AlphaImg, ref byte[] maskdata, int MaskRowSize) {
		int num = MaskRowSize * 8;
		int width = AlphaImg.Width;
		int height = AlphaImg.Height;
		Rectangle rect = new Rectangle(0, 0, width, height);
		BitmapData bitmapData = AlphaImg.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		for (int i = 0; i < height; i++) {
			byte* ptr = (byte*)bitmapData.Scan0.ToPointer() + (nint)bitmapData.Stride * (nint)i + 3;
			int num2 = num * (height - 1 - i);
			for (int j = 0; j < width; j++) {
				if (*ptr > 127) {
					BooleanBitArray.SetMSbFirst(maskdata, num2, value: false);
				} else {
					BooleanBitArray.SetMSbFirst(maskdata, num2, value: true);
				}
				num2++;
				ptr += 4;
			}
		}
		AlphaImg.UnlockBits(bitmapData);
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

	public unsafe bool WriteBitmap(Bitmap img, Bitmap AlphaImgMask, Point hotSpot) {
		long num = IcoHeader.GetStructSize() + DirectoryEntry.GetStructSize() * m_Entries.Length;
		if (img == null || img.Width <= 0 || img.Height <= 0) {
			ErrorMsg = "Invalid image passed to \"WriteBitmap\".";
			return false;
		}
		MemoryStream memoryStream = null;
		if (img.Width >= 256 || img.Height >= 256) {
			memoryStream = new MemoryStream();
			img.Save(memoryStream, ImageFormat.Png);
		}
		int width = img.Width;
		int height = img.Height;
		int bitsPerPixel = EvanBitmap.GetBitsPerPixel(img);
		int num2 = SizeComp(width, height, bitsPerPixel);
		int num3 = width / 8;
		int num4 = width * height / 8;
		if (num3 % 4 != 0) {
			int num5 = 4 - num3 % 4;
			num3 += num5;
			num4 += num5 * height;
		}
		if (m_NumWritten != 0) {
			num = m_Entries[m_NumWritten - 1].dwImageOffset + m_Entries[m_NumWritten - 1].dwBytesInRes;
		}
		m_Entries[m_NumWritten].bWidth = (byte)((width < 256) ? ((byte)width) : 0);
		m_Entries[m_NumWritten].bHeight = (byte)((height < 256) ? ((byte)height) : 0);
		m_Entries[m_NumWritten].bColorCount = 0;
		m_Entries[m_NumWritten].bReserved = 0;
		m_Entries[m_NumWritten].Planes_XHotspot = 1;
		m_Entries[m_NumWritten].BitCount_YHotspot = (ushort)bitsPerPixel;
		if (IcoCurType.Cursor == m_type) {
			m_Entries[m_NumWritten].Planes_XHotspot = (ushort)hotSpot.X;
			m_Entries[m_NumWritten].BitCount_YHotspot = (ushort)hotSpot.Y;
		}
		m_Entries[m_NumWritten].dwBytesInRes = (uint)(BITMAPINFOHEADER.GetStructSize() + ((bitsPerPixel == 8) ? 1024 : 0) + num2 + num4);
		if (memoryStream != null) {
			m_Entries[m_NumWritten].dwBytesInRes = (uint)memoryStream.Length;
		}
		m_Entries[m_NumWritten].dwImageOffset = (uint)num;
		long offset = m_StreamStart + IcoHeader.GetStructSize() + DirectoryEntry.GetStructSize() * m_NumWritten;
		m_outS.Seek(offset, SeekOrigin.Begin);
		m_Entries[m_NumWritten].WriteToStream(m_outS);
		m_outS.Seek(m_StreamStart + num, SeekOrigin.Begin);
		if (memoryStream != null) {
			m_outS.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
		} else {
			new BITMAPINFOHEADER(width, height * 2, bitsPerPixel).WriteToStream(m_outS);
			if (8 == bitsPerPixel) {
				ColorPalette palette = img.Palette;
				int num6 = palette.Entries.Length;
				for (int i = 0; i < num6; i++) {
					EOStreamUtility.WriteBGRAColor(m_outS, palette.Entries[i]);
				}
			}
			img.RotateFlip(RotateFlipType.Rotate180FlipX);
			Rectangle rect = new Rectangle(0, 0, width, height);
			BitmapData bitmapData = img.LockBits(rect, ImageLockMode.ReadOnly, img.PixelFormat);
			EOStreamUtility.WriteRaw(m_outS, bitmapData.Scan0.ToPointer(), num2);
			img.UnlockBits(bitmapData);
			img.RotateFlip(RotateFlipType.Rotate180FlipX);
			Bitmap alphaImg = ((AlphaImgMask == null) ? img : AlphaImgMask);
			byte[] maskdata = new byte[num4];
			MakeMask(alphaImg, ref maskdata, num3);
			m_outS.Write(maskdata, 0, num4);
		}
		m_NumWritten++;
		return true;
	}
}
