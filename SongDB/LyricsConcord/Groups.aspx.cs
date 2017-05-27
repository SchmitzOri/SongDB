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
    public partial class Groups : System.Web.UI.Page
    {
        protected GroupAllResponse groups;
        protected GetWordsResponse words;

        protected void Page_Load(object sender, EventArgs e)
        {
            groups = ServiceAccessor.MakeRequest<GroupAllRequest, GroupAllResponse>(new GroupAllRequest(), "GroupAll");
            words = ServiceAccessor.MakeRequest<GetWordsRequest, GetWordsResponse>(new GetWordsRequest(), "GetWords");
        }

        [WebMethod]
        public static object AddGroup(string name, List<string> words)
        {
            return ServiceAccessor.MakeRequest<GroupAddRequest, GroupAddResponse>(new GroupAddRequest() { Name = name, Words = words }, "GroupAdd").Id;
        }

        [WebMethod]
        public static List<string> Words(Guid id)
        {
            return ServiceAccessor.MakeRequest<GroupWordsRequest, GroupWordsResponse>(new GroupWordsRequest() { Id = id }, "GroupWords").Words.Select(w => w.Item2).ToList();
        }

        [WebMethod]
        public static object UpdateGroup(Guid id, string name, List<string> words)
        {
            return ServiceAccessor.MakeRequest<GroupUpdateRequest, GroupUpdateResponse>(new GroupUpdateRequest() { Id = id, Name = name, Words = words }, "GroupUpdate").Success;
        }
    }
}