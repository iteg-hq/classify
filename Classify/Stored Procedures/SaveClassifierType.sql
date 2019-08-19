CREATE PROCEDURE dbo.SaveClassifierType
    @TypeCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF @TypeCode = '' THROW 51000, 'Type code can not be empty', 1;
IF @TypeCode IS NULL THROW 51000, 'Type code can not be NULL', 1;

IF @Name IS NULL SET @Name = @TypeCode;

EXEC internal.SaveClassifier @TypeCode, '', @Name, @Description;


/*
DECLARE @TypeCodeID INT;

EXEC internal.GetCodeID @TypeCode, @TypeCodeID OUTPUT;

INSERT INTO internal.ClassifierType (
    ClassifierTypeCodeID
  , [Name]
  , [Description]
  )
VALUES (
    @TypeCodeID
  , COALESCE(@Name, '')
  , COALESCE(@Description, '')
  )
;
*/