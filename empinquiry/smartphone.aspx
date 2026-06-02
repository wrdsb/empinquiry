<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="smartphone.aspx.cs" Inherits="empinquiry.smartphone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>

    <script>
    </script>

    <style>
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">

                <asp:Label ID="lblSelectedEmployee"
                    runat="server"
                    BackColor="AliceBlue">                                     
                </asp:Label>

                <asp:Panel ID="panelsmart" runat="server" DefaultButton="btn_Add">
                    <asp:Table runat="server">


                        <asp:TableRow>
                            <asp:TableCell>Order Date  </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_orderDate" Width="300px" runat="server" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>



                        <asp:TableRow>
                            <asp:TableCell>Phone Number</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="tb_phoneNumber" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
                            </asp:TableCell>
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
                            <asp:TableCell>
                                <asp:Button ID="btn_Add" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>

                <asp:Label ID="Labellist"
                    runat="server"
                    BackColor="AliceBlue">
                </asp:Label>

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
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-sm btn-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
