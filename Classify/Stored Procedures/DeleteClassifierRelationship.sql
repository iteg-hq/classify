CREATE PROCEDURE dbo.DeleteClassifierRelationship
    @ClassifierTypeCode NVARCHAR(200)
  , @ClassifierCode NVARCHAR(200)
  , @ClassifierRelationshipTypeCode NVARCHAR(200)
  , @RelatedClassifierTypeCode NVARCHAR(200)
  , @RelatedClassifierCode NVARCHAR(200)
AS
SET NOCOUNT, XACT_ABORT ON;

DECLARE @ClassifierTypeCodeID INT;
DECLARE @ClassifierCodeID INT;
DECLARE @ClassifierRelationshipTypeCodeID INT;
DECLARE @RelatedClassifierTypeCodeID INT;
DECLARE @RelatedClassifierCodeID INT;

EXEC internal.GetCodeID @ClassifierTypeCode,             @ClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierCode,                 @ClassifierCodeID OUTPUT;
EXEC internal.GetCodeID @ClassifierRelationshipTypeCode, @ClassifierRelationshipTypeCodeID OUTPUT;
EXEC internal.GetCodeID @RelatedClassifierTypeCode,      @RelatedClassifierTypeCodeID OUTPUT;
EXEC internal.GetCodeID @RelatedClassifierCode,          @RelatedClassifierCodeID OUTPUT;

INSERT INTO internal.ClassifierRelationship (
    ClassifierTypeCodeID
  , ClassifierCodeID
  , ClassifierRelationshipTypeCodeID
  , RelatedClassifierTypeCodeID
  , RelatedClassifierCodeID
  , IsDeleted
  )
VALUES (
    @ClassifierTypeCodeID
  , @ClassifierCodeID
  , @ClassifierRelationshipTypeCodeID
  , @RelatedClassifierTypeCodeID
  , @RelatedClassifierCodeID
  , 1
  )
;