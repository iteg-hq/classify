
  - - # Classify
      
        ## SQL API
      
        - **About:** Returns single row containing the name and version of Classify (e.g. 'Classify v2')
      
        - **CopyClassifier:** Copies a classifier to a new classifier of the same type.
          
          Parameters:
          
        - **@ClassifierTypeCode:** 
          - **@ClassifierCode:** 
          - **@NewClassifierCode:** 
          
        - **DeleteClassifier:**
          
          Parameters:
      
          - **@TypeCode:** 
          - **@ClassifierCode:** 
          
        - **DeleteClassifierRelationship:**
          
          Parameters:
          
          - **@ClassifierTypeCode:** 
          - **@ClassifierCode:** 
          - **@ClassifierRelationshipType:** 
          - **@RelatedClassifierTypeCode:** 
          - **@RelatedClassifierCode:** 
          
        - **DeleteClassifierType:**
          
          Parameters:
          
          - **@TypeCode:** 
          
        - **GetClassifierRelationships:**
          - **@ClassifierTypeCode:** 
          - **@ClassifierCode:** 
          
        - **GetClassifiers:**
          
          - **@ClassifierTypeCode:** 
          
        - **GetClassifierType:**
          
          - **@ClassifierTypeCode:** 
          
        - **GetClassifierTypes:**
      
        - **SaveClassifier:**
          
          - **@TypeCode:** 
          - **@Code:** 
          - **@Name:** 
          - **@Description:** 
          
        - **SaveClassifierRelationship:**
          - **@ClassifierTypeCode:** 
          - **@ClassifierCode:** 
          - **@ClassifierRelationshipTypeCode:** 
          - **@RelatedClassifierTypeCode:** 
          - **@RelatedClassifierCode:** 
          - **@Description:** 
          - **@Weight FLOAT:** 
          
        - **SaveClassifierRelationshipType:**
          - **@ClassifierRelationshipTypeCode:** 
          - **@Name:** 
          - **@Description:** 
          
        - **SaveClassifierType:**
          - **@TypeCode:** 
          - **@Name:** 
          - **@Description:** 