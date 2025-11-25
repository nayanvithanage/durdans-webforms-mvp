<%@ Page Title="Appointments Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Appointments Dashboard</h2>

    <!-- Admin Filter Panel -->
    <asp:Panel ID="pnlAdminFilters" runat="server" Visible="false">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">Filter Appointments</h4>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtPatientName" CssClass="control-label">Patient Name</asp:Label>
                            <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" placeholder="Search by patient name"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ddlDoctorFilter" CssClass="control-label">Doctor</asp:Label>
                            <asp:DropDownList ID="ddlDoctorFilter" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All Doctors" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ddlHospitalFilter" CssClass="control-label">Hospital</asp:Label>
                            <asp:DropDownList ID="ddlHospitalFilter" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All Hospitals" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ddlStatusFilter" CssClass="control-label">Status</asp:Label>
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All" Value="All" Selected="True" />
                                <asp:ListItem Text="Upcoming" Value="Upcoming" />
                                <asp:ListItem Text="Past" Value="Past" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtFromDate" CssClass="control-label">From Date</asp:Label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtToDate" CssClass="control-label">To Date</asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">&nbsp;</label><br />
                            <asp:Button ID="btnFilter" runat="server" Text="Apply Filters" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnClearFilters" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btnClearFilters_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Tabs for Upcoming/Past -->
    <ul class="nav nav-tabs" role="tablist">
        <li role="presentation" class="active">
            <asp:LinkButton ID="lnkUpcoming" runat="server" OnClick="lnkUpcoming_Click" CssClass="nav-link">
                Upcoming Appointments
            </asp:LinkButton>
        </li>
        <li role="presentation">
            <asp:LinkButton ID="lnkPast" runat="server" OnClick="lnkPast_Click" CssClass="nav-link">
                Past Appointments
            </asp:LinkButton>
        </li>
    </ul>

    <!-- Message Display -->
    <div class="row" style="margin-top: 15px;">
        <div class="col-md-12">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-info"></asp:Label>
        </div>
    </div>

    <!-- Upcoming Appointments -->
    <asp:Panel ID="pnlUpcoming" runat="server">
        <h3>Upcoming Appointments</h3>
        <asp:Repeater ID="rptUpcomingAppointments" runat="server" OnItemCommand="rptAppointments_ItemCommand">
            <HeaderTemplate>
                <div class="row">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-md-4" style="margin-bottom: 20px;">
                    <div class="card border-success">
                        <div class="card-header bg-success text-white">
                            <strong>Appointment #<%# Eval("Id") %></strong>
                            <span class="badge badge-light float-right">Upcoming</span>
                        </div>
                        <div class="card-body">
                            <p><strong>Patient:</strong> <%# Eval("Patient.Name") %></p>
                            <p><strong>Doctor:</strong> <%# Eval("Doctor.Name") %></p>
                            <p><strong>Hospital:</strong> <%# Eval("Hospital.Name") %></p>
                            <p><strong>Date:</strong> <%# ((DateTime)Eval("AppointmentDate")).ToString("dd MMM yyyy") %></p>
                            <p><strong>Time:</strong> <%# FormatTime((TimeSpan)Eval("AppointmentTime")) %></p>
                            <p><small><strong>Booked By:</strong> <%# Eval("BookedBy") %> (<%# Eval("BookingType") %>)</small></p>
                        </div>
                        <div class="card-footer">
                            <asp:LinkButton ID="btnEdit" runat="server" 
                                CommandName="Edit" 
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-primary"
                                Visible='<%# GetIsAdmin() %>'>
                                <i class="bi bi-pencil"></i> Edit
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" 
                                CommandName="Delete" 
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-danger"
                                OnClientClick="return confirm('Are you sure you want to cancel this appointment?');"
                                Visible='<%# GetIsAdmin() %>'>
                                <i class="bi bi-trash"></i> Cancel
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        <!-- Add this after the Upcoming Appointments Repeater (around line 139) -->
<div class="row" style="margin-top: 20px;">
    <div class="col-md-6">
        <div class="form-inline">
            <label for="ddlPageSizeUpcoming" class="mr-2">Items per page:</label>
            <asp:DropDownList ID="ddlPageSizeUpcoming" runat="server" CssClass="form-control" 
                AutoPostBack="true" OnSelectedIndexChanged="ddlPageSizeUpcoming_SelectedIndexChanged">
                <asp:ListItem Text="10" Value="10" Selected="True" />
                <asp:ListItem Text="25" Value="25" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>
        <p class="text-muted" style="margin-top: 10px;">
            <asp:Label ID="lblUpcomingCount" runat="server"></asp:Label>
        </p>
    </div>
    <div class="col-md-6 text-right">
        <asp:Panel ID="pnlUpcomingPagination" runat="server">
            <ul class="pagination justify-content-end">
                <li class="page-item">
                    <asp:LinkButton ID="lnkFirstUpcoming" runat="server" CssClass="page-link" 
                        OnClick="lnkFirstUpcoming_Click" Text="First" />
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkPrevUpcoming" runat="server" CssClass="page-link" 
                        OnClick="lnkPrevUpcoming_Click" Text="Previous" />
                </li>
                <li class="page-item active">
                    <span class="page-link">
                        Page <asp:Label ID="lblCurrentPageUpcoming" runat="server"></asp:Label> 
                        of <asp:Label ID="lblTotalPagesUpcoming" runat="server"></asp:Label>
                    </span>
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkNextUpcoming" runat="server" CssClass="page-link" 
                        OnClick="lnkNextUpcoming_Click" Text="Next" />
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkLastUpcoming" runat="server" CssClass="page-link" 
                        OnClick="lnkLastUpcoming_Click" Text="Last" />
                </li>
            </ul>
        </asp:Panel>
    </div>
