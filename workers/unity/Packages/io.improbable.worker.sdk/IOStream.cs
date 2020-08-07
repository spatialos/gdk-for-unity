using System;
using System.IO;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public enum OpenMode
    {
        /* Opens the stream in the default mode. */
        OpenModeDefault = 0x00,
    }

    public sealed unsafe class IOStream : IDisposable
    {
        private readonly CIO.StreamHandle stream;

        private IOStream(CIO.StreamHandle stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            stream.Dispose();
        }

        public static IOStream CreateRingBufferStream(uint capacity)
        {
            return new IOStream(CIO.CreateRingBufferStream(capacity));
        }

        public static IOStream CreateFileStream(string fileName, OpenMode openMode)
        {
            fixed (byte* fileNameBytes = ApiInterop.ToUtf8Cstr(fileName))
            {
                return new IOStream(CIO.CreateFileStream(fileNameBytes, (CIO.OpenMode) openMode));
            }
        }

        public long Write(byte[] data)
        {
            ThrowIfStreamClosed();

            var remainingCapacity = CIO.StreamGetRemainingWriteCapacityBytes(stream);
            if (remainingCapacity < data.Length)
            {
                throw new NotSupportedException("Not enough stream capacity to write data.");
            }

            var bytesWritten = 0L;
            fixed (byte* dataToWrite = data)
            {
                bytesWritten = CIO.StreamWrite(stream, dataToWrite, 1);
            }

            if (bytesWritten != -1)
            {
                return bytesWritten;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Read(uint bytesToRead, out byte[] streamData)
        {
            ThrowIfStreamClosed();

            streamData = new byte[bytesToRead];

            var bytesRead = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesRead = CIO.StreamRead(stream, streamDataPointer, bytesToRead);
            }

            if (bytesRead != -1)
            {
                return bytesRead;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Read(byte[] streamData)
        {
            ThrowIfStreamClosed();

            var bytesToRead = (uint) streamData.Length;
            var bytesRead = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesRead = CIO.StreamRead(stream, streamDataPointer, bytesToRead);
            }

            if (bytesRead != -1)
            {
                return bytesRead;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Peek(uint bytesToPeek, out byte[] streamData)
        {
            ThrowIfStreamClosed();

            streamData = new byte[bytesToPeek];

            var bytesPeeked = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesPeeked = CIO.StreamPeek(stream, streamDataPointer, bytesToPeek);
            }

            if (bytesPeeked != -1)
            {
                return bytesPeeked;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Ignore(uint bytesToIgnore)
        {
            ThrowIfStreamClosed();

            var bytesIgnored = CIO.StreamIgnore(stream, bytesToIgnore);

            if (bytesIgnored != -1)
            {
                return bytesIgnored;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public uint GetRemainingCapacity()
        {
            return CIO.StreamGetRemainingWriteCapacityBytes(stream);
        }

        private void ThrowIfStreamClosed()
        {
            if (stream.IsClosed)
            {
                throw new ObjectDisposedException("Cannot access a disposed object.");
            }
        }
    }
}
