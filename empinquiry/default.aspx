<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="empinquiry._default" %>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <h2>Welcome <asp:Label ID="lbl_name" runat="server"></asp:Label>!</h2>
                <%--<h3>Audit</h3>--%>
                <div style ="background-color:aliceblue">                  
                    Please provide Audit information for the current search: 
                    <ol>
                        <li>Purpose</li>
                        <li>Ticket #</li>                       
                    </ol>
                </div>    
               
                <div class="form-group">
                    <label class="required" for="tb_purpose">Purpose of the search <span class="required_error"></span></label>

                    <asp:TextBox ID="tb_purpose" runat="server" CssClass="form-control"  TextMode="MultiLine" 
                    ValidationGroup="submit" MaxLength="400">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfv_purpose" runat="server" ControlToValidate="tb_purpose"
                    Display="Dynamic" Text="Required" ForeColor="Red" ErrorMessage="Purpose Required" 
                    ValidationGroup="submit">
                    </asp:RequiredFieldValidator>
    
                </div>
                <div class="form-group">
                    <label class="required" for="tb_ticket">Ticket # <span class="required_error"></span></label>

                    <asp:TextBox ID="tb_ticket" runat="server" CssClass="form-control"  TextMode="MultiLine" 
                    ValidationGroup="submit" MaxLength="400">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfv_ticket" runat="server" ControlToValidate="tb_ticket"
                    Display="Dynamic" Text="Required" ForeColor="Red" ErrorMessage="Ticket Required" 
                    ValidationGroup="submit">
                    </asp:RequiredFieldValidator>
    
                </div>


                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-md-12">
                         <asp:Button ID="btn_clear" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btn_clear_Click" /> 
                         <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-primary"  ValidationGroup="submit" OnClick ="btn_submit_Click"/>  
                    </div>
                </div>
               
             </div>
        </div>
    </div>
</asp:Content>
