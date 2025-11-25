<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="Durdans_WebForms_MVP.Pages.Register" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div class="d-flex justify-content-center align-items-center" style="min-height: 80vh;">
            <div class="register-box" style="width: 400px;">
                <div class="card card-outline card-primary">
                    <div class="card-header text-center">
                        <a href="../Default.aspx" class="h1"><b>Durdans</b>Clinic</a>
                    </div>
                    <div class="card-body">
                        <p class="login-box-msg">Register a new membership</p>

                        <div class="form-group mb-3">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control"
                                    placeholder="Username" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-user"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                                CssClass="text-danger small" ErrorMessage="Username is required." Display="Dynamic" />
                        </div>

                        <div class="form-group mb-3">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email"
                                    placeholder="Email" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-envelope"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                                CssClass="text-danger small" ErrorMessage="Email is required." Display="Dynamic" />
                        </div>

                        <div class="form-group mb-3">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password"
                                    placeholder="Password" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-lock"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                                CssClass="text-danger small" ErrorMessage="Password is required." Display="Dynamic" />
                        </div>

                        <div class="form-group mb-3">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtConfirmPassword" CssClass="form-control"
                                    TextMode="Password" placeholder="Retype password" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-lock"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword"
                                CssClass="text-danger small" ErrorMessage="Confirm Password is required."
                                Display="Dynamic" />
                            <asp:CompareValidator runat="server" ControlToValidate="txtConfirmPassword"
                                ControlToCompare="txtPassword" CssClass="text-danger small"
                                ErrorMessage="Passwords do not match." Display="Dynamic" />
                        </div>

                        <div class="form-group mb-3">
                            <label class="small text-muted">Date of Birth</label>
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtDateOfBirth" CssClass="form-control"
                                    TextMode="Date" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-calendar"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDateOfBirth"
                                CssClass="text-danger small" ErrorMessage="Date of Birth is required."
                                Display="Dynamic" />
                        </div>

                        <div class="form-group mb-3">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtContactNumber" CssClass="form-control"
                                    TextMode="Phone" placeholder="Contact Number" />
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-phone"></span>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactNumber"
                                CssClass="text-danger small" ErrorMessage="Contact Number is required."
                                Display="Dynamic" />
                        </div>

                        <div class="row">
                            <div class="col-8">
                                <div class="icheck-primary">
                                    <input type="checkbox" id="agreeTerms" name="terms" value="agree">
                                    <label for="agreeTerms">
                                        I agree to the <a href="#">terms</a>
                                    </label>
                                </div>
                            </div>
                            <div class="col-4">
                                <asp:Button runat="server" OnClick="Register_Click" Text="Register"
                                    CssClass="btn btn-primary btn-block" />
                            </div>
                        </div>

                        <div class="social-auth-links text-center mt-2 mb-3">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>

                        <a href="Login.aspx" class="text-center">I already have a membership</a>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>