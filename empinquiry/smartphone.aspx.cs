using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace empinquiry
{
    public partial class smartphone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (Session["auditComplete"] == null || Convert.ToBoolean(Session["auditComplete"]) == false)
            {
                Response.Redirect("Login.aspx");
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                if (Session["surname"] == null || Session["firstname"] == null)
                {
                    Session.Clear();
                    Session.Abandon();

                    Response.Redirect("login.aspx");
                }
                string empId = Session["selectedEmpId"].ToString();
                string empName = $"{Session["selectedFirstname"]} {Session["selectedSurname"]}";
                lblSelectedEmployee.Text = $"Add smartphone details for employee Id : {empId} | Name : {empName}";
                Labellist.Text ="List of smartphone orders for employee Id : " + empId + " | Name : " + empName;

                BindGrid();

            }

        }
        private void BindGrid()
        {
            // Replace this with your actual data retrieval logic
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[9] {
                new DataColumn("OrderDate"), new DataColumn("Phone"), new DataColumn("Tier"),
                new DataColumn("Item"), new DataColumn("Rogers"), new DataColumn("BoardPaid"),
                new DataColumn("EligibleDate"), new DataColumn("Forms"), new DataColumn("Notes")
            });

            // Sample data row
            dt.Rows.Add(DateTime.Now, "555-0199", "Gold", "Phone Case", "Yes", "$50", DateTime.Now.AddMonths(6), "Completed", "N/A");

            smartphoneOrdersGrid.DataSource = dt;
            smartphoneOrdersGrid.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Logic to add a new order
            // You can collect data from input fields and insert it into your database
            // After adding, re-bind the grid to show the new data
           
            string phoneNumber = !string.IsNullOrEmpty(tb_phoneNumber.Text)? tb_phoneNumber.Text:string.Empty;
            DateTime? selectedOrderDateTime = null;

            if (DateTime.TryParse(tb_orderDate.Text, out DateTime parsedDate))
            {
                selectedOrderDateTime = parsedDate;
            }

            string selectedTier = ddl_tier.SelectedValue;
            string selectedItem = ddl_orderedItem.SelectedValue;
            bool? isRogersYesSelected = null;
            bool? isBoardYesSelected = null;

            if (rbl_RogersYesNo.SelectedIndex != -1)
            {
                isRogersYesSelected = Convert.ToBoolean(rbl_RogersYesNo.SelectedValue);
                // Use the boolean value here
            }

            if (rbl_BoardYesNo.SelectedIndex != -1)
            {
                isBoardYesSelected = Convert.ToBoolean(rbl_BoardYesNo.SelectedValue);
                // Use the boolean value here
            }

            DateTime? selectedEligibleDateTime = null;

            if (DateTime.TryParse(tb_eligibleDate.Text, out DateTime parsedDate2))
            {
                selectedEligibleDateTime = parsedDate2;
            }



            BindGrid();
        }

        protected void ddl_tier_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void ddl_orderedItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}