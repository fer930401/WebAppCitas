<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevaCita.aspx.cs" Inherits="WebAppCitas.NuevaCita" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nueva Cita | Skytex</title>
    <!-- asignacion de icono de barra de navegador -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="shortcut icon" href="<%=ResolveUrl("~/Media/skytex.ico") %>" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("Content/bootstrap.css")%>" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("Scripts/modal.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("Scripts/jquery-1.9.1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("Scripts/bootstrap.js") %>"></script>   
    <script type="text/javascript" src="<%= ResolveClientUrl("Scripts/jquery-1.9.1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("Scripts/ie-emulation-modes-warning.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("Scripts/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("Scripts/ie10-viewport-bug-workaround.js") %>"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-2.2.1.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-2.1.1.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <script>
        $(function () {
            $("#<%= txtFecCita.ClientID %>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%= txtFecCita.ClientID %>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
</head>
<body>
    
    <%-- LLenar este formulario al seleccionar la hora de inicio y fin desde la tabla de scheduler --%>
    <form id="form1" runat="server">
        <div class="container">
            <div <% Response.Write(Session["visible"]); %>>
                <nav class="navbar navbar-inverse">
                    <div class="container-fluid">
                        <!-- Brand and toggle get grouped for better mobile display -->
                        <div class="navbar-header">
                          <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                          </button>
                          <a class="navbar-brand" href="#">
                            <img src="Media/logo_skytex.png" class="img-responsive"  width="20" height="20"/>
                          </a>
                        </div>

                        <!-- Collect the nav links, forms, and other content for toggling -->
                        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                            <% if (Session["NombreUser"] == null)
                               {
                            %>
                              <!--<li><a href="#"><span class="glyphicon glyphicon-log-in" aria-hidden="true"> </span> Login</a></li>-->
                            <%       
                               }
                               else
                               {
                            %>
                              <ul class="nav navbar-nav">
                                <li class="active"><a href="AgendaCitas.aspx"><span class="glyphicon glyphicon-home" aria-hidden="true"> </span> Inicio <span class="sr-only">(current)</span></a></li>
                                <!--<li class="dropdown">
                                  <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Dropdown <span class="caret"></span></a>
                                  <ul class="dropdown-menu">
                                    <li><a href="AgendarCita.aspx">Agenda</a></li>
                                    <li><a href="#">Another action</a></li>
                                    <li><a href="#">Something else here</a></li>
                                    <li role="separator" class="divider"></li>
                                    <li><a href="#">Separated link</a></li>
                                    <li role="separator" class="divider"></li>
                                    <li><a href="#">One more separated link</a></li>
                                  </ul>
                                </li>-->
                              </ul>
                              <ul class="nav navbar-nav navbar-right">
                                <li><asp:LinkButton ID="lnkCerrarSession" runat="server" OnClick="CerrarSession"><span class="glyphicon glyphicon-log-in" aria-hidden="true"> </span> Cerrar Sesion</asp:LinkButton></li>
                              </ul>
                            <%   
                               }
                            %>
                        </div><!-- /.navbar-collapse -->
                    </div><!-- /.container-fluid -->
                </nav>
                <br />
                <br />
                <br />
            </div>
            <br />
            <br />
            <div class="container-fluid">
                <br />
                <div class="row">
                    <div class="col-xs-6">
                        <div class="input-group">
                            <asp:Label ID="lbSala" runat="server" Text="Sala de juntas:" CssClass="input-group-addon" ></asp:Label>
                            <asp:DropDownList ID="ddSala" runat="server" CssClass="form-control" required="true"  AutoPostBack="true" OnSelectedIndexChanged="ddSala_SelectedIndexChanged" TabIndex ="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-5">
                        <div class="input-group">
                            <asp:Label ID="lbFecCita" runat="server" Text="Fecha de cita:" CssClass="input-group-addon"></asp:Label>
                            <asp:TextBox ID="txtFecCita" CssClass="form-control" runat="server" Type="date" required="true" AutoPostBack="true" OnTextChanged="txtFecCita_SelectedIndexChanged" TabIndex="2"></asp:TextBox>
                            
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-5">
                        <div class="input-group">
                            <asp:Label ID="lbHoraE" runat="server" Text="Hora de entrada:" CssClass="input-group-addon"></asp:Label>
                            <asp:DropDownList ID="ddHoraE" runat="server" CssClass="form-control" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddHoraE_SelectedIndexChanged" TabIndex="3"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-5">
                        <div class="input-group">
                            <asp:Label ID="lbHoraS" runat="server" Text="Hora de salida:" CssClass="input-group-addon"></asp:Label>
                            <asp:DropDownList ID="ddHoraS" runat="server" CssClass="form-control" required="true" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="ddHoraS_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-4">
                            <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="Reagendar" CssClass="btn btn-success" />
                            <asp:Button ID="ButtonCancel" runat="server" Text="Cancelar" OnClick="ButtonCancel_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
            <script>
                $.datepicker.regional['es'] = {
                    closeText: 'Cerrar',
                    prevText: '< Ant',
                    nextText: 'Sig >',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                    };
                    $.datepicker.setDefaults($.datepicker.regional['es']);
                    $(function () {
                        $("#<%= txtFecCita.ClientID %>").datepicker();
                });
            </script>
            <div <% Response.Write(Session["visible"]); %>>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div class="container">
                    <footer class="piePagina">
	                    <address>
                            <strong>Skytex México S.A de C.V.</strong><br />
                            Corredor Industrial Quetzalcóatl, Huejotzingo, Pue<br />
                        </address>

                        <address>
                            <strong>Pagina Web</strong><br />
                            <a href="http://www.skytex.com.mx/">Skytex México</a>
                        </address>
                    </footer>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
