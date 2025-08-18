<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="empinquiry.reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
               <%-- <h2 style="background-color:aliceblue"><strong>Employee Inquiry Search</strong></h2>--%>
            </div>
        </div>

      

        <div class="row">
            <div class="col-md-12">
                <p style="background-color:aliceblue">Please use the fields to search for the employee record</p>
                <br />
                <asp:Table runat ="server">
                    <asp:TableRow>                       
                        <asp:TableCell>Name</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_name" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Preferred Name</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_preferredname" runat="server"></asp:TextBox></asp:TableCell>    
                        <asp:TableCell>PAL\UserID</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_pal" runat="server"></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>    
                        <asp:TableCell>Email Address</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_email" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Phone Number</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_phone" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Postal Code</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_postalcode" runat="server"></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btn_clear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btn_clear_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btn_search" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btn_search_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
            </div>
        </div>

        <div class="row">
            <div class="col-md-12" style="min-height:200px;">
                <asp:ListView ID="lv_incidents" runat="server" DataSourceID="DataSource_incidents">
                    <LayoutTemplate>
                        <table class="table table-responsive table-bordered">
                            <tr>
                                <th>Name</th>
                                <th>Preferred Name</th>
                                <th>UserID</th>
                                <th>EMail</th>
                                <th>Phone</th>
                                <th>Job</th>
                                <th>Postal code</th>
                                <th>Last Date</th>
                                <th>Status</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><asp:Label ID="lbl_name" runat="server" 
                                Text='<%# String.Format("{0}, {1}", Eval("victim_surname"),Eval("victim_firstname")) %>'>
                                </asp:Label>
                            </td>
                            <td><asp:Label ID="Label6" runat="server" Text='Preferred'></asp:Label></td>
                            <td><asp:Label ID="lbl_grade" runat="server" Text='<%#Eval("victim_grade") %>'></asp:Label></td>
                            
                            <%--<td><asp:Label ID="lbl_cause" runat="server" Text='<%#Eval("cause_name") %>'></asp:Label></td>
                            <td><asp:Label ID="lbl_location" runat="server" Text='<%#Eval("location_name") %>'></asp:Label></td>--%>
                            <td><asp:Label ID="Label2" runat="server" Text='test@email.com'></asp:Label></td>
                            <td><asp:Label ID="Label3" runat="server" Text='1234'></asp:Label></td>
                            <td><asp:Label ID="Label4" runat="server" Text='s/w analyst'></asp:Label></td>
                            <td><asp:Label ID="Label5" runat="server" Text='n2d 5gh'></asp:Label></td>
                            
                            <td>
                                <asp:Label ID="lbl_incident_date" runat="server" 
                                    Text='<%#Bind("date_occurred","{0:MMMM dd, yyyy}") %>'></asp:Label>
                            </td>
                            <td><asp:Label ID="Label1" runat="server" Text='Active'></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr style="background-color:#e6e6e6">
                            <td><asp:Label ID="lbl_name" runat="server" 
                                Text='<%# String.Format("{0}, {1}", Eval("victim_surname"),Eval("victim_firstname")) %>'>
                                </asp:Label>
                            </td>
                            <td><asp:Label ID="Label6" runat="server" Text='Preferred'></asp:Label></td>
                            <td><asp:Label ID="lbl_grade" runat="server" Text='<%#Eval("victim_grade") %>'></asp:Label></td>
                            <%--<td><asp:Label ID="lbl_cause" runat="server" Text='<%#Eval("cause_name") %>'></asp:Label></td>
                            <td><asp:Label ID="lbl_location" runat="server" Text='<%#Eval("location_name") %>'></asp:Label></td>--%>
                            <td><asp:Label ID="Label2" runat="server" Text='test@email.com'></asp:Label></td>
                            <td><asp:Label ID="Label3" runat="server" Text='1234'></asp:Label></td>
                            <td><asp:Label ID="Label4" runat="server" Text='s/w analyst'></asp:Label></td>
                            <td><asp:Label ID="Label5" runat="server" Text='n2d 5gh'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_incident_date" runat="server" 
                                    Text='<%#Bind("date_occurred","{0:MMMM dd, yyyy}") %>'></asp:Label>
                            </td>
                            <td><asp:Label ID="Label1" runat="server" Text='Active'></asp:Label></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        We didn't find any data.
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="DataSource_incidents" runat="server" ConnectionString="<%$ ConnectionStrings:SQLDB %>"></asp:SqlDataSource>
</asp:Content>
