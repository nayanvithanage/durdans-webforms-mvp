<%@ Page Title="Add Doctor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddDoctor.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Admin.AddDoctor" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Register New Doctor</h2>
        <p class="lead">Enter doctor details and availability.</p>

        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-md-2">Name</label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" CssClass="text-danger"
                        ErrorMessage="Name is required." />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-md-2">Specialization</label>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlSpecialization" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Select --" Value="" />
                        <asp:ListItem Text="Cardiology" Value="Cardiology" />
                        <asp:ListItem Text="Dermatology" Value="Dermatology" />
                        <asp:ListItem Text="Neurology" Value="Neurology" />
                        <asp:ListItem Text="Pediatrics" Value="Pediatrics" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSpecialization"
                        CssClass="text-danger" ErrorMessage="Specialization is required." />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-md-2">Consultation Fee (LKR)</label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtFee" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFee" CssClass="text-danger"
                        ErrorMessage="Fee is required." />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-md-2">Available Days</label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDays" runat="server" CssClass="form-control" placeholder="e.g., Mon, Wed, Fri">
                    </asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-md-2">Available Time</label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtTime" runat="server" CssClass="form-control"
                        placeholder="e.g., 09:00 AM - 12:00 PM"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button ID="btnRegister" runat="server" Text="Register Doctor" CssClass="btn btn-primary"
                        OnClick="Register_Click" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            </div>
        </div>
    </asp:Content>