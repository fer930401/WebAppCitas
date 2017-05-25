using DayPilot.Json;
using DayPilot.Utils;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Bubble;
using DayPilot.Web.Ui.Events.Scheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using LogicaNegocio;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Web.Configuration;


namespace WebAppCitas
{
    public partial class AgendaCitas : System.Web.UI.Page
    {
        LogicaNegocioCls logicaNegocio = new LogicaNegocioCls();
        DataTable table;
        string mensaje = "";
        int? error = 0;
        string userCve = "";
        //string CadenaConecta = @"Data Source=192.168.18.96;Initial Catalog=skytex;User ID=soludin_develop;Password=dinamico20";
        string CadenaConecta = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20";
        //string CadenaConecta = @"Data Source=SQL;Initial Catalog=skytex;User ID=soludin_develop;Password=dinamico20";

        protected void Page_Load(object sender, EventArgs e)
        {
            /*Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!this.IsPostBack)
            {
                Session["Reset"] = true;
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
                SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
                int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
                ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);
            }*/
            /* add 24/01/2017 - se agrego la validacion de la session existente, se no existir una session se redirecciona al login */
            if (Session["NombreUser"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {

                /* la columna validar no se implementara, por esa razon no aparece la columna en la tabla */
                //GridView1.Columns[7].Visible = false;
                userCve = Session["UserCve"].ToString().TrimStart(' ').TrimEnd(' ');
                /* mod 25/01/2017 - se cambio el codigo del metodo llenaGrid al metodo initData(), llenarGrid no se ocupa */
                initData();
                /* mod 25/01/2017 - se cambio el nombre del metodo LoadData por llenaGrid, este es el metodo que llena el grid con la informacion de las citas para el dia de hoy */
                LlenaGrid();
                Label1.Text = Session["NombreUser"].ToString();

                if (Session["RolCve"].ToString().Equals("GER") == true)
                {
                    /* agenda citas,cancela citas, no mueve citas */
                    /* Autorizar */
                    GridView1.Columns[7].Visible = false;
                    /* Reagendar */
                    GridView1.Columns[8].Visible = false;
                    /* Cancelar */
                    //GridView1.Columns[9].Visible = false;
                    /* acceso planta */
                    GridView1.Columns[10].Visible = false;
                    Session["visible"] = "";
                }
                else if (Session["RolCve"].ToString().Equals("VGC") == true)
                {
                    /* solo ve el grid con las citas de ese dia, y el boton "acceso a planta" */
                    DayPilotScheduler1.Visible = false;
                    DayPilotNavigator1.Visible = false;
                    btReinicar.Visible = false;
                    /* Autorizar */
                    GridView1.Columns[7].Visible = false;
                    /* Reagendar */
                    GridView1.Columns[8].Visible = false;
                    /* Cancelar */
                    GridView1.Columns[9].Visible = false;
                    /* acceso planta */
                    //GridView1.Columns[10].Visible = false;
                    Session["visible"] = "style = 'display:none'";
                }
                else if (Session["RolCve"].ToString().Equals("ATD") == true)
                {
                    /* valida, autoriza, mueve fechas */
                    /* Autorizar */
                    //GridView1.Columns[7].Visible = false;
                    /* Reagendar */
                    // GridView1.Columns[8].Visible = false;
                    /* Cancelar */
                    GridView1.Columns[9].Visible = false;
                    /* acceso planta */
                    GridView1.Columns[10].Visible = false;
                    Session["visible"] = "";
                }
                else if (Session["RolCve"].ToString().Equals("DIR") == true)
                {
                    /* agenda citas, mueve citas, cancela citas, mueve fechas */
                    /* acceso planta */
                    GridView1.Columns[10].Visible = false;
                    Session["visible"] = "";
                }
                else if (Session["RolCve"].ToString().Equals("ADT") == true)
                {
                    /* agenda citas, mueve citas, cancela citas, mueve fechas y el boton "acceso a planta" */
                    Session["visible"] = "";
                }
                else
                {
                    DayPilotScheduler1.Visible = false;
                    btReinicar.Visible = false;
                    GridView1.Visible = false;
                    Session["visible"] = "style = 'display:none'";
                }
                
                if (!IsPostBack)
                {
                    /*if (Session["fecFiltro"] != null)
                    {
                        fechaFiltro.Text = Session["fecFiltro"].ToString();
                    }*/
                    initData();
                    /* se modifico para que apareciera el nombre del usuario logueado */
                    //Session["NombreUser"] = "gio";
                    Label1.Text = Session["NombreUser"].ToString();

                    //setDataSourceAndBind();
                    DayPilotScheduler1.UpdateWithMessage("Bienvenido a la Administracion de acceso a planta y salas");
                    //LoadData();
                    //GridViewRow row = GridView1.SelectedRow;
                    //if (Session["NombreUser"].ToString().Equals("Vigilancia"))
                    //{
                    //    if (GridView1.Columns[7].ToString().Equals("Si"))
                    //    {
                    //        row.Cells[7].Text = "Autorizada";
                    //    }
                    //    GridView1.Columns[8].Visible = false;
                    //    GridView1.Columns[9].Visible = true;
                    //}
                    //else if (Session["NombreUser"].ToString().Equals("Admin"))
                    //{
                    //    if (GridView1.Columns[7].ToString().Equals("Si"))
                    //    {
                    //        row.Cells[7].Text = "Autorizada";
                    //    }
                    //    GridView1.Columns[8].Visible = true;
                    //    GridView1.Columns[9].Visible = false;
                    //}
                    //else
                    //{
                    //    GridView1.Columns[8].Visible = false;
                    //    GridView1.Columns[9].Visible = false;
                    //}
                    if (Session["RolCve"].ToString().Equals("GER") == true)
                    {
                        /* agenda citas,cancela citas, no mueve citas */
                        /* Autorizar */
                        GridView1.Columns[7].Visible = false;
                        /* Reagendar */
                        GridView1.Columns[8].Visible = false;
                        /* Cancelar */
                        //GridView1.Columns[9].Visible = false;
                        /* acceso planta */
                        GridView1.Columns[10].Visible = false;
                        Session["visible"] = "";
                    }
                    else if (Session["RolCve"].ToString().Equals("VGC") == true)
                    {
                        /* solo ve el grid con las citas de ese dia, y el boton "acceso a planta" */
                        DayPilotScheduler1.Visible = false;
                        DayPilotNavigator1.Visible = false;
                        btReinicar.Visible = false;
                        /* Autorizar */
                        GridView1.Columns[7].Visible = false;
                        /* Reagendar */
                        GridView1.Columns[8].Visible = false;
                        /* Cancelar */
                        GridView1.Columns[9].Visible = false;
                        /* acceso planta */
                        //GridView1.Columns[10].Visible = false;
                        Session["visible"] = "style = 'display:none'";
                    }
                    else if (Session["RolCve"].ToString().Equals("ATD") == true)
                    {
                        /* valida, autoriza, mueve fechas */
                        /* Autorizar */
                        //GridView1.Columns[7].Visible = false;
                        /* Reagendar */
                        // GridView1.Columns[8].Visible = false;
                        /* Cancelar */
                        GridView1.Columns[9].Visible = false;
                        /* acceso planta */
                        GridView1.Columns[10].Visible = false;
                        Session["visible"] = "";
                    }
                    else if (Session["RolCve"].ToString().Equals("DIR") == true)
                    {
                        /* agenda citas, mueve citas, cancela citas, mueve fechas */
                        /* acceso planta */
                        GridView1.Columns[10].Visible = false;
                        Session["visible"] = "";
                    }
                    else if (Session["RolCve"].ToString().Equals("ADT") == true)
                    {
                        /* agenda citas, mueve citas, cancela citas, mueve fechas y el boton "acceso a planta" */
                        Session["visible"] = "";
                    }
                    else
                    {
                        DayPilotScheduler1.Visible = false;
                        btReinicar.Visible = false;
                        GridView1.Visible = false;
                        Session["visible"] = "style = 'display:none'";
                    }
                }
            }
        }

        private void initData()
        {
            /*
            if (Session[PageHash] == null)
            {
                Session[PageHash] = DataGeneratorScheduler.GetData();
            }
            table = (DataTable)Session[PageHash];
            DayPilotScheduler1.DataSource = Session["AllFeatures"];
            DayPilotScheduler1.DataSource = Session["AllFeatures"];
            DayPilotScheduler1.DataSource = DataGeneratorScheduler.GetData();
            DayPilotScheduler1.DataBind();
            table = (DataTable)DataGeneratorScheduler.GetData();
            LoadResources();
             */
            /* mod 25/01/2017 - de comentarios las variables day,mes, year porque no se cocupaban */
            /*int day = 0;
            int mes = 0;
            int year = 0;*/
            DateTime fecha;

            Entidades.xcdconapl_cl Informacion = logicaNegocio.obtInfPantalla("001", "AppAge", "AppWeb", "horas");
            if (Informacion != null)
            {
                fecha = DateTime.Today.AddDays(Convert.ToInt32(Informacion.prm4));
                /*day = fecha.Day;
                mes = fecha.Month;
                year = fecha.Year;*/

                /* mod 25/01/2017 - se modifico el formato de la fecha que recibe como parametro de startdate el grid*/
                //DayPilotScheduler1.StartDate = new DateTime(year, mes, day);
                /*DayPilotScheduler1.StartDate = fecha;
                DayPilotScheduler1.Days = Convert.ToInt32(Informacion.prm3);//4//Year.Days(DateTime.Today.Year);*/
                if (Session["fechaAgendada"] != null)
                {
                    if (Session["command"].ToString().Equals("navigate") == false)
                    {
                        DayPilotScheduler1.StartDate = (DateTime)Session["fechaAgendada"];
                    }
                }
                else
                {
                    //DayPilotCalendar1.StartDate = System.DateTime.Now;
                }
                DayPilotScheduler1.Separators.Add(DateTime.Now, Color.Red);
                DayPilotScheduler1.SetScrollX(DateTime.Now);

                /* mod 25/01/2017 - se agrego el codigo que obtendra de manera dinamica el nombre y el id de las salas disponibles para realizar alguna cita */
                LoadResources();
                //Llena el grid de daypilot
                setDataSourceAndBind();
            }
        }
        public void LoadResources()
        {
            /* mod 25/01/2017 - se agrego el codigo que obtendra de manera dinamica el nombre y el id de las salas disponibles para realizar alguna cita */
            DayPilotScheduler1.Resources.Clear();

            SqlDataAdapter da = new SqlDataAdapter("SELECT rtrim(spd_cve) as spd_cve, concat('Sala ',rtrim(spd_cve)) as nomSala FROM xcdconapl_cl where tipdoc_cve = 'ltasis' and prm2 = 'sala' and sp_cve = 'PAD' and spd_cve != 'Otra' group by prm1,spd_cve", ConfigurationManager.ConnectionStrings["skytexConnectionString"].ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow r in dt.Rows)
            {
                string name = (string)r["nomSala"];
                string id = Convert.ToString(r["spd_cve"]);

                DayPilotScheduler1.Resources.Add(name, id);
            }

            /*
            List<Entidades.sp_ConsCitasCalendario_Result> conC = logicaNegocio.consCitasCal();
            DayPilotScheduler1.DataSource = conC;
            DayPilotScheduler1.DataStartField = "fec_ultact";
            DayPilotScheduler1.DataEndField = "fec_prom";
            DayPilotScheduler1.DataTextField = "imp_letra";
            DayPilotScheduler1.DataIdField = "num_fol";
            DayPilotScheduler1.DataResourceField = "refer";
            DayPilotScheduler1.DataBind();
            */
        }
        private void setDataSourceAndBind()
        {
            // ensure that filter is loaded
            //string filter = (string)DayPilotScheduler1.ClientState["filter"];
            // llena el grid con la informacion regresada por el metodo GetData()
            DayPilotScheduler1.DataSource = DataGeneratorScheduler.GetData();
            DayPilotNavigator1.DataSource = DataGeneratorScheduler.GetData();
            table = (DataTable)DataGeneratorScheduler.GetData();
            DayPilotScheduler1.DataBind();
        }
        private void LlenaGrid()
        {
            /*DataTable dt = new DataTable();
            DateTime fechaActual = DateTime.Now;
            //DateTime fechaActual = Convert.ToDateTime("2017-02-02");
            DateTime hoy = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day);
            List<Entidades.sp_consCitas_Result> datos = logicaNegocio.consCitas("ltasis", hoy);
            CitasDiarias.SelectCommand = "exec sp_consCitas 'ltasis','2017-01-24'";
            */
            if (Session["RolCve"] == null || Session["UserCve"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                if (Session["fecFiltro"] == null)
                {
                    Session["fecFiltro"] = DateTime.Now.ToString("yyyy-MM-dd");
                }
                //SqlDataSource SqlDataSource1 = new SqlDataSource();
                //SqlDataSource1.ID = "CitasDiarias";
                //this.Page.Controls.Add(SqlDataSource1);
                //SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
                //SqlDataSource1.SelectCommand = "exec sp_consCitas 'ltasis','" + Session["RolCve"].ToString().Trim(' ') + "','" + Session["UserCve"].ToString().Trim(' ') + "','" + Session["fecFiltro"].ToString() + "'";
                //GridView1.DataSource = SqlDataSource1;
                //GridView1.DataBind();
                /*GridView1.DataSource = datos;
                GridView1.DataBind();*/

                /*cambiar boton al tener autorizada la cita*/

                //DataTable dt = new DataTable();

                //dt.Columns.Add("numCita");
                //dt.Columns.Add("Nombre");
                //dt.Columns.Add("Empresa");
                //dt.Columns.Add("fec_visita");
                //dt.Columns.Add("sala");
                //dt.Columns.Add("razon_visita");
                //dt.Columns.Add("detalles");
                //dt.Columns.Add("autorizada");

                //DataRow dr;

                //dr = dt.NewRow();
                //dr["numCita"] = "1";
                //dr["Nombre"] = "Persona X";
                //dr["Empresa"] = "Empresa X";
                //dr["fec_visita"] = System.DateTime.Now;
                //dr["sala"] = "Sala A";
                //dr["razon_visita"] = "Razones X";
                //dr["detalles"] = "Se van a tratar asuntos X";
                //dr["autorizada"] = "Si";
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr["numCita"] = "2";
                //dr["Nombre"] = "Persona Y";
                //dr["Empresa"] = "Empresa Y";
                //dr["fec_visita"] = System.DateTime.Now;
                //dr["sala"] = "Sala B";
                //dr["razon_visita"] = "Razones Y";
                //dr["detalles"] = "Se van a tratar asuntos Y";
                //dr["autorizada"] = "No";
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr["numCita"] = "3";
                //dr["Nombre"] = "Persona Z";
                //dr["Empresa"] = "Empresa Z";
                //dr["fec_visita"] = System.DateTime.Now;
                //dr["sala"] = "Sala C";
                //dr["razon_visita"] = "Razones Z";
                //dr["detalles"] = "Se van a tratar asuntos Z";
                //dr["autorizada"] = "No";
                //dt.Rows.Add(dr);

                //this.GridView1.DataSource = dt;
                //this.GridView1.DataBind();
            }
        }
        public void llenarGrid()
        {
            int day = 0;
            int mes = 0;
            int year = 0;
            DateTime fecha;
            Entidades.xcdconapl_cl Informacion = logicaNegocio.obtInfPantalla("001", "AppAge", "AppWeb", "horas");

            if (Informacion != null)
            {
                fecha = DateTime.Today.AddDays(Convert.ToInt32(Informacion.prm4));
                day = fecha.Day;
                mes = fecha.Month;
                year = fecha.Year;

                DayPilotScheduler1.StartDate = new DateTime(year, mes, day);
                DayPilotScheduler1.Days = Convert.ToInt32(Informacion.prm3);//4//Year.Days(DateTime.Today.Year);
                DayPilotScheduler1.Separators.Add(DateTime.Now, Color.Red);
                DayPilotScheduler1.SetScrollX(DateTime.Now);

                setDataSourceAndBind(); //Llena el dasource del grid
                LoadResources();
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (row.RowIndex > -1)
                for (int i = 0; i < row.Cells.Count; i += 1)
                    row.Cells[i].Visible = false;
        }

        protected void DayPilotScheduler1_EventMove(object sender, DayPilot.Web.Ui.Events.EventMoveEventArgs e)
        {
            if (Session["RolCve"].ToString().Equals("ATD") == true || Session["RolCve"].ToString().Equals("DIR") == true || Session["RolCve"].ToString().Equals("ADT") == true)
            {
                /*
                #region Simulation of database update

                DataRow dr = table.Rows.Find(e.Id);
                if (dr != null)
                {
                    dr["fec_ultact"] = e.NewStart;
                    dr["fec_prom"] = e.NewEnd;
                    dr["refer"] = e.NewResource;
                    table.AcceptChanges();
                }
                else // moved from outside
                {
                    dr = table.NewRow();
                    dr["fec_ultact"] = e.NewStart;
                    dr["fec_prom"] = e.NewEnd;
                    dr["num_fol"] = e.Id;
                    dr["imp_letra"] = e.Text;
                    dr["refer"] = e.NewResource;

                    table.Rows.Add(dr);
                    table.AcceptChanges();
                }

                #endregion
                */
                //setDataSourceAndBind();
                if (e.OldStart.ToString().Equals(e.NewStart.ToString()) == false || e.OldResource.Equals(e.NewResource) == false)
                {
                    DataRow dr = table.Rows.Find(e.Id);
                    if (dr != null)
                    {
                        string ef_cve = "001",
                        tipdoc_cve = "LTASIS",
                        refer = "",
                        art_tip = "SER",
                        sku_cve = "2801",
                        id_ultact = Session["UserCve"].ToString();
                        DateTime NFechaIni = e.NewStart;
                        DateTime NFechaFin = e.NewEnd;
                        DateTime fec_ultact = DateTime.Now;

                        string horaIni;
                        SqlConnection _conn = new SqlConnection(CadenaConecta);
                        SqlCommand _cmd = new SqlCommand();
                        _cmd.Connection = _conn;
                        _cmd.CommandType = CommandType.Text;
                        _cmd.CommandText = String.Format("select min(spd_cve) from xcdconapl_cl where tipdoc_cve = 'ltasis' and num_reng = 0");
                        _conn.Open();
                        horaIni = Convert.ToString(_cmd.ExecuteScalar());
                        _cmd.ExecuteNonQuery();
                        _conn.Close();
                        string[] horaIniVal = horaIni.Split(':');

                        string horaFin;
                        SqlConnection _conn2 = new SqlConnection(CadenaConecta);
                        SqlCommand _cmd2 = new SqlCommand();
                        _cmd2.Connection = _conn2;
                        _cmd2.CommandType = CommandType.Text;
                        _cmd2.CommandText = String.Format("select max(spd_cve) from xcdconapl_cl where tipdoc_cve = 'ltasis' and num_reng = 0");
                        _conn2.Open();
                        horaFin = Convert.ToString(_cmd2.ExecuteScalar());
                        _cmd2.ExecuteNonQuery();
                        _conn2.Close();
                        string[] horaFinVal = horaFin.Split(':');

                        DateTime fec_valFin = new DateTime(NFechaIni.Year, NFechaIni.Month, NFechaIni.Day, Int32.Parse(horaFinVal[0]), Int32.Parse(horaFinVal[1]), 00);
                        DateTime fec_valIni = new DateTime(NFechaIni.Year, NFechaIni.Month, NFechaIni.Day, Int32.Parse(horaIniVal[0]), Int32.Parse(horaIniVal[1]), 00);
                        if (NFechaFin > fec_valFin)
                        {
                            Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, solo se puede reagendar antes de las " + horaFin + " horas.'); window.location.href = 'AgendaCitas.aspx';</script>");
                        }
                        else if (NFechaIni < fec_valIni)
                        {
                            Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, solo se puede reagendar despues de las " + horaIni + " horas.'); window.location.href = 'AgendaCitas.aspx';</script>");
                        }
                        else
                        {
                            Int32 numCita = Int32.Parse(e.Id);

                            int? error = 0, errorD = 0;
                            if (e.NewResource.Equals("A") == true)
                            {
                                refer = "1";
                            }
                            else if (e.NewResource.Equals("B") == true)
                            {
                                refer = "2";
                            }
                            else if (e.NewResource.Equals("C") == true)
                            {
                                refer = "3";
                            }
                            else if (e.NewResource.Equals("Otra") == true)
                            {
                                refer = "4";
                            }

                            try
                            {
                                errorD = 0;
                                mensaje = "";
                                Entidades.sp_AutValCita_Result Reagendo = logicaNegocio.autValCita(ef_cve.Trim(), tipdoc_cve.Trim(), numCita, 6, 1, 1, id_ultact, "Reasignacion", NFechaIni, NFechaFin, refer);
                                if (Reagendo != null)
                                {
                                    error = Reagendo.error;
                                    mensaje = Reagendo.mensaje;
                                    if (error == 0)
                                    {
                                        Session["fechaAgendada"] = e.NewEnd;
                                        Session["command"] = "movido";
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Se reagendo la cita correctamente'); window.location.href = 'AgendaCitas.aspx';", true);
                                    }
                                    else
                                    {
                                        Session["fechaAgendada"] = e.NewEnd;
                                        Session["command"] = "movido";
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al reagendar: " + mensaje + "'); window.location.href = 'AgendaCitas.aspx';", true);
                                    }
                                }
                                else
                                {
                                    Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                                }
                            }
                            catch (Exception errores)
                            {//catch 1
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generear encabezado: " + errores.InnerException.Message + "');", true);
                                Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                            }//catch 1  
                            if (error == 0)
                            {
                                try
                                {
                                    //sendEmail(numCita, NFechaIni, NFechaFin, e.NewResource, id_ultact/*, e.Text*/);
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("<script type=\"text/javascript\">alert('Error al enviar email, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                                }
                            }
                        }
                    }
                }
                //DayPilotScheduler1.ScrollX = 2900;

                //DayPilotScheduler1.UpdateWithMessage("El evento se movio a la fecha.");
                //Response.Write("<script type=\"text/javascript\">alert('Cita movida correctamente'); window.location.href = 'AgendaCitas.aspx';</script>");
            }
            else
            {
                //setDataSourceAndBind();

                //DayPilotScheduler1.ScrollX = 2900;
                Response.Write("<script type=\"text/javascript\">alert('No tienes permisos para mover las citas'); window.location.href = 'AgendaCitas.aspx';</script>");
                //DayPilotScheduler1.UpdateWithMessage("No tienes permisos para mover las citas");
            }
        }
        protected void DayPilotScheduler1_BeforeEventRender(object sender, BeforeEventRenderEventArgs e)
        {
            string status = e.DataItem["status"].ToString();
            switch (status)
            {
                case "1": //agendadas (confirmado)
                    e.BackgroundColor = "#E6E6E6"; // gris
                    e.BorderColor = "#D1D1D1";
                    e.FontColor = "black";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    //e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    //e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    break;
                case "2": // canceladas (cancelado)
                    e.BackgroundColor = "#990000"; //rojo
                    e.BorderColor = "#730000";
                    e.FontColor = "white";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    e.EventMoveEnabled = false;
                    break;
                case "3": // accesado a planta (terminado)
                    e.BackgroundColor = "#9A5C00"; //cafe-amarrillo
                    e.BorderColor = "#734100";
                    e.FontColor = "white";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    e.EventMoveEnabled = false;
                    break;
                case "4": // Autorizado ("")
                    e.BackgroundColor = "#037D34"; //verde
                    e.BorderColor = "#005B24";
                    e.FontColor = "white";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    break;
                case "5": // validado ("")
                    e.BackgroundColor = "#00489A"; //azul
                    e.BorderColor = "#002E73";
                    e.FontColor = "white";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    e.EventMoveEnabled = false;
                    break;
                case "6": //reagendadas ("")
                    e.BackgroundColor = "#E6E6E6"; // gris
                    e.BorderColor = "#D1D1D1";
                    e.FontColor = "black";
                    e.EventDeleteEnabled = false; // eliminacion desactivada
                    e.EventRightClickEnabled = false; //click derecho desactivado
                    //e.EventClickEnabled = false; //desactivamos el click sobre la cita
                    //e.EventResizeEnabled = false; // desactivamos el alargamiento de la cita
                    break;
            }
        }
        protected void DayPilotBubble1_RenderContent(object sender, RenderEventArgs e)
        {
            if (e is RenderResourceBubbleEventArgs)
            {
                RenderResourceBubbleEventArgs re = e as RenderResourceBubbleEventArgs;
                e.InnerHTML = "<b>Resource header details</b><br />Value: " + re.ResourceId;
            }
            else if (e is RenderCellBubbleEventArgs)
            {
                RenderCellBubbleEventArgs re = e as RenderCellBubbleEventArgs;
                e.InnerHTML = "<b>Cell details</b><br />Resource:" + re.ResourceId + "<br />From:" + re.Start + "<br />To: " + re.End;
            }
        }
        protected string PageHash
        {
            get
            {
                return null;
            }
        }
        protected void DayPilotScheduler1_Refresh(object sender, DayPilot.Web.Ui.Events.RefreshEventArgs e)
        {
            DayPilotScheduler1.StartDate = e.StartDate;
            setDataSourceAndBind();
            DayPilotScheduler1.Update(CallBackUpdateType.Full);
        }
        protected void DayPilotBubble1_RenderEventBubble(object sender, RenderEventBubbleEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<b>{0}</b><br />", e.Text);
            sb.AppendFormat("Cita Nº: {0}<br />", e.Value);
            sb.AppendFormat("Fecha Entrada: {0}<br />", e.Start);
            sb.AppendFormat("Fecha Salida: {0}<br />", e.End);
            sb.AppendFormat("<br />");

            e.InnerHTML = sb.ToString();

        }
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DateTime fechaActual = DateTime.Now;
            DateTime hoy = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day);

            //Determine the RowIndex of the Row whose Button was clicked.
            int row = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            //Get the value of column from the DataKeys using the RowIndex.
            int folio = Convert.ToInt32(GridView1.DataKeys[row].Values[0]);

            /*bool autorizado = logicaNegocio.autValCita("001", "ltasis", folio, 1, 1, 1, "GEC");
            if (autorizado)
            {
                Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada'); document.location = 'AgendaCitas.aspx'</script>");
            }*/

            /* mod 25/01/2017 - de modifico la validacion del resultado del cambio del status de la cita */
            Entidades.sp_AutValCita_Result autorizada = logicaNegocio.autValCita("001", "ltasis", folio, 4, 1, 1, userCve, "Autorizacion", DateTime.Now, DateTime.Now, "");
            if (autorizada != null)
            {
                error = autorizada.error;
                mensaje = autorizada.mensaje;
                if (Convert.ToInt32(error) == 0)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada con Num. Cita: " + mensaje + "'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('" + error + "'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
            }
            //Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada'); document.location = 'AgendaCitas.aspx'</script>");
        }
        /* el boton validar no se implementara, por esa razon no aparece la columna en la tabla */
        protected void btnValidar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DateTime fechaActual = DateTime.Now;
            DateTime hoy = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day);

            //Determine the RowIndex of the Row whose Button was clicked.
            int row = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            //Get the value of column from the DataKeys using the RowIndex.
            int folio = Convert.ToInt32(GridView1.DataKeys[row].Values[0]);

            /*bool autorizado = logicaNegocio.autValCita("001", "ltasis", folio, 1, 1, 1, "GEC");
            if (autorizado)
            {*/
            Response.Write("<script type=\"text/javascript\">alert('Entrada validada'); window.location.href = 'AgendaCitas.aspx';</script>");
            /*}*/
        }

        protected void btReinicar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgendaCitas.aspx");
        }

        protected void btAgendarCita_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgendarCita.aspx");
        }

