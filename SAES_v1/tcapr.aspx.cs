using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1
{
    public partial class tcapr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
                Response.End();
            }
            else
            {

                c_prog_campus.Attributes.Add("onblur", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                c_prog_campus.Attributes.Add("oninput", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_estatus();
                    combo_campus();
                }

            }
        }

        protected void LlenaPagina()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);
            if (Session["usuario"].ToString() == "")
            {
                Response.Redirect("Default.aspx");
            }
            string QerySelect = "select tusme_update from tuser, tusme " +
                          " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                          " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 10 ";

            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter sqladapter = new MySqlDataAdapter();

                DataSet dssql1 = new DataSet();

                MySqlCommand commandsql1 = new MySqlCommand(QerySelect, conexion);
                sqladapter.SelectCommand = commandsql1;
                sqladapter.Fill(dssql1);
                sqladapter.Dispose();
                commandsql1.Dispose();

                if (dssql1.Tables[0].Rows[0][0].ToString() == "1")
                {
                    btn_programa.Visible = true;
                }
                
            }
            catch (Exception ex)
            {
                ///logs
            }
        }
        protected void combo_estatus()
        {
            e_prog_campus.Items.Clear();
            e_prog_campus.Items.Add(new ListItem("Activo", "A"));
            e_prog_campus.Items.Add(new ListItem("Inactivo", "B"));
        }
        private DataTable GetData(MySqlCommand cmd)
        {
            DataTable dt = new DataTable();
            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString;
            MySqlConnection con = new MySqlConnection(strConnString);
            MySqlDataAdapter sda = new MySqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }
        protected void combo_campus()
        {
            search_campus.Items.Clear();
            string Query = "SELECT DISTINCT tcamp_clave Clave, tcamp_desc Campus FROM tcamp " +
                            "UNION " +
                            "SELECT DISTINCT '0','----Selecciona un Campus----' Campus  " +
                            "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaCampus = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaCampus.Load(DatosMySql, LoadOption.OverwriteChanges);
                search_campus.DataSource = TablaCampus;
                search_campus.DataValueField = "Clave";
                search_campus.DataTextField = "Campus";
                search_campus.DataBind();

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
        protected void grid_programas_bind(string campus)
        {
            string Query = "";
            Query = "SELECT tcapr_tprog_clave Clave,tprog_desc Nombre,tnive_desc Nivel, tmoda_desc Modalidad, tcapr_ind_admi Admision,tcapr_estatus Estatus_code, " +
                    "CASE WHEN tcapr_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, DATE_FORMAT(tcapr_date, '%d/%m/%Y') Fecha,tcapr_tcamp_clave c_campus " +
                    "FROM tcapr " +
                    "INNER JOIN tprog ON tprog_clave = tcapr_tprog_clave " +
                    "INNER JOIN tnive ON tnive_clave = tprog_tnive_clave " +
                    "INNER JOIN tmoda ON tmoda_clave = tprog_tmoda_clave " +
                    "WHERE tcapr_tcamp_clave = '" + campus + "' " +
                    "ORDER BY 1";
            try
            {
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(Query, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Programas");
                GridProgramas.DataSource = ds;
                GridProgramas.EditIndex = -1;
                GridProgramas.DataBind();
                GridProgramas.DataMember = "Programas";
                GridProgramas.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridProgramas.UseAccessibleHeader = true;
                GridProgramas.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
        }
        protected bool validar_clave_programa(string clave)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tprog WHERE tprog_clave='" + clave + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            if (dt.Rows[0]["Indicador"].ToString() != "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void search_campus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (search_campus.SelectedValue != "0")
            {
                c_prog_campus.Text = null;
                n_prog_campus.Text = null;
                grid_programas_bind(search_campus.SelectedValue);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                guardar_prog.Visible = true;
                update_prog.Visible = false;

            }
            else
            {
                btn_programa.Visible = false;
                GridProgramas.Visible = false;
            }

        }
        protected void c_prog_campus_TextChanged(object sender, EventArgs e)
        {
            if (!validar_clave_programa(c_prog_campus.Text))
            {
                string Query = "SELECT DISTINCT tprog_desc Programa FROM tprog WHERE tprog_clave='" + c_prog_campus.Text + "'";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);

                ConexionMySql.Open();
                MySqlDataAdapter mysqladapter = new MySqlDataAdapter();
                DataSet dsmysql = new DataSet();
                MySqlCommand cmdmysql = new MySqlCommand(Query, ConexionMySql);
                mysqladapter.SelectCommand = cmdmysql;
                mysqladapter.Fill(dsmysql);
                mysqladapter.Dispose();
                cmdmysql.Dispose();
                ConexionMySql.Close();
                n_prog_campus.Text = dsmysql.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                n_prog_campus.Text = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar_clave", "validarclavePrograma_N('ContentPlaceHolder1_c_prog_campus',1);", true);
            }


        }
        protected void GridProgramas_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridProgramas.SelectedRow;
            search_campus.SelectedValue = row.Cells[9].Text;
            c_prog_campus.Text = row.Cells[1].Text;
            n_prog_campus.Text = HttpUtility.HtmlDecode(row.Cells[2].Text); ;
            combo_estatus();
            e_prog_campus.SelectedValue = row.Cells[6].Text;
            if (row.Cells[5].Text=="S")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar_check", "activar_check();", true);
            }
            guardar_prog.Visible = false;
            update_prog.Visible = true;
            grid_programas_bind(search_campus.SelectedValue);
        }

        protected void cancelar_prog_Click(object sender, EventArgs e)
        {
            combo_campus();
            c_prog_campus.Text = null;
            n_prog_campus.Text = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check", "desactivar_check();", true);
            combo_estatus();
            guardar_prog.Visible = true;
            update_prog.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            GridProgramas.Visible = false;

        }

        protected void guardar_prog_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(c_prog_campus.Text))
            {
                string admision = "";
                string selected = Request.Form["customSwitches"];
                string campus = search_campus.SelectedValue;
                if (selected == "on") { admision = "S"; } else { admision = "N"; }
                string Query = "INSERT INTO tcapr Values ('" + search_campus.SelectedValue + "','" + c_prog_campus.Text + "','" + admision + "','" + Session["usuario"].ToString() + "',  current_timestamp() ,'" + e_prog_campus.SelectedValue + "')";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    combo_campus();
                    c_prog_campus.Text = null;
                    n_prog_campus.Text = null;
                    search_campus.SelectedValue = campus;
                    grid_programas_bind(campus);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar_campos_campus", "validar_campos_campus();", true);
            }
        }

        protected void update_prog_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(c_prog_campus.Text))
            {
                string admision = "";
                string selected = Request.Form["customSwitches"];
                string campus = search_campus.SelectedValue;
                if (selected == "on") { admision = "S"; } else { admision = "N"; }
                string Query = "UPDATE tcapr SET tcapr_ind_admi='" + admision + "',tcapr_estatus='" + e_prog_campus.SelectedValue + "',tcapr_date= current_timestamp(),tcapr_tuser_clave='" + Session["usuario"].ToString() + "' WHERE tcapr_tcamp_clave='" + search_campus.SelectedValue + "' AND tcapr_tprog_clave='" + c_prog_campus.Text + "'";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    combo_campus();
                    c_prog_campus.Text = null;
                    n_prog_campus.Text = null;
                    search_campus.SelectedValue = campus;
                    grid_programas_bind(campus);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar_campos_campus", "validar_campos_campus();", true);
            }
        }
    }
}