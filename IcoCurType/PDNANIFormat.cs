using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PaintDotNet;
using IcoCurType.Util;

namespace IcoCurType;

internal class PDNANIFormat : FileType {
	public PDNANIFormat() : base("Animated Cursors", new FileTypeOptions {
		SupportsLayers = true,
		SupportsCancellation = false,
		SaveExtensions = new string[] { ".ani" },
		LoadExtensions = new string[] { ".ani" },
	}) {
	}

	protected override Document OnLoad(Stream input) {
		EvanRIFFFormat evanRIFFFormat = new EvanRIFFFormat();
		if (evanRIFFFormat.InitFromStream(input) != 0) {
			return null;
		}
		if (evanRIFFFormat.MasterChunk.GetStringHeaderID().ToLower() != "acon") {
			throw new InvalidDataException("The specified RIFF file is not an animated cursor.");
		}

		int chunkID = 0x6e6f6369;
		Document document = null;
		List<EvanRIFFFormat.Chunk> list = evanRIFFFormat.FindAllChunks(chunkID);
		foreach (EvanRIFFFormat.Chunk item in list) {
			input.Seek(item.DataOffset(), SeekOrigin.Begin);
			EOIcoCurLoader eOIcoCurLoader = new EOIcoCurLoader(input);
			Bitmap image = eOIcoCurLoader.GetImage(0u);
			if (image == null) {
				throw new InvalidDataException("Data within the animated cursor file is corrupt or invalid");
			}
			if (document == null) {
				document = new Document(image.Width, image.Height);
			}
			image.RotateFlip(RotateFlipType.Rotate180FlipX);
			Surface surface = Surface.CopyFromBitmap(image);
			image.Dispose();
			BitmapLayer value = new BitmapLayer(surface);
			document.Layers.Add(value);
		}
		return document;
	}

	protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback) {
		ANISaveOptForm aNISaveOptForm = new ANISaveOptForm();
		aNISaveOptForm.InitFromDocument(input);
		if (aNISaveOptForm.ShowDialog() == DialogResult.OK) {
			uint animDelay = aNISaveOptForm.GetAnimDelay();
			Point hotSpot = aNISaveOptForm.GetHotSpot();
			uint num = 4286u;
			uint num2 = (num + 8) * (uint)input.Layers.Count;
			uint dword = num2 + 60;
			byte[] buffer = new byte[4] { 82, 73, 70, 70 };
			output.Write(buffer, 0, 4);
			EOStreamUtility.Write_uint(output, dword);
			byte[] buffer2 = new byte[4] { 65, 67, 79, 78 };
			output.Write(buffer2, 0, 4);
			Writeanih(output, (uint)input.Layers.Count, animDelay);
			byte[] buffer3 = new byte[4] { 76, 73, 83, 84 };
			output.Write(buffer3, 0, 4);
			EOStreamUtility.Write_uint(output, num2 + 4);
			byte[] buffer4 = new byte[4] { 102, 114, 97, 109 };
			output.Write(buffer4, 0, 4);
			for (int i = 0; i < input.Layers.Count; i++) {
				byte[] buffer5 = new byte[4] { 105, 99, 111, 110 };
				output.Write(buffer5, 0, 4);
				EOStreamUtility.Write_uint(output, num);
				Bitmap bitmapLayerResized = EOPDNUtility.GetBitmapLayerResized(input, i, 32, 32);
				EOIcoCurWriter eOIcoCurWriter = new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor);
				eOIcoCurWriter.WriteBitmap(bitmapLayerResized, null, hotSpot);
			}
		}
	}

	private void Writeanih(Stream outS, uint NumFrames, uint FrameRate) {
		byte[] buffer = new byte[4] { 97, 110, 105, 104 };
		outS.Write(buffer, 0, 4);
		uint dword = 36u;
		EOStreamUtility.Write_uint(outS, dword);
		EOStreamUtility.Write_uint(outS, dword);
		EOStreamUtility.Write_uint(outS, NumFrames);
		EOStreamUtility.Write_uint(outS, NumFrames);
		EOStreamUtility.Write_uint(outS, 0u);
		EOStreamUtility.Write_uint(outS, 0u);
		EOStreamUtility.Write_uint(outS, 0u);
		EOStreamUtility.Write_uint(outS, 0u);
		EOStreamUtility.Write_uint(outS, FrameRate);
		EOStreamUtility.Write_uint(outS, 1u);
	}
}
