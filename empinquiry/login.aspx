<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="empinquiry.login" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="login">
    <div id="logo">
        <%--<img src="https://s3.amazonaws.com/wrdsb-theme/images/WRDSB_Logo.svg" />--%>
        <img src="images/WRDSB_Logo.svg" />
    </div>
    <h1><%:ConfigurationManager.AppSettings["loginTitle"].ToString() %> </h1>

    <form id="loginform" runat="server">
    <fieldset>
        <!-- Error Msg -->
    <div class="alert alert-danger" role="alert" id="loginErrors" runat="server" visible="False"></div>
    <asp:LinkButton ID="logout2" runat="server" visible="False" Text="Logout" OnClick="Logout" />

        <!-- Login button -->
    <asp:Label for="tb_login" Text="Please enter wrdsb email address" runat="server"></asp:Label>
    <asp:TextBox ID="tb_login" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Button ID="bt_login" Text ="Login" runat ="server" OnClick="btLogin_Click" CssClass="btn btn-primary" />

    </fieldset>
    </form>
</div>
</asp:Content>