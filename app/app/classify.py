import pyodbc

class ClassifierCollection(object):
    def __init__(self):
        self.types = list()
        self.classifiers = list()
        self.relationships = list()

    def add_classifier(self, type_code, code, name, description):
        type_ = self.get_classifier_type_by_code(type_code)
        classifier = Classifier(type_, code, name, description)
        self.classifiers.append(classifier)
        type_.classifiers.append(classifier)

    def add_classifier_relationship(self, type_code, code, relationship_code, related_type_code, related_code, description, weight):
        classifier = self.get_classifier_by_code(type_code, code)
        related_classifier = self.get_classifier_by_code(related_type_code, related_code)
        r = ClassifierRelationship(classifier, relationship_code, related_classifier, description, weight)
        self.relationships.append(r)
        classifier.children.append(r)

    def get_classifier_type_by_code(self, type_code):
        for t in self.types:
            if t.code == type_code:
                return t
        else:
            raise KeyError(type_code)

    def __getitem__(self, type_code):
        return self.get_classifier_type_by_code(type_code)

    def get_classifier_by_code(self, type_code, code):
        type_ = self.get_classifier_type_by_code(type_code)
        return type_.get_classifier_by_code(code)


class ClassifierType(object):
    def __init__(self, code, name, description):
        self.code = code
        self.name = name
        self.description = description
        self.classifiers = list()

    def get_classifier_by_code(self, code):
        for c in self.classifiers:
            if c.code == code:
                return c
        else:
            raise KeyError(code)

    def __getitem__(self, code):
        return self.get_classifier_by_code(code)

    def __repr__(self):
        return "<ClassifierType: %s>" % self.code


class Classifier(object):
    def __init__(self, type_, code, name, description):
        self.type_ = type_
        self.code = code
        self.name = name
        self.description = description
        self.children = list()

    def __repr__(self):
        return "<Classifier: %s:%s>" % (self.type_.code, self.code)


class ClassifierRelationship(object):
    def __init__(self, classifier, relationship_code, related_classifier, description=None, weight=None):
        self.classifier = classifier
        self.relationship_code = relationship_code
        self.related_classifier = related_classifier


def load(connectionstring):
    classifier_collection = ClassifierCollection()
    conn = pyodbc.connect(connectionstring)
    cursor = conn.cursor()
    cursor.execute("SELECT ClassifierTypeCode, ClassifierTypeName, ClassifierTypeDescription FROM dbo.ClassifierType ORDER BY ClassifierTypeCode;")
    for row in cursor.fetchall():
        classifier_collection.types.append(ClassifierType(*row))

    cursor.execute("SELECT ClassifierTypeCode, ClassifierCode, ClassifierName, ClassifierDescription FROM dbo.Classifier;")
    for row in cursor.fetchall():
        classifier_collection.add_classifier(*row)


    cursor.execute("SELECT ClassifierTypeCode, ClassifierCode, RelationshipTypeCode, RelatedClassifierTypeCode, RelatedClassifierCode, [Description], [Weight] FROM dbo.ClassifierRelationship")
    for row in cursor.fetchall():
        classifier_collection.add_classifier_relationship(*row)

    return classifier_collection
