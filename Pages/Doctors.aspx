<%@ Page Title="Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Doctors.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Doctors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Our Doctors</h2>
    <p>Select a specialist for your consultation.</p>

    <asp:GridView ID="gvDoctors" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Doctor Name" />
            <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
            <asp:BoundField DataField="ConsultationFee" HeaderText="Fee (LKR)" DataFormatString="{0:C}" />
        </Columns>
    </asp:GridView>
</asp:Content>
