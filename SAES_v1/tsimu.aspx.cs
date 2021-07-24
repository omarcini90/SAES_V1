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
    public partial class tsimu : System.Web.UI.Page
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

                if (!IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Session["matricula"] as string) && !string.IsNullOrEmpty(Session["nombre"] as string) && !string.IsNullOrEmpty(Session["periodo"] as string) && !string.IsNullOrEmpty(Session["campus"] as string) && !string.IsNullOrEmpty(Session["programa"] as string) && !string.IsNullOrEmpty(Session["tipo_ingreso"] as string) && !string.IsNullOrEmpty(Session["tasa"] as string))
                    {

                        ddl_periodo.Attributes.Add("disabled", "");
                        ddl_Campus.Attributes.Add("disabled", "");
                        ddl_Programa.Attributes.Add("disabled", "");
                        ddl_tipo_ingreso.Attributes.Add("disabled", "");
                        ddl_tasa_f.Attributes.Add("disabled", "");

                        txt_matricula.Text = Session["matricula"].ToString();
                        txt_nombre.Text = Session["nombre"].ToString();
                        combo_periodo();
                        ddl_periodo.SelectedValue = Session["periodo"].ToString();
                        combo_campus();
                        ddl_Campus.SelectedValue = Session["campus"].ToString();
                        combo_Programa();
                        ddl_Programa.SelectedValue = Session["programa"].ToString();
                        combo_tipo_ingreso();
                        ddl_tipo_ingreso.SelectedValue = Session["tipo_ingreso"].ToString();
                        combo_tasa_financiera();
                        ddl_tasa_f.SelectedValue = Session["tasa"].ToString();
                        combo_plan_beca();
                    }
                }
            }
           

        }

        protected void combo_periodo()
        {
            string strQuerydire = "";
            strQuerydire = "select tpees_clave clave, tpees_desc periodo from tpees where tpees_estatus='A' and tpees_fin >= curdate() " +
                            "union " +
                            "select '0' clave, '----Selecciona un periodo----' periodo " +
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

                ddl_periodo.DataSource = TablaEstado;
                ddl_periodo.DataValueField = "Clave";
                ddl_periodo.DataTextField = "Periodo";
                ddl_periodo.DataBind();

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
        protected void combo_campus()
        {
            ddl_Programa.Items.Clear();
            ddl_Programa.Items.Add(new ListItem("----Selecciona un programa----", "0"));
            string strQuerydire = "";
            strQuerydire = "select tcamp_clave clave, tcamp_desc campus from tcamp where tcamp_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona un campus----' campus " +
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

                ddl_Campus.DataSource = TablaEstado;
                ddl_Campus.DataValueField = "Clave";
                ddl_Campus.DataTextField = "Campus";
                ddl_Campus.DataBind();

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
        protected void combo_Programa()
        {
            string strQuerydire = "";
            strQuerydire = "select distinct tprog_clave clave, tprog_desc programa from tcapr, tprog " +
                            " where tcapr_estatus='A' and tcapr_tcamp_clave='" + ddl_Campus.SelectedValue + "'" +
                            " and tcapr_tprog_clave=tprog_clave and tprog_ind_admi='X'  " +
                            "union " +
                            "select '0' clave, '----Selecciona un programa----' programa " +
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

                ddl_Programa.DataSource = TablaEstado;
                ddl_Programa.DataValueField = "Clave";
                ddl_Programa.DataTextField = "Programa";
                ddl_Programa.DataBind();

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
        protected void combo_tipo_ingreso()
        {
            string strQuerydire = "";
            strQuerydire = "select ttiin_clave clave, ttiin_desc Tipo from ttiin where ttiin_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona un tipo de ingreso----' Tipo " +
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

                ddl_tipo_ingreso.DataSource = TablaEstado;
                ddl_tipo_ingreso.DataValueField = "Clave";
                ddl_tipo_ingreso.DataTextField = "Tipo";
                ddl_tipo_ingreso.DataBind();

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
        protected void combo_tasa_financiera()
        {
            string strQuerydire = "";
            strQuerydire = "select ttasa_clave clave, ttasa_desc Tasa from ttasa where ttasa_estatus='A' " +
                            "union " +
                            "select '0' clave, '----Selecciona una tasa financiera----' Tasa " +
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

                ddl_tasa_f.DataSource = TablaEstado;
                ddl_tasa_f.DataValueField = "Clave";
                ddl_tasa_f.DataTextField = "Tasa";
                ddl_tasa_f.DataBind();

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
        protected void combo_plan_beca()
        {
            string Query = "select distinct tpcbe_clave clave, tpcbe_desc Plan from tsimu, tpcbe, tprog " +
                " where tsimu_id = '" + txt_matricula.Text + "' and tsimu_tcoco_clave = tpcbe_tcoco_cargo " +
                " and tpcbe_tcamp_clave='" + ddl_Campus.SelectedValue + "' and tprog_clave='" + ddl_Programa.SelectedValue + "' " +
                " and (tpcbe_tnive_clave=tprog_tnive_clave or tpcbe_tnive_clave='000')  " +
                " union " +
                " select '0' clave, '----Selecciona un plan o beca----' Plan from dual order by clave";

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

                ddl_plan_beca.DataSource = TablaEstado;
                ddl_plan_beca.DataValueField = "Clave";
                ddl_plan_beca.DataTextField = "Plan";
                ddl_plan_beca.DataBind();

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
        protected void grid_simulador_bind()
        {
            //Codigo que llena la tabla del simulador. 
        }
       
        protected void back_Click(object sender, EventArgs e)
        {
            Response.Redirect("tadmi.aspx");
        }
    }
}