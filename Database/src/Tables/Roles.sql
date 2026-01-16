IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

CREATE TABLE dbo.Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL,
    RoleName NVARCHAR(50) NOT NULL,
    RoleCode VARCHAR(50) NOT NULL,
    Status INT NOT NULL,
    CreatedBy INT Not Null,
    CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
    ModifiedBy INT null,
    ModifiedDate DATETIME DEFAULT GETDATE() NULL
    --CONSTRAINT FK_Role_Company FOREIGN KEY (company_id) REFERENCES dbo.Company(id)
);
GO

