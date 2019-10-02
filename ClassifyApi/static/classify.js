function _strcmp(a, b) {
  if (a > b) return 1;
  if (a < b) return -1;
  return 0;
}

class App {
  constructor(rootURI) {
    this.rootURI = rootURI;
    this.types = new Map();
    this.selectedType = null;

    this.classifiers = new Map();
    this.selectedClassifier = null;

    this.newTypeCode = null;
    this.newTypeName = null;
    this.newTypeDescription = null;

    this.newClassifierCode = null;
    this.newClassifierName = null;
    this.newClassifierDescription = null;

    this.newRelationshipRelationshipTypeCode = null;
    this.newRelationshipRelatedClassifierTypeCode = null;
    this.newRelationshipRelatedClassifierCode = null;

    this.onAddType = (type) => { };
    this.onAddClassifier = (classifier) => { };
  }

  getTypes() {
    return fetch(this.rootURI)
      .then(response => response.json())
      .then(dtos => dtos.map(dto => { return { dto: dto } }));
  }

  // Load types from API and add them to the navigation list
  loadTypes() {
    this.getTypes()
      .then(types => types.forEach(t => this.addType(t))) // map(method) sets "this" of method equal to undefined
    //.then(() => this.selectType("Bordereau"))
  }

  // Add a type to the navigation list
  addType(type) {
    this.types.set(type.dto.code, type);
    this.onAddType(type);
  }

  deleteType(typeCode) {
    let index = this.types.findIndex(t => t.code > typeCode)
    this.types.splice(index, 1);
    this.onDeleteType(this.types[index]);
  }


  selectType(typeCode, forceRefresh = false) {
    let newType = this.types.get(typeCode);
    if (!forceRefresh && this.selectedType && newType.dto.code == this.selectedType.dto.code) return;
    if (this.selectedType) this.selectedType.onDeselect();
    newType.onSelect();
    this.selectedType = newType;

    this.classifiers.clear();

    return this.getClassifiers(this.selectedType)
      .then(classifiers => {
        let n = 0;
        classifiers.forEach(
          classifier => {
            n += 1;
            classifier.showWeight = (classifier.dto.weight < 100);
            this.addClassifier(classifier);
          }
        );
        return n;
      })
  }

  setTypeName(name) {
    this.selectedType.dto.name = name;
    this.saveSelectedType();
  }

  setTypeDescription(description) {
    this.selectedType.dto.description = description;
    this.saveSelectedType();
  }

  setCurrentClassifierName(name) {
    this.selectedClassifier.dto.name = name;
    this.saveSelectedClassifier(this.selectedClassifier);
  }

  setCurrentClassifierDescription(description) {
    this.selectedClassifier.dto.description = description;
    this.saveSelectedClassifier(this.selectedClassifier);
  }


  saveSelectedType() {
    let type = this.selectedType;
    const options = {
      method: "PUT",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(type.dto)
    }
    return fetch(type.dto.changeURI, options)
      .then(response => response.json())
      .then(dto => {
        type.dto = dto;
        type.onChange();
      });
  }

