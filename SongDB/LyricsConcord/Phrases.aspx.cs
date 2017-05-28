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
    public partial class Phrases : System.Web.UI.Page
    {
        protected PhraseAllResponse phrases;

        protected void Page_Load(object sender, EventArgs e)
        {
            phrases = ServiceAccessor.MakeRequest<PhraseAllRequest, PhraseAllResponse>(new PhraseAllRequest(), "PhraseAll");
        }

        [WebMethod]
        public static object AddPhrase(List<string> words)
        {
            return ServiceAccessor.MakeRequest<PhraseAddRequest, PhraseAddResponse>(new PhraseAddRequest() { Words = words }, "PhraseAdd");
        }

        [WebMethod]
        public static object DeletePhrase(Guid id)
        {
            return ServiceAccessor.MakeRequest<PhraseDeleteRequest, PhraseDeleteResponse>(new PhraseDeleteRequest() { Id = id }, "PhraseAdd");
        }
    }
}