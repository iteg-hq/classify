CREATE VIEW dbo.Classifier
AS
SELECT
    ot.ClassifierID
  , ct.CodeValue AS ClassifierTypeCode
  , c.CodeValue AS ClassifierCode
  , ot.[Name] AS ClassifierName
  , ot.[Description] AS ClassifierDescription
  , ot.UpdatedBy
  , ot.UpdatedOn
FROM internal.Classifier AS ot
INNER JOIN internal.Code AS c
  ON c.CodeID = ot.ClassifierCodeID
INNER JOIN internal.Code AS ct
  ON ct.CodeID = ot.ClassifierTypeCodeID
WHERE ot.rv = (
    SELECT MAX(rv)
    FROM internal.Classifier AS it
    WHERE it.ClassifierCodeID = ot.ClassifierCodeID
      AND it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
  )
  AND ot.IsDeleted = 0
  AND ot.ClassifierTypeCodeID != 0 -- Not relationship types
  AND ot.ClassifierCodeID != -1 -- Not classifier types
;
