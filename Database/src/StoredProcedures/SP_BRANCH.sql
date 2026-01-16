CREATE OR ALTER PROCEDURE SP_BRANCH
@Type VARCHAR(255)=NULL,
@branchId INT = null,
@companyId INT = null

AS
BEGIN
IF (@Type = 'ListInit')
BEGIN
SELECT 
    b.BranchId,
    b.CompanyId,
    BranchName,
    BranchCode,
    Address,
    ma.Username as ManagerName,
    b.Phone,
    b.Status,
    sm.DisplayName as StatusName,
    b.CreatedBy,
    c.Username as CreatedByName,
    b.CreatedDate,
    b.ModifiedBy,
    m.Username as ModifiedByName,
    b.ModifiedDate
FROM Branch b
LEFT JOIN Users ma with (nolock) on ma.UserId = b.Manager
INNER JOIN Users c with (nolock) on c.UserId = b.CreatedBy
LEFT JOIN Users m with (nolock) on m.UserId = b.ModifiedBy
INNER JOIN SystemMetadata sm with (nolock) on sm.MetadataId = b.Status 
where b.CompanyId = @companyId 
END

 IF (@Type = 'PageInit')
    BEGIN
        SELECT * FROM Users u  WHERE u.Status = 1
	    SELECT * FROM SystemMetadata m WHERE m.Category='STATUS';
		IF(@branchId>0)
		BEGIN
          SELECT * FROM Branch b where b.companyid = @companyId  AND b.BranchId = @branchId 
        END
    END
END;

