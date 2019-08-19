CREATE PROCEDURE internal.DeleteClassifier
    @TypeCode NVARCHAR(100)
  , @Code NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @TypeCodeID INT;
DECLARE @CodeID INT;

EXEC internal.GetCodeID @TypeCode, @TypeCodeID OUTPUT;
EXEC internal.GetCodeID @Code, @CodeID OUTPUT;

INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , IsDeleted
  )
VALUES (
    @TypeCodeID
  , @CodeID
  , 1
  )
;
