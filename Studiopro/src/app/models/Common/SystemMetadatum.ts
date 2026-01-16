interface SystemMetadatum {
    MetadataId: number;

    Category: string;

    Code: string;

    DisplayName: string;

    NumericValue: number | null;

    SortOrder: number | null;

    IsActive: boolean | null;

    IsSystem: boolean;

    Description: string | null;

    CreatedDate: Date | string | null;

    ModifiedDate: Date | string | null;
}