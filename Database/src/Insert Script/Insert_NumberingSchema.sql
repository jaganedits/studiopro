
--CREATE SEQUENCE UserSeq
--    START WITH 1
--    INCREMENT BY 1;
--CREATE SEQUENCE RoleSeq
--    START WITH 1
--    INCREMENT BY 1;
--CREATE SEQUENCE DeptSeq
--	START WITH 1
--	INCREMENT BY 1;
--CREATE SEQUENCE PlantSeq
--	START WITH 1
--	INCREMENT BY 1;
--CREATE SEQUENCE LocSeq
--	START WITH 1
--	INCREMENT BY 1;





SET IDENTITY_INSERT NumberingSchema ON;
TRUNCATE TABLE NumberingSchema;
INSERT INTO NumberingSchema 
( numberschemaid, DocumentID, Prefix, IsDateFormat, SymbolID, Suffix, Status, 
  CreatedBy,CreatedOn,Modifiedby,Modifiedon, sequencename)
VALUES 
	(1,  9, 'ROL', 0, 4, '', 1, 1, GETDATE(), 1, GETDATE(), 'RoleSeq')
	--(2,  19, 'USR', 0, 18, '', 1, 1, GETDATE(), 1, GETDATE(), 'UserSeq'),
	--(3,  10, 'DEPT', 0, 18, '', 1, 1, GETDATE(), 1, GETDATE(), 'DeptSeq'),
	--(4,  22, 'PLANT', 0, 18, '', 1, 1, GETDATE(), 1, GETDATE(), 'PlantSeq'),
	--(5,  23, 'LOC', 0, 18, '', 1, 1, GETDATE(), 1, GETDATE(), 'LocSeq')


;

SET IDENTITY_INSERT NumberingSchema OFF;




