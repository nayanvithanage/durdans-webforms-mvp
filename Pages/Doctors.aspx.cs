using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class Doctors : System.Web.UI.Page
    {
        private DoctorService _service = new DoctorService();

        // Controls declared in .aspx but missing from designer file
        protected global::System.Web.UI.WebControls.GridView gvDoctors;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDoctors();
            }
        }

        private void BindDoctors()
        {
            gvDoctors.DataSource = _service.GetAllDoctors();
            gvDoctors.DataBind();
        }
    }
}
