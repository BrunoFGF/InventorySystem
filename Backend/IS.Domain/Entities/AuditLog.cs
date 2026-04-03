namespace IS.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}