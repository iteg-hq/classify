CREATE PROCEDURE dbo.GetClassifierTypes
AS
SET NOCOUNT, XACT_ABORT ON;

SELECT
    ClassifierTypeCode
  , ClassifierTypeName
  , ClassifierTypeDescription
FROM dbo.ClassifierType
;
