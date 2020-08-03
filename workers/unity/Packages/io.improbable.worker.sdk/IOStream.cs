using System;
using System.IO;
using System.Runtime.InteropServices;
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
            GC.SuppressFinalize(this);
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
            var remainingCapacity = CIO.StreamGetRemainingWriteCapacityBytes(stream);
            if (remainingCapacity < data.Length)
            {
                throw new IOException("Not enough stream capacity to write data.");
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

        public (byte[], long) Read(uint bytesToRead)
        {
            var streamData = new byte[bytesToRead];

            var bytesRead = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesRead = CIO.StreamRead(stream, streamDataPointer, bytesToRead);
            }

            if (bytesRead != -1)
            {
                return (streamData, bytesRead);
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public long Read(byte[] streamData)
        {
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

        public byte[] Peek(uint bytes)
        {
            var streamData = new byte[bytes];

            var bytesPeeked = 0L;
            fixed (byte* streamDataPointer = streamData)
            {
                bytesPeeked = CIO.StreamPeek(stream, streamDataPointer, bytes);
            }

            if (bytesPeeked != -1)
            {
                return streamData;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }

        public void Ignore(uint bytesToIgnore)
        {
            var bytesIgnored = CIO.StreamIgnore(stream, bytesToIgnore);

            if (bytesIgnored != -1)
            {
                return;
            }

            var rawError = CIO.StreamGetLastError(stream);
            throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
        }
    }
}
