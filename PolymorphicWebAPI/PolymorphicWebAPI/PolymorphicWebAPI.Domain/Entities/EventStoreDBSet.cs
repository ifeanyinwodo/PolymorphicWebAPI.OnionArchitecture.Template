using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolymorphicWebAPI.Domain.Entities
{

    [Table("eventstore")]
    public  class EventStoreDBSet
    {
        [Column("id")]
        public virtual Guid Id { get; set; }

        [Column("data")]
        public virtual string Data { get; set; }

        [Column("version")]
        public virtual int Version { get; set; }

        [Column("createdat")]
        public virtual DateTime CreatedAt { get; set; }

        [Description("ignore")]
        [Column("sequence")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual int Sequence { get; set; }

        [Column("name")]
        public virtual string Name { get; set; }

      

        [Column("aggregateid")]
        public virtual string AggregateId { get; set; }
    }
}
