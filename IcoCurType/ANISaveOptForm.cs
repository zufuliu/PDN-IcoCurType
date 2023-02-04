using System;
using System.Drawing;
using System.Windows.Forms;
using PaintDotNet;

namespace IcoCurType;

internal partial class ANISaveOptForm : Form {
	private Bitmap[] AnimBMs;
	private int AnimIndex;

	public ANISaveOptForm() {
		InitializeComponent();
	}

	public void InitFromDocument(Document doc) {
		int count = doc.Layers.Count;
		AnimBMs = new Bitmap[count];
		for (int i = 0; i < count; i++) {
			AnimBMs[i] = EOPDNUtility.GetBitmapLayerResized(doc, i, 32, 32);
		}
		AnimIndex = 0;
		AnimTimer.Enabled = true;
		FPSCombo.SelectedIndex = 5;
	}

	private void AnimTimer_Tick(object sender, EventArgs e) {
		AnimPictureBox.Image = AnimBMs[AnimIndex];
		AnimIndex++;
		if (AnimIndex >= AnimBMs.Length) {
			AnimIndex = 0;
		}
	}

	private void FPSCombo_SelectedIndexChanged(object sender, EventArgs e) {
		AnimTimer.Interval = 1000 / Convert.ToInt32(FPSCombo.Text);
	}

	public uint GetAnimDelay() {
		return 60u / Convert.ToUInt32(FPSCombo.Text);
	}

	public Point GetHotSpot() {
		return new Point((int)XUpDown.Value, (int)YUpDown.Value);
	}

	private void Form_Shown(object sender, EventArgs e) {
		base.TopMost = true;
		Focus();
		BringToFront();
	}
}
