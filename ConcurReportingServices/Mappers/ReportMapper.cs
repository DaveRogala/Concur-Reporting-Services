using ConcurExpense.Models;
using ConcurReportingDatabaseServices.Models;

namespace ConcurReporting.Mappers;

internal static class ReportMapper
{
    public static Report ReportFromDto(this ReportDto dto, DateTime utcNow)
    {
        return new Report
        {
            ConcurID = dto.ID,
            CompanyCode = dto.OrgUnit1?.Code,
            CostCenterCode = dto.OrgUnit2?.Code,
            Department = dto.OrgUnit3?.Code,
            LastModifiedDateTimeUtc = dto.LastModifiedDate ?? DateTime.MinValue,
            AmountDueCompanyCard = dto.AmountDueCompanyCard,
            AmountDueEmployee = dto.AmountDueEmployee,
            ApprovalStatusCode = dto.ApprovalStatusCode,
            ApprovalStatusName = dto.ApprovalStatusName,
            ApproverLoginID = dto.ApproverLoginID,
            ApproverName = dto.ApproverName,
            CountryISO2 = (dto.Country?.Trim()?.Length ?? 0) == 2 ? dto.Country : null,
            CountrySubdivision = dto.CountrySubdivision,
            CreateDate = dto.CreateDate,
            CurrencyCode = dto.CurrencyCode,
            EmployeeGroup = dto.Custom15?.Code,
            EverSentBack = dto.EverSentBack,
            HasException = dto.HasException,
            LastComment = dto.LastComment,
            LedgerName = dto.LedgerName,
            Name = dto.Name,
            OwnerLoginID = dto.OwnerLoginID,
            OwnerName = dto.OwnerName,
            PaidDate = dto.PaidDate,
            PaymentStatusCode = dto.PaymentStatusCode,
            PaymentStatusName = dto.PaymentStatusName,
            PersonalAmount = dto.PersonalAmount,
            PolicyID = dto.PolicyID,
            ProcessingPaymentDate = dto.ProcessingPaymentDate,
            ReceiptsReceived = dto.ReceiptsReceived,
            SubmitDate = dto.SubmitDate,
            Total = dto.Total,
            TotalApprovedAmount = dto.TotalApprovedAmount,
            UserDefinedDate = dto.UserDefinedDate,  
            VendorID = dto.Custom17?.Value,
            DateTimeAddedUtc = utcNow,
            DateTimeUpdatedUtc = utcNow

        };
    }
    public static Report UpdateReportFromDto(this Report entity,  ReportDto dto, DateTime utcNow)
    {
        entity.ConcurID = dto.ID;
        entity.CompanyCode = dto.OrgUnit1?.Code;
        entity.CostCenterCode = dto.OrgUnit2?.Code;
        entity.Department = dto.OrgUnit3?.Code;
        entity.LastModifiedDateTimeUtc = dto.LastModifiedDate ?? DateTime.MinValue;
        entity.AmountDueCompanyCard = dto.AmountDueCompanyCard;
        entity.AmountDueEmployee = dto.AmountDueEmployee;
        entity.ApprovalStatusCode = dto.ApprovalStatusCode;
        entity.ApprovalStatusName = dto.ApprovalStatusName;
        entity.ApproverLoginID = dto.ApproverLoginID;
        entity.ApproverName = dto.ApproverName;
        entity.CountryISO2 = (dto.Country?.Trim()?.Length ?? 0) == 2 ? dto.Country : null;
        entity.CountrySubdivision = dto.CountrySubdivision;
        entity.CreateDate = dto.CreateDate;
        entity.CurrencyCode = dto.CurrencyCode;
        entity.EmployeeGroup = dto.Custom15?.Code;
        entity.EverSentBack = dto.EverSentBack;
        entity.HasException = dto.HasException;
        entity.LastComment = dto.LastComment;
        entity.LedgerName = dto.LedgerName;
        entity.Name = dto.Name;
        entity.OwnerLoginID = dto.OwnerLoginID;
        entity.OwnerName = dto.OwnerName;
        entity.PaidDate = dto.PaidDate;
        entity.PaymentStatusCode = dto.PaymentStatusCode;
        entity.PaymentStatusName = dto.PaymentStatusName;
        entity.PersonalAmount = dto.PersonalAmount;
        entity.PolicyID = dto.PolicyID;
        entity.ProcessingPaymentDate = dto.ProcessingPaymentDate;
        entity.ReceiptsReceived = dto.ReceiptsReceived;
        entity.SubmitDate = dto.SubmitDate;
        entity.Total = dto.Total;
        entity.TotalApprovedAmount = dto.TotalApprovedAmount;
        entity.UserDefinedDate = dto.UserDefinedDate;
        entity.VendorID = dto.Custom17?.Value;
        entity.DateTimeUpdatedUtc = utcNow;

        return entity;
    }
}
