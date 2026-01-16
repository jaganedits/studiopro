CREATE OR ALTER PROCEDURE SP_ROLE
@Type VARCHAR(255)=NULL,
@roleId INT,
@companyId INT

AS
BEGIN
IF (@Type = 'ListInit')
BEGIN
SELECT 
    RoleId,
    CompanyId,
    RoleName,
    RoleCode,
    Status,
    sm.DisplayName as StatusName ,
    CreatedBy,
    r.CreatedDate,
    ModifiedBy,
    r.ModifiedDate
FROM Roles r
INNER JOIN SystemMetadata sm with (nolock) on sm.MetadataId = r.RoleId
where r.CompanyId = @companyId
END

 IF (@Type = 'PageInit')
    BEGIN
	    SELECT * FROM SystemMetadata m WHERE m.Category='STATUS';
		IF(@roleId>0)
		BEGIN
          SELECT * FROM roles r where r.companyid = @companyId  AND r.roleid = @roleId 
        END
    END
END;

