/* Copyright (c) Improbable Worlds Ltd, All Rights Reserved */
#ifndef WORKER_SDK_C_INCLUDE_IMPROBABLE_C_WORKER_H
#define WORKER_SDK_C_INCLUDE_IMPROBABLE_C_WORKER_H

#if defined(WORKER_DLL) && (defined(_MSC_VER) || defined(__ORBIS__))

#ifdef WORKER_DLL_EXPORT
#define WORKER_API __declspec(dllexport)
#else /* WORKER_DLL_EXPORT */
#define WORKER_API __declspec(dllimport)
#endif /* WORKER_DLL_EXPORT */

#else /* defined(WORKER_DLL) && (defined(_MSC_VER) || defined(__ORBIS__)) */
#define WORKER_API
#endif /* defined(WORKER_DLL) && (defined(_MSC_VER) || defined(__ORBIS__)) */

#ifdef __cplusplus
extern "C" {
#endif /* __cplusplus */

#include <stddef.h>
#include <stdint.h>

typedef int64_t Worker_EntityId;
typedef uint32_t Worker_ComponentId;
typedef uint32_t Worker_RequestId;

struct Schema_CommandRequest;
struct Schema_CommandResponse;
struct Schema_ComponentData;
struct Schema_ComponentUpdate;
struct Worker_Connection;
struct Worker_ConnectionFuture;
struct Worker_Constraint;
struct Worker_DeploymentListFuture;
struct Worker_Locator;
struct Worker_OpList;
struct Worker_SnasphotInputStream;
struct Worker_SnapshotOutputStream;

typedef struct Schema_CommandRequest Schema_CommandRequest;
typedef struct Schema_CommandResponse Schema_CommandResponse;
typedef struct Schema_ComponentData Schema_ComponentData;
typedef struct Schema_ComponentUpdate Schema_ComponentUpdate;
typedef struct Worker_Connection Worker_Connection;
typedef struct Worker_ConnectionFuture Worker_ConnectionFuture;
typedef struct Worker_Constraint Worker_Constraint;
typedef struct Worker_DeploymentListFuture Worker_DeploymentListFuture;
typedef struct Worker_Locator Worker_Locator;
typedef struct Worker_OpList Worker_OpList;
typedef struct Worker_SnapshotInputStream Worker_SnapshotInputStream;
typedef struct Worker_SnapshotOutputStream Worker_SnapshotOutputStream;

/**
 * Defaults.
 */
/* General asynchronous IO. */
#define WORKER_DEFAULTS_SEND_QUEUE_CAPACITY 4096
#define WORKER_DEFAULTS_RECEIVE_QUEUE_CAPACITY 4096
#define WORKER_DEFAULTS_LOG_MESSAGE_QUEUE_CAPACITY 256
#define WORKER_DEFAULTS_BUILT_IN_METRICS_REPORT_PERIOD_MILLIS 5000
/* General networking. */
#define WORKER_DEFAULTS_NETWORK_CONNECTION_TYPE WORKER_NETWORK_CONNECTION_TYPE_TCP
#define WORKER_DEFAULTS_CONNECTION_TIMEOUT_MILLIS 60000
/* TCP. */
#define WORKER_DEFAULTS_TCP_MULTIPLEX_LEVEL 32
#define WORKER_DEFAULTS_TCP_SEND_BUFFER_SIZE 65536
#define WORKER_DEFAULTS_TCP_RECEIVE_BUFFER_SIZE 65536
#define WORKER_DEFAULTS_TCP_NO_DELAY 0
/* RakNet. */
#define WORKER_DEFAULTS_RAKNET_HEARTBEAT_TIMEOUT_MILLIS 60000
/* Protocol logging. */
#define WORKER_DEFAULTS_LOG_PREFIX "protocol-log-"
#define WORKER_DEFAULTS_MAX_LOG_FILES 10
#define WORKER_DEFAULTS_MAX_LOG_FILE_SIZE_BYTES (1024 * 1024)

/**
 * Enum defining the severities of log messages that can be sent to SpatialOS and received from the
 * SDK.
 */
typedef enum Worker_LogLevel {
  WORKER_LOG_LEVEL_DEBUG = 1,
  WORKER_LOG_LEVEL_INFO = 2,
  WORKER_LOG_LEVEL_WARN = 3,
  WORKER_LOG_LEVEL_ERROR = 4,
  WORKER_LOG_LEVEL_FATAL = 5
} Worker_LogLevel;

/** Enum defining possible command status codes. */
typedef enum Worker_StatusCode {
  WORKER_STATUS_CODE_SUCCESS = 1,
  WORKER_STATUS_CODE_TIMEOUT = 2,
  WORKER_STATUS_CODE_NOT_FOUND = 3,
  WORKER_STATUS_CODE_AUTHORITY_LOST = 4,
  WORKER_STATUS_CODE_PERMISSION_DENIED = 5,
  WORKER_STATUS_CODE_APPLICATION_ERROR = 6,
  WORKER_STATUS_CODE_INTERNAL_ERROR = 7,
} Worker_StatusCode;

/** Enum defining the possible authority states for an entity component. */
typedef enum Worker_Authority {
  WORKER_AUTHORITY_NOT_AUTHORITATIVE = 0,
  WORKER_AUTHORITY_AUTHORITATIVE = 1,
  WORKER_AUTHORITY_AUTHORITY_LOSS_IMMINENT = 2,
} Worker_Authority;

/** Enum defining the possible modes of loopback when updating a component. */
typedef enum Worker_ComponentUpdateLoopback {
  WORKER_COMPONENT_UPDATE_LOOPBACK_NONE = 0,
  WORKER_COMPONENT_UPDATE_LOOPBACK_SHORT_CIRCUITED = 1,
} Worker_ComponentUpdateLoopback;

/** Parameters for sending a log message to SpatialOS. */
typedef struct Worker_LogMessage {
  /** The severity of the log message; defined in the Worker_LogLevel enumeration. */
  uint8_t level;
  /** The name of the logger. */
  const char* logger_name;
  /** The full log message. */
  const char* message;
  /** The ID of the entity this message relates to, or NULL for none. */
  const Worker_EntityId* entity_id;
} Worker_LogMessage;

/** Parameters for a gauge metric. */
typedef struct Worker_GaugeMetric {
  /* The name of the metric. */
  const char* key;
  /* The current value of the metric. */
  double value;
} Worker_GaugeMetric;

/* Parameters for a histogram metric bucket. */
typedef struct Worker_HistogramMetricBucket {
  /* The upper bound. */
  double upper_bound;
  /* The number of observations that were less than or equal to the upper bound. */
  uint32_t samples;
} Worker_HistogramMetricBucket;

/* Parameters for a histogram metric. */
typedef struct Worker_HistogramMetric {
  /* The name of the metric. */
  const char* key;
  /* The sum of all observations. */
  double sum;
  /* The number of buckets. */
  uint32_t bucket_count;
  /* Array of buckets. */
  const Worker_HistogramMetricBucket* buckets;
} Worker_HistogramMetric;

/** Parameters for sending metrics to SpatialOS. */
typedef struct Worker_Metrics {
  /** The load value of this worker. If NULL, do not report load. */
  const double* load;
  /** The number of gauge metrics. */
  uint32_t gauge_metric_count;
  /** Array of gauge metrics. */
  const Worker_GaugeMetric* gauge_metrics;
  /** The number of histogram metrics. */
  uint32_t histogram_metric_count;
  /** Array of histogram metrics. */
  const Worker_HistogramMetric* histogram_metrics;
} Worker_Metrics;

typedef void Worker_CommandRequestHandle;
typedef void Worker_CommandResponseHandle;
typedef void Worker_ComponentDataHandle;
typedef void Worker_ComponentUpdateHandle;

typedef void Worker_CommandRequestFree(Worker_ComponentId component_id, void* user_data,
                                       Worker_CommandRequestHandle* handle);
typedef void Worker_CommandResponseFree(Worker_ComponentId component_id, void* user_data,
                                        Worker_CommandResponseHandle* handle);
typedef void Worker_ComponentDataFree(Worker_ComponentId component_id, void* user_data,
                                      Worker_ComponentDataHandle* handle);
typedef void Worker_ComponentUpdateFree(Worker_ComponentId component_id, void* user_data,
                                        Worker_ComponentUpdateHandle* handle);

typedef Worker_CommandRequestHandle* Worker_CommandRequestCopy(Worker_ComponentId component_id,
                                                               void* user_data,
                                                               Worker_CommandRequestHandle* handle);
typedef Worker_CommandResponseHandle*
Worker_CommandResponseCopy(Worker_ComponentId component_id, void* user_data,
                           Worker_CommandResponseHandle* handle);
typedef Worker_ComponentDataHandle* Worker_ComponentDataCopy(Worker_ComponentId component_id,
                                                             void* user_data,
                                                             Worker_ComponentDataHandle* handle);
typedef Worker_ComponentUpdateHandle*
Worker_ComponentUpdateCopy(Worker_ComponentId component_id, void* user_data,
                           Worker_ComponentUpdateHandle* handle);

typedef uint8_t Worker_CommandRequestDeserialize(Worker_ComponentId component_id, void* user_data,
                                                 Schema_CommandRequest* source,
                                                 Worker_CommandRequestHandle** handle_out);
typedef uint8_t Worker_CommandResponseDeserialize(Worker_ComponentId component_id, void* user_data,
                                                  Schema_CommandResponse* source,
                                                  Worker_CommandResponseHandle** handle_out);
typedef uint8_t Worker_ComponentDataDeserialize(Worker_ComponentId component_id, void* user_data,
                                                Schema_ComponentData* source,
                                                Worker_ComponentDataHandle** handle_out);
typedef uint8_t Worker_ComponentUpdateDeserialize(Worker_ComponentId component_id, void* user_data,
                                                  Schema_ComponentUpdate* source,
                                                  Worker_ComponentUpdateHandle** handle_out);

typedef void Worker_CommandRequestSerialize(Worker_ComponentId component_id, void* user_data,
                                            Worker_CommandRequestHandle* handle,
                                            Schema_CommandRequest** target_out);
typedef void Worker_CommandResponseSerialize(Worker_ComponentId component_id, void* user_data,
                                             Worker_CommandResponseHandle* handle,
                                             Schema_CommandResponse** target_out);
typedef void Worker_ComponentDataSerialize(Worker_ComponentId component_id, void* user_data,
                                           Worker_ComponentDataHandle* handle,
                                           Schema_ComponentData** target_out);
typedef void Worker_ComponentUpdateSerialize(Worker_ComponentId component_id, void* user_data,
                                             Worker_ComponentUpdateHandle* handle,
                                             Schema_ComponentUpdate** target_out);

typedef struct Worker_ComponentVtable {
  /**
   * Component ID that this vtable is for. If this is the default vtable, this field is ignored.
   */
  Worker_ComponentId component_id;
  /** User data which will be passed directly to the callbacks supplied below. */
  void* user_data;

  /* The function pointers below are only necessary in order to use the user_handle fields present
   * in each of the Worker_CommandRequest, Worker_CommandResponse, Worker_ComponentData and
   * Worker_ComponentUpdate types, for the given component ID (or by default, if this is the default
   * vtable), in order to offload serialization and deserialization work to internal SDK threads.
   *
   * For simplest usage of the SDK, all function pointers can be set to NULL, and only the
   * schema_type field should be used in each type.
   *
   * In order to support usage of the user_handle field on instances of the corresponding type when
   * used as input data to the SDK, X_serialize() must be provided.
   *
   * In order to support usage of the user_handle field on instances of the corresponding type when
   * received as output data to the SDK, X_deserialize() must be provided.
   *
   * X_free() should free resources associated with the result of calling X_deserialize() or
   * X_copy() (if provided).
   *
   * This decision can be made on a per-component, per-handle-type, and per-direction (input or
   * output) basis. In the case of providing data to the SDK, the asynchronous serialization flow
   * can be disabled even on a per-call basis by providing a non-NULL schema_type pointer instead of
   * a user_handle pointer. The concrete types pointer to by the user_handle fields may different
   * between components or between handle types.
   *
   * All of the functions below, if provided, will be called from arbitary internal SDK threads, and
   * therefore must be thread-safe. A single user_handle pointer will not be passed to multiple
   * callbacks concurrently, but a user_handle may be copied twice and the _results_ of those copies
   * may be used concurrently.
   *
   * For a concrete example, consider calling Worker_Connection_SendComponentUpdate() with
   * short-circuiting enabled. The SDK will call component_update_copy() twice on the provided
   * user_handle. One copy will be used for the outgoing flow, and will be serialized with
   * component_update_serialize() and subsequently freed with component_update_free(). Concurrently,
   * the other copy will be passed back to the user as part of a Worker_OpList and freed with
   * component_update_free() when the OpList is deallocated (or, if its lifetime is extended with
   * Worker_AcquireComponentUpdate(), when the last reference is released by the user with
   * Worker_ReleaseComponentUpdate()).
   *
   * In general, the two most obvious strategies are:
   * 1) reference-counting. Have X_copy() (atomically) increase a reference count and return the
   *    same pointer it was given, have X_free() (atomically) decrease the reference count and
   *    deallocate if zero. X_deserialize() should allocate a new object with reference count of 1,
   *    set the reference count of any new handle passed into the SDK to 1 initially and call
   *    X_free() manually afterwards. In this case, data owned by the user_handle should never be
   *    mutated after its first use. (This is the approach used internally for the schema_type.)
   * 2) deep-copying. Have X_copy() allocate an entirely new deep copy of the object, and X_free()
   *    deallocate directly. In this case, user_handles can be mutated freely. */

  Worker_CommandRequestFree* command_request_free;
  Worker_CommandRequestCopy* command_request_copy;
  Worker_CommandRequestDeserialize* command_request_deserialize;
  Worker_CommandRequestSerialize* command_request_serialize;

  Worker_CommandResponseFree* command_response_free;
  Worker_CommandResponseCopy* command_response_copy;
  Worker_CommandResponseDeserialize* command_response_deserialize;
  Worker_CommandResponseSerialize* command_response_serialize;

  Worker_ComponentDataFree* component_data_free;
  Worker_ComponentDataCopy* component_data_copy;
  Worker_ComponentDataDeserialize* component_data_deserialize;
  Worker_ComponentDataSerialize* component_data_serialize;

  Worker_ComponentUpdateFree* component_update_free;
  Worker_ComponentUpdateCopy* component_update_copy;
  Worker_ComponentUpdateDeserialize* component_update_deserialize;
  Worker_ComponentUpdateSerialize* component_update_serialize;
} Worker_ComponentVtable;

/* The four handle types below behave similarly. They support both direct use of schema data types,
 * and alternatively conversion between schema types and custom user-defined handle types on worker
 * threads.
 *
 * When passing an object into the API, either:
 * - assign a new object created via the schema API (e.g. Schema_CreateComponentUpdate()) to the
 *   schema_type field. In this case, the API takes ownership of the schema object.
 * - leave the schema_type field NULL, and provide a custom pointer in the user_handle field. In
 *   this case, the corresponding vtable for the component must supply copy, free and serialize
 *   functions. The API will call X_copy() zero or more times, call X_serialize() if necessary to
 *   convert to a new schema object, and call X_free() on each copy.
 * In both cases, the user does not need to explicitly deallocate schema object (e.g. with
 * Schema_DestroyCompnentUpdate()).
 *
 * When the API passes an object to the user, either:
 * - if no deserialize() function is provided in the corresponding vtable for the component, only
 *   the schema_type field will be non-NULL. The API owns this object, and it will usually be
 *   deallocated when the user-supplied callback returns. To extend the lifetime of the data, call
 *   the relevant Worker_AcquireX() function (e.g. Worker_AcquireComponentUpdate()) and use the
 *   resulting pointer. This must then be explicitly deallocated by calling the corresponding
 *   Worker_ReleaseX() function (e.g. Worker_ReleaseComponentUpdate()) to avoid memory leaks.
 * - if an X_deserialize() function is provided, in which case a X_free() function should also be
 *   provided, both the schema_type and user_handle fields will be non-NULL, the latter filled with
 *   the result of calling the X_deserialize() function. Again, the API owns these objects and will
 *   usually deallocate them. The relevant Worker_AcquireX() function works as before, and will
 *   extend the lifetime of both the schema_type and the user_handle (by calling the user-provided
 *   X_copy() function in the latter case). If only the user_handle needs to be preserved, this is
 *   possible by manually calling the user-provided copy() and free() functions (or otherwise, since
 *   the semantics of the user_handles is up to the user).
 *
 * Note that objects pointed-to by the schema_type fields must _not_ be mutated by the user when
 * owned by the SDK (either because they have been passed as input data to the SDK, or because they
 * were passed out of the SDK to user code), as the SDK may be using them internal concurrently.
 *
 * Similarly, the user must ensure any use of a SDK-owned user_handle is safe with respect to the
 * SDK passing other copies of the handle to the vtable concurrently.
 *
 * Since the schema_type is deallocated when the last copy of a user_handle is freed, it is
 * generally safe for a user_handle produced by X_deserialize() to depend on data owned by the
 * schema_type, and for a schema_type produced by X_serialize() to depend on data owned by the
 * user_handle. */

/**
 * An object used to represent a command request by either raw schema data or some user-defined
 * handle type.
 */
typedef struct Worker_CommandRequest {
  void* reserved;
  Worker_ComponentId component_id;
  Schema_CommandRequest* schema_type;
  Worker_CommandRequestHandle* user_handle;
} Worker_CommandRequest;

/**
 * An object used to represent a command response by either raw schema data or some user-defined
 * handle type.
 */
typedef struct Worker_CommandResponse {
  void* reserved;
  Worker_ComponentId component_id;
  Schema_CommandResponse* schema_type;
  Worker_CommandResponseHandle* user_handle;
} Worker_CommandResponse;

/**
 * An object used to represent a component data snapshot by either raw schema data or some
 * user-defined handle type.
 */
typedef struct Worker_ComponentData {
  void* reserved;
  Worker_ComponentId component_id;
  Schema_ComponentData* schema_type;
  Worker_ComponentDataHandle* user_handle;
} Worker_ComponentData;

/**
 * An object used to represent a component update by either raw schema data or some user-defined
 * handle type.
 */
typedef struct Worker_ComponentUpdate {
  void* reserved;
  Worker_ComponentId component_id;
  Schema_ComponentUpdate* schema_type;
  Worker_ComponentUpdateHandle* user_handle;
} Worker_ComponentUpdate;

/** Acquire a reference to extend the lifetime of a command request owned by the SDK. */
Worker_CommandRequest* Worker_AcquireCommandRequest(const Worker_CommandRequest* request);
/** Acquire a reference to extend the lifetime of a command response owned by the SDK. */
Worker_CommandResponse* Worker_AcquireCommandResponse(const Worker_CommandResponse* response);
/** Acquire a reference to extend the lifetime of a component data snapshot owned by the SDK. */
Worker_ComponentData* Worker_AcquireComponentData(const Worker_ComponentData* data);
/** Acquire a reference to extend the lifetime of a component update owned by the SDK. */
Worker_ComponentUpdate* Worker_AcquireComponentUpdate(const Worker_ComponentUpdate* update);
/** Release a reference obtained by Worker_AcquireCommandRequest. */
void Worker_ReleaseCommandRequest(Worker_CommandRequest* request);
/** Release a reference obtained by Worker_AcquireCommandResponse. */
void Worker_ReleaseCommandResponse(Worker_CommandResponse* response);
/** Release a reference obtained by Worker_AcquireComponentData. */
void Worker_ReleaseComponentData(Worker_ComponentData* data);
/** Release a reference obtained by Worker_AcquireComponentUpdate. */
void Worker_ReleaseComponentUpdate(Worker_ComponentUpdate* update);

/** Represents an entity with an ID and a component data snapshot. */
typedef struct Worker_Entity {
  /** The ID of the entity. */
  Worker_EntityId entity_id;
  /** Number of components for the entity. */
  uint32_t component_count;
  /** Array of initial component data for the entity. */
  const Worker_ComponentData* components;
} Worker_Entity;

typedef enum Worker_ConstraintType {
  WORKER_CONSTRAINT_TYPE_ENTITY_ID = 1,
  WORKER_CONSTRAINT_TYPE_COMPONENT = 2,
  WORKER_CONSTRAINT_TYPE_SPHERE = 3,
  WORKER_CONSTRAINT_TYPE_AND = 4,
  WORKER_CONSTRAINT_TYPE_OR = 5,
  WORKER_CONSTRAINT_TYPE_NOT = 6,
} Worker_ConstraintType;

typedef struct Worker_EntityIdConstraint { Worker_EntityId entity_id; } Worker_EntityIdConstraint;

typedef struct Worker_ComponentConstraint {
  Worker_ComponentId component_id;
} Worker_ComponentConstraint;

typedef struct Worker_SphereConstraint {
  double x;
  double y;
  double z;
  double radius;
} Worker_SphereConstraint;

typedef struct Worker_AndConstraint {
  uint32_t constraint_count;
  Worker_Constraint* constraints;
} Worker_AndConstraint;

typedef struct Worker_OrConstraint {
  uint32_t constraint_count;
  Worker_Constraint* constraints;
} Worker_OrConstraint;

typedef struct Worker_NotConstraint { Worker_Constraint* constraint; } Worker_NotConstraint;

/** A single query constraint. */
typedef struct Worker_Constraint {
  /** The type of constraint, defined using Worker_ConstraintType. */
  uint8_t constraint_type;
  /** Union with fields corresponding to each constraint type. */
  union {
    Worker_EntityIdConstraint entity_id_constraint;
    Worker_ComponentConstraint component_constraint;
    Worker_SphereConstraint sphere_constraint;
    Worker_AndConstraint and_constraint;
    Worker_OrConstraint or_constraint;
    Worker_NotConstraint not_constraint;
  };
} Worker_Constraint;

typedef enum Worker_ResultType {
  WORKER_RESULT_TYPE_COUNT = 1,
  WORKER_RESULT_TYPE_SNAPSHOT = 2,
} Worker_ResultType;

/** An entity query. */
typedef struct Worker_EntityQuery {
  /** The constraint for this query. */
  Worker_Constraint constraint;
  /** Result type for this query, using Worker_ResultType. */
  uint8_t result_type;
  /** Number of component IDs in the array for a snapshot result type. */
  uint32_t snapshot_result_type_component_id_count;
  /** Pointer to component ID data for a snapshot result type. NULL means all component IDs. */
  const Worker_ComponentId* snapshot_result_type_component_ids;
} Worker_EntityQuery;

/** An interest override for a particular (entity ID, component ID) pair. */
typedef struct Worker_InterestOverride {
  /** The ID of the component for which interest is being overridden. */
  uint32_t component_id;
  /** Whether the worker is interested in this component. */
  uint8_t is_interested;
} Worker_InterestOverride;

/** Worker attributes that are part of a worker's runtime configuration. */
typedef struct Worker_WorkerAttributes {
  /** Number of worker attributes. */
  uint32_t attribute_count;
  /** Will be NULL if there are no attributes associated with the worker. */
  const char** attributes;
} Worker_WorkerAttributes;

/* The ops are placed in the same order everywhere. This order should be used everywhere there is
   some similar code for each op (including wrapper APIs, definitions, function calls, thunks, and
   so on).

   (SECTION 1) GLOBAL ops, which do not depend on any entity. */

/** Data for a log message from the SDK. */
typedef struct Worker_DisconnectOp {
  /** The reason for the disconnect. */
  const char* reason;
} Worker_DisconnectOp;

/** Data for a FlagUpdate operation. */
typedef struct Worker_FlagUpdateOp {
  /** The name of the updated worker flag. */
  const char* name;
  /**
   * The new value of the updated worker flag.
   * A null value indicates that the flag has been deleted.
   */
  const char* value;
} Worker_FlagUpdateOp;

/** Data for a log message from the SDK. */
typedef struct Worker_LogMessageOp {
  /** The severity of the log message; defined in the Worker_LogLevel enumeration. */
  uint8_t level;
  /** The message. */
  const char* message;
} Worker_LogMessageOp;

/** Data for a set of built-in metrics reported by the SDK. */
typedef struct Worker_MetricsOp { Worker_Metrics metrics; } Worker_MetricsOp;

/** Data for a critical section boundary (enter or leave) operation. */
typedef struct Worker_CriticalSectionOp {
  /** Whether the protocol is entering a critical section (true) or leaving it (false). */
  uint8_t in_critical_section;
} Worker_CriticalSectionOp;

/* (SECTION 2) ENTITY-SPECIFIC ops, which do not depend on any component. */

/** Data for an AddEntity operation. */
typedef struct Worker_AddEntityOp {
  /** The ID of the entity that was added to the worker's view of the simulation. */
  Worker_EntityId entity_id;
} Worker_AddEntityOp;

/** Data for a RemoveEntity operation. */
typedef struct Worker_RemoveEntityOp {
  /** The ID of the entity that was removed from the worker's view of the simulation. */
  Worker_EntityId entity_id;
} Worker_RemoveEntityOp;

/** Data for a ReserveEntityIdsResponse operation. */
typedef struct Worker_ReserveEntityIdsResponseOp {
  /** The ID of the reserve entity ID request for which there was a response. */
  Worker_RequestId request_id;
  /** Status code of the response, using Worker_StatusCode. */
  uint8_t status_code;
  /** The error message. */
  const char* message;
  /**
   * If successful, an ID which is the first in a contiguous range of newly allocated entity
   * IDs which are guaranteed to be unused in the current deployment.
   */
  Worker_EntityId first_entity_id;
  /** If successful, the number of IDs reserved in the contiguous range, otherwise 0. */
  uint32_t number_of_entity_ids;
} Worker_ReserveEntityIdsResponseOp;

/** Data for a CreateEntity operation. */
typedef struct Worker_CreateEntityResponseOp {
  /** The ID of the request for which there was a response. */
  Worker_RequestId request_id;
  /** Status code of the response, using Worker_StatusCode. */
  uint8_t status_code;
  /** The error message. */
  const char* message;
  /** If successful, the entity ID of the newly created entity. */
  Worker_EntityId entity_id;
} Worker_CreateEntityResponseOp;

/** Data for a DeleteEntity operation. */
typedef struct Worker_DeleteEntityResponseOp {
  /** The ID of the delete entity request for which there was a command response. */
  Worker_RequestId request_id;
  /** The ID of the target entity of this request. */
  Worker_EntityId entity_id;
  /** Status code of the response, using Worker_StatusCode. */
  uint8_t status_code;
  /** The error message. */
  const char* message;
} Worker_DeleteEntityResponseOp;

/** A response indicating the result of an entity query request. */
typedef struct Worker_EntityQueryResponseOp {
  /** The ID of the entity query request for which there was a response. */
  Worker_RequestId request_id;
  /** Status code of the response, using Worker_StatusCode. */
  uint8_t status_code;
  /** The error message. */
  const char* message;
  /**
   * Number of entities in the result set. Reused to indicate the result itself for CountResultType
   * queries.
   */
  uint32_t result_count;
  /**
   * Array of entities in the result set. Will be NULL if the query was a count query. Snapshot data
   * in the result is deserialized with the corresponding vtable Deserialize function and freed with
   * the vtable Free function when the OpList is destroyed.
   */
  const Worker_Entity* results;
} Worker_EntityQueryResponseOp;

/* (SECTION 3) COMPONENT-SPECIFIC ops. */

/** Data for an AddComponent operation. */
typedef struct Worker_AddComponentOp {
  /** The ID of the entity for which a component was added. */
  Worker_EntityId entity_id;
  /**
   * The initial data for the new component. Deserialized with the corresponding vtable Deserialize
   * function and freed with the vtable Free function when the OpList is destroyed.
   */
  Worker_ComponentData data;
} Worker_AddComponentOp;

/** Data for a RemoveComponent operation. */
typedef struct Worker_RemoveComponentOp {
  /** The ID of the entity for which a component was removed. */
  Worker_EntityId entity_id;
  /** The ID of the component that was removed. */
  Worker_ComponentId component_id;
} Worker_RemoveComponentOp;

/** Data for an AuthorityChange operation. */
typedef struct Worker_AuthorityChangeOp {
  /** The ID of the entity for which there was an authority change. */
  Worker_EntityId entity_id;
  /** The ID of the component over which the worker's authority has changed. */
  Worker_ComponentId component_id;
  /** The authority state of the component, using the Worker_Authority enumeration. */
  uint8_t authority;
} Worker_AuthorityChangeOp;

/** Data for a ComponentUpdate operation. */
typedef struct Worker_ComponentUpdateOp {
  /** The ID of the entity for which there was a component update. */
  Worker_EntityId entity_id;
  /**
   * The new component data for the updated entity. Deserialized with the corresponding vtable
   * Deserialize function and freed with the vtable Free function when the OpList is destroyed.
   */
  Worker_ComponentUpdate update;
} Worker_ComponentUpdateOp;

/** Data for a CommandRequest operation. */
typedef struct Worker_CommandRequestOp {
  /** The incoming command request ID. */
  Worker_RequestId request_id;
  /** The ID of the entity for which there was a command request. */
  Worker_EntityId entity_id;
  /** Upper bound on request timeout provided by the platform. */
  uint32_t timeout_millis;
  /** The ID of the worker that sent the request. */
  const char* caller_worker_id;
  /** The attributes of the worker that sent the request. */
  Worker_WorkerAttributes caller_attribute_set;
  /**
   * The command request data. Deserialized with the corresponding vtable Deserialize function and
   * freed with the vtable Free function when the OpList is destroyed.
   */
  Worker_CommandRequest request;
} Worker_CommandRequestOp;

/** Data for a CommandResponse operation. */
typedef struct Worker_CommandResponseOp {
  /** The ID of the command request for which there was a command response. */
  Worker_RequestId request_id;
  /** The ID of the entity originally targeted by the command request. */
  Worker_EntityId entity_id;
  /** Status code of the response, using Worker_StatusCode. */
  uint8_t status_code;
  /** The error message. */
  const char* message;
  /**
   * The command response data. Deserialized with the corresponding vtable Deserialize function and
   * freed with the vtable Free function when the OpList is destroyed.
   */
  Worker_CommandResponse response;
  /** The command ID given to Worker_Connection_SendCommandRequest. */
  uint32_t command_id;
} Worker_CommandResponseOp;

/** Enum defining different possible op types. */
typedef enum Worker_OpType {
  WORKER_OP_TYPE_DISCONNECT = 1,
  WORKER_OP_TYPE_FLAG_UPDATE = 2,
  WORKER_OP_TYPE_LOG_MESSAGE = 3,
  WORKER_OP_TYPE_METRICS = 4,
  WORKER_OP_TYPE_CRITICAL_SECTION = 5,
  WORKER_OP_TYPE_ADD_ENTITY = 6,
  WORKER_OP_TYPE_REMOVE_ENTITY = 7,
  WORKER_OP_TYPE_RESERVE_ENTITY_IDS_RESPONSE = 8,
  WORKER_OP_TYPE_CREATE_ENTITY_RESPONSE = 9,
  WORKER_OP_TYPE_DELETE_ENTITY_RESPONSE = 10,
  WORKER_OP_TYPE_ENTITY_QUERY_RESPONSE = 11,
  WORKER_OP_TYPE_ADD_COMPONENT = 12,
  WORKER_OP_TYPE_REMOVE_COMPONENT = 13,
  WORKER_OP_TYPE_AUTHORITY_CHANGE = 14,
  WORKER_OP_TYPE_COMPONENT_UPDATE = 15,
  WORKER_OP_TYPE_COMMAND_REQUEST = 16,
  WORKER_OP_TYPE_COMMAND_RESPONSE = 17,
} Worker_OpType;

/** Data for a single op contained within an op list. */
typedef struct Worker_Op {
  /** The type of this op, defined in Worker_OpType. */
  uint8_t op_type;
  union {
    Worker_DisconnectOp disconnect;
    Worker_FlagUpdateOp flag_update;
    Worker_LogMessageOp log_message;
    Worker_MetricsOp metrics;
    Worker_CriticalSectionOp critical_section;
    Worker_AddEntityOp add_entity;
    Worker_RemoveEntityOp remove_entity;
    Worker_ReserveEntityIdsResponseOp reserve_entity_ids_response;
    Worker_CreateEntityResponseOp create_entity_response;
    Worker_DeleteEntityResponseOp delete_entity_response;
    Worker_EntityQueryResponseOp entity_query_response;
    Worker_AddComponentOp add_component;
    Worker_RemoveComponentOp remove_component;
    Worker_AuthorityChangeOp authority_change;
    Worker_ComponentUpdateOp component_update;
    Worker_CommandRequestOp command_request;
    Worker_CommandResponseOp command_response;
  };
} Worker_Op;

/** An op list, usually returned by WorkerProtocol_Connection_GetOpList. */
typedef struct Worker_OpList {
  Worker_Op* ops;
  uint32_t op_count;
} Worker_OpList;

/** Parameters for configuring a RakNet connection. Used by Worker_NetworkParameters. */
typedef struct Worker_RakNetNetworkParameters {
  /** Time (in milliseconds) that RakNet should use for its heartbeat protocol. */
  uint32_t heartbeat_timeout_millis;
} Worker_RakNetNetworkParameters;

/** Parameters for configuring a TCP connection. Used by Worker_NetworkParameters. */
typedef struct Worker_TcpNetworkParameters {
  /** The number of multiplexed TCP connections to use. */
  uint8_t multiplex_level;
  /** Size in bytes of the TCP send buffer. */
  uint32_t send_buffer_size;
  /** Size in bytes of the TCP receive buffer. */
  uint32_t receive_buffer_size;
  /** Whether to enable TCP_NODELAY. */
  uint8_t no_delay;
} Worker_TcpNetworkParameters;

/** Network connection type used by the Worker_NetworkParameters struct. */
typedef enum Worker_NetworkConnectionType {
  /** Use this flag to connect over TCP. */
  WORKER_NETWORK_CONNECTION_TYPE_TCP = 0,
  /** Use this flag to connect over RakNet. */
  WORKER_NETWORK_CONNECTION_TYPE_RAKNET = 1,
} Worker_NetworkConnectionType;

/** Parameters for configuring the network connection. */
typedef struct Worker_NetworkParameters {
  /**
   * Set this flag to non-zero to connect to SpatialOS using the externally-visible IP address. This
   * flag must be set when connecting externally (i.e. from outside the cloud) to a cloud
   * deployment.
   */
  uint8_t use_external_ip;
  /**
   * Type of network connection to use when connecting to SpatialOS, defined in
   * Worker_NetworkConnectionType.
   */
  uint8_t connection_type;
  /** Parameters used if the WORKER_NETWORK_RAKNET flag is set. */
  Worker_RakNetNetworkParameters raknet;
  /** Parameters used if the WORKER_NETWORK_TCP flag is set. */
  Worker_TcpNetworkParameters tcp;
  /** Timeout for the connection to SpatialOS to be established. */
  uint64_t connection_timeout_millis;
} Worker_NetworkParameters;

/**
 * Tuning parameters for configuring protocol logging in the SDK. Used by
 * Worker_ConnectionParameters.
 */
typedef struct Worker_ProtocolLoggingParameters {
  /** Log file names are prefixed with this prefix, are numbered, and have the extension .log. */
  const char* log_prefix;
  /**
   * Maximum number of log files to keep. Note that logs from any previous protocol logging
   * sessions will be overwritten.
   */
  uint32_t max_log_files;
  /** Once the size of a log file reaches this size, a new log file is created. */
  uint32_t max_log_file_size_bytes;
} Worker_ProtocolLoggingParameters;

/** Parameters for creating a Worker_Connection and connecting to SpatialOS. */
typedef struct Worker_ConnectionParameters {
  /** Worker type (platform). */
  const char* worker_type;

  /** Network parameters. */
  Worker_NetworkParameters network;

  /**
   * Number of messages that can be stored on the send queue. When the send queue is full, calls to
   * Worker_Connection_Send functions can block.
   */
  uint32_t send_queue_capacity;
  /**
   * Number of messages that can be stored on the receive queue. When the receive queue is full,
   * SpatialOS can apply QoS and drop messages to the worker.
   */
  uint32_t receive_queue_capacity;
  /**
   * Number of messages logged by the SDK that can be stored in the log message queue. When the log
   * message queue is full, messages logged by the SDK can be dropped.
   */
  uint32_t log_message_queue_capacity;
  /**
   * The Connection tracks several internal metrics, such as send and receive queue statistics. This
   * parameter controls how frequently the Connection will return a MetricsOp reporting its built-in
   * metrics. If set to zero, this functionality is disabled.
   */
  uint32_t built_in_metrics_report_period_millis;

  /** Parameters for configuring protocol parameters. */
  Worker_ProtocolLoggingParameters protocol_logging;
  /** Whether to enable protocol logging at startup. */
  uint8_t enable_protocol_logging_at_startup;

  /** Number of component vtables. */
  uint32_t component_vtable_count;
  /** Component vtable for each component that the connection will deal with. */
  const Worker_ComponentVtable* component_vtables;
  /** Default vtable used when a component is not registered. Only used if not NULL. */
  const Worker_ComponentVtable* default_component_vtable;
} Worker_ConnectionParameters;

/** Parameters for authenticating using a SpatialOS login token. */
typedef struct Worker_LoginTokenCredentials {
  /** The token would typically be provided on the command-line by the SpatialOS launcher. */
  const char* token;
} Worker_LoginTokenCredentials;

/** Parameters for authenticating using Steam credentials. */
typedef struct Worker_SteamCredentials {
  /**
   * Steam ticket for the steam app ID and publisher key corresponding to the project name specified
   * in the Worker_LocatorParameters. Typically obtained from the steam APIs.
   */
  const char* ticket;
  /**
   * Deployment tag to request access for. If non-empty, must match the following regex:
   * [A-Za-z0-9][A-Za-z0-9_]*
   */
  const char* deployment_tag;
} Worker_SteamCredentials;

/** Locator credentials type used by the Worker_LocatorParameters struct. */
typedef enum Worker_LocatorCredentialsTypes {
  WORKER_LOCATOR_LOGIN_TOKEN_CREDENTIALS = 1,
  WORKER_LOCATOR_STEAM_CREDENTIALS = 2,
} Worker_LocatorCredentialsTypes;

/** Parameters for authenticating and logging in to a SpatialOS deployment. */
typedef struct Worker_LocatorParameters {
  /** The name of the SpatialOS project. */
  const char* project_name;
  /**
   * Type of credentials to use when authenticating via the Locator, defined in
   * Worker_LocatorCredentialsTypes
   */
  uint8_t credentials_type;
  /** Parameters used if the WORKER_LOGIN_TOKEN_CREDENTIALS flag is set. */
  Worker_LoginTokenCredentials login_token;
  /** Parameters used if the WORKER_STEAM_CREDENTIALS flag is set. */
  Worker_SteamCredentials steam;
  /** Parameters for configuring logging. */
  Worker_ProtocolLoggingParameters logging;
  /** Whether to enable logging for the Locator flow. */
  uint8_t enable_logging;
} Worker_LocatorParameters;

/** Details of a specific deployment obtained via Worker_Locator_GetDeploymentListAsync. */
typedef struct Worker_Deployment {
  /** Name of the deployment. */
  const char* deployment_name;
  /** The name of the assembly used by this deployment. */
  const char* assembly_name;
  /** Description of the deployment. */
  const char* description;
  /** Number of users currently connected to the deployment. */
  uint32_t users_connected;
  /** Total user capacity of the deployment. */
  uint32_t users_capacity;
} Worker_Deployment;

/** A deployment list obtained via Worker_Locator_GetDeploymentListAsync. */
typedef struct Worker_DeploymentList {
  /** Number of deployments. */
  uint32_t deployment_count;
  /** Array of deployments. */
  Worker_Deployment* deployments;
  /** Will be non-NULL if an error occurred. */
  const char* error;
} Worker_DeploymentList;

/**
 * A queue status update when connecting to a deployment via Worker_Locator_ConnectAsync.
 */
typedef struct Worker_QueueStatus {
  /** Position in the queue. Decreases as we advance to the front of the queue. */
  uint32_t position_in_queue;
  /** Will be non-NULL if an error occurred. */
  const char* error;
} Worker_QueueStatus;

/** Component update parameters. Used to modify the behaviour of a component update request. */
typedef struct Worker_UpdateParameters {
  /**
   * Controls how the update is sent back to the worker from which it was sent. Defined in the
   * Worker_ComponentUpdateLoopback enumeration.
   */
  uint8_t loopback;
} Worker_UpdateParameters;

/** Command parameters. Used to modify the behaviour of a command request. */
typedef struct Worker_CommandParameters {
  /**
   * Allow command requests to bypass the bridge when this worker is authoritative over the target
   * entity-component.
   */
  uint8_t allow_short_circuit;
} Worker_CommandParameters;

/** Locator callback typedef. */
typedef void Worker_DeploymentListCallback(void* user_data,
                                           const Worker_DeploymentList* deployment_list);
/** Locator callback typedef. */
typedef uint8_t Worker_QueueStatusCallback(void* user_data, const Worker_QueueStatus* queue_status);
/** Worker flags callback typedef. */
typedef void Worker_GetFlagCallback(void* user_data, const char* value);

/**
 * Returns a new Worker_ConnectionParameters with default values set.
 */
WORKER_API Worker_ConnectionParameters Worker_DefaultConnectionParameters();

/**
 * Creates a client which can be used to connect to a SpatialOS deployment via a locator service.
 * This is the standard flow used to connect a local worker to a cloud deployment.
 *
 * The hostname would typically be either "locator.improbable.io" (for production) or
 * "locator-staging.improbable.io" (for staging).
 */
WORKER_API Worker_Locator* Worker_Locator_Create(const char* hostname,
                                                 const Worker_LocatorParameters* params);
/** Frees resources for a Worker_Locator created with Worker_Locator_Create. */
WORKER_API void Worker_Locator_Destroy(Worker_Locator* locator);
/**
 * Queries the current list of deployments for the project given in the
 * Worker_LocatorParameters.
 */
WORKER_API Worker_DeploymentListFuture*
Worker_Locator_GetDeploymentListAsync(const Worker_Locator* locator);
/**
 * Connects to a specific deployment. The deployment name should be obtained by calling
 * Worker_Locator_GetDeploymentListAsync. The callback should return zero to cancel queuing,
 * or non-zero to continue queueing.
 *
 * Returns a Worker_ConnectionFuture that can be used to obtain a Worker_Connection
 * by using Worker_ConnectionFuture_Get. Caller is responsible for destroying it when no
 * longer needed by using Worker_ConnectionFuture_Destroy.
 */
WORKER_API Worker_ConnectionFuture*
Worker_Locator_ConnectAsync(const Worker_Locator* locator, const char* deployment_name,
                            const Worker_ConnectionParameters* params, void* data,
                            Worker_QueueStatusCallback* callback);

/**
 * Connect to a SpatialOS deployment via a receptionist. This is the flow used to connect a managed
 * worker running in the cloud alongside the deployment, and also to connect any local worker to a
 * (local or remote) deployment via a locally-running receptionist.
 *
 * The hostname and port would typically be provided by SpatialOS on the command-line, if this is a
 * managed worker on the cloud, or otherwise be predetermined (e.g. localhost:7777 for the default
 * receptionist of a locally-running deployment).
 *
 * Returns a Worker_ConnectionFuture that can be used to obtain a Worker_Connection
 * by using Worker_ConnectionFuture_Get. Caller is responsible for destroying it when no
 * longer needed by using Worker_ConnectionFuture_Destroy.
 */
WORKER_API Worker_ConnectionFuture* Worker_ConnectAsync(const char* hostname, uint16_t port,
                                                        const char* worker_id,
                                                        const Worker_ConnectionParameters* params);

/** Destroys a Worker_DeploymentListFuture. Blocks until the future has completed. */
WORKER_API void Worker_DeploymentListFuture_Destroy(Worker_DeploymentListFuture* future);
/**
 * Gets the result of a Worker_DeploymentListFuture, waiting for up to *timeout_millis to
 * become available (or forever if timeout_millis is NULL).
 *
 * It is an error to call this method again once it has succeeded (e.g. not timed out) once.
 */
WORKER_API void Worker_DeploymentListFuture_Get(Worker_DeploymentListFuture* future,
                                                const uint32_t* timeout_millis, void* data,
                                                Worker_DeploymentListCallback* callback);

/** Destroys a Worker_ConnectionFuture. Blocks until the future has completed. */
WORKER_API void Worker_ConnectionFuture_Destroy(Worker_ConnectionFuture* future);
/**
 * Gets the result of a Worker_ConnectionFuture, waiting for up to *timeout_millis to
 * become available (or forever if timeout_millis is NULL). It returns NULL in case of a timeout.
 *
 * It is an error to call this method again once it has succeeded (e.g. not timed out) once.
 */
WORKER_API Worker_Connection* Worker_ConnectionFuture_Get(Worker_ConnectionFuture* future,
                                                          const uint32_t* timeout_millis);

/**
 * Frees resources for a Worker_Connection created with Worker_ConnectAsync or
 * Worker_Locator_ConnectAsync.
 */
WORKER_API void Worker_Connection_Destroy(Worker_Connection* connection);
/** Sends a log message from the worker to SpatialOS. */
WORKER_API void Worker_Connection_SendLogMessage(Worker_Connection* connection,
                                                 const Worker_LogMessage* log_message);
/** Sends metrics data for the worker to SpatialOS. */
WORKER_API void Worker_Connection_SendMetrics(Worker_Connection* connection,
                                              const Worker_Metrics* metrics);
/** Requests SpatialOS to reserve multiple entity IDs. */
WORKER_API Worker_RequestId Worker_Connection_SendReserveEntityIdsRequest(
    Worker_Connection* connection, const uint32_t number_of_entity_ids,
    const uint32_t* timeout_millis);
/**
 * Requests SpatialOS to create an entity. The entity data is serialized immediately using the
 * corresponding vtable Serialize function; no copy is made or ownership transferred.
 */
WORKER_API Worker_RequestId Worker_Connection_SendCreateEntityRequest(
    Worker_Connection* connection, uint32_t component_count, const Worker_ComponentData* components,
    const Worker_EntityId* entity_id, const uint32_t* timeout_millis);
/** Requests SpatialOS to delete an entity. */
WORKER_API Worker_RequestId Worker_Connection_SendDeleteEntityRequest(
    Worker_Connection* connection, Worker_EntityId entity_id, const uint32_t* timeout_millis);
/** Queries SpatialOS for entity data. */
WORKER_API Worker_RequestId Worker_Connection_SendEntityQueryRequest(
    Worker_Connection* connection, const Worker_EntityQuery* entity_query,
    const uint32_t* timeout_millis);
/**
 * Sends a component update for the given entity to SpatialOS. Note that the sent component update
 * is added as an operation to the operation list and will be returned by a subsequent call to
 * Worker_connection_GetOpList. The update data is copied with the corresponding vtable Copy
 * function and the copy is later freed with the vtable Free function.
 */
WORKER_API void
Worker_Connection_SendComponentUpdate(Worker_Connection* connection, Worker_EntityId entity_id,
                                      const Worker_ComponentUpdate* component_update,
                                      const Worker_UpdateParameters* update_parameters);
/**
 * Sends a command request targeting the given entity and component to SpatialOS. If timeout_millis
 * is null, the default will be used. The request data is copied with the corresponding vtable Copy
 * function and the copy is later freed with the vtable Free function.
 *
 * The command_id parameter has no effect other than being exposed in the
 * Worker_CommandResponseOp so that callers can correctly handle command failures.
 *
 * The command parameters argument must not be NULL.
 */
WORKER_API Worker_RequestId Worker_Connection_SendCommandRequest(
    Worker_Connection* connection, Worker_EntityId entity_id, const Worker_CommandRequest* request,
    uint32_t command_id, const uint32_t* timeout_millis,
    const Worker_CommandParameters* command_parameters);
/**
 * Sends a command response for the given request ID to SpatialOS. The response data is copied with
 * the corresponding vtable Copy function and the copy is later freed with the vtable Free function.
 */
WORKER_API void Worker_Connection_SendCommandResponse(Worker_Connection* connection,
                                                      Worker_RequestId request_id,
                                                      const Worker_CommandResponse* response);
/** Sends an explicit failure for the given command request ID to SpatialOS. */
WORKER_API void Worker_Connection_SendCommandFailure(Worker_Connection* connection,
                                                     Worker_RequestId request_id,
                                                     const char* message);
/**
 * Sends a diff-based component interest update for the given entity to SpatialOS. By default, the
 * worker receives data for all entities according to the default component interest specified in
 * its bridge settings. This function allows interest override by (entity ID, component ID) pair to
 * force the data to either always be sent or never be sent. Note that this does not apply if the
 * worker is _authoritative_ over a particular (entity ID, component ID) pair, in which case the
 * data is always sent.
 */
WORKER_API void
Worker_Connection_SendComponentInterest(Worker_Connection* connection, Worker_EntityId entity_id,
                                        const Worker_InterestOverride* interest_override,
                                        uint32_t interest_override_count);
/**
 * Sends an acknowledgement of the receipt of an AuthorityLossImminent authority change for a
 * component. Sending the acknowledgement signifies that this worker is ready to lose authority
 * over the component.
 */
WORKER_API void Worker_Connection_SendAuthorityLossImminentAcknowledgement(
    Worker_Connection* connection, Worker_EntityId entity_id, Worker_ComponentId component_id);
/**
 * Enables or disables protocol logging. Logging uses the parameters specified when the connection
 * was created. Enabling it when already enabled, or disabling it when already disabled, do nothing.
 *
 * Note that logs from any previous protocol logging sessions will be overwritten.
 */
WORKER_API void Worker_Connection_SetProtocolLoggingEnabled(Worker_Connection* connection,
                                                            uint8_t enabled);
/** Returns true if the connection has been successfully created and communication is ongoing. */
WORKER_API uint8_t Worker_Connection_IsConnected(const Worker_Connection* connection);
/**
 * Retrieves the ID of the worker as assigned by the runtime. The returned pointer points to data
 * that is owned by the SDK and will remain valid for the lifetime of the connection.
 */
WORKER_API const char* Worker_Connection_GetWorkerId(const Worker_Connection* connection);
/**
 * Retrieves the attributes associated with the worker at runtime. The result to data that is owned
 * by the SDK and will remain valid for the lifetime of the connection.
 */
WORKER_API const Worker_WorkerAttributes*
Worker_Connection_GetWorkerAttributes(const Worker_Connection* connection);
/**
 * Queries the worker flag with the given name. If the worker flag does not exist, the value will
 * be NULL.
 *
 * Worker flags are remotely configurable and may change during the runtime of the worker,
 * including addition and deletion.
 */
WORKER_API void Worker_Connection_GetFlag(const Worker_Connection* connection, const char* name,
                                          void* user_data, Worker_GetFlagCallback* callback);
/**
 * Retrieves the list of operations that have occurred since the last call to this function.
 *
 * If timeout_millis is non-zero, the function will block until there is at least one operation to
 * return, or the timeout has been exceeded. If the timeout is exceeded, an empty list will be
 * returned.
 *
 * If timeout_millis is zero the function is non-blocking.
 *
 * It is the caller's responsibility to destroy the returned Worker_OpList with the
 * Worker_OpList_Destroy function.
 */
WORKER_API Worker_OpList* Worker_Connection_GetOpList(Worker_Connection* connection,
                                                      uint32_t timeout_millis);
/** Frees resources for Worker_OpList returned by Worker_Connection_GetOpList. */
WORKER_API void Worker_OpList_Destroy(Worker_OpList* op_list);

typedef struct Worker_SnapshotParameters {
  /** Number of component vtables. */
  uint32_t component_vtable_count;
  /** Component vtable for each component that the connection will deal with. */
  const Worker_ComponentVtable* component_vtables;
  /** Default vtable used when a component is not registered. Only used if not NULL. */
  const Worker_ComponentVtable* default_component_vtable;
} Worker_SnapshotParameters;

/**
 * Opens a Worker_SnapshotInputStream. The caller must manage the memory of the
 * returned Worker_SnapshotInputStream* by calling Worker_SnapshotInputStream to
 * write the EOF and release resources.
 *
 * If an error occurs, a pointer to a Worker_SnapshotInputStream is still returned.
 * Calling Worker_SnapshotInputStream_GetError with this pointer will return
 * an error message describing any error that occured. In the event of an error, the caller still
 * must release the memory of the Worker_SnapshotInputStream by calling
 * Worker_SnapshotInputStream.
 */
WORKER_API Worker_SnapshotInputStream*
Worker_SnapshotInputStream_Create(const char* filename, const Worker_SnapshotParameters* params);

/** Closes the SnapshotInputStream and releases its resources. */
WORKER_API void Worker_SnapshotInputStream_Destroy(Worker_SnapshotInputStream* input_stream);

/**
 * Returns zero (false) if the Worker_SnapshotInputStream has reached the EOF
 * of the Snapshot.
 */
WORKER_API uint8_t Worker_SnapshotInputStream_HasNext(Worker_SnapshotInputStream* input_stream);

/**
 * Reads next Worker_Entity* entity from input_stream.
 *
 * Worker_SnapshotInputStream_ReadEntity manages the memory for the returned entity internally. The
 * next call to Worker_SnapshotInputStream_ReadEntity or Worker_SnapshotInputStream_Destroy
 * invalidates this value; use Worker_AcquireComponentData as usual to preserve component data.
 */
WORKER_API const Worker_Entity*
Worker_SnapshotInputStream_ReadEntity(Worker_SnapshotInputStream* input_stream);

/**
 * Must be called after any operation on Worker_SnapshotInputStream to get the error
 * message associated with previous operation. If error is null, no error occured.
 *
 * Returns a read only const char* representation of the error message.
 */
WORKER_API const char*
Worker_SnapshotInputStream_GetError(Worker_SnapshotInputStream* input_stream);

/**
 * Opens Worker_SnapshotOutputStream stream. The caller must manage the memory of the
 * returned Worker_SnapshotOutputStream* by calling
 * Worker_SnapshotOutputStream_Destroy to write the EOF and release resources.
 *
 * If an error occurs, a pointer to a Worker_SnapshotOutputStream is still returned.
 * Calling Worker_SnapshotOutputStream_GetError with this pointer will return
 * an error message describing any error that occured. In the event of an error, the caller still
 * must release the memory of the Worker_SnapshotOutputStream by calling
 * Worker_SnapshotOutputStream_Destroy.
 */
WORKER_API Worker_SnapshotOutputStream*
Worker_SnapshotOutputStream_Create(const char* filename, const Worker_SnapshotParameters* params);

/** Closes the snapshot output stream and releases its resources. */
WORKER_API void Worker_SnapshotOutputStream_Destroy(Worker_SnapshotOutputStream* output_stream);

/**
 * Writes next entity_id, entity pair from input. Must call
 * Worker_SnapshotOutputStream_GetError
 * to get any error that occured during operation.
 * Returns non-zero (true) if the write was successful.
 */
WORKER_API uint8_t Worker_SnapshotOutputStream_WriteEntity(
    Worker_SnapshotOutputStream* output_stream, const Worker_Entity* entity);

/**
 * Must be called after any operation on Worker_SnapshotOutputStream to get the error
 * message associated with previous operation. If error is null, no error occured.
 *
 * Returns a read only const char* representation of the error message.
 */
WORKER_API const char*
Worker_SnapshotOutputStream_GetError(Worker_SnapshotOutputStream* output_stream);

#ifdef __cplusplus
}
#endif /* __cplusplus */

#endif /* WORKER_SDK_C_INCLUDE_IMPROBABLE_C_WORKER_H */
