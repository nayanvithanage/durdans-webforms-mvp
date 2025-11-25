<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BookAppointment.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.BookAppointment" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Book an Appointment</h2>

        <div class="row">
            <div class="col-md-6">
                <!-- Left Panel: Selection Form -->
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Appointment Details</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <!-- Patient Selection -->
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="ddlPatient"
                                    CssClass="col-md-4 control-label">Select Patient</asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPatient" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPatient"
                                        InitialValue="" CssClass="text-danger" ErrorMessage="Please select a patient."
                                        Display="Dynamic" />
                                </div>
                            </div>

                            <!-- Specialization Selection -->
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="ddlSpecialization"
                                    CssClass="col-md-4 control-label">
                                    Select Specialization</asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlSpecialization" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="Id" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSpecialization_SelectedIndexChanged">
                                        <asp:ListItem Text="-- Select Specialization --" Value="" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <!-- Doctor Selection -->
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="ddlDoctor"
                                    CssClass="col-md-4 control-label">
                                    Select Doctor</asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="Id" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
                                        <asp:ListItem Text="-- Select Doctor --" Value="" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDoctor"
                                        InitialValue="" CssClass="text-danger" ErrorMessage="Please select a doctor."
                                        Display="Dynamic" />
                                </div>
                            </div>

                            <!-- Hospital Selection -->
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="ddlHospital"
                                    CssClass="col-md-4 control-label">Select Hospital</asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlHospital" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="Id">
                                        <asp:ListItem Text="-- Select Hospital --" Value="" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHospital"
                                        InitialValue="" CssClass="text-danger" ErrorMessage="Please select a hospital."
                                        Display="Dynamic" />
                                </div>
                            </div>

                            <!-- Date Selection with Calendar -->
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Select Date</asp:Label>
                                <div class="col-md-8">
                                    <asp:Calendar ID="calAppointmentDate" runat="server" CssClass="table table-bordered"
                                        OnDayRender="calAppointmentDate_DayRender"
                                        OnSelectionChanged="calAppointmentDate_SelectionChanged" BackColor="White"
                                        BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest"
                                        Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px"
                                        Width="100%">
                                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                        <NextPrevStyle VerticalAlign="Bottom" />
                                        <OtherMonthDayStyle ForeColor="#808080" />
                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <SelectorStyle BackColor="#CCCCCC" />
                                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <WeekendDayStyle BackColor="#FFFFCC" />
                                    </asp:Calendar>
                                    <small class="text-muted">Past dates are disabled</small>
                                </div>
                            </div>

                            <!-- Load Time Slots Button -->
                            <div class="form-group">
                                <div class="col-md-offset-4 col-md-8">
                                    <asp:Button ID="btnLoadTimeSlots" runat="server" Text="Load Available Time Slots"
                                        CssClass="btn btn-info btn-block" OnClick="LoadTimeSlots_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <!-- Right Panel: Time Slot Selection -->
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title">Available Time Slots</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlTimeSlots" runat="server" Visible="false">
                            <div class="alert alert-info">
                                <strong>Legend:</strong>
                                <span class="label label-success">Green = Available</span>
                                <span class="label label-warning">Amber = Almost Full</span>
                                <span class="label label-danger">Red = Unavailable</span>
                            </div>

                            <asp:Repeater ID="rptTimeSlots" runat="server" OnItemCommand="rptTimeSlots_ItemCommand">
                                <HeaderTemplate>
                                    <div class="time-slots-container">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnTimeSlot" runat="server"
                                        CssClass='<%# "time-slot " + Eval("CssClass") %>' CommandName="SelectTimeSlot"
                                        CommandArgument='<%# Eval("TimeValue") %>' Enabled='<%# Eval("IsAvailable") %>'
                                        ToolTip='<%# Eval("Status") %>'>
                                        <%# Eval("DisplayText") %>
                                            <br />
                                            <small>
                                                <%# Eval("Status") %>
                                            </small>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                    </div>
                    </FooterTemplate>
                    </asp:Repeater>

                    <asp:HiddenField ID="hfSelectedTimeSlot" runat="server" />

                    <div style="margin-top: 20px;">
                        <asp:Label ID="lblSelectedSlot" runat="server" CssClass="alert alert-info" Visible="false">
                        </asp:Label>
                    </div>

                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnConfirmBooking" runat="server" Text="Confirm Booking"
                            CssClass="btn btn-success btn-lg btn-block" OnClick="Book_Click" />
                    </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlNoSlots" runat="server" Visible="false">
                        <div class="alert alert-warning">
                            <strong>No time slots available.</strong> Please select a different date or doctor.
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        </div>

        <!-- Message Display -->
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-info"></asp:Label>
            </div>
        </div>

        <!-- CSS for Time Slots -->
        <style>
            .time-slots-container {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
                gap: 10px;
                margin-top: 15px;
            }

            .time-slot {
                display: block;
                padding: 15px 10px;
                text-align: center;
                border-radius: 5px;
                text-decoration: none;
                font-weight: bold;
                transition: all 0.3s ease;
                border: 2px solid transparent;
            }

            .time-slot-available {
                background-color: #d4edda;
                color: #155724;
                border-color: #c3e6cb;
            }

            .time-slot-available:hover {
                background-color: #28a745;
                color: white;
                transform: scale(1.05);
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            }

            .time-slot-almost-full {
                background-color: #fff3cd;
                color: #856404;
                border-color: #ffeaa7;
            }

            .time-slot-almost-full:hover {
                background-color: #ffc107;
                color: white;
                transform: scale(1.05);
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            }

            .time-slot-unavailable {
                background-color: #f8d7da;
                color: #721c24;
                border-color: #f5c6cb;
                cursor: not-allowed;
                opacity: 0.6;
            }

            .time-slot.selected {
                border: 3px solid #007bff;
                box-shadow: 0 0 10px rgba(0, 123, 255, 0.5);
            }
        </style>
    </asp:Content>