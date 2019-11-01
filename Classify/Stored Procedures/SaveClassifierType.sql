CREATE PROCEDURE dbo.SaveClassifierType
    @Code NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF @Code = '' THROW 51000, 'Type code can not be empty', 1;
IF @Code IS NULL THROW 51000, 'Type code can not be NULL', 1;

IF @Name IS NULL SET @Name = @Code;

EXEC internal.SaveClassifier @Code, '', @Name, @Description;


/*
DECLARE @CodeID INT;

EXEC internal.GetCodeID @Code, @CodeID OUTPUT;

INSERT INTO internal.ClassifierType (
    ClassifierTypeCodeID
  , [Name]
  , [Description]
  )
VALUES (
    @CodeID
  , COALESCE(@Name, '')
  , COALESCE(@Description, '')
  )
;
*/