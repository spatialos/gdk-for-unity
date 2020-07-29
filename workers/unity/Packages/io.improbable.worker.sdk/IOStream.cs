using System;
using System.IO;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;

namespace Packages.io.improbable.worker.sdk
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
            // Maybe use StreamGetRemainingWriteCapacityBytes to prevent attempting to write to the stream if
            // sizeof(data) > remaining capacity

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
            var b = new byte[bytesToRead];
            var c = GCHandle.Alloc(b, GCHandleType.Pinned);
            var bytesRead = CIO.StreamRead(stream, (byte*) c.AddrOfPinnedObject(), bytesToRead);

            if (bytesRead == -1)
            {
                var rawError = CIO.StreamGetLastError(stream);
                throw new Exception(ApiInterop.FromUtf8Cstr(rawError));
            }

            if (bytesRead != bytesToRead)
            {
                throw new Exception($"Failed to read {bytesToRead} bytes, only read ${bytesRead}.");
            }

            return c.Target as byte[];
        }

        public byte[] Peek(uint bytes)
        {
            var b = new byte[bytes];
            var c = GCHandle.Alloc(b, GCHandleType.Pinned);
            var bytesPeeked = CIO.StreamPeek(stream, (byte*) c.AddrOfPinnedObject(), bytes);

            if (bytesPeeked == -1)
            {
                var rawError = CIO.StreamGetLastError(stream);
                throw new Exception(ApiInterop.FromUtf8Cstr(rawError));
            }

            if (bytesPeeked != bytes)
            {
                throw new Exception($"Failed to read {bytes} bytes, only read ${bytes}.");
            }

            return c.Target as byte[];
        }

        public void Ignore(uint bytesToIgnore)
        {
            CIO.StreamIgnore(stream, bytesToIgnore);
        }
    }
}
