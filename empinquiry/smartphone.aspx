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

                 <!-- For Grid -->
                <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
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
                <!-- Add New button -->
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add Smartphone Details" OnClick ="btnAdd_Click" />

            </div>
        </div>
    </div>
</asp:Content>
