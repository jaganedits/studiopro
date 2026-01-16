interface Company {
    CompanyId: number;

    CompanyName: string;

    Tagline: string | null;

    Phone: number;

    AlternatePhone: number | null;

    Email: string | null;

    Website: string | null;

    Facebook: string | null;

    Instagram: string | null;

    Youtube: string | null;

    Whatsapp: string | null;

    GstNumber: string | null;

    PanNumber: string | null;

    CinNumber: string | null;

    BankName: string | null;

    AccountNumber: string | null;

    AccountHolderName: string | null;

    IfscCode: string | null;

    BranchName: string | null;

    UpiId: string | null;

    InvoicePrefix: string | null;

    InvoiceStartNumber: number | null;

    TermsConditions: string | null;

    FooterNote: string | null;

    LogonName: string | null;

    LogoPath: string | null;

    SignaturenName: string | null;

    SignaturePath: string | null;

    Status: number;

    StatusName: string;

    CreatedBy: number;

    CreatedByName: string;

    CreatedDate: Date | string;

    ModifiedBy: number | null;

    ModifiedByName: string;

    ModifiedDate: Date | string | null;
}
