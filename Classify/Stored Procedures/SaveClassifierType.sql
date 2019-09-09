CREATE PROCEDURE dbo.SaveClassifierType
    @ClassifierTypeCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF @ClassifierTypeCode = '' THROW 51000, 'Type code can not be empty', 1;
IF @ClassifierTypeCode IS NULL THROW 51000, 'Type code can not be NULL', 1;

IF @Name IS NULL SET @Name = @ClassifierTypeCode;

EXEC internal.SaveClassifier @ClassifierTypeCode, '', @Name, @Description;


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