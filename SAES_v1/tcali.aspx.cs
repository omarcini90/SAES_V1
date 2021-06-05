using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1
{
    public partial class tcali : System.Web.UI.Page
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

                //c_campus.Attributes.Add("onblur", "validarclaveCampus('ContentPlaceHolder1_c_campus',0)");
                //c_campus.Attributes.Add("oninput", "validarclaveCampus('ContentPlaceHolder1_c_campus',0)");
                //n_campus.Attributes.Add("onblur", "validarNombreCampus('ContentPlaceHolder1_n_campus')");
                //n_campus.Attributes.Add("oninput", "validarNombreCampus('ContentPlaceHolder1_n_campus')");
                //txt_puntos.Attributes.Add("type", "number");

                if (!IsPostBack)
                {
                    LlenaPagina();
                    combo_estatus();
                }
                
            }
        }

        private void LlenaPagina()
        {
            System.Threading.Thread.Sleep(50);

            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 4 and tusme_tmede_clave = 3 ";

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
                if (dssql1.Tables[0].Rows.Count == 0 || dssql1.Tables[0].Rows[0][1].ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "permisos", "sin_acceso();", true);
                }
                else
                {
                    if (dssql1.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        btn_tcali.Visible = true;
                    }
                    //grid_tespr_bind();
                    Nivel();
                }

            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        private void Nivel()
        {
            ddl_tnive.Items.Clear();
            string Query = "";
            Query = "SELECT DISTINCT  tnive_clave Clave, tnive_desc Nombre " +
                        "FROM tnive " +
                        " where tnive_estatus='A' " +
                        " union " +
                        " select '00' Clave, '-----' Nombre from dual " +
                        " ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaNivel = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaNivel.Load(DatosMySql, LoadOption.OverwriteChanges);
                ddl_tnive.DataSource = TablaNivel;
                ddl_tnive.DataValueField = "Clave";
                ddl_tnive.DataTextField = "Nombre";
                ddl_tnive.DataBind();

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

        protected void combo_estatus()
        {
            ddl_estatus.Items.Clear();
            ddl_estatus.Items.Add(new ListItem("Activo", "A"));
            ddl_estatus.Items.Add(new ListItem("Inactivo", "B"));
        }

        

        

        protected void grid_tcali_bind(object sender, EventArgs e)
        {
            string strQueryGrid = "";
            strQueryGrid = " select tcali_clave clave, tcali_puntos puntos, tcali_ind_aprob aprobatoria, tcali_ind_prom promedio,  " +
              " tcali_estatus c_estatus,CASE WHEN tcali_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tcali_date,'%Y-%m-%d')) fecha " +
              " from tcali where tcali_tnive_clave='" + ddl_tnive.SelectedValue + "'  order by puntos "; 
            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryGrid, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Tcali");
                Gridtcali.DataSource = ds;
                Gridtcali.DataBind();
                Gridtcali.DataMember = "Tcali";
                Gridtcali.HeaderRow.TableSection = TableRowSection.TableHeader;
                Gridtcali.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_tcali", "load_datatable_tcali();", true);
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        private void grid_tcali_bind()
        {
            string strQueryGrid = "";
            strQueryGrid = " select tcali_clave clave, tcali_puntos puntos, tcali_ind_aprob aprobatoria, tcali_ind_prom promedio,  " +
              " tcali_estatus c_estatus,CASE WHEN tcali_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tcali_date,'%Y-%m-%d')) fecha " +
              " from tcali where tcali_tnive_clave='" + ddl_tnive.SelectedValue + "'  order by puntos ";
            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryGrid, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Tcali");
                Gridtcali.DataSource = ds;
                Gridtcali.DataBind();
                Gridtcali.DataMember = "Tcali";
                Gridtcali.HeaderRow.TableSection = TableRowSection.TableHeader;
                Gridtcali.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_tcali", "load_datatable_tcali();", true);
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {


            txt_tcali.Text = null;
            txt_puntos.Text = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_aprob", "desactivar_check_aprob();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_prom", "desactivar_check_prom();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            combo_estatus();
            btn_save.Visible = true;
            btn_update.Visible = false;
            Gridtcali.Visible = false;
            Nivel();
            
            //grid_tespr_bind();
           
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string pattern = "/^[0-9]+$/";
            Regex numerico = new Regex(pattern);
            bool prueba = numerico.IsMatch(txt_puntos.Text);
            if ( ddl_tnive.SelectedValue != "00" && !String.IsNullOrEmpty(txt_tcali.Text) && !String.IsNullOrEmpty(txt_puntos.Text) && !numerico.IsMatch(txt_puntos.Text))
            {
                    string aprob = "";
                    string selected = Request.Form["aprob"];
                    if (selected == "on")
                    { aprob = "S"; }
                    else
                    { aprob = "N"; }
                    string prom = "";
                    selected = Request.Form["prom"];
                    if (selected == "on")
                    { prom = "S"; }
                    else 
                    { prom = "N"; }
                    string strCadSQL = "INSERT INTO tcali Values ('" + ddl_tnive.SelectedValue + "','" + txt_tcali.Text + "','" + 
                    txt_puntos.Text + "','" + aprob + "','" + prom + "','" + ddl_estatus.SelectedValue + "',current_timestamp(),'" +
                    Session["usuario"].ToString() + "')";
                        
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        txt_tcali.Text = null;
                        txt_puntos.Text = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_aprob", "desactivar_check_aprob();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_prom", "desactivar_check_prom();", true);
                        combo_estatus();
                        //grid_tespr_bind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
                        grid_tcali_bind();
                        
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message;
                    }
                    finally
                    {
                        conexion.Close();
                    }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcali();", true);
                //grid_tespr_bind();
            }


        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (ddl_tnive.SelectedValue != "00" && !String.IsNullOrEmpty(txt_tcali.Text) && !String.IsNullOrEmpty(txt_puntos.Text))
            {
                string aprob = "";
                string selected = Request.Form["aprob"];
                if (selected == "on")
                { aprob = "S"; }
                else
                { aprob = "N"; }
                string prom = "";
                selected = Request.Form["prom"];
                if (selected == "on")
                { prom = "S"; }
                else
                { prom = "N"; }
                string strCadSQL = "UPDATE tcali SET tcali_puntos='" + txt_puntos.Text + "', tcali_ind_aprob='" + aprob + "', tcali_ind_prom='" + prom + "',tcali_estatus='" + ddl_estatus.SelectedValue + "', tcali_user='" + Session["usuario"].ToString() + "', tcali_date=CURRENT_TIMESTAMP() WHERE tcali_tnive_clave='" + ddl_tnive.SelectedValue + "' and tcali_clave='" + txt_tcali.Text + "'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    //grid_tespr_bind();
                    txt_tcali.Text = null;
                    txt_puntos.Text = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_aprob", "desactivar_check_aprob();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_prom", "desactivar_check_prom();", true);
                    combo_estatus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
                    grid_tcali_bind();
                }
                catch (Exception ex)
                {
                    string test = ex.Message;
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcali();", true);
            }
        }

        protected bool valida_tcali(string tnive, string tcali)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tcali WHERE tcali_clave='" + tcali + "' and tcali_tnive_clave='" + tnive + "'";
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

        protected void Gridtcali_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = Gridtcali.SelectedRow;
            txt_tcali.Text = row.Cells[1].Text;
            txt_puntos.Text = row.Cells[2].Text;
            if (row.Cells[3].Text == "S")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar_check_aprob", "activar_check_aprob();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_aprob", "desactivar_check_aprob();", true);
            }
            if (row.Cells[4].Text == "S")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar_check_prom", "activar_check_prom();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check_prom", "desactivar_check_prom();", true);
            }
            combo_estatus();
            ddl_estatus.SelectedValue = row.Cells[5].Text;
            btn_update.Visible = true;
            btn_save.Visible = false;
            txt_tcali.Attributes.Add("readonly", "");
            //grid_tespr_bind();
        }
    }
}