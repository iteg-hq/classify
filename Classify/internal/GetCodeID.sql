CREATE PROCEDURE internal.GetCodeID
    @Code NVARCHAR(200)
  , @ID INT OUTPUT
AS
SET NOCOUNT, XACT_ABORT ON;
SET @ID = NULL; -- In case a non-null value gets passed
SELECT @ID = CodeID
FROM internal.Code
WHERE CodeValue = @Code
;

IF @ID IS NULL
BEGIN
  INSERT INTO internal.Code ( CodeValue ) VALUES ( @Code );
  SET @ID = SCOPE_IDENTITY();
END

RETURN @ID;
