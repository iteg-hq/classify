namespace Classify
{
    public interface IClassifierRelationship
    {
        IClassifier Classifier { get; set; }
        IClassifier RelatedClassifier { get; set; }
        string RelationshipTypeCode { get; set; }
        string Description { get; set; }
        double Weight { get; set; }
    }
}