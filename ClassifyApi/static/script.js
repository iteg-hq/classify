'use strict';


/*
function getValue() {
  var template = $('#getValue').html();
  var rendered = Mustache.render(template);
  let div = document.createElement("div");
  div.innerHTML = rendered;

  div.querySelector("button.btn-primary").onclick = () => alert("!");
  document.querySelector("#foo").appendChild(div);
  $("#getValueModal").modal("toggle")

}
*/

let app = null;
let typeMap = new Map();
let classifierMap = new Map();
let template = null;


function updateType(type) {
  $(".currentTypeCode").text(type.dto.code);
  $(".currentTypeName").text(type.dto.name);
  $(".currentTypeDescription").text(type.dto.description);
  document.getElementById("currentTypeCodeInput").value = type.dto.code;
  document.getElementById("currentTypeNameInput").value = type.dto.name;
  document.getElementById("currentTypeDescriptionInput").value = type.dto.description;
}

async function startup() {
  template = $('#template').html();
  Mustache.parse(template);

  app = new App("api/types");

  // Create DOM elements and attach event handlers to type object
  app.onAddType = function (type) {
    const classifierTypeList = document.getElementById("types");
    let item = makeTypeElement(type);

    // Find first element with greater contents
    const refElement = [...classifierTypeList.querySelectorAll(".classifier-type")].find(e => e.textContent.localeCompare(item.textContent, "en", { sensitivity: "base" }) == 1);
    if (!refElement) { classifierTypeList.appendChild(item); }
    else { classifierTypeList.insertBefore(item, refElement); }

    type.onSelect = function () {
      item.classList.add("active");
      // Cleanup
      let links = document.getElementById("classifierLinks");
      while (links.hasChildNodes()) links.removeChild(links.firstChild);
      let classifiers = document.getElementById("classifiers");
      while (classifiers.hasChildNodes()) classifiers.removeChild(classifiers.firstChild);
      type.onChange()
    };

    type.onDeselect = function () { item.classList.remove("active"); };
    type.onRemove = function () { classifierTypeList.removeChild(item); };
    type.onChange = function () {
      item.textContent = this.dto.name || this.dto.code;
      updateType(this)
    }
  }

  app.onAddClassifier = function (classifier) {
    console.log("onAddClassifier");
    let row = document.createElement("div");
    row.innerHTML = Mustache.render(template, { classifier: classifier, type: app.selectedType });
    document.getElementById("classifiers").appendChild(row);

    let link = document.createElement("li");
    link.innerHTML = `<a href="#${classifier.dto.code}"> ${classifier.dto.name || classifier.dto.code}</a>`;
    document.getElementById("classifierLinks").appendChild(link);

    classifier.onDelete = function () {
      row.parentElement.removeChild(row);
      link.parentElement.removeChild(link);
    };

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
  }

  app.loadTypes();

  window.onpopstate = event => { if (event.state) navigate(event.state.typeCode) };

  //await loadTypes();

  const urlParams = new URLSearchParams(window.location.search);
  //navigate(urlParams.get("TypeCode"));
}










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
function makeTypeElement(type) {
  let item = document.createElement("li");
  item.classList.add("list-group-item", "classifier-type", "font-weight-light");
  item.addEventListener("click", function () { app.selectType(type.dto.code); });
  item.textContent = type.dto.name || type.dto.code;
  return item;
}




((callback) => {
  if (document.readyState != "loading") callback();
  else document.addEventListener("DOMContentLoaded", callback);
})(startup);



