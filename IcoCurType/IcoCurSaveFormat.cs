using System.Drawing;

namespace IcoCurType;

internal struct IcoCurSaveFormat {
	public Size Dimensions;
	public bool EightBit;

	public IcoCurSaveFormat(int width, int height, bool eightBit) {
		Dimensions = new Size(width, height);
		EightBit = eightBit;
	}
}
