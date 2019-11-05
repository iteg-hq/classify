CREATE VIEW history.Classifier
AS
SELECT
    ct.CodeValue AS ClassifierTypeCode
  , c.CodeValue AS ClassifierCode
  , ot.[Name] AS ClassifierName
  , ot.[Description] AS ClassifierDescription
  , ot.UpdatedBy
  , ot.UpdatedOn
  , ot.IsDeleted
FROM internal.Classifier AS ot
INNER JOIN internal.Code AS c
  ON c.CodeID = ot.ClassifierCodeID
INNER JOIN internal.Code AS ct
  ON ct.CodeID = ot.ClassifierTypeCodeID
WHERE ot.IsDeleted = 0
  AND ot.ClassifierTypeCodeID != 0 -- Not relationship types
  AND ot.ClassifierCodeID != -1 -- Not classifier types
;
