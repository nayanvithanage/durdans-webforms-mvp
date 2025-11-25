<%@ Page Title="Hospital Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageHospital.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Admin.AddHospital" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Hospital Management</h2>

        <!-- Add Hospital Section -->
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Add New Hospital</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">
                            Hospital Name</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control"
                                placeholder="Enter hospital name" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                                CssClass="text-danger" ErrorMessage="Hospital name is required." Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="col-md-2 control-label">
                            Address</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control" TextMode="MultiLine"
                                Rows="3" placeholder="Enter hospital address" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                CssClass="text-danger" ErrorMessage="Address is required." Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="AddHospital_Click" Text="Add Hospital"
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

        <!-- Hospital List Section -->
        <div class="panel panel-info" style="margin-top: 30px;">
            <div class="panel-heading">
                <h3 class="panel-title">Hospital List</h3>
            </div>
            <div class="panel-body">
                <asp:GridView ID="gvHospitals" runat="server" CssClass="table table-striped table-bordered"
                    AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="gvHospitals_RowEditing"
                    OnRowCancelingEdit="gvHospitals_RowCancelingEdit" OnRowUpdating="gvHospitals_RowUpdating"
                    OnRowDeleting="gvHospitals_RowDeleting" EmptyDataText="No hospitals found. Add one above.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Name" HeaderText="Hospital Name" />
                        <asp:BoundField DataField="Address" HeaderText="Address" />
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Content>