from flask import render_template, request, Response
from app import app, classify

CONNECTION_STRING = "Driver={SQL Server Native Client 11.0};Server=localhost;Database=Classify;Trusted_Connection=yes;"


@app.route('/')
def index():
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=None, classifier_code=None)

@app.route('/api/download')
def download():
    classifier_collection = classify.load(CONNECTION_STRING)
    nl = "\r\n"
    output = "classifier_type_code;classifier_code;classifier_relationship_type_code;related_classifier_type_code;related_classifier_code" + nl
    for type_ in classifier_collection.types:
        output += type_.code + nl
        for classifier in type_.classifiers:
            output += ";".join((type_.code, classifier.code)) + nl
            for relationship in classifier.children:
                output += ";".join((type_.code, classifier.code, relationship.relationship_code, relationship.related_classifier.type_.code, relationship.related_classifier.code)) + nl
    return Response(output, mimetype='text/plain')

@app.route('/types/<type_code>')
def classifier_type(type_code):
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=type_code, classifier_code=None)

@app.route('/types/<type_code>/c/<classifier_code>')
def classifier(type_code, classifier_code):
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=type_code, classifier_code=classifier_code)

