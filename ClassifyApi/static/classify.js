function _strcmp(a, b) {
  if (a > b) return 1;
  if (a < b) return -1;
  return 0;
}

class Bind {
  constructor() {
    this.scope = new Map();
  }

  get(key) {
    return this.scope.get(key);
  }

  set(key, value) {
    this.scope.set(key, value);
    console.log("set");
    document.querySelectorAll(`[data-bind=${key}]`).forEach(element => {
      if (element.type && (element.type === "text" || element.type === "textarea")) element.value = value;
      else element.textContent = value;
    });
  }
}



class App {
  constructor(rootURI) {
    this.rootURI = rootURI;
    this.typeCache = new Map();
    this.classifierCache = new Map();
    this.selectedType = null;
    this.selectedClassifier = null;
    this.values = new Bind();
  }

  getTypes(callback) {
    return fetch(this.rootURI)
      .then(response => response.json())
      .then(dtos => dtos.map(dto => {
        this.typeCache.set(dto.code, dto);
        callback(dto.code, dto.name);
      }));
  }

  selectType(typeCode) {
    // If the type is already selected, return
    if (this.selectedType && typeCode == this.selectedType.code) return;
    // If some other type is already selected, deselect it
    if (this.selectedType) this.onDeselectType(this.selectedType.code);
    // Select a new type
    this.selectedType = this.typeCache.get(typeCode);

    this.values.set("typeCode", this.selectedType.code);
    this.values.set("typeName", this.selectedType.name);
    this.values.set("typeDescription", this.selectedType.description);

    this.onSelectType(typeCode, this.selectedType.name, this.selectedType.description);
    // Clear classifier class
    this.classifierCache.clear();
    this.onHideClassifiers();
    // Get, cache, and show classifiers for the new type
    return this.getClassifiers(typeCode)
      .then(classifiers => {
        classifiers.forEach(
          classifier => {
            classifier.showWeight = (classifier.weight < 100);
            this.classifierCache.set(classifier.code, classifier);
            this.onShowClassifier(classifier.code, classifier.name, classifier.description);
          }
        );
      })
  }



  deleteType(typeCode) {
    let type = this.typeCache.get(typeCode);
    option = { "method": "DELETE" };
    fetch(type.deleteURI, options).then(
      this.typeCache.delete(typeCode)
    );
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
              this.selectType(code);
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
            .then(classifier => {
              this.classifiers.set(classifier.code, classifier);
              this.onShowClassifier(code, name, description);
              return classifier;
            })
        } else {
          console.log(response.Message);
        }
      })
  }

  getClassifiers(typeCode) {
    const type = this.typeCache.get(typeCode);
    return fetch(type.getMembersURI)
      .then(response => response.json())
    //.then()
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
