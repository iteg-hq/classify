CREATE PROCEDURE dbo.GetClassifierRelationships
    @ClassifierTypeCode NVARCHAR(500)
  , @ClassifierCode NVARCHAR(500)
AS
SET NOCOUNT, XACT_ABORT ON;

SELECT
    ClassifierTypeCode
  , ClassifierCode
  , ClassifierRelationshipTypeCode
  , RelatedClassifierTypeCode
  , RelatedClassifierCode
  , [Description]
  , [Weight]
FROM dbo.ClassifierRelationship
WHERE ClassifierTypeCode = @ClassifierTypeCode
  AND ClassifierCode = @ClassifierCode
;
