CREATE PROCEDURE dbo.GetClassifier
    @ClassifierTypeCode NVARCHAR(500)
  , @ClassifierCode NVARCHAR(500)
AS
SET NOCOUNT, XACT_ABORT ON;

SELECT
    ClassifierTypeCode
  , ClassifierCode
  , ClassifierName
  , ClassifierDescription
  , UpdatedBy
  , UpdatedOn
FROM dbo.Classifier
WHERE ClassifierTypeCode = @ClassifierTypeCode
  AND ClassifierCode = @ClassifierCode
;
