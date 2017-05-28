using CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LyricsConcord
{
    public partial class Relations : System.Web.UI.Page
    {
        protected RelationTypesResponse relTypes;
        protected RelationAllResponse relations;
        protected GetWordsResponse words;

        protected void Page_Load(object sender, EventArgs e)
        {
            relations = ServiceAccessor.MakeRequest<RelationAllRequest, RelationAllResponse>(new RelationAllRequest(), "RelationAll");
            relTypes = ServiceAccessor.MakeRequest<RelationTypesRequest, RelationTypesResponse>(new RelationTypesRequest(), "RelationTypes");
            words = ServiceAccessor.MakeRequest<GetWordsRequest, GetWordsResponse>(new GetWordsRequest(), "GetWords");
        }

        [WebMethod]
        public static object AddRelation(Guid type, Guid word1, Guid word2)
        {
            return ServiceAccessor.MakeRequest<RelationAddRequest, RelationAddResponse>(new RelationAddRequest() { RelationType = type, Word1 = word1, Word2 = word2 }, "RelationAdd").Id;
        }

        [WebMethod]
        public static object DeleteRelation(Guid id)
        {
            return ServiceAccessor.MakeRequest<RelationDeleteRequest, RelationDeleteResponse>(new RelationDeleteRequest() { Id = id }, "RelationDelete").Success;
        }
    }
}