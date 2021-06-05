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
    public partial class tpaco : System.Web.UI.Page
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

                //c_prog_campus.Attributes.Add("onblur", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                //c_prog_campus.Attributes.Add("oninput", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_campus_cobranza();
                    combo_tipo_periodo();
                    combo_concepto_calendario();
                    combo_concepto_cobranza();
                }

            }
        }

        private void LlenaPagina()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);
            string QerySelect = "select tusme_update from tuser, tusme " +
                         " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                         " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 9 ";

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
                    cobranza_btn.Visible = true;
                }
                //grid_cobranza_bind();
            }
            catch (Exception ex)
            {
                ///logs
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
        protected void combo_campus_cobranza()
        {
            cobranza_n.Items.Clear();
            cobranza_n.Items.Add(new ListItem("--------", "0"));

            string Query = "SELECT DISTINCT tcamp_clave Clave, tcamp_desc Campus FROM tcamp " +
                            "UNION " +
                            "SELECT DISTINCT '0','--------' Campus  " +
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
                cobranza_c.DataSource = TablaCampus;
                cobranza_c.DataValueField = "Clave";
                cobranza_c.DataTextField = "Campus";
                cobranza_c.DataBind();

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

        protected void combo_nivel_cobranza(string campus)
        {
            cobranza_n.Items.Clear();
            string Query = "SELECT DISTINCT tprog_tnive_clave clave, tnive_desc nivel FROM tcapr, tnive, tprog WHERE tcapr_estatus='A' AND tcapr_tcamp_clave='" + campus + "' AND tcapr_tprog_clave=tprog_clave AND tprog_tnive_clave=tnive_clave  " +
                           "UNION " +
                           "SELECT DISTINCT '0','--------' Campus  " +
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
                cobranza_n.DataSource = TablaCampus;
                cobranza_n.DataValueField = "Clave";
                cobranza_n.DataTextField = "Nivel";
                cobranza_n.DataBind();

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

        protected void combo_tipo_periodo()
        {
            cobranza_tipo_p.Items.Clear();
            string Query = "SELECT '01' clave, 'ANTICIPADO' tipo_per " +
                            "UNION " +
                            "SELECT '02' clave, 'REGULAR' tipo_per " +
                            "UNION " +
                            "SELECT '03' clave, 'EXTEMPORANEO' tipo_per " +
                           "UNION " +
                           "SELECT DISTINCT '0' clave,'--------' tipo_per  " +
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
                cobranza_tipo_p.DataSource = TablaCampus;
                cobranza_tipo_p.DataValueField = "Clave";
                cobranza_tipo_p.DataTextField = "tipo_per";
                cobranza_tipo_p.DataBind();

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

        protected void combo_periodos()
        {
            string Query = "SELECT DISTINCT tpees_clave Clave,CONCAT(tpees_clave,'-',tpees_desc) Periodo FROM tpees WHERE tpees_clave like '" + cobranza_p.Text + "%' " +
                           "UNION " +
                           "SELECT DISTINCT '0','' Periodo  " +
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
                dd_periodo.DataSource = TablaCampus;
                dd_periodo.DataValueField = "Clave";
                dd_periodo.DataTextField = "Periodo";
                dd_periodo.DataBind();

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
        protected void combo_concepto_calendario()
        {
            string Query = "SELECT DISTINCT tcoca_clave Clave, tcoca_desc Concepto_Cal FROM tcoca " +
                           "UNION " +
                           "SELECT DISTINCT '0','--------' Concepto_Cal  " +
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
                cobranza_conc_cal.DataSource = TablaCampus;
                cobranza_conc_cal.DataValueField = "Clave";
                cobranza_conc_cal.DataTextField = "Concepto_Cal";
                cobranza_conc_cal.DataBind();

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

        protected void combo_concepto_cobranza()
        {
            string Query = "SELECT DISTINCT tcoco_clave Clave, tcoco_desc Concepto_Cob FROM tcoco " +
                           "UNION " +
                           "SELECT DISTINCT '0','--------' Concepto_Cob  " +
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
                cobranza_concepto.DataSource = TablaCampus;
                cobranza_concepto.DataValueField = "Clave";
                cobranza_concepto.DataTextField = "Concepto_Cob";
                cobranza_concepto.DataBind();

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
        protected void grid_cobranza_bind()
        {
            string Query = "";
            Query = "SELECT tpaco_tcamp_clave C_Campus,tcamp_desc Campus, tpaco_tnive_clave C_Nivel,tnive_desc Nivel,tpaco_clave Clave,CASE WHEN tpaco_clave='01' THEN 'ANTICIPADO' WHEN tpaco_clave='02' THEN 'REGULAR' WHEN tpaco_clave='03' THEN 'EXTEMPORANEO' END Tipo_Periodo,tpaco_pdesc_insc Desc_Insc,tpaco_pdesc_parc Desc_Col,tcoca_clave C_Conce_Cal,tcoca_desc Conce_Calendario,tcoco_clave C_Conce_Cob,tcoco_desc Conce_Cobranza,tpaco_tpees_clave Periodo " +
                    "FROM tpaco " +
                    "INNER JOIN tcamp ON tcamp_clave = tpaco_tcamp_clave " +
                    "INNER JOIN tnive ON tnive_clave = tpaco_tnive_clave " +
                    "INNER JOIN tcoco ON tcoco_clave = tpaco_tcoco_clave " +
                    "INNER JOIN tcoca ON tcoca_clave = tpaco_tcoca_clave " +
                    "WHERE tpaco_tpees_clave='" + cobranza_p.Text + "' ";

            if (cobranza_c.SelectedValue != "0")
            {
                Query += "AND tpaco_tcamp_clave='" + cobranza_c.SelectedValue + "' ";
            }
            if (cobranza_n.SelectedValue != "0")
            {
                Query += "AND tpaco_tnive_clave='" + cobranza_n.SelectedValue + "' ";
            }
            if (cobranza_tipo_p.SelectedValue != "0")
            {
                Query += "AND tpaco_clave='" + cobranza_tipo_p.SelectedValue + "' ";
            }
            Query += " ORDER BY 1,3,5";
            try
            {
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(Query, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "cobranza");
                GridCobranza.DataSource = ds;
                GridCobranza.EditIndex = -1;
                GridCobranza.DataBind();
                GridCobranza.DataMember = "cobranza";
                GridCobranza.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridCobranza.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
        }
        protected void cobranza_p_TextChanged(object sender, EventArgs e)
        {
            if (!valida_periodo(cobranza_p.Text))
            {
                grid_cobranza_bind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_cobranza", "load_datatable_cobranza();", true);
            }
            else
            {
                dd_term.Visible = true;
                term_text.Visible = false;
                combo_periodos();
            }

        }

        protected void cobranza_c_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_nivel_cobranza(cobranza_c.SelectedValue);
            if (!String.IsNullOrEmpty(cobranza_p.Text))
            {
                grid_cobranza_bind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_cobranza", "load_datatable_cobranza();", true);
            }
            
        }

        protected void dd_periodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            dd_term.Visible = false;
            term_text.Visible = true;
            cobranza_p.Text = dd_periodo.SelectedValue;
            if (dd_periodo.SelectedValue != "0")
            {
                grid_cobranza_bind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_cobranza", "load_datatable_cobranza();", true);
            }
            
        }

        protected void search_term_Click(object sender, ImageClickEventArgs e)
        {
            dd_term.Visible = true;
            term_text.Visible = false;
            combo_periodos();
        }

        protected bool valida_periodo(string periodo)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tpees WHERE tpees_clave='" + periodo + "'";
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

        protected bool valida_tipo_peri()
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tpaco WHERE tpaco_tpees_clave='" + cobranza_p.Text + "' AND tpaco_tcamp_clave='" + cobranza_c.SelectedValue + "' AND tpaco_tnive_clave='" + cobranza_n.SelectedValue + "' AND tpaco_clave='" + cobranza_tipo_p.SelectedValue + "'";
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
        protected void GridCobranza_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_campus_cobranza();
            GridViewRow row = GridCobranza.SelectedRow;
            cobranza_c.SelectedValue = row.Cells[1].Text;
            combo_nivel_cobranza(cobranza_c.SelectedValue);
            cobranza_n.SelectedValue = row.Cells[3].Text;
            combo_tipo_periodo();
            cobranza_tipo_p.SelectedValue = row.Cells[5].Text;
            cobranza_p.Text = row.Cells[13].Text;
            combo_concepto_calendario();
            cobranza_conc_cal.SelectedValue = row.Cells[9].Text;
            combo_concepto_cobranza();
            cobranza_concepto.SelectedValue = row.Cells[11].Text;
            descuento_col.Text = row.Cells[8].Text;
            descuento_ins.Text = row.Cells[7].Text;
            guardar_cob.Visible = false;
            actualizar_cob.Visible = true;
            grid_cobranza_bind();
        }

        protected void cancelar_cob_Click(object sender, EventArgs e)
        {
            combo_campus_cobranza();
            combo_concepto_cobranza();
            combo_concepto_calendario();
            combo_tipo_periodo();
            cobranza_p.Text = null;
            descuento_col.Text = null;
            descuento_ins.Text = null;
            guardar_cob.Visible = true;
            actualizar_cob.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            GridCobranza.Visible = false;
        }

        protected void guardar_cob_Click(object sender, EventArgs e)
        {
            if (cobranza_c.SelectedValue != "0" && cobranza_n.SelectedValue != "0" && cobranza_tipo_p.SelectedValue != "0" && !String.IsNullOrEmpty(cobranza_p.Text) && cobranza_concepto.SelectedValue != "0" && cobranza_conc_cal.SelectedValue != "0" && !String.IsNullOrEmpty(descuento_col.Text) && !String.IsNullOrEmpty(descuento_ins.Text))
            {
                if (valida_tipo_peri())
                {
                    string campus = cobranza_c.SelectedValue;
                    string nivel = cobranza_n.SelectedValue;
                    string periodo = cobranza_p.Text;
                    string t_periodo = cobranza_tipo_p.SelectedValue;
                    string Query = "INSERT INTO tpaco VALUES('" + cobranza_p.Text + "','" + cobranza_c.SelectedValue + "','" + cobranza_n.SelectedValue + "','" + cobranza_tipo_p.SelectedValue + "'," + descuento_ins.Text + "," + descuento_col.Text + ",'" + cobranza_conc_cal.SelectedValue + "','" + cobranza_concepto.SelectedValue + "','" + Session["usuario"].ToString() + "',current_timestamp())";
                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        combo_campus_cobranza();
                        cobranza_p.Text = periodo;
                        descuento_col.Text = null;
                        descuento_ins.Text = null;
                        combo_concepto_cobranza();
                        combo_concepto_calendario();
                        combo_tipo_periodo();
                        actualizar_cob.Visible = false;
                        guardar_cob.Visible = true;
                        combo_nivel_cobranza(campus);
                        cobranza_c.SelectedValue = campus;
                        cobranza_n.SelectedValue = nivel;
                        cobranza_tipo_p.SelectedValue = t_periodo;
                        grid_cobranza_bind();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "validartperiodo_cob", "validartperiodo_cob('ContentPlaceHolder1_cobranza_tipo_p',1);", true);
                } 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar_campos_cob", "validar_campos_cobranza();", true);
            }
            
        }

        protected void actualizar_cob_Click(object sender, EventArgs e)
        {
            if(cobranza_c.SelectedValue!="0" && cobranza_n.SelectedValue!="0" && cobranza_tipo_p.SelectedValue!="0" && !String.IsNullOrEmpty(cobranza_p.Text) && cobranza_concepto.SelectedValue!="0" && cobranza_conc_cal.SelectedValue!="0" && !String.IsNullOrEmpty(descuento_col.Text) && !String.IsNullOrEmpty(descuento_ins.Text))
            {
                string campus = cobranza_c.SelectedValue;
                string nivel = cobranza_n.SelectedValue;
                string periodo = cobranza_p.Text;
                string t_periodo = cobranza_tipo_p.SelectedValue;
                string Query = "UPDATE tpaco SET tpaco_pdesc_insc='" + descuento_ins.Text + "',tpaco_pdesc_parc='" + descuento_col.Text + "',tpaco_tcoca_clave='" + cobranza_conc_cal.Text + "',tpaco_tcoco_clave='" + cobranza_concepto.SelectedValue + "',tpaco_user='" + Session["usuario"].ToString() + "',tpaco_date= current_timestamp() WHERE tpaco_tpees_clave='" + cobranza_p.Text + "' AND tpaco_tcamp_clave='" + cobranza_c.SelectedValue + "' AND tpaco_tnive_clave='" + cobranza_n.Text + "' AND tpaco_clave='" + cobranza_tipo_p.SelectedValue + "'";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    combo_campus_cobranza();
                    cobranza_p.Text = periodo;
                    descuento_col.Text = null;
                    descuento_ins.Text = null;
                    combo_concepto_cobranza();
                    combo_concepto_calendario();
                    combo_tipo_periodo();
                    actualizar_cob.Visible = false;
                    guardar_cob.Visible = true;
                    combo_nivel_cobranza(campus);
                    cobranza_c.SelectedValue = campus;
                    cobranza_n.SelectedValue = nivel;
                    cobranza_tipo_p.SelectedValue = t_periodo;
                    grid_cobranza_bind();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar_campos_cob", "validar_campos_cobranza();", true);
            }
        }
    }
}