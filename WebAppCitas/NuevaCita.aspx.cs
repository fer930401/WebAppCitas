using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppCitas;
using Entidades;
using LogicaNegocio;
using System.Globalization;
using System.Data.SqlClient;
using System.Net.Mail;

namespace WebAppCitas
{
    public partial class NuevaCita : System.Web.UI.Page
    {
        LogicaNegocioCls logicaNegocio;
        string ef_cve = "001";
        int idCita = 0;
        int opc = 0;
        string idSalaIni;
        //string CadenaConecta = @"Data Source=192.168.18.96;Initial Catalog=skytex;User ID=soludin_develop;Password=dinamico20";
        string CadenaConecta = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20";
        //string CadenaConecta = @"Data Source=SQL;Initial Catalog=skytex;User ID=soludin_develop;Password=dinamico20";
                    
        protected void Page_Load(object sender, EventArgs e)
        {
            logicaNegocio = new LogicaNegocioCls();
            idCita = Int32.Parse(Request["id"].ToString());
            opc = Int32.Parse(Request["opc"].ToString());
            if (Session["NombreUser"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
            /*else if (Session["RolCve"] != null && Session["RolCve"].ToString().Equals("VIG") == true || Session["RolCve"].ToString().Equals("GER") == true)
            {
                Response.Redirect("AgendaCitas.aspx", false);
            }*/
            else
            {
                //Response.Write(opc);
                if (opc == 1)
                {
                    Session["visible"] = "style = 'display:none'";
                }
                else
                {
                    Session["visible"] = "";
                }
                if (!IsPostBack)
                {
                    idCita = Int32.Parse(Request["id"].ToString());
                    //Response.Write(Session["RolCve"]);
                    llenaCboSala();

                    string fechaCita;
                    SqlConnection _conn = new SqlConnection(CadenaConecta);
                    SqlCommand _cmd = new SqlCommand();
                    _cmd.Connection = _conn;
                    _cmd.CommandType = CommandType.Text;
                    _cmd.CommandText = String.Format("select CONVERT(VARCHAR(10), fec_ultact, 21) as fec_Cita from itmov where tipo_doc = 'ltasis' and ef_cve = '001' and num_fol = '{0}'", idCita);
                    _conn.Open();
                    fechaCita = Convert.ToString(_cmd.ExecuteScalar());
                    _cmd.ExecuteNonQuery();
                    _conn.Close();
                    txtFecCita.Text = fechaCita;
                    llenaCboHoraE();
                    llenaCboHoraS();
                    /*ddHoraE.Enabled = false;*/
                    ddHoraS.Enabled = false;
                    /*txtFecCita.Enabled = false;*/
                    ButtonOK.Enabled = false;
                }
            }
        }

        protected void ButtonOK_Click(object sender, EventArgs e)
        {
            if (Session["RolCve"].ToString().Equals("ATD") == true || Session["RolCve"].ToString().Equals("DIR") == true || Session["RolCve"].ToString().Equals("ADT") == true)
            {
                string tipdoc_cve = "LTASIS",
                refer = ddSala.Text.Trim(' '),
                art_tip = "SER",
                sku_cve = "2801",
                id_ultact = Session["UserCve"].ToString();

                DateTime fechaE = Convert.ToDateTime(txtFecCita.Text),
                        horaE = Convert.ToDateTime(ddHoraE.Text),
                        horaS = Convert.ToDateTime(ddHoraS.Text);

                DateTime NFechaIni = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day, horaE.Hour, horaE.Minute, horaE.Second),
                            NFechaFin = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day, horaS.Hour, horaS.Minute, horaS.Second);
                DateTime fec_ultact = DateTime.Now;

                int? error = 0;
                string mensaje;
                //Modal.Close(this, "OK");
                try
                {
                    Entidades.sp_AutValCita_Result Reagendo = logicaNegocio.autValCita(ef_cve.Trim(), tipdoc_cve.Trim(), idCita, 6, 1, 1, id_ultact, "Reagendado", NFechaIni, NFechaFin, refer);
                    if (Reagendo != null)
                    {
                        error = Reagendo.error;
                        mensaje = Reagendo.mensaje;
                        if (error == 0)
                        {
                            if (opc == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Se reagendo la cita correctamente');", true);
                                Modal.Close(this, "OK");
                            }
                            else if (opc == 2)
                            {
                                Session["fechaAgendada"] = NFechaIni;
                                Session["command"] = "movido";
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Se reagendo la cita correctamente'); window.location.href = 'AgendaCitas.aspx';", true);
                            }
                            else
                            {
                                Response.Write("Error");
                            }

                        }
                        else
                        {
                            if (opc == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al reagendar: " + mensaje + "');", true);
                                Modal.Close(this, "error");
                            }
                            else if (opc == 2)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al reagendar: " + mensaje + "'); window.location.href = 'AgendaCitas.aspx';", true);
                            }
                            else
                            {
                                Response.Write("Error");
                            }

                        }
                    }
                    else
                    {
                        Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                    }
                }
                catch (Exception errores)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
                if (error == 0)
                {
                    try
                    {
                        if (refer.Equals("1") == true)
                        {
                            refer = "A";
                        }
                        else if (refer.Equals("2") == true)
                        {
                            refer = "B";
                        }
                        else if (refer.Equals("3") == true)
                        {
                            refer = "C";
                        }
                        else if (refer.Equals("4") == true)
                        {
                            refer = "Otra";
                        }
                        //sendEmail(idCita, NFechaIni, NFechaFin, refer, id_ultact);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('Error al enviar email, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                    }
                }
            }
            else
            {
                Response.Write("<script type=\"text/javascript\">alert('No tienes permisos para cambiar las fechas de las citas');</script>");
                Modal.Close(this, "error");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            //Modal.Close(this);
            if (opc == 1)
            {
                Modal.Close(this);
            }
            else if (opc == 2)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "window.location.href = 'AgendaCitas.aspx';", true);
            }
            else
            {
                Response.Write("Error");
            }
        }
        protected void llenaCboSala()
        {
            string idSala;
            SqlConnection _conn = new SqlConnection(CadenaConecta);
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = String.Format("select refer from itmov where tipo_doc = 'ltasis' and ef_cve = '001' and num_fol = '{0}'", idCita);
            _conn.Open();
            idSala = Convert.ToString(_cmd.ExecuteScalar());
            idSalaIni = Convert.ToString(_cmd.ExecuteScalar());
            _cmd.ExecuteNonQuery();
            _conn.Close();

            List<Entidades.qcomsala1_Result> sala = logicaNegocio.obtCboSala("ltasis", "PAS");
            ddSala.DataSource = sala;
            ddSala.DataTextField = "spd_cve";
            ddSala.DataValueField = "prm1";
            ddSala.DataBind();
            ddSala.SelectedValue = idSala.Trim(' ');
            ddSala.Items.Insert(0, new ListItem("------Selecciona Sala------", "NA"));
        }
        protected void llenaCboHoraE()
        {
            DateTime fechaE = Convert.ToDateTime(txtFecCita.Text);
            DateTime fec_ultact = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);
            string sala = ddSala.Text,
                    refer = ddSala.Text;
            short plazo = 4;

