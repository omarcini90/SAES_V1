﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1.Repositorio
{
    public partial class Preview_Page : System.Web.UI.Page
    {
        public string ruta_tmp;
        applyWeb.Data.Data objAreas = new applyWeb.Data.Data(System.Configuration.ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            //carga_estatus();

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
                Response.End();
            }
            if (Session["Rol"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            if (Session["Rol"].ToString() == "Alumno")
            {
                UpdatePanel.Visible = false;
            }
            else
            {
                permisos();
            }
            string valor = Convert.ToString(Request.QueryString["id_archivo"]);
            string ext = Convert.ToString(Request.QueryString["Formato"]);
            if (valor == null || ext == null)
            {
                Response.Redirect("../Default.aspx");
            }

            string contenttype = null;
            HttpContext context = HttpContext.Current;
            string baseUrl = context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/');
            string ruta = "../UploadedFiles/" + valor;
            string base_url_docs = "http://" + baseUrl + "/" + ruta;
            //string docs = "http://docs.google.com/viewer?url=";
            string docs_office = "https://view.officeapps.live.com/op/embed.aspx?src=";


            switch (ext)
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    ruta_tmp = "<embed src=\"" + docs_office + base_url_docs + "\" width=\"100%\" height=\"500px\">";
                    break;
                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    ruta_tmp = "<embed src=\"" + docs_office + base_url_docs + "\" width=\"100%\" height=\"500px\">";
                    break;
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    ruta_tmp = "<embed src=\"" + docs_office + base_url_docs + "\" width=\"100%\" height=\"500px\">";
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    ruta_tmp = "<embed src=\"" + docs_office + base_url_docs + "\" width=\"100%\" height=\"500px\">";
                    //ruta_tmp="<iframe src = \"" + docs_office + base_url_docs + "\" width = \"100%\" height = \"500px\" frameborder = \"0\" ></ iframe >";

                    break;
                case ".jpg":
                    contenttype = "image/jpg";
                    ruta_tmp = "<img alt=\"\" src=\"" + ruta + "\" width=\"90%\" />";
                    break;
                case ".png":
                    contenttype = "image/png";
                    ruta_tmp = "<img alt=\"\" src=\"" + ruta + "\" width=\"90%\" />";
                    break;
                case ".gif":
                    contenttype = "image/gif";
                    ruta_tmp = "<img alt=\"\" src=\"" + ruta + "\" width=\"90%\" />";
                    break;
                case ".jpeg":
                    contenttype = "image/gif";
                    ruta_tmp = "<img alt=\"\" src=\"" + ruta + "\" width=\"90%\" />";
                    break;

                case ".pdf":
                    contenttype = "application/pdf";
                    ruta_tmp = "<embed src=\"" + ruta + "#toolbar=1\" width=\"100%\" height=\"500px\">";
                    break;
            }

            if (!IsPostBack)
            {
                carga_estatus();


            }

        }
        protected void carga_estatus()
        {
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            string Query = "";
            string IDTipoDocumento = Convert.ToString(Request.QueryString["IDTipoDocumento"]);

            Query = "SELECT A.IDEstatus,A.Nombre " +
                    "FROM Estatus A " +
                    "INNER JOIN Permisos_Documentos_Estatus B ON B.IDEstatus = A.IDEstatus " +
                    "INNER JOIN Rol C ON C.IDRol=B.IDRol " +
                    "WHERE B.IDTipoDocumento = '" + IDTipoDocumento + "' AND C.Nombre = '" + Session["Rol"].ToString() + "'";

            MySqlDataAdapter MySqladapter = new MySqlDataAdapter(Query, ConexionMySql);
            DataTable dt = new DataTable();
            MySqladapter.Fill(dt);
            DropDownEstatus.DataSource = dt;
            DropDownEstatus.DataBind();
            DropDownEstatus.DataTextField = "Nombre";
            DropDownEstatus.DataValueField = "IDEstatus";
            DropDownEstatus.DataBind();
            DropDownEstatus.Items.Insert(0, new ListItem("Seleccionar Estatus", "0"));

        }
        protected void Guarda_Rev_Click(object sender, EventArgs e)
        {
            string comentarios = TextBox1.Text;
            string IDEstatus = DropDownEstatus.SelectedValue.ToString();
            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string strQuery = "SELECT DISTINCT IDTipoDocumento FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            DataTable dt = GetData(cmd);



            try
            {

                if (IDEstatus == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "slide_down_alert();", true);
                }
                else if (valida_estatus())
                {
                    //actualiza_banner_actualiza();

                    if (IDEstatus == "1")
                    {
                        //actualiza_banner();
                    }
                    else if (IDEstatus == "6" && dt.Rows[0]["IDTipoDocumento"].ToString() == "4" || IDEstatus == "6" && dt.Rows[0]["IDTipoDocumento"].ToString() == "9")
                    {
                        documento_invalido(comentarios);
                    }
                    else
                    {
                        inserta_estatus(comentarios, IDEstatus);
                        DropDownEstatus.ClearSelection();
                        TextBox1.Text = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "slide_up_save();", true);
                    }
                }
                else
                {
                    if (IDEstatus == "1")
                    {
                        //actualiza_banner();
                    }
                    else if (IDEstatus == "6" && dt.Rows[0]["IDTipoDocumento"].ToString() == "4" || IDEstatus == "6" && dt.Rows[0]["IDTipoDocumento"].ToString() == "9")
                    {
                        documento_invalido(comentarios);
                    }
                    else
                    {
                        inserta_estatus(comentarios, IDEstatus);
                        DropDownEstatus.ClearSelection();
                        TextBox1.Text = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "slide_up_save();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                DirectoryInfo virtualDirPath = new DirectoryInfo(Server.MapPath("~/Logs/"));
                StreamWriter sw = new StreamWriter(virtualDirPath + Convert.ToString(Request.QueryString["id_archivo"]) + ".txt", true);
                sw.WriteLine(ex.Message.ToString());
                sw.Close();
            }

        }
        protected void inserta_estatus(string comentarios, string IDEstatusDocumento)
        {
            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string strQuery = "SELECT DISTINCT IDAlumno FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            DataTable dt = GetData(cmd);


            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            MySqlCommand cmd1 = new MySqlCommand("Actualiza_Estatus_Comentario", ConexionMySql);
            cmd1.Parameters.Add("@IDDocumento_in", MySqlDbType.Int32).Value = IDDocumento;
            cmd1.Parameters.Add("@Comentario_in", MySqlDbType.VarChar).Value = comentarios;
            cmd1.Parameters.Add("@IDEstatusDoc_in", MySqlDbType.Int32).Value = Convert.ToInt32(IDEstatusDocumento);
            cmd1.Parameters.Add("@User_in", MySqlDbType.VarChar).Value = Session["user"].ToString();
            cmd1.CommandType = CommandType.StoredProcedure;
            //cmd1.Connection = ConexionMySql;
            ConexionMySql.Open();
            cmd1.ExecuteNonQuery();

            ConexionMySql.Close();
            if (IDEstatusDocumento != "6")
            {
                if (IDEstatusDocumento == "2")
                {
                    string idestatus = "5";
                    envia_correo_estatus(idestatus, comentarios);
                }
                else
                {
                    envia_correo_estatus(IDEstatusDocumento, comentarios);
                }
            }
            actualiza_estatus_expediente(IDEstatusDocumento);
            log(dt.Rows[0]["IDAlumno"].ToString(), IDDocumento, IDEstatusDocumento);

        }
        protected void DropDownEstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (valida_estatus())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "slide_down_estatus();", true);
            }
        }
        protected bool valida_estatus()
        {

            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string strQuery = "SELECT DISTINCT IDEstatusDocumento FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            DataTable dt = GetData(cmd);
            if (dt.Rows[0]["IDEstatusDocumento"].ToString() == "1")
            {
                //RevisaDoc.Visible = false;
                //warning.Visible = true;
                return true;
            }
            else
            {
                //RevisaDoc.Visible = true;
                //warning.Visible = false;
                return false;
            }

        }
        protected void documento_invalido(string comentario)
        {
            string IDEstatus = DropDownEstatus.SelectedValue.ToString();
            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string strQuery = "SELECT DISTINCT IDAlumno FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            DataTable dt = GetData(cmd);
            if (dt != null)
            {
                //string IDAlumno = dt.Rows[0]["IDAlumno"].ToString();
                //string strQuery_banner = "";
                //strQuery_banner = "SELECT DISTINCT SPRIDEN_PIDM FROM SPRIDEN WHERE SPRIDEN_CHANGE_IND IS NULL AND SPRIDEN_ID='" + IDAlumno + "'";
                //OracleConnection ConexionBanner = new OracleConnection(ConfigurationManager.ConnectionStrings["ConexionNOAH"].ConnectionString);
                //ConexionBanner.Open();
                //OracleDataReader DatosBanner;
                //var ConsultaOracle = new OracleCommand();
                //ConsultaOracle.CommandType = CommandType.Text;
                //ConsultaOracle.CommandText = strQuery_banner;
                //ConsultaOracle.Connection = ConexionBanner;
                //DatosBanner = ConsultaOracle.ExecuteReader();

                //DatosBanner.Read();
                //int Pidm = DatosBanner.GetInt32(0);
                //inserta_invalido_banner(Pidm, comentario);
                //ConexionBanner.Close();
            }
            envia_correo_estatus(IDEstatus, "");
        }
        protected void envia_correo_estatus(string Estatus, string Comentarios)
        {
            //string IDEstatus = DropDownEstatus.SelectedValue.ToString();
            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string IDAlumno;

            if (Session["Rol"].ToString() == "Alumno") { IDAlumno = Session["iduser"].ToString(); }
            else
            {
                string strQuery_1 = "SELECT DISTINCT IDAlumno FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
                MySqlCommand cmd_1 = new MySqlCommand(strQuery_1);
                cmd_1.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
                DataTable dt = GetData(cmd_1);
                IDAlumno = dt.Rows[0]["IDAlumno"].ToString();
            }
            string strQuery = "SELECT DISTINCT IDNotificacion,IDEstatus,Descripcion,Tipo_Notificacion, Asunto_correo,Cuerpo_correo FROM Configuracion_Notificaciones WHERE Dias=0 AND IDEstatus=@IDEstatus";
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand(strQuery);
            try
            {
                ConexionMySql.Open();

                cmd.Parameters.AddWithValue("@IDEstatus", Estatus);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.Connection = ConexionMySql;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    if (dr.GetString(3) == "Alumno" && dr.GetInt32(0) != 8)
                    {
                        obtener_plantilla_correo(dr.GetInt32(0), Estatus, IDDocumento, IDAlumno, Comentarios);
                    }
                    else if (dr.GetString(3) == "Administrativo")
                    {
                        if (Session["user"].ToString().Length > 10)
                        {

                            obtener_plantilla_correo_admin(dr.GetInt32(0), Estatus, IDDocumento, IDAlumno, "notificaciones");
                        }
                        else
                        {
                            obtener_plantilla_correo_admin(dr.GetInt32(0), Estatus, IDDocumento, IDAlumno, "notificaciones");
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                DirectoryInfo virtualDirPath = new DirectoryInfo(Server.MapPath("~/Logs/"));
                StreamWriter sw = new StreamWriter(virtualDirPath + "Error(envia_correo_estatus)_" + IDAlumno + ".txt", true);

                sw.WriteLine(ex.Message.ToString());
                sw.WriteLine(cmd.CommandText);
                sw.Close();
            }
            finally { ConexionMySql.Close(); }



        }
        protected void obtener_plantilla_correo(int IDNotificacion, string IDEstatus, string IDDocumento, string IDAlumno, string comentarios)
        {

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);

            MySqlCommand cmd = new MySqlCommand("Creacion_correo", ConexionMySql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDNotificacion_in", IDNotificacion);
            cmd.Parameters.AddWithValue("@IDEstatus_in", IDEstatus);
            cmd.Parameters.AddWithValue("@IDDocumento_in", IDDocumento);
            cmd.Parameters.AddWithValue("@IDAlumno_in", IDAlumno);
            cmd.Parameters.Add("@Correo", MySqlDbType.VarChar, 100);
            cmd.Parameters["@Correo"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Subject", MySqlDbType.VarChar, 1000);
            cmd.Parameters["@Subject"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Body", MySqlDbType.VarChar, 8000);
            cmd.Parameters["@Body"].Direction = ParameterDirection.Output;
            try
            {
                ConexionMySql.Open();
                //Executing the SP

                int i = cmd.ExecuteNonQuery();
                //Storing the output parameters value in 3 different variables.
                string Correo = Convert.ToString(cmd.Parameters["@Correo"].Value);
                string Subject = Convert.ToString(cmd.Parameters["@Subject"].Value);
                string Body;
                if (IDEstatus == "5" || IDEstatus == "2")
                {
                    Body = cmd.Parameters["@Body"].Value.ToString();
                    string Body_1 = Body.Replace("<p>&nbsp;_Obervaciones</p>", "<p>&nbsp;Comentarios de la revisión: " + comentarios + "</p>");
                    servicio_correo(Correo, Subject, Body_1, IDAlumno);
                }
                else
                {
                    Body = Convert.ToString(cmd.Parameters["@Body"].Value);
                    if (IDNotificacion != 8) { servicio_correo(Correo, Subject, Body, IDAlumno); }
                }

                //TextBox2.Text = Correo + "--" + Subject + "--" + Body;


            }

            catch (Exception ex)
            {
                DirectoryInfo virtualDirPath = new DirectoryInfo(Server.MapPath("~/Logs/"));
                StreamWriter sw = new StreamWriter(virtualDirPath + "Error(obtener_plantilla_correo)_" + IDAlumno + ".txt", true);

                sw.WriteLine(ex.Message.ToString());
                sw.Close();
            }
            finally
            {
                ConexionMySql.Close();
            }



        }
        protected void obtener_plantilla_correo_admin(int IDNotificacion, string IDEstatus, string IDDocumento, string IDAlumno, string User)
        {

            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);

            MySqlCommand cmd = new MySqlCommand("Creacion_correo_admin", ConexionMySql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDNotificacion_in", IDNotificacion);
            cmd.Parameters.AddWithValue("@IDEstatus_in", IDEstatus);
            cmd.Parameters.AddWithValue("@IDDocumento_in", IDDocumento);
            cmd.Parameters.AddWithValue("@IDAlumno_in", IDAlumno);
            cmd.Parameters.AddWithValue("@user", User);
            cmd.Parameters.Add("@Correo", MySqlDbType.VarChar, 100);
            cmd.Parameters["@Correo"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Subject", MySqlDbType.VarChar, 1000);
            cmd.Parameters["@Subject"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Body", MySqlDbType.VarChar, 8000);
            cmd.Parameters["@Body"].Direction = ParameterDirection.Output;
            try
            {
                ConexionMySql.Open();
                //Executing the SP

                int i = cmd.ExecuteNonQuery();
                //Storing the output parameters value in 3 different variables.
                //Se agrega estas lineas para poder enviar los correos de validacion de certificado. 
                string Correo = null;
                string Subject = null;
                string Body = null;
                if (IDNotificacion == 17)
                {
                    String[] mailTo = ConfigurationManager.AppSettings["DSE_Email"].ToString().Split(',');
                    foreach (String mail in mailTo)
                    {
                        Correo = mail;
                        Subject = Convert.ToString(cmd.Parameters["@Subject"].Value);
                        Body = Convert.ToString(cmd.Parameters["@Body"].Value);
                        servicio_correo(Correo, Subject, Body, IDAlumno);
                    }
                }
                else
                {
                    Correo = Convert.ToString(cmd.Parameters["@Correo"].Value);
                    Subject = Convert.ToString(cmd.Parameters["@Subject"].Value);
                    Body = Convert.ToString(cmd.Parameters["@Body"].Value);
                    servicio_correo(Correo, Subject, Body, IDAlumno);
                }

                //TextBox2.Text = Correo + "--" + Subject + "--" + Body;

            }

            catch (Exception ex)
            {
                DirectoryInfo virtualDirPath = new DirectoryInfo(Server.MapPath("~/Logs/"));
                StreamWriter sw = new StreamWriter(virtualDirPath + "Error(obtener_plantilla_correo_admin)_" + IDAlumno + ".txt", true);

                sw.WriteLine(ex.Message.ToString());
                sw.Close();
            }
            finally
            {
                ConexionMySql.Close();
            }



        }
        protected void servicio_correo(string Correo, string subject, string body, string IDAlumno)
        {
            //MailMessage mailMessage = new MailMessage();
            //var fromAddress = new MailAddress(ConfigurationManager.AppSettings["AdminMail"].ToString(), ConfigurationManager.AppSettings["DisplayName"].ToString());
            //MailMessage message = new MailMessage(fromAddress, Correo,subject,body);
            //message.IsBodyHtml = true;
            //SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["MailHost"].ToString(), int.Parse(ConfigurationManager.AppSettings["Port"].ToString()));
            //smtpClient.UseDefaultCredentials = true;

            MailAddress from = new MailAddress(ConfigurationManager.AppSettings["AdminMail"].ToString(), ConfigurationManager.AppSettings["DisplayName"].ToString());
            MailAddress to = new MailAddress(Correo);
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["MailHost"].ToString(), int.Parse(ConfigurationManager.AppSettings["Port"].ToString()));
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["AdminMail"].ToString(), ConfigurationManager.AppSettings["pwMail"].ToString());
            client.EnableSsl = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                DirectoryInfo virtualDirPath = new DirectoryInfo(Server.MapPath("~/Logs/"));
                StreamWriter sw = new StreamWriter(virtualDirPath + "Error(servicio_correo)_" + IDAlumno + ".txt", true);

                sw.WriteLine(ex.Message.ToString());
                sw.Close();
            }

        }
        protected void actualiza_estatus_expediente(string IDEstatus)
        {
            string IDDocumento = Convert.ToString(Request.QueryString["IDDocumento"]);
            string strQuery = "SELECT DISTINCT IDAlumno FROM Documentos_Alumno WHERE IDDocumento=@IDDocumento";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDDocumento", MySqlDbType.Int32).Value = Convert.ToInt32(IDDocumento);
            DataTable dt = GetData(cmd);
            string IDAlumno = dt.Rows[0]["IDAlumno"].ToString();
            ArrayList arrParametros = new ArrayList();
            arrParametros.Add(new applyWeb.Data.Parametro("@IDEstatusDoc_in", IDEstatus));
            arrParametros.Add(new applyWeb.Data.Parametro("@IDAlumno_in", IDAlumno));
            DataSet dsUsuariosCampus = objAreas.ExecuteSP("Actualiza_Estatus_Expediente", arrParametros);
        }
        protected void log(string IDAlumno, string IDDocumento, string IDEstatus)
        {
            string strQuery = "SELECT DISTINCT Nombre FROM Estatus WHERE IDEstatus=@IDEstatus";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@IDEstatus", MySqlDbType.Int32).Value = Convert.ToInt32(IDEstatus);
            DataTable dt = GetData(cmd);


            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandText = "INSERT INTO Logs (Proceso,Descripcion,Usuario,Fecha) VALUES ('Cambio de estatus',CONCAT('Se cambio el estatus del Documento con ID: ',@IDDocumento,' al estatus: ',@Estatus,' del alumno: ',@IDAlumno),@UserLog,@fecha_mod)";
            cmd1.Parameters.Add("@IDAlumno", MySqlDbType.VarChar).Value = IDAlumno;
            cmd1.Parameters.Add("@IDDocumento", MySqlDbType.VarChar).Value = IDDocumento;
            cmd1.Parameters.Add("@Estatus", MySqlDbType.VarChar).Value = dt.Rows[0]["Nombre"].ToString();
            cmd1.Parameters.Add("@UserLog", MySqlDbType.VarChar).Value = Session["user"].ToString(); ;
            cmd1.Parameters.Add("@fecha_mod", MySqlDbType.DateTime).Value = DateTime.Now;
            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = ConexionMySql;
            ConexionMySql.Open();
            cmd1.ExecuteNonQuery();
            ConexionMySql.Close();

        }
        protected void permisos()
        {
            MySqlConnection ConexionMySql = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString);
            ConexionMySql.Open();
            string strQuery = "SELECT DISTINCT A.IDPrivilegio,b.Permiso FROM Permisos_App_Rol A INNER JOIN Permisos_App B ON A.IDPrivilegio=B.IDPrivilegio INNER JOIN Rol C ON A.IDRol=C.IDRol WHERE B.IDMenu=3 AND B.IDSubMenu=1 AND C.Nombre='" + Session["Rol"].ToString() + "'";
            MySqlCommand cmd = new MySqlCommand(strQuery, ConexionMySql);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int IDprivilegio = dr.GetInt32(0);

                if (IDprivilegio == 20) { UpdatePanel.Visible = true; } //Permiso para subir documentos
                else { UpdatePanel.Visible = false; }

            }
            ConexionMySql.Close();
        }
        //Se agrega este control para obtener el nivel para diferenciar el correo de validación
        protected string obtener_nivel(string idalumno)
        {
            string strQuery = "select IDNivel from Alumno where IDAlumno=@idAlumno";
            MySqlCommand cmd = new MySqlCommand(strQuery);
            cmd.Parameters.Add("@idAlumno", MySqlDbType.VarChar).Value = idalumno;
            DataTable dt = GetData(cmd);
            string nivel = dt.Rows[0]["IDNivel"].ToString();
            return nivel;
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

    }
}