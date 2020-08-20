using System;
using System.Runtime.InteropServices;
using Int64 = System.Int64;
using Uint64 = System.UInt64;
using Uint32 = System.UInt32;
using Uint8 = System.Byte;
using Char = System.Byte;
using IntPtr = System.IntPtr;

namespace Improbable.Worker.CInterop.Internal
{
    internal unsafe class CIO
    {
        internal class StorageHandle : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                StorageDestroy(handle);
                return true;
            }
        }

        internal class StreamHandle : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                StreamDestroy(handle);
                return true;
            }
        }

        [Flags]
        public enum OpenMode : Uint32
        {
            /**
             * Allow input operations on the stream. Input operations always occur at the read position, which
             * is initialized to the beginning of the stream.
             */
            OpenModeRead = 0x01,

            /**
             * Allow output operations on the stream. Output operations always occur at the write position,
             * which is initialized to the end of the stream.
             */
            OpenModeWrite = 0x02,

            /**
             * Truncates any existing content upon opening. If not set, writes are appended to the end of the
             * stream's existing content.
             */
            OpenModeTruncate = 0x04,

            /**
             * Specify that writes should be appended to the stream's existing content, if any exists.
             */
            OpenModeAppend = 0x08,
        }

        /**
         * Creates a generic storage object.
         *
         * Storage objects can be used in conjunction with other APIs to efficiently manage the lifetime
         * of various data (e.g. trace items).
         *
         * Memory stored in a given storage object remains valid until either the storage object is cleared
         * or it is destroyed.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Create")]
        public static extern StorageHandle StorageCreate();

        /* Destroys the trace storage. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Destroy")]
        public static extern void StorageDestroy(IntPtr storage);

        /**
         * Clears the storage object.
         *
         * This marks memory previously stored in this storage object as available to re-use/overwrite
         * but does not actually free the memory. This leads to fewer allocations than, for example, using
         * a new storage object each time.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Clear")]
        public static extern void StorageClear(StorageHandle storage);

        /**
         * Creates an I/O stream implemented as a ring buffer.
         *
         * The ring buffer stream has a maximum write capacity equal to the number of bytes specified upon
         * creation. Attempted writes which would exceed this capacity will succeed, but return the lower,
         * actual number of bytes written. Reading from the stream increases write capacity by the amount of
         * bytes successfully read.
         *
         * The stream has no readable data when first created.
         *
         * Returns a pointer to a valid ring buffer stream. Never returns NULL.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_CreateRingBufferStream")]
        public static extern StreamHandle CreateRingBufferStream(Uint32 capacityBytes);

        /**
         * Creates an I/O stream implemented as a read/write file.
         *
         * The file stream has a conceptually infinite capacity; its true capacity depends on the
         * underlying filesystem.
         *
         * The open_mode argument should be passed as a combination of OpenMode values.
         *
         * Returns a pointer to a file stream. Never returns NULL. You *must* call Io_Stream_GetLastError to
         * check if an error occurred during file stream creation.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_CreateFileStream")]
        public static extern StreamHandle CreateFileStream(Char* filename, OpenMode openMode);

        /* Destroys the I/O stream. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Destroy")]
        public static extern void StreamDestroy(IntPtr storage);

        /**
         * Writes as much of the given data as possible to the stream.
         *
         * Returns the actual number of bytes written. This may be less than the given length iff the stream
         * has finite capacity and there is insufficient remaining capacity for the full write.
         *
         * Returns -1 on error. Call StreamGetLastError to get the associated error message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Write")]
        public static extern Int64 StreamWrite(StreamHandle stream, Uint8* bytes, Uint32 length);

        /**
         * Gets the remaining write capacity in bytes.
         *
         * Returns the maximum value for stream implementations with conceptually infinite capacity, like
         * file streams, regardless of how much data has previously been written.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_GetRemainingWriteCapacityBytes")]
        public static extern Uint32 StreamGetRemainingWriteCapacityBytes(StreamHandle stream);

        /**
         * Reads as much of the stream's data as possible into the given buffer.
         *
         * Returns the actual number of bytes read. This may be less than the given length iff the stream
         * has less data than the requested amount.
         *
         * Returns -1 on error. Call StreamGetLastError to get the associated error message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Read")]
        public static extern Int64 StreamRead(StreamHandle stream, Uint8* bytes, Uint32 length);

        /**
         * Reads as much of the stream's data as possible into the given buffer without advancing the read
         * position i.e. a subsequent read of the same size would provide the same data.
         *
         * Returns the actual number of bytes read. This may be less than the given length iff the stream
         * has less data than the requested amount.
         *
         * Returns -1 on error. Call StreamGetLastError to get the associated error message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Peek")]
        public static extern Int64 StreamPeek(StreamHandle stream, Uint8* bytes, Uint32 length);

        /**
         * Extracts the given number of bytes from the stream and discards them.
         *
         * Returns the actual number of bytes extracted i.e. the number of bytes by which the read position
         * has advanced. This may be less than the given length iff the stream has less data than the
         * requested amount.
         *
         * Returns -1 on error. Call StreamGetLastError to get the associated error message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Ignore")]
        public static extern Int64 StreamIgnore(StreamHandle stream, Uint32 length);

        /**
         * Returns the last error which occurred during an API call on this stream. Returns nullptr if no
         * such error has occurred.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_GetLastError")]
        public static extern Char* StreamGetLastError(StreamHandle stream);

        /**
         * Clears the stream's current error such that the next call to Io_Stream_GetLastError returns
         * nullptr.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_ClearError")]
        public static extern void StreamClearError(StreamHandle stream);
    }
}
