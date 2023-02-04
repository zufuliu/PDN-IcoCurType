using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PaintDotNet;

namespace IcoCurType;

internal partial class IcoSaveForm : Form {
	private Document g_Doc;
	private List<Size> g_Sizes;
	private int[] g_SupportedDims;

	public bool WantMerged => rbMerged.Checked;

	public IcoSaveForm(Document doc) {
		InitializeComponent();
		g_SupportedDims = new int[7] { 256, 128, 64, 48, 32, 24, 16 };
		g_Doc = doc;
		UpdateList();
	}

	private void btnSelectAll_Click(object sender, EventArgs e) {
		for (int i = 0; i < clbFormats.Items.Count; i++) {
			clbFormats.SetItemChecked(i, value: true);
		}
	}

	private int ClosestMultipleOf8(int num) {
		int num2 = num / 8 * 8;
		int num3 = num2 + 8;
		if (num3 - num < num - num2) {
			return num3;
		}
		return num2;
	}

	internal List<IcoCurSaveFormat> GetSaveFormats() {
		List<IcoCurSaveFormat> list = new List<IcoCurSaveFormat>();
		for (int i = 0; i < clbFormats.Items.Count; i++) {
			if (clbFormats.GetItemChecked(i)) {
				IcoCurSaveFormat item = new IcoCurSaveFormat(g_Sizes[i].Width, g_Sizes[i].Height, clbFormats.Items[i].ToString().Contains("8-bit"));
				list.Add(item);
			}
		}
		return list;
	}

	private bool IsSupportedDim(int w, int h) {
		if (w != h) {
			return false;
		}
		for (int i = 0; i < g_SupportedDims.Length; i++) {
			if (w == g_SupportedDims[i]) {
				return true;
			}
		}
		return false;
	}

	private void ModeCheckChanged(object sender, EventArgs e) {
		clbFormats.Enabled = rbMerged.Checked;
	}

	private void SelNoneBtn_Click(object sender, EventArgs e) {
		for (int i = 0; i < clbFormats.Items.Count; i++) {
			clbFormats.SetItemChecked(i, value: false);
		}
	}

	private void UpdateList() {
		clbFormats.Items.Clear();
		if (g_Doc != null) {
			int width = g_Doc.Width;
			int height = g_Doc.Height;
			if (g_Doc.Width <= 256 && g_Doc.Height <= 256 && !IsSupportedDim(g_Doc.Width, g_Doc.Height)) {
				g_Sizes = new List<Size>(14);
				string item = $"{width}x{height}, 32-bit";
				string item2 = $"{width}x{height}, 8-bit";
				clbFormats.Items.Add(item, isChecked: true);
				clbFormats.Items.Add(item2, isChecked: true);
				g_Sizes.Add(new Size(width, height));
				g_Sizes.Add(new Size(width, height));
			} else {
				g_Sizes = new List<Size>(12);
			}
			clbFormats.Items.Add("256x256, PNG", isChecked: false);
			clbFormats.Items.Add("128x128, 32-bit", isChecked: false);
			clbFormats.Items.Add("128x128, 8-bit", isChecked: false);
			clbFormats.Items.Add("64x64, 32-bit", isChecked: false);
			clbFormats.Items.Add("64x64, 8-bit", isChecked: false);
			clbFormats.Items.Add("48x48, 32-bit", isChecked: false);
			clbFormats.Items.Add("48x48, 8-bit", isChecked: false);
			clbFormats.Items.Add("32x32, 32-bit", isChecked: true);
			clbFormats.Items.Add("32x32, 8-bit", isChecked: true);
			clbFormats.Items.Add("24x24, 32-bit", isChecked: false);
			clbFormats.Items.Add("24x24, 8-bit", isChecked: false);
			clbFormats.Items.Add("16x16, 32-bit", isChecked: true);
			clbFormats.Items.Add("16x16, 8-bit", isChecked: true);
			g_Sizes.Add(new Size(256, 256));
			g_Sizes.Add(new Size(128, 128));
			g_Sizes.Add(new Size(128, 128));
			g_Sizes.Add(new Size(64, 64));
			g_Sizes.Add(new Size(64, 64));
			g_Sizes.Add(new Size(48, 48));
			g_Sizes.Add(new Size(48, 48));
			g_Sizes.Add(new Size(32, 32));
			g_Sizes.Add(new Size(32, 32));
			g_Sizes.Add(new Size(24, 24));
			g_Sizes.Add(new Size(24, 24));
			g_Sizes.Add(new Size(16, 16));
			g_Sizes.Add(new Size(16, 16));
		}
	}

	private void Form_Shown(object sender, EventArgs e) {
		base.TopMost = true;
		Focus();
		BringToFront();
	}
}
