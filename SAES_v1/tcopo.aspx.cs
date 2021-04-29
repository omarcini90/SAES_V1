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
    public partial class tcopo : System.Web.UI.Page
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

                  c_zip.Attributes.Add("onblur", "validarclaveZip('ContentPlaceHolder1_c_zip', 0);");
                  c_zip.Attributes.Add("oninput", "validarclaveZip('ContentPlaceHolder1_c_zip', 0);");
                  n_zip.Attributes.Add("onblur", "validarNombreZip('ContentPlaceHolder1_n_zip');");
                  n_zip.Attributes.Add("oninput", "validarNombreZip('ContentPlaceHolder1_n_zip');");

                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_estatus();
                    combo_paises_zip();
                }
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
                    btn_zip.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ///logs
            }

        }
        protected void combo_estatus()
        {
            e_zip.Items.Clear();
            e_zip.Items.Add(new ListItem("Activo", "A"));
            e_zip.Items.Add(new ListItem("Inactivo", "B"));
        }
        protected void cbop_zip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbop_zip.SelectedValue != "0")
            {
                combo_estados_zip();
            }
        }

        protected void cboe_zip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboe_zip.SelectedValue != "0")
            {
                combo_delegacion_zip();
            }
        }

        protected void cbod_zip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbod_zip.SelectedValue != "0")
            {
                grid_bind_zip();
            }
        }

        protected void grid_bind_zip()
        {
            string Query = "";
            Query = "SELECT DISTINCT tcopo_clave CLAVE, tcopo_desc NOMBRE,tpais_clave C_PAIS,tpais_desc PAIS,tcopo_testa_clave C_ESTADO,testa_desc ESTADO,tcopo_tdele_clave C_DELEGACION,tdele_desc DELEGACION,tcopo_estatus ESTATUS_CODE,CASE WHEN tcopo_estatus='A' THEN 'Activo' ELSE 'Inactivo' END ESTATUS, DATE_FORMAT(tcopo_date,'%d/%m/%Y') FECHA " +
                    "FROM TCOPO " +
                    "INNER JOIN TESTA ON testa_clave = tcopo_testa_clave and testa_tpais_clave = tcopo_tpais_clave " +
                    "INNER JOIN TDELE ON tdele_clave = tcopo_tdele_clave and tdele_testa_clave = testa_clave and tdele_tpais_clave = testa_tpais_clave " +
                    "INNER JOIN TPAIS ON tpais_clave = tcopo_tpais_clave " +
                    "WHERE tpais_clave='"+cbop_zip.SelectedValue+"' AND tcopo_testa_clave='"+cboe_zip.SelectedValue+"' AND tcopo_tdele_clave='"+cbod_zip.SelectedValue+"' " +
                    "ORDER BY 3,5,1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(Query, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Zip");
                GridZip.DataSource = ds;
                GridZip.DataBind();
                GridZip.DataMember = "Zip";
                GridZip.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridZip.UseAccessibleHeader = true;

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

        protected void GridZip_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = GridZip.SelectedRow;
                combo_paises_zip();
                cbop_zip.SelectedValue = row.Cells[3].Text;
                combo_estados_zip();
                cboe_zip.SelectedValue = row.Cells[5].Text;
                combo_delegacion_zip();
                cbod_zip.SelectedValue = row.Cells[7].Text;
                c_zip.Text = row.Cells[1].Text;
                c_zip.Attributes.Add("readonly", "");
                n_zip.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                n_zip.Attributes.Add("readonly", "");
                combo_estatus();
                e_zip.SelectedValue = row.Cells[9].Text;
                save_zip.Visible = false;
                update_zip.Visible = true;
                grid_bind_zip();
            }catch(Exception ex)
            {
                ///logs
            }

        }

        protected void combo_paises_zip()
        {
            cbop_zip.Items.Clear();
            cboe_zip.Items.Clear();
            cboe_zip.Items.Add(new ListItem("----Selecciona un estado----", "0"));
            cbod_zip.Items.Clear();
            cbod_zip.Items.Add(new ListItem("----Selecciona una delegación---", "0"));

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

                cbop_zip.DataSource = TablaEstado;
                cbop_zip.DataValueField = "Clave";
                cbop_zip.DataTextField = "Nombre";
                cbop_zip.DataBind();

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
        protected void combo_estados_zip()
        {
            string Query = "SELECT TESTA_CLAVE Clave,TESTA_DESC Nombre FROM TESTA WHERE TESTA_ESTATUS='A' AND TESTA_TPAIS_CLAVE= " + cbop_zip.SelectedValue +
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

                cboe_zip.DataSource = TablaEstado;
                cboe_zip.DataValueField = "Clave";
                cboe_zip.DataTextField = "Nombre";
                cboe_zip.DataBind();

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
        protected void combo_delegacion_zip()
        {
            string Query = "SELECT tdele_clave CLAVE,tdele_desc NOMBRE FROM TDELE WHERE tdele_tpais_clave='" + cbop_zip.SelectedValue + "' AND tdele_testa_clave='" + cboe_zip.SelectedValue + "' " +
                            "UNION " +
                            "SELECT '0' CLAVE,'----Selecciona una delegación----' NOMBRE " +
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

                cbod_zip.DataSource = TablaEstado;
                cbod_zip.DataValueField = "Clave";
                cbod_zip.DataTextField = "Nombre";
                cbod_zip.DataBind();

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
        protected bool valida_clave_zip(string clave,string nombre)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM TCOPO WHERE tcopo_tpais_clave='" + cbop_zip.SelectedValue + "' AND tcopo_testa_clave='" + cboe_zip.SelectedValue + "'AND tcopo_tdele_clave='" + cbod_zip.SelectedValue + "' AND tcopo_clave='" + clave + "' AND tcopo_desc='" + nombre + "'" ;

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
        protected void save_zip_Click(object sender, EventArgs e)
        {
            if (cbop_zip.SelectedValue != "0" && cboe_zip.SelectedValue != "0" && cbod_zip.SelectedValue != "0" && !String.IsNullOrEmpty(c_zip.Text) && !String.IsNullOrEmpty(n_zip.Text))
            {
                if (valida_clave_zip(c_zip.Text,n_zip.Text))
                {
                    string Query = "INSERT INTO TCOPO VALUES ('" + c_zip.Text + "','" + n_zip.Text + "','" + cbop_zip.SelectedValue + "','" + cboe_zip.SelectedValue + "','" + cbod_zip.SelectedValue + "','" + Session["usuario"].ToString() + "',CURRENT_TIMESTAMP(),'" + e_zip.SelectedValue + "', NULL)";

                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        c_zip.Text = null;
                        n_zip.Text = null;
                        combo_estatus();
                        grid_bind_zip();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar_z", "save();", true);
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
                    grid_bind_zip();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarclaveZip('ContentPlaceHolder1_c_zip', 1);", true);
                }
            }
            else
            {
                grid_bind_zip();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_zip();", true);
            }
        }

        protected void update_zip_Click(object sender, EventArgs e)
        {
            if (cbop_zip.SelectedValue != "0" && cboe_zip.SelectedValue != "0" && cbod_zip.SelectedValue != "0" && !String.IsNullOrEmpty(c_zip.Text) && !String.IsNullOrEmpty(n_zip.Text))
            {
                string Query = "UPDATE tcopo SET tcopo_user='" + Session["usuario"].ToString() + "',tcopo_date=CURRENT_TIMESTAMP(),tcopo_estatus='" + e_zip.SelectedValue + "' WHERE tcopo_clave='" + c_zip.Text + "' AND tcopo_tpais_clave='" + cbop_zip.SelectedValue + "' AND tcopo_testa_clave='" + cboe_zip.SelectedValue + "' AND tcopo_tdele_clave='" + cbod_zip.SelectedValue + "'";


                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_bind_zip();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_Z", "update();", true);
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
                grid_bind_zip();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_zip();", true);
            }
        }

        protected void cancel_zip_Click(object sender, EventArgs e)
        {
            combo_paises_zip();
            combo_estatus();
            c_zip.Text = null;
            c_zip.Attributes.Remove("readonly");
            n_zip.Text = null;
            GridZip.Visible = false;
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