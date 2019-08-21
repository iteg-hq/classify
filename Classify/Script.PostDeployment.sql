-- Insert system data rows
SET IDENTITY_INSERT internal.Code ON
IF NOT EXISTS (SELECT 1 FROM internal.Code WHERE CodeID = -1 )
  INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( -1, '' )
IF NOT EXISTS (SELECT 1 FROM internal.Code WHERE CodeID = 0 )
  INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( 0, 'ClassifierRelationshipType' )
SET IDENTITY_INSERT internal.Code OFF

EXEC SaveClassifierType 'ClassifierRelationshipType', 'Classifier Relationship Type', 'The type of classifiers that classify classifier relationships';
