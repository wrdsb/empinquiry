<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="smartphone.aspx.cs" Inherits="empinquiry.smartphone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
    <script>
        function setEligibleDate() {

            var orderDate = document.getElementById('<%= tb_orderDate.ClientID %>').value;

            if (orderDate) {

                var dt = new Date(orderDate);

                dt.setFullYear(dt.getFullYear() + 3);

                var yyyy = dt.getFullYear();
                var mm = String(dt.getMonth() + 1).padStart(2, '0');
                var dd = String(dt.getDate()).padStart(2, '0');

                document.getElementById('<%= tb_eligibleDate.ClientID %>').value =
                    yyyy + '-' + mm + '-' + dd;
            }
        }


        $(document).on("input", "#tb_phoneNumber", function () {
            //console.log("Input detected: " + this.value); // Check F12 console
            this.value = this.value.replace(/[^0-9\s\-()+]/g, '');
        });



    </script>


    <script type="text/javascript">
        function hideLabel() {
            // Option A: Instant hide after delay
            setTimeout(function () {
                document.getElementById('<%= lblsubmit.ClientID %>').style.display = 'none';
            }, 5000); // 5000 milliseconds = 5 seconds       
        }
</script>

    <style>
        /* Style the container */
        .custom-radio-list {
            font-family: Arial, sans-serif;
            gap: 20px;
            display: flex;
        }

            /* Style the labels (the text next to buttons) */
            .custom-radio-list label {
                margin-left: 5px;
                cursor: pointer;
                color: #333;
            }

            /* Style the radio input itself */
            .custom-radio-list input[type="radio"] {
                cursor: pointer;
                transform: scale(1.2); /* Make buttons slightly larger */
            }

            /* Hover effect on the labels */
            .custom-radio-list label:hover {
                color: #007bff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="note">
                    <asp:Label ID="lblSelectedEmployee"
                        runat="server"
                        Font-Bold="true">                       
                    </asp:Label>
                </div>
                <div class="announcement">
                    <asp:Label ID="Labelinfo"
                        runat="server"
                        Font-Bold="true">                       
                    </asp:Label>
                </div>

                <asp:Panel ID="panelsmart" runat="server" DefaultButton="btn_Add">
                    <asp:Table runat="server">


                        <asp:TableRow>
                            <asp:TableCell>Order Date  </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_orderDate" Width="300px" runat="server" TextMode="Date" CssClass="form-control" onchange="setEligibleDate();"></asp:TextBox>

                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Phone Number</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_phoneNumber" runat="server" Width="300px" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </asp:TableCell>
                            <%-- <asp:TableCell>
                                <asp:RegularExpressionValidator
                                    ID="revPhoneNumber"
                                    runat="server"
                                    ControlToValidate="tb_phoneNumber"
                                    ValidationExpression="^[0-9+\s\-()]*$"
                                    ErrorMessage="Phone number cannot contain letters."
                                    ForeColor="Red">
                                </asp:RegularExpressionValidator>
                            </asp:TableCell>--%>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Tier</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddl_tier" runat="server" AutoPostBack="true" Width="300px" CssClass="form-control" OnSelectedIndexChanged="ddl_tier_SelectedIndexChanged">
                                    <asp:ListItem Text="Tier1" Value="Tier1" />
                                    <asp:ListItem Text="Tier2" Value="Tier2" />
                                    <asp:ListItem Text="Tier3" Value="Tier3" />
                                    <asp:ListItem Text="Tier4" Value="Tier4" />
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Ordered Item</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddl_orderedItem" runat="server" AutoPostBack="true" Width="300px" CssClass="form-control" OnSelectedIndexChanged="ddl_orderedItem_SelectedIndexChanged">
                                    <asp:ListItem Text="SIM" Value="SIM" />
                                    <asp:ListItem Text="Phone" Value="Phone" />
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Rogers Account Created</asp:TableCell>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="rbl_RogersYesNo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="custom-radio-list">
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell> Board Contribution Paid</asp:TableCell>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="rbl_BoardYesNo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="custom-radio-list">
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Next Eligible Date</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_eligibleDate" Width="300px" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>


                        <asp:TableRow>
                            <asp:TableCell>Link Options Form</asp:TableCell>
                            <asp:TableCell>
                                <asp:HyperLink
                                    ID="lnkGoogleFolder"
                                    runat="server"
                                    Text="Open Google Folder"
                                    Target="_blank" NavigateUrl="https://drive.google.com/drive/folders/1b0ClP5MY2XEAMFTpOo46LTmtORJ4UXak">
                                </asp:HyperLink>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>Notes</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_notes" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    MaxLength="400">
                                </asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>


                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btn_Add" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblsubmit"
                                    runat="server"
                                    Font-Bold="true" ForeColor="ForestGreen">
                                </asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>


                    </asp:Table>
                </asp:Panel>
                <div class="announcement">
                    <asp:Label ID="lbllist"
                        runat="server"
                        Font-Bold="true">
                    </asp:Label>
                </div>

                <!-- For Grid -->
                <asp:GridView ID="smartphoneOrdersGrid" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone #" />
                        <asp:BoundField DataField="Tier" HeaderText="Tier" />
                        <asp:BoundField DataField="Item" HeaderText="Item" />
                        <asp:BoundField DataField="Rogers" HeaderText="Rogers" />
                        <asp:BoundField DataField="BoardPaid" HeaderText="Board Paid" />
                        <asp:BoundField DataField="EligibleDate" HeaderText="Eligible Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="Forms" HeaderText="Forms" />
                        <asp:BoundField DataField="Notes" HeaderText="Notes" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-sm btn-primary" />
                                <!--<asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-sm btn-danger" />-->
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
