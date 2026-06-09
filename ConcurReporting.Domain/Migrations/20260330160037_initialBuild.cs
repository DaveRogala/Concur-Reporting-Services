using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcurReporting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class initialBuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cnc");

            migrationBuilder.CreateTable(
                name: "Journeys",
                schema: "cnc",
                columns: table => new
                {
                    JourneyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    OdometerStart = table.Column<int>(type: "int", nullable: true),
                    OdometerEnd = table.Column<int>(type: "int", nullable: true),
                    BusinessDistance = table.Column<int>(type: "int", nullable: true),
                    PersonalDistance = table.Column<int>(type: "int", nullable: true),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: true),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.JourneyId);
                });

            migrationBuilder.CreateTable(
                name: "QueryHistories",
                schema: "cnc",
                columns: table => new
                {
                    QueryHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryHistories", x => x.QueryHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "cnc",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountDueCompanyCard = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AmountDueEmployee = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ApprovalStatusCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovalStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApproverLoginID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApproverName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CountryISO2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CountrySubdivision = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    EmployeeGroup = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    EverSentBack = table.Column<bool>(type: "bit", nullable: false),
                    HasException = table.Column<bool>(type: "bit", nullable: false),
                    LastComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerLoginID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatusCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PaymentStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PolicyID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProcessingPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiptsReceived = table.Column<bool>(type: "bit", nullable: false),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalApprovedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalClaimedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UserDefinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendorID = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    LastModifiedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ConcurID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                schema: "cnc",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyCardTransactionID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExpenseID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HasAppliedCashAdvance = table.Column<bool>(type: "bit", nullable: false),
                    HasAttendees = table.Column<bool>(type: "bit", nullable: false),
                    HasImage = table.Column<bool>(type: "bit", nullable: false),
                    HasVAT = table.Column<bool>(type: "bit", nullable: false),
                    IsPaidByExpensePay = table.Column<bool>(type: "bit", nullable: false),
                    IsPersonalCardCharge = table.Column<bool>(type: "bit", nullable: false),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceiptReceived = table.Column<bool>(type: "bit", nullable: false),
                    TaxReceiptType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    TransactionCurrencyCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TripID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorDescription = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    VendorListItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JourneyId = table.Column<int>(type: "int", nullable: true),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ConcurID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    AllocationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    BillRuleRegOrRateNumber = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CheckOutDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContractOrRFPTitleAndIdentifyingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractTitleIdentifyingNumberAndDate = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    EntrySubtractFromGross = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    ExpenseTypeCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    ExpenseTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasComments = table.Column<bool>(type: "bit", nullable: false),
                    HasExceptions = table.Column<bool>(type: "bit", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false),
                    IsImageRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsPersonal = table.Column<bool>(type: "bit", nullable: false),
                    ItemsGiven = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    LengthInHours = table.Column<int>(type: "int", nullable: true),
                    LocationCountryISO2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocationSubdivision = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NatureOfContact = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    NetTaxAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    NonTaxableNonDeductibleAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    NonTaxDeductibleVATAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    PersonalChargeConfirmation = table.Column<bool>(type: "bit", nullable: true),
                    PersonDesignatedInContractOrRFP = table.Column<bool>(type: "bit", maxLength: 48, nullable: true),
                    PostedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProposedService = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    SpendCategoryCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    SpendCategoryName = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxableDeductibleAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxDeductibleVATAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxInvoiceNumber = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxReclaimAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    TaxReclaimCountryRegion = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VATCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    LastModifiedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entries_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalSchema: "cnc",
                        principalTable: "Journeys",
                        principalColumn: "JourneyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "cnc",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itemizations",
                schema: "cnc",
                columns: table => new
                {
                    ItemizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ConcurID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    AllocationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    BillRuleRegOrRateNumber = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CheckOutDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContractOrRFPTitleAndIdentifyingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractTitleIdentifyingNumberAndDate = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    EntrySubtractFromGross = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    ExpenseTypeCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    ExpenseTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasComments = table.Column<bool>(type: "bit", nullable: false),
                    HasExceptions = table.Column<bool>(type: "bit", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false),
                    IsImageRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsPersonal = table.Column<bool>(type: "bit", nullable: false),
                    ItemsGiven = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    LengthInHours = table.Column<int>(type: "int", nullable: true),
                    LocationCountryISO2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocationSubdivision = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NatureOfContact = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    NetTaxAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    NonTaxableNonDeductibleAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    NonTaxDeductibleVATAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    PersonalChargeConfirmation = table.Column<bool>(type: "bit", nullable: true),
                    PersonDesignatedInContractOrRFP = table.Column<bool>(type: "bit", maxLength: 48, nullable: true),
                    PostedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProposedService = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    SpendCategoryCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    SpendCategoryName = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxableDeductibleAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxDeductibleVATAmount = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxInvoiceNumber = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TaxReclaimAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    TaxReclaimCountryRegion = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VATCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    LastModifiedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itemizations", x => x.ItemizationId);
                    table.ForeignKey(
                        name: "FK_Itemizations_Entries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "cnc",
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allocations",
                schema: "cnc",
                columns: table => new
                {
                    AllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    IsPercentEdited = table.Column<bool>(type: "bit", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    Account1 = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    EntryId = table.Column<int>(type: "int", nullable: true),
                    ItemizationId = table.Column<int>(type: "int", nullable: true),
                    DateTimeAddedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ConcurID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.AllocationId);
                    table.ForeignKey(
                        name: "FK_Allocations_Entries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "cnc",
                        principalTable: "Entries",
                        principalColumn: "EntryId");
                    table.ForeignKey(
                        name: "FK_Allocations_Itemizations_ItemizationId",
                        column: x => x.ItemizationId,
                        principalSchema: "cnc",
                        principalTable: "Itemizations",
                        principalColumn: "ItemizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_CompanyCode_CostCenterCode",
                schema: "cnc",
                table: "Allocations",
                columns: new[] { "CompanyCode", "CostCenterCode" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_CompanyCode_CostCenterCode_Account",
                schema: "cnc",
                table: "Allocations",
                columns: new[] { "CompanyCode", "CostCenterCode", "Account" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_ConcurID",
                schema: "cnc",
                table: "Allocations",
                column: "ConcurID",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_EntryId",
                schema: "cnc",
                table: "Allocations",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_ItemizationId",
                schema: "cnc",
                table: "Allocations",
                column: "ItemizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CompanyCode_CostCenterCode",
                schema: "cnc",
                table: "Entries",
                columns: new[] { "CompanyCode", "CostCenterCode" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ConcurID",
                schema: "cnc",
                table: "Entries",
                column: "ConcurID",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_JourneyId",
                schema: "cnc",
                table: "Entries",
                column: "JourneyId",
                unique: true,
                filter: "[JourneyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ReportId",
                schema: "cnc",
                table: "Entries",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Itemizations_CompanyCode_CostCenterCode",
                schema: "cnc",
                table: "Itemizations",
                columns: new[] { "CompanyCode", "CostCenterCode" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Itemizations_ConcurID",
                schema: "cnc",
                table: "Itemizations",
                column: "ConcurID",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Itemizations_EntryId",
                schema: "cnc",
                table: "Itemizations",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApprovalStatusCode",
                schema: "cnc",
                table: "Reports",
                column: "ApprovalStatusCode")
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApproverLoginID",
                schema: "cnc",
                table: "Reports",
                column: "ApproverLoginID")
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CompanyCode_CostCenterCode",
                schema: "cnc",
                table: "Reports",
                columns: new[] { "CompanyCode", "CostCenterCode" })
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ConcurID",
                schema: "cnc",
                table: "Reports",
                column: "ConcurID",
                unique: true)
                .Annotation("SqlServer:FillFactor", 90);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PaymentStatusCode",
                schema: "cnc",
                table: "Reports",
                column: "PaymentStatusCode")
                .Annotation("SqlServer:FillFactor", 90);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocations",
                schema: "cnc");

            migrationBuilder.DropTable(
                name: "QueryHistories",
                schema: "cnc");

            migrationBuilder.DropTable(
                name: "Itemizations",
                schema: "cnc");

            migrationBuilder.DropTable(
                name: "Entries",
                schema: "cnc");

            migrationBuilder.DropTable(
                name: "Journeys",
                schema: "cnc");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "cnc");
        }
    }
}
