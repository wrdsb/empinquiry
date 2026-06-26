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
    public partial class smartphoneReport : System.Web.UI.Page
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
                //string empId = Session["selectedEmpId"].ToString();
                //string empName = $"{Session["selectedFirstname"]} {Session["selectedSurname"]}";
                //lblSelectedEmployee.Text = $"Selected Employee Id : {empId} | Name : {empName}";
                //lblinfo.Text = "Please fill out the form below to add a new smartphone order for the selected employee. After submitting, the order will be displayed in the grid below.";

                BindGrid();

            }



        }
        private void BindGrid()
        {
            //GetRecords();
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
                    string sql = @" SELECT  FORMAT(order_date, 'yyyy-MM-dd')            AS OrderDate
                                            , phone_number                              AS Phone
                                            , tier                                      AS Tier
                                            , ordered_item                              AS Item
                                            , CASE
                                                WHEN rogers_account_created = 1 
                                                THEN 'Yes'
                                                ELSE 'No'
                                              END                                       AS Rogers
                                            , CASE
                                                WHEN board_contribution_paid = 1 
                                                THEN 'Yes'
                                                ELSE 'No'
                                              END                                       AS BoardPaid
                                            , FORMAT(next_eligible_date, 'yyyy-MM-dd')  AS EligibleDate
                                            , form_link                                 AS Forms
                                            , notes                                     AS Notes
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
                
            }
            else
            {
                
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


        protected void btnView_Click(object sender, EventArgs e)
        {
            // Logic to add a new order
            // You can collect data from input fields and insert it into your database
            // After adding, re-bind the grid to show the new data

            //MessageBox.Show("update : " + Session["update"]);

            if (DateTime.TryParse(tb_fromDate.Text, out DateTime parsedDate))
            {
                selectedOrderDate = parsedDate;
            }

            phoneNumber = !string.IsNullOrEmpty(tb_phoneNumber.Text) ? tb_phoneNumber.Text : string.Empty;

            selectedTier = ddl_tier.SelectedValue;

            selectedItem = ddl_orderedItem.SelectedValue;

            isRogersYesSelected = ddl_RogersYesNo.SelectedIndex != -1 ? ddl_RogersYesNo.SelectedValue : string.Empty;

            isBoardYesSelected = ddl_BoardYesNo.SelectedIndex != -1 ? ddl_BoardYesNo.SelectedValue : string.Empty;

            if (DateTime.TryParse(tb_toDate.Text, out DateTime parsedDate2))
            {
                selectedEligibleDateTime = parsedDate2;
            }

            

            // logic to save the collected data to the database
            if (Convert.ToBoolean(Session["update"]))
            {
                updateRecords();
                
            }
            else
            {
                if (SaveRecords())
                {
                    
                }
            }
            // Register the JavaScript function to run after the page loads
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", "hideLabel();", true);

            BindGrid();
            ClearFormControls();

        }
        void updateRecords()
        {
            try
            {
                string sql = @"
                                UPDATE  hd_empinquiry_smartphone
                                SET     order_date = @OrderDate
                                        , phone_number = @PhoneNumber
                                        , tier = @Tier
                                        , ordered_item = @OrderedItem
                                        , rogers_account_created = @RogersAccountCreated
                                        , board_contribution_paid = @BoardContributionPaid
                                        , next_eligible_date = @NextEligibleDate
                                        , notes = @Notes
                                WHERE   Id = @Id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {

                        cmd.Parameters.AddWithValue("@OrderDate", selectedOrderDate);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@Tier", selectedTier);
                        cmd.Parameters.AddWithValue("@OrderedItem", selectedItem);
                        cmd.Parameters.AddWithValue("@RogersAccountCreated", isRogersYesSelected);
                        cmd.Parameters.AddWithValue("@BoardContributionPaid", isBoardYesSelected);
                        cmd.Parameters.AddWithValue("@NextEligibleDate", selectedEligibleDateTime);
                        cmd.Parameters.AddWithValue("@Notes", notes);
                        cmd.Parameters.AddWithValue("@Id", hfId.Value);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();


                    }
                }
            }
            catch (Exception e) { }
        }

        bool SaveRecords()
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
                        cmd.Parameters.AddWithValue("@Notes", notes);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }

            catch (Exception ex)
            {
               
                tb_fromDate.Focus();
                return false;
            }
            return true;

        }
      

        protected void ddl_tier_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddl_orderedItem_SelectedIndexChanged(object sender, EventArgs e)
        {
      
        }
        private void ClearFormControls()
        {
            tb_fromDate.Text = string.Empty;
            tb_phoneNumber.Text = string.Empty;
            tb_toDate.Text = string.Empty;
            
            ddl_tier.SelectedIndex = 0;
            ddl_orderedItem.SelectedIndex = 0;

            ddl_RogersYesNo.SelectedIndex = 0;
            ddl_BoardYesNo.SelectedIndex = 0;

            Session["update"] = false;
            hfId.Value = "";

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
                                    tb_fromDate.Text =
                                        Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd");
                                    tb_phoneNumber.Text = reader["phone_number"].ToString();
                                    ddl_tier.SelectedValue = reader["tier"].ToString();
                                    ddl_orderedItem.SelectedValue = reader["ordered_item"].ToString();
                                    ddl_RogersYesNo.SelectedValue =
                                        reader["rogers_account_created"].ToString() == "True" ? "1" : "0";
                                    ddl_BoardYesNo.SelectedValue =
                                        reader["board_contribution_paid"].ToString() == "True" ? "1" : "0";
                                    tb_toDate.Text =
                                        Convert.ToDateTime(reader["next_eligible_date"])
                                        .ToString("yyyy-MM-dd");
                                    
                                }
                            }
                        }
                    }
                }
                hfId.Value = id.ToString();
                Session["update"] = true;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int rowIndex = Convert.ToInt32(btn.CommandArgument);

            try
            {
                int id = Convert.ToInt32(
                    smartphoneOrdersGrid.DataKeys[rowIndex].Value);

                string sql = @"DELETE FROM hd_empinquiry_smartphone WHERE Id = @Id";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                BindGrid();
                

            }
            catch (Exception ex) { }
        }

        protected void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFormControls();
            
        }
    }
}