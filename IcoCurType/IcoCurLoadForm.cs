using System;
using System.Drawing;
using System.Windows.Forms;
using PaintDotNet;
using IcoCurType.Util;

namespace IcoCurType;

internal partial class IcoCurLoadForm : Form {
	private uint LargestHeight;
	private uint LargestWidth;
	private EOIcoCurLoader ldr;

	public IcoCurLoadForm(EOIcoCurLoader loader, uint count) {
		InitializeComponent();
		ldr = loader;
		SetImageCount(count);
		for (uint num = 0u; num < count; num++) {
			uint out_Width = 0u;
			uint out_Height = 0u;
			uint out_bpp = 0u;
			if (!loader.GetImageDimensions(num, ref out_Width, ref out_Height, ref out_bpp)) {
				MessageBox.Show(loader.ErrorMsg);
			}
			if (out_Width > LargestWidth) {
				LargestWidth = out_Width;
			}
			if (out_Height > LargestHeight) {
				LargestHeight = out_Height;
			}
			AddImageToList(out_Width, out_Height, out_bpp, 0u);
		}
	}

	public void AddImageToList(uint width, uint height, uint bpp, uint compression) {
		if (compression != 0) {
			listBox1.Items.Add($"{width}x{height}, (compressed)");
		} else {
			listBox1.Items.Add($"{width}x{height}, {bpp}-bit");
		}
	}

	public Document BuildDocument() {
		if (LoadOneRBtn.Checked && listBox1.SelectedIndex == -1) {
			return null;
		}
		uint selectedIndex = (uint)listBox1.SelectedIndex;
		uint out_Width = 0u;
		uint out_Height = 0u;
		uint out_bpp = 0u;
		ldr.GetImageDimensions(selectedIndex, ref out_Width, ref out_Height, ref out_bpp);
		Document document = (!LoadOneRBtn.Checked) ? new Document((int)LargestWidth, (int)LargestHeight) : new Document((int)out_Width, (int)out_Height);
		if (document == null) {
			MessageBox.Show("Could not build a PDN document from the image data.", "Icon/Cursor Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return null;
		}
		if (LoadOneRBtn.Checked) {
			Bitmap image = ldr.GetImage(selectedIndex);
			image.RotateFlip(RotateFlipType.Rotate180FlipX);
			Surface surface = Surface.CopyFromBitmap(image);
			image.Dispose();
			BitmapLayer bitmapLayer = new BitmapLayer(surface);
			bitmapLayer.Name = "Background";
			document.Layers.Add(bitmapLayer);
			return document;
		}
		for (int i = 0; i < listBox1.Items.Count; i++) {
			Bitmap image2 = ldr.GetImage((uint)i);
			out_Width = (uint)image2.Width;
			out_Height = (uint)image2.Height;
			image2.RotateFlip(RotateFlipType.Rotate180FlipX);
			Bitmap bitmap = EvanBitmap.ResizeCropPad(image2, (int)LargestWidth, (int)LargestHeight);
			image2.Dispose();
			Surface surface2 = Surface.CopyFromBitmap(bitmap);
			bitmap.Dispose();
			BitmapLayer bitmapLayer2 = new BitmapLayer(surface2);
			bitmapLayer2.Name = $"{out_Width}x{out_Height}";
			document.Layers.Add(bitmapLayer2);
		}
		return document;
	}

	private void IcoCurLoadForm_Shown(object sender, EventArgs e) {
		listBox1.SelectedIndex = 0;
		base.TopMost = true;
		Focus();
		BringToFront();
	}

	private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
		Bitmap image = ldr.GetImage((uint)listBox1.SelectedIndex);
		if (image != null) {
			image.RotateFlip(RotateFlipType.Rotate180FlipX);
			pictureBox1.Image = image;
		}
	}

	public void SetImageCount(uint count) {
		RadioItemsGBox.Text = $"There are {count} images available";
	}

	private void LoadAllRBtn_CheckedChanged(object sender, EventArgs e) {
		listBox1.SelectedIndex = -1;
		listBox1.Enabled = false;
	}

	private void LoadOneRBtn_CheckedChanged(object sender, EventArgs e) {
		listBox1.SelectedIndex = 0;
		listBox1.Enabled = true;
	}
}
