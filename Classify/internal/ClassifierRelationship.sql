CREATE TABLE internal.ClassifierRelationship (
    ClassifierTypeCodeID INT NOT NULL
  , ClassifierCodeID INT NOT NULL
  , RelationshipTypeCodeID INT NOT NULL
  , RelatedClassifierTypeCodeID INT NOT NULL
  , RelatedClassifierCodeID INT NOT NULL
  , [Description] NVARCHAR(500) NOT NULL DEFAULT ''
  , [Weight] FLOAT NULL
  , IsDeleted BIT NOT NULL DEFAULT 0
  , rv ROWVERSION
  , UpdatedBy SYSNAME NOT NULL DEFAULT CURRENT_USER
  , CONSTRAINT PK_ClassifierRelationship PRIMARY KEY (ClassifierTypeCodeID, ClassifierCodeID, RelationshipTypeCodeID, RelatedClassifierTypeCodeID, RelatedClassifierCodeID, rv)
)
