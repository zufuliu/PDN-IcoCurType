using System;
using System.Drawing;
using PaintDotNet;

namespace IcoCurType;

[Serializable]
internal class CurSaveConfigToken : SaveConfigToken {
	public Point HotSpot;
	public bool EightBit;

	public override object Clone() {
		return new CurSaveConfigToken(this);
	}

	public CurSaveConfigToken() {
		HotSpot = new Point(0, 0);
		EightBit = false;
	}

	protected CurSaveConfigToken(CurSaveConfigToken copyMe) {
		HotSpot = copyMe.HotSpot;
		EightBit = copyMe.EightBit;
	}
}
