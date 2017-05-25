<%@ Page Title="" Language="C#" MasterPageFile="~/Citas.Master" AutoEventWireup="true" CodeBehind="AgendaCitas.aspx.cs" Inherits="WebAppCitas.AgendaCitas" EnableEventValidation="false" Debug="false" EnableViewState="false" ViewStateMode="Disabled" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("Content/scheduler_8.css")%>" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("Content/styles.css")%>" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <%-- <script>
        $(function () {
            $("#<%= //fechaFiltro.ClientID %>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
    <script>
        $('#txtFecCita').change(function () {
            $('#ddHoraE').removeAttr('disabled');
        })
    </script>--%>
    <script type="text/javascript">
        function SessionExpireAlert(timeout) {
            var seconds = timeout / 1000;
            document.getElementsByName("secondsIdle").innerHTML = seconds;
            document.getElementsByName("seconds").innerHTML = seconds;
            setInterval(function () {
                seconds--;
                document.getElementById("seconds").innerHTML = seconds;
                document.getElementById("secondsIdle").innerHTML = seconds;
            }, 1000);
            setTimeout(function () {
                //Show Popup before 20 seconds of timeout.
                $find("mpeTimeout").show();
            }, timeout - 10 * 1000);
            setTimeout(function () {
                window.location = "Login.aspx";
            }, timeout);
        };
        function ResetSession() {
            //Redirect to refresh Session.
            window.location = window.location.href;
        }
    </script>
    <script type="text/javascript">
      
        /* Event editing helpers - modal dialog */
        /*function dialog() {
            var modal = new DayPilot.Modal();
            modal.onClosed = function (args) {
                window.console && console.log(args);
                if (args.result == "OK") {
                    dps1.commandCallBack('refresh');
                }
                dps1.clearSelection();
            };
            return modal;
        }

        function timeRangeSelected(start, end, resource) {
            var modal = dialog();
            modal.showUrl("NuevaCita.aspx");
        }
        */
        function dialog() {
            var modal = new DayPilot.Modal();
            modal.top = 100;
            modal.dragDrop = true;
            modal.width = 800;
            modal.opacity = 70;
            modal.border = "10px solid #d0d0d0";
            modal.height = 300;
            modal.zIndex = 100;
            modal.closed = function (args) {
                if (this.result == 'OK') {
                    dps1.commandCallBack('nuevaFecha');
                    setTimeout("location.href='AgendaCitas.aspx'");
                } else if (this.result == 'error') {
                    dps1.commandCallBack('error');
                }
                dps1.clearSelection();
            };

            
            return modal;
        }
        function eventClick(e) {
            var modal = dialog();
            modal.showUrl("NuevaCita.aspx?id=" + e.id() + "&opc=1");
            //alert(e.id());
            //modal.showUrl("Edit.aspx?id=" + e.value() + "&hash=<= PageHash %>");
        }
        
        function afterRender(isCallBack) {
            dpn.visibleRangeChangedCallBack(); // update free/busy after adding/changing/deleting events in the calendar
        }
    </script>

    <div class="container-fluid">
        <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <h3>Session Idle:&nbsp;<span id="secondsIdle"></span>&nbsp;seconds.</h3>
        <asp:LinkButton ID="lnkFake" runat="server" />
        <asp:ModalPopupExtender ID="mpeTimeout" BehaviorID ="mpeTimeout" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
            OkControlID="btnYes" CancelControlID="btnNo" BackgroundCssClass="modalBackground" OnOkScript = "ResetSession()">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style=display: none">
            <div class="header">
                Session Expiring!
            </div>
            <div class="body">
                Your Session will expire in&nbsp;<span id="seconds"></span>&nbsp;seconds.<br />
                Do you want to reset?
            </div>
            <div class="footer" align="right">
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="yes" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="no" />
            </div>
        </asp:Panel>--%>
        <div class="row">
            <div class="col-md-2">
                <div <% Response.Write(Session["visible"]); %>>
                    <br />
                    <br />
                </div>
                <DayPilot:DayPilotNavigator
                    ID="DayPilotNavigator1" runat="server"
                    ClientObjectName="dpn"
                    ShowMonths="2"
                    SkipMonths="2"
                    BoundDayPilotID="DayPilotScheduler1"
                    SelectMode="Day"
                    CssClassPrefix="navigator"
                    ShowWeekNumbers="true"
                    DataStartField="start"
                    DataEndField="end"
                    CellHeight="15"></DayPilot:DayPilotNavigator>
                <br />
            </div>
            <div class="col-md-10">
                <DayPilot:DayPilotScheduler
                    ID="DayPilotScheduler1"
                    runat="server"
                    DataStartField="start"
                    DataEndField="end"
                    DataTextField="name"
                    DataIdField="id"
                    DataResourceField="column"
                    RowHeaderWidth="100"
                    Scale="CellDuration"
                    CellDuration="30"
                    RowMinHeight="60"
                    EventHeight="60"
                    CellWidth="60"
                    ClientObjectName="dps1"
                    EventMoveHandling="PostBack"
                    OnEventMove="DayPilotScheduler1_EventMove"
                    ShowToolTip="true"
                    HeightSpec="Fixed"
                    Height="300"
                    TreeEnabled="true"
                    TreeIndent="15"
                    SyncResourceTree="true"
                    DurationBarVisible="true"
                    CellGroupBy="Month"
                    OnBeforeEventRender="DayPilotScheduler1_BeforeEventRender"
                    EventMoveJavaScript="dpc1.eventMoveCallBack(e, newStart, newEnd, oldColumn, newColumn, data);"
                    EventClickHandling="JavaScript"
                    EventClickJavaScript="eventClick(e);"
                    DataValueField="id"
                    OnCommand="DayPilotScheduler1_Command"
                    EventResizeHandling="PostBack"
                    OnEventResize="DayPilotScheduler1_EventResize"
                    BusinessBeginsHour="8"
                    BusinessEndsHour="18"
                    ShowNonBusiness="true"
                    OnIncludeCell="DayPilotScheduler1_IncludeCell"
                    BubbleID="DayPilotBubble1"
                    xResourceBubbleID="DayPilotBubble1"
                    Theme="scheduler_8">
                    <TimeHeaders>
                        <DayPilot:TimeHeader GroupBy="Month" Format="MMMM yyyy" />
                        <DayPilot:TimeHeader GroupBy="Day" Format="" />
                        <DayPilot:TimeHeader GroupBy="Hour" Format="" />
                    </TimeHeaders>
                    <Resources>
                        <DayPilot:Resource Name="Salas" Id="Salas" Expanded="true">
                            <Children>
                                <DayPilot:Resource Name="Sala A" Id="A" Expanded="False" />
                                <DayPilot:Resource Name="Sala B" Id="B" Expanded="False" />
                                <DayPilot:Resource Name="Sala C" Id="C" Expanded="False" />
                            </Children>
                        </DayPilot:Resource>
                    </Resources>
                </DayPilot:DayPilotScheduler>
                <DayPilot:DayPilotBubble
                    ID="DayPilotBubble1"
                    runat="server"
                    OnRenderContent="DayPilotBubble1_RenderContent"
                    ClientObjectName="bubble"
                    OnRenderEventBubble="DayPilotBubble1_RenderEventBubble"
                    Width="250"
                    Position="EventTop">
                </DayPilot:DayPilotBubble>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <br />
                <br />
                <img src="Media/Usuario.png" width="170" height="170" /><br />
                <div class="alert alert-info text-center" role="alert">
                    Bienvenid@: <strong>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></strong>
                </div>
                <asp:Button ID="btReinicar" runat="server" Text="Fecha actual" OnClick="btReinicar_Click" class="btn btn col-lg-12 btnFA"></asp:Button>
                <br />
                <br />
                <div <% Response.Write(Session["visible"]); %>>
                    <button type="button" class="btn btn col-lg-12 btnFA" data-toggle="modal" data-target="#AgendaCitas" data-whatever="@mdo">Agendar cita</button>
                    <div class="modal fade" id="AgendaCitas" tabindex="-1" role="dialog" aria-labelledby="AgendaCita">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title" id="AgendaCita" align="center">Seleccione tipo de cita que agendará:</h4>
                                </div>
                                <div class="modal-body">

                                    <div class="form-group" align="center">
                                        <asp:Button ID="Button1" runat="server" class="btn btn-primary" Text="Permiso de Acceso a Sala (PAS)" OnClick="btPAS_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btPAP" runat="server" class="btn btn-primary" Text="Permiso de Acceso a Planta (PAP)" OnClick="btPAP_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btPTP" runat="server" class="btn btn-primary" Text="Permiso de Trabajo en Planta (PTP)" OnClick="btPTP_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btPAD" runat="server" class="btn btn-primary" Text="Permiso de Acceso a Dirección (PAD)" OnClick="btPAD_Click" />

                                    </div>
                                </div>
                                <%--
                                  <div class="modal-footer">
                                   <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                  </div>
                                --%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-10">
                <div class="row">
                    <div <% Response.Write(Session["visible"]); %>>
                        <br />
                        <div class="row">
                            <h3 class="text-center">Status en la agenda:</h3>
                            <br />
                        </div>
                        <div>
                            <div class="col-md-8 col-md-offset-3">
                                <ul class="list-inline">
                                    <li>
                                        <div class="alert-agendada" role="alert"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>Agendada </div>
                                    </li>
                                    <li>
                                        <div class="alert-autorizada" role="alert"><span class="glyphicon glyphicon-time" aria-hidden="true"></span>Autorizada </div>
                                    </li>
                                    <!--<li>
                                        <div class="alert-validada" role="alert"><span class="glyphicon glyphicon-check" aria-hidden="true"></span> Validada </div>
                                    </li>-->
                                    <li>
                                        <div class="alert-terminada" role="alert"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>Terminada </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
                <%--<div class="row">
                    <div class="form-inline col-md-8 col-md-offset-2">
                        <h4 class = "text-center"><strong>Fitros de Busqueda: </strong></h4>
                        <div class="form-group">
                            <label for="fechaFiltro">Fecha de Visita:</label>
                            <asp:TextBox ID="fechaFiltro" CssClass="form-control" runat="server" Type="date" AutoPostBack="true" OnTextChanged="fechaFiltro_TextChanged" ></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="nomEmpresaFiltro">Nombre Empresa:</label>
                            <asp:TextBox ID="nomEmpresaFiltro" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>
                <div class="row">
                    <h4>Citas para el dia de hoy:</h4>
                    <% 
                        for (int i = 1; i <= GridView1.Rows.Count; i++)
                        {
                            foreach (GridViewRow row in GridView1.Rows)
                            {
                                if (row.Cells[6].Text.Equals("Autorizado") == true)
                                {
                                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#026714");
                                    row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    row.Cells[7].Text = "";
                                    row.Cells[8].Text = "";
                                    row.Cells[9].Text = "";
                                    //DateTime fechaCita = DateTime.Parse(row.Cells[2].Text);
                                    if (DateTime.Now.ToString("yyyy-MM-dd").Equals(row.Cells[2].Text.Substring(0, 10)) == false)
                                    {
                                        row.Cells[10].Text = "";
                                    }
                                    //row.Cells[9].Text = "";
                                    //row.Cells[8].Text = "";
                                }
                                else if (row.Cells[6].Text.Equals("Terminado") == true)
                                {
                                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#A25600");
                                    row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    row.Cells[7].Text = "";
                                    row.Cells[8].Text = "";
                                    row.Cells[9].Text = "";
                                    row.Cells[10].Text = "";
                                    //row.Cells[10].Text = "";
                                }
                                else if (row.Cells[6].Text.Equals("Confirmado") == true)
                                {
                                    //row.Cells[7].Text = "";
                                    //row.Cells[9].Text = "";
                                    row.Cells[10].Text = "";
                                }
                                if (row.Cells[6].Text.Equals("Reagendado") == true)
                                {
                                    //row.Cells[8].Text = "";
                                    //row.Cells[9].Text = "";
                                    row.Cells[10].Text = "";
                                }
                                else if (row.Cells[6].Text.Equals("Cancelado") == true)
                                {
                                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#AD251A");
                                    row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    row.Cells[7].Text = "";
                                    row.Cells[8].Text = "";
                                    row.Cells[9].Text = "";
                                    row.Cells[10].Text = "";
                                    //row.Cells[10].Text = "";
                                }
                                else if (row.Cells[4].Text.Equals("Otra") == true)
                                {
                                    if (row.Cells[6].Text.Equals("Autorizado") == true)
                                    {
                                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#026714");
                                        row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        row.Cells[7].Text = "";
                                        row.Cells[8].Text = "";
                                        row.Cells[9].Text = "";
                                        //row.Cells[9].Text = "";
                                        //row.Cells[8].Text = "";
                                    }
                                    else if (row.Cells[6].Text.Equals("Terminado") == true)
                                    {
                                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#A25600");
                                        row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        row.Cells[7].Text = "";
                                        row.Cells[8].Text = "";
                                        row.Cells[9].Text = "";
                                        row.Cells[10].Text = "";
                                        //row.Cells[10].Text = "";
                                    }
                                    else if (row.Cells[6].Text.Equals("Confirmado") == true)
                                    {
                                        //row.Cells[7].Text = "";
                                        //row.Cells[9].Text = "";
                                        row.Cells[10].Text = "";
                                    }
                                    else if (row.Cells[6].Text.Equals("Reagendado") == true)
                                    {
                                        //row.Cells[8].Text = "";
                                        //row.Cells[9].Text = "";
                                        row.Cells[10].Text = "";
                                    }
                                    else if (row.Cells[6].Text.Equals("Cancelado") == true)
                                    {
                                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#AD251A");
                                        row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        row.Cells[7].Text = "";
                                        row.Cells[8].Text = "";
                                        row.Cells[9].Text = "";
                                        row.Cells[10].Text = "";
                                        //row.Cells[10].Text = "";
                                    }
                                    else
                                    {
                                        row.Cells[8].Text = "<a class='btn btn-primary' href='NuevasCita.aspx?id=" + row.Cells[0].Text + "&opc=2' class='btn btn-primary' />Reagendar</a>";
                                        //row.Cells[9].Text = "";
                                        //row.Cells[10].Text = "";
                                    }

                                }
                            }
                        }
                    %>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True"
                        HeaderStyle-BackColor="#363636" HeaderStyle-ForeColor="White"
                        CssClass="table table-hover" HeaderStyle-Height="50px"
                        EmptyDataText="No hay citas agendadas el dia de hoy" DataKeyNames="num_fol"
                        PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" DataSourceID="CitasSalas">
                        <PagerStyle CssClass="pagination-ys" />
                        <Columns>
                            <asp:BoundField DataField="num_fol" HeaderText="Num Cita:" SortExpression="num_fol" ItemStyle-Width="150" />
                            <asp:BoundField DataField="imp_letra" HeaderText="Empresa:" ReadOnly="True" SortExpression="imp_letra" ItemStyle-Width="150" />
                            <asp:BoundField DataField="fec_ultact" HeaderText="Fecha de Visita:" SortExpression="fec_ultact" ItemStyle-Width="150" />
                            <asp:BoundField DataField="fec_prom" HeaderText="Fecha de Salida:" SortExpression="fec_prom" ItemStyle-Width="150" />
                            <asp:BoundField DataField="spd_cve" HeaderText="Sala:" SortExpression="spd_cve" />
                            <asp:BoundField DataField="fec_letra" HeaderText="Razon de Visita:" SortExpression="fec_letra" ItemStyle-Width="150" />
                            <asp:BoundField DataField="nombre" HeaderText="Status:" SortExpression="nombre" ItemStyle-Width="150" />
                            <asp:TemplateField HeaderText="Autorizar:" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Button Text="Autorizar" ID="btnAutorizar" runat="server" CommandName="Autorizar" CssClass="btn btn-success" OnClick="btnAutorizar_Click" OnClientClick="if ( !confirm('Desea autorizar esta cita?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reagendar:" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <input type="button" onclick="location.href='NuevaCita.aspx?id=' + <%#Eval("num_fol")%>    +'&opc=2'" value="Reagendar" class="btn btn-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cancelar Cita:" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Button Text="Cancelar" ID="btnCancelar" runat="server" CommandName="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" OnClientClick="if ( !confirm('Desea cancelar esta cita?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accesos:" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Button Text="Acceso a Planta" ID="btnAcceso" runat="server" CommandName="Acceso" CssClass="btn btn-warning" OnClick="btnAcceso_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="CitasSalas" runat="server" ConnectionString="<%$ ConnectionStrings:conString %>"
                        SelectCommand="exec sp_consCitas @tipdoc_cve,@rol_cve,@user_cve,@fecha">
                        <SelectParameters>
                            <asp:Parameter Name="tipdoc_cve" Type="String" DefaultValue="ltasis" />
                        </SelectParameters>
                        <SelectParameters>
                            <asp:SessionParameter Name="rol_cve" Type="String" SessionField="RolCve" />
                        </SelectParameters>
                        <SelectParameters>
                            <asp:SessionParameter Name="user_cve" Type="String" SessionField="UserCve" />
                        </SelectParameters>
                        <SelectParameters>
                            <asp:SessionParameter Name="fecha" Type="String" SessionField="fecFiltro" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
