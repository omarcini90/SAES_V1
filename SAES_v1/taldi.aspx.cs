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
    public partial class taldi : System.Web.UI.Page
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
                    Llena_pagina();
                    combo_tipo_direccion();
                    combo_estatus();
                    combo_pais();
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
        private void Llena_pagina()
        {
            System.Threading.Thread.Sleep(50);

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
                    btn_taldi.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        protected void combo_tipo_direccion()
        {
            string strQuerydire = "";
            strQuerydire = "select tdire_clave clave, tdire_desc direccion from tdire where tdire_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona un tipo----' direccion " +
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

                ddl_tipo_direccion.DataSource = TablaEstado;
                ddl_tipo_direccion.DataValueField = "Clave";
                ddl_tipo_direccion.DataTextField = "Direccion";
                ddl_tipo_direccion.DataBind();

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

        protected void combo_pais()
        {
            ddl_estado.Items.Clear();
            ddl_estado.Items.Add(new ListItem("----Selecciona un estado----", "0"));
            ddl_delegacion.Items.Clear();
            ddl_delegacion.Items.Add(new ListItem("----Selecciona una delegación----", "0"));
            ddl_colonia.Items.Clear();
            ddl_colonia.Items.Add(new ListItem("----Selecciona una colonia----", "0"));

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

                ddl_pais.DataSource = TablaEstado;
                ddl_pais.DataValueField = "Clave";
                ddl_pais.DataTextField = "Nombre";
                ddl_pais.DataBind();

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

        protected void combo_estado(string c_pais)
        {
            ddl_estado.Items.Clear();
            ddl_delegacion.Items.Clear();
            ddl_delegacion.Items.Add(new ListItem("----Selecciona una delegación----", "0"));
            string Query = "SELECT TESTA_CLAVE Clave,TESTA_DESC Nombre FROM TESTA WHERE TESTA_ESTATUS='A' AND TESTA_TPAIS_CLAVE= " + c_pais +
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

                ddl_estado.DataSource = TablaEstado;
                ddl_estado.DataValueField = "Clave";
                ddl_estado.DataTextField = "Nombre";
                ddl_estado.DataBind();

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

        protected void combo_delegacion(string c_pais, string c_estado)
        {
            ddl_delegacion.Items.Clear();
            string Query = "SELECT tdele_clave CLAVE,tdele_desc NOMBRE FROM TDELE WHERE tdele_tpais_clave='" + c_pais + "' AND tdele_testa_clave='" + c_estado + "' " +
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

                ddl_delegacion.DataSource = TablaEstado;
                ddl_delegacion.DataValueField = "Clave";
                ddl_delegacion.DataTextField = "Nombre";
                ddl_delegacion.DataBind();

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

        protected void combo_colonia(string c_pais, string c_estado, string c_delegacion, string zip)
        {
            ddl_colonia.Items.Clear();
            string Query = "SELECT DISTINCT ROW_NUMBER() OVER (PARTITION BY tcopo_clave,tcopo_tpais_clave,tcopo_testa_clave,tcopo_tdele_clave ORDER BY tcopo_clave) Clave, tcopo_desc Nombre FROM tcopo WHERE tcopo_tpais_clave='" + c_pais + "' AND tcopo_testa_clave='" + c_estado + "' AND tcopo_tdele_clave='" + c_delegacion + "' AND tcopo_clave='" + txt_zip.Text + "' " +
                            "UNION " +
                            "SELECT '0' CLAVE,'----Selecciona una colonia----' NOMBRE " +
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

                ddl_colonia.DataSource = TablaEstado;
                ddl_colonia.DataValueField = "Clave";
                ddl_colonia.DataTextField = "Nombre";
                ddl_colonia.DataBind();

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

        protected void txt_matricula_TextChanged(object sender, EventArgs e)
        {
            if (valida_matricula(txt_matricula.Text))
            {
                if (valida_direccion(txt_matricula.Text))
                {
                    grid_direccion_bind(txt_matricula.Text);
                }else if (txt_matricula.Text.Contains("%"))
                {
                    grid_direccion_bind(txt_matricula.Text);
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

        protected void ddl_pais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_pais.SelectedValue != "0")
            {
                combo_estado(ddl_pais.SelectedValue);
            }
            else
            {
                combo_pais();
            }
        }

        protected void ddl_estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_estado.SelectedValue != "0")
            {
                combo_delegacion(ddl_pais.SelectedValue, ddl_estado.SelectedValue);
            }
            else
            {
                combo_estado(ddl_pais.SelectedValue);
            }
        }

        protected void txt_zip_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_zip.Text))
            {
                if (valida_zip(txt_zip.Text))
                {
                    combo_colonia(ddl_pais.SelectedValue, ddl_estado.SelectedValue, ddl_delegacion.SelectedValue, txt_zip.Text);
                }
                else
                {
                    //Validación ZIP
                }
            }
        }

        protected bool valida_zip(string zip)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tcopo WHERE tcopo_clave='" + zip + "'";
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

        protected bool valida_direccion(string matricula)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM taldi WHERE taldi_tpers_num = (SELECT DISTINCT tpers_num FROM tpers WHERE tpers_id='"+matricula+"')";
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

        protected void grid_direccion_bind(string matricula)
        {
            string strQueryDir = "";
            strQueryDir = " select tpers_num id_num,tpers_id clave, concat(tpers_nombre,' ',tpers_paterno,' ',tpers_materno) nombre, " +
                           " taldi_tdire_clave tipo_dir, tdire_desc descripcion, taldi_consec consecutivo, taldi_calle direccion, " +
                           " taldi_estatus c_estatus, case when taldi_estatus='A' then 'Activo' else 'Inactivo' end estatus,fecha(date_format(taldi_date,'%Y-%m-%d')) fecha " +
                           " from ( SELECT tpers_id matricula, concat(tpers_nombre,' ',tpers_paterno,' ',tpers_materno) alumno  from tpers where tpers_tipo='E') estu, taldi, tpers, tdire " +
                           " where tpers_id=estu.matricula and tpers_num=taldi_tpers_num and   tdire_clave=taldi_tdire_clave and tpers_id like '" + matricula + "'";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQueryDir, ConexionMySql);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Direccion");
                GridDireccion.DataSource = ds;
                GridDireccion.DataBind();
                GridDireccion.DataMember = "Direccion";
                GridDireccion.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridDireccion.UseAccessibleHeader = true;

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

        protected void GridDireccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected string nombre_alumno(string matricula)
        {
            string nombre = "";
            string Query = "";
            Query = "SELECT tpers_num, CONCAT(tpers_nombre,' ',tpers_materno,' ',tpers_paterno) nombre FROM tpers WHERE tpers_id = '" + matricula + "'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            nombre = dt.Rows[0]["nombre"].ToString();
            id_num= dt.Rows[0]["tpers_num"].ToString();
            return nombre;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_ciudad.Text) && !String.IsNullOrEmpty(txt_dirección.Text) && !String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_zip.Text) && ddl_tipo_direccion.SelectedValue!="0" && ddl_pais.SelectedValue!="0" && ddl_estado.SelectedValue!="0" && ddl_delegacion.SelectedValue!="0" && ddl_colonia.SelectedValue!="0")
            {

                string Query = "INSERT INTO taldi (taldi_tpers_num,taldi_consec,taldi_tdire_clave,taldi_calle,taldi_colonia,taldi_testa_clave,taldi_tdele_clave,taldi_tpais_clave,taldi_tcopo_clave,taldi_ciudad,taldi_estatus,taldi_date,taldi_user) VALUES ( " +
                              id_num + "," + consecutivo(id_num) + "," + ddl_tipo_direccion.SelectedValue + "," + txt_dirección.Text + "," + ddl_colonia.SelectedItem.Text + "," + ddl_estado.SelectedValue + "," + ddl_delegacion.SelectedValue + "," + ddl_pais.SelectedValue + "," + txt_zip.Text + "," + txt_ciudad.Text + "," + ddl_estatus.SelectedValue + ",CURRENT_TIMESTAMP()," + Session["usuario"].ToString()+")";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(Query, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    txt_matricula.Text = null;
                    txt_nombre.Text = null;
                    txt_zip.Text = null;
                    txt_ciudad.Text = null;
                    txt_dirección.Text = null;
                    combo_pais();
                    combo_estatus();
                    combo_tipo_direccion();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_periodo();", true);
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_ciudad.Text) && !String.IsNullOrEmpty(txt_dirección.Text) && !String.IsNullOrEmpty(txt_matricula.Text) && !String.IsNullOrEmpty(txt_zip.Text) && ddl_tipo_direccion.SelectedValue != "0" && ddl_pais.SelectedValue != "0" && ddl_estado.SelectedValue != "0" && ddl_delegacion.SelectedValue != "0" && ddl_colonia.SelectedValue != "0")
            {

            }
            else
            {

            }
        }

        protected string consecutivo(string id_num)
        {
            string consecutivo = "";
            string Query = "";
            Query = "select IFNULL(max(taldi_consec),0)+1 consecutivo from taldi where taldi_tpers_num=1 and taldi_tdire_clave='"+ddl_tipo_direccion.SelectedValue+"'";
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            consecutivo = dt.Rows[0]["consecutivo"].ToString();
            return consecutivo;
        }
    }
}