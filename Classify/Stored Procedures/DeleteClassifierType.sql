CREATE PROCEDURE dbo.DeleteClassifierType
    @TypeCode NVARCHAR(100)
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @ClassifierTypeCodeID INT;
EXEC internal.GetCodeID @TypeCode, @ClassifierTypeCodeID OUTPUT;

-- Delete all type member relationships (inbound and outbound)
INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , RelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , Description
  , IsDeleted    
  )
SELECT
    ClassifierTypeCodeID
  , ClassifierCodeID
  , RelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , Description
  , 1
FROM internal.ClassifierRelationship
WHERE ClassifierTypeCodeID = @ClassifierTypeCodeID
  OR  RelatedClassifierTypeCodeID = @ClassifierTypeCodeID;

  -- Delete all type members
INSERT INTO internal.Classifier (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , Description
  , IsDeleted    
  )
SELECT
    ClassifierTypeCodeID
  , ClassifierCodeID
  , [Name]
  , Description
  , 1
FROM internal.Classifier
WHERE ClassifierTypeCodeID = @ClassifierTypeCodeID;

-- Then delete the type
EXEC internal.DeleteClassifier @TypeCode, '';
