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
    public partial class talte : System.Web.UI.Page
    {
        public string id_num = null;
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
                    combo_tipo_telefono();
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

        protected void combo_tipo_telefono()
        {
            string strQuerydire = "";
            strQuerydire = "select ttele_clave clave, ttele_desc telefono from ttele where ttele_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona un tipo----' telefono " +
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

                ddl_tipo_telefono.DataSource = TablaEstado;
                ddl_tipo_telefono.DataValueField = "Clave";
                ddl_tipo_telefono.DataTextField = "Telefono";
                ddl_tipo_telefono.DataBind();

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
        protected void grid_telefono_bind(string matricula)
        {
            string strQueryDir = "";
            strQueryDir = " select tpers_num id_num,tpers_id clave, concat(tpers_nombre,' ',tpers_paterno,' ',tpers_materno) nombre, "+
                            "talte_ttele_clave tipo_tel, ttele_desc descripcion, talte_consec consecutivo, talte_lada lada, talte_tel telefono,talte_ext extension, " +
                            "talte_estatus c_estatus, case when talte_estatus = 'A' then 'Activo' else 'Inactivo' end estatus, fecha(date_format(talte_date, '%Y-%m-%d')) fecha " +
                            "from(SELECT tpers_id matricula, concat(tpers_nombre, ' ', tpers_paterno, ' ', tpers_materno) alumno  from tpers where tpers_tipo = 'E') estu, talte, tpers,ttele " +
                            "where tpers_id = estu.matricula and tpers_num = talte_tpers_num and ttele_clave = talte_ttele_clave and tpers_id like '" + matricula + "'";

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryDir, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Telefono");
                GridTelefono.DataSource = ds;
                GridTelefono.DataBind();
                GridTelefono.DataMember = "Telefono";
                GridTelefono.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridTelefono.UseAccessibleHeader = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                GridTelefono.Visible = true;

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

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            btn_save.Visible = true;
            btn_update.Visible = false;
            txt_matricula.Text = null;
            txt_nombre.Text = null;
            combo_tipo_telefono();
            combo_estatus();
            txt_lada.Text = null;
            txt_telefono.Text = null;
            txt_extension.Text = null;
            ddl_tipo_telefono.Attributes.Remove("disabled");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            GridTelefono.Visible=false;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_lada.Text) && !String.IsNullOrEmpty(txt_telefono.Text) && ddl_tipo_telefono.SelectedValue!="0")
            {
                string Query = "INSERT INTO talte (talte_tpers_num,talte_consec,talte_ttele_clave,talte_lada,talte_tel,talte_ext,talte_estatus,talte_date,talte_user) VALUES ( " +
                              lbl_id_pers.Text + "," + consecutivo(lbl_id_pers.Text) + ",'" + ddl_tipo_telefono.SelectedValue + "','" + txt_lada.Text + "','" + txt_telefono.Text + "','" + txt_extension.Text + "','" + ddl_estatus.SelectedValue + "',CURRENT_TIMESTAMP(),'" + Session["usuario"].ToString() + "')";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    txt_lada.Text = null;
                    txt_telefono.Text = null;
                    txt_extension.Text = null;
                    combo_tipo_telefono();
                    combo_estatus();
                    grid_telefono_bind(txt_matricula.Text);
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
                    grid_telefono_bind(txt_matricula.Text);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_telefono();", true);
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_lada.Text) && !String.IsNullOrEmpty(txt_telefono.Text) && ddl_tipo_telefono.SelectedValue != "0")
            {
                string Query = "UPDATE talte SET talte_lada='" + txt_lada.Text + "',talte_tel='" + txt_telefono.Text + "',talte_ext='" + txt_extension.Text + "',talte_estatus='" + ddl_estatus.SelectedValue + "',talte_date=current_timestamp(),talte_user='" + Session["usuario"].ToString() + "' WHERE talte_tpers_num='" + lbl_id_pers.Text + "' AND talte_consec='" + lbl_consecutivo.Text + "' AND talte_ttele_clave='" + ddl_tipo_telefono.SelectedValue + "'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    grid_telefono_bind(txt_matricula.Text);
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
                    grid_telefono_bind(txt_matricula.Text);
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_telefono();", true);
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
                    grid_telefono_bind(txt_matricula.Text);
                }
                else if (txt_matricula.Text.Contains("%"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    grid_telefono_bind(txt_matricula.Text);
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
            Query = "SELECT COUNT(*) Indicador FROM talte WHERE talte_tpers_num = (SELECT DISTINCT tpers_num FROM tpers WHERE tpers_id='" + matricula + "')";
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
            Query = "select IFNULL(max(talte_consec),0)+1 consecutivo from talte where talte_tpers_num="+id_num+ " and talte_ttele_clave='" + ddl_tipo_telefono.SelectedValue + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            consecutivo = dt.Rows[0]["consecutivo"].ToString();
            return consecutivo;
        }
        protected void GridTelefono_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.Visible = false;
            btn_update.Visible = true;
            GridViewRow row = GridTelefono.SelectedRow;
            txt_matricula.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
            combo_tipo_telefono();
            ddl_tipo_telefono.SelectedValue = row.Cells[4].Text;
            lbl_consecutivo.Text= row.Cells[6].Text;
            txt_lada.Text= row.Cells[7].Text;
            txt_telefono.Text= row.Cells[8].Text;
            txt_extension.Text = HttpUtility.HtmlDecode(row.Cells[9].Text.Trim());
            combo_estatus();
            ddl_estatus.SelectedValue= row.Cells[10].Text;
            ddl_tipo_telefono.Attributes.Add("disabled", "");
            grid_telefono_bind(txt_matricula.Text);
        }
    }
}