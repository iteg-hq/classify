CREATE VIEW dbo.ClassifierRelationship
AS
SELECT
    c1.CodeValue AS ClassifierTypeCode
  , c2.CodeValue AS ClassifierCode
  , c3.CodeValue AS ClassifierRelationshipTypeCode
  , c4.CodeValue AS RelatedClassifierTypeCode
  , c5.CodeValue AS RelatedClassifierCode
  , r.[Description]
  , r.[Weight]
  , r.rv
FROM internal.ClassifierRelationship AS r
INNER JOIN internal.Code AS c1
  ON c1.CodeID = r.ClassifierTypeCodeID
INNER JOIN internal.Code AS c2
  ON c2.CodeID = r.ClassifierCodeID
INNER JOIN internal.Code AS c3
  ON c3.CodeID = r.RelationshipTypeCodeID
INNER JOIN internal.Code AS c4
  ON c4.CodeID = r.RelatedClassifierTypeCodeID
INNER JOIN internal.Code AS c5
  ON c5.CodeID = r.RelatedClassifierCodeID
WHERE r.rv = (
    SELECT MAX(rv)
    FROM internal.ClassifierRelationship AS it
    WHERE it.ClassifierCodeID = r.ClassifierCodeID
      AND it.ClassifierTypeCodeID = r.ClassifierTypeCodeID
      AND it.RelationshipTypeCodeID = r.RelationshipTypeCodeID
      AND it.RelatedClassifierCodeID = r.RelatedClassifierCodeID
      AND it.RelatedClassifierTypeCodeID = r.RelatedClassifierTypeCodeID
  )
  AND IsDeleted = 0
;
