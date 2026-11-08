using System;
using PolicyService.Test.TestData;
using Xunit;
using static Xunit.Assert;

namespace PolicyService.Test.Domain;

public class PolicyTerminationTest
{
    [Fact]
    public void CanTerminateActivePolicyInTheMiddleOfCoverPeriod()
    {
        var baseDate = DateTime.Today;
        var offer = OfferFactory.NewOfferValidUntil(baseDate.AddDays(5), baseDate);

        var policy = offer.Buy(PolicyHolderFactory.Abc());

        var terminationResult = policy.Terminate(baseDate.AddDays(3));

        PolicyAssert
            .AssertThat(policy)
            .HasVersions(2)
            .HasVersion(2)
            .StatusIsTerminated();

        PolicyVersionAssert
            .AssertThat(terminationResult.TerminalVersion)
            .TotalPremiumIs(180M);
    }

    [Fact]
    public void CannotTerminateTerminatedPolicy()
    {
        var policy = PolicyFactory.AlreadyTerminatedPolicy();

        Exception ex = Throws<ApplicationException>(() => policy.Terminate(DateTime.Now));
        Equal($"Policy {policy.Number} is already terminated", ex.Message);
    }
}