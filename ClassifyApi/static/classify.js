
class Classify {
  constructor() {
    this.apiRoot = "/api/types";
  }

  async addType(classifierType) {
    const options = {
      method: "POST",
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(classifierType)
    }
    fetch("api/types", options)
      .then(async function (response) {
        if (response.ok) {
        const dto = await response.json(); // The returned DTO, with links
        } else {
          let error = await response.json();
          console.log(error.Message);
        }
      })

  }

  getTypes() {
    fetch("api/types")
      .then(response => response.json())
      .then(typeCollection => {
        typeCollection.foreEach(t => )
      });
  }
}

class ClassifierType {
  constructor(dto) {
    
  }
}