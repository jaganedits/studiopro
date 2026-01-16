
CREATE OR ALTER procedure [dbo].[SP_USER]    
--declare
  @type VARCHAR(250) = null,    
  @userId INT = null,    
  @branchId INT = null ,
  @employeeId INT = null,
  @companyId INT=null
AS     
BEGIN   

IF(@type ='PageInit')
BEGIN
     --status
     SELECT * FROM SystemMetadata m WHERE m.Category='STATUS';  

     --role
     SELECT * FROM Roles r where r.Status = 1

END

END