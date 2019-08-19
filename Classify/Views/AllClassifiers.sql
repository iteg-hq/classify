CREATE VIEW dbo.AllClassifiers
AS
SELECT
    ct.CodeValue AS ClassifierTypeCode
  , c.CodeValue AS ClassifierCode
  , ot.[Name] AS ClassifierName
  , ot.[Description] AS ClassifierDescription
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
;