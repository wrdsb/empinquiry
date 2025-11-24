<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="empinquiry.reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btn_search">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>Emp Id</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_empId" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>Surname</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_surname" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>First Name</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_firstname" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>Former Name</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_formername" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Known as firstname</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_preferredfirstname" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>Known as surname</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_preferredsurname" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>PAL\UserID</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_pal" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>Email</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_email" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            
                        </asp:TableRow>
                        <asp:TableRow>
                             <asp:TableCell>Phone</asp:TableCell>
                             <asp:TableCell>
                                 <asp:TextBox ID="tb_phone" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                             </asp:TableCell>
                            <asp:TableCell>Group Code</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_grpcode" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>Job</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddl_job" runat="server" CssClass="form-control" Width="150px" Height="34px"
                                    DataSourceID="SqlDataSource_job" DataTextField="job_code_description" DataValueField="description_abbr"
                                    OnDataBound="ddl_job_DataBound">
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>Status</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control" Width="150px" Height="34px"
                                    DataSourceID="SqlDataSource_status" DataTextField="emp_activity_code" DataValueField="emp_activity_code"
                                    OnDataBound="ddl_status_DataBound">
                                </asp:DropDownList>
                            </asp:TableCell>
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
                </asp:Panel>
                <br />
            </div>
        </div>

        <!-- For Grid -->

        <div class="row">
            <div class="col-md-12" style="min-height: 200px;">
                <asp:ListView ID="lv_search" runat="server" DataSourceID="DataSource_search" OnItemCommand="lv_search_ItemCommand"
                    OnPagePropertiesChanging="lv_search_PagePropertiesChanging" OnSorting ="lv_search_Sorting">
                    <LayoutTemplate>
                        <table class="table table-responsive table-bordered">
                            <tr>
                                <asp:Literal runat="server" ID="litDetails"></asp:Literal>
                            </tr>
                            <tr>
                                <th>Emp Id</th>
                                <th>Name (Surname, Firstname)</th>
                                <th>Known as (Surname, Firstname)</th>
                                <%--<th>Known as surname</th>--%>
                                <th>Former Name</th>
                                <th>UserID</th>
                                <th>Email</th>
                                <th>Phone</th>
                                <th>Postal code</th>                         
                                <th> 
                                    <asp:LinkButton ID="lnkSortJobCode" runat="server" CommandName="Sort" CommandArgument="job_code">
                                        Job code 
                                    </asp:LinkButton>
                                </th>
                                <th>Job Desc</th>
                                <th>Group code</th>
                                <%--<th>Location code</th>
                                <th>Record change date</th>--%>
                                <th>Status</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_empId" runat="server" Text='<%#Eval("employee_id")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_name" runat="server"
                                    Text='<%# String.Format("{0}, {1}", Eval("surname"),Eval("first_name")) %>'>
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_known_as" runat="server" 
                                    Text='<%# String.Format("{0}, {1}", Eval("known_as"),Eval("known_as_first")) %>'></asp:Label></td>
                            <%--<td>
                                <asp:Label ID="lbl_known_as_surname" runat="server" Text='<%#Eval("known_as")%>'></asp:Label></td>--%>
                            <td>
                                <asp:Label ID="lbl_former_name" runat="server" Text='<%#Eval("former_name")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_user_id" runat="server" Text='<%#Eval("user_id")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_email_address" runat="server" Text='<%#Eval("e_mail_address")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_phone" runat="server"
                                    Text='<%# String.Format("{0}-{1}", Eval("telephone_area"), Eval("telephone_no"))%>'>
                                </asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_postal_code" runat="server" Text='<%#Eval("postal_code")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_jobcode" runat="server" Text='<%#Eval("job_code") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_jobdesc" runat="server" Text='<%#Eval("description_text") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_group_code" runat="server" Text='<%#Eval("emp_group_code")%>'></asp:Label></td>
                            <%--
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text='<%#Eval("location_code")%>'></asp:Label></td>

                            <td>
                                <asp:Label ID="lbl_review_date" runat="server" 
                                    Text='<%#Bind("record_change_date","{0:MMMM dd, yyyy}") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <%--<%# Eval("emp_activity_code") %>--%>
                                <asp:Button
                                    ID="btnDetails"
                                    Width="100px" Height="40px"
                                    CssClass="btn btn-primary"
                                    runat="server"
                                    Text='<%#Eval("emp_activity_code")%>'
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("employee_id") + ";" + Eval("emp_activity_code") + ";" + Eval("location_code") + ";" + Eval("record_change_date") %>'
                                    Visible='<%# Eval("emp_activity_code").ToString() == "ONLEAVE" ||
                                        Eval("emp_activity_code").ToString() == "DECEASED" || 
                                        Eval("emp_activity_code").ToString() == "INACTIVE" || 
                                        Eval("emp_activity_code").ToString() == "ONLEAVE" || 
                                        Eval("emp_activity_code").ToString() == "OTHER" || 
                                        Eval("emp_activity_code").ToString() == "RESIGNED" || 
                                        Eval("emp_activity_code").ToString() == "RETIRED" || 
                                        Eval("emp_activity_code").ToString() == "TERMINAT" ||
                                        Eval("emp_activity_code").ToString() == "ACTIVE"
                                        %>' />

                               <%-- <asp:Label
                                    ID="lbl_activity_code"
                                    runat="server"
                                    Text='<%#Eval("emp_activity_code")%>'
                                    Visible='<%# Eval("emp_activity_code").ToString() == "ACTIVE" %>'></asp:Label>--%>

                            </td>
                        </tr>
                    </ItemTemplate>
                    <%-- TODO ======TODO  <AlternatingItemTemplate>
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
                    <SelectedItemTemplate>
                        <tr style="background-color: lightcyan">
                            <td>
                                <asp:Label ID="lbl_empId" runat="server" Text='<%#Eval("employee_id")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_name" runat="server"
                                    Text='<%# String.Format("{0}, {1}", Eval("surname"),Eval("first_name")) %>'>
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_known_as" runat="server" 
                                    Text='<%# String.Format("{0}, {1}", Eval("known_as"),Eval("known_as_first")) %>'></asp:Label></td>
                            <%--<td>
                                <asp:Label ID="lbl_known_as_surname" runat="server" Text='<%#Eval("known_as")%>'></asp:Label></td>--%>
                            <td>
                                <asp:Label ID="lbl_former_name" runat="server" Text='<%#Eval("former_name")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_user_id" runat="server" Text='<%#Eval("user_id")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_email_address" runat="server" Text='<%#Eval("e_mail_address")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_phone" runat="server"
                                    Text='<%# String.Format("{0}-{1}", Eval("telephone_area"), Eval("telephone_no"))%>'>
                                </asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_postal_code" runat="server" Text='<%#Eval("postal_code")%>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_jobcode" runat="server" Text='<%#Eval("job_code") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_jobdesc" runat="server" Text='<%#Eval("description_text") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl_group_code" runat="server" Text='<%#Eval("emp_group_code")%>'></asp:Label></td>
                            <%--
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text='<%#Eval("location_code")%>'></asp:Label></td>

                            <td>
                                <asp:Label ID="lbl_review_date" runat="server" 
                                    Text='<%#Bind("record_change_date","{0:MMMM dd, yyyy}") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <%--<%# Eval("emp_activity_code") %>--%>
                                <asp:Button
                                    ID="btnDetails"
                                    Width="100px" Height="40px"
                                    CssClass="btn btn-primary"
                                    runat="server"
                                    Text='<%#Eval("emp_activity_code")%>'
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("employee_id") + ";" + Eval("emp_activity_code") + ";" + Eval("location_code") + ";" + Eval("record_change_date") %>'
                                    Visible='<%# Eval("emp_activity_code").ToString() == "ONLEAVE" ||
                                        Eval("emp_activity_code").ToString() == "DECEASED" || 
                                        Eval("emp_activity_code").ToString() == "INACTIVE" || 
                                        Eval("emp_activity_code").ToString() == "ONLEAVE" || 
                                        Eval("emp_activity_code").ToString() == "OTHER" || 
                                        Eval("emp_activity_code").ToString() == "RESIGNED" || 
                                        Eval("emp_activity_code").ToString() == "RETIRED" || 
                                        Eval("emp_activity_code").ToString() == "TERMINAT" ||
                                        Eval("emp_activity_code").ToString() == "ACTIVE"
                                        %>' />

                               <%-- <asp:Label
                                    ID="lbl_activity_code"
                                    runat="server"
                                    Text='<%#Eval("emp_activity_code")%>'
                                    Visible='<%# Eval("emp_activity_code").ToString() == "ACTIVE" %>'></asp:Label>--%>

                            </td>
                        </tr>

                    </SelectedItemTemplate>
                    <EmptyDataTemplate>
                        We didn't find any data.
                    </EmptyDataTemplate>
                </asp:ListView>
                <asp:DataPager ID="MyDataPager" EnableEventValidation="false" runat="server" PagedControlID="lv_search" PageSize="25">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button"
                            ShowFirstPageButton="True" ShowLastPageButton="True"
                            PreviousPageText="&laquo; Prev"
                            NextPageText="Next &raquo;"
                            FirstPageText="First"
                            LastPageText="Last" />
                        <asp:NumericPagerField ButtonCount="5" />
                    </Fields>
                </asp:DataPager>  
                <!-- Add multiple &nbsp; for more space -->
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblCount" runat="server" CssClass="text-info"></asp:Label>
               
           </div>        
        </div>
    </div>


    <!-- Custom Modal -->
    <div id="detailsModal" class="myModal">
        <div class="myModal-content">
            <span class="myClose" onclick="document.getElementById('detailsModal').style.display='none';">&times;</span>
            <asp:Literal ID="litDetails" runat="server"></asp:Literal>
        </div>
    </div>


    <asp:SqlDataSource ID="DataSource_search" runat="server" ConnectionString="<%$ ConnectionStrings:SQLDB %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource_Job" runat="server" ConnectionString="<%$ ConnectionStrings:SQLDB %>"
        SelectCommand="SELECT DISTINCT job_code,description_abbr, description_text, job_code + ' - ' + description_text AS job_code_description FROM ec_jobs ORDER BY description_text"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource_status" runat="server" ConnectionString="<%$ ConnectionStrings:SQLDB %>"
        SelectCommand="SELECT DISTINCT(emp_activity_code) FROM ec_employee ORDER BY emp_activity_code"></asp:SqlDataSource>
</asp:Content>
