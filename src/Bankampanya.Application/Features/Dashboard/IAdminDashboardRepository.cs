using Bankampanya.Application.Features.Dashboard.Dtos;

namespace Bankampanya.Application.Features.Dashboard;

public interface IAdminDashboardRepository
{
    Task<AdminDashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken);
}
