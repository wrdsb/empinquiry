<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="empinquiry.reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">            
                <asp:Table runat ="server">
                    <asp:TableRow> 
                        <asp:TableCell>Employee Id</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_empId" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Surname</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_surname" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>First Name</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_firstname" runat="server"></asp:TextBox></asp:TableCell>                     
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Preferred Name</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_preferredname" runat="server"></asp:TextBox></asp:TableCell> 
                        <asp:TableCell>PAL\UserID</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_pal" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Job Code</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_jobcode" runat="server"></asp:TextBox></asp:TableCell>                       
                    </asp:TableRow>
                    <asp:TableRow>   
                        <asp:TableCell>Email Address</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_email" runat="server"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Phone Number</asp:TableCell>
                        <asp:TableCell><asp:TextBox ID="tb_phone" runat="server"></asp:TextBox></asp:TableCell>                                             
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

        <!-- For Grid -->

        <div class="row">
            <div class="col-md-12" style="min-height:200px;">
                <asp:ListView ID="lv_incidents" runat="server" DataSourceID="DataSource_incidents">
                    <LayoutTemplate>
                        <table class="table table-responsive table-bordered">
                            <tr>
                                <th>Emp Id</th>
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
                            <td><asp:Label ID="lbl_empId" runat="server" Text='<%#Eval("employee_id")%>'></asp:Label></td>
                            <td><asp:Label ID="lbl_name" runat="server" 
                                Text='<%# String.Format("{0}, {1}", Eval("surname"),Eval("first_name")) %>'>
                                </asp:Label>
                            </td>
                            <td><asp:Label ID="lbl_known_as" runat="server" Text='<%#Eval("known_as_first")%>'></asp:Label></td>
                            <td><asp:Label ID="lbl_user_id" runat="server" Text='<%#Eval("user_id")%>'></asp:Label></td>                               
                            <td><asp:Label ID="lbl_email_address" runat="server" Text='<%#Eval("e_mail_address")%>'></asp:Label></td>
                            <td><asp:Label ID="lbl_phone" runat="server" Text='<%#Eval("telephone_no")%>'></asp:Label></td>
                            <td><asp:Label ID="lbl_jobcode" runat="server" Text='s/w analyst'></asp:Label></td>
                            <td><asp:Label ID="lbl_postal_code" runat="server" Text='<%#Eval("postal_code")%>'></asp:Label></td>                            
                            <td>
                                <asp:Label ID="lbl_review_date" runat="server" 
                                    Text='<%#Bind("review_date","{0:MMMM dd, yyyy}") %>'></asp:Label>
                            </td>
                            <td><asp:Label ID="lbl_activity_code" runat="server" Text='<%#Eval("emp_activity_code")%>'></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                <%--  <AlternatingItemTemplate>
                        <tr style="background-color:#e6e6e6">
                            <td><asp:Label ID="lbl_name" runat="server" 
                                Text='<%# String.Format("{0}, {1}", Eval("victim_surname"),Eval("victim_firstname")) %>'>
                                </asp:Label>
                            </td>
                            <td><asp:Label ID="Label6" runat="server" Text='Preferred'></asp:Label></td>
                            <td><asp:Label ID="lbl_grade" runat="server" Text='<%#Eval("victim_grade") %>'></asp:Label></td>
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
                    </AlternatingItemTemplate>--%>
                    <EmptyDataTemplate>
                        We didn't find any data.
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="DataSource_incidents" runat="server" ConnectionString="<%$ ConnectionStrings:SQLDB %>"></asp:SqlDataSource>
</asp:Content>
