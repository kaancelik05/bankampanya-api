using Bankampanya.Application.Features.Dashboard.Dtos;

namespace Bankampanya.Application.Features.Dashboard;

public sealed class AdminDashboardService(IAdminDashboardRepository repository)
{
    public Task<AdminDashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken)
        => repository.GetSummaryAsync(cancellationToken);
}
