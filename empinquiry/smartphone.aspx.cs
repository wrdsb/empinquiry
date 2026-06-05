using Amazon.Runtime.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

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
                lblSelectedEmployee.Text = $"Selected Employee Id : {empId} | Name : {empName}";
                Labelinfo.Text = "Please fill out the form below to add a new smartphone order for the selected employee. After submitting, the order will be displayed in the grid below.";

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
            //dt.Rows.Add(DateTime.Now, "555-0199", "Gold", "Phone Case", "Yes", "$50", DateTime.Now.AddMonths(6), "Completed", "N/A");
            //MessageBox.Show($"Table Columns: {dt.Columns.Count} | Values Provided: 9");

            try
            {
                if (selectedOrderDate != null)
                {
                    DataRow newRow = dt.NewRow();

                    newRow["OrderDate"] = selectedOrderDate;
                    newRow["Phone"] = phoneNumber;
                    newRow["Tier"] = selectedTier;
                    newRow["Item"] = selectedItem;
                    newRow["Rogers"] = isRogersYesSelected;
                    newRow["BoardPaid"] = isBoardYesSelected;
                    newRow["EligibleDate"] = selectedEligibleDateTime;
                    newRow["Forms"] = "Forms - TBD";
                    newRow["Notes"] = notes;

                    dt.Rows.Add(newRow);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            if (dt.Rows.Count == 0)
            {
                Labellist.Text = "No data to display in the grid.";
            }
            else
            {
                Labellist.Text = "Please find the list of smartphone orders for the selected employee.";
            }


            smartphoneOrdersGrid.DataSource = dt;
            smartphoneOrdersGrid.DataBind();
        }

        public DateTime? selectedOrderDate { get; set; }
        public string phoneNumber { get; set; }
        public string selectedTier { get; set; }
        public string selectedItem { get; set; }
        public string isRogersYesSelected { get; set; }
        public string isBoardYesSelected { get; set; }
        public DateTime? selectedEligibleDateTime { get; set; }
        public string notes { get; set; }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Logic to add a new order
            // You can collect data from input fields and insert it into your database
            // After adding, re-bind the grid to show the new data

            if (DateTime.TryParse(tb_orderDate.Text, out DateTime parsedDate))
            {
                selectedOrderDate = parsedDate;
            }

            phoneNumber = !string.IsNullOrEmpty(tb_phoneNumber.Text) ? tb_phoneNumber.Text : string.Empty;

            selectedTier = ddl_tier.SelectedValue;

            selectedItem = ddl_orderedItem.SelectedValue;

            isRogersYesSelected = rbl_RogersYesNo.SelectedIndex != -1 ? rbl_RogersYesNo.SelectedValue : string.Empty;

            isBoardYesSelected = rbl_BoardYesNo.SelectedIndex != -1 ? rbl_BoardYesNo.SelectedValue : string.Empty;

            if (DateTime.TryParse(tb_eligibleDate.Text, out DateTime parsedDate2))
            {
                selectedEligibleDateTime = parsedDate2;
            }

            notes = !string.IsNullOrEmpty(tb_notes.Text) ? tb_notes.Text : string.Empty;

            // TODO: Add logic to save the collected data to the database

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