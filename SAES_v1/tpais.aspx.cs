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
    public partial class tpais : System.Web.UI.Page
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
                
                c_pais.Attributes.Add("onblur", "validarclavePais('ContentPlaceHolder1_c_pais',0)");
                c_pais.Attributes.Add("oninput", "validarclavePais('ContentPlaceHolder1_c_pais',0)");
                n_pais.Attributes.Add("onblur", "validarNombrePais('ContentPlaceHolder1_n_pais')");
                n_pais.Attributes.Add("oninput", "validarNombrePais('ContentPlaceHolder1_n_pais')");
                LlenaPagina();
                if (!IsPostBack)
                {
                    combo_estatus();
                }
                
            }
            
            

        }

        protected void combo_estatus()
        {
            estatus_pais.Items.Clear();
            estatus_pais.Items.Add(new ListItem("Activo", "A"));
            estatus_pais.Items.Add(new ListItem("Inactivo", "B"));
        }

        private void LlenaPagina()
        {
            //System.Threading.Thread.Sleep(50);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);

            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"] + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 1 ";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            MySqlDataAdapter sqladapter = new MySqlDataAdapter();

            DataSet dssql1 = new DataSet();
            try
            {
                MySqlCommand commandsql1 = new MySqlCommand(QerySelect, ConexionMySql);
                sqladapter.SelectCommand = commandsql1;
                sqladapter.Fill(dssql1);
                sqladapter.Dispose();
                commandsql1.Dispose();
                if (dssql1.Tables[0].Rows.Count == 0 || dssql1.Tables[0].Rows[0][1].ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "permisos", "sin_acceso();", true);
                }
            }
            catch (Exception ex)
            {
                //Logs
            }
            if (dssql1.Tables[0].Rows[0][0].ToString() == "1")
            {
                btn_pais.Visible = true;
            }
            grid_bind_pais();


        }

        protected void grid_bind_pais()
        {
            string strQueryPaises = "";
            strQueryPaises = "SELECT TPAIS_CLAVE CLAVE, TPAIS_DESC NOMBRE, TPAIS_GENTIL GENTIL, TPAIS_ESTATUS ESTATUS_CODE, CASE WHEN TPAIS_ESTATUS='A' THEN 'Activo' ELSE 'Inactivo' END ESTATUS, DATE_FORMAT(TPAIS_DATE,'%d/%m/%Y') FECHA FROM TPAIS ORDER BY CLAVE";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);

            //Label1.Text = strQueryEsc;
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryPaises, ConexionMySql);
            DataSet ds = new DataSet();
            dataadapter.Fill(ds, "Paises");
            GridPaises.DataSource = ds;
            GridPaises.DataBind();
            GridPaises.DataMember = "Paises";
            GridPaises.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridPaises.UseAccessibleHeader = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);

            ConexionMySql.Close();
        }

        protected void GridPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridPaises.SelectedRow;

            c_pais.Text = row.Cells[1].Text;
            n_pais.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            g_pais.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
            combo_estatus();
            estatus_pais.SelectedValue = row.Cells[4].Text;
            save_pais.Visible = false;
            update_pais.Visible = true;
            c_pais.Attributes.Add("readonly", "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fade_datatable", "fade_datatable();", true);
            grid_bind_pais();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "fadein_datatable", "fadein_datatable();", true);

        }

        protected void cancel_pais_Click(object sender, EventArgs e)
        {
            c_pais.Text = null;
            n_pais.Text= null;
            g_pais.Text = null;
            combo_estatus();
            c_pais.Attributes.Remove("readonly");
            save_pais.Visible = true;
            update_pais.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            grid_bind_pais();
        }

        protected bool valida_clave(string clave)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM TPAIS WHERE TPAIS_CLAVE='" + clave + "'";
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

        protected void save_pais_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(c_pais.Text) && !String.IsNullOrEmpty(n_pais.Text))
            {
                if (valida_clave(c_pais.Text))
                {
                    
                    string Query = "INSERT INTO TPAIS VALUES ('" + c_pais.Text + "','" + n_pais.Text + "','" + g_pais.Text + "','" + Session["usuario"].ToString() + "',CURRENT_TIMESTAMP(),'" + estatus_pais.SelectedValue + "')";
                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        c_pais.Text = null;
                        n_pais.Text = null;
                        combo_estatus();
                        grid_bind_pais();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message;
                        //Logs
                    }
                    finally
                    {
                        ConexionMySql.Close();
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarclavePais('ContentPlaceHolder1_c_pais',1);", true);
                }
            }
            else
            {
                //Validación de campos obligatorios JS
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_pais();", true);
            }
        }

        protected void update_pais_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(c_pais.Text) && !String.IsNullOrEmpty(n_pais.Text))
            {

                string Query = "UPDATE tpais SET tpais_desc='" + n_pais.Text + "',tpais_gentil='" + g_pais.Text + "',tpais_tuser_clave='" + Session["usuario"].ToString() + "',tpais_date=CURRENT_TIMESTAMP(),tpais_estatus='" + estatus_pais.SelectedValue + "' WHERE tpais_clave='" + c_pais.Text + "'";
                MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                ConexionMySql.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {

                    mysqlcmd.ExecuteNonQuery();
                    grid_bind_pais();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
                }
                catch (Exception ex)
                {
                    string test = ex.Message;
                    //Logs
                }
                finally
                {
                    ConexionMySql.Close();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_pais();", true);
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