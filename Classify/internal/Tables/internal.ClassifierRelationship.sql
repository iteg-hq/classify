CREATE TABLE internal.ClassifierRelationship (
    ClassifierRelationshipID INT NOT NULL
  , ClassifierTypeCodeID INT NOT NULL
  , ClassifierCodeID INT NOT NULL
  , ClassifierRelationshipTypeCodeID INT NOT NULL
  , RelatedClassifierTypeCodeID INT NOT NULL
  , RelatedClassifierCodeID INT NOT NULL
  , [Description] NVARCHAR(500) NOT NULL DEFAULT ''
  , [Weight] FLOAT NULL
  , IsDeleted BIT NOT NULL DEFAULT 0
  , rv ROWVERSION
  , UpdatedBy SYSNAME NOT NULL DEFAULT CURRENT_USER
  , UpdatedOn DATETIME2(7) NOT NULL DEFAULT CURRENT_TIMESTAMP
  , CONSTRAINT PK_ClassifierRelationship PRIMARY KEY (ClassifierTypeCodeID, ClassifierCodeID, ClassifierRelationshipTypeCodeID, RelatedClassifierTypeCodeID, RelatedClassifierCodeID, rv)
)
