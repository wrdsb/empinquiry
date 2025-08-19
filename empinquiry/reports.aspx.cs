using System;
using System.Collections.Generic;
using System.Linq;
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

            if (string.IsNullOrEmpty(surname) &&
                string.IsNullOrEmpty(knownas) &&
                string.IsNullOrEmpty(pal) &&
                string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(phone) &&
                string.IsNullOrEmpty(empid) &&
                string.IsNullOrEmpty(firstname))
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
                    usr.user_id 
                    FROM ec_employee emp 
                    INNER JOIN hd_ec_users usr
                    ON (emp.EMPLOYEE_ID = usr.EMPLOYEE_ID)
                    WHERE ";
            query += string.IsNullOrEmpty(firstname) ? "" : "emp.first_name LIKE '%" + firstname + "%' AND ";
            query += string.IsNullOrEmpty(surname) ? "" : "emp.surname LIKE '%" + surname + "%' AND ";
            query += string.IsNullOrEmpty(knownas) ? "" : "emp.known_as_first LIKE '%" + knownas + "%' AND ";
            query += string.IsNullOrEmpty(pal) ? "" : "usr.user_id LIKE '%" + pal + "%' AND ";
            query += string.IsNullOrEmpty(email) ? "" : "emp.e_mail_address LIKE '%" + email + "%' AND ";
            query += string.IsNullOrEmpty(phone) ? "" : "emp.telephone_no LIKE '%" + phone + "%' AND ";
            query += string.IsNullOrEmpty(area) ? "" : "emp.telephone_area LIKE '%" + area + "%' AND ";
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

            DataSource_incidents.SelectCommand = query;
            lv_incidents.DataBind();
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Clear();

        }
    }
}