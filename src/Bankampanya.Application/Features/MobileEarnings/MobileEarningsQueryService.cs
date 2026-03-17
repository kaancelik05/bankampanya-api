using Bankampanya.Application.Features.MobileEarnings.Dtos;

namespace Bankampanya.Application.Features.MobileEarnings;

public sealed class MobileEarningsQueryService(IMobileEarningsQueryRepository repository)
{
    public Task<MobileEarningsDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
        => repository.GetDashboardAsync(cancellationToken);
}
