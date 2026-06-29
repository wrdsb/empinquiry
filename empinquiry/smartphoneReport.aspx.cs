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
            }
        }
        private void BindGrid()
        {
            GetRecords();
        }
        void GetRecords()
        {

            string connString = ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString;

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = @" SELECT  employee_id                                 AS EmpId
                                            , employee_name                             AS Name
                                            , FORMAT(order_date, 'yyyy-MM-dd')          AS OrderDate
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
                                    FROM    [hd_empinquiry_smartphone] WHERE ";

                    if (dateType == "ORDER_DATE")
                        sql += " order_date BETWEEN @fromDate and @toDate ";
                    else if (dateType == "ELIGIBLE_DATE")
                        sql += " next_eligible_date BETWEEN @fromDate and @toDate ";


                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            //cmd.Parameters.AddWithValue("@EmployeeID", empId);
                            if (dateType == "ORDER_DATE" || dateType == "ELIGIBLE_DATE")
                            {
                                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                                cmd.Parameters.AddWithValue("@toDate", toDate);

                            }

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

        public DateTime? fromDate { get; set; }
        public string phoneNumber { get; set; }
        public string selectedTier { get; set; }
        public string selectedItem { get; set; }
        public string selectedRogers { get; set; }
        public string selectedBoard { get; set; }
        public DateTime? toDate { get; set; }
        public string dateType { get; set; }
       

        protected void btnView_Click(object sender, EventArgs e)
        {
            dateType = ddl_DateType.SelectedValue;
            if (DateTime.TryParse(tb_fromDate.Text, out DateTime parsedDate))
            {
                fromDate = parsedDate;
            }

            phoneNumber = !string.IsNullOrEmpty(tb_phoneNumber.Text) ? tb_phoneNumber.Text : string.Empty;

            selectedTier = ddl_tier.SelectedValue;

            selectedItem = ddl_orderedItem.SelectedValue;

            selectedRogers = ddl_RogersYesNo.SelectedValue;

            selectedBoard = ddl_BoardYesNo.SelectedValue;

            if (DateTime.TryParse(tb_toDate.Text, out DateTime parsedDate2))
            {
                toDate = parsedDate2;
            }

            if(string.IsNullOrEmpty(tb_fromDate.Text) && 
               string.IsNullOrEmpty(tb_toDate.Text))
                return;

            BindGrid();
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
            ddl_DateType.SelectedIndex = 0;

            smartphoneOrdersGrid.DataSource = null;
            smartphoneOrdersGrid.DataBind();

        }
        protected void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFormControls();
            
        }
    }
}