        protected void btPAP_Click(object sender, EventArgs e)
        {
            Response.Redirect("PAP.aspx");
        }

        protected void btPTP_Click(object sender, EventArgs e)
        {
            Response.Redirect("PTP.aspx");
        }

        protected void btPAD_Click(object sender, EventArgs e)
        {
            Response.Redirect("PAD.aspx");
        }

        protected void btPAS_Click(object sender, EventArgs e)
        {
            Response.Redirect("PAS.aspx");
        }
        /* add 27/01/2017 se agregaron los metodos de los botones cancelar y acceso a planta */
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DateTime fechaActual = DateTime.Now;
            DateTime hoy = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day);

            //Determine the RowIndex of the Row whose Button was clicked.
            int row = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            //Get the value of column from the DataKeys using the RowIndex.
            int folio = Convert.ToInt32(GridView1.DataKeys[row].Values[0]);

            Entidades.sp_AutValCita_Result cancelada = logicaNegocio.autValCita("001", "ltasis", folio, 2, 1, 1, userCve, "Cancelacion", DateTime.Now, DateTime.Now, "");
            if (cancelada != null)
            {
                error = cancelada.error;
                mensaje = cancelada.mensaje;
                if (Convert.ToInt32(error) == 0)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Cita: cancelada correctamente'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('" + error + "'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
            }
            //Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada'); document.location = 'AgendaCitas.aspx'</script>");
        }

        protected void btnAcceso_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DateTime fechaActual = DateTime.Now;
            DateTime hoy = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day);

            //Determine the RowIndex of the Row whose Button was clicked.
            int row = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            //Get the value of column from the DataKeys using the RowIndex.
            int folio = Convert.ToInt32(GridView1.DataKeys[row].Values[0]);
            Session["numCita"] = folio;
            //string ConnectionString = @"Data Source=skyhdev3;Initial Catalog=develop;User ID=soludin_develop;Password=dinamico20"; ;
            string r = "";
            SqlConnection cnn = new SqlConnection(CadenaConecta);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = String.Format("select plazo from itmov where tipo_doc = 'ltasis' and ef_cve = '001' and num_fol in ({0})", folio);
            cnn.Open();
            /* ejecucion de la insercion en la base de datos */
            r = cmd.ExecuteScalar().ToString();
            cmd.ExecuteNonQuery();
            cnn.Close();
            Session["acceso"] = r;
            /*bool autorizado = logicaNegocio.autValCita("001", "ltasis", folio, 1, 1, 1, "GEC");
            if (autorizado)
            {
                Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada'); document.location = 'AgendaCitas.aspx'</script>");
            }*/

            /* mod 25/01/2017 - de modifico la validacion del resultado del cambio del status de la cita */
            Entidades.sp_AutValCita_Result autorizada = logicaNegocio.autValCita("001", "ltasis", folio, 3, 1, 1, userCve, "Acceso Planta" + userCve, DateTime.Now, DateTime.Now, "");
            if (autorizada != null)
            {
                error = autorizada.error;
                mensaje = autorizada.mensaje;
                if (Convert.ToInt32(error) == 0)
                {
                    /*Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada con Num. Cita: '" + mensaje + "''); document.location = 'AgendaCitas.aspx'</script>");*/
                    /* se va a mostrar la pantalla que cargara el pdf con la etiqueta de pase */
                    Response.Write("<script>");
                    Response.Write("window.open('Pases.aspx','_blank')");
                    Response.Write("</script>");
                    Response.Write("<script type=\"text/javascript\">window.location.href = 'AgendaCitas.aspx';</script>");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('" + error + "'); window.location.href = 'AgendaCitas.aspx';</script>");
                }
            }
            //Response.Write("<script type=\"text/javascript\">alert('Entrada Autorizada'); document.location = 'AgendaCitas.aspx'</script>");
            //Response.Write("<script>");
            //Response.Write("window.open('Pases.aspx','_blank')");
            //Response.Write("</script>");
            //Response.Write("<script type=\"text/javascript\">document.location = 'AgendaCitas.aspx'</script>");
        }
        protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
        {
            switch (e.Command)
            {
                case "navigate":
                    DateTime start = (DateTime)e.Data["start"];
                    DateTime end = (DateTime)e.Data["end"];
                    DateTime day = (DateTime)e.Data["day"];
                    Session["command"] = "navigate";
                    //coloca el marcado en la fecha que el usuario selecciona
                    DayPilotScheduler1.StartDate = start;
                    //muestra un mensaje con la fecha a la que el usuario se a movido
                    DayPilotScheduler1.UpdateWithMessage("Haz Cambiado La fecha A: " + start);
                    //carga la informacion de esa fecha
                    initData();
                    DayPilotScheduler1.DataBind();
                    DayPilotScheduler1.Update();
                    break;
                case "nuevaFecha":
                    /*DayPilotScheduler1.DataBind();
                    //muestra un mensaje con la fecha a la que el usuario a actualizado
                    DayPilotScheduler1.UpdateWithMessage("Haz Cambiado La Fecha De La Cita");
                    //carga la informacion de esa fecha
                    initData();*/
                    break;
                case "error":
                    DayPilotScheduler1.DataBind();
                    //muestra un mensaje con la fecha a la que el usuario a actualizado
                    DayPilotScheduler1.UpdateWithMessage("No Se Pudo Cambiar La Fecha a la Cita");
                    //carga la informacion de esa fecha
                    break;
                default:
                    throw new Exception("Unknown command.");

            }
        }
        public void sendEmail(int? numCita, DateTime fechaIni, DateTime fechaFin, string sala, string id_ultact/*, string empresa*/)
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
                /*"<tr>" +
                 "<td width='60%'>" +
                  "<strong>Empresa:</strong> " + empresa +
                 "</td>" +
                "</tr>" +*/
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

        protected void fechaFiltro_TextChanged(object sender, EventArgs e)
        {
            //Session["fecFiltro"] = fechaFiltro.Text;
            //Response.Redirect("AgendaCitas.aspx", false);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            LlenaGrid();
        }
        protected void DayPilotScheduler1_EventResize(object sender, DayPilot.Web.Ui.Events.EventResizeEventArgs e)
        {
            if (Session["RolCve"].ToString().Equals("ATD") == true || Session["RolCve"].ToString().Equals("DIR") == true || Session["RolCve"].ToString().Equals("ADT") == true)
            {
                /*
                #region Simulation of database update

                DataRow dr = table.Rows.Find(e.Id);
                if (dr != null)
                {
                    dr["fec_ultact"] = e.NewStart;
                    dr["fec_prom"] = e.NewEnd;
                    dr["refer"] = e.NewResource;
                    table.AcceptChanges();
                }
                else // moved from outside
                {
                    dr = table.NewRow();
                    dr["fec_ultact"] = e.NewStart;
                    dr["fec_prom"] = e.NewEnd;
                    dr["num_fol"] = e.Id;
                    dr["imp_letra"] = e.Text;
                    dr["refer"] = e.NewResource;

                    table.Rows.Add(dr);
                    table.AcceptChanges();
                }

                #endregion
                */
                //setDataSourceAndBind();
                /* revisar!! */

                DataRow dr = table.Rows.Find(e.Id);
                if (dr != null)
                {
                    string ef_cve = "001",
                    tipdoc_cve = "LTASIS",
                    refer = "",
                    art_tip = "SER",
                    sku_cve = "2801",
                    id_ultact = Session["UserCve"].ToString();
                    DateTime NFechaIni = e.NewStart;
                    DateTime NFechaFin = e.NewEnd;
                    DateTime fec_ultact = DateTime.Now;
                    Int32 numCita = Int32.Parse(e.Id);

                    string horaIni;
                    SqlConnection _conn = new SqlConnection(CadenaConecta);
                    SqlCommand _cmd = new SqlCommand();
                    _cmd.Connection = _conn;
                    _cmd.CommandType = CommandType.Text;
                    _cmd.CommandText = String.Format("select min(spd_cve) from xcdconapl_cl where tipdoc_cve = 'ltasis' and num_reng = 0");
                    _conn.Open();
                    horaIni = Convert.ToString(_cmd.ExecuteScalar());
                    _cmd.ExecuteNonQuery();
                    _conn.Close();
                    string[] horaIniVal = horaIni.Split(':');

                    string horaFin;
                    SqlConnection _conn2 = new SqlConnection(CadenaConecta);
                    SqlCommand _cmd2 = new SqlCommand();
                    _cmd2.Connection = _conn2;
                    _cmd2.CommandType = CommandType.Text;
                    _cmd2.CommandText = String.Format("select max(spd_cve) from xcdconapl_cl where tipdoc_cve = 'ltasis' and num_reng = 0");
                    _conn2.Open();
                    horaFin = Convert.ToString(_cmd2.ExecuteScalar());
                    _cmd2.ExecuteNonQuery();
                    _conn2.Close();
                    string[] horaFinVal = horaFin.Split(':');

                    DateTime fec_valFin = new DateTime(NFechaIni.Year, NFechaIni.Month, NFechaIni.Day, Int32.Parse(horaFinVal[0]), Int32.Parse(horaFinVal[1]), 00);
                    DateTime fec_valIni = new DateTime(NFechaIni.Year, NFechaIni.Month, NFechaIni.Day, Int32.Parse(horaIniVal[0]), Int32.Parse(horaIniVal[1]), 00);
                    if (NFechaFin > fec_valFin)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, solo se puede reagendar antes de las " + horaFin + " horas.'); window.location.href = 'AgendaCitas.aspx';</script>");
                    }
                    else if (NFechaIni < fec_valIni)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, solo se puede reagendar despues de las " + horaIni +  " horas.'); window.location.href = 'AgendaCitas.aspx';</script>");
                    }
                    else
                    {
                        int? error = 0, errorD = 0;
                        if (e.Resource.Equals("A") == true)
                        {
                            refer = "1";
                        }
                        else if (e.Resource.Equals("B") == true)
                        {
                            refer = "2";
                        }
                        else if (e.Resource.Equals("C") == true)
                        {
                            refer = "3";
                        }
                        else if (e.Resource.Equals("Otra") == true)
                        {
                            refer = "4";
                        }

                        try
                        {
                            errorD = 0;
                            mensaje = "";
                            Entidades.sp_AutValCita_Result Reagendo = logicaNegocio.autValCita(ef_cve.Trim(), tipdoc_cve.Trim(), numCita, 7, 1, 1, id_ultact, "Reasignacion", NFechaIni, NFechaFin, refer);
                            if (Reagendo != null)
                            {
                                error = Reagendo.error;
                                mensaje = Reagendo.mensaje;
                                if (error == 0)
                                {
                                    Session["fechaAgendada"] = e.NewEnd;
                                    Session["command"] = "movido";
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Se reagendo la cita correctamente'); window.location.href = 'AgendaCitas.aspx';", true);
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al reagendar: " + mensaje + "'); window.location.href = 'AgendaCitas.aspx';", true);
                                }
                            }
                            else
                            {
                                Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                            }
                        }
                        catch (Exception errores)
                        {//catch 1
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generear encabezado: " + errores.InnerException.Message + "');", true);
                            Response.Write("<script type=\"text/javascript\">alert('Error al reagendar la cita, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                        }//catch 1  
                        if (error == 0)
                        {
                            try
                            {
                                //sendEmail(numCita, NFechaIni, NFechaFin, e.NewResource, id_ultact/*, e.Text*/);
                            }
                            catch (Exception ex)
                            {
                                Response.Write("<script type=\"text/javascript\">alert('Error al enviar email, intente mas tarde'); window.location.href = 'AgendaCitas.aspx';</script>");
                            }
                        }
                    }
                }
                //DayPilotScheduler1.ScrollX = 2900;

                //DayPilotScheduler1.UpdateWithMessage("El evento se movio a la fecha.");
                //Response.Write("<script type=\"text/javascript\">alert('Cita movida correctamente'); window.location.href = 'AgendaCitas.aspx';</script>");
            }
            else
            {
                //setDataSourceAndBind();

                //DayPilotScheduler1.ScrollX = 2900;
                Response.Write("<script type=\"text/javascript\">alert('No tienes permisos para mover las citas'); window.location.href = 'AgendaCitas.aspx';</script>");
                //DayPilotScheduler1.UpdateWithMessage("No tienes permisos para mover las citas");
            }
        }
        protected void DayPilotScheduler1_IncludeCell(object sender, IncludeCellEventArgs e)
        {
            // hiding lunch break
            //if (e.Start.Hour == 13)
            //{
            //    e.Visible = false;
            //}
            // oculto el domingo del navegator
            if (e.Start.DayOfWeek == 0)
            {
                e.Visible = false;
            }
        }
    }
}