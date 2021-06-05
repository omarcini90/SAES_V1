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
    public partial class tcseq : System.Web.UI.Page
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

                //c_prog_campus.Attributes.Add("onblur", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                //c_prog_campus.Attributes.Add("oninput", "validarclavePrograma('ContentPlaceHolder1_c_prog_campus')");
                //LlenaPagina();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "menu", "carga_menu();", true);
                if (!IsPostBack)
                {
                    combo_campus();
                }

            }
        }

        protected void combo_campus()
        {
            search_campus.Items.Clear();
            string Query = "SELECT DISTINCT tcamp_clave Clave, tcamp_desc Campus FROM tcamp " +
                            "UNION " +
                            "SELECT DISTINCT '0','--------' Campus  " +
                            "ORDER BY 1";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();
            DataTable TablaCampus = new DataTable();
            MySqlCommand ConsultaMySql = new MySqlCommand();
            MySqlDataReader DatosMySql;
            try
            {
                ConsultaMySql.Connection = ConexionMySql;
                ConsultaMySql.CommandType = CommandType.Text;
                ConsultaMySql.CommandText = Query;
                DatosMySql = ConsultaMySql.ExecuteReader();
                TablaCampus.Load(DatosMySql, LoadOption.OverwriteChanges);
                search_campus.DataSource = TablaCampus;
                search_campus.DataValueField = "Clave";
                search_campus.DataTextField = "Campus";
                search_campus.DataBind();

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

        protected void grid_secuencia_bind()
        {
            string strQueryCuenta = " select tseqn_clave seq, tseqn_desc nombre, tcseq_numero, tcseq_longitud, tcamp_desc campus from tseqn " +
                " left outer join tcseq on tcseq_tcamp_clave ='" + search_campus.SelectedValue + "' and tseqn_clave = tcseq_tseqn_clave " +
                " inner join tcamp on tcamp_clave=tcseq_tcamp_clave "+
                " where tseqn_tipo='C' order by tseqn_clave";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
            ConexionMySql.Open();

            try
            {
                
                DataSet ds1 = new DataSet();
                MySqlDataAdapter dataadapter2 = new MySqlDataAdapter(strQueryCuenta, ConexionMySql);
                dataadapter2.Fill(ds1, "Plan");
                GridSequence.DataSource = ds1;
                GridSequence.DataBind();
                GridSequence.DataMember = "Plan";
                GridSequence.HeaderRow.TableSection = TableRowSection.TableHeader;
                GridSequence.UseAccessibleHeader = true;
                

                for (int i = 0; i < GridSequence.Rows.Count; i++)
                {
                    TextBox numero = (TextBox)GridSequence.Rows[i].FindControl("valor");
                    TextBox largo = (TextBox)GridSequence.Rows[i].FindControl("longitud");

                    numero.Text = ds1.Tables[0].Rows[i][2].ToString();
                    largo.Text = ds1.Tables[0].Rows[i][3].ToString();

                }
                
                GridSequence.Visible = true;
                btn_seq.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);

                ConexionMySql.Close();
            }
            catch (Exception ex)
            {
                ///logs
            }
        }

        protected void search_campus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (search_campus.SelectedValue != "0")
            {
                grid_secuencia_bind();
            }
            else
            {
                GridSequence.Visible = false;
            }
            
        }

        protected void cancelar_seq_Click(object sender, EventArgs e)
        {
            grid_secuencia_bind();
        }

        protected void guardar_seq_Click(object sender, EventArgs e)
        {
            string indicador = null;
            for (int i = 0; i < GridSequence.Rows.Count; i++)
            {
                TextBox numero = (TextBox)GridSequence.Rows[i].FindControl("valor");
                TextBox largo = (TextBox)GridSequence.Rows[i].FindControl("longitud");
                if(!String.IsNullOrEmpty(numero.Text) && !String.IsNullOrEmpty(largo.Text))
                {
                    string Query = "UPDATE tcseq SET tcseq_numero = '" + numero.Text + "', tcseq_longitud = '" + largo.Text + "', tcseq_date = current_timestamp(), tcseq_user = '" + Session["usuario"].ToString() + "' WHERE tcseq_tcamp_clave = '" + search_campus.SelectedValue + "' AND tcseq_tseqn_clave = '" + GridSequence.Rows[i].Cells[0].Text + "'";
                    MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionStringSAES"].ConnectionString);
                    ConexionMySql.Open();
                    MySqlCommand mysqlcmd = new MySqlCommand(Query, ConexionMySql);
                    mysqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        mysqlcmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        ///logs
                    }
                    indicador += "0";
                }
                else
                {
                    GridSequence.HeaderRow.TableSection = TableRowSection.TableHeader;
                    GridSequence.UseAccessibleHeader = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "validarValor", "validarValor('ContentPlaceHolder1_GridSequence_valor_"+i+"');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "validarLongitud", "validarLongitud('ContentPlaceHolder1_GridSequence_longitud_" + i + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
                    indicador += "1";
                }
                
            }

            if (!indicador.Contains("1"))
            {
                grid_secuencia_bind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Guardar", "save();", true);
            }
            
        }
    }
}