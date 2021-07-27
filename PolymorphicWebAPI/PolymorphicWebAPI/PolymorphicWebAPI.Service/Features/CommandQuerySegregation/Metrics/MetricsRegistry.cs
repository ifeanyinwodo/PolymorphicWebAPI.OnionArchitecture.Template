using App.Metrics;
using App.Metrics.Counter;


namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics
{
    public static class MetricsRegistry
    {
        public static CounterOptions CreatedMediatorItemCategory => new CounterOptions
        {
            Name = "Created Mediator Category",
            Context = "Created Mediator Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedMediatorSingleItemCategory => new CounterOptions
        {
            Name = "Retrieved Mediator Single  Category",
            Context = "Retrieved Mediator Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedMediatorAllItemCategory => new CounterOptions
        {
            Name = "Retrieved Mediator All Category",
            Context = "Retrieved Mediator Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions CreatedRestItemCategory => new CounterOptions
        {
            Name = "Created Rest Category",
            Context = "Created Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions UpdatedRestItemCategory => new CounterOptions
        {
            Name = "Updated Rest Category",
            Context = "Updated Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedRestSingleItemCategory => new CounterOptions
        {
            Name = "Retrieved Rest Single  Category",
            Context = "Retrieved Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedRestAllItemCategory => new CounterOptions
        {
            Name = "Retrieved Rest All Category",
            Context = "Retrieved Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };
      


        public static CounterOptions FailedCreatedRestItemCategory => new CounterOptions
        {
            Name = "Failed Created Rest Category",
            Context = "Failed Created Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions FailedRetrievedRestItemCategory => new CounterOptions
        {
            Name = "Failed Retrieved Rest Single  Category",
            Context = "Failed Retrieved Rest Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };




        public static CounterOptions CreatedGraphQLItemCategory => new CounterOptions
        {
            Name = "Created GraphQL Category",
            Context = "Created GraphQL Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedGraphQLSingleItemCategory => new CounterOptions
        {
            Name = "Retrieved GraphQL Single  Category",
            Context = "Retrieved GraphQL Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions RetrievedGraphQLAllItemCategory => new CounterOptions
        {
            Name = "Retrieved GraphQL All Category",
            Context = "Retrieved GraphQL Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };



        public static CounterOptions FailedCreatedGraphQLItemCategory => new CounterOptions
        {
            Name = "Failed Created GraphQL Category",
            Context = "Created GraphQL Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions FailedRetrievedGraphQLItemCategory => new CounterOptions
        {
            Name = "Failed Retrieved GraphQL  Category",
            Context = "Failed Retrieved GraphQL Item Category for ItemCategory WebAPI",
            MeasurementUnit = Unit.Calls
        };


    }
}
