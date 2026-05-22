using AegisCore.Core.Models;

namespace AegisCore.Core.Interfaces;

public interface IIntegrityMonitor
{
    Task StartMonitoringAsync(CancellationToken cancellationToken = default);
    Task StopMonitoringAsync();
    event EventHandler<IntegrityViolation>? ViolationDetected;
}
