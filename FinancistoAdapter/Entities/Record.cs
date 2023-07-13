using System;

namespace FinancistoAdapter.Entities;

public abstract class Record : IRecord
{
    [RecordProperty("_id")]
    public virtual int Id { get; set; }
    
    [RecordProperty("updated_on", Converter = typeof(DateTimeConverter))]
    public virtual DateTime? UpdatedOn { get; set; }
}