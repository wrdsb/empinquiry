using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace empinquiry
{
    public partial class reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
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


        protected void btn_search_Click(object sender, EventArgs e)
        {
            //Loggers.Log("Performing search operation from reports page by user: " + Session["username"]);
            btn_search.Focus();
            
            if(!GenerateQuery())
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter at least one search criteria.');", true);
                return;
            }
            if (btn_search.Text == Resource.NextInquiry)
            {
                //Loggers.Log("Redirecting to default.aspx for next inquiry by user: " + Session["username"]);
                Response.Redirect("default.aspx");
                return;
            }
            btn_clear.Enabled = false;
            showSearchData();
            BindTotalRecordCount();
            btn_search.Text = Resource.NextInquiry;
            Session["auditComplete"] = null;// reset audit flag to force re-login for next inquiry
            lbl_homeloc.Visible = true;
            ch_home_location.Visible = true;

        }

        bool GenerateQuery()
        {
            //Loggers.Log("Building search query from reports page by user: " + Session["username"]);

            try
            {
                string query = "";
                string firstname = tb_firstname.Text;
                string surname = tb_surname.Text;
                string knownasfirstname = tb_preferredfirstname.Text;
                string pal = tb_pal.Text;
                string email = tb_email.Text;
                string phone = tb_phone.Text;
                string area = string.Empty;
                string knownassurname = tb_preferredsurname.Text;
                if (phone.Length > 0) // work around to split area code from phone number
                {
                    if (phone.Length > 3)
                    {
                        area = phone.Substring(0, 3);
                        phone = phone.Substring(3);
                    }
                    else
                    {
                        area = phone;
                        phone = string.Empty;
                    }
                }

                string empid = tb_empId.Text;
                string job = ddl_job.SelectedValue;
                job = job.Replace("'", "''"); // replace single quote with double quote to avoid SQL error
                string status = ddl_status.SelectedValue;
                string formername = tb_formername.Text;
                string groupcode = tb_grpcode.Text;

                if (string.IsNullOrEmpty(surname) &&
                    string.IsNullOrEmpty(knownasfirstname) &&
                    string.IsNullOrEmpty(pal) &&
                    string.IsNullOrEmpty(email) &&
                    string.IsNullOrEmpty(phone) &&
                    string.IsNullOrEmpty(empid) &&
                    string.IsNullOrEmpty(firstname) &&
                    string.IsNullOrEmpty(job) &&
                    string.IsNullOrEmpty(status) &&
                    string.IsNullOrEmpty(formername) &&
                    string.IsNullOrEmpty(knownassurname) &&
                    string.IsNullOrEmpty(groupcode))
                    return false;

                /*
                 * WHERE empos.home_location_ind = 'Y' 
                        AND empos.position_start_date <= getdate()
                        AND (empos.position_end_date >= getdate() or empos.position_end_date is null) AND
                 * */

                query = @"SELECT 
                    emp.employee_id,
                    emp.surname,
                    emp.first_name,
                    emp.known_as_first,
                    emp.postal_code,
                    emp.telephone_area,
                    emp.telephone_no,
                    emp.emp_activity_code,
                    emp.e_mail_address,  
                    emp.former_name,
                    emp.known_as,

                    job.description_text,
                    job.job_code,

                    empos.emp_group_code,
                    empos.location_code,
                    empos.record_change_date,
                    empos.home_location_ind,

                    usr.user_id 

                    FROM ec_employee emp 
                    INNER JOIN hd_ec_users usr
                    ON (emp.employee_id = usr.employee_id)     
                    INNER JOIN ec_employee_positions empos
                    ON (emp.employee_id = empos.employee_id) 
                    INNER JOIN ec_jobs job
                    ON (empos.job_code = job.job_code) 

                    WHERE ";



                query += string.IsNullOrEmpty(firstname) ? "" : "emp.first_name LIKE '%" + firstname + "%' AND ";
                query += string.IsNullOrEmpty(surname) ? "" : "emp.surname LIKE '%" + surname + "%' AND ";
                query += string.IsNullOrEmpty(knownasfirstname) ? "" : "emp.known_as_first LIKE '%" + knownasfirstname + "%' AND ";
                query += string.IsNullOrEmpty(status) ? "" : "emp.emp_activity_code = '" + status + "' AND ";
                query += string.IsNullOrEmpty(empid) ? "" : "emp.employee_id ='" + empid + "' AND ";
                query += string.IsNullOrEmpty(email) ? "" : "emp.e_mail_address LIKE '%" + email + "%' AND ";
                query += string.IsNullOrEmpty(phone) ? "" : "emp.telephone_no LIKE '%" + phone + "%' AND ";
                query += string.IsNullOrEmpty(area) ? "" : "emp.telephone_area LIKE '%" + area + "%' AND ";
                query += string.IsNullOrEmpty(formername) ? "" : "emp.former_name LIKE '%" + formername + "%' AND ";
                query += string.IsNullOrEmpty(knownassurname) ? "" : "emp.known_as LIKE '%" + knownassurname + "%' AND ";

                query += string.IsNullOrEmpty(job) ? "" : "job.description_abbr = '" + job + "' AND ";

                query += string.IsNullOrEmpty(pal) ? "" : "usr.user_id LIKE '%" + pal + "%' AND ";

                query += string.IsNullOrEmpty(groupcode) ? "" : "empos.emp_group_code LIKE '%" + groupcode + "%' AND ";

                //query += @" empos.home_location_ind = 'Y' 
                //        AND 
                query += @" empos.position_start_date <= getdate() 
                        AND 
                        (empos.position_end_date IS NULL OR empos.position_end_date = emp.termination_date)";


                query += " ORDER BY emp.employee_id ASC";
                //Loggers.Log("Search query built: " + query);
                Global.searchQuery = query;
                return true;
            }
            catch (Exception ex)
            {
                Loggers.Log("Error occurred while building search query from reports page by user: " + Session["username"] + " . Error: " + ex.Message);
                Loggers.Log("Stack Trace: " + ex.StackTrace);
                Loggers.Log("Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A"));
                Loggers.Log("Source: " + ex.Source);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while building the search query. Please try again later.');", true);
                return false;
            }
        }

        protected void showSearchData()
        {
            try 
            {
                DataSource_search.SelectCommand = Global.searchQuery;
                lv_search.DataBind();
                lv_search.SelectedIndex = -1;             
            }
            catch(Exception ex)
            {
                Loggers.Log("Error occurred while binding search query " + Session["username"] + " . Error: " + ex.Message);
                Loggers.Log("Stack Trace: " + ex.StackTrace);
                Loggers.Log("Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A"));
                Loggers.Log("Source: " + ex.Source);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while binding search query. Please try again later.');", true);
            }

        }

        void BindTotalRecordCount()
        {
            try
            {
                SQLProvider SqlDB = new SQLProvider();
                bool success;
                DataTable dt = SqlDB.GetDataTable(Global.searchQuery, out success);
                int count = dt.Rows.Count;
                lblCount.Text = "Total Records: " + count.ToString();
            }
            catch (Exception ex)
            {
                Loggers.Log("Error occurred while fetching total record count from reports page by user: " + Session["username"] + " . Error: " + ex.Message);
                Loggers.Log("Stack Trace: " + ex.StackTrace);
                Loggers.Log("Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A"));
                Loggers.Log("Source: " + ex.Source);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while fetching total record count. Please try again later.');", true);
            }
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Redirect("reports.aspx");
        }

        protected void ddl_job_DataBound(object sender, EventArgs e)
        {
            if (ddl_job.Items.Count > 0)
            {
                ddl_job.Items.Insert(0, new ListItem("", ""));
                ddl_job.SelectedIndex = 0;
            }
        }

        protected void lv_search_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                try
                {
                    // Retrieve the single string from the CommandArgument
                    string commandArgs = e.CommandArgument.ToString();

                    // Split the string using the semicolon separator
                    string[] args = commandArgs.Split(';');

                    // Access the individual values
                    string empid = args[0];
                    string status = args[1];
                    //string groupcode = args[2];
                    string locationcode = args[2];
                    string recordchangedate = args[3];

                    //groupcode = string.IsNullOrEmpty(groupcode) ? "" : groupcode;
                    locationcode = string.IsNullOrEmpty(locationcode) ? "" : locationcode;
                    recordchangedate = string.IsNullOrEmpty(recordchangedate) ? "" : Convert.ToDateTime(recordchangedate).ToString("MMMM dd, yyyy");

                    //Loggers.Log("Fetching additional details for Employee ID: " + empid + " with status: " + status);

                    string query = "";
                    string leaveStartDate = string.Empty;
                    string leaveEndDate = string.Empty;
                    string terminationDate = string.Empty;

                    if (status == "ONLEAVE")
                    {
                        // Query to get leave details for the employee
                        query = string.Format("SELECT " +
                            "leave_start_date," +
                            "leave_end_date " +
                            "FROM ec_employee_leaves " +
                            "WHERE leave_start_date <= getdate() " +
                            "AND (leave_end_date >= getdate() or leave_end_date is null) " +
                            "AND employee_id = {0}", empid);
                    }
                    else
                    {
                        // Query to get termination details for the employee
                        query = string.Format("SELECT " +
                            "termination_date " +
                            "FROM ec_employee " +
                            "WHERE employee_id = {0}  ", empid);
                    }

                    string connString = ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString;
                    SqlConnection con = new SqlConnection(connString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (status == "ONLEAVE")
                        {
                            while (reader.Read())
                            {
                                leaveStartDate = reader["leave_start_date"].ToString().Trim();
                                leaveEndDate = reader["leave_end_date"].ToString().Trim();

                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                terminationDate = reader["termination_date"].ToString().Trim();
                            }

                        }
                    }
                    else
                    {
                        //throw new Exception("Incorrect Value or Format.");
                    }
                    reader.Close();
                    con.Close();

                    string detailsHtml = string.Empty;
                    if (status == "ONLEAVE")
                    {

                        string formatstartdate = string.IsNullOrEmpty(leaveStartDate) ? "" : Convert.ToDateTime(leaveStartDate).ToString("MMMM dd, yyyy");
                        string formatenddate = string.IsNullOrEmpty(leaveEndDate) ? "" : Convert.ToDateTime(leaveEndDate).ToString("MMMM dd, yyyy");


                        detailsHtml = $"<table class='table table-sm'>" +
                                            $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                            //$"<tr><td>Group Code</td><td>{groupcode}</td></tr>" +
                                            $"<tr><td>Location Code</td><td>{locationcode}</td></tr>" +
                                            $"<tr><td>Record change date</td><td>{recordchangedate}</td></tr>" +
                                            $"<tr><td>Leave start date</td><td>{formatstartdate}</td></tr>" +
                                            $"<tr><td>Leave end date</td><td>{formatenddate}</td></tr>" +
                                            $"</table>";
                    }
                    else if (status == "ACTIVE")
                    {
                        detailsHtml = $"<table class='table table-sm'>" +
                                             $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                             //$"<tr><td>Group Code</td><td>{groupcode}</td></tr>" +
                                             $"<tr><td>Location Code</td><td>{locationcode}</td></tr>" +
                                             $"<tr><td>Record change date</td><td>{recordchangedate}</td></tr>" +
                                             $"</table>";
                    }
                    else
                    {
                        string formatdate = string.IsNullOrEmpty(terminationDate) ? "" : Convert.ToDateTime(terminationDate).ToString("MMMM dd, yyyy");
                        detailsHtml = $"<table class='table table-sm'>" +
                                             $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                             //$"<tr><td>Group Code</td><td>{groupcode}</td></tr>" +
                                             $"<tr><td>Location Code</td><td>{locationcode}</td></tr>" +
                                             $"<tr><td>Record change date</td><td>{recordchangedate}</td></tr>" +
                                             $"<tr><td>Last official date</td><td>{formatdate}</td></tr>" +
                                             $"</table>";
                    }

                    // Inject into a Literal or Modal placeholder
                    litDetails.Text = detailsHtml;

                    // Show modal (custom CSS modal)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal",
                        "document.getElementById('detailsModal').style.display = 'block';", true);

                    showSearchData();
                    lv_search.SelectedIndex = e.Item.DataItemIndex % 25;
                }
                catch (Exception ex)
                {
                    Loggers.Log("Error occurred while fetching additional details from reports page by user: " + Session["username"] + " . Error: " + ex.Message);
                    Loggers.Log("Stack Trace: " + ex.StackTrace);
                    Loggers.Log("Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A"));
                    Loggers.Log("Source: " + ex.Source);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while fetching the details. Please try again later.');", true);
                }
            }

        }

        protected void ddl_status_DataBound(object sender, EventArgs e)
        {
            if (ddl_status.Items.Count > 0)
            {
                ddl_status.Items.Insert(0, new ListItem("", ""));
                ddl_status.SelectedIndex = 0;
            }
        }
        protected void lv_search_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            // Tell the DataPager the new page properties
            MyDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            lv_search.SelectedIndex = -1;

            // Rebind the data for the new page
            showSearchData();
        }

        protected void lv_search_Sorting(object sender, ListViewSortEventArgs e)
        {
            string sortColumn = e.SortExpression;
            if (sortColumn == "job_code")
            {
                // Determine sort direction
                string lastSortColumn = ViewState["SortColumn"] as string;
                string lastDirection = ViewState["SortDirection"] as string ?? "ASC";

                string newDirection = "ASC";

                if (sortColumn == lastSortColumn && lastDirection == "ASC")
                    newDirection = "DESC";

                // Save sort state
                ViewState["SortColumn"] = sortColumn;
                ViewState["SortDirection"] = newDirection;

                // Modify SELECT query 
                Global.searchQuery = Global.searchQuery.Split(new string[] { " ORDER BY " }, StringSplitOptions.None)[0];
                Global.searchQuery = Global.searchQuery + " ORDER BY " + sortColumn + " " + newDirection;
                Global.searchQuery = Global.searchQuery + " , emp.employee_id ASC"; // to maintain consistent order
                showSearchData();
            }
        }

        

        protected void home_location_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Global.searchQuery))
                return;
            if (ch_home_location.Checked)
            {
                Global.searchQuery = Global.searchQuery.Replace("WHERE ", "WHERE empos.home_location_ind = 'Y' AND ");
            }
            else
            {
                Global.searchQuery = Global.searchQuery.Replace("empos.home_location_ind = 'Y' AND ", " ");
            }
            showSearchData();
            BindTotalRecordCount();

        }   
    }
}