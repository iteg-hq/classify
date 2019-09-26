CREATE PROCEDURE internal.SaveClassifier
    @ClassifierTypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;
DECLARE @ClassifierID INT;

EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode, @ClassifierCodeID OUTPUT;

-- TODO: Pick up attributes for change detection?
SELECT TOP 1 @ClassifierID = ClassifierID
FROM internal.Classifier
WHERE ClassifierTypeCodeID = @ClassifierTypeCodeID
  AND ClassifierCodeID = @ClassifierCodeID
;

IF @ClassifierID IS NULL
BEGIN
  SET @ClassifierID = NEXT VALUE FOR internal.Identifier
END

INSERT INTO internal.Classifier (
    ClassifierID
  , ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , [Description]
)
VALUES (
    @ClassifierID
  , @ClassifierTypeCodeID
  , @ClassifierCodeID
  , COALESCE(@Name, @ClassifierCode, '')
  , COALESCE(@Description, '')
)
