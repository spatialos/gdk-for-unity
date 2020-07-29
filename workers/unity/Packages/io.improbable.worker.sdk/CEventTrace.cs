using System.Runtime.InteropServices;
using Uint64 = System.UInt64;
using Uint32 = System.UInt32;
using Uint16 = System.UInt16;
using Uint8 = System.Byte;
using Int8 = System.SByte;
using Char = System.Byte;
using FunctionPtr = System.IntPtr;
using IntPtr = System.IntPtr;

// This file must match the C API (/worker_sdk/c/include/improbable/c_trace.h) *exactly*!
// Current Worker SDK Version: 14.7.0
namespace Improbable.Worker.CInterop.Internal
{
    internal unsafe class CEventTrace
    {
        /**
         * Data for an event. This is a collection of key-value pairs (fields). Use EventData* functions to
         * read or write fields.
         */
        public class EventData : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                EventDataDestroy(handle);
                return true;
            }
        }

        public class EventTracer : CptrHandle
        {
            protected override bool ReleaseHandle()
            {
                EventTracerDestroy(handle);
                return true;
            }
        }

        /**
         * An identifier for a span which can reasonably be expected to be unique across an entire
         * deployment.
         */
        [StructLayout(LayoutKind.Sequential)]
        public struct SpanId
        {
            public fixed Char Data[16];
        }

        public enum ItemType
        {
            Span = 1,
            Event = 2
        }

        /**
         * Returns a span ID representing a special null ID. This can be used to indicate that a span
         * should not be actively traced.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_SpanId_Null")]
        public static extern SpanId SpanIdNull();

        /** Returns a randomly generated span ID. This should only be used for testing purposes.  */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_SpanId_GenerateTestSpanId")]
        public static extern SpanId GenerateTestSpanId();


        /** Returns whether the given span IDs are equal. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_SpanId_Equal")]
        public static extern Uint8 SpanIdEqual(SpanId a, SpanId b);

        /** Returns a hash of the the given span ID. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_SpanId_Hash")]
        public static extern Uint8 SpanIdHash(SpanId spanId);


        /** Data for adding a span, used by EventTracerAddSpan. */
        [StructLayout(LayoutKind.Sequential)]
        public struct SpanOptions
        {
            public Uint32 CauseCount;
            public SpanId* Causes;
        }

        /** Data for a span added to the event-tracer. */
        [StructLayout(LayoutKind.Sequential)]
        public struct Span
        {
            public SpanId Id;
            public Uint32 CauseCount;
            public SpanId* Causes;
        }

        /**
         * Creates an empty event data object. This should be populated with EventDataAddStringField
         * before being added to the event-tracer.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_Create")]
        public static extern EventData EventDataCreate();

        /** Frees resources for the event data object.*/
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_Destroy")]
        public static extern void EventDataDestroy(IntPtr data);

        /**
         * Adds the key value pair as a field to the given event data object. Note that this may invalidate
         * any keys or values retrieved with EventData_Get*.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_AddStringFields")]
        public static extern void EventDataAddStringFields(EventData data, Uint32 count, Char** keys, Char** values);

        /** Returns the number of fields on the given event data object. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_GetFieldCount")]
        public static extern Uint32 EventDataGetFieldCount(EventData data);

        /**
         * Returns all the key value pairs in the event data object. keys and values must have capacity for
         * at least EventDataGetFieldCount(data) elements. This method is provided to discover key
         * value pairs of unknown event schema data, therefore the ordering of key value pairs is entirely
         * arbitrary.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_GetStringFields")]
        public static extern void EventDataGetStringFields(EventData data, Char** keys, Char** values);

        /** Returns the value for the given key. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventData_GetFieldValue")]
        public static extern Char* EventDataGetFieldValue(EventData data, Char* key);

        /** Data for an event added to the event-tracer. */
        [StructLayout(LayoutKind.Sequential)]
        public struct Event
        {
            public SpanId Id;
            public Uint64 UnixTimestampMillis;
            public Char* Message;
            public Char* Type;

            // Use the EventData* methods to read the data.
            public IntPtr Data;
        }

        /** An item added to the event-tracer. */
        [StructLayout(LayoutKind.Sequential)]
        public struct Item
        {
            /** The type of the item, defined using ItemType. */
            public Uint8 ItemType;

            /** An item can either be a Span or an Event. */
            public Union ItemUnion;

            [StructLayout(LayoutKind.Explicit)]
            public struct Union
            {
                [FieldOffset(0)] public Span Span;

                [FieldOffset(0)] public Event Event;
            }
        }

        /**
         * The callback type for spans or events added to the EventTracer. The Item will
         * only be valid for the duration of the callback.
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TraceCallback(void* userData, Item* item);

        /** Parameters for configuring the event-tracer. */
        [StructLayout(LayoutKind.Sequential)]
        public struct EventTracerParameters
        {
            /** The callback to invoke when a span or event is added to the event-tracer. */
            public FunctionPtr TraceCallback;

            /** User data to provide to the callback above. */
            public void* UserData;
        }

        /** Creates an event-tracer. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "EventTracerCreate")]
        public static extern EventTracer EventTracerCreate(EventTracerParameters* parameters);

        /** Frees resources for an event-tracer. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "EventTracerDestroy")]
        public static extern void EventTracerDestroy(IntPtr eventTracer);

        /**
         * Enables the event-tracer. When adding spans to the event-tracer, a non-null span ID will be
         * returned and the provided TraceCallback will be invoked.
         * Note that the TraceCallback will NOT be invoked for events added with a null span ID. If a span
         * was added while the event-tracer was disabled, the TraceCallback will NOT be invoked for any
         * events added to the span (even if the event-tracer is enabled).
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_Enable")]
        public static extern void EventTracerEnable(EventTracer eventTracer);

        /**
         * Disables the EventTracer. When adding spans to the event-tracer, a null span ID will be
         * returned and the provided TraceCallback will NOT be invoked.
         * Note that the TraceCallback will be invoked for events added with a non-null span ID. If a span
         * was added while the event-tracer was enabled, the Trace_Callback will be invoked for any events
         * added to the span (even if the event-tracer is disabled).
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_Disable")]
        public static extern void EventTracerDisable(EventTracer eventTracer);

        /**
         * Sets the per thread active span ID for the EventTracer. This ID may be used internally by
         * the Worker API. For example, subsequent calls to Worker_Connection_Send* will attach the set ID
         * to the internal messages. Calling this function when there is already a non-null span ID active
         * is safe and will overwrite the existing active span ID with the given ID. We recommend unsetting
         * the active span ID when no longer needed to avoid creating causal relationships between
         * unrelated spans.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_SetActiveSpanId")]
        public static extern void EventTracerSetActiveSpanId(EventTracer eventTracer, SpanId spanId);

        /**
         * Unsets the active span ID on the event-tracer for the current thread.
         * EventTracerGetActiveSpanId will return a null span ID.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_UnsetActiveSpanId")]
        public static extern void EventTracerUnsetActiveSpanId(EventTracer eventTracer);

        /** Gets the active span ID on the event-tracer. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_GetActiveSpanId")]
        public static extern SpanId EventTracerGetActiveSpanId(EventTracer eventTracer);

        /** Adds a span to the event-tracer. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_AddSpan")]
        public static extern SpanId EventTracerAddSpan(EventTracer eventTracer, SpanId* causes, Uint32 causeCount);

        /**
         * Adds an event to the event-tracer. Note that the `UnixTimestampMillis` field in the event will
         * be ignored. Ownership of the event is NOT taken by the event-tracer, it is up to the user to
         * free TraceEvent.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_AddEvent")]
        public static extern void EventTracerAddEvent(EventTracer eventTracer, Event @event);

        /**
         * Returns true if the given (partial) event object should be sampled. Currently, only the `spanId`
         * field of the event is considered. This method is useful if generation of the event's message or
         * data is expensive, e.g. if it involves allocation.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_EventTracer_ShouldSampleEvent")]
        public static extern Uint8 EventTracerShouldSampleEvent(EventTracer eventTracer, Event @event);

        /**
         * Create a new trace item from the memory owned by this storage. The item will be valid as long
         * as the storage's memory is valid i.e. until the storage is cleared or destroyed.
         *
         * The item is initialized by copying the provided item; pass a NULL item argument to create an
         * item in an uninitialized state.
         *
         * Directly creating a TraceItem object (on the stack or the heap) by other means than calling this
         * method is discouraged as it will lead to undefined behaviour when passing that item to certain
         * trace API methods (e.g. SerializeItemToStream).
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_Item_Create")]
        public static extern Item* ItemCreate(CIO.StorageHandle storage, Item* item);

        /**
         * Returns a pointer to a thread-local trace item.
         *
         * The item is initially uninitialized, but successive calls to this method on the same thread
         * always returns the same item, which may have been modified by previous usage.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_Item_GetThreadLocal")]
        public static extern Item* ItemGetThreadLocal();

        /**
         * Get the serialized size of the trace item in bytes.
         *
         * Note that each call to GetSerializedItemSize invalidates the internal state necessary to
         * serialize the previous item. Therefore, you must call SerializeItemToStream with one item
         * before calling GetSerializedItemSize with the next item.
         *
         * Returns 0 on error. You can call GetLastError to get the associated error message,
         * but it is safe to pass 0 as the size to a subsequent call to SerializeItemToStream.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_GetSerializedItemSize")]
        public static extern Uint32 GetSerializedItemSize(Item* item);

        /**
         * Serialize the given trace item to the stream.
         *
         * The size argument must be the result of a call to GetSerializedItemSize with the same item.
         * Otherwise, behaviour is undefined. It is not necessary to check that GetSerializedItemSize
         * returned a non-zero item size. Instead, this can be indirectly checked by passing the size to
         * this method and checking its return value for an error.
         *
         * The caller is responsible for ensuring that the provided stream has sufficient remaining capacity
         * to hold the serialized item.
         *
         * Returns 1 on success, 0 on error. Call GetLastError to get the associated error
         * message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_SerializeItemToStream")]
        public static extern Int8 SerializeItemToStream(CIO.StreamHandle stream, Item* item, Uint32 size);

        /**
         * Get the serialized size, in bytes, of the next serialized trace item to be read from the stream.
         *
         * Returns 0 either if there was an error or the stream did not contain enough data to calculate
         * the next item's serialized size. You can call GetLastError to get the associated error
         * message, but it is safe to pass 0 as the size to a subsequent call to
         * DeserializeItemFromStream.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_GetNextSerializedItemSize")]
        public static extern Uint32 GetNextSerializedItemSize(CIO.StreamHandle stream);

        /**
         * Deserialize the next trace item from the stream.
         *
         * If the deserialized item does not need to be used after the next call to
         * DeserializeItemFromStream, it is recommended to pass the item returned by
         * ItemGetThreadLocal to deserialize into. Otherwise, pass an item stored in an IoStorage
         * object.
         *
         * The size argument must be the result of a previous call to GetNextSerializedItemSize.
         * Otherwise, behaviour is undefined. It is not necessary to check that
         * GetNextSerializedItemSize returned a non-zero item size. Instead, this can be indirectly
         * checked by passing the size to this method and checking its return value for an error.
         *
         * Returns 1 if the next serialized item was successfully deserialized into the provided item.
         * Returns 0 if the stream only contained a partial (serialized) trace item. Writing the rest of
         * the data of the next serialized trace item to the stream may result in the next call to
         * DeserializeItemFromStream being successful.
         * Returns -1 if there was an error during serialization. Call GetLastError to get the
         * associated error message.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_DeserializeItemFromStream")]
        public static extern Int8 DeserializeItemFromStream(CIO.StreamHandle stream, Item* item, Uint32 size);

        /**
         * Returns the last error which occurred during a trace API method call. Returns nullptr if no
         * such error has occurred.
         */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_GetLastError")]
        public static extern Char* GetLastError();

        /* Clears the current error such that the next call to GetLastError returns nullptr. */
        [DllImport(Constants.WorkerLibrary, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "Trace_ClearError")]
        public static extern void ClearError();
    }
}
