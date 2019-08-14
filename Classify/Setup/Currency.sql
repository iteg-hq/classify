CREATE PROCEDURE setup.Currency
AS
EXEC SaveClassifierType 'Currency', 'Currency', 'An ISO 4217 currency';
EXEC SaveClassifier 'Currency', 'DKK',  'Danish Kroner';
EXEC SaveClassifier 'Currency', 'GBP',  'British Pound';
EXEC SaveClassifier 'Currency', 'EUR',  'Euro';

EXEC SaveClassifierType 'CurrencyPair', 'Currency Pair', 'An ISO 4217 currency pair';
EXEC SaveClassifier 'CurrencyPair', 'GBP/DKK',  'British Pound in Kroner';
EXEC SaveClassifier 'CurrencyPair', 'EUR/DKK',  'Euro in Kroner';

EXEC SaveClassifierRelationship 'CurrencyPair', 'EUR/DKK', 'ExchangeRate', 'Date', '2019-01-01', @Weight = 746.33;
EXEC SaveClassifierRelationship 'CurrencyPair', 'GBP/DKK', 'ExchangeRate', 'Date', '2019-01-01', @Weight = 803.98;
