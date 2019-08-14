CREATE VIEW dbo.ClassifierType
AS
SELECT
    c.CodeValue AS ClassifierTypeCode
  , ot.[Name] AS ClassifierTypeName
  , ot.[Description] AS ClassifierTypeDescription
FROM internal.ClassifierType AS ot
INNER JOIN internal.Code AS c
  ON c.CodeID = ot.ClassifierTypeCodeID
WHERE rv = (
    SELECT MAX(rv)
    FROM internal.ClassifierType AS it
    WHERE it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
  )
  AND IsDeleted = 0
;