</div>
        <asp:Label ID="lblNoUpcoming" runat="server" CssClass="alert alert-info" Visible="false">
            No upcoming appointments found.
        </asp:Label>
    </asp:Panel>

    <!-- Past Appointments -->
    <asp:Panel ID="pnlPast" runat="server" Visible="false">
        <h3>Past Appointments</h3>
        <asp:Repeater ID="rptPastAppointments" runat="server" OnItemCommand="rptAppointments_ItemCommand">
            <HeaderTemplate>
                <div class="row">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-md-4" style="margin-bottom: 20px;">
                    <div class="card border-secondary">
                        <div class="card-header bg-secondary text-white">
                            <strong>Appointment #<%# Eval("Id") %></strong>
                            <span class="badge badge-light float-right">Past</span>
                        </div>
                        <div class="card-body">
                            <p><strong>Patient:</strong> <%# Eval("Patient.Name") %></p>
                            <p><strong>Doctor:</strong> <%# Eval("Doctor.Name") %></p>
                            <p><strong>Hospital:</strong> <%# Eval("Hospital.Name") %></p>
                            <p><strong>Date:</strong> <%# ((DateTime)Eval("AppointmentDate")).ToString("dd MMM yyyy") %></p>
                            <p><strong>Time:</strong> <%# FormatTime((TimeSpan)Eval("AppointmentTime")) %></p>
                            <p><small><strong>Booked By:</strong> <%# Eval("BookedBy") %> (<%# Eval("BookingType") %>)</small></p>
                        </div>
                        <div class="card-footer">
                            <asp:LinkButton ID="btnDelete" runat="server" 
                                CommandName="Delete" 
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-danger"
                                OnClientClick="return confirm('Are you sure you want to delete this appointment?');"
                                Visible='<%# GetIsAdmin() %>'>
                                <i class="bi bi-trash"></i> Delete
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        !-- Add similar pagination for Past Appointments (after line 183) -->
<div class="row" style="margin-top: 20px;">
    <div class="col-md-6">
        <div class="form-inline">
            <label for="ddlPageSizePast" class="mr-2">Items per page:</label>
            <asp:DropDownList ID="ddlPageSizePast" runat="server" CssClass="form-control" 
                AutoPostBack="true" OnSelectedIndexChanged="ddlPageSizePast_SelectedIndexChanged">
                <asp:ListItem Text="10" Value="10" Selected="True" />
                <asp:ListItem Text="25" Value="25" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>
        <p class="text-muted" style="margin-top: 10px;">
            <asp:Label ID="lblPastCount" runat="server"></asp:Label>
        </p>
    </div>
    <div class="col-md-6 text-right">
        <asp:Panel ID="pnlPastPagination" runat="server">
            <ul class="pagination justify-content-end">
                <li class="page-item">
                    <asp:LinkButton ID="lnkFirstPast" runat="server" CssClass="page-link" 
                        OnClick="lnkFirstPast_Click" Text="First" />
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkPrevPast" runat="server" CssClass="page-link" 
                        OnClick="lnkPrevPast_Click" Text="Previous" />
                </li>
                <li class="page-item active">
                    <span class="page-link">
                        Page <asp:Label ID="lblCurrentPagePast" runat="server"></asp:Label> 
                        of <asp:Label ID="lblTotalPagesPast" runat="server"></asp:Label>
                    </span>
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkNextPast" runat="server" CssClass="page-link" 
                        OnClick="lnkNextPast_Click" Text="Next" />
                </li>
                <li class="page-item">
                    <asp:LinkButton ID="lnkLastPast" runat="server" CssClass="page-link" 
                        OnClick="lnkLastPast_Click" Text="Last" />
                </li>
            </ul>
        </asp:Panel>
    </div>
</div>
        <asp:Label ID="lblNoPast" runat="server" CssClass="alert alert-info" Visible="false">
            No past appointments found.
        </asp:Label>
    </asp:Panel>
</asp:Content>