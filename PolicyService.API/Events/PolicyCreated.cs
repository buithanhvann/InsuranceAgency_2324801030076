using System;
using MediatR;

namespace PolicyService.Api.Events;

public class PolicyCreated : INotification
{
    public string PolicyNumber { get; set; } = default!;
    public DateTime PolicyFrom { get; set; }
    public DateTime PolicyTo { get; set; }
    public PolicyHolder PolicyHolder { get; set; } = default!;
    public string ProductCode { get; set; } = default!;
    public decimal TotalPremium { get; set; }
    public string AgentLogin { get; set; } = default!;
}

public class PolicyHolder
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
