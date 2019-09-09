CREATE PROCEDURE dbo.SaveClassifier
    @ClassifierTypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF @ClassifierCode = '' THROW 51000, 'Code can not be empty', 1;
IF @ClassifierCode IS NULL THROW 51000, 'Code can not be NULL', 1;

-- If the Classifier Type classifier does not exist, create it;
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ClassifierType
    WHERE ClassifierTypeCode = @ClassifierTypeCode
  )
  EXEC dbo.SaveClassifierType @ClassifierTypeCode;


EXEC internal.SaveClassifier @ClassifierTypeCode, @ClassifierCode, @Name, @Description;


/*
DECLARE @ClassifierTypeCodeID INT;

EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;

INSERT INTO internal.ClassifierType (
    ClassifierTypeCodeID
  , [Name]
  , [Description]
  )
VALUES (
    @ClassifierTypeCodeID
  , COALESCE(@Name, '')
  , COALESCE(@Description, '')
  )
;
*/