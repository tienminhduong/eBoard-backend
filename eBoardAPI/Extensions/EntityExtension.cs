using eBoardAPI.Entities;

namespace eBoardAPI.Extensions;

public static class EntityExtension
{
    public static void CalculateExpectedAmount(this FundIncome fundIncome, int numberOfStudents)
    {
        fundIncome.ExpectedAmount = fundIncome.AmountPerStudent * numberOfStudents;
    }

    public static void UpdateStatus(this FundIncomeDetail fundIncomeDetail, int expectAmount)
    {
        fundIncomeDetail.ContributionStatus = fundIncomeDetail.ContributedAmount >= expectAmount
            ? nameof(Consts.PaymentStatus.FULL)
            : nameof(Consts.PaymentStatus.PARTIAL);
    }
}