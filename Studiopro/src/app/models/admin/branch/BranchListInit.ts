interface BranchListInit {
     BranchId: number;

    CompanyId: number;

    BranchName: string;

    BranchCode: string;

    Address: string | null;

    ManagerName: string | null;

    Phone: number | null;

    Status: number;
    
    StatusName: string;

    CreatedBy: number;
    
    CreatedByName: string;

    CreatedDate: Date | string;

    ModifiedBy: number | null;
    
    ModifiedByName: string;

    ModifiedDate: Date | string | null;
}