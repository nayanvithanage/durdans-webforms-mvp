<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BookAppointment.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.BookAppointment" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Book an Appointment</h2>

        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlPatient" CssClass="col-md-2 control-label">Select
                    Patient</asp:Label>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlPatient" runat="server" CssClass="form-control" DataTextField="Name"
                        DataValueField="Id"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPatient" InitialValue=""
                        CssClass="text-danger" ErrorMessage="Please select a patient." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlSpecialization" CssClass="col-md-2 control-label">
                    Specialization</asp:Label>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlSpecialization" runat="server" CssClass="form-control" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSpecialization_SelectedIndexChanged">
                        <asp:ListItem Text="-- Select Specialization --" Value="" />
                        <asp:ListItem Text="Cardiology" Value="Cardiology" />
                        <asp:ListItem Text="Pediatrics" Value="Pediatrics" />
                        <asp:ListItem Text="Dermatology" Value="Dermatology" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlDoctor" CssClass="col-md-2 control-label">Select
                    Doctor</asp:Label>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-control" DataTextField="Name"
                        DataValueField="Id">
                        <asp:ListItem Text="-- Select Doctor --" Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDoctor" InitialValue=""
                        CssClass="text-danger" ErrorMessage="Please select a doctor." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtDate" CssClass="col-md-2 control-label">Appointment
                    Date</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="txtDate" TextMode="Date" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" CssClass="text-danger"
                        ErrorMessage="The Date field is required." />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" OnClick="Book_Click" Text="Confirm Booking" CssClass="btn btn-success" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-info"></asp:Label>
                </div>
            </div>
        </div>
    </asp:Content>