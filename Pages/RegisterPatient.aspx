<%@ Page Title="Register Patient" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegisterPatient.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.RegisterPatient" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Register New Patient</h2>

        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">Full Name
                </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" CssClass="text-danger"
                        ErrorMessage="The Name field is required." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtDOB" CssClass="col-md-2 control-label">Date of Birth
                </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="txtDOB" TextMode="Date" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDOB" CssClass="text-danger"
                        ErrorMessage="The Date of Birth field is required." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtContact" CssClass="col-md-2 control-label">Contact
                    Number</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="txtContact" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContact" CssClass="text-danger"
                        ErrorMessage="The Contact Number field is required." />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" OnClick="Register_Click" Text="Register" CssClass="btn btn-primary" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                </div>
            </div>
        </div>
    </asp:Content>