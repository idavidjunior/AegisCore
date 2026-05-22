namespace AegisCore.Core.Models;

public class IntegrityViolation
{
    public Guid Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public ViolationType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
    public bool IsResolved { get; set; }
}

public enum ViolationType
{
    FileModified,
    FileDeleted,
    FileCreated,
    RegistryModified,
    SignatureInvalid,
    HashMismatch
}
