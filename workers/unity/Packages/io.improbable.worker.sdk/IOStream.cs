using System;
using System.IO;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    [Flags]
    public enum OpenMode
    {
        /**
         * Allow read operations on the stream. Read operations always occur at the read position, which
         * is initialized to the beginning of the stream.
         */
        OpenModeRead = 1,

        /**
         * Allow write operations on the stream. Write operations always occur at the write position,
         * which is initialized to the end of the stream.
         */
        OpenModeWrite = 2,

        /**
         * Truncates any existing content upon opening. If not set, writes are appended to the end of the
         * stream's existing content.
         */
        OpenModeTruncate = 4,

        /**
         * Specify that writes should be appended to the stream's existing content, if any exists.
         */
        OpenModeAppend = 8,
    }

    public sealed unsafe class IOStream : IDisposable
    {
        internal readonly CIO.StreamHandle Stream;

        private IOStream(CIO.StreamHandle stream)
        {
            Stream = stream;
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            Stream.Dispose();
        }

        public static IOStream CreateRingBufferStream(uint capacity)
        {
            return new IOStream(CIO.CreateRingBufferStream(capacity));
        }

        public static IOStream CreateFileStream(string fileName,
            OpenMode? openMode = OpenMode.OpenModeRead | OpenMode.OpenModeWrite | OpenMode.OpenModeTruncate)
        {
            fixed (byte* fileNameBytes = ApiInterop.ToUtf8Cstr(fileName))
            {
                return new IOStream(CIO.CreateFileStream(fileNameBytes, (uint) openMode.GetValueOrDefault()));
            }
        }

        public long Write(byte[] data)
        {
            ThrowIfStreamClosed();

            var remainingCapacity = CIO.StreamGetRemainingWriteCapacityBytes(Stream);
            if (remainingCapacity < data.Length)
            {
                throw new NotSupportedException("Not enough stream capacity to write data.");
            }

            var bytesWritten = 0L;
            fixed (byte* dataToWrite = data)
            {
                bytesWritten = CIO.StreamWrite(Stream, dataToWrite, 1);
            }

            if (bytesWritten != -1)
            {
                return bytesWritten;
            }

            var rawError = CIO.StreamGetLastError(Stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Read(uint bytesToRead, out byte[] streamData)
        {
            ThrowIfStreamClosed();

            streamData = new byte[bytesToRead];

            var bytesRead = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesRead = CIO.StreamRead(Stream, streamDataPointer, bytesToRead);
            }

            if (bytesRead != -1)
            {
                return bytesRead;
            }

            var rawError = CIO.StreamGetLastError(Stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Read(byte[] streamData)
        {
            ThrowIfStreamClosed();

            var bytesToRead = (uint) streamData.Length;
            var bytesRead = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesRead = CIO.StreamRead(Stream, streamDataPointer, bytesToRead);
            }

            if (bytesRead != -1)
            {
                return bytesRead;
            }

            var rawError = CIO.StreamGetLastError(Stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Peek(uint bytesToPeek, out byte[] streamData)
        {
            ThrowIfStreamClosed();

            streamData = new byte[bytesToPeek];

            var bytesPeeked = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesPeeked = CIO.StreamPeek(Stream, streamDataPointer, bytesToPeek);
            }

            if (bytesPeeked != -1)
            {
                return bytesPeeked;
            }

            var rawError = CIO.StreamGetLastError(Stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Ignore(uint bytesToIgnore)
        {
            ThrowIfStreamClosed();

            var bytesIgnored = CIO.StreamIgnore(Stream, bytesToIgnore);

            if (bytesIgnored != -1)
            {
                return bytesIgnored;
            }

            var rawError = CIO.StreamGetLastError(Stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public uint GetRemainingCapacity()
        {
            return CIO.StreamGetRemainingWriteCapacityBytes(Stream);
        }

        private void ThrowIfStreamClosed()
        {
            if (Stream.IsClosed)
            {
                throw new ObjectDisposedException("Cannot access a disposed object.");
            }
        }
    }
}
