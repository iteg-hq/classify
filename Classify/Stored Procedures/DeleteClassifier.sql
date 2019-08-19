CREATE PROCEDURE dbo.DeleteClassifier
    @TypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;
IF @ClassifierCode = '' THROW 51000, 'Classifier Code cannot be blank', 1;
EXEC internal.DeleteClassifier @TypeCode, @ClassifierCode;
