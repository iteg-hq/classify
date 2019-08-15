from flask import render_template
from app import app, classify

CONNECTION_STRING = "Driver={SQL Server Native Client 11.0};Server=localhost;Database=Classify;Trusted_Connection=yes;"


@app.route('/')
def index():
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=None, classifier_code=None)

@app.route('/<type_code>')
def classifier_type(type_code):
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=type_code, classifier_code=None)

@app.route('/<type_code>/<classifier_code>')
def classifier(type_code, classifier_code):
    classifier_collection = classify.load(CONNECTION_STRING)
    return render_template('index.html', classifier_collection=classifier_collection, type_code=type_code, classifier_code=classifier_code)

