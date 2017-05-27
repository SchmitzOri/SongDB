using CommonDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LyricsConcord
{
    public partial class Export : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlInputFile File1;
        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static object UploadFile(object file)
        {
            byte[] backupData = null;

            return ServiceAccessor.UploadBackup(backupData);
        }

        protected void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                try
                {
                    byte[] backupData = null;
                    using (var binaryReader = new System.IO.BinaryReader(File1.PostedFile.InputStream))
                    {
                        backupData = binaryReader.ReadBytes(File1.PostedFile.ContentLength);
                    }

                    ServiceAccessor.UploadBackup(backupData);
                    // TODO: label
                    Response.Write("The file has been uploaded.");
                }
                catch (Exception ex)
                {
                    // TODO: label
                    Response.Write("Error: " + ex.Message);
                }
            }
            else
            {
                // TODO: Label
                Response.Write("Please select a file to upload.");
            }
        }
    }
}