namespace AegisCore.Core.Models;

public class ScanResult
{
    public Guid Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public ScanStatus Status { get; set; }
    public DateTime ScannedAt { get; set; }
    public string? Message { get; set; }
    public int RiskScore { get; set; }
}

public enum ScanStatus
{
    Unknown,
    Safe,
    Suspicious,
    Malicious,
    Error
}
