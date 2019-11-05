# Classify

## Installation

Install Classify and connect to the instance:

1. Download the latest dacpac from https://github.com/iteg-hq/classify/releases/latest.
2. Install it [(instruction)]( https://docs.microsoft.com/en-us/sql/relational-databases/data-tier-applications/deploy-a-data-tier-application?view=sql-server-ver15#deploy-a-dac-using-the-wizard).
3. Open a connection to the server in SQL Server Management Studio or similar, then issue `USE Classify` to connect to the database.
4. Run `About` to check that Classify is installed in the database

## Introduction (SQL)

This section introduces the main concepts of Classify and demonstrates the SQL API using an (only somewhat contrived) example.

### Classifiers and classifier types

Classify is a tool for creating and managing simple master data entities called *classifiers*. Classifiers can be used to represent entities about which you need to store data.

Classifiers could be used to store data about countries: Existing data contain a country code (e.g. "DK"), but you need the name of the country (in this case, "Denmark"). Storing the country master data as classifiers provides a simple way to associate the name with the code.

Since the code "DK" may not be unique across all kinds of classifiers, our classifier must belong to a *classifier type*. The code must be unique in the context of that type.

Create the country classifier type with `SaveClassifierType`:

```mssql
EXEC SaveClassifierType @Code='country', 
                        @Name='Country',
                        @Description='Country as defined in ISO 3166.';

SELECT * FROM ClassifierType;
```

Note that `ClassifierType` has audit information: `UpdatedBy` and `UpdatedOn` show who made the last change, and when.

Next, create Denmark as a classifier:

```mssql
EXEC SaveClassifier @TypeCode='country',
                    @Code='DK',
                    @Name='Denmark';

SELECT * FROM Classifier;
```

You can change the name of a classifier by calling `SaveClassifier` again:

```mssql
EXEC SaveClassifier @TypeCode='country',
                    @Code='DK',
                    @Name='Danmark';

SELECT * FROM Classifier;
```

Classify is insert-only, and a full audit trail is stored. The audit information is available in the `history` schema:

```mssql
SELECT * FROM history.Classifier;
```

### Classifier relationships

Classifiers have no attributes beyond a name and a description. If you need to store more complex information, like the fact that Denmark belongs to Scandinavia, you'll need to express that as a relationship between two classifiers (in this case, Denmark and Scandinavia).

First, create the classifier for Scandinavia:

```mssql
EXEC SaveClassifier @TypeCode='country_group',
                    @Code='scandinavia',
                    @Name='Scandinavia';
```

(Note that we didn't explicitly create the classifier type "country_group". Classify create the type when we first use it).

Now, we can relate Denmark to Scandinavia:

```mssql
EXEC SaveClassifierRelationship @ClassifierTypeCode = 'country',
                                @ClassifierCode = 'DK',
                                @ClassifierRelationshipTypeCode = 'Belongs to',
                                @RelatedClassifierTypeCode = 'country_group',
                                @RelatedClassifierCode = 'scandinavia';
                                
SELECT * FROM ClassifierRelationship;
```

Again, we didn't define the relationship type code "Belongs to" (although there is a procedure for that: `SaveClassifierRelationshipType`).

In fact, Classify will create both classifier types, classifiers, and classifier relationship types whenever we use them. We can record that Denmark uses the currency with code 'DKK' with `SaveClassifierRelationship`:

```mssql
EXEC SaveClassifierRelationship @ClassifierTypeCode = 'country',
                                @ClassifierCode = 'DK',
                                @ClassifierRelationshipTypeCode = 'Uses currency',
                                @RelatedClassifierTypeCode = 'currency',
                                @RelatedClassifierCode = 'DKK';
                                
SELECT * FROM ClassifierRelationship;
```

In our example, we might like to attach a name to 'DKK' at some point. We can do this by calling `SaveClassifier`:

```mssql
EXEC SaveClassifier @TypeCode='currency',
                    @Code='DKK',
                    @Name='Danish Kroner';
```

Implicit definition of types, classifiers and relationship types means that using (for instance) an undefined classifier is never an error. It simplifies the code that you submit to Classify, but can be a source of subtle errors.

## Introduction (Web app)

This section repeats the example from the previous section, only using the web app.

### Classifiers and classifier types

Classify is a tool for creating and managing simple master data entities called *classifiers*. Classifiers can be used to represent entities about which you need to store data.

Classifiers could be used to store data about countries: Existing data contain a country code (e.g. "DK"), but you need the name of the country (in this case, "Denmark"). Storing the country master data as classifiers provides a simple way to associate the name with the code.

Since the code "DK" may not be unique across all kinds of classifiers, our classifier must belong to a *classifier type*. The code must be unique in the context of that type.

First, add a classifier type called "Country":

- Click the "Add Type..." button
- Set Code = "country"
- Set Name = "Country"
- Click "Save", then "Close"

Then add "Denmark" as a classifier belonging to that type:

- Click the "Add Classifier..." button
- Set Code = "DK"
- Set Name = "Denmark"
- Click "Save", then "Close"

Then try changing the name of the classifier:

- Click the "Edit Classifier..." button
- Set Name = "Danmark"
- Click "Save", then "Close"

### Classifier relationships

Classifiers have no attributes beyond a name and a description. If you need to store more complex information, like the fact that Denmark belongs to Scandinavia, you'll need to express that as a relationship between two classifiers (in this case, Denmark and Scandinavia).

Add a new classifier type for country groups:

- Click the "Add Type..." button
- Set Code = "Country Group"
- Click "Save", then "Close"

Add "Scandinavia" as a classifier:

- Click the "Add Classifier..." button
- Set Code = "Scandinavia"
- Click "Save", then "Close"

Add "Belongs to" as a classifier relationship type:

- Select the "Classifier Relationship Type" under "Types".
- Click the "Add Classifier..." button
- Set Code = "Belongs to"
- Click "Save", then "Close"

Finally, relate Denmark to Scandinavia:

- Select the "Country" type
- Select "Danmark"
- Click "Add related classifier..."
-  Select relationship type "Belongs to"
- Select classifier type "Country Group"
- Select classifier "Scandinavia"
- Click "Save", then "Close"

The web app does not support implicit creation of classifiers, so relating a country to a currency requires that you repeat the last four steps, using "Currency", "Danish Kroner", and "Uses currency" in the first three of those steps.

## SQL API

### About

Returns single row containing the name and version of Classify (e.g. 'Classify v2')

### SaveClassifierType

Create or alter a classifier type.

*Parameters:*

- ClassifierTypeCode: The type code that identifies the classifier type. This cannot be changed.
- Name: The name of the classifier type. If no name is given, the name will be equal to the code.
- Description: 

###  SaveClassifier

Create or alter a classifier. The classifier must belong to a type.

*Parameters:*

- ClassifierTypeCode: The type code of the classifier. This identifies the classifier and cannot be changed.
- ClassifierCode: The code that identifies the classifier. Must be unique within the scope of the classifier type.
- Name: The name of the classifier. If no name is given, the name will be equal to the code.
- Description: A description of the classifier.

### CopyClassifier

Copies a classifier to a new classifier of the same type.

*Parameters:*

- ClassifierTypeCode:  The type code of the source classifier - the target classifier will have the same type.
- ClassifierCode:  The code of the source classifier.
- NewClassifierCode:  The code of the target classifier.



### DeleteClassifier

Deletes a classifier and all its relationships (both inbound and outbound).

*Parameters:*

- ClassifierTypeCode: The type code of the classifier.
- ClassifierCode: The code of the classifier.

### DeleteClassifierRelationship

 Deletes a single classifier relationship

*Parameters:*

- ClassifierTypeCode: The type code of the classifier.
- ClassifierCode: The code of the classifier.
- ClassifierRelationshipType: The code of the relationship type.
- RelatedClassifierTypeCode: The type code of the related classifier.
- RelatedClassifierCode: The code of the related classifier.

### DeleteClassifierType

Deletes a classifier type as well as all classifiers and classifier relationships associated with it.

*Parameters:*

- ClassifierTypeCode: The code of the classifier type.

###  SaveClassifierRelationship

Create or alter a relationship between two classifiers.

*Parameters:*

- ClassifierTypeCode: The type code of the primary classifier.
- ClassifierCode: The code of the primary classifier.
- ClassifierRelationshipTypeCode: The code identifying the relationship type.
- RelatedClassifierTypeCode:  The type code of the related classifier.
- RelatedClassifierCode:  The code of the related classifier.
- Description: A description of the relationship.
- Weight (float): A numeric weight that quantifies the "strength" of the relationship. Used to express relationships that are not 

### SaveClassifierRelationshipType

Create or alter a relationship type.

*Parameters:*

- ClassifierRelationshipTypeCode: The code of the relationship
- Name: The name of the relationship
- Description: Description of the relationship

