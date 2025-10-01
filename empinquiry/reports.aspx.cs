using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace empinquiry
{
    public partial class reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            showSearchData();
        }



        protected void showSearchData()
        {

            string query = "";
            string firstname = tb_firstname.Text;
            string surname = tb_surname.Text;
            string knownas = tb_preferredname.Text;
            string pal = tb_pal.Text;
            string email = tb_email.Text;
            string phone = tb_phone.Text;
            string area = string.Empty;
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

            if (string.IsNullOrEmpty(surname) &&
                string.IsNullOrEmpty(knownas) &&
                string.IsNullOrEmpty(pal) &&
                string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(phone) &&
                string.IsNullOrEmpty(empid) &&
                string.IsNullOrEmpty(firstname) &&
                string.IsNullOrEmpty(job) &&
                string.IsNullOrEmpty(status) &&
                string.IsNullOrEmpty(formername))
                return;

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

                    job.description_text,
                    job.job_code,

                    empos.emp_group_code,
                    empos.location_code,

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
            query += string.IsNullOrEmpty(knownas) ? "" : "emp.known_as_first LIKE '%" + knownas + "%' AND ";
            query += string.IsNullOrEmpty(status) ? "" : "emp.emp_activity_code = '" + status + "' AND ";
            query += string.IsNullOrEmpty(empid) ? "" : "emp.employee_id ='" + empid + "' AND ";
            query += string.IsNullOrEmpty(email) ? "" : "emp.e_mail_address LIKE '%" + email + "%' AND ";
            query += string.IsNullOrEmpty(phone) ? "" : "emp.telephone_no LIKE '%" + phone + "%' AND ";
            query += string.IsNullOrEmpty(area) ? "" : "emp.telephone_area LIKE '%" + area + "%' AND ";
            query += string.IsNullOrEmpty(formername) ? "" : "emp.former_name LIKE '%" + formername + "%' AND ";

            query += string.IsNullOrEmpty(job) ? "" : "job.description_abbr = '" + job + "' AND ";

            query += string.IsNullOrEmpty(pal) ? "" : "usr.user_id LIKE '%" + pal + "%' AND ";

            query += @" empos.home_location_ind = 'Y' 
                        AND 
                        empos.position_start_date <= getdate() 
                        AND 
                        (empos.position_end_date is null or empos.POSITION_END_DATE = emp.TERMINATION_DATE)";


            query += " ORDER BY emp.employee_id";

            DataSource_search.SelectCommand = query;
            lv_search.DataBind();
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
                // Retrieve the single string from the CommandArgument
                string commandArgs = e.CommandArgument.ToString();

                // Split the string using the semicolon separator
                string[] args = commandArgs.Split(';');

                // Access the individual values
                string empid = args[0];
                string status = args[1];


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
                    throw new Exception("Incorrect Value or Format.");
                }
                reader.Close();
                con.Close();

                string detailsHtml = string.Empty;
                if (status == "ONLEAVE")
                {

                    string formatstartdate = Convert.ToDateTime(leaveStartDate).ToString("MMMM dd, yyyy");
                    string formatenddate = Convert.ToDateTime(leaveEndDate).ToString("MMMM dd, yyyy");


                    detailsHtml = $"<table class='table table-sm'>" +
                                        $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                        $"<tr><td>Leave start date</td><td>{formatstartdate}</td></tr>" +
                                        $"<tr><td>Leave end date</td><td>{formatenddate}</td></tr>" +
                                        $"</table>";
                }
                else
                {
                    string formatdate = Convert.ToDateTime(terminationDate).ToString("MMMM dd, yyyy");
                    detailsHtml = $"<table class='table table-sm'>" +
                                         $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                         $"<tr><td>Termination date</td><td>{formatdate}</td></tr>" +
                                         $"</table>";
                }

                // Inject into a Literal or Modal placeholder
                litDetails.Text = detailsHtml;

                // Show modal (custom CSS modal)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal",
                    "document.getElementById('detailsModal').style.display = 'block';", true);
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
    }
}