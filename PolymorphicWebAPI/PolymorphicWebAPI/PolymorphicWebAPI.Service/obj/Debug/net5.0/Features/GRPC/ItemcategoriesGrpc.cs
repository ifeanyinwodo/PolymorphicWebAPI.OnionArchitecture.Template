// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Features/GRPC/itemcategories.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace PolymorphicWebAPI.Service.Features.GRPC {
  public static partial class ItemCategories
  {
    static readonly string __ServiceName = "itemcategories.ItemCategories";

    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));
    static readonly grpc::Marshaller<global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories> __Marshaller_itemcategories_GetAllCategories = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories.Parser));
    static readonly grpc::Marshaller<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest> __Marshaller_itemcategories_GetCategoryRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest.Parser));
    static readonly grpc::Marshaller<global::PolymorphicWebAPI.Service.Features.GRPC.Category> __Marshaller_itemcategories_Category = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PolymorphicWebAPI.Service.Features.GRPC.Category.Parser));

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories> __Method_Get = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Get",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_itemcategories_GetAllCategories);

    static readonly grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories> __Method_GetCategory = new grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetCategory",
        __Marshaller_itemcategories_GetCategoryRequest,
        __Marshaller_itemcategories_GetAllCategories);

    static readonly grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category> __Method_Post = new grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Post",
        __Marshaller_itemcategories_Category,
        __Marshaller_itemcategories_Category);

    static readonly grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category> __Method_Update = new grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Update",
        __Marshaller_itemcategories_Category,
        __Marshaller_itemcategories_Category);

    static readonly grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Remove = new grpc::Method<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::Google.Protobuf.WellKnownTypes.Empty>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Remove",
        __Marshaller_itemcategories_GetCategoryRequest,
        __Marshaller_google_protobuf_Empty);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::PolymorphicWebAPI.Service.Features.GRPC.ItemcategoriesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ItemCategories</summary>
    [grpc::BindServiceMethod(typeof(ItemCategories), "BindService")]
    public abstract partial class ItemCategoriesBase
    {
      public virtual global::System.Threading.Tasks.Task Get(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::IServerStreamWriter<global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task GetCategory(global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest request, grpc::IServerStreamWriter<global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::PolymorphicWebAPI.Service.Features.GRPC.Category> Post(global::PolymorphicWebAPI.Service.Features.GRPC.Category request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::PolymorphicWebAPI.Service.Features.GRPC.Category> Update(global::PolymorphicWebAPI.Service.Features.GRPC.Category request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.Empty> Remove(global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ItemCategoriesBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Get, serviceImpl.Get)
          .AddMethod(__Method_GetCategory, serviceImpl.GetCategory)
          .AddMethod(__Method_Post, serviceImpl.Post)
          .AddMethod(__Method_Update, serviceImpl.Update)
          .AddMethod(__Method_Remove, serviceImpl.Remove).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ItemCategoriesBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Get, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories>(serviceImpl.Get));
      serviceBinder.AddMethod(__Method_GetCategory, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::PolymorphicWebAPI.Service.Features.GRPC.GetAllCategories>(serviceImpl.GetCategory));
      serviceBinder.AddMethod(__Method_Post, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category>(serviceImpl.Post));
      serviceBinder.AddMethod(__Method_Update, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::PolymorphicWebAPI.Service.Features.GRPC.Category, global::PolymorphicWebAPI.Service.Features.GRPC.Category>(serviceImpl.Update));
      serviceBinder.AddMethod(__Method_Remove, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::PolymorphicWebAPI.Service.Features.GRPC.GetCategoryRequest, global::Google.Protobuf.WellKnownTypes.Empty>(serviceImpl.Remove));
    }

  }
}
#endregion