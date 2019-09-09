CREATE PROCEDURE internal.SaveClassifier
    @ClassifierTypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;

EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode, @ClassifierCodeID OUTPUT;

INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , [Description]
)
VALUES (
    @ClassifierTypeCodeID
  , @ClassifierCodeID
  , COALESCE(@Name, @ClassifierCode, '')
  , COALESCE(@Description, '')
)
