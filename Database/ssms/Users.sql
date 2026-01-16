IF OBJECT_ID('dbo.[Users]', 'U') IS NOT NULL DROP TABLE dbo.[User];
GO

CREATE TABLE dbo.[Users] (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL,
    BranchId INT NOT NULL,
    RoleId INT NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    AvatarName NVARCHAR(250) NULL,
    AvatarUrl NVARCHAR(MAX) NULL,
    LastLogin DATETIME2 NULL,
    Status INT NOT NULL,
    CreatedBy INT Not Null,
    CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
    ModifiedBY INT null,
    ModifiedDate DATETIME DEFAULT GETDATE() NULL
);
GO