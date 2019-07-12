namespace Classify
{
    public class ClassifierRelationship
    {
        public Classifier Classifier;
        public Classifier RelatedClassifier;
        public string RelationshipTypeCode;
        public string Description;
        public double Weight = 100.0;

        public override string ToString()
        {
            return $"{Classifier} [{RelationshipTypeCode}] {RelatedClassifier}";
        }
        public ClassifierRelationship(Classifier classifier, string relationshipTypeCode, Classifier relatedClassifier)
        {
            Classifier = classifier;
            RelatedClassifier = relatedClassifier;
            RelationshipTypeCode = relationshipTypeCode;
        }
    }

    /*
    public class ClassifierRelationshipView
    {
        public Classifier Classifier;
        public ClassifierRelationship ClassifierRelationship;

        public ClassifierRelationshipView(Classifier classifier, ClassifierRelationship classifierRelationship)
        {
            Classifier = classifier;
            ClassifierRelationship = classifierRelationship;
        }

        public override string ToString()
        {
            return Description;
        }
        public string Description
        {
            get
            {
                if (Classifier == ClassifierRelationship.Classifier)
                {
                    return $"this [{ClassifierRelationship.RelationshipTypeCode}] {ClassifierRelationship.RelatedClassifier} {ClassifierRelationship.Weight.ToString()}";
                }
                else if (Classifier == ClassifierRelationship.RelatedClassifier)
                {
                    return $"{ClassifierRelationship.Classifier} [{ClassifierRelationship.RelationshipTypeCode}] this {ClassifierRelationship.Weight.ToString()}";
                }
                else
                {
                    return $"!!!! {ClassifierRelationship.Classifier} [{ClassifierRelationship.RelationshipTypeCode}] {ClassifierRelationship.RelatedClassifier} {ClassifierRelationship.Weight.ToString()} !!!!";
                }

            }
        }
    }
    */
}