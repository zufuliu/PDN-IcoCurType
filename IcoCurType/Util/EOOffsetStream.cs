using System.IO;

namespace IcoCurType.Util;

internal sealed class EOOffsetStream : Stream {
	private Stream m_base;
	private long m_pos;

	public override bool CanRead => m_base.CanRead;
	public override bool CanSeek => m_base.CanSeek;
	public override bool CanWrite => m_base.CanWrite;
	public override long Length => m_base.Length - m_pos;

	public override long Position {
		get {
			return m_base.Position - m_pos;
		}
		set {
			m_base.Position = m_pos + value;
		}
	}

	public EOOffsetStream(Stream underlyingStream) {
		m_base = underlyingStream;
		m_pos = underlyingStream.Position;
	}

	public override void Flush() {
		m_base.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count) {
		return m_base.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin) {
		return m_base.Seek(offset + m_pos, origin);
	}

	public override void SetLength(long value) {
		m_base.SetLength(value + m_pos);
	}

	public override void Write(byte[] buffer, int offset, int count) {
		m_base.Write(buffer, offset, count);
	}
}
