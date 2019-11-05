using Classify;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace ClassifyApi.Controllers
{


    public class ClassifierTypeController : ApiController
    {
        private readonly DAL dal = new DAL(ConfigurationManager.ConnectionStrings["Classify"].ConnectionString);

        // GET api/types
        [Route("api/types")]
        public IEnumerable<ClassifierTypeDto> GetClassifierTypes()
        {
            return dal.GetClassifierTypes().Select(t => new ClassifierTypeDto(t)).OrderBy(c => c.Code);
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
            return Created(classifierTypeDto.URI, new ClassifierTypeDto(classifierType));
        }

        // PUT api/type/{classifierTypeCode}
        [Route("api/types")]
        public ClassifierTypeDto PutClassifierType([FromBody] ClassifierTypeDto classifierType)
        {
            if (dal.TryGetClassifierType(classifierType.Code, out ClassifierType type))
            {
                return new ClassifierTypeDto(dal.SaveClassifierType(classifierType));
            }
            return null;
        }

        // DELETE api/types
        [Route("api/types")]
        public IHttpActionResult DeleteClassifierType([FromUri] ClassifierTypeQuery query)
        {
            if (dal.GetClassifierTypes().Any(t => t.Code == query.ClassifierTypeCode))
            {
                dal.DeleteClassifierType(query.ClassifierTypeCode);
                return Ok();
            }
            else
            {
                return BadRequest("Classifier type not found");
            }
        }


        // GET api/type/{classifierTypeCode}/members
        [Route("api/classifiers")]
        public IEnumerable<ClassifierDto> GetClassifiers([FromUri] ClassifierTypeQuery query)
        {
            return dal[query.ClassifierTypeCode].GetMembers().Select(c => new ClassifierDto(c)).OrderBy(c => c.Code);
        }


        // POST api/classifiers
        [Route("api/classifiers")]
        public ClassifierDto PostClassifier([FromBody] ClassifierDto classifierDto)
        {
            return new ClassifierDto(dal[classifierDto.TypeCode].AddMember(classifierDto));
        }

        [Route("api/classifiers")]
        public ClassifierDto PutClassifier([FromBody] ClassifierDto classifierDto)
        {
            return new ClassifierDto(dal.SaveClassifier(classifierDto));
        }

        // DELETE api/type/{classifierTypeCode}/member/{classifierCode}
        [Route("api/classifiers")]
        public bool DeleteClassifier([FromUri] ClassifierQuery query)
        {
            return dal[query.ClassifierTypeCode].DeleteMember(query.ClassifierCode);
        }


        [Route("api/relationships")]
        public IEnumerable<ClassifierRelationshipDto> GetClassifierRelationships([FromUri] ClassifierQuery query)
        {
            return dal[query.ClassifierTypeCode][query.ClassifierCode].GetRelated().Select(r => new ClassifierRelationshipDto(r));
        }

        [Route("api/relationships")]
        public ClassifierRelationshipDto PostClassifierRelationship([FromBody] ClassifierRelationshipDto relationship)
        {
            dal[relationship.Classifier.TypeCode][relationship.Classifier.Code].AddRelated(relationship.RelationshipTypeCode, relationship.RelatedClassifier, weight: relationship.Weight);
            return relationship;
        }

        [Route("api/relationships")]
        public IHttpActionResult DeleteClassifierRelationship([FromUri] ClassifierRelationshipQuery q)
        {
            dal.DeleteClassifierRelationship(
                q.ClassifierTypeCode,
                q.ClassifierCode,
                q.RelationshipTypeCode,
                q.RelatedClassifierTypeCode,
                q.RelatedClassifierCode
                );
            return Ok();
        }

        [Route("api/relationshiptypes")]
        public IEnumerable<string> GetClassifierRelationshipTypes()
        {
            return dal.GetClassifierRelationshipTypes();
        }

    }
}
