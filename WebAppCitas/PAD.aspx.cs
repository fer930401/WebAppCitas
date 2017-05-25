using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using LogicaNegocio;

namespace WebAppCitas
{
    public partial class PAD : System.Web.UI.Page
    {
        LogicaNegocioCls logicaNegocio;

        DataTable DT = new DataTable();

        string ef_cve = "001";
        protected void Page_Load(object sender, EventArgs e)
        {
            logicaNegocio = new LogicaNegocioCls();
            if (Session["NombreUser"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
            else if (Session["RolCve"] != null && Session["RolCve"].ToString().Equals("VGC") == true)
            {
                Response.Redirect("AgendaCitas.aspx", false);
            }
            else
            {
                /* mod 24/01/2017 - se llamo al metodo InicializaGrid fuera del la validacion del postback para evitar que en la tabla se llenar con informacion duplicada */
                //InicializaGrid();
                if (!IsPostBack)
                {
                    InicializaGrid();
                    llenaCboSala();
                    ddHoraE.Enabled = false;
                    ddHoraS.Enabled = false;
                    txtFecCita.Enabled = false;
                    ddArea.Enabled = false;
                    txtMaquina.Enabled = false;
                    txtVisita.Enabled = false;
                    txtEmpresa.Enabled = false;
                    txtDesc.Enabled = false;
                    txtPersona.Enabled = false;
                    txtIfe.Enabled = false;
                    imbtAgregar.Enabled = false;
                    btnGuardarDatosPAD.Enabled = false;
                }

                string script = @"<script type='text/javascript'> FechaAct(1, 1, 1);</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, false);
            }
        }

        protected void InicializaGrid()
        {
            /* mod 30.01.2017 - se modifico el llenado de grid para evitar el problema de f5 */
            /*DT.Columns.Add("Persona", Type.GetType("System.String"));
            DT.Columns.Add("IFE", Type.GetType("System.String"));
            Session["DT"] = DT;
            gvPersEx.DataSource = DT;
            gvPersEx.DataBind();*/
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Persona"), new DataColumn("IFE") });
            ViewState["Citas"] = dt;
            /*gvPersEx.DataSource = DT;
            gvPersEx.DataBind();*/
            gvPersEx.DataSource = (DataTable)ViewState["Citas"];
            gvPersEx.DataBind();
        }

        protected void btnGuardarDatos(object sender, EventArgs e)
        {
            string  ef_cve      = "001",
                    tipdoc_cve  = "LTASIS",
                    imp_letra   = txtEmpresa.Text,
                    fec_letra   = txtDesc.Text,
                    refer       = ddSala.Text,
                    suc_aten    = ddArea.Text,
                    desc_op     = txtPersona.Text,
                    lote_num    = txtIfe.Text,
                    art_tip     = "SER",
                    sku_cve     = "2801",
                    obs         = txtMaquina.Text,
                    pr5         = txtNomSala.Text,
                    id_ultact   = Convert.ToString(Session["UserCve"]),
                    mensaje     = "",
                    mensajeD    = "",
                    pr1         = "0",
                    uuid        = txtVisita.Text;

            DateTime    fechaE  = Convert.ToDateTime(txtFecCita.Text),
                        horaE   = Convert.ToDateTime(ddHoraE.Text),
                        horaS   = Convert.ToDateTime(ddHoraS.Text);
            
            DateTime    fec_ultact  = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day, horaE.Hour, horaE.Minute, horaE.Second),             
                        fec_prom    = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day, horaS.Hour, horaS.Minute, horaS.Second);

   
            short plazo = 4;

            int info = gvPersEx.Rows.Count;
            int? error = 0, errorD = 0;

            if (info == 0)
            {//Si no hay registro de visitante
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Te falta capturar personas a ingresar');", true);
                txtPersona.Focus();
            }//Si no hay registro de visitante
            else
            {//Si hay registros de visitante
                try
                { //try 1
                    Entidades.sp_InsertarEncabezadoCitas_Result Encabezado = logicaNegocio.insEncabezado(ef_cve.Trim(), tipdoc_cve.Trim(), fec_ultact, fec_prom, imp_letra.Trim(), fec_letra.Trim(), refer.Trim(), id_ultact.Trim(), plazo, suc_aten.Trim(), obs.Trim(), pr5.Trim(), pr1.Trim(), uuid.Trim());

                    error = Encabezado.error;
                    mensaje = Encabezado.mensaje;

                    if (error == 0)
                    { //if 1
                        DT = (DataTable)ViewState["Citas"];
                        if (DT.Rows.Count > 0)
                        {//if 2
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {//if 3
                                string[] nombre = DT.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                                string[] ifePassport = DT.Rows.OfType<DataRow>().Select(j => j[1].ToString()).ToArray();
                                try
                                {// try2
                                    Entidades.sp_InsertarDetalleCitas_Result detInsertado = logicaNegocio.insDetalle(ef_cve, tipdoc_cve, Int32.Parse(mensaje), nombre[i], ifePassport[i], fechaE, art_tip, sku_cve);
                                    errorD = detInsertado.error;
                                    mensajeD = detInsertado.mensaje;
                                    if (error != 0)
                                    {//if 5
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generar detalle: " + mensajeD + "');", true);
                                    }// if 5
                                }// try2
                                catch (Exception errores)
                                {//catch 2
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generar detalle: " + errores.InnerException.Message + "');", true);
                                }//catch 2
                            }//if 3
                        }//if 2
                    }//if 1
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generear encabezado: " + mensaje + "');", true);
                    }
                }// try 1
                catch (Exception errores)
                {//catch 1
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error al generear encabezado: " + errores.InnerException.Message + "');", true);
                }//catch 1
                if (error == 0)
                {
                    try
                    {
                        errorD = 0;
                        mensajeD = "";
                        Entidades.sp_AutValCita_Result Confirmo = logicaNegocio.autValCita(ef_cve.Trim(), tipdoc_cve.Trim(), Int32.Parse(mensaje), 1, 1, 1, id_ultact, "Confirmacion", DateTime.Now, DateTime.Now, "");

                        errorD = Confirmo.error;
                        mensajeD = Confirmo.mensaje;
                        if (error == 0)
                        {
                            Session["fechaAgendada"] = fechaE;
                            Session["command"] = "movido";
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Se genero la cita correctamente'); window.location.href = 'AgendaCitas.aspx';", true);
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al confirmar: : " + mensajeD + "');", true);
                        }
                    }
                    catch (Exception errores)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Problema al confirmar: " + errores.InnerException.Message + "');", true);
                    }
                }
            }//Si hay registros de visitante
        }

        protected void imbtAgregar_Click(object sender, EventArgs e)
        {
            /* mod 30.01.2017 - se modifico el llenado de grid para evitar el problema de f5 */
            
            //Leemos la información
            string strPersona = txtPersona.Text;
            string strIFE = txtIfe.Text;

            //Leemos el datatable
            if (strPersona != "" && strIFE != "")
            {

                //DT = (DataTable)Session["DT"];

                ////Insertamos el registro
                //DT.Rows.Add(strPersona, strIFE);

                ////Asignamos del DT al gridview
                //gvPersEx.DataSource = DT;
                //gvPersEx.DataBind();

                ////Actualizamos el DT de la variable de sessión
                //Session["DT"] = DT;

                //btnGuardarDatosPAD.Enabled = true;
                ////Limpiamos la pantalla
                //txtPersona.Text = "";
                //txtIfe.Text = "";
                //txtPersona.Focus();
                // asignamos los valores de la tabla a un variable
                DataTable dt = (DataTable)ViewState["Citas"];

                //Insertamos el registro
                dt.Rows.Add(txtPersona.Text.Trim(), txtIfe.Text.Trim());

                //Actualizamos el DT de la variable de sessión
                ViewState["Citas"] = dt;

                //Asignamos del DT al gridview
                gvPersEx.DataSource = (DataTable)ViewState["Citas"];
                gvPersEx.DataBind();

                //Limpiamos los textbox
                txtPersona.Text = string.Empty;
                txtIfe.Text = string.Empty;

                // habilitamos el boton para guarda la info
                btnGuardarDatosPAD.Enabled = true;
                txtPersona.Focus();
            }
            
            
        }

        protected void gvPersEx_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            /*
            DataTable DT = new DataTable();
            DT = (DataTable)Session["DT"];

            GridViewRow row = gvPersEx.SelectedRow;
            DT.Rows[e.RowIndex].Delete();
            //DT.Rows[e.RowIndex].AcceptChanges();
            gvPersEx.DataSource = DT;
            gvPersEx.DataBind();
            */
            DataTable DT = new DataTable();
            DT = (DataTable)ViewState["Citas"];

            GridViewRow row = gvPersEx.SelectedRow;
            DT.Rows[e.RowIndex].Delete();
            //DT.Rows[e.RowIndex].AcceptChanges();
            gvPersEx.DataSource = DT;
            gvPersEx.DataBind();
        }

        protected void llenaCboHoraE()
        {
            DateTime fechaE = Convert.ToDateTime(txtFecCita.Text);
            DateTime fec_ultact = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);
            string sala = ddSala.Text;
            string refer = ddSala.Text;

            short plazo = 4;

            if (sala == "NA")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Selecciona la sala');", true);
                ddSala.Focus();
            }
            else
            {
                List<Entidades.qcomWebHora_Result> horaE = logicaNegocio.obtCboHora("ltasis", fec_ultact, refer, "00:00", ef_cve, plazo, fec_ultact);
                ddHoraE.DataSource = horaE;
                ddHoraE.DataTextField = "Hora";
                ddHoraE.DataValueField = "Clave";
                ddHoraE.DataBind();
                ddHoraE.Items.Insert(0, new ListItem("---Selecciona Hora Entrada---", "NA"));
            }
        }

        protected void llenaCboHoraS()
        {
            DateTime fechaE = Convert.ToDateTime(txtFecCita.Text);
            DateTime fec_ultact = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);
            string sala = ddSala.Text;
            string refer = ddSala.Text;
            string horaE = ddHoraE.Text;
            short plazo = 4;


            List<Entidades.qcomWebHora_Result> horaS = logicaNegocio.obtCboHora("ltasis", fec_ultact, refer, horaE, ef_cve, plazo, fec_ultact);
            ddHoraS.DataSource = horaS;
            ddHoraS.DataTextField = "Hora";
            ddHoraS.DataValueField = "Clave";
            ddHoraS.DataBind();
            ddHoraS.Items.Insert(0, new ListItem("----Selecciona Hora Salida----", "NA"));
        }

        protected void llenaCboSala()
        {
            List<Entidades.qcomsala1_Result> salas = logicaNegocio.obtCboSala("ltasis", "PAD");
            ddSala.DataSource = salas;
            ddSala.DataTextField = "spd_cve";
            ddSala.DataValueField = "prm1";
            ddSala.DataBind();
            ddSala.Items.Insert(0, new ListItem("------ Selecciona Sala ------", "NA"));
        }

        protected void txtFecCita_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaCboHoraE();
            ddHoraE.Enabled = true;
            DateTime    fecha   = Convert.ToDateTime(txtFecCita.Text), 
                        fechaE  = DateTime.Now;

            DateTime fechaAct = new DateTime(fechaE.Year, fechaE.Month, fechaE.Day);

            if (fecha >= fechaAct)
            {
                ddHoraE.Enabled = true;
            }
            else
            {
              //  txtFecCita.Text = fechaAct.ToString("yyyy-MM-dd");
                if (fecha >= fechaAct)
                {
                    ddHoraE.Enabled = true;
                    llenaCboHoraE();
                    ddHoraE.Focus();
                }
                else
                { 
                    string script = "alert('La fecha es invalida');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                    txtFecCita.Text = "yyyy-mm-dd";
                    //txtFecCita.Focus();
                }

            }
        }

        protected void llenaCboAreaVisita()
        {
            List<Entidades.qcomare1_Result> areaVisita = logicaNegocio.obtCboArea("001", "ltasis");
            ddArea.DataSource = areaVisita;
            ddArea.DataTextField = "nombre";
            ddArea.DataValueField = "clave";
            ddArea.DataBind();
            ddArea.Items.Insert(0, new ListItem("---- Selecciona Area Visita ----", "NA"));
        }

        protected void ddSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            String valor = ddSala.SelectedValue;
            txtFecCita.Enabled = true;
            if (valor.Trim() == "4")
            {
                txtNomSala.Enabled = true;
                txtNomSala.Focus();
            }
            else
            {
                txtNomSala.Text = "";
                txtNomSala.Enabled = false;
                //txtFecCita.Focus();
                if (ddHoraE.Enabled == true)
                {
                    ddHoraE.SelectedIndex = 0;
                }

                if (ddHoraS.Enabled == true)
                {
                    ddHoraS.SelectedIndex = 0;
                }
                if (ddArea.Enabled == true)
                {
                    ddArea.SelectedIndex = 0;
                }
                txtNomSala.Enabled = false;
                txtVisita.Enabled = false;
                txtMaquina.Enabled = false;
                txtEmpresa.Enabled = false;
                txtDesc.Enabled = false;
                txtPersona.Enabled = false;
                txtIfe.Enabled = false;
                imbtAgregar.Enabled = false;
                btnGuardarDatosPAD.Enabled = false;
            }            
        }

        protected void ddHoraE_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaCboHoraS();
            ddHoraS.Enabled = true;
            ddHoraS.Focus();
        }

        protected void ddArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMaquina.Focus();
            txtVisita.Enabled   = true;
            txtMaquina.Enabled  = true;
            txtEmpresa.Enabled  = true;
            txtDesc.Enabled     = true;
            txtPersona.Enabled  = true;
            txtIfe.Enabled      = true;
            imbtAgregar.Enabled = true;
            btnGuardarDatosPAD.Enabled = true;
        }

        protected void ddHoraS_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddArea.Enabled = true;
            ddArea.Focus();
            llenaCboAreaVisita();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgendaCitas.aspx", false);
        }

    }
}


