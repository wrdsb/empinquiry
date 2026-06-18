using Amazon.Runtime.Documents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                lblinfo.Text = "Please fill out the form below to add a new smartphone order for the selected employee. After submitting, the order will be displayed in the grid below.";

                BindGrid();

            }

        }
        private void BindGrid()
        {
            // Replace this with your actual data retrieval logic
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[9] {
            //    new DataColumn("OrderDate"), new DataColumn("Phone"), new DataColumn("Tier"),
            //    new DataColumn("Item"), new DataColumn("Rogers"), new DataColumn("BoardPaid"),
            //    new DataColumn("EligibleDate"), new DataColumn("Forms"), new DataColumn("Notes")
            //});

            //// Sample data row
            ////dt.Rows.Add(DateTime.Now, "555-0199", "Gold", "Phone Case", "Yes", "$50", DateTime.Now.AddMonths(6), "Completed", "N/A");
            ////MessageBox.Show($"Table Columns: {dt.Columns.Count} | Values Provided: 9");

            //try
            //{
            //    if (selectedOrderDate != null)
            //    {
            //        DataRow newRow = dt.NewRow();

            //        newRow["OrderDate"] = selectedOrderDate;
            //        newRow["Phone"] = phoneNumber;
            //        newRow["Tier"] = selectedTier;
            //        newRow["Item"] = selectedItem;
            //        newRow["Rogers"] = isRogersYesSelected;
            //        newRow["BoardPaid"] = isBoardYesSelected;
            //        newRow["EligibleDate"] = selectedEligibleDateTime;
            //        newRow["Forms"] = "Forms - TBD";
            //        newRow["Notes"] = notes;

            //        dt.Rows.Add(newRow);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}

            GetRecords();



        }
        void GetRecords()
        {
            string empId = Session["selectedEmpId"].ToString();

            string connString = ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString;

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = @" SELECT  order_date AS OrderDate
                                            , phone_number                          AS Phone
                                            , tier                                  AS Tier
                                            , ordered_item                          AS Item
                                            , CASE
                                                WHEN rogers_account_created = 1 
                                                THEN 'Yes'
                                                ELSE 'No'
                                              END                                   AS Rogers
                                            , CASE
                                                WHEN board_contribution_paid = 1 
                                                THEN 'Yes'
                                                ELSE 'No'
                                              END                                   AS BoardPaid
                                            , next_eligible_date                    AS EligibleDate
                                            , form_link                             AS Forms
                                            , notes                                 AS Notes
                                            , Id
                                    FROM    [hd_empinquiry_smartphone]
                                    WHERE   employee_id = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", empId);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            smartphoneOrdersGrid.DataSource = dt;
            smartphoneOrdersGrid.DataBind();

            if (dt.Rows.Count == 0)
            {
                lbllist.Text = "No data to display in the grid.";
            }
            else
            {
                lbllist.Text = "Please find the smartphone orders displayed in the grid for the selected employee.";
            }
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

            // logic to save the collected data to the database

            SaveRecords();

            BindGrid();
            ClearFormControls();
            lblsubmit.Text = "Submitted successfully!";

            lblsubmit.Style["display"] = "block"; // Ensure it is visible if previously hidden

            // Register the JavaScript function to run after the page loads
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", "hideLabel();", true);
        }

        void SaveRecords()
        {
            try
            {
               
                string sql = @"INSERT INTO [hd_empinquiry_smartphone]
                (
                    employee_id
                    , employee_name
                    , order_date
                    , phone_number
                    , tier
                    , ordered_item
                    , rogers_account_created
                    , board_contribution_paid
                    , next_eligible_date
                    , form_link
                    , notes
                )
                VALUES
                (
                    @EmployeeID,
                    @EmployeeName,
                    @OrderDate,
                    @PhoneNumber,
                    @Tier,
                    @OrderedItem,
                    @RogersAccountCreated,
                    @BoardContributionPaid,
                    @NextEligibleDate,
                    @FormLink,
                    @Notes
                )";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", Session["selectedEmpId"].ToString());
                        cmd.Parameters.AddWithValue("@EmployeeName", $"{Session["selectedSurname"]}, {Session["selectedFirstname"]}");
                        cmd.Parameters.AddWithValue("@OrderDate", selectedOrderDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@Tier", selectedTier);
                        cmd.Parameters.AddWithValue("@OrderedItem", selectedItem);
                        cmd.Parameters.AddWithValue("@RogersAccountCreated", isRogersYesSelected);
                        cmd.Parameters.AddWithValue("@BoardContributionPaid", isBoardYesSelected);
                        cmd.Parameters.AddWithValue("@NextEligibleDate", selectedEligibleDateTime);
                        cmd.Parameters.AddWithValue("@FormLink", "");
                        cmd.Parameters.AddWithValue("@Notes", tb_notes.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            
            catch (Exception ex)
            {

            }

        }

        protected void ddl_tier_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddl_orderedItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ClearFormControls()
        {
            tb_orderDate.Text = string.Empty;
            tb_phoneNumber.Text = string.Empty;
            tb_eligibleDate.Text = string.Empty;
            tb_notes.Text = string.Empty;

            ddl_tier.SelectedIndex = 0;
            ddl_orderedItem.SelectedIndex = 0;

            rbl_RogersYesNo.SelectedIndex = -1;
            rbl_BoardYesNo.SelectedIndex = -1;

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            int rowIndex = Convert.ToInt32(btn.CommandArgument);

            try
            {


                int id = Convert.ToInt32(
                    smartphoneOrdersGrid.DataKeys[rowIndex].Value);

                // Use this id to retrieve the row from the database
                string sql = @" SELECT *
                                FROM hd_empinquiry_smartphone
                                WHERE Id = @Id";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    tb_orderDate.Text =
                                        Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd");
                                    tb_phoneNumber.Text = reader["phone_number"].ToString();
                                    ddl_tier.SelectedValue = reader["tier"].ToString();
                                    ddl_orderedItem.SelectedValue = reader["ordered_item"].ToString();
                                    rbl_RogersYesNo.SelectedValue =
                                        reader["rogers_account_created"].ToString() == "True"? "1":"0";
                                    rbl_BoardYesNo.SelectedValue =
                                        reader["board_contribution_paid"].ToString() == "True"? "1":"0";
                                    tb_eligibleDate.Text =
                                        Convert.ToDateTime(reader["next_eligible_date"])
                                        .ToString("yyyy-MM-dd");
                                    tb_notes.Text = reader["notes"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}