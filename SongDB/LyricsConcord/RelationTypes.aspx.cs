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
    public partial class RelationTypes : System.Web.UI.Page
    {
        protected RelationTypesResponse types;

        protected void Page_Load(object sender, EventArgs e)
        {
            types = ServiceAccessor.MakeRequest<RelationTypesRequest, RelationTypesResponse>(new RelationTypesRequest(), "RelationTypes");
        }

        [WebMethod]
        public static object AddRelationType(string name)
        {
            return ServiceAccessor.MakeRequest<RelationAddTypeRequest, RelationAddTypeResponse>(new RelationAddTypeRequest() { Name = name }, "RelationAddType").TypeId;
        }

        [WebMethod]
        public static int RelationTypeUseCount(Guid id)
        {
            return ServiceAccessor.MakeRequest<RelationTypeUseCountRequest, RelationTypeUseCountResponse>(new RelationTypeUseCountRequest() { TypeId = id }, "RelationTypeUseCount").Count;
        }
        
        [WebMethod]
        public static object RelationTypeDelete(Guid id)
        {
            return ServiceAccessor.MakeRequest<RelationTypeDeleteRequest, RelationTypeDeleteResponse>(new RelationTypeDeleteRequest() { TypeId = id }, "RelationTypeDelete").Success;
        }
    }
}