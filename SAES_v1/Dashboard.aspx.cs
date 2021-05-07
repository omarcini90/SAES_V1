using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public string labels_dashboard_1;
        public string data_dashboard_1;
        public string label_dashboard_1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                combo_periodo();
                combo_campus();
                dashboard_1();
                
            }
            
        }


        protected void combo_periodo()
        {
            ddl_periodo.Items.Clear();
            ddl_campus.Items.Clear();
            ddl_campus.Items.Add(new ListItem("----Selecciona un Campus----", "0"));
            ddl_nivel.Items.Clear();
            ddl_nivel.Items.Add(new ListItem("----Selecciona un Nivel----", "0"));


            string Query = "SELECT tpees_clave Clave,tpees_desc Nombre FROM tpees " +
                          "UNION " +
                          "SELECT '0' Clave,'----Selecciona un Periodo----' Nombre " +
                          "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaEstado = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaEstado.Load(DatosMySql, LoadOption.OverwriteChanges);

                ddl_periodo.DataSource = TablaEstado;
                ddl_periodo.DataValueField = "Clave";
                ddl_periodo.DataTextField = "Nombre";
                ddl_periodo.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            finally
            {
                ConexionMySql.Close();
            }
        }

        protected void ddl_periodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            dashboard_1();
        }

        protected void combo_campus()
        {
            string Query = "SELECT tcamp_clave Clave, tcamp_desc Nombre FROM tcamp " +
                          "UNION " +
                          "SELECT '0' Clave,'----Selecciona un Campus----' Nombre " +
                          "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaEstado = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaEstado.Load(DatosMySql, LoadOption.OverwriteChanges);

                ddl_campus.DataSource = TablaEstado;
                ddl_campus.DataValueField = "Clave";
                ddl_campus.DataTextField = "Nombre";
                ddl_campus.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            finally
            {
                ConexionMySql.Close();
            }
        }

        protected void ddl_campus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_campus.SelectedValue != "0")
            {
                combo_nivel(ddl_campus.SelectedValue);
                dashboard_1();
            }
        }

        protected void combo_nivel(string campus)
        {
            string Query = "SELECT DISTINCT tnive_clave Clave, tnive_desc Nombre FROM tnive INNER JOIN tprog ON tprog_tnive_clave=tnive_clave INNER JOIN tcapr ON tcapr_tprog_clave=tprog_clave WHERE tcapr_tcamp_clave='" + campus + "'" +
                          "UNION " +
                          "SELECT '0' Clave,'----Selecciona un Nivel----' Nombre " +
                          "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaEstado = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaEstado.Load(DatosMySql, LoadOption.OverwriteChanges);

                ddl_nivel.DataSource = TablaEstado;
                ddl_nivel.DataValueField = "Clave";
                ddl_nivel.DataTextField = "Nombre";
                ddl_nivel.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            finally
            {
                ConexionMySql.Close();
            }
        }

        protected void ddl_nivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            dashboard_1();
        }

        protected void dashboard_1()
        {
            string labels = null;
            string label = null;
            string data = null;
            string Query = "";
            if (ddl_periodo.SelectedValue == "0" && ddl_campus.SelectedValue == "0" && ddl_nivel.SelectedValue == "0")
            {
                Query = "SELECT testu_tpees_clave periodo,COUNT(testu_tpers_num) alumnos " +
                            "FROM testu " +
                            "WHERE testu_tstal_clave = 'AC' AND SUBSTRING(testu_tpees_clave,1,4)>= (DATE_FORMAT(current_timestamp(), '%Y') - 1) " +
                            "GROUP BY testu_tpees_clave " +
                            "ORDER BY 1";
                label = "'Alumnos'";
            }
            else if (ddl_periodo.SelectedValue != "0" && ddl_campus.SelectedValue == "0" && ddl_nivel.SelectedValue == "0")
            {
                Query = "SELECT tcamp_desc campus,COUNT(testu_tpers_num) alumnos " +
                            "FROM testu " +
                            "INNER JOIN tcamp ON tcamp_clave = testu_tcamp_clave " +
                            "WHERE testu_tstal_clave = 'AC' AND SUBSTRING(testu_tpees_clave,1,4)>= (DATE_FORMAT(current_timestamp(), '%Y') - 1) " +
                            "AND testu_tpees_clave = '" + ddl_periodo.SelectedValue + "' " +
                            "GROUP BY tcamp_desc " +
                            "ORDER BY 1 ";
                label = "'"+ ddl_periodo.SelectedItem.Text+ "'";
            }
            else if (ddl_periodo.SelectedValue != "0" && ddl_campus.SelectedValue != "0" && ddl_nivel.SelectedValue == "0")
            {
                Query = "SELECT tnive_desc nivel,COUNT(testu_tpers_num) alumnos  " +
                            "FROM testu " +
                            "INNER JOIN tcamp ON tcamp_clave = testu_tcamp_clave " +
                            "INNER JOIN tcapr ON tcapr_tcamp_clave = tcamp_clave and tcapr_tprog_clave = testu_tprog_clave " +
                            "INNER JOIN tprog ON tprog_clave = tcapr_tprog_clave and tprog_clave = testu_tprog_clave " +
                            "INNER JOIN tnive on tnive_clave = tprog_tnive_clave " +
                            "WHERE testu_tstal_clave = 'AC'  AND SUBSTRING(testu_tpees_clave,1,4)>= (DATE_FORMAT(current_timestamp(), '%Y') - 1) " +
                            "AND testu_tpees_clave = '" + ddl_periodo.SelectedValue + "' AND tcamp_clave = '" + ddl_campus.SelectedValue + "' " +
                            "GROUP BY tnive_desc " +
                            "ORDER BY 1 ";
                label = "'"+ ddl_periodo.SelectedValue + "-" + ddl_campus.SelectedItem.Text + "'";
            }
            else if (ddl_periodo.SelectedValue != "0" && ddl_campus.SelectedValue != "0" && ddl_nivel.SelectedValue != "0")
            {
                Query = "SELECT tnive_desc nivel,COUNT(testu_tpers_num) alumnos  " +
                           "FROM testu " +
                           "INNER JOIN tcamp ON tcamp_clave = testu_tcamp_clave " +
                           "INNER JOIN tcapr ON tcapr_tcamp_clave = tcamp_clave and tcapr_tprog_clave = testu_tprog_clave " +
                           "INNER JOIN tprog ON tprog_clave = tcapr_tprog_clave and tprog_clave = testu_tprog_clave " +
                           "INNER JOIN tnive on tnive_clave = tprog_tnive_clave " +
                           "WHERE testu_tstal_clave = 'AC'  AND SUBSTRING(testu_tpees_clave,1,4)>= (DATE_FORMAT(current_timestamp(), '%Y') - 1) " +
                           "AND testu_tpees_clave = '" + ddl_periodo.SelectedValue + "' AND tcamp_clave = '" + ddl_campus.SelectedValue + "' AND tnive_clave = '" +ddl_nivel.SelectedValue+ "' "+
                           "GROUP BY tnive_desc " +
                           "ORDER BY 1 ";
                label = "'"+ddl_periodo.SelectedValue + "-" + ddl_campus.SelectedItem.Text+"-"+ddl_nivel.SelectedValue+ "'";
            }
            else
            {
                //valida campos
            }



            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            MySqlCommand myCommand = new MySqlCommand(Query, ConexionMySql);
            ConexionMySql.Open();
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            try
            {
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        labels += "\"" + myReader.GetString(0) + "\",";
                        data += myReader.GetInt32(1) + ",";
                    }

                    labels = labels.TrimEnd(',');
                    data = data.TrimEnd(',');
                    create_script(labels, label, data);
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                myReader.Close();
                ConexionMySql.Close();
            }
        }

        protected void create_script(string labels, string label, string data)
        {
            // Define the name and type of the client scripts on the page.
            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                StringBuilder cstext1 = new StringBuilder();
                cstext1.Append("<script>");
                cstext1.Append("const labels = ["+labels+"];");
                cstext1.Append("const data = {labels: labels,datasets: [{ label: "+label+",backgroundColor: 'rgba(255, 99, 132,0.1)',borderColor: 'rgb(255, 99, 132)',data: ["+data+"],fill: true,tension: 0.4}]};");
                cstext1.Append("const config = {type: 'line',data,options: {responsive: true,plugins: {title: {display: true,text: 'Alumnos activos',font: {size: 14,family: \"Raleway\"}},legend: {labels: {font: {size: 14,family: \"Raleway\"}}}}}};");
                cstext1.Append("var myChart = new Chart(document.getElementById('dashboard_1'),config);");
                cstext1.Append("</script>");

                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString());
            }
        }
    }
}