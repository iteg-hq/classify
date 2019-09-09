

let selectedClassifierType;
let selectedClassifier;
let typeMap = new Map();
let classifierMap = new Map();
let relatedMap = new Map();

let ready = (callback) => {
  if (document.readyState != "loading") callback();
  else document.addEventListener("DOMContentLoaded", callback);
}


function startup() {
  // Event handlers
  document.getElementById("addType").addEventListener("submit", function (event) {
    event.preventDefault()
    addType()
  });

  document.getElementById("addClassifier").addEventListener("submit", function (event) {
    event.preventDefault()
    addClassifier()
  });

  // event handler for "hashchange"
  let frag = window.location.hash;
  let period = frag.search(frag);
  loadTypes();
};


function loadTypes() {
  // Build the type list
  typeMap.clear();
  fetch("api/types")
    .then(response => response.json())
    .then(typeCollection => {
      // Find the list element, then put cards in it
      var classifierList = document.getElementById("types");
      typeCollection.members.forEach(classifierType => {
        typeMap.set(classifierType.code, classifierType);
        card = makeTypeCard(classifierType);
        classifierList.append(card);
      })
    })
}


function makeCard(dto, cardClass, clickHandler) {
  let card = document.createElement('div');
  card.classList.add("card", cardClass);

  let cardBody = document.createElement('div');
  cardBody.classList.add("card-body");
  card.append(cardBody);

  let remove = document.createElement('img');
  remove.classList.add("small-button");
  remove.src = "static/delete.png"
  remove.addEventListener("click", event => deleteItem(event, dto, card));
  cardBody.append(remove);

  let cardLabel = document.createElement('span');
  cardLabel.classList.add("card-text");
  cardLabel.innerText = dto.code;
  cardLabel.addEventListener("click", event => clickHandler(event, dto, card));
  cardBody.append(cardLabel);

  dto.card = card;
  return card;
}

makeTypeCard = (classifierType) => makeCard(classifierType, 'classifier-type', selectType);
makeClassifierCard = (classifier) => makeCard(classifier, 'classifier', selectClassifier);

function addType() {
  var typeCode = document.getElementById("typeCode").value;
  if (!typeCode) return;
  const options = {
    method: "POST",
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ code: typeCode })
  }
  fetch("api/types", options)
    .then(async function (response) {
      if (response.ok) {
        let classifierType = await response.json();
        var classifierTypeList = document.getElementById("types");
        classifierTypeList.append(makeTypeCard(classifierType));
      } else {
        let error = await response.json();
        console.log(error.Message);
      }
    })
}

function addClassifier() {
  const classifierCode = document.getElementById("classifierCode").value;
  if (!classifierCode) return;
  const dto = {
    typeCode: selectedClassifierType.code,
    code: classifierCode
  }
  document.querySelectorAll(".active")
  const options = {
    method: "POST",
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(dto)
  }
  fetch(selectedClassifierType.addMemberURI, options)
    .then(async function (response) {
      if (response.ok) {
        const classifier = await response.json();
        const classifierList = document.getElementById("classifiers");
        classifierList.append(makeClassifierCard(classifier));
      } else {
        const error = await response.json();
        console.log(error.Message);
      }
    })
}


function deleteItem(event, dto, element) {
  element.parentNode.removeChild(element);
  options = {
    method: "DELETE",
    headers: {
      'Content-Type': 'application/json'
    }
  };
  fetch(dto.deleteURI, options)
    .then(response => {
      if (!response.ok)
      {
        console.log("Delete failed");
      }
    });
  event.stopPropagation();
}


function selectType(event, classifierType, card) {
  if (selectedClassifierType) {
    selectedClassifierType.card.classList.remove("active");
  }
  selectedClassifierType = classifierType;
  // Enable classifier creation
  document.getElementById("classifierCode").disabled = false;
  activeClassifierType = classifierType;
  card.classList.add("active");
  fetch(classifierType.getMembersURI)
    .then(response => response.json())
    .then(classifierCollection => {
      var classifierList = document.getElementById("classifiers");
      while (classifierList.childElementCount) { classifierList.removeChild(classifierList.firstChild); }
      classifierCollection.members.forEach(classifier => {
        classifierList.append(makeClassifierCard(classifier))
      })
    })
}

function selectClassifier(event, classifier, card) {
  console.dir(selectedClassifier);
  if (selectedClassifier) {
    selectedClassifier.card.classList.remove("active");
  }
  selectedClassifier = classifier;
  card.classList.add("active");
  fetch(classifier.getRelatedURI)
    .then(response => response.json())
    .then(relatedCollection => {
      var relatedList = document.getElementById("related");
      while (relatedList.childElementCount) { relatedList.removeChild(relatedList.firstChild); }
      relatedCollection.relatedClassifierSets.forEach(related => {
        //console.dir(related)

        let card = document.createElement('div');
        card.classList.add("card", 'related');

        let cardHeader = document.createElement('div');
        cardHeader.classList.add("card-header");

        let remove = document.createElement('img');
        remove.classList.add("small-button");
        remove.src = "static/delete.png"
        remove.addEventListener("click", event => deleteItem(event, related, card));

        let title = document.createElement('span');
        title.innerText = related.relationshipTypeCode;

        let list = document.createElement('ul');
        list.classList.add("list-group", "list-group-flush");

        related.relatedClassifiers.forEach(c => {
          let listItem = document.createElement('li');
          listItem.classList.add("list-group-item");
          listItem.innerHTML = `${c.code} <small>[${c.typeCode}]</small>`;
          list.append(listItem);
        })

        card.append(cardHeader);
        cardHeader.append(remove);
        cardHeader.append(title);
        card.append(list);

        relatedList.append(card)
      })
    })
}




ready(startup);
