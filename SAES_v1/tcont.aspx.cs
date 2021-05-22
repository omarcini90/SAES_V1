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
    public partial class tcont : System.Web.UI.Page
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
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 12 ";

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
                        btn_tcont.Visible = true;
                    }
                    grid_tcont_bind();
                }

            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();

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
        protected void grid_tcont_bind()
        {
            string strQueryGrid = "";
            strQueryGrid = " select tcont_clave clave, tcont_desc nombre,  " +
              " tcont_estatus c_estatus,CASE WHEN tcont_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tcont_date,'%Y-%m-%d')) fecha " +
              " from tcont order by clave ";
            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryGrid, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Tcont");
                Gridtcont.DataSource = ds;
                Gridtcont.DataBind();
                Gridtcont.DataMember = "Tcont";
                Gridtcont.HeaderRow.TableSection = TableRowSection.TableHeader;
                Gridtcont.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_tcont.Text = null;
            txt_nombre.Text = null;
            combo_estatus();
            btn_save.Visible = true;
            btn_update.Visible = false;
            txt_tcont.Attributes.Remove("readonly");
            grid_tcont_bind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_tcont.Text) && !String.IsNullOrEmpty(txt_nombre.Text))
            {
                if (valida_tcont(txt_tcont.Text))
                {
                    string strCadSQL = "INSERT INTO tcont Values ('" + txt_tcont.Text + "','" + txt_nombre.Text + "','" +
                    Session["usuario"].ToString() + "',current_timestamp(),'" + ddl_estatus.SelectedValue + "')";
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        txt_tcont.Text = null;
                        txt_nombre.Text = null;
                        combo_estatus();
                        grid_tcont_bind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarClave('ContentPlaceHolder1_txt_tcont',1);", true);
                    grid_tcont_bind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcont();", true);
                grid_tcont_bind();
            }


        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_tcont.Text) && !String.IsNullOrEmpty(txt_nombre.Text))
            {
                string strCadSQL = "UPDATE tcont SET tcont_desc='" + txt_nombre.Text + "', tcont_estatus='" + ddl_estatus.SelectedValue + "', tcont_user='" + Session["usuario"].ToString() + "', tcont_date=CURRENT_TIMESTAMP() WHERE tcont_clave='" + txt_tcont.Text + "'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_tcont_bind();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcont();", true);
            }
        }

        protected bool valida_tcont(string tcont)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tcont WHERE tcont_clave='" + tcont + "'";
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

        protected void Gridtcont_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = Gridtcont.SelectedRow;
            txt_tcont.Text = row.Cells[1].Text;
            txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            combo_estatus();
            ddl_estatus.SelectedValue = row.Cells[3].Text;
            btn_update.Visible = true;
            btn_save.Visible = false;
            txt_tcont.Attributes.Add("readonly", "");
            grid_tcont_bind();
        }
    }
}