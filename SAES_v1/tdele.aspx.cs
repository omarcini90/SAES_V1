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
    public partial class tdele : System.Web.UI.Page
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

                c_deleg.Attributes.Add("onblur", "validarclaveDelegacion('ContentPlaceHolder1_c_deleg', 0));");
                c_deleg.Attributes.Add("oninput", "validarclaveDelegacion('ContentPlaceHolder1_c_deleg', 0));");
                n_deleg.Attributes.Add("onblur", "validarNombreDelegacion('ContentPlaceHolder1_e_deleg')");
                n_deleg.Attributes.Add("oninput", "validarNombreDelegacion('ContentPlaceHolder1_e_deleg')");
                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_estatus();
                    combo_paises_deleg();
                    combo_estados_deleg();
                }
            }
        }

        protected void combo_estatus()
        {
            e_deleg.Items.Clear();
            e_deleg.Items.Add(new ListItem("Activo", "A"));
            e_deleg.Items.Add(new ListItem("Inactivo", "B"));
        }
        protected void combo_paises_deleg()
        {
            cbop_deleg.Items.Clear();
            cboe_deleg.Items.Clear();
            cboe_deleg.Items.Add(new ListItem("----Selecciona un estado----", "0"));

            string Query = "SELECT TPAIS_CLAVE Clave,TPAIS_DESC Nombre FROM TPAIS WHERE TPAIS_ESTATUS='A' " +
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

                cbop_deleg.DataSource = TablaEstado;
                cbop_deleg.DataValueField = "Clave";
                cbop_deleg.DataTextField = "Nombre";
                cbop_deleg.DataBind();

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
        protected void cbop_deleg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbop_deleg.SelectedValue == "0")
            {
                combo_paises_deleg();
                
            }
            else
            {
                cboe_deleg.Items.Clear();
                combo_estados_deleg();
            }
            c_deleg.Attributes.Remove("readonly");
            GridDelegacion.Visible = false;
        }
        protected void combo_estados_deleg()
        {
            string Query = "SELECT TESTA_CLAVE Clave,TESTA_DESC Nombre FROM TESTA WHERE TESTA_ESTATUS='A' AND TESTA_TPAIS_CLAVE= " + cbop_deleg.SelectedValue +
                            " UNION " +
                            "SELECT '0' Clave,'----Selecciona un estado----' Nombre " +
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

                cboe_deleg.DataSource = TablaEstado;
                cboe_deleg.DataValueField = "Clave";
                cboe_deleg.DataTextField = "Nombre";
                cboe_deleg.DataBind();

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);

            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"] + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 3 ";

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
                    btn_delegacion.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //logs
            }
            finally
            {
                ConexionMySql.Close();
            }            
        }

        protected void grid_bind_delegacion()
        {
            string strQueryProg = "";
            strQueryProg = "SELECT DISTINCT tdele_clave CLAVE,tdele_desc NOMBRE,tpais_clave C_PAIS,tpais_desc PAIS,testa_clave C_ESTADO,testa_desc ESTADO,tdele_estatus ESTATUS_CODE,CASE WHEN tdele_estatus='A' THEN 'Activo' ELSE 'Inactivo' END ESTATUS, DATE_FORMAT(tdele_date,'%d/%m/%Y') FECHA " +
                            "FROM TDELE " +
                            "INNER JOIN TESTA ON testa_clave = tdele_testa_clave AND testa_tpais_clave = tdele_tpais_clave " +
                            "INNER JOIN TPAIS ON tpais_clave = testa_tpais_clave " +
                            "WHERE tpais_clave='"+cbop_deleg.SelectedValue+"' AND testa_clave='"+cboe_deleg.SelectedValue+"'" +
                            "ORDER BY 3,1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryProg, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Delegaciones");
                GridDelegacion.DataSource = ds;
                GridDelegacion.DataBind();
                GridDelegacion.DataMember = "Delegaciones";
                GridDelegacion.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridDelegacion.UseAccessibleHeader = true;
                GridDelegacion.Visible = true;
            }
            catch (Exception ex)
            {
                ///logs
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
                ConexionMySql.Close();
            }
        }
        protected void GridDelegacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridDelegacion.SelectedRow;
            try
            {
                c_deleg.Text = row.Cells[1].Text;
                n_deleg.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                combo_paises_deleg();
                cbop_deleg.SelectedValue = row.Cells[3].Text;
                combo_estados_deleg();
                cboe_deleg.SelectedValue = row.Cells[5].Text;
                combo_estatus();
                e_deleg.SelectedValue = row.Cells[7].Text;
                c_deleg.Attributes.Add("readonly", "");
                save_deleg.Visible = false;
                update_deleg.Visible = true;
                grid_bind_delegacion();
            }
            catch (Exception ex)
            {
                ///logs
            }


        }

        protected void cboe_deleg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboe_deleg.SelectedValue != "0")
            {
                c_deleg.Text = null;
                n_deleg.Text = null;
                c_deleg.Attributes.Remove("readonly");
                combo_estatus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                grid_bind_delegacion();
            }
        }

        protected void cancel_deleg_Click(object sender, EventArgs e)
        {
            save_deleg.Visible = true;
            update_deleg.Visible = false;
            combo_paises_deleg();
            cboe_deleg.SelectedValue = "0";
            combo_estatus();
            c_deleg.Text = null;
            c_deleg.Attributes.Remove("readonly");
            n_deleg.Attributes.Remove("readonly");
            n_deleg.Text = null;
            GridDelegacion.Visible = false;
        }

        protected void save_deleg_Click(object sender, EventArgs e)
        {
            if (cbop_deleg.SelectedValue != "0" && cboe_deleg.SelectedValue != "0" && !String.IsNullOrEmpty(c_deleg.Text) && !String.IsNullOrEmpty(n_deleg.Text))
            {
                if (valida_clave_dele(c_deleg.Text))
                {
                    string Query = "INSERT INTO TDELE VALUES ('" + c_deleg.Text + "','" + n_deleg.Text + "','" + cboe_deleg.SelectedValue + "','" + cbop_deleg.SelectedValue + "','" + Session["usuario"].ToString() + "',CURRENT_TIMESTAMP(),'" + e_deleg.SelectedValue + "')";

                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        c_deleg.Text = null;
                        n_deleg.Text = null;
                        combo_estatus();
                        grid_bind_delegacion();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar_d", "save();", true);
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
                    grid_bind_delegacion();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarclaveDelegacion('ContentPlaceHolder1_c_deleg', 1);", true);
                }
            }
            else
            {
                grid_bind_delegacion();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_delegacion();", true);
            }
        }

        protected void update_deleg_Click(object sender, EventArgs e)
        {
            if (cbop_deleg.SelectedValue != "0" && cboe_deleg.SelectedValue != "0" && !String.IsNullOrEmpty(c_deleg.Text) && !String.IsNullOrEmpty(n_deleg.Text))
            {
                string Query = "UPDATE tdele SET tdele_desc='" + n_deleg.Text + "',tdele_user='" + Session["usuario"].ToString() + "',tdele_date=CURRENT_TIMESTAMP(),tdele_estatus='" + e_deleg.SelectedValue + "' WHERE tdele_clave='" + c_deleg.Text + "' AND tdele_testa_clave='" + cboe_deleg.SelectedValue + "' AND tdele_tpais_clave='" + cbop_deleg.SelectedValue + "'";


                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_bind_delegacion();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_d", "update();", true);
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
                grid_bind_delegacion();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_delegacion();", true);
            }
        }

        protected bool valida_clave_dele(string clave)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM TDELE WHERE tdele_tpais_clave='" + cbop_deleg.SelectedValue + "' AND tdele_testa_clave='" + cboe_deleg.SelectedValue + "' AND tdele_clave='" + clave + "'";

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
    }
}