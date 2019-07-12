CREATE TABLE internal.ClassifierType (
    ClassifierTypeCodeID INT NOT NULL
  , [Name] NVARCHAR(100) NOT NULL DEFAULT ''
  , [Description] NVARCHAR(500) NOT NULL DEFAULT ''
  , IsDeleted BIT NOT NULL DEFAULT 0
  , rv ROWVERSION
  , UpdatedBy SYSNAME NOT NULL DEFAULT CURRENT_USER
  , CONSTRAINT PK_ClassifierType PRIMARY KEY (ClassifierTypeCodeID, rv)
)
