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

        //search students where school code is this school code and bind to list view incidents
        protected void btn_search_Click(object sender, EventArgs e)
        {
            showReportData();
        }

       

        protected void showReportData()
        {
            string query = "";
            string schoolyear = "";
            string name = tb_name.Text;
            string knownas = tb_preferredname.Text;
            string postalcode = tb_postalcode.Text;
            string pal = tb_pal.Text;
            string email = tb_email.Text;
            string phone = tb_phone.Text;
            

            if (string.IsNullOrEmpty(name) &&
                string.IsNullOrEmpty(knownas) &&
                string.IsNullOrEmpty(postalcode) &&
                string.IsNullOrEmpty(pal) &&
                string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(phone))
                return;

            if (DateTime.Now.Month > 8 && DateTime.Now.Month <= 12)
                schoolyear = DateTime.Now.ToString("yyyy") + DateTime.Now.AddYears(1).ToString("yyyy");
            else
                schoolyear = DateTime.Now.AddYears(-1).ToString("yyyy") + DateTime.Now.ToString("yyyy");

            //query = string.Format(@"SELECT TOP 10 i.victim_surname, i.victim_firstname, i.victim_grade, 
            //        c.name AS cause_name,
            //        i.date_occurred, l.name AS location_name
            //        FROM incidents i
            //        LEFT JOIN causes c
            //        ON i.cause=c.value
            //        LEFT JOIN location_types l
            //        ON i.location_type=l.value
            //        WHERE (i.victim_surname like '%{0}%'
            //        OR i.victim_firstname like '%{0}%')
            //        AND school_year='{1}'
            //        ORDER BY i.date_occurred desc", name, schoolyear);

            query = @"SELECT employee_id,
                    surname,
                    first_name,
                    known_as,
                    postal_code,
                    telephone_no,
                    emp_activity_code,
                    review_date,          
                    e_mail_address,       
                    user_id 
                    FROM ec_employee 
                    WHERE ";
                    query += string.IsNullOrEmpty(name) ? "" : "(surname LIKE '%" + name + "%' OR first_name LIKE '%" + name + "%') ";
                    query += string.IsNullOrEmpty(knownas) ? "" : (string.IsNullOrEmpty(name) ? "" : " AND ") + " known_as LIKE '%" + knownas + "%' ";
                    query += string.IsNullOrEmpty(postalcode) ? "" : (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(knownas) ? "" : " AND ") + " postal_code LIKE '%" + postalcode + "%' ";
                    query += string.IsNullOrEmpty(pal) ? "" : (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(knownas) && string.IsNullOrEmpty(postalcode) ? "" : " AND ") + " user_id LIKE '%" + pal + "%' ";  
                    query += string.IsNullOrEmpty(email) ? "" : (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(knownas) && string.IsNullOrEmpty(postalcode) && string.IsNullOrEmpty(pal) ? "" : " AND ") + " e_mail_address LIKE '%" + email + "%' ";
                    query += string.IsNullOrEmpty(phone) ? "" : (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(knownas) && string.IsNullOrEmpty(postalcode) && string.IsNullOrEmpty(pal) && string.IsNullOrEmpty(email) ? "" : " AND ") + " telephone_no LIKE '%" + phone + "%' ";





            DataSource_incidents.SelectCommand = query;
            lv_incidents.DataBind();
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Clear();

        }
    }
}