CREATE OR ALTER PROCEDURE [dbo].[GetPrimaryKey]
--DECLARE
@documentid INT = null,
@documentno VARCHAR(MAX) = NULL  OUT
as
BEGIN
    SET NOCOUNT ON;

    DECLARE @ERRMSG VARCHAR(MAX);
    DECLARE @FUNCTIONNAME VARCHAR(MAX);
    DECLARE @SEVERITY INT = 16;
    DECLARE @STATE INT = 1;
    DECLARE @docum AS TABLE
    (
        documentno VARCHAR(30)
    );

    IF ((SELECT COUNT(1) FROM Numberingschema WHERE documentid = @documentid AND (ISNULL(sequencename, '') <> '')) = 0)
    BEGIN
        SELECT TOP 1 @FUNCTIONNAME = functionname FROM [Function] WHERE functionid = @documentid;
        SET @ERRMSG = 'Numbering Schema not configured for ' + @FUNCTIONNAME + '. Kindly configure Numbering Schema first.';
        RAISERROR (@ERRMSG, @SEVERITY, @STATE);
        RETURN;
    END;

    DECLARE @sequencename VARCHAR(200);
    SELECT @sequencename = (SELECT sequencename FROM Numberingschema WHERE documentid = @documentid);

    DECLARE @sequencQuery NVARCHAR(MAX) = '
        DECLARE @functionname VARCHAR(100);
        DECLARE @rowCount INTEGER;
        DECLARE @i INTEGER = 1;
        DECLARE @MaxCount INTEGER;
        DECLARE @prefix VARCHAR(30);
        DECLARE @suffix VARCHAR(20);
        DECLARE @symbol VARCHAR(20);
        DECLARE @isdateformate BIT;
        DECLARE @Currentdate VARCHAR(50) = ''-'' + 
            (SELECT RIGHT(YEAR(GETDATE()), 2) + RIGHT(CONCAT(''0'', MONTH(GETDATE())), 2) + RIGHT(CONCAT(''0'', DAY(GETDATE())), 2));
			SELECT @rowCount = NEXT VALUE FOR ' + @sequencename + ';
  
        SELECT @functionname = (SELECT UPPER(SUBSTRING(functionname,1,3)) AS FunctionName FROM [Function] WHERE functionid = ' + CAST(@documentid AS VARCHAR) + ');            
        SELECT @prefix = (SELECT prefix FROM Numberingschema WHERE documentid = ' + CAST(@documentid AS VARCHAR) + ');            
        SELECT @symbol = (
            SELECT m.DisplayName FROM Numberingschema N
            INNER JOIN SystemMetadata m ON m.MetadataId = N.symbolid
            WHERE documentid = ' + CAST(@documentid AS VARCHAR) + ');           
        SELECT @suffix = (SELECT suffix FROM Numberingschema WHERE documentid = ' + CAST(@documentid AS VARCHAR) + ');
        SELECT @isdateformate = (SELECT isdateformat FROM Numberingschema WHERE documentid = ' + CAST(@documentid AS VARCHAR) + ');
		 DECLARE @SEQ VARCHAR(MAX);
        IF (@isdateformate = 0)
        BEGIN
            SET @MaxCount = LEN(@suffix);
            SET @SEQ = CASE 
			WHEN LEN(@suffix) > 0 THEN RIGHT(REPLICATE(''0'', LEN(@suffix)) + CAST(@rowCount AS VARCHAR), LEN(@suffix))
			ELSE CAST(@rowCount AS VARCHAR) 
			END;
        END
        ELSE
        BEGIN
            SET @SEQ = CAST(@rowCount AS VARCHAR);
        END        
        IF (@isdateformate = 1)
    BEGIN
        SET @documentno = CONCAT(@prefix, @Currentdate, @SEQ);
    END
    ELSE
    BEGIN
        SET @documentno = CONCAT(@prefix, @symbol, @SEQ);
    END;
    ';

    EXEC sp_executesql @sequencQuery, N'@documentno VARCHAR(MAX) OUT', @documentno = @documentno OUT;

    SELECT @documentno;

END;
