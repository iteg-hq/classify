CREATE PROCEDURE dbo.SaveClassifierType
    @TypeCode NVARCHAR(100)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

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
