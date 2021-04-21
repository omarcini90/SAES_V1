﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1.Repositorio
{
    public partial class ListadoExpediente : System.Web.UI.Page
    {
        applyWeb.Data.Data objExpediente = new applyWeb.Data.Data(System.Configuration.ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
        string id_page_ex;
        string id_page;
        string id_page_reload;
        public double porcentaje;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                permisos();
            }
            catch
            {
                Response.Redirect("../Default.aspx");
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
                Response.End();
            }
            if (Session["Rol"] == null || (Session["Rol"].ToString().Equals("Alumno")))
            {
                Response.Redirect("../Default.aspx");
            }
            else
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (!IsPostBack)
                {
                    try
                    {
                        progress_bar();
                        carga_nombre_alumno();
                        CargaListaExpediente(Request.QueryString["IDAlumno"].ToString());

                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("../Default.aspx");
                    }
                }

            }
            

            string id_archivo = Convert.ToString(Request.QueryString["id_archivo"]);
            if (id_archivo != null)
            {
                elimina_preview(id_archivo, Request.QueryString["IDAlumno"].ToString());
            }

        }

        private DataTable GetData(MySqlCommand cmd)
        {
            DataTable dt = new DataTable();
            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;
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

        protected void CargaListaExpediente(string pIDAlumno)
        {
            try
            {
                ArrayList arrParametros = new ArrayList();
                arrParametros.Add(new applyWeb.Data.Parametro("@IDAlumno_in", pIDAlumno));
                arrParametros.Add(new applyWeb.Data.Parametro("@Rol", Session["Rol"].ToString()));
                //Session["AlumnoInperson"] = pIDAlumno;

                DataSet dsExpedientes = objExpediente.ExecuteSP("Obtener_Listado_Documentos_Alumno", arrParametros);
                gvExpediente.DataSource = dsExpedientes;
                gvExpediente.DataBind();
                gvExpediente.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvExpediente.UseAccessibleHeader = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "load_datatable", "load_datatable();", true);
            }
            catch (Exception e)
            {
                string text = e.Message + e.ToString() + e.StackTrace.ToString();
            }
        }

        protected void gvExpediente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Subir")
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvExpediente.Rows[index];
                string IDTipoDocumento = row.Cells[0].Text;
                string IDDocumento = row.Cells[1].Text;
                string IDAlumno = row.Cells[2].Text;
                if (IDDocumento != "0")
                {
                    
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "cargar_doc('InputFile.aspx?IDTipoDocumento=" + IDTipoDocumento + "&IDDocumento=" + IDDocumento + "&IDAlumno=" + IDAlumno + "','ListadoExpediente.aspx?IDAlumno=" + IDAlumno + "')", true);
                }
                else
                {
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "JQDialog('InputFile.aspx?IDTipoDocumento=" + IDTipoDocumento + "&IDDocumento=" + IDDocumento + "&IDAlumno=" + IDAlumno + "','ListadoExpediente.aspx?IDAlumno=" + IDAlumno + "');", true);
                }


            }
            else if (e.CommandName == "Preview")
            {
                DirectoryInfo vPath = new DirectoryInfo(Server.MapPath("~/UploadedFiles/"));
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvExpediente.Rows[index];
                string IDTipoDocumento = row.Cells[0].Text;
                string IDDocumento = row.Cells[1].Text;
                string IDAlumno = row.Cells[2].Text;
                cargar_vista_previa(IDTipoDocumento, IDDocumento, IDAlumno);
                
            }
            else if (e.CommandName == "Comentarios")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvExpediente.Rows[index];
                string IDDocumento = row.Cells[1].Text;
                string IDAlumno = row.Cells[2].Text;
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "JQDialog_Comentarios('Comentarios.aspx?IDDocumento=" + IDDocumento + "','ListadoExpediente.aspx?IDAlumno=" + IDAlumno +"');", true);

            }
        }

        private void cargar_vista_previa(string IdTipoDocumento, string IDDocumento, string IDAlumno)
        {
            DirectoryInfo vPath = new DirectoryInfo(Server.MapPath("~/UploadedFiles/"));
            string strQuery = "SELECT DISTINCT Documento,Formato FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento AND IDTipoDocumento=@IDTipoDocumento AND IDAlumno=@IDAlumno";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            cmd.Parameters.Add("@IDTipoDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IdTipoDocumento);
            cmd.Parameters.Add("@IDAlumno", MySqlDbType.VarChar).Value = IDAlumno;
            DataTable dt = GetData(cmd);
            if (dt != null)
            {
                Byte[] bytes = (Byte[])dt.Rows[0]["Documento"];
                string extension = "." + dt.Rows[0]["Formato"];
                string path = vPath.ToString() + IdTipoDocumento + "_" + IDDocumento + "_" + IDAlumno + extension;
                File.WriteAllBytes(path, bytes);
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "JQDialog_preview('Preview_Page.aspx?id_archivo=" + IdTipoDocumento + "_" + IDDocumento + "_" + IDAlumno + extension + "&formato=" + extension + "&IDDocumento=" + IDDocumento + "&IDTipoDocumento=" + IdTipoDocumento + "','ListadoExpediente.aspx?IDAlumno=" + IDAlumno + "&id_archivo=" + IdTipoDocumento + "_" + IDDocumento + "_" + IDAlumno + extension + "');", true);

            }

        }

        protected void gvExpediente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IDDocumento = e.Row.Cells[1].Text;
                string Estatus = e.Row.Cells[4].Text;
                ImageButton preview = e.Row.FindControl("imgPreview") as ImageButton;
                ImageButton subir = e.Row.FindControl("imgExpediente") as ImageButton;
                ImageButton comentarios = e.Row.FindControl("imgComentarios") as ImageButton;
                if (IDDocumento != "0" && Estatus == "ACEPTADO")
                {
                    preview.Visible = true;
                    subir.Visible = false;
                    comentarios.Visible = true;
                }
                else if (IDDocumento != "0" && Estatus != "ACEPTADO")
                {
                    preview.Visible = true;
                    comentarios.Visible = true;
                }
                else
                {
                    preview.Visible = false;
                    comentarios.Visible = false;
                }
            }
        }

        private void elimina_preview(string id_archivo, string IDAlumno)
        {
            string id_page_reload_1 = Convert.ToString(Request.QueryString["PageEx"]);
            DirectoryInfo vPath = new DirectoryInfo(Server.MapPath("~/UploadedFiles/"));
            File.Delete(vPath.ToString() + id_archivo);
            
            Response.Redirect("ListadoExpediente.aspx?IDAlumno=" + IDAlumno );
        }

        protected void carga_nombre_alumno()
        {
            string IDAlumno = Request.QueryString["IDAlumno"].ToString();
            string strQuery = "SELECT DISTINCT Nombre FROM Alumno WHERE IDAlumno=@IDAlumno";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDAlumno", MySqlDbType.VarChar).Value = IDAlumno;
            DataTable dt = GetData(cmd);
            Nombre_Alumno.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dt.Rows[0]["Nombre"].ToString());

        }


        protected void permisos()
        {
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            ConexionMySql.Open();
            string strQuery = "SELECT DISTINCT A.IDPrivilegio,b.Permiso FROM Permisos_App_Rol A INNER JOIN Permisos_App B ON A.IDPrivilegio=B.IDPrivilegio INNER JOIN Rol C ON A.IDRol=C.IDRol WHERE B.IDMenu=3 AND B.IDSubMenu=1 AND C.Nombre='" + Session["Rol"].ToString() + "'";
            MySqlCommand cmd = new MySqlCommand(strQuery, ConexionMySql);
            MySqlDataReader dr = cmd.ExecuteReader();
            gvExpediente.Columns[6].Visible = false;
            while (dr.Read())
            {
                int IDprivilegio = dr.GetInt32(0);

                if (IDprivilegio == 19) { gvExpediente.Columns[6].Visible = true; } //Permiso para subir documentos


            }
            ConexionMySql.Close();
        }

        protected void progress_bar()
        {
            string IDAlumno = Convert.ToString(Request.QueryString["IDAlumno"]);


            string strQuery = "SELECT DISTINCT COUNT(TDA.IDTipoDocumento)Documentos, " +
                              "(SELECT COUNT(*) FROM Documentos_Alumno WHERE IDEstatusDocumento in (17,3) AND IDAlumno='" + IDAlumno + "')Entregados " +
                              "FROM TiposDocumento_nivel TDA " +
                              "JOIN TipoDocumento TD ON TD.IDTipoDocumento = TDA.IDTipoDocumento " +
                              "JOIN Alumno AL ON AL.CodigoProcedencia = TDA.IDProcedencia AND AL.CodigoTipoIngreso = TDA.IDTipoIngreso AND AL.IDNivel=TDA.IDNivel  AND AL.IDModalidad=TDA.IDModalidad AND AL.IDAlumno = '" + IDAlumno + "' ";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand(strQuery);
            DataTable dt = GetData(cmd);
            double Entregados = Convert.ToDouble(dt.Rows[0]["Entregados"]);
            double test_2 = Convert.ToDouble(dt.Rows[0]["Documentos"]);
            porcentaje = (Convert.ToDouble(dt.Rows[0]["Entregados"]) * 100) / Convert.ToDouble(dt.Rows[0]["Documentos"]);

            lbl_bar.Text = "Documentos entregados " + dt.Rows[0]["Entregados"].ToString() + " de " + dt.Rows[0]["Documentos"].ToString() + " (" + Math.Round(porcentaje).ToString() + "%)";

            if (Math.Round(porcentaje) < 40)
            {
                html_progress_bar.Attributes.Add("style", "width:" + Math.Round(porcentaje).ToString() + "%;color:#000;");
                html_progress_bar.Attributes.Add("class", "progress-bar progress-bar-striped progress-bar-animated");
            }
            else if (Math.Round(porcentaje) > 90)
            {
                html_progress_bar.Attributes.Add("style", "width:" + Math.Round(porcentaje).ToString() + "%");
                html_progress_bar.Attributes.Add("class", "progress-bar progress-bar-striped progress-bar-animated bg-success");
            }
            else
            {
                html_progress_bar.Attributes.Add("style", "width:" + Math.Round(porcentaje).ToString() + "%");
                html_progress_bar.Attributes.Add("class", "progress-bar progress-bar-striped progress-bar-animated");
            }
        }
    }
}