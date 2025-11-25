using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages.Admin
{
    public partial class AddHospital : AdminBasePage
    {
        private HospitalService _hospitalService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _hospitalService = new HospitalService();

            if (!IsPostBack)
            {
                LoadHospitals();
            }
        }

        private void LoadHospitals()
        {
            try
            {
                var hospitals = _hospitalService.GetAllHospitals();
                gvHospitals.DataSource = hospitals;
                gvHospitals.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading hospitals: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void AddHospital_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var hospital = new Hospital
                    {
                        Name = txtName.Text.Trim(),
                        Address = txtAddress.Text.Trim()
                    };

                    int newId = _hospitalService.AddHospital(hospital);

                    lblMessage.Text = $"Hospital added successfully! ID: {newId}";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Clear form
                    txtName.Text = "";
                    txtAddress.Text = "";

                    // Reload grid
                    LoadHospitals();
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = "Validation Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        protected void gvHospitals_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvHospitals.EditIndex = e.NewEditIndex;
            LoadHospitals();
        }

        protected void gvHospitals_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHospitals.EditIndex = -1;
            LoadHospitals();
        }

        protected void gvHospitals_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int hospitalId = (int)gvHospitals.DataKeys[e.RowIndex].Value;
                TextBox txtName = (TextBox)gvHospitals.Rows[e.RowIndex].Cells[1].Controls[0];
                TextBox txtAddress = (TextBox)gvHospitals.Rows[e.RowIndex].Cells[2].Controls[0];

                var hospital = _hospitalService.GetHospitalById(hospitalId);
                if (hospital != null)
                {
                    hospital.Name = txtName.Text.Trim();
                    hospital.Address = txtAddress.Text.Trim();

                    _hospitalService.UpdateHospital(hospital);

                    lblMessage.Text = "Hospital updated successfully!";
                    lblMessage.CssClass = "text-success alert alert-success";
                }

                gvHospitals.EditIndex = -1;
                LoadHospitals();
            }
            catch (ArgumentException ex)
            {
                lblMessage.Text = "Validation Error: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void gvHospitals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int hospitalId = (int)gvHospitals.DataKeys[e.RowIndex].Value;
                _hospitalService.DeleteHospital(hospitalId);

                lblMessage.Text = "Hospital deleted successfully!";
                lblMessage.CssClass = "text-success alert alert-success";

                LoadHospitals();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _hospitalService?.Dispose();
        }
    }
}
