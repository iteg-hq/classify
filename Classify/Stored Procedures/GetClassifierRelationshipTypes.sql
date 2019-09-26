CREATE PROCEDURE dbo.GetClassifierRelationshipTypes
    --@ClassifierTypeCode NVARCHAR(500)
AS
SET NOCOUNT, XACT_ABORT ON;

SELECT DISTINCT ClassifierRelationshipTypeCode
FROM ClassifierRelationshipType
ORDER BY 1

/*
SELECT DISTINCT ClassifierRelationshipTypeCode
FROM ClassifierRelationship
WHERE ClassifierTypeCode = @ClassifierTypeCode
ORDER BY 1
;
*/