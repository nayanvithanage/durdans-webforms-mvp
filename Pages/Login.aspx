<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Durdans_WebForms_MVP.Pages.Login" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div class="d-flex justify-content-center align-items-center" style="min-height: 80vh;">
            <div class="login-box" style="width: 400px;">
                <div class="card card-outline card-primary">
                    <div class="card-header text-center">
                        <a href="../Default.aspx" class="h1"><b>Durdans</b>Clinic</a>
                    </div>
                    <div class="card-body">
                        <p class="login-box-msg">Sign in to start your session</p>

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

                        <div class="row">
                            <div class="col-8">
                                <div class="icheck-primary">
                                    <input type="checkbox" id="remember">
                                    <label for="remember">
                                        Remember Me
                                    </label>
                                </div>
                            </div>
                            <div class="col-4">
                                <asp:Button runat="server" OnClick="Login_Click" Text="Sign In"
                                    CssClass="btn btn-primary btn-block" />
                            </div>
                        </div>

                        <div class="social-auth-links text-center mt-2 mb-3">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>

                        <p class="mb-1">
                            <a href="#">I forgot my password</a>
                        </p>
                        <p class="mb-0">
                            <a href="Register.aspx" class="text-center">Register a new membership</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>