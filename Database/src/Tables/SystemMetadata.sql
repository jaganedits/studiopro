-- Create a single metadata/lookup table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemMetadata' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.SystemMetadata (
        MetadataId INT IDENTITY(1,1) PRIMARY KEY,
        Category VARCHAR(50) NOT NULL,          -- e.g., 'STATUS', 'DAY', 'MONTH', 'COLOR'
        Code VARCHAR(50) NOT NULL,              -- e.g., 'ACTIVE', 'MONDAY', 'JANUARY'
        DisplayName VARCHAR(100) NOT NULL,      -- e.g., 'Active', 'Monday', 'January'
        NumericValue INT NULL,                  -- For ordering or numeric representation
        SortOrder INT DEFAULT 0,                -- For custom ordering
        IsActive BIT DEFAULT 1,                 -- To soft-deactivate if needed
        IsSystem BIT DEFAULT 1 NOT NULL,        -- Mark as system/unchangeable value
        Description NVARCHAR(500) NULL,
        CreatedDate DATETIME DEFAULT GETDATE(),
        ModifiedDate DATETIME DEFAULT GETDATE(),
        
        -- Composite unique constraint
        CONSTRAINT UQ_Metadata_Category_Code UNIQUE (Category, Code)
    );
    
    PRINT 'Table dbo.SystemMetadata created successfully.';
END
ELSE
BEGIN
    PRINT 'Table dbo.SystemMetadata already exists.';
END
GO

-- Create indexes for performance if they don't exist
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SystemMetadata_Category' AND object_id = OBJECT_ID('dbo.SystemMetadata'))
BEGIN
    CREATE INDEX IX_SystemMetadata_Category ON dbo.SystemMetadata(Category, IsActive);
    PRINT 'Index IX_SystemMetadata_Category created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IX_SystemMetadata_Category already exists.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SystemMetadata_Category_Code' AND object_id = OBJECT_ID('dbo.SystemMetadata'))
BEGIN
    CREATE INDEX IX_SystemMetadata_Category_Code ON dbo.SystemMetadata(Category, Code);
    PRINT 'Index IX_SystemMetadata_Category_Code created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IX_SystemMetadata_Category_Code already exists.';
END
GO