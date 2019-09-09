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
  , 0 AS IsInbound
  , UpdatedBy
  , UpdatedOn
FROM dbo.ClassifierRelationship
WHERE ClassifierTypeCode = @ClassifierTypeCode
  AND ClassifierCode = @ClassifierCode
UNION ALL
SELECT
    ClassifierTypeCode
  , ClassifierCode
  , ClassifierRelationshipTypeCode
  , RelatedClassifierTypeCode
  , RelatedClassifierCode
  , [Description]
  , [Weight]
  , 1 AS IsInbound
  , UpdatedBy
  , UpdatedOn
FROM dbo.ClassifierRelationship
WHERE RelatedClassifierTypeCode = @ClassifierTypeCode
  AND RelatedClassifierCode = @ClassifierCode
;
