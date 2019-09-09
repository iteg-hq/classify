namespace Classify
{
    public class ClassifierRelationship
    {
        private readonly DAL dal;
        private readonly string classifierTypeCode;
        private readonly string classifierCode;
        private readonly string relatedClassifierTypeCode;
        private readonly string relatedClassifierCode;

        public Classifier Classifier { get => dal.GetClassifier(classifierTypeCode, classifierCode); }
        public Classifier RelatedClassifier { get => dal.GetClassifier(relatedClassifierTypeCode, relatedClassifierCode); }

        public string RelationshipTypeCode;
        public string Description;
        public double Weight = 100.0;

        public override string ToString()
        {
            return $"{Classifier} [{RelationshipTypeCode}] {RelatedClassifier}";
        }
        public ClassifierRelationship(
            DAL dal,
            string classifierTypeCode,
            string classifierCode,
            string relationshipTypeCode,
            string relatedClassifierTypeCode,
            string relatedClassifierCode
            )
        {
            this.dal = dal;
            this.classifierTypeCode = classifierTypeCode;
            this.classifierCode = classifierCode;
            RelationshipTypeCode = relationshipTypeCode;
            this.relatedClassifierTypeCode = relatedClassifierTypeCode;
            this.relatedClassifierCode = relatedClassifierCode;
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