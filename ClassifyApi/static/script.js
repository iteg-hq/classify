'use strict';


Vue.component('type', {
  props: ["type", "selectedTypeCode", "selectedClassifierCode"],
  data: function () { return { classifiers: [] } },
  methods: {
    onNavigate: function (typeCode, classifierCode) {
      location.hash = `/${typeCode}/${classifierCode || ""}`
    }
  },
  template: `
    <li>
      <span @click="onNavigate(type.code, null)">
        {{ type.name == type.code ? type.code : type.name + " (" + type.code + ")" }}
      </span>
      <ul v-if="type.code == selectedTypeCode">
        <classifier v-for="classifier in type.members" 
                    :type="type"
                    :classifier="classifier"
                    :selectedClassifierCode="selectedClassifierCode"
                    :key="classifier.code"
                    @navigate="onNavigate"
        >
        </classifier>
      </ul>
    </li>
  `
})

Vue.component('classifier', {
  props: ["type", "classifier", "selectedClassifierCode"],
  data: function () { return { isExpanded: false } },
  template: `
    <li :class="{active: classifier.code == selectedClassifierCode}" @click="$emit('navigate', type.code, classifier.code)">
      <span>
        {{ classifier.name == classifier.code ? classifier.code : classifier.name + " (" + classifier.code + ")" }}
      </span>
    </li>
  `
})


let app2;

Vue.component("editable", {
  props: ["value"],
  data: function () { return { "edit": false } },
  template: `
    <span>
      <span v-if="!edit">{{ value }}</span>
      <input v-if="edit" type="text" v-bind:value="value" @change="$emit('change', this.value)"></input>
      <small @click="edit=!edit">[edit]</small>
    </span>
  `
  //template: "<div style='border: solid 1px black'>{{ value }}</div>"
});


Vue.component('full-type', {
  props: ["type"],
  template: `
    <div class="row" v-if="type">
      <div class="col">
        <h1><editable v-bind:value="type.name" /></h1>
        <p>Code: {{ type.code }}</p>        
        <p>Description: <editable v-model:value="type.description"></editable></p>        
      </div>
    </div>
    `,
  methods: {
    onChangeDescription(value) {
      console.log(value);
    }
  }
});


Vue.component('full-classifier', {
  props: ["classifier"],
  template: `
    <div class="row">
      <div class="col">
        <h2>{{ classifier.name }}</h2>
        <p>Code: {{ classifier.code }}</p>
        <p>Description: {{ classifier.description || "No description" }}</p>
        <ul v-if="classifier.relationships">
          <li v-if="relationshipType.relationships.length > 1" v-for="relationshipType in classifier.relationships">
            {{ relationshipType.relationshipTypeCode }}
            <ul>
              <li v-for="related in relationshipType.relationships">
                {{ related.relatedClassifier.typeCode }}.{{ related.relatedClassifier.code }}
                <span v-if="related.weight < 100.0">{{ related.weight }}</span>
              </li>
            </ul>
          </li>
          <li v-else>
            [{{ relationshipType.relationships[0].relationshipTypeCode }}]
            <a :href="'#' + relationshipType.relationships[0].relatedClassifier.typeCode + '/' + relationshipType.relationships[0].relatedClassifier.code ">
              {{ relationshipType.relationships[0].relatedClassifier.typeCode }}.{{ relationshipType.relationships[0].relatedClassifier.code }}
            </a>
          </li>
        </ul>
      </div>
    </div>
    `
});


