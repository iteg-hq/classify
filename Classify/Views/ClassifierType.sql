CREATE VIEW dbo.ClassifierType
AS
SELECT
    c.CodeValue AS ClassifierTypeCode
  , ot.[Name] AS ClassifierTypeName
  , ot.[Description] AS ClassifierTypeDescription
  , ot.UpdatedBy
  , ot.UpdatedOn
FROM internal.Classifier AS ot
INNER JOIN internal.Code AS c
  ON c.CodeID = ot.ClassifierTypeCodeID
WHERE ot.rv = (
    SELECT MAX(rv)
    FROM internal.Classifier AS it
    WHERE it.ClassifierCodeID = ot.ClassifierCodeID
      AND it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
  )
  AND ot.IsDeleted = 0
  AND ot.ClassifierTypeCodeID != 0
  AND ot.ClassifierCodeID = -1 -- Classifier types
;
