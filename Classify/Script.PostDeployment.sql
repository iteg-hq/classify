-- Insert system data rows
IF NOT EXISTS (SELECT 1 FROM internal.Code WHERE CodeID = -1 ) INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( -1, '' ); -- Blank is reserved for the classifier code assigned to classifier type instances
IF NOT EXISTS (SELECT 1 FROM internal.Code WHERE CodeID = 0 )  INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( 0, 'ClassifierRelationshipType' );

EXEC SaveClassifierType 'ClassifierRelationshipType', 'Classifier Relationship Type', 'The type of classifiers that classify classifier relationships';
