using AegisCore.Core.Models;

namespace AegisCore.Core.Interfaces;

public interface IScanner
{
    Task<IEnumerable<ScanResult>> ScanDirectoryAsync(string path, CancellationToken cancellationToken = default);
    Task<ScanResult> ScanFileAsync(string filePath, CancellationToken cancellationToken = default);
}
