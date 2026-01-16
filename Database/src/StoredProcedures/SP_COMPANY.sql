CREATE OR ALTER PROCEDURE SP_COMPANY
	@Type VARCHAR(255)=NULL,
	@companyId INT = null
AS
BEGIN 
if (@Type = 'PageInit')
BEGIN
SELECT * FROM SystemMetadata m WHERE m.Category='STATUS';
	IF(@companyId>1)
	BEGIN
		SELECT * FROM Company 
		SELECT * FROM CompanyAddress
	END
END
END