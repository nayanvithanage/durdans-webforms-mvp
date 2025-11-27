<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Durdans_WebForms_MVP._Default" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="container mt-4">
            <!-- Welcome Message -->
            <div class="row mb-4">
                <div class="col-12">
                    <h1 class="display-4">
                        <asp:Label ID="lblWelcome" runat="server" Text="Welcome to Durdans Clinic" />
                    </h1>
                    <p class="lead text-muted">What would you like to do today?</p>
                    <hr />
                </div>
            </div>

            <!-- Admin Actions Panel -->
            <asp:Panel ID="pnlAdminActions" runat="server" Visible="false">
                <h3 class="mb-3">Admin Dashboard</h3>
                <div class="row g-4">
                    <!-- Manage Doctors Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-person-badge"></i> Manage Doctors
                                </h5>
                                <p class="card-text">Add, edit, or remove doctors from the system.</p>
                                <a href="~/Pages/Admin/ManageDoctors.aspx" runat="server" class="btn btn-primary">
                                    Go to Doctors
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- Manage Hospitals Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-hospital"></i> Manage Hospitals
                                </h5>
                                <p class="card-text">Manage hospital locations and information.</p>
                                <a href="~/Pages/Admin/ManageHospital.aspx" runat="server" class="btn btn-primary">
                                    Go to Hospitals
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- Manage Specializations Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-clipboard-pulse"></i> Manage Specializations
                                </h5>
                                <p class="card-text">Add or edit medical specializations.</p>
                                <a href="~/Pages/Admin/ManageSpecializations.aspx" runat="server"
                                    class="btn btn-primary">
                                    Go to Specializations
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- Register Patient Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-person-plus"></i> Manage Patients
                                </h5>
                                <p class="card-text">Register a new patient in the system.</p>
                                <a href="~/Pages/Admin/ManagePatient.aspx" runat="server" class="btn btn-primary">
                                    Register Patient
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- Book Appointment Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-calendar-check"></i> Book Appointment
                                </h5>
                                <p class="card-text">Book an appointment for any patient.</p>
                                <a href="~/Pages/BookAppointment.aspx" runat="server" class="btn btn-primary">
                                    Book Appointment
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- View All Appointments Card -->
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-calendar3"></i> View All Appointments
                                </h5>
                                <p class="card-text">View and manage all appointments.</p>
                                <a href="~/Pages/Dashboard.aspx" runat="server" class="btn btn-primary">
                                    View Appointments
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Patient Actions Panel -->
            <asp:Panel ID="pnlPatientActions" runat="server" Visible="false">
                <h3 class="mb-3">Patient Dashboard</h3>
                <div class="row g-4">
                    <!-- Book Appointment Card -->
                    <div class="col-md-6">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-calendar-check"></i> Book Appointment
                                </h5>
                                <p class="card-text">Schedule a new appointment with a doctor.</p>
                                <a href="~/Pages/BookAppointment.aspx" runat="server" class="btn btn-primary btn-lg">
                                    Book Now
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- My Appointments Card -->
                    <div class="col-md-6">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <i class="bi bi-calendar3"></i> My Appointments
                                </h5>
                                <p class="card-text">View your upcoming and past appointments.</p>
                                <a href="~/Pages/Dashboard.aspx" runat="server" class="btn btn-primary btn-lg">
                                    View Appointments
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Guest Panel (Not Logged In) -->
            <asp:Panel ID="pnlGuest" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-8 mx-auto text-center">
                        <h2>Welcome to Durdans Clinic Management System</h2>
                        <p class="lead">Please login or register to access the system.</p>
                        <div class="mt-4">
                            <a href="~/Pages/Login.aspx" runat="server" class="btn btn-primary btn-lg me-3">
                                <i class="bi bi-box-arrow-in-right"></i> Login
                            </a>
                            <a href="~/Pages/Register.aspx" runat="server" class="btn btn-outline-primary btn-lg">
                                <i class="bi bi-person-plus"></i> Register
                            </a>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>

    </asp:Content>