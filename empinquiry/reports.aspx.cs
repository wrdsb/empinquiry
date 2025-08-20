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
                    area = phone.Substring(0,3);
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

            if (string.IsNullOrEmpty(surname) &&
                string.IsNullOrEmpty(knownas) &&
                string.IsNullOrEmpty(pal) &&
                string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(phone) &&
                string.IsNullOrEmpty(empid) &&
                string.IsNullOrEmpty(firstname) &&
                string.IsNullOrEmpty(job))
                return;

            query = @"SELECT 
                    emp.employee_id,
                    emp.surname,
                    emp.first_name,
                    emp.known_as_first,
                    emp.postal_code,
                    emp.telephone_area,
                    emp.telephone_no,
                    emp.emp_activity_code,
                    emp.review_date,          
                    emp.e_mail_address,  
                    job.description_text,
                    usr.user_id 
                    FROM ec_employee emp 
                    INNER JOIN hd_ec_users usr
                    ON (emp.employee_id = usr.employee_id)     
                    INNER JOIN ec_employee_positions empos
                    ON (emp.employee_id = empos.employee_id) 
                    INNER JOIN ec_jobs job
                    ON (empos.job_code = job.job_code) 
                    WHERE empos.home_location_ind = 'Y' 
                    AND empos.position_start_date <= getdate()
                    AND (empos.position_end_date >= getdate() or empos.position_end_date is null) AND ";
            query += string.IsNullOrEmpty(firstname) ? "" : "emp.first_name LIKE '%" + firstname + "%' AND ";
            query += string.IsNullOrEmpty(surname) ? "" : "emp.surname LIKE '%" + surname + "%' AND ";
            query += string.IsNullOrEmpty(knownas) ? "" : "emp.known_as_first LIKE '%" + knownas + "%' AND ";
            query += string.IsNullOrEmpty(pal) ? "" : "usr.user_id LIKE '%" + pal + "%' AND ";
            query += string.IsNullOrEmpty(email) ? "" : "emp.e_mail_address LIKE '%" + email + "%' AND ";
            query += string.IsNullOrEmpty(phone) ? "" : "emp.telephone_no LIKE '%" + phone + "%' AND ";
            query += string.IsNullOrEmpty(area) ? "" : "emp.telephone_area LIKE '%" + area + "%' AND ";
            query += string.IsNullOrEmpty(job) ? "" : "job.description_abbr = '" + job + "' AND ";
            query += string.IsNullOrEmpty(empid) ? "" : "emp.employee_id ='" + empid + "'";

            string andAtEnd = query.Substring(query.Length - 4, 3);
            if (andAtEnd == "AND")
            {
                var lastIndex = query.LastIndexOf("AND");
                if (lastIndex > 0)
                {
                    query = query.Remove(lastIndex, 3); // Remove the last "AND"
                }
            }

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
                string empid = e.CommandArgument.ToString();

                string query = ""; 
                string leaveStartDate =string.Empty;
                string leaveEndDate = string.Empty;
                // Query to get leave details for the employee
                query = string.Format("SELECT " +
                    "leave_start_date," +
                    "leave_end_date " +
                    "FROM ec_employee_leaves " +
                    "WHERE leave_start_date <= getdate() " +
                    "AND (leave_end_date >= getdate() or leave_end_date is null) " +
                    "AND employee_id = {0}", empid);

                string connString = ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        leaveStartDate = reader["leave_start_date"].ToString().Trim();
                        leaveEndDate = reader["leave_end_date"].ToString().Trim();
                        
                    }
                }
                else
                {
                    throw new Exception("Incorrect Value or Format.");
                }
                reader.Close();
                con.Close();

                string formatstartdate = Convert.ToDateTime(leaveStartDate).ToString("MMMM dd, yyyy");
                string formatenddate = Convert.ToDateTime(leaveEndDate).ToString("MMMM dd, yyyy");


                string detailsHtml = $"<table class='table table-sm'>" +
                                     $"<tr><td>Employee ID</td><td>{empid}</td></tr>" +
                                     $"<tr><td>Leave start date</td><td>{formatstartdate}</td></tr>" +
                                     $"<tr><td>Leave end date</td><td>{formatenddate}</td></tr>" +
                                     $"</table>";

                // Inject into a Literal or Modal placeholder
                litDetails.Text = detailsHtml;

                // Show modal (custom CSS modal)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal",
                    "document.getElementById('detailsModal').style.display = 'block';", true);
            }
        }
    }
}