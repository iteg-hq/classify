CREATE VIEW dbo.ClassifierRelationshipType
AS
SELECT
    ot.ClassifierID AS ClassifierRelationshipTypeID
  , c.CodeValue AS ClassifierRelationshipTypeCode
  , ot.[Name] AS ClassifierRelationshipTypeName
  , ot.[Description] AS ClassifierRelationshipTypeDescription
  , ot.UpdatedBy
  , ot.UpdatedOn
FROM internal.Classifier AS ot
INNER JOIN internal.Code AS c
  ON c.CodeID = ot.ClassifierCodeID
WHERE ot.rv = (
    SELECT MAX(rv)
    FROM internal.Classifier AS it
    WHERE it.ClassifierCodeID = ot.ClassifierCodeID
      AND it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
  )
  AND ot.IsDeleted = 0
  AND ot.ClassifierTypeCodeID = 0 -- Relationship types
  AND ot.ClassifierCodeID != -1 -- Not classifier types

;
