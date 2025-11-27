<%@ Page Title="Manage Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageDoctors.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Admin.ManageDoctors" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Manage Doctors</h2>

        <!-- Section 1: Doctor Registration -->
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Register New Doctor</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">Doctor
                            Name</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control"
                                placeholder="Enter doctor's full name" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                                CssClass="text-danger" ErrorMessage="Doctor name is required."
                                ValidationGroup="DoctorRegistration" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ddlSpecialization"
                            CssClass="col-md-2 control-label">Specialization</asp:Label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSpecialization" runat="server" CssClass="form-control"
                                DataTextField="Name" DataValueField="Id">
                                <asp:ListItem Text="-- Select Specialization --" Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSpecialization"
                                InitialValue="" CssClass="text-danger" ErrorMessage="Please select a specialization."
                                ValidationGroup="DoctorRegistration" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtFee" CssClass="col-md-2 control-label">
                            Consultation Fee</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtFee" CssClass="form-control" TextMode="Number"
                                placeholder="Enter consultation fee" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFee" CssClass="text-danger"
                                ErrorMessage="Consultation fee is required." ValidationGroup="DoctorRegistration"
                                Display="Dynamic" />
                            <asp:RangeValidator runat="server" ControlToValidate="txtFee" MinimumValue="1"
                                MaximumValue="100000" Type="Currency" CssClass="text-danger"
                                ErrorMessage="Fee must be between 1 and 100,000." ValidationGroup="DoctorRegistration"
                                Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Assign Hospitals</asp:Label>
                        <div class="col-md-10">
                            <asp:CheckBoxList ID="cblHospitals" runat="server" CssClass="checkbox" DataTextField="Name"
                                DataValueField="Id" RepeatDirection="Vertical" />
                            <small class="text-muted">Select one or more hospitals where this doctor practices</small>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="RegisterDoctor_Click" Text="Register Doctor"
                                CssClass="btn btn-primary" ValidationGroup="DoctorRegistration" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Label ID="lblDoctorMessage" runat="server" CssClass="text-info"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Section 2: Doctor List -->
        <div class="panel panel-info" style="margin-top: 30px;">
            <div class="panel-heading">
                <h3 class="panel-title">Registered Doctors</h3>
            </div>
            <div class="panel-body">
                <asp:GridView ID="gvDoctors" runat="server" CssClass="table table-striped table-bordered"
                    AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="gvDoctors_RowEditing"
                    OnRowCancelingEdit="gvDoctors_RowCancelingEdit" OnRowUpdating="gvDoctors_RowUpdating"
                    OnRowDeleting="gvDoctors_RowDeleting" OnRowDataBound="gvDoctors_RowDataBound"
                    EmptyDataText="No doctors found. Register one above.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:TemplateField HeaderText="Specialization">
                            <ItemTemplate>
                                <%# Eval("Specialization.Name") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditSpecialization" runat="server" CssClass="form-control"
                                    DataTextField="Name" DataValueField="Id">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ConsultationFee" HeaderText="Fee" DataFormatString="{0:C}" />
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Section 3: Doctor Availability Settings -->
        <div class="panel panel-success" style="margin-top: 30px;">
            <div class="panel-heading">
                <h3 class="panel-title">Set Doctor Availability</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ddlDoctorSelect"
                            CssClass="col-md-2 control-label">Select Doctor</asp:Label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDoctorSelect" runat="server" CssClass="form-control"
                                DataTextField="Name" DataValueField="Id" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDoctorSelect_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select Doctor --" Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDoctorSelect"
                                InitialValue="" CssClass="text-danger" ErrorMessage="Please select a doctor."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ddlHospitalSelect"
                            CssClass="col-md-2 control-label">Select Hospital</asp:Label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlHospitalSelect" runat="server" CssClass="form-control"
                                DataTextField="Name" DataValueField="Id">
                                <asp:ListItem Text="-- Select Hospital --" Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHospitalSelect"
                                InitialValue="" CssClass="text-danger" ErrorMessage="Please select a hospital."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Days of Week</asp:Label>
                        <div class="col-md-10">
                            <asp:CheckBoxList ID="cblDaysOfWeek" runat="server" CssClass="checkbox"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Monday" Value="Monday" />
                                <asp:ListItem Text="Tuesday" Value="Tuesday" />
                                <asp:ListItem Text="Wednesday" Value="Wednesday" />
                                <asp:ListItem Text="Thursday" Value="Thursday" />
                                <asp:ListItem Text="Friday" Value="Friday" />
                                <asp:ListItem Text="Saturday" Value="Saturday" />
                                <asp:ListItem Text="Sunday" Value="Sunday" />
                            </asp:CheckBoxList>
                            <small class="text-muted">Select one or more days when doctor is available</small>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ddlStartTime" CssClass="col-md-2 control-label">
                            Start Time</asp:Label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStartTime" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlStartTime" InitialValue=""
                                CssClass="text-danger" ErrorMessage="Start time is required."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                        </div>

                        <asp:Label runat="server" AssociatedControlID="ddlEndTime" CssClass="col-md-2 control-label">End
                            Time</asp:Label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEndTime" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEndTime" InitialValue=""
                                CssClass="text-danger" ErrorMessage="End time is required."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtMaxBookings"
                            CssClass="col-md-2 control-label">Max Bookings Per Slot</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtMaxBookings" CssClass="form-control" TextMode="Number"
                                Text="3" placeholder="Maximum appointments per time slot" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMaxBookings"
                                CssClass="text-danger" ErrorMessage="Max bookings is required."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                            <asp:RangeValidator runat="server" ControlToValidate="txtMaxBookings" MinimumValue="1"
                                MaximumValue="10" Type="Integer" CssClass="text-danger"
                                ErrorMessage="Max bookings must be between 1 and 10."
                                ValidationGroup="AvailabilitySettings" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="AddAvailability_Click" Text="Add Availability"
                                CssClass="btn btn-success" ValidationGroup="AvailabilitySettings" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Label ID="lblAvailabilityMessage" runat="server" CssClass="text-info"></asp:Label>
                        </div>
                    </div>
                </div>

                <!-- Availability List GridView -->
                <div style="margin-top: 20px;">
                    <h4>Current Availability Schedule</h4>
                    <asp:GridView ID="gvAvailability" runat="server" CssClass="table table-striped table-bordered"
                        AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="gvAvailability_RowEditing"
                        OnRowCancelingEdit="gvAvailability_RowCancelingEdit" OnRowUpdating="gvAvailability_RowUpdating"
                        OnRowDeleting="gvAvailability_RowDeleting"
                        EmptyDataText="No availability schedules found. Add one above.">
                        <Columns>
                            <asp:BoundField DataField="DoctorName" HeaderText="Doctor" ReadOnly="True" />
                            <asp:BoundField DataField="HospitalName" HeaderText="Hospital" ReadOnly="True" />
                            <asp:BoundField DataField="DayOfWeek" HeaderText="Day" ReadOnly="True" />
                            <asp:BoundField DataField="TimeSlot" HeaderText="Time Slot" ReadOnly="True" />
                            <asp:BoundField DataField="MaxBookingsPerSlot" HeaderText="Max Bookings" />
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Content>