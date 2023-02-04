using System;
using System.Windows.Forms;
using PaintDotNet;

namespace IcoCurType;

internal partial class CurSaveConfigWidget : SaveConfigWidget {
	public CurSaveConfigWidget() {
		InitializeComponent();
	}

	protected override void InitFileType() {
		fileType = new PDNIcoCurFormat();
	}

	protected override void InitTokenFromWidget() {
		((CurSaveConfigToken)base.Token).HotSpot.X = (int)XUpDown.Value;
		((CurSaveConfigToken)base.Token).HotSpot.Y = (int)YUpDown.Value;
		((CurSaveConfigToken)base.Token).EightBit = Bit8RBtn.Checked;
	}

	protected override void InitWidgetFromToken(SaveConfigToken token) {
		CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken)token;
		XUpDown.Value = curSaveConfigToken.HotSpot.X;
		YUpDown.Value = curSaveConfigToken.HotSpot.Y;
		Bit8RBtn.Checked = curSaveConfigToken.EightBit;
		Bit32RBtn.Checked = !curSaveConfigToken.EightBit;
	}

	private void HotSpotChange(object sender, EventArgs e) {
		UpdateToken();
	}

	private void HotSpotKeyPress(object sender, KeyPressEventArgs e) {
		UpdateToken();
	}

	private void DepthCheckChanged(object sender, EventArgs e) {
		UpdateToken();
	}
}
