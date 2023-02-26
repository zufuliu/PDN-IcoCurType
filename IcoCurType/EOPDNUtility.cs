using System;
using System.Drawing;
using PaintDotNet;
using PaintDotNet.Rendering;

namespace IcoCurType;

internal static class EOPDNUtility {
	public static void Render(Document doc, RenderArgs args, bool clearBackground) {
		Render(doc, args, new Rectangle[1] { args.Surface.Bounds }, 0, 1, clearBackground);
	}

	public static void Render(Document doc, RenderArgs args, Rectangle[] roi, int startIndex, int length, bool clearBackground) {
		IRenderer<ColorBgra> renderer = doc.CreateRenderer();
		for (int i = startIndex; i < startIndex + length; i++) {
			Rectangle rectangle = roi[i];
			if (clearBackground) {
				args.Surface.Fill(rectangle, ColorBgra.Zero);
			}

			using ISurface<ColorBgra> dst = args.Surface.CreateWindow(rectangle);
			renderer.Render(dst, rectangle.Location);
		}
	}

	private static bool IsInBounds(Layer layer, Rectangle roi) {
		if (roi.Left < 0 || roi.Top < 0 || roi.Left >= layer.Width || roi.Top >= layer.Height || roi.Right > layer.Width || roi.Bottom > layer.Height) {
			return false;
		}

		return true;
	}

	private static void Render(Layer layer, RenderArgs args, Rectangle roi) {
		if (args.Surface.Width != layer.Width || args.Surface.Height != layer.Height) {
			throw new ArgumentException();
		}

		if (!IsInBounds(layer, roi)) {
			throw new ArgumentOutOfRangeException("roi");
		}

		layer.CreateRenderer().Render(args.Surface.CreateWindow(roi), roi.Location);
	}

	public static Bitmap GetBitmapLayer(Document doc, int index) {
		using Surface surface = new Surface(doc.Width, doc.Height);
		Rectangle roi = new Rectangle(0, 0, doc.Width, doc.Height);
		using (RenderArgs args = new RenderArgs(surface)) {
			Render((Layer)doc.Layers[index], args, roi);
		}
		using Bitmap original = surface.CreateAliasedBitmap();
		return new Bitmap(original);
	}

	public static Bitmap GetBitmapLayerResized(Document Doc, int index, int Width, int Height) {
		Bitmap result = null;
		using Surface surface = new Surface(Doc.Width, Doc.Height);
		Rectangle roi = new Rectangle(0, 0, Doc.Width, Doc.Height);
		using (RenderArgs args = new RenderArgs(surface)) {
			Render((Layer)Doc.Layers[index], args, roi);
		}
		if (Doc.Width != Width || Doc.Height != Height) {
			using Surface surface2 = new Surface(Width, Height);
			surface2.FitSurface(ResamplingAlgorithm.LinearLowQuality, surface);
			using Bitmap original = surface2.CreateAliasedBitmap();
			result = new Bitmap(original);
		} else {
			using Bitmap original2 = surface.CreateAliasedBitmap();
			result = new Bitmap(original2);
		}
		return result;
	}

	public static Bitmap ResizedBitmapFromSurface(Surface surf, Size desiredSize) {
		if (desiredSize.Width != surf.Width || desiredSize.Height != surf.Height) {
			using Surface surface = new Surface(desiredSize);
			surface.FitSurface(ResamplingAlgorithm.LinearLowQuality, surf);
			return new Bitmap(surface.CreateAliasedBitmap());
		}
		using Bitmap original = surf.CreateAliasedBitmap();
		return new Bitmap(original);
	}

	public static Surface ResizeSurface(Size size, Surface surf) {
		Surface surface = new Surface(size);
		ResamplingAlgorithm algorithm = ResamplingAlgorithm.AdaptiveBestQuality;
		if (size.Width > surf.Width || size.Height > surf.Height) {
			algorithm = ResamplingAlgorithm.NearestNeighbor;
		}
		surface.FitSurface(algorithm, surf);
		return surface;
	}
}