  createType() {
    const options = {
      method: "POST",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        code: this.newTypeCode,
        name: this.newTypeName,
        description: this.newTypeDescription
      })
    }
    return fetch("api/types", options)
      .then(response => {
        if (response.ok) {
          response.json()
            .then(dto => {
              let type = { dto: dto }
              this.addType(type);
              this.selectType(type.dto.code);
              return type;
            })
        } else {
          console.log(response.Message);
        }
      })
  }
  /*
  saveType(type, method) {
    const options = {
      method: method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(type.dto)
    }
    return fetch(type.dto.changeURI, options)
      .then(response => {
        if (response.ok) {
          response.json().then(dto => { type.dto = dto })
          return type;
        } else {
          console.dir(response);
        }
      })
    // TODO: Check response
  }
  */
  deleteClassifier() {
    this.onDelete
  }

  deleteSelectedType() {
    const options = {
      method: "DELETE",
      headers: { 'Content-Type': 'application/json' }
    }
    return fetch(this.selectedType.dto.deleteURI, options)
      .then(response => {
        this.selectedType.onRemove();
        this.types.delete(this.selectedType.dto.code);
        this.selectedType = null;
      })
  }

  createClassifier() {
    const options = {
      method: "POST",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        code: this.newClassifierCode,
        name: this.newClassifierName || this.newClassifierCode,
        description: this.newClassifierDescription || "",
        typeCode: this.selectedType.dto.code
      })
    }
    return fetch(this.selectedType.dto.addMemberURI, options)
      .then(response => {
        if (response.ok) {
          response.json()
            .then(dto => {
              let classifier = { dto: dto };
              this.addClassifier(classifier);
              return classifier;
            })
        } else {
          console.log(response.Message);
        }
      })
  }




  getClassifiers(type) {
    return fetch(type.dto.getMembersURI)
      .then(response => response.json())
      .then(classifiers => classifiers.map(c => { return { dto: c } }))
  }


  changeClassifier(classifier) {
    const options = {
      method: "POST",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(this.selectedType.addMemberURI)
    }
    return fetch(classifier.dto.changeURI, options)
      .then(response => response.json())
      .then(dto => {
        classifier.dto = dto;
        classifier.onChange();
      });
  }

  editClassifier(classifierCode) {
    this.selectedClassifier = this.classifiers.get(classifierCode);
    this.selectedClassifier.onEdit()
  }

  saveSelectedClassifier() {
    let classifier = this.selectedClassifier;
    const options = {
      method: "PUT",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(classifier.dto)
    }
    return fetch(classifier.dto.changeURI, options)
      .then(response => response.json())
      .then(dto => {
        classifier.dto = dto;
        classifier.onChange();
        return classifier;
      });
  }

  deleteClassifier(code) {
    let classifier = this.classifiers.get(code);
    fetch(classifier.dto.deleteURI, { method: "DELETE" })
      .then(response => {
        if (response.ok) {
          classifier.onDelete();
        } else {
          console.log(response.Message);
        }
      })
  }


  addClassifier(classifier) {
    this.classifiers.set(classifier.dto.code, classifier);
    this.onAddClassifier(classifier);
    return classifier;
  }

  createRelationship() {
    const options = {
      method: "POST",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        classifier: {
          typeCode: this.selectedType.dto.code,
          code: this.selectedClassifier.dto.code
        },
        relationshipTypeCode: this.newRelationshipRelationshipTypeCode,
        relatedClassifier: {
          typeCode: this.newRelationshipRelatedClassifierTypeCode,
          code: this.newRelationshipRelatedClassifierCode
        }
      })
    }

    return fetch("api/relationships", options)
      .then(response => response.json())
      .then(dto => this.selectType(this.selectedType.dto.code, true));

  }

  addRelationship(classifierCode) {
    this.selectedClassifier = this.classifiers.get(classifierCode);
    this.selectedClassifier.getRelationship(
      this.getRelationshipTypes.bind(this),
      (rtype) => { this.newRelationshipRelationshipTypeCode = rtype; },
      this.getTypes.bind(this),
      (type) => { this.newRelationshipRelatedClassifierTypeCode = type; return this.getClassifiers(this.types.get(type)); },
      (code) => { this.newRelationshipRelatedClassifierCode = code; },
      this.createRelationship.bind(this),
    );
  }

  getRelationshipTypes() {
    return fetch("/api/relationshiptypes").then(response => response.json());
  }

  deleteRelationship(classifierCode, relationshipTypeCode, typeCode, code) {
    let classifier = this.classifiers.get(classifierCode);
    let relationshipCollection = classifier.dto.relationships.find(r => r.relationshipTypeCode == relationshipTypeCode);
    let relationship = relationshipCollection.relationships.find(r => r.relatedClassifier.typeCode == typeCode && r.relatedClassifier.code == code);
    document.querySelector("li[]")
    //return fetch(relationship.deleteURI, {"method": "DELETE"});
  }
}
