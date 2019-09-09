using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classify;

namespace ClassifyApi.Controllers
{
    public class ClassifierTypeController : ApiController
    {
        readonly DAL dal = new DAL(ConfigurationManager.ConnectionStrings["Classify"].ConnectionString);


        // GET api/types
        [Route("api/types")]
        public ClassifierTypeCollectionDto GetClassifierTypeCollection()
        {
            return new ClassifierTypeCollectionDto(dal.GetClassifierTypes());
        }

        // POST api/types
        [Route("api/types")]
        public IHttpActionResult PostClassifierType([FromBody] ClassifierTypeDto classifierTypeDto)
        {
            if (dal.TryGetClassifierType(classifierTypeDto.Code, out ClassifierType type))
            {
                return BadRequest($"A classifier type with code {type.Code} already exists.");
            }
            var classifierType = dal.SaveClassifierType(classifierTypeDto);
            return Created($"api/type/{classifierType.Code}", new ClassifierTypeDto(classifierType));
        }


        // GET api/type/{classifierTypeCode}
        [Route("api/type/{classifierTypeCode}")]
        public ClassifierTypeDto GetClassifierType(string classifierTypeCode)
        {
            return new ClassifierTypeDto(dal[classifierTypeCode]);
        }

        // PUT api/type/{classifierTypeCode}
        [Route("api/type/{classifierTypeCode}")]
        public IHttpActionResult PutClassifierType([FromBody] ClassifierTypeDto classifierType)
        {
            if (dal.TryGetClassifierType(classifierType.Code, out ClassifierType type))
            {
                dal.SaveClassifierType(classifierType);
                return Ok();
            }
            return Created($"api/type/{classifierType.Code}", classifierType);
        }

        // DELETE api/type/{classifierTypeCode}
        [Route("api/type/{classifierTypeCode}")]
        public IHttpActionResult DeleteClassifierType(string classifierTypeCode)
        {
            if (dal.GetClassifierTypes().Any(t => t.Code == classifierTypeCode))
            {
                dal.DeleteClassifierType(classifierTypeCode);
                return Ok();
            }
            else
            {
                return BadRequest("Classifier type not found");
            }
        }


        // GET api/type/{classifierTypeCode}/members
        [Route("api/type/{classifierTypeCode}/members")]
        public ClassifierCollectionDto GetClassifierTypeMembers(string classifierTypeCode)
        {
            return new ClassifierCollectionDto(dal[classifierTypeCode]);
        }


        // POST api/type/{classifierTypeCode}/members
        [Route("api/type/{classifierTypeCode}/members")]
        public IHttpActionResult PostClassifier(string classifierTypeCode, [FromBody] ClassifierDto classifierDto)
        {
            return Ok(dal[classifierTypeCode].AddMember(classifierDto));
        }

        // GET api/type/{classifierTypeCode}/member/{classifierCode}
        [Route("api/type/{classifierTypeCode}/member/{classifierCode}")]
        public ClassifierDto GetClassifier(string classifierTypeCode, string classifierCode)
        {
            return new ClassifierDto(dal[classifierTypeCode][classifierCode]);
        }

        // DELETE api/type/{classifierTypeCode}/member/{classifierCode}
        [Route("api/type/{classifierTypeCode}/member/{classifierCode}")]
        public bool DeleteClassifier(string classifierTypeCode, string classifierCode)
        {
            return dal[classifierTypeCode].DeleteMember(classifierCode);
        }

        // GET api/type/{classifierTypeCode}/member/{classifierCode}/related
        [Route("api/type/{classifierTypeCode}/member/{classifierCode}/related")]
        public RelatedClassifiersDto GetClassifierRelationshipTypes(string classifierTypeCode, string classifierCode)
        {
            Dictionary<string, RelatedClassifierSetDto> typeMap = new Dictionary<string, RelatedClassifierSetDto>();
            foreach (var r in dal[classifierTypeCode][classifierCode].GetRelated())
            {
                if (!typeMap.Keys.Contains(r.RelationshipTypeCode))
                {
                    typeMap[r.RelationshipTypeCode] = new RelatedClassifierSetDto { RelationshipTypeCode = r.RelationshipTypeCode };
                }
                typeMap[r.RelationshipTypeCode].RelatedClassifiers.Add(new RelatedClassifierDto
                {
                    TypeCode = r.RelatedClassifier.TypeCode,
                    Code = r.RelatedClassifier.Code,
                    Description = r.Description,
                    Weight = r.Weight
                });
            }
            return new RelatedClassifiersDto()
            {
                ClassifierTypeCode = classifierTypeCode,
                ClassifierCode = classifierCode,
                RelatedClassifierSets = typeMap.Values.ToList()
            };
        }
    }
}
