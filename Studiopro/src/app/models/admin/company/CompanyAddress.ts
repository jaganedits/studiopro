interface CompanyAddress {
    CompanyAddressId: number;

    CompanyId: number;

    Label: string;

    Address: string;

    Area: string | null;

    City: string;

    State: string;

    Pincode: string;

    Landmark: string | null;

    IsPrimary: boolean;

    Status: number;

    StatusName: string;

    CreatedBy: number;

    CreatedByName: string;

    CreatedDate: Date | string;

    ModifiedBy: number | null;

    ModifiedByName: string;

    ModifiedDate: Date | string | null;
}