async function startup() {
  app2 = new Vue({
    el: '#root',
    data: function () {
      return {
        types: [],
        selectedTypeCode: null,
        selectedClassifierCode: null
      };
    },
    computed: {
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
        ;
    },
    methods: {
      onHashChange: function () {
        let m = location.hash.match(/#\/?([^/]+)(?:\/([^/]+))?/);
        if (!m) {
          console.log(location.hash);
          console.log("Invalid hash, need '#/?typecode(/code)?'");
          return;
        }
        let typeCode = decodeURIComponent(m[1]);
        this.selectedTypeCode = typeCode;
        let classifierCode = decodeURIComponent(m[2]);
        if (classifierCode) {
          this.selectedClassifierCode = classifierCode;
        }
      },
    }
  })

  return;

  template = $('#template').html();
  Mustache.parse(template);

  app = new App("api/types");
  app.onSelectType = onSelectType;
  app.onDeselectType = onDeselectType;
  app.onHideClassifiers = onHideClassifiers;

  // Create DOM elements and attach event handlers to type object
  app.onAddType = function (code, name) {
    let type = null;
    const classifierTypeList = document.getElementById("types");
    let item = makeTypeElement(code, name);

    // Find first element with greater contents
    const refElement = [...classifierTypeList.querySelectorAll("[data-typecode]")].find(e => e.textContent.localeCompare(item.textContent, "en", { sensitivity: "base" }) == 1);
    if (!refElement) { classifierTypeList.appendChild(item); }
    else { classifierTypeList.insertBefore(item, refElement); }
  }

  app.onShowClassifier = onShowClassifier;
  /*
      function foo() {
  
        classifier.onChange = function () {
          $(".name", row).text(this.dto.name);
          $(".description", row).text(this.dto.description);
        }
  
        classifier.onEdit = function () {
          document.getElementById("editClassifierModal-code").value = this.dto.code;
          document.getElementById("editClassifierModal-name").value = this.dto.name;
          document.getElementById("editClassifierModal-description").value = this.dto.description;
          $("#editClassifierModal").modal("show");
        }
  
        classifier.getRelationship = function (
          getRelationshipTypeCodes,
          setRelationshipTypeCode,
          getTypeCodes,
          setTypeCode,
          setClassifierCode,
          saveRelationship
        ) {
  
          let selectRelationshipType = document.getElementById("selectRelationshipType");
          $(selectRelationshipType).empty();
  
          let opt1 = document.createElement("option");
          opt1.value = "";
          opt1.text = "Select a relationship type...";
          selectRelationshipType.add(opt1)
  
          selectRelationshipType.addEventListener("change", function () { setRelationshipTypeCode(this.value); });
          getRelationshipTypeCodes()
            .then(rtypes => rtypes.forEach(t => {
              let opt = document.createElement("option");
              opt.value = t;
              opt.text = t;
              selectRelationshipType.add(opt);
            }))
  
          let selectType = document.getElementById("selectType");
          $(selectType).empty();
  
          let opt2 = document.createElement("option");
          opt2.value = "";
          opt2.text = "Select a classifier type...";
          selectType.add(opt2)
  
          getTypeCodes()
            .then(types => types.forEach(t => {
              let opt = document.createElement("option");
              opt.value = t.dto.code;
              opt.text = `${t.dto.name} (${t.dto.code})`;
              selectType.add(opt);
            }))
  
          let selectClassifier = document.getElementById("selectClassifier");
          selectClassifier.addEventListener("change", function () { setClassifierCode(this.value); });
          selectType.addEventListener("change", function () {
            $(selectClassifier).empty();
            let opt3 = document.createElement("option");
            opt3.value = "";
            opt3.text = "Select a classifier...";
            selectClassifier.add(opt3)
  
            setTypeCode(this.value)
              .then(classifiers => classifiers.forEach(c => {
                let opt = document.createElement("option");
                opt.value = c.dto.code;
                opt.text = `${c.dto.name} (${c.dto.code})`;
                selectClassifier.add(opt);
              }))
          });
  
          $("#saveClassifierRelationshipButton").click(saveRelationship);
  
          $("#addRelationshipModal").modal("show");
}
        */

  app.getTypes(function (code, name) {
    const classifierTypeList = document.getElementById("types");
    let item = makeTypeElement(code, name);

    // Find first element with greater contents
    const refElement = [...classifierTypeList.querySelectorAll(".classifier-type")].find(e => e.textContent.localeCompare(item.textContent, "en", { sensitivity: "base" }) == 1);
    if (!refElement) { classifierTypeList.appendChild(item); }
    else { classifierTypeList.insertBefore(item, refElement); }
  });
}
window.onpopstate = event => { if (event.state) navigate(event.state.typeCode) };

//await loadTypes();

const urlParams = new URLSearchParams(window.location.search);
//navigate(urlParams.get("TypeCode"));

async function launchAddRelationshipModal(typeCode, classifierCode) {
  let response = await fetch("/api/relationshiptypes");
  let relationshipTypes = await response.json();
  relationshipTypes.forEach(t => {
    let opt = document.createElement("option")
    opt.value = t;
    opt.text = t;
    document.getElementById("selectRelationshipType").add(opt);
  });

  response = await fetch("/api/types");
  let types = await response.json();
  types.forEach(t => {
    let opt = document.createElement("option")
    opt.value = t.code;
    opt.text = t.name;
    document.getElementById("selectType").add(opt);
  });

  $("#addRelationshipModal").modal("show");

  $("#saveClassifierRelationshipButton").click(() => {
    addRelationship({
      classifier: {
        typeCode: typeCode,
        code: classifierCode
      },
      relationshipTypeCode: document.getElementById("selectRelationshipType").value,
      relatedClassifier: {
        typeCode: document.getElementById("selectType").value,
        code: document.getElementById("selectClassifier").value
      }
    })
  })
}

async function navigate(typeCode) {
  if (!typeCode) return;
  const element = getTypeElement(typeCode);
  if (!element) {
    document.getElementById("main").innerHTML = `<br/><div class="alert alert-warning" role="alert">Classifier type ${typeCode} not found</div>`;
    return;
  }
  showType(element);
}

// Wrap a classifier type DTO in a DOM element
function makeTypeElement(code, name) {
  let item = document.createElement("li");
  item.classList.add("list-group-item", "classifier-type", "font-weight-light");
  item.setAttribute("data-typecode", code);
  item.addEventListener("click", function () { app.selectType(code); });
  item.textContent = name;
  return item;
}

((callback) => {
  if (document.readyState != "loading") callback();
  else document.addEventListener("DOMContentLoaded", callback);
})(startup);



