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
    public partial class tpees : System.Web.UI.Page
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

                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctrl_fecha_i", "ctrl_fecha_i();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctrl_fecha_f", "ctrl_fecha_f();", true);

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
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 1 and tusme_tmede_clave = 5 ";

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
                        btn_periodo.Visible = true;
                    }
                    grid_periodo_bind();
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
        protected void grid_periodo_bind()
        {
            string strQueryPeriodos = "";
            strQueryPeriodos = " select tpees_clave clave, tpees_desc nombre, tpees_ofic oficial, " +
              " date_format(tpees_inicio,'%d/%m/%Y') fecha_ini, date_format(tpees_fin,'%d/%m/%Y') fecha_fin, " +
              " tpees_estatus c_estatus,CASE WHEN tpees_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tpees_date,'%Y-%m-%d')) fecha " +
              " from tpees order by clave desc";
            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryPeriodos, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Periodos");
                GridPeriodo.DataSource = ds;
                GridPeriodo.DataBind();
                GridPeriodo.DataMember = "Periodos";
                GridPeriodo.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridPeriodo.UseAccessibleHeader = true;
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
            txt_periodo.Text = null;
            txt_nombre.Text = null;
            txt_oficial.Text = null;
            txt_fecha_i.Text = null;
            txt_fecha_f.Text = null;
            combo_estatus();
            btn_save.Visible = true;
            btn_update.Visible = false;
            txt_periodo.Attributes.Remove("readonly");
            grid_periodo_bind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_periodo.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_fecha_i.Text) && !String.IsNullOrEmpty(txt_fecha_f.Text))
            {
                if (valida_periodo(txt_periodo.Text))
                {
                    string strCadSQL = "INSERT INTO tpees Values ('" + txt_periodo.Text + "','" + txt_nombre.Text + "','" +
                    txt_oficial.Text + "', STR_TO_DATE('" + string.Format(txt_fecha_i.Text,"dd/MM/yyyy") + "','%d/%m/%Y'), STR_TO_DATE('" + string.Format(txt_fecha_f.Text, "dd/MM/yyyy") + "','%d/%m/%Y'),'" +
                    Session["usuario"].ToString() + "',current_timestamp(),'" + ddl_estatus.SelectedValue + "')";
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        txt_periodo.Text = null;
                        txt_nombre.Text = null;
                        txt_oficial.Text = null;
                        txt_fecha_i.Text = null;
                        txt_fecha_f.Text = null;
                        combo_estatus();
                        grid_periodo_bind();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarclavePeriodo('ContentPlaceHolder1_txt_periodo',1);", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_periodo();", true);
            }


        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_periodo.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_fecha_i.Text) && !String.IsNullOrEmpty(txt_fecha_f.Text))
            {
                string strCadSQL = "UPDATE tpees SET tpees_desc='"+txt_nombre.Text+ "', tpees_inicio=STR_TO_DATE('" + txt_fecha_i.Text + "','%d/%m/%Y'),tpees_fin=STR_TO_DATE('" + txt_fecha_f.Text + "','%d/%m/%Y'),tpees_estatus='"+ddl_estatus.SelectedValue+"',tpees_ofic='"+txt_oficial.Text+"', tpees_user='"+Session["usuario"].ToString()+"', tpees_date=CURRENT_TIMESTAMP() WHERE tpees_clave='"+txt_periodo.Text+"'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_periodo_bind();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_periodo();", true);
            }
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

        protected void GridPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridPeriodo.SelectedRow;
            txt_periodo.Text = row.Cells[1].Text;
            txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txt_oficial.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
            combo_estatus();
            ddl_estatus.SelectedValue = row.Cells[6].Text;
            txt_fecha_i.Text = row.Cells[4].Text;
            txt_fecha_f.Text = row.Cells[5].Text;
            btn_update.Visible = true;
            btn_save.Visible = false;
            txt_periodo.Attributes.Add("readonly", "");
            grid_periodo_bind();
        }
    }
}