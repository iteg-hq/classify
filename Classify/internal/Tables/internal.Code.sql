﻿CREATE TABLE internal.Code (
    CodeID INT NOT NULL IDENTITY(1,1)
  , CodeValue NVARCHAR(500) NOT NULL
  , CONSTRAINT PK_Code PRIMARY KEY (CodeID)
  , CONSTRAINT UQ_Code UNIQUE (CodeValue)
)