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
using SAES_v1;

namespace SAES_v1
{
    public partial class tcodo : System.Web.UI.Page
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
                        btn_tcodo.Visible = true;
                    }
                    //grid_tcodo_bind();

                    txt_tcodo.Text = ""; lbl_nombre.Text = "";

                    string Query0 = "SELECT TCAMP_CLAVE Clave,TCAMP_DESC Nombre FROM TCAMP WHERE TCAMP_ESTATUS='A' " +
                            " UNION SELECT '000' Clave, '------' Nombre from dual ORDER BY 1";
                    string Query1 = "SELECT TNIVE_CLAVE Clave,TNIVE_DESC Nombre FROM TNIVE WHERE TNIVE_ESTATUS='A' " +
                            " UNION SELECT '000' Clave, '------' Nombre from dual ORDER BY 1";
                    string Query2 = "SELECT TCOLE_CLAVE Clave,TCOLE_DESC Nombre FROM TCOLE WHERE TCOLE_ESTATUS='A' " +
                            " UNION SELECT '000' Clave, '------' Nombre from dual ORDER BY 1";

                    string Query3 = "SELECT TMODA_CLAVE Clave,TMODA_DESC Nombre FROM TMODA WHERE TMODA_ESTATUS='A' " +
                            " UNION SELECT '000' Clave, '------' Nombre from dual ORDER BY 1";
                    string Query4 = "SELECT TPROG_CLAVE Clave,TPROG_DESC Nombre FROM TPROG WHERE TPROG_ESTATUS='A' " +
                            " UNION SELECT '0000000000' Clave, '------' Nombre from dual ORDER BY 1";
                    string Query5 = "SELECT TTIIN_CLAVE Clave,TTIIN_DESC Nombre FROM TTIIN WHERE TTIIN_ESTATUS='A' " +
                            " UNION SELECT '000' Clave, '------' Nombre from dual ORDER BY 1";
                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    DataTable TablaCampus = new DataTable();
                    DataTable TablaNivel = new DataTable();
                    DataTable TablaColegio = new DataTable();
                    DataTable TablaModalidad = new DataTable();
                    DataTable TablaPrograma = new DataTable();
                    DataTable TablaTipo = new DataTable();
                    MySqlCommand ConsultaMySql = new MySqlCommand();
                    MySqlDataReader DatosMySql;
                    try
                    {
                        ConsultaMySql.Connection = ConexionMySql;
                        ConsultaMySql.CommandType = CommandType.Text;
                        double contador = ddl_campus.Items.Count;
                            ConsultaMySql.CommandText = Query0;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaCampus.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_campus.DataSource = TablaCampus;
                            ddl_campus.DataValueField = "Clave";
                            ddl_campus.DataTextField = "Nombre";
                            ddl_campus.DataBind();


                            ConsultaMySql.CommandText = Query1;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaNivel.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_nivel.DataSource = TablaNivel;
                            ddl_nivel.DataValueField = "Clave";
                            ddl_nivel.DataTextField = "Nombre";
                            ddl_nivel.DataBind();

                            ConsultaMySql.CommandText = Query2;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaColegio.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_colegio.DataSource = TablaColegio;
                            ddl_colegio.DataValueField = "Clave";
                            ddl_colegio.DataTextField = "Nombre";
                            ddl_colegio.DataBind();

                            ConsultaMySql.CommandText = Query3;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaModalidad.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_modalidad.DataSource = TablaModalidad;
                            ddl_modalidad.DataValueField = "Clave";
                            ddl_modalidad.DataTextField = "Nombre";
                            ddl_modalidad.DataBind();

                            ConsultaMySql.CommandText = Query4;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaPrograma.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_programa.DataSource = TablaPrograma;
                            ddl_programa.DataValueField = "Clave";
                            ddl_programa.DataTextField = "Nombre";
                            ddl_programa.DataBind();

                            ConsultaMySql.CommandText = Query5;
                            DatosMySql = ConsultaMySql.ExecuteReader();
                            TablaTipo.Load(DatosMySql, LoadOption.OverwriteChanges);

                            ddl_tipo.DataSource = TablaTipo;
                            ddl_tipo.DataValueField = "Clave";
                            ddl_tipo.DataTextField = "Nombre";
                            ddl_tipo.DataBind();
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

        protected void ddl_campus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string QueryNivel = "";
            QueryNivel = "select distinct tprog_tnive_clave clave, tnive_desc nivel from tcapr, tnive, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tnive_clave=tnive_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryNivel = QueryNivel + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            QueryNivel = QueryNivel + " union " +
                             " select '000' clave, '---------' nivel from dual " +
                              " order by clave ";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            try
            {

                ConexionMySql.Open();
                DataTable TablaNivel = new DataTable();
                MySqlCommand ConsultaMySql1 = new MySqlCommand();
                MySqlDataReader DatosMySql1;
                ConsultaMySql1.Connection = ConexionMySql;
                ConsultaMySql1.CommandType = CommandType.Text;
                ConsultaMySql1.CommandText = QueryNivel;
                DatosMySql1 = ConsultaMySql1.ExecuteReader();
                TablaNivel.Load(DatosMySql1, LoadOption.OverwriteChanges);

                ddl_nivel.DataSource = TablaNivel;
                ddl_nivel.DataValueField = "clave";
                ddl_nivel.DataTextField = "nivel";
                ddl_nivel.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }


            string QueryColegio = "";
            QueryColegio = "select distinct tprog_tcole_clave clave, tcole_desc colegio from tcapr, tcole, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tcole_clave=tcole_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryColegio = QueryColegio + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryColegio = QueryColegio + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            QueryColegio = QueryColegio + " union " +
                " select '000' clave, '---------' colegio from dual ";

            QueryColegio = QueryColegio + " order by clave "; ;
           
            try
            {

                DataTable TablaColegio = new DataTable();
                MySqlCommand ConsultaMySql1 = new MySqlCommand();
                MySqlDataReader DatosMySql1;
                ConsultaMySql1.Connection = ConexionMySql;
                ConsultaMySql1.CommandType = CommandType.Text;
                ConsultaMySql1.CommandText = QueryColegio;
                DatosMySql1 = ConsultaMySql1.ExecuteReader();
                TablaColegio.Load(DatosMySql1, LoadOption.OverwriteChanges);

                ddl_colegio.DataSource = TablaColegio;
                ddl_colegio.DataValueField = "clave";
                ddl_colegio.DataTextField = "colegio";
                ddl_colegio.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            string QueryModalidad = "";
            QueryModalidad = "select distinct tprog_tmoda_clave clave, tmoda_desc modalidad from tcapr, tmoda, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tmoda_clave=tmoda_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            QueryModalidad = QueryModalidad + " union " +
                " select '000' clave, '---------' modalidad from dual ";

            QueryModalidad = QueryModalidad + " order by clave "; ;

            try
            {

                DataTable TablaModalidad = new DataTable();
                MySqlCommand ConsultaMySql1 = new MySqlCommand();
                MySqlDataReader DatosMySql1;
                ConsultaMySql1.Connection = ConexionMySql;
                ConsultaMySql1.CommandType = CommandType.Text;
                ConsultaMySql1.CommandText = QueryModalidad;
                DatosMySql1 = ConsultaMySql1.ExecuteReader();
                TablaModalidad.Load(DatosMySql1, LoadOption.OverwriteChanges);

                ddl_modalidad.DataSource = TablaModalidad;
                ddl_modalidad.DataValueField = "clave";
                ddl_modalidad.DataTextField = "modalidad";
                ddl_modalidad.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            string QueryPrograma = "";
            QueryPrograma = "select distinct tprog_clave clave, tprog_desc programa from tcapr, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            if (ddl_modalidad.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tmoda_clave = '" + ddl_modalidad.SelectedValue.ToString() + "'";
            }
            QueryPrograma = QueryPrograma + " union " +
                " select '000' clave, '---------' programa from dual ";

            QueryPrograma = QueryPrograma + " order by clave "; ;

            try
            {

                DataTable TablaPrograma = new DataTable();
                MySqlCommand ConsultaMySql1 = new MySqlCommand();
                MySqlDataReader DatosMySql1;
                ConsultaMySql1.Connection = ConexionMySql;
                ConsultaMySql1.CommandType = CommandType.Text;
                ConsultaMySql1.CommandText = QueryPrograma;
                DatosMySql1 = ConsultaMySql1.ExecuteReader();
                TablaPrograma.Load(DatosMySql1, LoadOption.OverwriteChanges);

                ddl_programa.DataSource = TablaPrograma;
                ddl_programa.DataValueField = "clave";
                ddl_programa.DataTextField = "programa";
                ddl_programa.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            ConexionMySql.Close();

        }

        protected void ddl_nivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string QueryColegio = "";
            QueryColegio = "select distinct tprog_tcole_clave clave, tcole_desc colegio from tcapr, tcole, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tcole_clave=tcole_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryColegio = QueryColegio + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryColegio = QueryColegio + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            QueryColegio = QueryColegio + " union " +
                " select '000' clave, '-----' colegio from dual ";

            QueryColegio = QueryColegio + " order by clave "; ;

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            try
            {

                DataTable TablaColegio = new DataTable();
                MySqlCommand ConsultaMySqlCole = new MySqlCommand();
                MySqlDataReader DatosMySqlCole;
            ConsultaMySqlCole.Connection = ConexionMySql;
            ConsultaMySqlCole.CommandType = CommandType.Text;
            ConsultaMySqlCole.CommandText = QueryColegio;
            DatosMySqlCole = ConsultaMySqlCole.ExecuteReader();
                TablaColegio.Load(DatosMySqlCole, LoadOption.OverwriteChanges);

                ddl_colegio.DataSource = TablaColegio;
                ddl_colegio.DataValueField = "clave";
                ddl_colegio.DataTextField = "colegio";
                ddl_colegio.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            string QueryModalidad = "";
            QueryModalidad = "select distinct tprog_tmoda_clave clave, tmoda_desc modalidad from tcapr, tmoda, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tmoda_clave=tmoda_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            QueryModalidad = QueryModalidad + " union " +
                " select '000' clave, '---------' modalidad from dual ";

            QueryModalidad = QueryModalidad + " order by clave "; ;

            try
            {

                DataTable TablaModalidad = new DataTable();
                MySqlCommand ConsultaMySqlModa = new MySqlCommand();
                MySqlDataReader DatosMySqlModa;
                ConsultaMySqlModa.Connection = ConexionMySql;
                ConsultaMySqlModa.CommandType = CommandType.Text;
                ConsultaMySqlModa.CommandText = QueryModalidad;
                DatosMySqlModa = ConsultaMySqlModa.ExecuteReader();
                TablaModalidad.Load(DatosMySqlModa, LoadOption.OverwriteChanges);

                ddl_modalidad.DataSource = TablaModalidad;
                ddl_modalidad.DataValueField = "clave";
                ddl_modalidad.DataTextField = "modalidad";
                ddl_modalidad.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            string QueryPrograma = "";
            QueryPrograma = "select distinct tprog_clave clave, tprog_desc programa from tcapr, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            if (ddl_modalidad.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tmoda_clave = '" + ddl_modalidad.SelectedValue.ToString() + "'";
            }
            QueryPrograma = QueryPrograma + " union " +
                " select '000' clave, '---------' programa from dual ";

            QueryPrograma = QueryPrograma + " order by clave "; ;

            try
            {

                DataTable TablaPrograma = new DataTable();
                MySqlCommand ConsultaMySqlProg = new MySqlCommand();
                MySqlDataReader DatosMySqlProg;
                ConsultaMySqlProg.Connection = ConexionMySql;
                ConsultaMySqlProg.CommandType = CommandType.Text;
                ConsultaMySqlProg.CommandText = QueryPrograma;
                DatosMySqlProg = ConsultaMySqlProg.ExecuteReader();
                TablaPrograma.Load(DatosMySqlProg, LoadOption.OverwriteChanges);

                ddl_programa.DataSource = TablaPrograma;
                ddl_programa.DataValueField = "clave";
                ddl_programa.DataTextField = "programa";
                ddl_programa.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            ConexionMySql.Close();

        }

        protected void ddl_colegio_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();


            string QueryModalidad = "";
            QueryModalidad = "select distinct tprog_tmoda_clave clave, tmoda_desc modalidad from tcapr, tmoda, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave and tprog_tmoda_clave=tmoda_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryModalidad = QueryModalidad + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            QueryModalidad = QueryModalidad + " union " +
                " select '000' clave, '---------' modalidad from dual ";

            QueryModalidad = QueryModalidad + " order by clave "; ;

            try
            {

                DataTable TablaModalidad = new DataTable();
                MySqlCommand ConsultaMySqlModa = new MySqlCommand();
                MySqlDataReader DatosMySqlModa;
                ConsultaMySqlModa.Connection = ConexionMySql;
                ConsultaMySqlModa.CommandType = CommandType.Text;
                ConsultaMySqlModa.CommandText = QueryModalidad;
                DatosMySqlModa = ConsultaMySqlModa.ExecuteReader();
                TablaModalidad.Load(DatosMySqlModa, LoadOption.OverwriteChanges);

                ddl_modalidad.DataSource = TablaModalidad;
                ddl_modalidad.DataValueField = "clave";
                ddl_modalidad.DataTextField = "modalidad";
                ddl_modalidad.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            string QueryPrograma = "";
            QueryPrograma = "select distinct tprog_clave clave, tprog_desc programa from tcapr, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            if (ddl_modalidad.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tmoda_clave = '" + ddl_modalidad.SelectedValue.ToString() + "'";
            }
            QueryPrograma = QueryPrograma + " union " +
                " select '000' clave, '---------' programa from dual ";

            QueryPrograma = QueryPrograma + " order by clave "; ;

            try
            {

                DataTable TablaPrograma = new DataTable();
                MySqlCommand ConsultaMySqlProg = new MySqlCommand();
                MySqlDataReader DatosMySqlProg;
                ConsultaMySqlProg.Connection = ConexionMySql;
                ConsultaMySqlProg.CommandType = CommandType.Text;
                ConsultaMySqlProg.CommandText = QueryPrograma;
                DatosMySqlProg = ConsultaMySqlProg.ExecuteReader();
                TablaPrograma.Load(DatosMySqlProg, LoadOption.OverwriteChanges);

                ddl_programa.DataSource = TablaPrograma;
                ddl_programa.DataValueField = "clave";
                ddl_programa.DataTextField = "programa";
                ddl_programa.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            ConexionMySql.Close();

        }

        protected void ddl_modalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();

            string QueryPrograma = "";
            QueryPrograma = "select distinct tprog_clave clave, tprog_desc programa from tcapr, tprog " +
                                " where tcapr_estatus='A' " +
                                " and tcapr_tprog_clave=tprog_clave ";
            if (ddl_campus.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tcapr_tcamp_clave = '" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tnive_clave = '" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tcole_clave = '" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            if (ddl_modalidad.SelectedValue != "000")
            {
                QueryPrograma = QueryPrograma + " and tprog_tmoda_clave = '" + ddl_modalidad.SelectedValue.ToString() + "'";
            }
            QueryPrograma = QueryPrograma + " union " +
                " select '000' clave, '---------' programa from dual ";

            QueryPrograma = QueryPrograma + " order by clave "; ;

            try
            {

                DataTable TablaPrograma = new DataTable();
                MySqlCommand ConsultaMySqlProg = new MySqlCommand();
                MySqlDataReader DatosMySqlProg;
                ConsultaMySqlProg.Connection = ConexionMySql;
                ConsultaMySqlProg.CommandType = CommandType.Text;
                ConsultaMySqlProg.CommandText = QueryPrograma;
                DatosMySqlProg = ConsultaMySqlProg.ExecuteReader();
                TablaPrograma.Load(DatosMySqlProg, LoadOption.OverwriteChanges);

                ddl_programa.DataSource = TablaPrograma;
                ddl_programa.DataValueField = "clave";
                ddl_programa.DataTextField = "programa";
                ddl_programa.DataBind();

            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            ConexionMySql.Close();

        }

        protected void Busqueda_Documento(object sender, EventArgs e)
        {

                string strQuery = "";
                strQuery = " select tdocu_clave clave, tdocu_desc nombre " +
                           " from tdocu ";

                strQuery = strQuery + " order by clave ";

                //resultado.Text = strQuery;
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQuery, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Documentos");
                Gridtdocu.DataSource = ds;
                Gridtdocu.DataBind();
                Gridtdocu.DataMember = "Documentos";
                if (Gridtdocu.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "no_existe", "noexist();", true);
                }
                else
                {
                    Gridtdocu.HeaderRow.TableSection = TableRowSection.TableHeader;
                    Gridtdocu.UseAccessibleHeader = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_tdocu", "load_datatable_tdocu();", true);
                }
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;
            }
            Gridtdocu.Visible = true;
            conexion.Close();
        }


        protected void grid_tcodo_bind()
        {
            string strQuery = "";
            strQuery = " select tcodo_tdocu_clave clave, tdocu_desc nombre,  " +
              " tcodo_tcamp_clave Campus, tcodo_tnive_clave Nivel, tcodo_tcole_clave Colegio," +
              " tcodo_tmoda_clave Modalidad, tcodo_tprog_clave Programa, tcodo_ttiin_clave Tipo, " +
              " tcodo_estatus c_estatus,CASE WHEN tcodo_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tcodo_date,'%Y-%m-%d')) fecha " +
              " from tcodo, tdocu " +
              " where tcodo_tdocu_clave=tdocu_clave ";

            strQuery = strQuery + " order by clave ";

            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQuery, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Tcodo");
                Gridtcodo.DataSource = ds;
                Gridtcodo.DataBind();
                Gridtcodo.DataMember = "Tcodo";
                Gridtcodo.HeaderRow.TableSection = TableRowSection.TableHeader;
                Gridtcodo.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_tcodo", "load_datatable_tcodo();", true);
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_tcodo.Text = null;
            lbl_nombre.Text = null;
            LlenaPagina();
            combo_estatus();
            btn_save.Visible = true;
            btn_update.Visible = false;
            txt_tcodo.Attributes.Remove("readonly");
            //grid_tstdo_bind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
            Gridtcodo.Visible = false;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_tcodo.Text) && !String.IsNullOrEmpty(lbl_nombre.Text))
            {
                if (valida_tcodo(txt_tcodo.Text, ddl_campus.SelectedValue, ddl_nivel.SelectedValue, ddl_colegio.SelectedValue,
                    ddl_modalidad.SelectedValue, ddl_programa.SelectedValue, ddl_tipo.SelectedValue))
                {
                    string strCadSQL = "INSERT INTO tcodo Values ('" + txt_tcodo.Text + "','" + ddl_campus.SelectedValue + "','" +
                    ddl_nivel.SelectedValue + "','" + ddl_colegio.SelectedValue + "','" + ddl_programa.SelectedValue + "','" +
                    ddl_modalidad.SelectedValue + "','" + ddl_tipo.SelectedValue + "','" +
                    Session["usuario"].ToString() + "',current_timestamp(),'" + ddl_estatus.SelectedValue + "')";
                    MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    conexion.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();
                        txt_tcodo.Text = null;
                        lbl_nombre.Text = null;
                        LlenaPagina();
                        combo_estatus();
                        //grid_tstdo_bind();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validarClave('ContentPlaceHolder1_txt_tcodo',1);", true);
                    //grid_tstdo_bind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcodo();", true);
                //grid_tstdo_bind();
            }


        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            string test = ddl_campus.SelectedValue;
            if (!String.IsNullOrEmpty(txt_tcodo.Text) && !String.IsNullOrEmpty(lbl_nombre.Text))
            {
                string strCadSQL = "UPDATE tcodo SET tcodo_tcamp_clave='" + ddl_campus.SelectedValue + "',tcodo_tnive_clave='" +
                    ddl_nivel.SelectedValue + "', tcodo_tcole_clave='" + ddl_colegio.SelectedValue + "', tcodo_tmoda_clave='" + 
                    ddl_modalidad.SelectedValue + "', tcodo_tprog_clave='" + ddl_programa.SelectedValue + "', tcodo_ttiin_clave='" +
                    ddl_tipo.SelectedValue + "', tcodo_estatus='" + ddl_estatus.SelectedValue + "', tcodo_user='" + Session["usuario"].ToString() + "', tcodo_date=CURRENT_TIMESTAMP() " +
                    " WHERE tcodo_tdocu_clave='" + txt_tcodo.Text + "' and tcodo_tcamp_clave='" + Global.campus + "'" +
                    " and tcodo_tnive_clave='" + Global.nivel + "' and tcodo_tcole_clave='" + Global.colegio + "'" +
                    " and tcodo_tmoda_clave='" + Global.modalidad + "' and tcodo_tprog_clave='" + Global.programa + "'" +
                    " and tcodo_ttiin_clave='" + Global.tipo_ing + "' and tcodo_estatus='" + Global.estatus + "'";
                MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                conexion.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(strCadSQL, conexion);
                mysqlcmd.CommandType = CommandType.Text;
                try
                {
                    mysqlcmd.ExecuteNonQuery();
                    //grid_tstdo_bind();
                    LlenaPagina();
                    combo_estatus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "update_p", "update();", true);
                    Gridtcodo.Visible = false;
                    btn_update.Visible = false;
                    btn_save.Visible = true;
                    //LlenaPagina();
                }
                catch (Exception ex)
                {
                    string test1 = ex.Message;
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "validar_campos_tcodo();", true);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string strQuery = "";
            strQuery = " select tcodo_tdocu_clave clave, tdocu_desc nombre,  " +
              " tcodo_tcamp_clave Campus, tcodo_tnive_clave Nivel, tcodo_tcole_clave Colegio," +
              " tcodo_tmoda_clave Modalidad, tcodo_tprog_clave Programa, tcodo_ttiin_clave Tipo, " +
              " tcodo_estatus c_estatus,CASE WHEN tcodo_estatus = 'A' THEN 'ACTIVO' ELSE 'INACTIVO' END Estatus, fecha(date_format(tcodo_date,'%Y-%m-%d')) fecha " +
              " from tcodo, tdocu " +
              " where tcodo_tdocu_clave=tdocu_clave ";

            if (txt_tcodo.Text != "")
            {
                strQuery = strQuery + " and tcodo_tdocu_clave='" + txt_tcodo.Text + "'";
            }
            if (ddl_campus.SelectedValue.ToString() != "000" )
            {
                strQuery = strQuery + " and tcodo_tcamp_clave='" + ddl_campus.SelectedValue.ToString() + "'";
            }
            if (ddl_nivel.SelectedValue.ToString() != "000")
            {
                strQuery = strQuery + " and tcodo_tnive_clave='" + ddl_nivel.SelectedValue.ToString() + "'";
            }
            if (ddl_colegio.SelectedValue.ToString() != "000")
            {
                strQuery = strQuery + " and tcodo_tcole_clave='" + ddl_colegio.SelectedValue.ToString() + "'";
            }
            if (ddl_modalidad.SelectedValue.ToString() != "000")
            {
                strQuery = strQuery + " and tcodo_tmoda_clave='" + ddl_modalidad.SelectedValue.ToString() + "'";
            }
            if (ddl_programa.SelectedValue.ToString() != "0000000000")
            {
                strQuery = strQuery + " and tcodo_tprog_clave='" + ddl_programa.SelectedValue.ToString() + "'";
            }
            if (ddl_tipo.SelectedValue.ToString() != "000")
            {
                strQuery = strQuery + " and tcodo_ttiin_clave='" + ddl_tipo.SelectedValue.ToString() + "'";
            }

            strQuery = strQuery + " order by clave ";

            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {

                
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(strQuery, conexion);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Tcodo");
                Gridtcodo.DataSource = ds;
                Gridtcodo.DataBind();
                Gridtcodo.DataMember = "Tcodo";

                if (Gridtcodo.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "remove_class", "remove_class();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "no_existe", "noexist();", true);
                }
                else
                {
                    Gridtcodo.HeaderRow.TableSection = TableRowSection.TableHeader;
                    Gridtcodo.UseAccessibleHeader = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable_tcodo", "load_datatable_tcodo();", true);
                    Gridtcodo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
        }

        protected bool valida_tcodo(string tcodo, string campus, string nivel, string colegio, string modalidad, string programa, string tipo)
        {
            string Query = "";
            Query = "SELECT COUNT(*) Indicador FROM tcodo WHERE tcodo_tdocu_clave='" + tcodo + "' and tcodo_tcamp_clave='" + campus +"' " +
                " and tcodo_tnive_clave='" + nivel + "' and tcodo_tcole_clave='" + colegio + "' and tcodo_tmoda_clave='" + modalidad + "' " +
                " and tcodo_tprog_clave='" + programa + "' and tcodo_ttiin_clave='" + tipo + "'" ;
            MySqlCommand cmd = new MySqlCommand(Query);
            DataTable dt = GetData(cmd);
            if (dt.Rows[0][0].ToString() != "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void Gridtdocu_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = Gridtdocu.SelectedRow;
            txt_tcodo.Text = row.Cells[1].Text;
            lbl_nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            Gridtdocu.Visible = false;
            //combo_estatus();
        }

            protected void Gridtcodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = Gridtcodo.SelectedRow;
            txt_tcodo.Text = row.Cells[1].Text;
            lbl_nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);

            combo_estatus();
            ddl_estatus.SelectedValue = row.Cells[9].Text;

            string strQuery = "";
            strQuery = " select tcodo_tdocu_clave, tdocu_desc nombre, tcodo_tcamp_clave campus, " +
                       " tcodo_tnive_clave nivel, tcodo_tcole_clave colegio, tcodo_tmoda_clave moda, tcodo_tprog_clave prog, tcodo_ttiin_clave tiin, " +
                       " fecha(date_format(tcodo_date, '%Y-%m-%d')), tcodo_estatus " +
                       " from tcodo, tdocu where tcodo_tdocu_clave='" + row.Cells[1].Text.ToString() + "'" +
                        " and tcodo_tcamp_clave ='" + row.Cells[3].Text.ToString() + "'" +
                        " and tcodo_tnive_clave ='" + row.Cells[4].Text.ToString() + "'" +
                        " and tcodo_tcole_clave ='" + row.Cells[5].Text.ToString() + "'" +
                        " and tcodo_tmoda_clave ='" + row.Cells[6].Text.ToString() + "'" +
                        " and tcodo_tprog_clave ='" + row.Cells[7].Text.ToString() + "'" +
                        " and tcodo_ttiin_clave ='" + row.Cells[8].Text.ToString() + "'" +
                        " and tcodo_tdocu_clave=tdocu_clave ";

            MySqlConnection conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            conexion.Open();
            try
            {

                MySqlDataAdapter sqladapter = new MySqlDataAdapter();

                DataSet dssql1 = new DataSet();

                MySqlCommand commandsql1 = new MySqlCommand(strQuery, conexion);
                sqladapter.SelectCommand = commandsql1;
                sqladapter.Fill(dssql1);
                sqladapter.Dispose();
                commandsql1.Dispose();

                string strQuerycampus = "select tcamp_clave clave, tcamp_desc campus from tcamp " +
                                 " union " +
                                 " select '000' clave, '---------' campus from dual " +
                                 " order by clave ";


                DataTable TablaCampus = new DataTable();
                MySqlCommand ConsultaMySql0 = new MySqlCommand();
                MySqlDataReader DatosMySql0;
                ConsultaMySql0.Connection = conexion;
                ConsultaMySql0.CommandType = CommandType.Text;
                ConsultaMySql0.CommandText = strQuerycampus;
                DatosMySql0 = ConsultaMySql0.ExecuteReader();
                TablaCampus.Load(DatosMySql0, LoadOption.OverwriteChanges);

                ddl_campus.DataSource = TablaCampus;
                ddl_campus.DataValueField = "clave";
                ddl_campus.DataTextField = "campus";
                ddl_campus.DataBind();

                ddl_campus.Items.FindByValue(row.Cells[3].Text.ToString()).Selected = true;

                string strQueryNivel = "select distinct tprog_tnive_clave clave, tnive_desc nivel from tcapr, tnive, tprog " +
                                " where tcapr_estatus='A' ";
                if (ddl_campus.SelectedValue != "000")
                {
                    strQueryNivel = strQueryNivel + " and tcapr_tcamp_clave='" + ddl_campus.SelectedValue.ToString() + "'";
                }
                strQueryNivel = strQueryNivel + " and tcapr_tprog_clave=tprog_clave and tprog_tnive_clave=tnive_clave " +
                                 " union " +
                                 " select '000' clave, '---------' nivel from dual " +
                                 " order by clave ";

                //resultado.Text = strQueryNivel;
                DataTable TablaNivel = new DataTable();
                MySqlCommand ConsultaMySql1 = new MySqlCommand();
                MySqlDataReader DatosMySql1;
                ConsultaMySql1.Connection = conexion;
                ConsultaMySql1.CommandType = CommandType.Text;
                ConsultaMySql1.CommandText = strQueryNivel;
                DatosMySql1 = ConsultaMySql1.ExecuteReader();
                TablaNivel.Load(DatosMySql1, LoadOption.OverwriteChanges);

                ddl_nivel.DataSource = TablaNivel;
                ddl_nivel.DataValueField = "clave";
                ddl_nivel.DataTextField = "nivel";
                ddl_nivel.DataBind();

                ddl_nivel.Items.FindByValue(row.Cells[4].Text.ToString()).Selected = true;

                string strQueryCole = "";
                strQueryCole = "select distinct tprog_tcole_clave clave, tcole_desc nombre from tcapr, tcole, tprog " +
                                " where tcapr_estatus='A' ";
                if (ddl_campus.SelectedValue.ToString() != "000")
                {
                    strQueryCole = strQueryCole + " and tcapr_tcamp_clave='" + ddl_campus.SelectedValue + "'";
                }
                if (ddl_nivel.SelectedValue.ToString() != "000")
                {
                    strQueryCole = strQueryCole + " and tprog_tnive_clave='" + ddl_nivel.SelectedValue + "'";
                }
                strQueryCole = strQueryCole + " and tcapr_tprog_clave=tprog_clave and tprog_tcole_clave=tcole_clave " +
                                     " union " +
                                     " select '000' clave, '---------' nombre from dual " +
                                     " order by clave ";

                DataTable TablaCole = new DataTable();
                MySqlCommand ConsultaMySql = new MySqlCommand();
                MySqlDataReader DatosMySql;
                ConsultaMySql.Connection = conexion;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = strQueryCole;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaCole.Load(DatosMySql, LoadOption.OverwriteChanges);

                ddl_colegio.DataSource = TablaCole;
                ddl_colegio.DataValueField = "clave";
                ddl_colegio.DataTextField = "nombre";
                ddl_colegio.DataBind();

                ddl_colegio.Items.FindByValue(row.Cells[5].Text.ToString()).Selected = true;

                string strQueryModa = "";
                strQueryModa = "select distinct tprog_tmoda_clave clave, tmoda_desc nombre from tcapr, tmoda, tprog " +
                                " where tcapr_estatus='A' ";
                if (ddl_campus.SelectedValue.ToString() != "000")
                {
                    strQueryModa = strQueryModa + " and tcapr_tcamp_clave='" + ddl_campus.SelectedValue + "'";
                }
                if (ddl_nivel.SelectedValue.ToString() != "000")
                {
                    strQueryModa = strQueryModa + " and tprog_tnive_clave='" + ddl_nivel.SelectedValue + "'";
                }
                if (ddl_colegio.SelectedValue.ToString() != "000")
                {
                    strQueryModa = strQueryModa + " and tprog_tcole_clave='" + ddl_colegio.SelectedValue + "'";
                }
                strQueryModa = strQueryModa +
                " and tcapr_tprog_clave=tprog_clave and tprog_tmoda_clave=tmoda_clave " +
                                     " union " +
                                     " select '000' clave, '---------' nombre from dual " +
                                     " order by clave ";

                DataTable TablaModa = new DataTable();
                MySqlCommand ConsultaMySql2 = new MySqlCommand();
                MySqlDataReader DatosMySql2;
                ConsultaMySql2.Connection = conexion;
                ConsultaMySql2.CommandType = CommandType.Text;
                ConsultaMySql2.CommandText = strQueryModa;
                DatosMySql2 = ConsultaMySql2.ExecuteReader();
                TablaModa.Load(DatosMySql2, LoadOption.OverwriteChanges);

                ddl_modalidad.DataSource = TablaModa;
                ddl_modalidad.DataValueField = "clave";
                ddl_modalidad.DataTextField = "nombre";
                ddl_modalidad.DataBind();

                ddl_modalidad.Items.FindByValue(row.Cells[6].Text.ToString()).Selected = true;

                string strQueryProg = "";
                strQueryProg = "select distinct tprog_clave clave, tprog_desc nombre from tcapr,  tprog " +
                                " where tcapr_estatus='A' ";
                if (ddl_campus.SelectedValue.ToString() != "000")
                {
                    strQueryProg = strQueryProg + " and tcapr_tcamp_clave='" + ddl_campus.SelectedValue + "'";
                }
                if (ddl_nivel.SelectedValue.ToString() != "000")
                {
                    strQueryProg = strQueryProg + " and tprog_tnive_clave='" + ddl_nivel.SelectedValue + "'";
                }
                if (ddl_colegio.SelectedValue.ToString() != "000")
                {
                    strQueryProg = strQueryProg + " and tprog_tcole_clave='" + ddl_colegio.SelectedValue + "'";
                }
                if (ddl_modalidad.SelectedValue.ToString() != "000")
                {
                    strQueryProg = strQueryProg + " and tprog_tmoda_clave='" + ddl_modalidad.SelectedValue + "'";
                }
                strQueryProg = strQueryProg +
                " and tcapr_tprog_clave=tprog_clave " +
                                     " union " +
                                     " select '0000000000' clave, '---------' nombre from dual " +
                                     " order by clave ";

                DataTable TablaProg = new DataTable();
                MySqlCommand ConsultaMySql3 = new MySqlCommand();
                MySqlDataReader DatosMySql3;
                ConsultaMySql3.Connection = conexion;
                ConsultaMySql3.CommandType = CommandType.Text;
                ConsultaMySql3.CommandText = strQueryProg;
                DatosMySql3 = ConsultaMySql3.ExecuteReader();
                TablaProg.Load(DatosMySql3, LoadOption.OverwriteChanges);

                ddl_programa.DataSource = TablaProg;
                ddl_programa.DataValueField = "clave";
                ddl_programa.DataTextField = "nombre";
                ddl_programa.DataBind();

                ddl_programa.Items.FindByValue(row.Cells[7].Text.ToString()).Selected = true;

                string strQueryTiin = "";
                strQueryTiin = "select distinct ttiin_clave clave, ttiin_desc nombre from ttiin " +
                                " where ttiin_estatus='A' " +
                                     " union " +
                                     " select '000' clave, '---------' nombre from dual " +
                                     " order by clave ";

                DataTable TablaTiin = new DataTable();
                MySqlCommand ConsultaMySql4 = new MySqlCommand();
                MySqlDataReader DatosMySql4;
                ConsultaMySql4.Connection = conexion;
                ConsultaMySql4.CommandType = CommandType.Text;
                ConsultaMySql4.CommandText = strQueryTiin;
                DatosMySql4 = ConsultaMySql4.ExecuteReader();
                TablaTiin.Load(DatosMySql4, LoadOption.OverwriteChanges);

                ddl_tipo.DataSource = TablaTiin;
                ddl_tipo.DataValueField = "clave";
                ddl_tipo.DataTextField = "nombre";
                ddl_tipo.DataBind();

                ddl_tipo.Items.FindByValue(row.Cells[8].Text.ToString()).Selected = true;

                /* if (dssql1.Tables[0].Rows[0][9].ToString() == "A")
                 {
                     CboEstatus.Items.FindByText("Activo").Selected = true;
                 }
                 if (dssql1.Tables[0].Rows[0][9].ToString() == "B")
                 {
                     CboEstatus.Items.FindByText("Baja").Selected = true;
                 }*/

                Global.campus= row.Cells[3].Text.ToString();
                Global.nivel = row.Cells[4].Text.ToString();
                Global.colegio = row.Cells[5].Text.ToString();
                Global.modalidad= row.Cells[6].Text.ToString();
                Global.programa = row.Cells[7].Text.ToString();
                Global.tipo_ing = row.Cells[8].Text.ToString();
                Global.estatus = row.Cells[9].Text.ToString();

                btn_update.Visible = true;
                btn_save.Visible = false;
                txt_tcodo.Attributes.Add("readonly", "");
                grid_tcodo_bind();
            }
            catch (Exception ex)
            {
                //resultado.Text = ex.Message;

            }
            conexion.Close();
        }
    }
}