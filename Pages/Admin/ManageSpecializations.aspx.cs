using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages.Admin
{
    public partial class ManageSpecializations : AdminBasePage
    {
        private SpecializationService _specializationService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _specializationService = new SpecializationService();

            if (!IsPostBack)
            {
                LoadSpecializations();
            }
        }

        private void LoadSpecializations()
        {
            try
            {
                var specializations = _specializationService.GetAllSpecializations();
                gvSpecializations.DataSource = specializations;
                gvSpecializations.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading specializations: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void AddSpecialization_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var specialization = new Specialization
                    {
                        Name = txtName.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        IsActive = chkIsActive.Checked
                    };

                    int newId = _specializationService.AddSpecialization(specialization);

                    lblMessage.Text = $"Specialization added successfully! ID: {newId}";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Clear form
                    txtName.Text = "";
                    txtDescription.Text = "";
                    chkIsActive.Checked = true;

                    // Reload grid
                    LoadSpecializations();
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

        protected void gvSpecializations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSpecializations.EditIndex = e.NewEditIndex;
            LoadSpecializations();
        }

        protected void gvSpecializations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSpecializations.EditIndex = -1;
            LoadSpecializations();
        }

        protected void gvSpecializations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int specializationId = (int)gvSpecializations.DataKeys[e.RowIndex].Value;
                TextBox txtName = (TextBox)gvSpecializations.Rows[e.RowIndex].Cells[1].Controls[0];
                TextBox txtDescription = (TextBox)gvSpecializations.Rows[e.RowIndex].Cells[2].Controls[0];
                CheckBox chkIsActive = (CheckBox)gvSpecializations.Rows[e.RowIndex].Cells[3].Controls[0];

                var specialization = _specializationService.GetSpecializationById(specializationId);
                if (specialization != null)
                {
                    specialization.Name = txtName.Text.Trim();
                    specialization.Description = txtDescription.Text.Trim();
                    specialization.IsActive = chkIsActive.Checked;

                    _specializationService.UpdateSpecialization(specialization);

                    lblMessage.Text = "Specialization updated successfully!";
                    lblMessage.CssClass = "text-success alert alert-success";
                }

                gvSpecializations.EditIndex = -1;
                LoadSpecializations();
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

        protected void gvSpecializations_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int specializationId = (int)gvSpecializations.DataKeys[e.RowIndex].Value;
                _specializationService.DeleteSpecialization(specializationId);

                lblMessage.Text = "Specialization deactivated successfully!";
                lblMessage.CssClass = "text-success alert alert-success";

                LoadSpecializations();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _specializationService?.Dispose();
        }
    }
}
