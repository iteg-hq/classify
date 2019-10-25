
  - - - - # Classify
          
            ## SQL API
          
            - **About:** Returns single row containing the name and version of Classify (e.g. 'Classify v2')
          
            - **CopyClassifier:** Copies a classifier to a new classifier of the same type.
          
              Parameters:
          
              - **@ClassifierTypeCode:**  The type code of the source classifier - the target classifier will have the same type.
              - **@ClassifierCode:**  The code of the source classifier.
              - **@NewClassifierCode:**  The code of the target classifier.
          
              
          
            - **DeleteClassifier:** Deletes a classifier and all its relationships (both inbound and outbound).
          
              Parameters:
          
              - **@ClassifierTypeCode:** The type code of the classifier.
              - **@ClassifierCode:** The code of the classifier.
          
            - **DeleteClassifierRelationship:** Deletes a single classifier relationship
          
              Parameters:
          
              - **@ClassifierTypeCode:** The type code of the classifier.
              - **@ClassifierCode:** The code of the classifier.
              - **@ClassifierRelationshipType:** The code of the relationship type.
              - **@RelatedClassifierTypeCode:** The type code of the related classifier.
              - **@RelatedClassifierCode:** The code of the related classifier.
          
            - **DeleteClassifierType:** Deletes a classifier type as well as all classifiers and classifier relationships associated with it.
          
              Parameters:
          
              - **@ClassifierTypeCode:** The code of the type.
          
            - **GetClassifierRelationships:** Get all *outbound* relationships of a classifier.
          
              Parameters:
          
              - **@ClassifierTypeCode:**  The type code of the classifier.
              - **@ClassifierCode:** The code of the classifier.
          
            - **GetClassifiers:** get all classifiers of a specific type.
          
              Parameters:
          
              - **@ClassifierTypeCode:** The code of the type.
          
            - **GetClassifierType:** Gets a classifier type.
          
              Parameters:
          
              - **@ClassifierTypeCode:** 
          
            - **GetClassifierTypes:**
          
            - **SaveClassifier:**
          
              Parameters:
          
              - **@ClassifierTypeCode:** 
              - **@ClassifierCode:** 
              - **@Name:** 
              - **@Description:** 
          
            - **SaveClassifierRelationship:**
          
              Parameters:
          
              - **@ClassifierTypeCode:** 
              - **@ClassifierCode:** 
              - **@ClassifierRelationshipTypeCode:** 
              - **@RelatedClassifierTypeCode:** 
              - **@RelatedClassifierCode:** 
              - **@Description:** 
              - **@Weight (float):** 
          
            - **SaveClassifierRelationshipType:**
          
              Parameters:
          
              - **@ClassifierRelationshipTypeCode:** 
              - **@Name:** 
              - **@Description:** 
          
            - **SaveClassifierType:**
          
              Parameters:
          
              - **@ClassifierTypeCode:** 
              - **@Name:** 
              - **@Description:** 