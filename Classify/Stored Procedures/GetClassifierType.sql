CREATE PROCEDURE dbo.GetClassifierType
    @ClassifierTypeCode NVARCHAR(500)
AS
SET NOCOUNT, XACT_ABORT ON;

SELECT
    ClassifierTypeCode
  , ClassifierTypeName
  , ClassifierTypeDescription
  , UpdatedBy
  , UpdatedOn
FROM dbo.ClassifierType
WHERE ClassifierTypeCode = @ClassifierTypeCode
;
