using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using LogicaNegocio;

namespace WebAppCitas
{
    public partial class Login : System.Web.UI.Page
    {

        LogicaNegocioCls logicaNegocio;

        protected void Page_Load(object sender, EventArgs e)
        {
            logicaNegocio = new LogicaNegocioCls();
            if (Session["NombreUser"] != null)
            {
                Response.Redirect("AgendaCitas.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    llenaCbo();
                }
                Label1.Visible = false;
            }
        }

        protected void llenaCbo()
        {
            List<Entidades.sp_webAppSalaCbo_Result> usuario = logicaNegocio.obtCboSala("001", "appSal", "cboUser", "", "", "");
            ddUsuario.DataSource = usuario;
            ddUsuario.DataTextField = "resultado";
            ddUsuario.DataValueField = "clave";
            ddUsuario.DataBind();
        }


        protected void btEntrar_Click(object sender, EventArgs e)
        {
            
            string Valor = ddUsuario.SelectedValue;
            string Password =  Request["pass"];
            string UserCve = Valor.Remove(3, 4).TrimEnd(' ').TrimStart(' ');
            string RolCve = Valor.Remove(0, 4).TrimEnd(' ').TrimStart(' ');
            /* mod 25/01/2017 - se elimino el uso de la variable tiporol */
            string TipoRol;
            string nomUser = ddUsuario.SelectedItem.Text.ToString().TrimEnd(' ').TrimStart(' ');
            

            if (Valor.Equals("xxx@xxx"))
            {
                string script = @"<script type='text/javascript'> Confirmar(" + 1 + ");</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, false);
            }
            else
            {           
                
                var result = logicaNegocio.accesoSistema("001", UserCve,Password);
                /* mod 24/01/2017 - se cambio la validacion del resultado del metodo accesoSistema */
                if (result != null)
                {
                    Session["NombreUser"] = ddUsuario.SelectedItem.ToString().TrimEnd(' ').TrimStart(' ');
                    Session["UserCve"] = UserCve.TrimEnd(' ').TrimStart(' ');
                    Session["Password"] = Password;
                    Session["RolCve"] = RolCve.TrimEnd(' ').TrimStart(' ');
                    
                    /* mod 25/01/2017 - se elimino el uso de la variable tiporol, ya que se toma  el rol configurado en el prm3 del xcdconapl_cl */
                    //TipoRol = logicaNegocio.obtInfRoles("AppWeb", "AppAge", "Roles", "001", RolCve);
                    /*obtInfRoles(string tipdoc_cve, string sp_cve, string spd_cve, string prm1, string prm2 )*/

                    /*if (RolCve.Equals("VIG") == true)
                    {
                        Response.Redirect("Vigilancia.aspx");
                    }
                    else
                    {*/
                        Response.Redirect("AgendaCitas.aspx");
                    /*}*/
                        
                }
                else
                {

                    /* mod 24/01/2017 - se cambio el mensaje de error al iniciar sesion */
                    Label1.Visible = true;
                    result = " El Usuario/Contraseña no se encontraron";
                    Label1.Text = "<div class='alert alert-danger' role='alert'><span class='glyphicon glyphicon-exclamation-sign' aria-hidden='true'></span> " + nomUser + ", La contraseña no coincide, intente de nuevo</div>";
                    /*string script = @"<script type='text/javascript'> Confirmar("+2+");</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, false);*/
                }
            }
            
        }

    }
}