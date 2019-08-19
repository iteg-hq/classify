CREATE PROCEDURE dbo.DeleteClassifierType
    @TypeCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;

EXEC internal.DeleteClassifier @TypeCode, '';
