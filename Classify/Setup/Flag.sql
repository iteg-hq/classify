CREATE PROCEDURE setup.Flag
AS
EXEC SaveClassifierType 'Flag', 'Flag', 'Logical values (true/false).';
EXEC SaveClassifier 'Flag', '1',  'True';
EXEC SaveClassifier 'Flag', '0', 'False';
