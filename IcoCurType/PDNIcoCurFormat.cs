using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PaintDotNet;
using IcoCurType.Util;

namespace IcoCurType;

[Guid("92F2EE29-8460-4abe-BE88-998F5F5A7681")]
internal class PDNIcoCurFormat : FileType, IFileTypeFactory {
	private List<IcoCurSaveFormat> m_saveFormats;

	public PDNIcoCurFormat() : base("Icons", new FileTypeOptions {
		SupportsLayers = true,
		SupportsCancellation = false,
		SaveExtensions = new string[] { ".ico" },
		LoadExtensions = new string[] { ".ico" },
	}) {
		m_saveFormats = null;
	}

	public FileType[] GetFileTypeInstances() {
		return new FileType[3] {
			new PDNIcoCurFormat(),
			new PDNCursorFormat(),
			new PDNANIFormat()
		};
	}

	public static Document GeneralLoad(Stream input) {
		EOIcoCurLoader eOIcoCurLoader = new EOIcoCurLoader(input);
		int num = eOIcoCurLoader.CountImages();
		switch (num) {
		case -2:
			MessageBox.Show("Icon/Cursor data is invalid and cannot be loaded");
			return null;
		case -1:
			MessageBox.Show("An error occured while trying to load the icon/cursor data.");
			return null;
		case 0:
			MessageBox.Show("No valid icons or cursors could be found in the specified file.");
			return null;
		case 1: {
				Bitmap image = eOIcoCurLoader.GetImage(0u);
				image.RotateFlip(RotateFlipType.Rotate180FlipX);
				Surface surface = Surface.CopyFromBitmap(image);
				BitmapLayer value = new BitmapLayer(surface);
				Document document = new Document(image.Width, image.Height);
				document.Layers.Add(value);
				return document;
			}
		default: {
				IcoCurLoadForm icoCurLoadForm = new IcoCurLoadForm(eOIcoCurLoader, (uint)num);
				if (icoCurLoadForm.ShowDialog() == DialogResult.OK) {
					return icoCurLoadForm.BuildDocument();
				}
				MessageBox.Show("Load Cancelled");
				Bitmap image2 = new Bitmap(256, 256);
				return Document.FromImage(image2);
			}
		}
	}

	protected override Document OnLoad(Stream input) {
		return GeneralLoad(input);
	}

	protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback) {
		IcoSaveForm icoSaveForm = new IcoSaveForm(input);
		if (icoSaveForm.ShowDialog() != DialogResult.OK) {
			return;
		}
		m_saveFormats = icoSaveForm.GetSaveFormats();
		bool wantMerged = icoSaveForm.WantMerged;
		icoSaveForm.Dispose();
		if (wantMerged) {
			Surface surface = new Surface(input.Width, input.Height);
			surface.Fill(ColorBgra.FromBgra(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
			using (RenderArgs args = new RenderArgs(surface)) {
				EOPDNUtility.Render(input, args, clearBackground: true);
			}
			EOIcoCurWriter icoWriter = new EOIcoCurWriter(output, m_saveFormats.Count, EOIcoCurWriter.IcoCurType.Icon);
			SaveImagesMerged(icoWriter, surface);
			surface.Dispose();
			return;
		}

		EOIcoCurWriter eOIcoCurWriter = new EOIcoCurWriter(output, input.Layers.Count, EOIcoCurWriter.IcoCurType.Icon);
		for (int i = 0; i < input.Layers.Count; i++) {
			if (input.Layers[i] is BitmapLayer) {
				string text = ((BitmapLayer)input.Layers[i]).Name;
				if (!text.Contains('x') && !text.Contains('X')) {
					throw new InvalidOperationException("Layer name was not set by user to be of the format #x#. Examples: 32x32, 64x64, etc.");
				}
				string[] array = text.Split('x', 'X');
				int num = Convert.ToInt32(array[0]);
				int num2 = Convert.ToInt32(array[1]);
				if (num > 256 || num2 > 256 || num <= 0 || num2 <= 0) {
					throw new InvalidOperationException("Layer name indicated an invalid icon dimension. Icon dimensions must be in the range [1, 256].");
				}
				Bitmap bitmap = null;
				using (Bitmap bm = EOPDNUtility.GetBitmapLayer(input, i)) {
					bitmap = EvanBitmap.ResizeCropPad(bm, num, num2);
				}
				eOIcoCurWriter.WriteBitmap(bitmap, bitmap, default(Point));
			}
		}
	}

	private void SaveImagesMerged(EOIcoCurWriter icoWriter, Surface renderedSurface) {
		foreach (IcoCurSaveFormat saveFormat in m_saveFormats) {
			if (saveFormat.EightBit) {
				Bitmap alphaImgMask = EOPDNUtility.ResizedBitmapFromSurface(renderedSurface, saveFormat.Dimensions);
				Surface surface = EOPDNUtility.ResizeSurface(saveFormat.Dimensions, renderedSurface);
				Bitmap img = Quantize(surface, 8, 255, enableTransparency: false, null);
				surface.Dispose();
				icoWriter.WriteBitmap(img, alphaImgMask, default(Point));
			} else {
				using Bitmap bitmap = EOPDNUtility.ResizedBitmapFromSurface(renderedSurface, saveFormat.Dimensions);
				icoWriter.WriteBitmap(bitmap, bitmap, default(Point));
			}
		}
	}
}
