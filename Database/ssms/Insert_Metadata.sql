TRUNCATE TABLE SystemMetadata

SET IDENTITY_INSERT dbo.SystemMetadata ON;
GO

-- Insert all metadata with your custom IDs
INSERT INTO dbo.SystemMetadata
(MetadataId, Category, Code, DisplayName, NumericValue, SortOrder, IsActive, IsSystem, Description, CreatedDate, ModifiedDate)
VALUES
-- =============================================
-- STATUS VALUES (ID 1-99)
-- =============================================
(1, 'STATUS', 'ACTIVE', 'Active', null, 1, 1, 1, null, GETDATE(), NULL),
(2, 'STATUS', 'INACTIVE', 'Inactive', null, 2, 1, 1, null, GETDATE(), NULL),
(3, 'CODE SYMBOL', 'HASHTAG', '#', null, 1, 1, 1, null, GETDATE(), NULL),
(4, 'CODE SYMBOL', 'DASH', '-', null, 1, 1, 1, null, GETDATE(), NULL);


go

SET IDENTITY_INSERT dbo.SystemMetadata OFF;

go
