using Auth.Features.Common;

namespace Auth.Features.Customer.Common;

public record CustomerProfileDto : UserProfileDto
{
    public string Address { get; set; } = default!;
}