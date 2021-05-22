using MySql.Data.MySqlClient;
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
    public partial class tpers : System.Web.UI.Page
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctrl_f_nac", "ctrl_f_nac();", true);
                txt_matricula.Attributes.Add("readonly", "");

                if (!IsPostBack)
                {
                    LlenaPagina();
                    combo_genero();
                    combo_estado_civil();
                }

            }
        }

        private void LlenaPagina()
        {
            string QerySelect = "select tusme_update, tusme_select from tuser, tusme " +
                              " where tuser_clave = '" + Session["usuario"].ToString() + "'" +
                              " and tusme_trole_clave = tuser_trole_clave and tusme_tmenu_clave = 3 and tusme_tmede_clave = 3 ";

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
                if (dssql1.Tables[0].Rows[0][0].ToString() == "1")
                {
                    btn_tpers.Visible = true;
                }

            }catch(Exception ex)
            {
                //logs
            }
            conexion.Close();
            grid_solicitudes_bind();
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

        protected void combo_genero()
        {
            ddl_genero.Items.Clear();
            ddl_genero.Items.Add(new ListItem("---Selecciona un Genero---", "0"));
            ddl_genero.Items.Add(new ListItem("Masculino", "M"));
            ddl_genero.Items.Add(new ListItem("Femenino", "F"));
            ddl_genero.Items.Add(new ListItem("No Aplica", "N"));
        }
        protected void combo_estado_civil()
        {
            ddl_estado_c.Items.Clear();
            ddl_estado_c.Items.Add(new ListItem("---Selecciona un Estado Civil---", "0"));
            ddl_estado_c.Items.Add(new ListItem("Casado", "C"));
            ddl_estado_c.Items.Add(new ListItem("Soltero", "S"));
            ddl_estado_c.Items.Add(new ListItem("Viudo", "V"));
            ddl_estado_c.Items.Add(new ListItem("Union Libre", "U"));
            ddl_estado_c.Items.Add(new ListItem("Divorsiado", "D"));
            ddl_estado_c.Items.Add(new ListItem("No Aplica", "N"));
        }
        protected void grid_solicitudes_bind()
        {
            string QueryEstudiantes = "select distinct tpers_id clave, tpers_nombre nombre, tpers_paterno paterno, tpers_materno materno, tpers_genero c_genero, CASE WHEN tpers_genero = 'F' THEN 'Femenino' WHEN tpers_genero = 'M' THEN 'Masculino' ELSE 'No Aplica' END genero, "+
                                       "tpers_edo_civ c_civil, CASE WHEN tpers_edo_civ = 'C' THEN 'Casado' WHEN tpers_edo_civ = 'S' THEN 'Soltero' WHEN tpers_edo_civ = 'V' THEN 'Viudo' WHEN tpers_edo_civ = 'U' THEN 'Union Libre' WHEN tpers_edo_civ = 'D' THEN 'Divorciado' ELSE 'No Aplica' END e_civil, tpers_curp curp, date_format(tpers_fecha_nac, ' %d/%m/%Y') fecha, tpers_usuario usuario, fecha(date_format(tpers_date, '%Y-%m-%d')) fecha_reg " +
                                        "from tpers " +
                                         "where tpers_tipo = 'E'";
            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(QueryEstudiantes, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Solicitudes");
                GridSolicitudes.DataSource = ds;
                GridSolicitudes.DataBind();
                GridSolicitudes.DataMember = "Solicitudes";
                GridSolicitudes.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridSolicitudes.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
            }
            catch (Exception ex)
            {
                //logs
            }
            conexion.Close();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_matricula.Text = null;
            txt_nombre.Text = null;
            txt_apellido_p.Text = null;
            txt_apellido_m.Text = null;
            combo_genero();
            combo_estado_civil();
            txt_curp.Text = null;
            txt_f_nac.Text = null;
            btn_save.Visible = true;
            btn_update.Visible = false;
            grid_solicitudes_bind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_apellido_p.Text) && !String.IsNullOrEmpty(txt_apellido_m.Text) && !String.IsNullOrEmpty(txt_curp.Text))
            {
                if (valida_curp_format(txt_curp.Text))
                {
                    string strCadSQL = "";
                    double pidm;
                    string matricula = "";
                    string genero = "";
                    string e_civil = "";
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    string strSeqnpidm = " update tseqn set tseqn_numero=tseqn_numero+1 where tseqn_clave='001'";
                    MySqlCommand myCommandupd1 = new MySqlCommand(strSeqnpidm, conexion);
                    myCommandupd1.ExecuteNonQuery();


                    string strSeqnpidm1 = " select tseqn_numero from tseqn where tseqn_clave='001' ";
                    DataSet dssql1 = new DataSet();
                    MySqlCommand commandsql1 = new MySqlCommand(strSeqnpidm1, conexion);
                    MySqlDataAdapter sqladapter = new MySqlDataAdapter();
                    sqladapter.SelectCommand = commandsql1;
                    sqladapter.Fill(dssql1);
                    sqladapter.Dispose();
                    commandsql1.Dispose();

                    pidm = Convert.ToDouble(dssql1.Tables[0].Rows[0][0].ToString());

                    string strSeqnId = " update tseqn set tseqn_numero=tseqn_numero+1 where tseqn_clave='002'";
                    MySqlCommand myCommandupd2 = new MySqlCommand(strSeqnId, conexion);
                    myCommandupd2.ExecuteNonQuery();

                    string strSeqnId1 = " select lpad(tseqn_numero,tseqn_longitud,'0') from tseqn where tseqn_clave='002' ";

                    DataSet dssql2 = new DataSet();
                    MySqlCommand commandsql2 = new MySqlCommand(strSeqnId1, conexion);
                    sqladapter.SelectCommand = commandsql2;
                    sqladapter.Fill(dssql2);
                    sqladapter.Dispose();
                    commandsql2.Dispose();

                    matricula = dssql2.Tables[0].Rows[0][0].ToString();
                    if (ddl_genero.SelectedValue == "0") { genero = "N"; } else { genero = ddl_genero.SelectedValue; }
                    if (ddl_estado_c.SelectedValue == "0") { e_civil = "N"; } else { e_civil = ddl_estado_c.SelectedValue; }

                    strCadSQL = "INSERT INTO tpers " +
                    " Values (" + pidm + ",'" + matricula + "','" + txt_apellido_p.Text + "','" + txt_apellido_m.Text + "','" + txt_nombre.Text + "','" +
                    genero + "', STR_TO_DATE('" + txt_f_nac.Text + "', ' %d/%m/%Y'),'" + e_civil + "','" + txt_curp.Text + "','E', null, current_timestamp(), '" +
                    Session["usuario"].ToString() + "')";

                    try
                    {
                        MySqlCommand myCommandinserta = new MySqlCommand(strCadSQL, conexion);
                        myCommandinserta.ExecuteNonQuery();
                        txt_nombre.Text = null;
                        txt_apellido_p.Text = null;
                        txt_apellido_m.Text = null;
                        combo_genero();
                        combo_estado_civil();
                        txt_curp.Text = null;
                        txt_f_nac.Text = null;
                        grid_solicitudes_bind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);

                    }
                    catch (Exception ex)
                    {
                        ///logs
                    }

                    conexion.Close();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_solicitud();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_solicitud();", true);
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_nombre.Text) && !String.IsNullOrEmpty(txt_apellido_p.Text) && !String.IsNullOrEmpty(txt_apellido_m.Text) && !String.IsNullOrEmpty(txt_curp.Text))
            {
                if (valida_curp_format(txt_curp.Text))
                {
                    string strCadSQL = "";
                    string genero = "";
                    string e_civil = "";
                    if (ddl_genero.SelectedValue == "0") { genero = "N"; } else { genero = ddl_genero.SelectedValue; }
                    if (ddl_estado_c.SelectedValue == "0") { e_civil = "N"; } else { e_civil = ddl_estado_c.SelectedValue; }
                    strCadSQL = " update tpers set tpers_nombre='" + txt_nombre.Text + "'," +
                            " tpers_paterno='" + txt_apellido_p.Text + "'," +
                            " tpers_materno='" + txt_apellido_m.Text + "'," +
                            " tpers_genero='" + genero + "'," +
                            " tpers_fecha_nac= STR_TO_DATE('" + txt_f_nac.Text + "', ' %d/%m/%Y'), " +
                            " tpers_edo_civ='" + e_civil + "'," +
                            " tpers_curp='" + txt_curp.Text + "'," +
                            " tpers_date=curdate()," +
                            " tpers_usuario='" + Session["usuario"].ToString() + "'" +
                            " where tpers_id='" + txt_matricula.Text + "'";
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    try
                    {
                        MySqlCommand myCommandinserta = new MySqlCommand(strCadSQL, conexion);
                        myCommandinserta.ExecuteNonQuery();
                        grid_solicitudes_bind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);

                    }
                    catch (Exception ex)
                    {
                        ///logs
                    }
                }
                else
                {
                    grid_solicitudes_bind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarCurp_format('0');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_solicitud();", true);
            }

        }

        protected void GridSolicitudes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridSolicitudes.SelectedRow;
            txt_matricula.Text = row.Cells[1].Text;
            txt_nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txt_apellido_p.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
            txt_apellido_m.Text = HttpUtility.HtmlDecode(row.Cells[4].Text);
            combo_genero();
            ddl_genero.SelectedValue = row.Cells[5].Text;
            combo_estado_civil();
            ddl_estado_c.SelectedValue = row.Cells[7].Text;
            txt_curp.Text = HttpUtility.HtmlDecode(row.Cells[9].Text);
            txt_f_nac.Text = row.Cells[10].Text;
            btn_save.Visible = false;
            btn_update.Visible = true;
            grid_solicitudes_bind();

        }

        protected bool valida_curp_format(string curp_form)
        {
            double indicador = 0;
            string regex_curp_of =
            "[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}" +
            "(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])" +
            "[HM]{1}" +
            "(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)" +
            "[B-DF-HJ-NP-TV-Z]{3}" +
            "[0-9A-Z]{1}[0-9]{1}$";
            Regex curp = new Regex(regex_curp_of);
            string dv = curp_form.Substring(17, 1);
            if (!curp.IsMatch(curp_form))
            {
                return false;
            }
            else
            {

                string diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
                double lngSuma = 0.0, lngDigito = 0.0;
                for (var i = 0; i < 17; i++)
                    lngSuma = lngSuma + diccionario.IndexOf(curp_form[i]) * (18 - i);
                lngDigito = 10 - lngSuma % 10;
                if (lngDigito == 10)
                {
                    indicador = 0;

                }
                else
                {
                    indicador = lngDigito;

                }
            }
            if (dv == indicador.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}