CREATE PROCEDURE dbo.DeleteClassifier
    @ClassifierTypeCode NVARCHAR(100)
  , @ClassifierCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;
IF @ClassifierCode = '' THROW 51000, 'Classifier Code cannot be blank', 1;

DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;
EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode, @ClassifierCodeID OUTPUT;

-- Close all classifier relationships (inbound and outbound)
INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , RelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , IsDeleted    
  )
SELECT DISTINCT
    ClassifierTypeCodeID
  , ClassifierCodeID
  , RelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , 1
FROM internal.ClassifierRelationship
WHERE (ClassifierTypeCodeID = @ClassifierTypeCodeID AND ClassifierCodeID = @ClassifierCodeID)
  OR  (RelatedClassifierTypeCodeID = @ClassifierTypeCodeID AND RelatedClassifierCodeID = @ClassifierCodeID)
;

-- Close the classifier
INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , IsDeleted
  )
VALUES (
    @ClassifierTypeCodeID
  , @ClassifierCodeID
  , 1
  )
;