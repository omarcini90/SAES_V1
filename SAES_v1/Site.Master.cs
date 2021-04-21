using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAES_v1
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["rol"].ToString() == "Alumno")
            {
                ///Menus///
                operacion.Visible = false;
                prospectos.Visible = false;
                admision.Visible = false;
                escolares.Visible = false;
                planeacion.Visible = false;
                Finanzas.Visible = false;
                Seguridad.Visible = false;
                ///SubMenus///
                tdocumentos.Visible = false;
                permisos_repo.Visible = false;
                expedientes.Visible = false;
            }
            else
            {
                ///SubMenu///
                carga_alumno.Visible = false;
            }
        }
    }
}