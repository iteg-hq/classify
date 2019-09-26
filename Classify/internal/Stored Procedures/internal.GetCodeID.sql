CREATE PROCEDURE internal.GetCodeID
    @Code NVARCHAR(200)
  , @ID INT OUTPUT
AS
SET NOCOUNT, XACT_ABORT ON;
SET @ID = NULL; -- In case a non-null value gets passed

INSERT INTO internal.Code ( CodeValue ) 
SELECT @Code
EXCEPT
SELECT CodeValue
FROM internal.Code
;

SELECT @ID = CodeID FROM internal.Code WHERE CodeValue = @Code;
