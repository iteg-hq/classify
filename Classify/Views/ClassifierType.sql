CREATE VIEW dbo.ClassifierType
AS
SELECT
    c.CodeValue AS TypeCode
  , ot.[Name]
  , ot.[Description]
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
