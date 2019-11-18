'use strict';

let app;

Vue.component("newClassifier", {
  props: ["id", "title", "classifier"],
  template: `
    <div class="modal fade" :id="id" role="dialog">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title">{{ title }}</h4>
            <button type="button" class="close" data-dismiss="modal">&times;</button>
          </div>
          <div class="modal-body">
            <form>
              <div class="form-group">
                <input type="text" class="form-control" placeholder="Code" v-model="classifier.code">
              </div>
              <div class="form-group">
                <input type="text" class="form-control" placeholder="Name" v-model="classifier.name">
              </div>
              <div class="form-group">
                <input type="text" class="form-control" placeholder="Description" v-model="classifier.description">
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary" @click="$emit('save')">Save</button>
          </div>
        </div>
      </div>
    </div>
`
})

/*
Vue.component("editable", {
    props: ["value", "attr"],
    template: `
    <span @click="edit">
      {{ this.value[this.attr] }}
    </span>
    `,
    methods: {
        edit: function() {
            let newValue = prompt("Edit value", this.value[this.attr]);
            if (!newValue) return;
            this.value[this.attr] = newValue;
            this.$emit("change");
        }
    }
});
*/
async function startup() {
  app = new Vue({
    el: '#root',
    data: function () {
      return {
        types: [],
        selectedTypeCode: null,
        selectedClassifierCode: null,
        newType: {
          code: "",
          name: "",
          description: ""
        },
        newClassifier: {
          code: "",
          name: "",
          description: ""
        },
        newRelationshipType: {
          code: "",
          name: "",
          description: ""
        },
        newRelationship: {
          classifier: { typeCode: "", code: "" },
          relationshipTypeCode: "",
          relatedClassifier: { typeCode: "", code: "" }
        }
      };
    },
    computed: {
      relationshipTypes: function () {
        let relationshipTypeType = this.types.find(t => t.code == "ClassifierRelationshipType");
        if (!relationshipTypeType) return [];
        return relationshipTypeType.members.map(c => c.code);
      },
      relatedClassifiers: function () {
        let relatedType = this.types.find(t => t.code == this.newRelationship.relatedClassifier.typeCode);
        if (!relatedType) return [];
        return relatedType.members;
      },
      selectedType: function () {
        return this.types.find(t => t.code == this.selectedTypeCode)
      },
      selectedClassifier: function () {
        let type = this.selectedType;
        if (!type) return null;
        return type.members.find(c => c.code == this.selectedClassifierCode);
      }
    },
    created: function () {
      window.addEventListener("hashchange", this.onHashChange);
      fetch("api/types")
        .then(response => response.json())
        .then(types => types.forEach(t => this.types.push(t)))
        .then(() => this.onHashChange())
        ;
    },
    methods: {
      createType: function () {
        if (this.newType.code == "") return;
        if (this.newType.name == "") this.newType.name = this.newType.code;
        const options = {
          method: "POST",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.newType)
        }
        return fetch("api/types", options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(type => {
                  this.types.push(type);
                  this.types.sort();
                  this.onNavigate(type.code);
                })
            } else {
              console.log(response);
            }
          })
      },
      /*
      createRelationshipType: function () {
        if (this.newRelationshipType.code == "") return;
        if (this.newRelationshipType.name == "") this.newRelationshipType.name = this.newRelationshipType.code;
        const options = {
          method: "POST",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.newType)
        }
        return fetch("api/relationshiptypes", options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(type => {
                  this.types.push(type);
                  this.types.sort();
                  this.onNavigate(type.code);
                })
            } else {
              console.log(response);
            }
          })
      },
      */
      saveType: function () {
        if (!this.selectedType) return;
        const options = {
          method: "PUT",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.selectedType)
        }
        return fetch("api/types", options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(type => {
                  this.types.splice(this.types.indexOf(this.selectedType), 1, type);
                })
            } else {
              console.log(response);
            }
          })
      },
      deleteType: function () {
        const options = { method: "DELETE" }
        fetch(this.selectedType.deleteURI, options)
          .then(response => {
            if (response.ok) {
              this.types.splice(this.types.indexOf(this.selectedType), 1);
              this.selectedTypeCode = null;
              console.log("deleted")
            } else {
              console.dir(response);
            }
          })
      },
      createClassifier: function () {
        console.dir(this.newClassifier);
        if (this.newClassifier.code == "") return;
        if (this.newClassifier.name == "") this.newClassifier.name = this.newClassifier.code;
        this.newClassifier.typeCode = this.selectedTypeCode;
        const options = {
          method: "POST",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.newClassifier)
        }
        return fetch(this.selectedType.addMemberURI, options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(classifier => {
                  this.selectedType.members.push(classifier);
                  this.selectedType.members.sort();
                  this.onNavigate(this.selectedTypeCode, classifier.code);
                })
            } else {
              console.log(response);
            }
          })
      },
      saveClassifier: function () {
        if (!this.selectedClassifier) return;
        const options = {
          method: "POST",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.selectedClassifier)
        }
        return fetch(this.selectedType.addMemberURI, options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(classifier => {
                  this.selectedType.members.splice(this.selectedType.members.indexOf(this.selectedClassifier), 1, classifier);
                })
            } else {
              console.log(response);
            }
          })
      },
      deleteClassifier: function () {
        const options = { method: "DELETE" }
        fetch(this.selectedClassifier.deleteURI, options)
          .then(response => {
            if (response.ok) {
              this.selectedType.members.splice(this.selectedType.members.indexOf(this.selectedClassifier), 1);
              this.selectedClassifierCode = null;
              console.log("deleted classifier")
            } else {
              console.dir(response);
            }
          })
      },
      createRelationship: function () {
        if (this.newRelationship.code == "") return;
        this.newRelationship.classifier.typeCode = this.selectedType.code;
        this.newRelationship.classifier.code = this.selectedClassifier.code;
        const options = {
          method: "POST",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.newRelationship)
        }
        console.dir(this.newRelationship);
        return fetch(this.selectedClassifier.relatedURI, options)
          .then(response => {
            if (response.ok) {
              response.json()
                .then(relationship => {
                  this.selectedClassifier.relationships.push(relationship);
                })
            } else {
              console.log(response);
            }
          })
      },

      deleteRelationship: function (relationship) {
        const options = {
          method: "DELETE",
          headers: { 'Content-Type': 'application/json', },
          body: JSON.stringify(this.newRelationship)
        }
        this.selectedClassifier.relationships.splice(this.selectedClassifier.relationships.indexOf(relationship), 1);
        fetch(relationship.deleteURI, options);
      },
      onHashChange: function () {
        if (!location.hash) return;
        let m = location.hash.match(/#\/?([^/]+)?(?:\/([^/]+))?/);
        if (!m) {
          console.log(location.hash);
          console.log("Invalid hash, need '#/?typecode(/code)?'");
          return;
        }
        let typeCode = decodeURIComponent(m[1]);
        if (!typeCode) return;
        this.selectedTypeCode = typeCode;
        let classifierCode = decodeURIComponent(m[2]);
        if (classifierCode) {
          this.selectedClassifierCode = classifierCode;
        }
      },
      onNavigate: function (typeCode, classifierCode) {
        location.hash = `/${typeCode}/${classifierCode || ""}`
      }
    }
  })

  return;

}

((callback) => {
  if (document.readyState != "loading") callback();
  else document.addEventListener("DOMContentLoaded", callback);
})(startup);



