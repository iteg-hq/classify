CREATE PROCEDURE dbo.SaveClassifierRelationship
    @ClassifierTypeCode NVARCHAR(200)
  , @ClassifierCode NVARCHAR(200)
  , @ClassifierRelationshipTypeCode NVARCHAR(200)
  , @RelatedClassifierTypeCode NVARCHAR(200)
  , @RelatedClassifierCode NVARCHAR(200)
  , @Description NVARCHAR(500) = NULL
  , @Weight FLOAT = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

IF NOT EXISTS (
    SELECT 1
    FROM dbo.Classifier
    WHERE ClassifierTypeCode = @ClassifierTypeCode
      AND ClassifierCode = @ClassifierCode
  )
  EXEC dbo.SaveClassifier @ClassifierTypeCode, @ClassifierCode;

IF NOT EXISTS (
    SELECT 1
    FROM dbo.Classifier
    WHERE ClassifierTypeCode = @RelatedClassifierTypeCode
      AND ClassifierCode = @RelatedClassifierCode
  )
  EXEC dbo.SaveClassifier @RelatedClassifierTypeCode, @RelatedClassifierCode;

IF NOT EXISTS (
    SELECT 1
    FROM dbo.ClassifierRelationshipType
    WHERE ClassifierRelationshipTypeCode = @ClassifierRelationshipTypeCode
  )
  EXEC dbo.SaveClassifierRelationshipType @ClassifierRelationshipTypeCode



DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;
DECLARE @ClassifierRelationshipTypeCodeID INT;
DECLARE @RelatedClassifierTypeCodeID INT;
DECLARE @RelatedClassifierCodeID INT;

EXEC internal.GetCodeID @ClassifierTypeCode, @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode, @ClassifierCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierRelationshipTypeCode, @ClassifierRelationshipTypeCodeID OUTPUT;
EXEC internal.GetCodeID @RelatedClassifierTypeCode, @RelatedClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @RelatedClassifierCode, @RelatedClassifierCodeID OUTPUT;

INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , RelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , [Description]
  , [Weight]
  )
VALUES (
    @ClassifierTypeCodeID
  , @ClassifierCodeID
  , @ClassifierRelationshipTypeCodeID
  , @RelatedClassifierTypeCodeID
  , @RelatedClassifierCodeID
  , COALESCE(@Description, '')
  , COALESCE(@Weight, 100.0)
  )
;