            if (sala == "NA")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Selecciona la sala');", true);
                ddSala.Focus();
            }
            else
            {
                string horaEntrada;
                //string CadenaConecta = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20"; ;
                SqlConnection _conn = new SqlConnection(CadenaConecta);
                SqlCommand _cmd = new SqlCommand();
                _cmd.Connection = _conn;
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = String.Format("select CONVERT(CHAR(5), fec_ultact, 8 ) as Hor_IniCit from itmov where tipo_doc = 'ltasis' and ef_cve = '001' and num_fol = '{0}'", idCita);
                _conn.Open();
                horaEntrada = Convert.ToString(_cmd.ExecuteScalar());
                _cmd.ExecuteNonQuery();
                _conn.Close();
                
                List<Entidades.qcomWebHoraReagenda_Result> horaE = logicaNegocio.obtCboHoraReagenda("ltasis", fec_ultact.ToString("yyyy-MM-dd"), refer, "00:00", ef_cve, plazo, fec_ultact.ToString("yyyy-MM-dd"), idCita);
                ddHoraE.DataSource = horaE;
                ddHoraE.DataTextField = "Hora";
                ddHoraE.DataValueField = "Clave";
                ddHoraE.DataBind();

                ddHoraE.Items.Insert(0, new ListItem("---Selecciona Hora Entrada---", "NA"));
                //ddHoraE.Items.Insert(0, new ListItem(horaEntrada, horaEntrada));
                //ddHoraE.SelectedValue = horaEntrada;
            }
        }
        protected void llenaCboHoraS()
        {
            DateTime fechaE = Convert.ToDateTime(txtFecCita.Text);
            DateTime fec_ultact = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);
            string sala = ddSala.Text,
                    refer = ddSala.Text,
                    horaE = ddHoraE.Text;
            short plazo = 2;


            if (sala == "NA")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Selecciona la sala');", true);
                ddSala.Focus();
            }
            else
            {
                string horaSalida;
                //string CadenaConecta = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20"; ;
                SqlConnection _conn = new SqlConnection(CadenaConecta);
                SqlCommand _cmd = new SqlCommand();
                _cmd.Connection = _conn;
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = String.Format("select CONVERT(CHAR(5), fec_prom, 8 ) as Hor_FinCit from itmov where tipo_doc = 'ltasis' and ef_cve = '001' and num_fol = '{0}'", idCita);
                _conn.Open();
                horaSalida = Convert.ToString(_cmd.ExecuteScalar());
                _cmd.ExecuteNonQuery();
                _conn.Close();

                List<Entidades.qcomWebHoraReagenda_Result> horaS = logicaNegocio.obtCboHoraReagenda("ltasis", fec_ultact.ToString("yyyy-MM-dd"), refer, horaE, ef_cve, plazo, fec_ultact.ToString("yyyy-MM-dd"), idCita);
                ddHoraS.DataSource = horaS;
                ddHoraS.DataTextField = "Hora";
                ddHoraS.DataValueField = "Clave";
                ddHoraS.DataBind();
                //ddHoraS.SelectedValue = horaSalida;
                ddHoraS.Items.Insert(0, new ListItem("----Selecciona Hora Salida----", "NA"));
            }
        }
        protected void txtFecCita_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime fecha = Convert.ToDateTime(txtFecCita.Text),
                        fechaE = DateTime.Now;
            DateTime fechaAct = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);

            if (fecha >= fechaAct)
            {
                llenaCboHoraE();
                ddHoraE.Enabled = true;
                ddHoraE.Focus();
            }
            else
            {
                ddHoraE.Enabled = false;
                
                string script = "alert('La fecha seleccionada no es valida');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                txtFecCita.Text = "yyyy-mm-dd";
                //txtFecCita.Focus();
            }
        }

        protected void ddHoraE_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaCboHoraS();
            ddHoraS.Focus();
            ddHoraS.Enabled = true;
        }

        protected void ddHoraS_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonOK.Enabled = true;
        }

        protected void ddSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFecCita.Enabled = true;
            txtFecCita.Text = "";
            //txtFecCita.Focus();
            if (ddHoraS.Enabled == true)
            {
                ddHoraS.SelectedIndex = 0;
                ddHoraS.Enabled = false;
            }
            ddHoraE.SelectedIndex = 0;
        }
        public void sendEmail(int? numCita, DateTime fechaIni, DateTime fechaFin, string sala, string id_ultact)
        {
            string emailRespon = emailUser(id_ultact);
            //var to = emailRespon;
            var to = "fernando.garcia@skytex.com.mx";
            var cc = "fernando.garcia@skytex.com.mx";
            var bcc = "fernando.garcia@skytex.com.mx";
            var emailRemitente = "soludin@skytex.com.mx";

            var eMailSubject = "Reasignacion de Cita";
            var eMailMessage =
                "<html lang='en'>" +
                "<head>" +
                    "<style>" +
                        "html { font-family: sans-serif; font-size: 11px -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;}" +
                        "body { font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.428571429; color: #333333; background-color: #ffffff; } " +
                    "</style>" +
                "</head>" +
                    "<body>" +
                    "<h4> Reasignacion de Citas - Skytex México</h4>" +
                    "<table cellpadding='0' cellspacing='0' width='700'>" +
                     "<tr>" +
                      "<td>" +
                        "<img src='http://i64.tinypic.com/2cwph5l.jpg' width='190' height='90' />" +
                      "</td>" +
                     "</tr>" +
                     "<tr>" +
                      "<td style='padding: 40px 30px 40px 30px;'>" +
                       "<table cellpadding='0' cellspacing='0' width='100%'>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "<strong>Se ha reasigando la siguiente Cita</strong>" +
                           "</td>" +
                          "</tr>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "" +
                           "</td>" +
                          "</tr>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "<strong>Num cita:</strong> " + numCita +
                           "</td>" +
                          "</tr>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "<strong>Nueva Fecha Inicio:</strong> " + fechaIni +
                           "</td>" +
                          "</tr>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "<strong>Nueva Fecha Fin:</strong> " + fechaFin +
                           "</td>" +
                          "</tr>" +
                          "<tr>" +
                           "<td width='60%'>" +
                            "<strong>Nueva Sala:</strong> " + sala +
                           "</td>" +
                          "</tr>" +
                         "</table>" +
                      "</td>" +
                     "</tr>" +
                     "<tr>" +
                      "<td bgcolor='#222222'>" +
                       "<p align='center'><font color= '#ffffff'></font></p>" +
                      "</td>" +
                     "</tr>" +
                    "</table>" +
                    "</body>" +
                "</html>";

            MailMessage mail = new MailMessage();
            mail.To.Add(new System.Net.Mail.MailAddress(to));
            //mail.To.Add(new System.Net.Mail.MailAddress("fergarciavera91@gmail.com", "Fernando 2"));
            mail.From = new System.Net.Mail.MailAddress(emailRemitente, "Agenda Salas de Citas", System.Text.Encoding.UTF8);
            mail.CC.Add(new System.Net.Mail.MailAddress(cc));
            mail.Bcc.Add(new System.Net.Mail.MailAddress(bcc));
            mail.Subject = eMailSubject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = eMailMessage;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            // Agregar el Adjunto si deseamos hacerlo
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(@"c:\Users\fernando.garcia\Documents\Proyectos Skytex\AplicacionWeb\Prueba.Presentacion\Activo\Reporte Ordenes.xls");
            //mail.Attachments.Add(attachment);

            // Configuración SMTP
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("outlook.skytex.com.mx", 25);

            // Crear Credencial de Autenticacion
            smtp.Credentials = new System.Net.NetworkCredential("soludin", "pluma");
            smtp.EnableSsl = false;

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string emailUser(string userCve)
        {
            string emailUsr;
            //string CadenaConecta = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20"; ;
            SqlConnection _conn = new SqlConnection(CadenaConecta);
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = String.Format("select dest_mail from xcuser where user_cve = '{0}'", userCve);
            _conn.Open();
            emailUsr = Convert.ToString(_cmd.ExecuteScalar());
            _cmd.ExecuteNonQuery();
            _conn.Close();
            return emailUsr;
        }
        protected void CerrarSession(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}