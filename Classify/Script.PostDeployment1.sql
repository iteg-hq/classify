/*
Post-Deployment Script Template              
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.    
 Use SQLCMD syntax to include a file in the post-deployment script.      
 Example:      :r .\myfile.sql                
 Use SQLCMD syntax to reference a variable in the post-deployment script.    
 Example:      :setvar TableName MyTable              
               SELECT * FROM [$(TableName)]          
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT internal.Code ON
INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( -1, '' )
INSERT INTO internal.Code ( CodeID, CodeValue ) VALUES ( 0, 'ClassifierRelationshipType' )
SET IDENTITY_INSERT internal.Code OFF

EXEC SaveClassifierType 'ClassifierRelationshipType', 'Classifier Relationship Type', 'The type of classifiers that classify classifier relationships';
