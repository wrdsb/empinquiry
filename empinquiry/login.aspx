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
    <div class="alert alert-danger" role="alert" id="loginErrors" runat="server" visible="False"></div>
    <asp:LinkButton ID="logout2" runat="server" visible="False" Text="Logout" OnClick="Logout" />

    </fieldset>
    </form>
    <h3>Reporting Property Damage?</h3>
    <a href="https://staff.wrdsb.ca/bfs/risk-services/damage-to-property-incident-report/incident-report-form/" onclick="ga('send', 'event', 'empinquiry Login Link', 'click', 'empinquiry Property Login Link');" target="_blank">WRDSB Property Incident Reporting</a>
</div>
</asp:Content>