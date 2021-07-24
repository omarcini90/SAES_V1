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
    public partial class tredo : System.Web.UI.Page
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctrl_fecha_i", "ctrl_fecha_i();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctrl_fecha_f", "ctrl_fecha_f();", true);
                ddl_estatus.Attributes.Add("disabled", "");

                if (!IsPostBack)
                {
                    combo_estatus();
                    llena_pagina();
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
        protected void llena_pagina()
        {
            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                          " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                          " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 3 and tusme_tmede_clave = 3 ";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            MySqlDataAdapter sqladapter = new MySqlDataAdapter();

            DataSet dssql1 = new DataSet();

            MySqlCommand commandsql1 = new MySqlCommand(QerySelect, ConexionMySql);
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
                btn_documentos.Visible = true;
            }
        }

        protected void combo_estatus()
        {
            string strQuerydire = "";
            strQuerydire = "select tstdo_clave clave, tstdo_desc estatus from tstdo where tstdo_estatus='A' " +
                            //"union " +
                            //"select '0' clave, '----Selecciona un periodo----' estatus " +
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

                ddl_estatus.DataSource = TablaEstado;
                ddl_estatus.DataValueField = "Clave";
                ddl_estatus.DataTextField = "Periodo";
                ddl_estatus.DataBind();

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

        protected void grid_documentos_bind(string matricula)
        {
            string Query_Docs= " select distinct tpers_num Id_Num,tadmi_tpees_clave periodo, tadmi_tprog_clave programa , tadmi_consecutivo consecutivo, " +
                                " tcodo_tdocu_clave clave_docto, tdocu_desc descripcion, " +
                                " tredo_tstdo_clave c_estatus,tstdo_desc estatus, date_format(tredo_fecha_limite,'%d %b %Y') f_limite,date_format(tredo_fecha_limite, '%d/%m/%Y') fecha_limite, date_format(tredo_fecha_entrega,'%d %b %Y') f_entrega,date_format(tredo_fecha_entrega, '%d/%m/%Y') fecha_entrega, date_format(tredo_date,'%d %b %Y') fecha from tpers " +
                                " inner join tadmi a on tadmi_tpers_num = tpers_num " +
                                " inner join  tcodo on tcodo_tdocu_clave = tcodo_tdocu_clave " +
                                " inner join tdocu on tcodo_tdocu_clave=tdocu_clave " +
                                " inner join tprog on tprog_clave = tadmi_tprog_clave " +
                                " left outer join tredo on tredo_tpers_num = tadmi_tpers_num and tredo_tpees_clave = tadmi_tpees_clave " +
                                "     and tredo_consecutivo = tadmi_consecutivo and tredo_tdocu_clave = tcodo_tdocu_clave " +
                                "left join tstdo on tstdo_clave=tredo_tstdo_clave " +
                                " where tpers_id = '" + matricula + "'" +
                                " and(tcodo_tcamp_clave = tadmi_tcamp_clave or tcodo_tcamp_clave = '000') " +
                                " and(tcodo_tnive_clave = tprog_tnive_clave or tcodo_tnive_clave = '000') " +
                                " and(tcodo_tcole_clave = tprog_tcole_clave or tcodo_tcole_clave = '000') " +
                                " and(tcodo_tmoda_clave = tprog_tmoda_clave or tcodo_tmoda_clave = '000') " +
                                " and(tcodo_tprog_clave = tprog_clave or tcodo_tprog_clave = '0000000000') " +
                                " and(tcodo_ttiin_clave = tadmi_ttiin_clave or tcodo_ttiin_clave = '000') " +
                                " and tcodo_estatus = 'A' " +
                                " order by tadmi_consecutivo desc, tadmi_tpees_clave, tcodo_tdocu_clave ";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(Query_Docs, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Solicitud");
                GridDocumentos.DataSource = ds;
                GridDocumentos.DataBind();
                GridDocumentos.DataMember = "Solicitud";
                GridDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridDocumentos.UseAccessibleHeader = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                GridDocumentos.Visible = true;

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

                txt_nombre.Text = nombre_alumno(txt_matricula.Text);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                grid_documentos_bind(txt_matricula.Text);

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
        protected bool valida_redo(string matricula, string documento)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tredo WHERE tredo_tpers_num = (SELECT DISTINCT tpers_num FROM tpers WHERE tpers_id='" + matricula + "') and tredo_tdocu_clave='"+documento+"'";
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

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_matricula.Text = null;
            txt_nombre.Text = null;
            txt_clave_doc.Text = null;
            txt_documento.Text = null;
            combo_estatus();
            txt_fecha_l.Text = null;
            txt_fecha_e.Text = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            GridDocumentos.Visible = false;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txt_fecha_l.Text))
            {
                if (valida_redo(txt_matricula.Text, txt_clave_doc.Text))
                {
                    //Update//
                    string Query = "UPDATE tredo SET tredo_fecha_limite=STR_TO_DATE('" + txt_fecha_l.Text + "', '%d/%m/%Y'), tredo_fecha_entrega=STR_TO_DATE('" + txt_fecha_e.Text + "', '%d/%m/%Y'), tredo_user='"+Session["usuario"].ToString()+"' WHERE tredo_tpers_num='"+lbl_id_pers.Text+"' AND tredo_tdocu_clave='"+txt_clave_doc.Text+"' AND tredo_consecutivo='"+lbl_consecutivo.Text+ "' AND tredo_tpees_clave='"+lbl_periodo.Text+"'";

                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        grid_documentos_bind(txt_matricula.Text);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message;
                    }
                }
                else
                {
                    //insert//
                    string Query="INSERT INTO tredo Values (" + lbl_id_pers.Text + ",'" + lbl_periodo.Text + "'," + lbl_consecutivo.Text + ",'" +
                    txt_clave_doc.Text + "','" + null + "',STR_TO_DATE('" + txt_fecha_l.Text + "', '%d/%m/%Y'), " +
                    " STR_TO_DATE('" + txt_fecha_e.Text + "', '%d/%m/%Y'),'" + Session["usuario"].ToString() + "',current_timestamp()) ";

                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        grid_documentos_bind(txt_matricula.Text);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_documentos();", true);
            }
        }

        protected void GridDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = GridDocumentos.SelectedRow;
                //txt_matricula.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                //txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
                lbl_periodo.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                lbl_consecutivo.Text = HttpUtility.HtmlDecode(row.Cells[4].Text);
                txt_clave_doc.Text = HttpUtility.HtmlDecode(row.Cells[5].Text);
                txt_documento.Text = HttpUtility.HtmlDecode(row.Cells[6].Text);
                combo_estatus();
                ddl_estatus.SelectedValue = row.Cells[7].Text;
                txt_fecha_l.Text= HttpUtility.HtmlDecode(row.Cells[10].Text).Trim();
                txt_fecha_e.Text= HttpUtility.HtmlDecode(row.Cells[12].Text).Trim();
                grid_documentos_bind(txt_matricula.Text);
            }
            catch (Exception EX)
            {
                string TEST = EX.Message;
            }
        }

        protected void consulta_docs_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Tcdoc.aspx");
        }
    }

}