using System.Drawing;
using System.IO;
using PaintDotNet;
using IcoCurType.Util;

namespace IcoCurType;

internal class PDNCursorFormat : FileType {
	public PDNCursorFormat() : base("Cursors", new FileTypeOptions {
		SupportsLayers = true,
		SupportsCancellation = false,
		SaveExtensions = new string[] { ".cur" },
		LoadExtensions = new string[] { ".cur" },
	}) {
	}

	protected override Document OnLoad(Stream input) {
		return PDNIcoCurFormat.GeneralLoad(input);
	}

	protected override SaveConfigToken OnCreateDefaultSaveConfigToken() {
		return new CurSaveConfigToken();
	}

	public override SaveConfigWidget CreateSaveConfigWidget() {
		return new CurSaveConfigWidget();
	}

	protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback) {
		CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken)token;
		EOIcoCurWriter eOIcoCurWriter = new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor);
		Surface surface = new Surface(input.Width, input.Height);
		surface.Fill(ColorBgra.FromBgra(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
		using (RenderArgs args = new RenderArgs(surface)) {
			EOPDNUtility.Render(input, args, clearBackground: true);
		}
		if (surface.Width != 32 || surface.Height != 32) {
			Surface surface2 = EOPDNUtility.ResizeSurface(new Size(32, 32), surface);
			surface.Dispose();
			surface = surface2;
		}
		Bitmap bitmap = surface.CreateAliasedBitmap();
		if (curSaveConfigToken.EightBit) {
			Bitmap img = Quantize(surface, 8, 255, enableTransparency: false, null);
			eOIcoCurWriter.WriteBitmap(img, bitmap, curSaveConfigToken.HotSpot);
		} else {
			eOIcoCurWriter.WriteBitmap(bitmap, bitmap, curSaveConfigToken.HotSpot);
		}
		bitmap.Dispose();
		surface.Dispose();
	}
}
