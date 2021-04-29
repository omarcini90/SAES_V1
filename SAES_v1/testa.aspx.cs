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
    public partial class testa : System.Web.UI.Page
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

                c_estado.Attributes.Add("onblur", "validarclaveEstado('ContentPlaceHolder1_c_estado',0)");
                c_estado.Attributes.Add("oninput", "validarclaveEstado('ContentPlaceHolder1_c_estado',0)");
                n_estado.Attributes.Add("onblur", "validarNombreEstado('ContentPlaceHolder1_n_estado')");
                n_estado.Attributes.Add("oninput", "validarNombreEstado('ContentPlaceHolder1_n_estado')");
                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_estatus();
                    combo_pais();
                }
            }
        }

        protected void combo_estatus()
        {
            estatus_estado.Items.Clear();
            estatus_estado.Items.Add(new ListItem("Activo", "A"));
            estatus_estado.Items.Add(new ListItem("Inactivo", "B"));
        }
        protected void combo_pais()
        {
            string Query = "SELECT TPAIS_CLAVE Clave,TPAIS_DESC Nombre FROM TPAIS WHERE TPAIS_ESTATUS='A'  " +
                            "UNION " +
                            "SELECT '0' Clave,'----Selecciona un país----' Nombre " +
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

                cbo_pais.DataSource = TablaEstado;
                cbo_pais.DataValueField = "Clave";
                cbo_pais.DataTextField = "Nombre";
                cbo_pais.DataBind();

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
        private void LlenaPagina()
        {
            //System.Threading.Thread.Sleep(50);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);
            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"] + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 2 ";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {
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
                    btn_estado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //Logs
            }
            grid_bind_estados();


        }
        protected void grid_bind_estados()
        {
            string strQueryEstados = "";
            strQueryEstados = "SELECT TESTA_CLAVE CLAVE, TESTA_DESC NOMBRE,TESTA_TPAIS_CLAVE C_PAIS,TPAIS_DESC PAIS, TESTA_ESTATUS ESTATUS_CODE,CASE WHEN TESTA_ESTATUS='A' THEN 'Activo' ELSE 'Inactivo' END ESTATUS, DATE_FORMAT(TESTA_DATE,'%d/%m/%Y') FECHA " +
                                "FROM TESTA " +
                                "INNER JOIN TPAIS ON TPAIS_CLAVE = TESTA_TPAIS_CLAVE " +
                                "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryEstados, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Estados");
                GridEstados.DataSource = ds;
                GridEstados.DataBind();
                GridEstados.DataMember = "Estados";
                GridEstados.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridEstados.UseAccessibleHeader = true;
            }
            catch (Exception ex)
            {
                //Logs
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
            ConexionMySql.Close();
        }
        protected void GridEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridEstados.SelectedRow;

            c_estado.Text = row.Cells[1].Text;
            n_estado.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            combo_estatus();
            combo_pais();
            cbo_pais.SelectedValue = row.Cells[3].Text;
            estatus_estado.SelectedValue = row.Cells[5].Text;
            save_estado.Visible = false;
            update_estado.Visible = true;
            c_estado.Attributes.Add("readonly", "");
            grid_bind_estados();

        }
        protected bool valida_clave_edo(string clave)
        {
            string Query = "";
            Query = "SELECT DISTINCT COUNT(*) Indicador " +
                    "FROM TESTA " +
                    "INNER JOIN TPAIS ON tpais_clave = testa_tpais_clave " +
                    "WHERE testa_tpais_clave = '" + cbo_pais.SelectedValue + "' " +
                    "AND testa_clave = '" + clave + "' ";

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
        protected void save_estado_Click(object sender, EventArgs e)
        {
            if (cbo_pais.SelectedValue != "0" && !String.IsNullOrEmpty(c_estado.Text) && !String.IsNullOrEmpty(n_estado.Text))
            {

                if (valida_clave_edo(c_estado.Text))
                {
                    string Query = "INSERT INTO TESTA VALUES ('" + c_estado.Text + "','" + cbo_pais.SelectedValue + "','" + n_estado.Text + "','" + Session["usuario"].ToString() + "',CURRENT_TIMESTAMP(),'" + estatus_estado.SelectedValue + "')";

                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        c_estado.Text = null;
                        n_estado.Text = null;
                        combo_estatus();
                        combo_pais();
                        grid_bind_estados();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar_e", "save();", true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarclaveEstado('ContentPlaceHolder1_c_estado',1);", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_estado();", true);
            }
        }

        protected void update_estado_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(c_estado.Text) && !String.IsNullOrEmpty(n_estado.Text) && cbo_pais.SelectedValue != "0")
            {
                string Query = "UPDATE testa SET testa_desc='" + n_estado.Text + "',testa_tuser_clave='" + Session["usuario"].ToString() + "',testa_date=CURRENT_TIMESTAMP(),testa_estatus='" + estatus_estado.SelectedValue + "' WHERE testa_clave='" + c_estado.Text + "' AND testa_tpais_clave='" + cbo_pais.SelectedValue + "'";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_bind_estados();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_e", "update();", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_estado();", true);
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

        protected void cancel_estado_Click(object sender, EventArgs e)
        {
            combo_pais();
            combo_estatus();
            c_estado.Text = null;
            c_estado.Attributes.Remove("readonly");
            n_estado.Text = null;
            save_estado.Visible = true;
            update_estado.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            grid_bind_estados();
        }
    }
}