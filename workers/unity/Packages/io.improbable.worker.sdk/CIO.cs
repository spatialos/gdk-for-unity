using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;
using Int64 = System.Int64;
using Uint64 = System.UInt64;
using Uint32 = System.UInt32;
using Uint8 = System.Byte;
using Char = System.Char;
using IntPtr = System.IntPtr;

namespace Improbable.Gdk
{
    internal unsafe class CIO
    {
#if UNITY_IOS
        private const string WorkerLibrary = "__Internal";
#else
        private const string WorkerLibrary = "improbable_worker";
#endif
        public class Storage : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                StorageDestroy(handle);
                return true;
            }
        }

        public class Stream : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                StreamDestroy(handle);
                return true;
            }
        }

        public enum OpenMode
        {
            /* Opens the stream in the default mode. */
            OpenModeDefault = 0x00,
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
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Create")]
        public static extern Storage StorageCreate();

        /* Destroys the trace storage. */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Destroy")]
        public static extern void StorageDestroy(IntPtr storage);

        /**
         * Clears the storage object.
         *
         * This marks memory previously stored in this storage object as available to re-use/overwrite
         * but does not actually free the memory. This leads to fewer allocations than, for example, using
         * a new storage object each time.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Storage_Clear")]
        public static extern void StorageClear(IntPtr storage);

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
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_CreateRingBufferStream")]
        public static extern Stream CreateRingBufferStream(Uint32 capacity_bytes);

        /**
         * Creates an I/O stream implemented as a read/write file.
         *
         * The file stream has a conceptually infinite capacity; its true capacity depends on the
         * underlying filesystem.
         *
         * Upon creation of the file stream, the file is created if it does not exist. The file stream is
         * initialized to read from the beginning of the file and append to the end, regardless of whether
         * it previously existed or not.
         *
         * Returns a pointer to a file stream. Never returns NULL. Call Io_Stream_GetLastError to check
         * if an error occurred during file stream creation.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_CreateFileStream")]
        public static extern Stream CreateFileStream(Char* filename, OpenMode open_mode);

        /* Destroys the I/O stream. */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
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
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Write")]
        public static extern Int64 StreamWrite(Stream stream, Uint8* bytes, Uint32 length);

        /**
         * Gets the remaining write capacity in bytes.
         *
         * Returns the maximum value for stream implementations with conceptually infinite capacity, like
         * file streams, regardless of how much data has previously been written.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_GetRemainingWriteCapacityBytes")]
        public static extern Uint32 StreamGetRemainingWriteCapacityBytes(Stream stream);

        /**
         * Reads as much of the stream's data as possible into the given buffer.
         *
         * Returns the actual number of bytes read. This may be less than the given length iff the stream
         * has less data than the requested amount.
         *
         * Returns -1 on error. Call Io_Stream_GetLastError to get the associated error message.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Read")]
        public static extern Uint64 StreamRead(Stream stream, Uint8* bytes, Uint32 length);

        /**
         * Reads as much of the stream's data as possible into the given buffer without advancing the read
         * position i.e. a subsequent read of the same size would provide the same data.
         *
         * Returns the actual number of bytes read. This may be less than the given length iff the stream
         * has less data than the requested amount.
         *
         * Returns -1 on error. Call StreamGetLastError() to get the associated error message.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Peek")]
        public static extern Int64 StreamPeek(Stream stream, Uint8* bytes, Uint32 length);

        /**
         * Extracts the given number of bytes from the stream and discards them.
         *
         * Returns the actual number of bytes extracted i.e. the number of bytes by which the read position
         * has advanced. This may be less than the given length iff the stream has less data than the
         * requested amount.
         *
         * Returns -1 on error. Call Io_Stream_GetLastError() to get the associated error message.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_Ignore")]
        public static extern Int64 StreamIgnore(Stream stream, Uint32 length);

        /**
         * Returns the last error which occurred during an API call on this stream. Returns nullptr if no
         * such error has occurred.
         */
        [DllImport(WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Io_Stream_GetLastError")]
        public static extern Char* StreamGetLastError(Stream stream);
    }
}
