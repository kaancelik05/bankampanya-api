namespace Bankampanya.Application.Features.MobileEarnings;

public interface IMobileEarningsQueryRepository
{
    Task<MobileEarnings.Dtos.MobileEarningsDashboardDto> GetDashboardAsync(CancellationToken cancellationToken);
}
