using ConcurExpense.Models;
using ConcurReporting.Helpers;
using ConcurReportingDatabaseServices.Models;

namespace ConcurReporting.Mappers;

internal static class ItemizationMapper
{
    public static Itemization ItemizationFromDto(this ItemizationDto dto, DateTime utcNow)
    {
        return new Itemization
        {          
            ConcurID = dto.ID,
            CompanyCode = dto.OrgUnit1?.Code,
            CostCenterCode = dto.OrgUnit2?.Code,
            Department = dto.OrgUnit3?.Code,
            LastModifiedDateTimeUtc = dto.LastModified ?? DateTime.MinValue,

            Account = dto.Custom11?.Code,
            AllocationType = dto.AllocationType,
            ApprovedAmount = dto.ApprovedAmount,
            BillRuleRegOrRateNumber = dto.Custom2?.Value,
            CheckInDate = dto.Custom12?.Value?.ToDateOnly(),
            CheckOutDate = dto.Custom13?.Value?.ToDateOnly(),
            ContractOrRFPTitleAndIdentifyingNumber = dto.Custom7?.Value,
            ContractTitleIdentifyingNumberAndDate = dto.Custom8?.Value,
            Description = dto.Description,
            EntrySubtractFromGross = dto.Custom37?.Value?.ToDecimal(),
            ExpenseTypeCode = dto.ExpenseTypeCode,
            ExpenseTypeName = dto.ExpenseTypeName,
            HasComments = dto.HasComments,
            HasExceptions = dto.HasExceptions,
            IsBillable = dto.IsBillable,
            IsImageRequired = dto.IsImageRequired,
            IsPersonal = dto.IsPersonal,
            ItemsGiven = dto.Custom5?.Value,
            LengthInHours = dto.Custom1?.Value?.ToInt(),
            LocationCountryISO2 = (dto.LocationCountry?.Trim()?.Length ?? 0) == 2 ? dto.LocationCountry : null,
            LocationName = dto.LocationName,
            LocationSubdivision = dto.LocationSubdivision,
            NatureOfContact = dto.Custom3?.Value,
            NetTaxAmount = dto.Custom40?.Value?.ToDecimal(),
            NonTaxableNonDeductibleAmount = dto.Custom32?.Value,
            NonTaxDeductibleVATAmount = dto.Custom34?.Value,
            PersonalChargeConfirmation = dto.Custom14?.Value?.ToBoolean(),
            PersonDesignatedInContractOrRFP = dto.Custom4?.Code?.ToBoolean(),
            PostedAmount = dto.PostedAmount,
            ProjectCode = dto.OrgUnit6?.Code,
            ProjectDescription = dto.OrgUnit6?.Value,
            ProposedService = dto.Custom6?.Value,
            SpendCategoryCode = dto.SpendCategoryCode,
            TaxableDeductibleAmount = dto.Custom31?.Value,
            TaxDeductibleVATAmount = dto.Custom33?.Value,
            TaxInvoiceNumber = dto.Custom36?.Value,
            TaxReclaimAmount = dto.Custom39?.Value?.ToDecimal(),
            TaxReclaimCountryRegion = dto.Custom35?.Value,
            TransactionAmount = dto.TransactionAmount,
            TransactionDate = dto.TransactionDate,
            VATCode = dto.Custom38?.Value,
            DateTimeAddedUtc = utcNow,
            DateTimeUpdatedUtc = utcNow

        };
    }
    public static Itemization UpdateItemizationFromDto(this Itemization entity, ItemizationDto dto, DateTime utcNow)
    {
        entity.CompanyCode = dto.OrgUnit1?.Code;
        entity.CostCenterCode = dto.OrgUnit2?.Code;
        entity.Department = dto.OrgUnit3?.Code;
        entity.LastModifiedDateTimeUtc = dto.LastModified ?? DateTime.MinValue;

        entity.Account = dto.Custom11?.Code;
        entity.AllocationType = dto.AllocationType;
        entity.ApprovedAmount = dto.ApprovedAmount;
        entity.BillRuleRegOrRateNumber = dto.Custom2?.Value;
        entity.CheckInDate = dto.Custom12?.Value?.ToDateOnly();
        entity.CheckOutDate = dto.Custom13?.Value?.ToDateOnly();
        entity.ContractOrRFPTitleAndIdentifyingNumber = dto.Custom7?.Value;
        entity.ContractTitleIdentifyingNumberAndDate = dto.Custom8?.Value;
        entity.Description = dto.Description;
        entity.EntrySubtractFromGross = dto.Custom37?.Value?.ToDecimal();
        entity.ExpenseTypeCode = dto.ExpenseTypeCode;
        entity.ExpenseTypeName = dto.ExpenseTypeName;
        entity.HasComments = dto.HasComments;
        entity.HasExceptions = dto.HasExceptions;
        entity.IsBillable = dto.IsBillable;
        entity.IsImageRequired = dto.IsImageRequired;
        entity.IsPersonal = dto.IsPersonal;
        entity.ItemsGiven = dto.Custom5?.Value;
        entity.LengthInHours = dto.Custom1?.Value?.ToInt();
        entity.LocationCountryISO2 = (dto.LocationCountry?.Trim()?.Length ?? 0) == 2 ? dto.LocationCountry : null;
        entity.LocationName = dto.LocationName;
        entity.LocationSubdivision = dto.LocationSubdivision;
        entity.NatureOfContact = dto.Custom3?.Value;
        entity.NetTaxAmount = dto.Custom40?.Value?.ToDecimal();
        entity.NonTaxableNonDeductibleAmount = dto.Custom32?.Value;
        entity.NonTaxDeductibleVATAmount = dto.Custom34?.Value;
        entity.PersonalChargeConfirmation = dto.Custom14?.Value?.ToBoolean();
        entity.PersonDesignatedInContractOrRFP = dto.Custom4?.Code?.ToBoolean();
        entity.PostedAmount = dto.PostedAmount;
        entity.ProjectCode = dto.OrgUnit6?.Code;
        entity.ProjectDescription = dto.OrgUnit6?.Value;
        entity.ProposedService = dto.Custom6?.Value;
        entity.SpendCategoryCode = dto.SpendCategoryCode;
        entity.TaxableDeductibleAmount = dto.Custom31?.Value;
        entity.TaxDeductibleVATAmount = dto.Custom33?.Value;
        entity.TaxInvoiceNumber = dto.Custom36?.Value;
        entity.TaxReclaimAmount = dto.Custom39?.Value?.ToDecimal();
        entity.TaxReclaimCountryRegion = dto.Custom35?.Value;
        entity.TransactionAmount = dto.TransactionAmount;
        entity.TransactionDate = dto.TransactionDate;
        entity.VATCode = dto.Custom38?.Value;
        entity.DateTimeUpdatedUtc = utcNow;

        return entity;
    }
}
