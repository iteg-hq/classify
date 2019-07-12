CREATE PROCEDURE dbo.DeleteClassifierType
    @TypeCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @TypeCodeID INT;

EXEC internal.GetCodeID @TypeCode, @TypeCodeID OUTPUT;

INSERT INTO internal.ClassifierType (
    ClassifierTypeCodeID
  , IsDeleted
  )
VALUES (
    @TypeCodeID
  , 1
  )
;

CREATE TABLE #DeleteThese (
    ClassifierTypeCodeID INT NOT NULL
  , ClassifierCodeID INT NOT NULL
  , PRIMARY KEY ( ClassifierTypeCodeID, ClassifierCodeID )
)

INSERT INTO #DeleteThese (
    ClassifierTypeCodeID
  , ClassifierCodeID
  )
SELECT 
    c1.CodeID
  , c2.CodeID
FROM dbo.Classifier AS cl
INNER JOIN internal.Code AS c1
  ON c1.CodeValue = cl.ClassifierTypeCode
INNER JOIN internal.Code AS c2
  ON c2.CodeValue = cl.ClassifierCode
WHERE ClassifierTypeCode = @TypeCode
;

INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , IsDeleted
  )
SELECT
    ClassifierTypeCodeID
  , ClassifierCodeID
  , 1
FROM #DeleteThese
;



