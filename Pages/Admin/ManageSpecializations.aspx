<%@ Page Title="Specialization Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageSpecializations.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Admin.ManageSpecializations" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Specialization Management</h2>

        <!-- Add Specialization Section -->
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Add New Specialization</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">
                            Specialization Name</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control"
                                placeholder="e.g., Cardiology, Pediatrics" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                                CssClass="text-danger" ErrorMessage="Specialization name is required."
                                Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtDescription"
                            CssClass="col-md-2 control-label">Description</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" TextMode="MultiLine"
                                Rows="3" placeholder="Brief description (optional)" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:CheckBox runat="server" ID="chkIsActive" Text="Active" Checked="true" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="AddSpecialization_Click" Text="Add Specialization"
                                CssClass="btn btn-primary" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-info"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Specialization List Section -->
        <div class="panel panel-info" style="margin-top: 30px;">
            <div class="panel-heading">
                <h3 class="panel-title">Specialization List</h3>
            </div>
            <div class="panel-body">
                <asp:GridView ID="gvSpecializations" runat="server" CssClass="table table-striped table-bordered"
                    AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="gvSpecializations_RowEditing"
                    OnRowCancelingEdit="gvSpecializations_RowCancelingEdit"
                    OnRowUpdating="gvSpecializations_RowUpdating" OnRowDeleting="gvSpecializations_RowDeleting"
                    EmptyDataText="No specializations found. Add one above.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Name" HeaderText="Specialization Name" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Content>