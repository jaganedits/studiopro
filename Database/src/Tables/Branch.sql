IF OBJECT_ID('dbo.Branch', 'U') IS NOT NULL DROP TABLE dbo.Branch;
GO

CREATE TABLE dbo.Branch (
    BranchId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL,
    BranchName NVARCHAR(50) NOT NULL,
    BranchCode VARCHAR(50) NOT NULL,
    Address Varchar(255) null,
    Manager INT null,
    Phone INT null,
    Status INT NOT NULL,
    CreatedBy INT Not Null,
    CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
    ModifiedBy INT null,
    ModifiedDate DATETIME DEFAULT GETDATE() NULL
    --CONSTRAINT FK_Role_Company FOREIGN KEY (company_id) REFERENCES dbo.Company(id)
);
GO

