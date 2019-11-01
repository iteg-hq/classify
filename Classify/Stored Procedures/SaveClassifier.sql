CREATE PROCEDURE dbo.SaveClassifier
    @TypeCode NVARCHAR(100)
  , @Code NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF @Code = '' THROW 51000, 'Code can not be empty', 1;
IF @Code IS NULL THROW 51000, 'Code can not be NULL', 1;

-- If the Classifier Type classifier does not exist, create it;
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ClassifierType
    WHERE ClassifierTypeCode = @TypeCode
  )
  EXEC dbo.SaveClassifierType @TypeCode;


EXEC internal.SaveClassifier @TypeCode, @Code, @Name, @Description;


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