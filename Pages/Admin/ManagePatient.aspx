<%@ Page Title="Manage Patients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManagePatient.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.RegisterPatient" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Manage Patients</h2>

        <!-- Registration Form Section -->
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Register New Patient</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">Full
                            Name</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                                CssClass="text-danger" ErrorMessage="The Name field is required." Display="Dynamic"
                                ValidationGroup="RegisterPatient" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtDOB" CssClass="col-md-2 control-label">Date of
                            Birth</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtDOB" TextMode="Date" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDOB" CssClass="text-danger"
                                ErrorMessage="The Date of Birth field is required." Display="Dynamic"
                                ValidationGroup="RegisterPatient" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtContact" CssClass="col-md-2 control-label">
                            Contact Number</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtContact" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContact"
                                CssClass="text-danger" ErrorMessage="The Contact Number field is required."
                                Display="Dynamic" ValidationGroup="RegisterPatient" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="Register_Click" Text="Register Patient"
                                CssClass="btn btn-primary" ValidationGroup="RegisterPatient" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Patient List Section -->
        <div class="panel panel-default" style="margin-top: 30px;">
            <div class="panel-heading">
                <h3 class="panel-title">Patient List</h3>
            </div>
            <div class="panel-body">
                <!-- Search Section -->
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-md-6">
                        <div class="form-inline">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                                placeholder="Search by name or contact number" style="width: 300px;"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default"
                                OnClick="btnSearch_Click" CausesValidation="false" />
                            <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn btn-default"
                                OnClick="btnClearSearch_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>

                <!-- Patient List -->
                <asp:Repeater ID="rptPatients" runat="server" OnItemCommand="rptPatients_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Date of Birth</th>
                                    <th>Contact Number</th>
                                    <th>Linked User</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Id") %>
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'
                                    Visible='<%# GetEditingPatientId() != (int)Eval("Id") %>'></asp:Label>
                                <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control"
                                    Text='<%# Eval("Name") %>'
                                    Visible='<%# GetEditingPatientId() == (int)Eval("Id") %>'></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblDOB" runat="server"
                                    Text='<%# ((DateTime)Eval("DateOfBirth")).ToString("dd MMM yyyy") %>'
                                    Visible='<%# GetEditingPatientId() != (int)Eval("Id") %>'></asp:Label>
                                <asp:TextBox ID="txtEditDOB" runat="server" TextMode="Date" CssClass="form-control"
                                    Text='<%# ((DateTime)Eval("DateOfBirth")).ToString("yyyy-MM-dd") %>'
                                    Visible='<%# GetEditingPatientId() == (int)Eval("Id") %>'></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblContact" runat="server" Text='<%# Eval("ContactNumber") %>'
                                    Visible='<%# GetEditingPatientId() != (int)Eval("Id") %>'></asp:Label>
                                <asp:TextBox ID="txtEditContact" runat="server" CssClass="form-control"
                                    Text='<%# Eval("ContactNumber") %>'
                                    Visible='<%# GetEditingPatientId() == (int)Eval("Id") %>'></asp:TextBox>
                            </td>
                            <td>
                                <%# Eval("User") !=null ? ((Durdans_WebForms_MVP.Models.User)Eval("User")).Username
                                    : "Not linked" %>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-primary"
                                    CausesValidation="false" Visible='<%# GetEditingPatientId() != (int)Eval("Id") %>'>
                                    Edit
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-success"
                                    ValidationGroup="UpdatePatient"
                                    Visible='<%# GetEditingPatientId() == (int)Eval("Id") %>'>
                                    Update
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-default"
                                    CausesValidation="false" Visible='<%# GetEditingPatientId() == (int)Eval("Id") %>'>
                                    Cancel
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger"
                                    OnClientClick="return confirm('Are you sure you want to delete this patient?');"
                                    CausesValidation="false" Visible='<%# GetEditingPatientId() != (int)Eval("Id") %>'>
                                    Delete
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Label ID="lblNoPatients" runat="server" CssClass="alert alert-info" Visible="false">
                    No patients found.
                </asp:Label>

                <!-- Pagination -->
                <div class="row" style="margin-top: 20px;">
                    <div class="col-md-6">
                        <div class="form-inline">
                            <label for="ddlPageSize" class="mr-2">Items per page:</label>
                            <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                <asp:ListItem Text="10" Value="10" Selected="True" />
                                <asp:ListItem Text="25" Value="25" />
                                <asp:ListItem Text="50" Value="50" />
                                <asp:ListItem Text="100" Value="100" />
                            </asp:DropDownList>
                        </div>
                        <p class="text-muted" style="margin-top: 10px;">
                            <asp:Label ID="lblPatientCount" runat="server"></asp:Label>
                        </p>
                    </div>
                    <div class="col-md-6 text-right">
                        <asp:Panel ID="pnlPagination" runat="server">
                            <ul class="pagination justify-content-end">
                                <li class="page-item">
                                    <asp:LinkButton ID="lnkFirst" runat="server" CssClass="page-link"
                                        OnClick="lnkFirst_Click" Text="First" CausesValidation="false" />
                                </li>
                                <li class="page-item">
                                    <asp:LinkButton ID="lnkPrev" runat="server" CssClass="page-link"
                                        OnClick="lnkPrev_Click" Text="Previous" CausesValidation="false" />
                                </li>
                                <li class="page-item active">
                                    <span class="page-link">
                                        Page <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                        of <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                                    </span>
                                </li>
                                <li class="page-item">
                                    <asp:LinkButton ID="lnkNext" runat="server" CssClass="page-link"
                                        OnClick="lnkNext_Click" Text="Next" CausesValidation="false" />
                                </li>
                                <li class="page-item">
                                    <asp:LinkButton ID="lnkLast" runat="server" CssClass="page-link"
                                        OnClick="lnkLast_Click" Text="Last" CausesValidation="false" />
                                </li>
                            </ul>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>