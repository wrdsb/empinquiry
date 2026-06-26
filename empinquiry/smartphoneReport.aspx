<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="smartphoneReport.aspx.cs" Inherits="empinquiry.smartphoneReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
    <script>
        function setToDate() {

            var fromDate = document.getElementById('<%= tb_fromDate.ClientID %>').value;

            if (fromDate) {

                var dt = new Date(fromDate);

                dt.setMonth(dt.getMonth() + 3);

                var yyyy = dt.getFullYear();
                var mm = String(dt.getMonth() + 1).padStart(2, '0');
                var dd = String(dt.getDate()).padStart(2, '0');

                document.getElementById('<%= tb_toDate.ClientID %>').value =
                    yyyy + '-' + mm + '-' + dd;
            }
        }


        $(document).on("input", "#tb_phoneNumber", function () {
            //console.log("Input detected: " + this.value); // Check F12 console
            this.value = this.value.replace(/[^0-9\s\-()+]/g, '');
        });



    </script>


    <script type="text/javascript">

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
                <asp:Panel ID="pnlDateRange" runat="server" GroupingText="Filter By Date Range">
                    <!-- Place your web controls inside here -->
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label runat="server">Date Type</asp:Label>
                            <asp:DropDownList ID="ddl_DateType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Order Date" Value="ORDER_DATE"></asp:ListItem>
                                <asp:ListItem Text="Eligibility Date" Value="NEXT_ELIGIBLE_DATE"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:Label runat="server">From Date</asp:Label>
                            <asp:TextBox ID="tb_fromDate" Width="200px" runat="server" CssClass="form-control" TextMode="Date" onchange="setToDate();"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:Label runat="server">To Date</asp:Label>
                            <asp:TextBox ID="tb_toDate" Width="200px" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="panelsmart" runat="server" GroupingText="Other Filters" DefaultButton="btn_Add">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                Phone Number                      
                                <asp:TextBox ID="tb_phoneNumber" runat="server" Width="200px" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                Tier
                                <asp:DropDownList ID="ddl_tier" runat="server" AutoPostBack="true" Width="200px" CssClass="form-control" OnSelectedIndexChanged="ddl_tier_SelectedIndexChanged">
                                    <asp:ListItem Text="Tier1" Value="Tier1" />
                                    <asp:ListItem Text="Tier2" Value="Tier2" />
                                    <asp:ListItem Text="Tier3" Value="Tier3" />
                                    <asp:ListItem Text="Tier4" Value="Tier4" />
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                Ordered Item
                                 <asp:DropDownList ID="ddl_orderedItem" runat="server" AutoPostBack="true" Width="200px" CssClass="form-control" OnSelectedIndexChanged="ddl_orderedItem_SelectedIndexChanged">
                                     <asp:ListItem Text="SIM" Value="SIM" />
                                     <asp:ListItem Text="Phone" Value="Phone" />
                                 </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Rogers Account Created</asp:TableCell>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="rbl_RogersYesNo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="custom-radio-list">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:TableCell>
                            <asp:TableCell>Board Contribution Paid</asp:TableCell>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="rbl_BoardYesNo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="custom-radio-list">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btn_Clear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btn_Clear_Click" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btn_Add" runat="server" CssClass="btn btn-primary" Text="Add Smartphone Order" OnClick="btnAdd_Click" />
                            </asp:TableCell>

                        </asp:TableRow>

                    </asp:Table>
                </asp:Panel>


                <!-- For Grid -->
                <asp:GridView ID="smartphoneOrdersGrid" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="table table-striped">
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
                                <asp:LinkButton ID="btnEdit"
                                    runat="server"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    Text="Modify"
                                    CssClass="btn btn-sm btn-primary"
                                    OnClick="btnEdit_Click" />
                                <asp:LinkButton ID="btnDelete"
                                    runat="server"
                                    Text="Delete"
                                    CssClass="btn btn-sm btn-primary"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    OnClick="btnDelete_Click"
                                    OnClientClick="return confirm('Are you sure to delete the smartphone order?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfId" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
