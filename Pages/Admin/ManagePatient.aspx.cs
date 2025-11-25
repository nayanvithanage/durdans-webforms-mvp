using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class RegisterPatient : AdminBasePage
    {
        private PatientService _patientService;
        private AppointmentService _appointmentService;
        private int? _editingPatientId = null;

        // Pagination properties
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                    return 1;
                return (int)ViewState["CurrentPage"];
            }
            set { ViewState["CurrentPage"] = value; }
        }

        private int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null)
                    return 10;
                return (int)ViewState["PageSize"];
            }
            set { ViewState["PageSize"] = value; }
        }

        private string SearchTerm
        {
            get
            {
                return ViewState["SearchTerm"]?.ToString() ?? "";
            }
            set { ViewState["SearchTerm"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _patientService = new PatientService();
            _appointmentService = new AppointmentService();

            if (!IsPostBack)
            {
                LoadPatients();
            }
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var patient = new Patient
                    {
                        Name = txtName.Text.Trim(),
                        DateOfBirth = DateTime.Parse(txtDOB.Text),
                        ContactNumber = txtContact.Text.Trim()
                    };

                    int newId = _patientService.RegisterPatient(patient);
                    
                    lblMessage.Text = $"Patient registered successfully! Patient ID: {newId}";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Clear form
                    txtName.Text = "";
                    txtDOB.Text = "";
                    txtContact.Text = "";

                    // Reload patient list
                    CurrentPage = 1;
                    LoadPatients();
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = "Validation Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                catch (InvalidOperationException ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-warning alert alert-warning";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An unexpected error occurred: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        private void LoadPatients()
        {
            try
            {
                var allPatients = _patientService.GetAllPatients();

                // Apply search filter
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    allPatients = allPatients.Where(p =>
                        p.Name.ToLower().Contains(SearchTerm.ToLower()) ||
                        p.ContactNumber.Contains(SearchTerm)).ToList();
                }

                // Calculate pagination
                int totalCount = allPatients.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);

                // Ensure current page is valid
                if (CurrentPage > totalPages && totalPages > 0)
                    CurrentPage = totalPages;
                if (CurrentPage < 1)
                    CurrentPage = 1;

                // Apply pagination
                var pagedPatients = allPatients
                    .OrderBy(p => p.Name)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                rptPatients.DataSource = pagedPatients;
                rptPatients.DataBind();
                lblNoPatients.Visible = !pagedPatients.Any();

                // Update pagination controls
                UpdatePagination(totalCount, totalPages);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading patients: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        private void UpdatePagination(int totalCount, int totalPages)
        {
            lblCurrentPage.Text = CurrentPage.ToString();
            lblTotalPages.Text = totalPages.ToString();
            lblPatientCount.Text = $"Showing {((CurrentPage - 1) * PageSize) + 1}-{Math.Min(CurrentPage * PageSize, totalCount)} of {totalCount} patients";

            // Update page size dropdown
            ddlPageSize.SelectedValue = PageSize.ToString();

            // Enable/disable navigation buttons
            lnkFirst.Enabled = CurrentPage > 1;
            lnkPrev.Enabled = CurrentPage > 1;
            lnkNext.Enabled = CurrentPage < totalPages;
            lnkLast.Enabled = CurrentPage < totalPages;

            // Show/hide pagination panel
            pnlPagination.Visible = totalPages > 1;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadPatients();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            SearchTerm = "";
            CurrentPage = 1;
            LoadPatients();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = int.Parse(ddlPageSize.SelectedValue);
            CurrentPage = 1;
            LoadPatients();
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadPatients();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPatients();
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadPatients();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            var allPatients = _patientService.GetAllPatients();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allPatients = allPatients.Where(p =>
                    p.Name.ToLower().Contains(SearchTerm.ToLower()) ||
                    p.ContactNumber.Contains(SearchTerm)).ToList();
            }
            int totalPages = (int)Math.Ceiling((double)allPatients.Count / PageSize);
            CurrentPage = totalPages;
            LoadPatients();
        }

        protected void rptPatients_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int patientId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Edit")
            {
                _editingPatientId = patientId;
                ViewState["EditingPatientId"] = patientId;
                LoadPatients();
            }
            else if (e.CommandName == "Cancel")
            {
                _editingPatientId = null;
                ViewState["EditingPatientId"] = null;
                LoadPatients();
            }
            else if (e.CommandName == "Update")
            {
                try
                {
                    // Get the patient
                    var patient = _patientService.GetPatientById(patientId);
                    if (patient == null)
                    {
                        lblMessage.Text = "Patient not found.";
                        lblMessage.CssClass = "text-danger alert alert-danger";
                        return;
                    }

                    // Get updated values from the repeater
                    TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
                    TextBox txtEditDOB = (TextBox)e.Item.FindControl("txtEditDOB");
                    TextBox txtEditContact = (TextBox)e.Item.FindControl("txtEditContact");

                    if (txtEditName != null && txtEditDOB != null && txtEditContact != null)
                    {
                        patient.Name = txtEditName.Text.Trim();
                        patient.DateOfBirth = DateTime.Parse(txtEditDOB.Text);
                        patient.ContactNumber = txtEditContact.Text.Trim();

                        _patientService.UpdatePatient(patient);

                        lblMessage.Text = "Patient updated successfully!";
                        lblMessage.CssClass = "text-success alert alert-success";

                        _editingPatientId = null;
                        ViewState["EditingPatientId"] = null;
                        LoadPatients();
                    }
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = "Validation Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error updating patient: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    _patientService.DeletePatient(patientId, _appointmentService);

                    lblMessage.Text = "Patient deleted successfully!";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Reload patients
                    LoadPatients();
                }
                catch (InvalidOperationException ex)
                {
                    lblMessage.Text = ex.Message;
                    lblMessage.CssClass = "text-warning alert alert-warning";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error deleting patient: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        protected int GetEditingPatientId()
        {
            if (ViewState["EditingPatientId"] != null)
            {
                return (int)ViewState["EditingPatientId"];
            }
            return 0;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Restore editing state
            if (ViewState["EditingPatientId"] != null)
            {
                _editingPatientId = (int)ViewState["EditingPatientId"];
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _patientService?.Dispose();
            _appointmentService?.Dispose();
        }
    }
}
