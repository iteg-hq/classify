CREATE PROCEDURE internal.SaveClassifier
    @TypeCode NVARCHAR(100)
  , @Code NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;

EXEC internal.GetCodeID @TypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @Code, @ClassifierCodeID OUTPUT;

INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , [Description]
)
VALUES (
    @ClassifierTypeCodeID
  , @ClassifierCodeID
  , COALESCE(@Name, @Code, '')
  , COALESCE(@Description, '')
)
