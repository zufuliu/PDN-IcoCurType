using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace IcoCurType.Util;

internal static class EvanBitmap {
	public unsafe static void AddToAlpha(Bitmap bm, int A_Add) {
		Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (byte* ptr2 = ptr + (nint)(bm.Width * bm.Height) * (nint)4; ptr < ptr2; ptr += 4) {
			int num = ptr[3];
			num += A_Add;
			if (num < 0) {
				num = 0;
			} else if (num > 255) {
				num = 255;
			}
			ptr[3] = (byte)num;
		}
		bm.UnlockBits(bitmapData);
	}

	private static uint ComputeSize(uint w, uint h, uint bpp, bool PaddedTo32) {
		uint num = w * bpp / 8u;
		if (PaddedTo32 && num % 4u != 0) {
			num += 4 - num % 4u;
		}
		return h * num;
	}

	public unsafe static int CountTransparentColumnsFromLeft(Bitmap bm) {
		int width = bm.Width;
		int height = bm.Height;
		Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)((byte*)bitmapData.Scan0.ToPointer() + (nint)(width * height) * (nint)4);
		int i;
		for (i = 0; i < width; i++) {
			for (uint* ptr2 = (uint*)((byte*)bitmapData.Scan0.ToPointer() + (nint)i * (nint)4); ptr2 < ptr; ptr2 += width) {
				if ((*ptr2 & 0xFF000000u) != 0) {
					bm.UnlockBits(bitmapData);
					return i;
				}
			}
		}
		bm.UnlockBits(bitmapData);
		return i;
	}

	public unsafe static int CountTransparentColumnsFromRight(Bitmap bm) {
		int width = bm.Width;
		int height = bm.Height;
		Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)((byte*)bitmapData.Scan0.ToPointer() + (nint)(width * height) * (nint)4);
		for (int num = width - 1; num >= 0; num--) {
			for (uint* ptr2 = (uint*)((byte*)bitmapData.Scan0.ToPointer() + (nint)num * (nint)4); ptr2 < ptr; ptr2 += width) {
				if ((*ptr2 & 0xFF000000u) != 0) {
					bm.UnlockBits(bitmapData);
					return width - num - 1;
				}
			}
		}
		bm.UnlockBits(bitmapData);
		return width;
	}

	public unsafe static int CountTransparentRowsFromBottom(Bitmap bm) {
		int width = bm.Width;
		int height = bm.Height;
		Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		for (int num = height - 1; num >= 0; num--) {
			uint* ptr = (uint*)((byte*)bitmapData.Scan0.ToPointer() + (nint)(num * width) * (nint)4);
			for (uint* ptr2 = ptr + width; ptr < ptr2; ptr++) {
				if ((*ptr & 0xFF000000u) != 0) {
					bm.UnlockBits(bitmapData);
					return height - num - 1;
				}
			}
		}
		bm.UnlockBits(bitmapData);
		return height;
	}

	public unsafe static void FillRawBits32(Bitmap bm, uint* bits, int w, int h) {
		int num = w * h;
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < num; i++) {
			bits[i] = ptr[i];
		}
		bm.UnlockBits(bitmapData);
	}

	public static PixelFormat FormatFrombpp(int bpp) {
		return bpp switch {
			1 => PixelFormat.Format1bppIndexed,
			8 => PixelFormat.Format8bppIndexed,
			15 => PixelFormat.Format16bppRgb555,
			16 => PixelFormat.Format16bppRgb565,
			24 => PixelFormat.Format24bppRgb,
			32 => PixelFormat.Format32bppArgb,
			_ => PixelFormat.Undefined,
		};
	}

	public unsafe static Bitmap FromBitsNative(void* bits, int w, int h, int bpp) {
		uint num = ComputeSize((uint)w, (uint)h, (uint)bpp, PaddedTo32: true);
		PixelFormat format = FormatFrombpp(bpp);
		Bitmap bitmap = new Bitmap(w, h);
		BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, format);
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (uint num2 = 0u; num2 < num; num2++) {
			ptr[(int)num2] = ((byte*)bits)[(int)num2];
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBits24(void* bits, int w, int h, bool PaddedTo32Bit) {
		Bitmap bitmap = new Bitmap(w, h);
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		int num = w * 3;
		if (PaddedTo32Bit && num % 4 != 0) {
			num += 4 - num % 4;
		}
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < h; i++) {
			uint* ptr2 = (uint*)(ptr + (nint)i * (nint)bitmapData.Stride);
			byte* ptr3 = (byte*)bits + (nint)i * (nint)num;
			if (i == h - 1) {
				for (int j = 0; j < w; j++) {
					ushort num2 = *(ushort*)(ptr3 + (nint)j * (nint)3);
					byte b = ptr3[j * 3 + 2];
					ptr2[j] = (uint)(num2 | (b << 16)) | 0xFF000000u;
				}
			} else {
				for (int k = 0; k < w; k++) {
					ptr2[k] = *(uint*)(ptr3 + (nint)k * (nint)3) | 0xFF000000u;
				}
			}
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBits32(uint* bits, int w, int h) {
		int num = w * h;
		Bitmap bitmap = new Bitmap(w, h);
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < num; i++) {
			ptr[i] = bits[i];
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBits4(void* bits, uint* Palette, int w, int h, bool PaddedTo32Bit) {
		Bitmap bitmap = new Bitmap(w, h);
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		int num = w / 2;
		if (PaddedTo32Bit && num % 4 != 0) {
			num += 4 - num % 4;
		}
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < h; i++) {
			uint* ptr2 = (uint*)(ptr + (nint)i * (nint)bitmapData.Stride);
			byte* ptr3 = (byte*)bits + (nint)i * (nint)num;
			for (int j = 0; j < w; j++) {
				bool flag = j % 2 == 0;
				uint num2 = ptr3[j / 2] & (flag ? 240u : 15u);
				if (flag) {
					num2 >>= 4;
				}
				ptr2[j] = Palette[num2] | 0xFF000000u;
			}
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBits8(void* bits, uint* Palette, int w, int h, bool PaddedTo32Bit) {
		Bitmap bitmap = new Bitmap(w, h);
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		int num = w;
		if (PaddedTo32Bit && w % 4 != 0) {
			num += 4 - w % 4;
		}
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < h; i++) {
			uint* ptr2 = (uint*)(ptr + (nint)i * (nint)bitmapData.Stride);
			byte* ptr3 = (byte*)bits + (nint)i * (nint)num;
			for (int j = 0; j < w; j++) {
				ptr2[j] = Palette[(int)ptr3[j]] | 0xFF000000u;
			}
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBitsBinary(void* bits, int w, int h, bool PaddedTo32Bit) {
		Bitmap bitmap = new Bitmap(w, h);
		Rectangle rect = new Rectangle(0, 0, w, h);
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		int num = w / 8;
		if (PaddedTo32Bit && num % 4 != 0) {
			num += 4 - num % 4;
		}
		byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < h; i++) {
			uint* ptr2 = (uint*)(ptr + (nint)i * (nint)bitmapData.Stride);
			byte* ptr3 = (byte*)bits + (nint)i * (nint)num;
			for (int j = 0; j < w; j++) {
				uint num2 = ptr3[j / 8];
				num2 = (num2 >> 7 - j % 8) & 1u;
				ptr2[j] = ((num2 == 0) ? 0xff000000u : uint.MaxValue);
			}
		}
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}

	public unsafe static Bitmap FromRawBitsbpp(void* bits, uint* Palette, int w, int h, int bpp, bool PaddedTo32Bit) {
		return bpp switch {
			32 => FromRawBits32((uint*)bits, w, h),
			24 => FromRawBits24(bits, w, h, PaddedTo32Bit),
			16 => FromBitsNative(bits, w, h, bpp),
			8 => FromRawBits8(bits, Palette, w, h, PaddedTo32Bit),
			4 => FromRawBits4(bits, Palette, w, h, PaddedTo32Bit),
			1 => FromRawBitsBinary(bits, w, h, PaddedTo32Bit),
			_ => null,
		};
	}

	public static int GetBitsPerPixel(Bitmap bm) {
		switch (bm.PixelFormat) {
		case PixelFormat.Format24bppRgb:
			return 24;
		case PixelFormat.Format8bppIndexed:
			return 8;
		case PixelFormat.Format32bppRgb:
		case PixelFormat.Format32bppPArgb:
		case PixelFormat.Format32bppArgb:
			return 32;
		default:
			return 0;
		}
	}

	public unsafe static void MakeOpaque(Bitmap bm) {
		int width = bm.Width;
		int height = bm.Height;
		int num = width * height;
		Rectangle rect = new Rectangle(0, 0, width, height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < num; i++) {
			ptr[i] |= 0xff000000u;
		}
		bm.UnlockBits(bitmapData);
	}

	public unsafe static void MakeSolidColor(Bitmap bm, uint IntColor) {
		int width = bm.Width;
		int height = bm.Height;
		int num = width * height;
		Rectangle rect = new Rectangle(0, 0, width, height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
		for (int i = 0; i < num; i++) {
			ptr[i] = IntColor;
		}
		bm.UnlockBits(bitmapData);
	}

	public unsafe static bool MaskToAlpha(Bitmap srcBM, Bitmap BWMask) {
		if (srcBM.Width != BWMask.Width || srcBM.Height != BWMask.Height) {
			return false;
		}
		int height = srcBM.Height;
		int width = srcBM.Width;
		Rectangle rect = new Rectangle(0, 0, width, height);
		uint* ptr = null;
		uint* ptr2 = null;
		BitmapData bitmapData = null;
		BitmapData bitmapData2 = null;
		try {
			bitmapData = BWMask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			ptr = (uint*)bitmapData.Scan0.ToPointer();
			bitmapData2 = srcBM.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			ptr2 = (uint*)bitmapData2.Scan0.ToPointer();
		} catch (Exception) {
			return false;
		}
		int num = width * height;
		for (int i = 0; i < num; i++) {
			if ((ptr[i] & 0xFFFFFF) == 0xffffff) {
				ptr2[i] &= 0xffffffu;
			}
		}
		BWMask.UnlockBits(bitmapData);
		srcBM.UnlockBits(bitmapData2);
		return true;
	}

	public unsafe static Bitmap ResizeCropPad(Bitmap bm, int NewWidth, int NewHeight) {
		if (NewWidth == bm.Width && NewHeight == bm.Height) {
			return new Bitmap(bm);
		}
		int height = bm.Height;
		int width = bm.Width;
		Rectangle rect = new Rectangle(0, 0, width, height);
		BitmapData bitmapData = bm.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
		uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
		Bitmap bitmap = new Bitmap(NewWidth, NewHeight);
		Rectangle rect2 = new Rectangle(0, 0, NewWidth, NewHeight);
		BitmapData bitmapData2 = bitmap.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		uint* ptr2 = (uint*)bitmapData2.Scan0.ToPointer();
		int num = ((NewWidth > bm.Width) ? bm.Width : NewWidth);
		for (int i = 0; i < NewHeight; i++) {
			uint* ptr3 = ptr2 + i * NewWidth;
			uint* ptr4 = ptr + i * width;
			if (i >= height) {
				for (int j = 0; j < NewWidth; j++) {
					ptr3[j] = 0u;
				}
				continue;
			}
			for (int k = 0; k < NewWidth; k++) {
				if (k < num) {
					ptr3[k] = ptr4[k];
				} else {
					ptr3[k] = 0u;
				}
			}
		}
		bitmap.UnlockBits(bitmapData2);
		bm.UnlockBits(bitmapData);
		return bitmap;
	}
}
