using System;
using System.IO;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public unsafe class IOStream : IDisposable
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

        public static IOStream CreateFileStream(string fileName)
        {
            fixed (byte* fileNameBytes = ApiInterop.ToUtf8Cstr(fileName))
            {
                return new IOStream(CIO.CreateFileStream(fileNameBytes, CIO.OpenMode.OpenModeDefault));
            }
        }

        public void Write(byte[] data)
        {
            var remainingCapacity = CIO.StreamGetRemainingWriteCapacityBytes(stream);
            if (remainingCapacity < data.Length)
            {
                throw new IOException("Not enough stream capacity to write data.");
            }

            fixed (byte* dataToWrite = data)
            {
                var bytesWritten = CIO.StreamWrite(stream, dataToWrite, 1);

                if (bytesWritten == -1)
                {
                    // The Stream write failed, check the error message
                    var rawError = CIO.StreamGetLastError(stream);
                    throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
                }

                if (bytesWritten != data.Length)
                {
                    // Check that the number of bytes we tried to write was the number of bytes written
                    throw new IOException("Failed to write all data to stream.");
                }
            }
        }

        public byte[] Read(uint bytesToRead)
        {
            var streamData = new byte[bytesToRead];
            var dataHandle = GCHandle.Alloc(streamData, GCHandleType.Pinned);
            var bytesRead = CIO.StreamRead(stream, (byte*) dataHandle.AddrOfPinnedObject(), bytesToRead);

            if (bytesRead == -1)
            {
                var rawError = CIO.StreamGetLastError(stream);
                throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
            }

            if (bytesRead != bytesToRead)
            {
                throw new IOException($"Failed to read {bytesToRead} bytes, only read {bytesRead}.");
            }

            return dataHandle.Target as byte[];
        }

        public byte[] Peek(uint bytes)
        {
            var streamData = new byte[bytes];
            var dataHandle = GCHandle.Alloc(streamData, GCHandleType.Pinned);
            var bytesPeeked = CIO.StreamPeek(stream, (byte*) dataHandle.AddrOfPinnedObject(), bytes);

            if (bytesPeeked == -1)
            {
                var rawError = CIO.StreamGetLastError(stream);
                throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
            }

            if (bytesPeeked != bytes)
            {
                throw new IOException($"Failed to read {bytes} bytes, only read {bytes}.");
            }

            return dataHandle.Target as byte[];
        }

        public void Ignore(uint bytesToIgnore)
        {
            var bytesIgnored = CIO.StreamIgnore(stream, bytesToIgnore);

            if (bytesIgnored == -1)
            {
                var rawError = CIO.StreamGetLastError(stream);
                throw new IOException(ApiInterop.FromUtf8Cstr(rawError));
            }

            if (bytesIgnored != bytesToIgnore)
            {
                throw new IOException($"Failed to ignore {bytesToIgnore} bytes, only ignored {bytesToIgnore}.");
            }
        }
    }
}
