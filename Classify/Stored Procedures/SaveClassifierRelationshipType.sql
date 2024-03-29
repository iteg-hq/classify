﻿CREATE PROCEDURE dbo.SaveClassifierRelationshipType
    @Code NVARCHAR(200)
  , @Name NVARCHAR(100) = NULL
  , @Description NVARCHAR(500) = NULL
AS
SET NOCOUNT, XACT_ABORT ON;

EXEC internal.SaveClassifier 'ClassifierRelationshipType', @Code, @Name, @Description;
;