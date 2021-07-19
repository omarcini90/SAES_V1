﻿using MySql.Data.MySqlClient;
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
    public partial class talco : System.Web.UI.Page
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

                if (!IsPostBack)
                {
                    LlenaPagina();
                    combo_estatus();
                    combo_tipo_correo();
                }

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

        private void LlenaPagina()
        {
            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 3 and tusme_tmede_clave = 3 ";

            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
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
            if (dssql1.Tables[0].Rows[0][0].ToString() == "1")
            {
                btn_talte.Visible = true;
            }

            conexion.Close();
        }

        protected void combo_tipo_correo()
        {
            string strQuerydire = "";
            strQuerydire = "select tmail_clave clave, tmail_desc correo from tmail where tmail_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona un tipo----' correo " +
                            "order by 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaEstado = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = strQuerydire;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaEstado.Load(DatosMySql, LoadOption.OverwriteChanges);

                ddl_tipo_correo.DataSource = TablaEstado;
                ddl_tipo_correo.DataValueField = "Clave";
                ddl_tipo_correo.DataTextField = "Correo";
                ddl_tipo_correo.DataBind();

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
        protected void combo_estatus()
        {
            ddl_estatus.Items.Clear();
            ddl_estatus.Items.Add(new ListItem("Activo", "A"));
            ddl_estatus.Items.Add(new ListItem("Inactivo", "B"));
        }

        protected void grid_correo_bind(string matricula)
        {
            string strQueryDir = "";
            strQueryDir = "  select tpers_num id_num,tpers_id clave, concat(tpers_nombre,' ',tpers_paterno,' ',tpers_materno) nombre, "+
                            "talco_tmail_clave tipo_mail, tmail_desc descripcion, talco_consec consecutivo, talco_correo correo, talco_preferido preferido, " +
                            "talco_estatus c_estatus, case when talco_estatus = 'A' then 'Activo' else 'Inactivo' end estatus, fecha(date_format(talco_date, '%Y-%m-%d')) fecha " +
                            "from(SELECT tpers_id matricula, concat(tpers_nombre, ' ', tpers_paterno, ' ', tpers_materno) alumno  from tpers where tpers_tipo = 'E') estu, talco, tpers,tmail " +
                            "where tpers_id = estu.matricula and tpers_num = talco_tpers_num and tmail_clave = talco_tmail_clave and tpers_id like '" + matricula + "'";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryDir, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Correo");
                GridCorreo.DataSource = ds;
                GridCorreo.DataBind();
                GridCorreo.DataMember = "Correo";
                GridCorreo.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridCorreo.UseAccessibleHeader = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                GridCorreo.Visible = true;

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
                ConexionMySql.Close();
            }
        }
        protected void txt_matricula_TextChanged(object sender, EventArgs e)
        {
            if (valida_matricula(txt_matricula.Text))
            {

                if (valida_telefono(txt_matricula.Text))
                {
                    txt_nombre.Text = nombre_alumno(txt_matricula.Text);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    grid_correo_bind(txt_matricula.Text);
                }
                else if (txt_matricula.Text.Contains("%"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    grid_correo_bind(txt_matricula.Text);
                }
                else
                {
                    txt_nombre.Text = nombre_alumno(txt_matricula.Text);
                }

            }
            else
            {
                ///Matricula no existe
            }
        }

        protected bool valida_matricula(string matricula)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tpers WHERE tpers_id Like '" + matricula + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            if (dt.Rows[0]["Indicador"].ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool valida_telefono(string matricula)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM talco WHERE talco_tpers_num = (SELECT DISTINCT tpers_num FROM tpers WHERE tpers_id='" + matricula + "')";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            if (dt.Rows[0]["Indicador"].ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected string nombre_alumno(string matricula)
        {
            string nombre = "";
            string Query = "";
            Query = "SELECT tpers_num, CONCAT(tpers_nombre,' ',tpers_materno,' ',tpers_paterno) nombre FROM tpers WHERE tpers_id = '" + matricula + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            nombre = dt.Rows[0]["nombre"].ToString();
            lbl_id_pers.Text = dt.Rows[0]["tpers_num"].ToString();
            return nombre;
        }

        protected string consecutivo(string id_num)
        {
            string consecutivo = "";
            string Query = "";
            Query = "select IFNULL(max(talco_consec),0)+1 consecutivo from talco where talco_tpers_num=" + id_num + " and talco_tmail_clave='" + ddl_tipo_correo.SelectedValue + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            consecutivo = dt.Rows[0]["consecutivo"].ToString();
            return consecutivo;
        }

        protected void GridCorreo_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.Visible = false;
            btn_update.Visible = true;
            GridViewRow row = GridCorreo.SelectedRow;
            txt_matricula.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
            combo_tipo_correo();
            ddl_tipo_correo.SelectedValue = row.Cells[4].Text;
            lbl_consecutivo.Text = row.Cells[6].Text;
            txt_correo.Text = row.Cells[7].Text;
            if (row.Cells[8].Text == "S")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar_check", "activar_check();", true);
            }
            combo_estatus();
            ddl_estatus.SelectedValue = row.Cells[9].Text;
            ddl_tipo_correo.Attributes.Add("disabled", "");
            grid_correo_bind(txt_matricula.Text);
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            
            btn_save.Visible = true;
            btn_update.Visible = false;
            txt_matricula.Text = null;
            txt_nombre.Text = null;
            combo_tipo_correo();
            combo_estatus();
            txt_correo.Text = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check", "desactivar_check();", true);
            ddl_tipo_correo.Attributes.Remove("disabled");
            GridCorreo.Visible = false;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string pattern_mail = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
            Regex mail = new Regex(pattern_mail);
            //var test_1 = mail.IsMatch(txt_correo.Text);
            if (!String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && ddl_tipo_correo.SelectedValue != "0" && mail.IsMatch(txt_correo.Text))
            {
                string preferido = "";
                string selected = Request.Form["customSwitches"];
                if (selected == "on") { preferido = "S"; } else { preferido = "N"; }
                string Query = "INSERT INTO talco (talco_tpers_num,talco_consec,talco_tmail_clave,talco_correo,talco_preferido,talco_estatus,talco_date,talco_user) VALUES ( " +
                              lbl_id_pers.Text + "," + consecutivo(lbl_id_pers.Text) + ",'" + ddl_tipo_correo.SelectedValue + "','" + txt_correo.Text + "','" + preferido + "','" + ddl_estatus.SelectedValue + "',CURRENT_TIMESTAMP(),'" + Session["usuario"].ToString() + "')";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    txt_correo.Text = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "desactivar_check", "desactivar_check();", true);
                    combo_tipo_correo();
                    combo_estatus();
                    grid_correo_bind(txt_matricula.Text);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
                }
                catch (Exception ex)
                {
                    string test = ex.Message;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txt_matricula.Text))
                {
                    grid_correo_bind(txt_matricula.Text);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_correo();", true);
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            string pattern_mail = @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/";
            Regex mail = new Regex(pattern_mail);
            if (!String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_correo.Text) && ddl_tipo_correo.SelectedValue != "0" && mail.IsMatch(txt_correo.Text))
            {
                string preferido = "";
                string selected = Request.Form["customSwitches"];
                if (selected == "on") { preferido = "S"; } else { preferido = "N"; }
                string Query = "UPDATE talco SET talco_correo='" + txt_correo.Text + "',talco_preferido='" + preferido + "',talco_estatus='" + ddl_estatus.SelectedValue + "',talco_date=current_timestamp(),talco_user='" + Session["usuario"].ToString() + "' WHERE talco_tpers_num='" + lbl_id_pers.Text + "' AND talco_consec='" + lbl_consecutivo.Text + "' AND talco_tmail_clave='" + ddl_tipo_correo.SelectedValue + "'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_correo_bind(txt_matricula.Text);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
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
                if (!String.IsNullOrEmpty(txt_matricula.Text))
                {
                    grid_correo_bind(txt_matricula.Text);
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_correo();", true);
            }
        }
    }
}