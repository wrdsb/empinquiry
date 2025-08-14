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
            if (string.IsNullOrEmpty(name))
                return;

            if (DateTime.Now.Month > 8 && DateTime.Now.Month <= 12)
                schoolyear = DateTime.Now.ToString("yyyy") + DateTime.Now.AddYears(1).ToString("yyyy");
            else
                schoolyear = DateTime.Now.AddYears(-1).ToString("yyyy") + DateTime.Now.ToString("yyyy");

            query = string.Format(@"SELECT TOP 10 i.victim_surname, i.victim_firstname, i.victim_grade, 
                    c.name AS cause_name,
                    i.date_occurred, l.name AS location_name
                    FROM incidents i
                    LEFT JOIN causes c
                    ON i.cause=c.value
                    LEFT JOIN location_types l
                    ON i.location_type=l.value
                    WHERE (i.victim_surname like '%{0}%'
                    OR i.victim_firstname like '%{0}%')
                    AND school_year='{1}'
                    ORDER BY i.date_occurred desc", name, schoolyear);

            DataSource_incidents.SelectCommand = query;
            lv_incidents.DataBind();
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Clear();

        }
    }
}