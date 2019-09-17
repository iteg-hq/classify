CREATE PROCEDURE dbo.CopyClassifier
    @ClassifierTypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
  , @NewClassifierCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;
IF @ClassifierTypeCode  = '' THROW 51000, '@ClassifierTypeCode cannot be blank', 1;
IF @ClassifierCode = '' THROW 51000, '@ClassifierCode cannot be blank', 1;
IF @NewClassifierCode = '' THROW 51000, '@NewClassifierCode cannot be blank', 1;

-- Copy-to-self is a no-op
IF @ClassifierCode = @NewClassifierCode
  RETURN

-- Get IDs
DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;
DECLARE @NewClassifierCodeID INT;

EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode, @ClassifierCodeID OUTPUT;
EXEC internal.GetCodeID @NewClassifierCode, @NewClassifierCodeID OUTPUT;

-- Reinsert most recent record of all classifier relationships (inbound and outbound), if non-deleted
INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , ClassifierRelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , [Description]
  , [Weight]
  , IsDeleted    
  )
SELECT
    ClassifierTypeCodeID
  , @NewClassifierCodeID
  , ClassifierRelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , [Description]
  , [Weight]
  , IsDeleted
FROM internal.ClassifierRelationship AS ot
WHERE ClassifierTypeCodeID = @ClassifierTypeCodeID
  AND ClassifierCodeID = @ClassifierCodeID
  AND ot.rv = (
    SELECT MAX(rv)
    FROM internal.ClassifierRelationship AS it
    WHERE it.ClassifierCodeID = ot.ClassifierCodeID
      AND it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
      AND it.ClassifierRelationshipTypeCodeID = ot.ClassifierRelationshipTypeCodeID
      AND it.RelatedClassifierCodeID = ot.RelatedClassifierCodeID
      AND it.RelatedClassifierTypeCodeID = ot.RelatedClassifierTypeCodeID
  )
  AND ot.IsDeleted = 0
;

INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , ClassifierRelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , [Description]
  , [Weight]
  , IsDeleted 
  )
SELECT
    ClassifierTypeCodeID
  , ClassifierCodeID
  , ClassifierRelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , @NewClassifierCodeID
  , [Description]
  , [Weight]
  , IsDeleted
FROM internal.ClassifierRelationship AS ot
WHERE RelatedClassifierTypeCodeID = @ClassifierTypeCodeID
  AND RelatedClassifierCodeID = @ClassifierCodeID
  AND ot.rv = (
    SELECT MAX(rv)
    FROM internal.ClassifierRelationship AS it
    WHERE it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
      AND it.ClassifierCodeID = ot.ClassifierCodeID
      AND it.ClassifierRelationshipTypeCodeID = ot.ClassifierRelationshipTypeCodeID
      AND it.RelatedClassifierTypeCodeID = ot.RelatedClassifierTypeCodeID
      AND it.RelatedClassifierCodeID = ot.RelatedClassifierCodeID
  )
  AND ot.IsDeleted = 0
;

INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , Description
  , IsDeleted
  )
SELECT
    ClassifierTypeCodeID
  , @NewClassifierCodeID
  , [Name]
  , [Description]
  , IsDeleted
FROM internal.Classifier AS ot
WHERE ClassifierTypeCodeID = @ClassifierTypeCodeID
  AND ClassifierCodeID = @ClassifierCodeID
  AND ot.rv = (
    SELECT MAX(rv)
    FROM internal.Classifier AS it
    WHERE it.ClassifierTypeCodeID = ot.ClassifierTypeCodeID
      AND it.ClassifierCodeID = ot.ClassifierCodeID
  )
  AND ot.IsDeleted = 0
;