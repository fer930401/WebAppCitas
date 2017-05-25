<%@ Page Title="" Language="C#" MasterPageFile="~/Citas.Master" AutoEventWireup="true" CodeBehind="PTP.aspx.cs" Inherits="WebAppCitas.PTP" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <script>
        $(function () {
            $("#<%= txtfecEntrada.ClientID %>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%= txtfecSalida.ClientID %>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
    <div class="page-header text-center">
        <h1>PERMISO DE TRABAJO EN PLANTA (PTP)</h1>
    </div>
    <form>
        <div class="row">
            <div class="col-xs-2">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbfecEntrada" runat="server" Text="Fecha de Entrada:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox  ID="txtfecEntrada" runat="server" CssClass="form-control" placeholder="dd/mm/yy" AutoPostBack="true" OnTextChanged="txtfecEntrada_OnTextChanged" TabIndex="1" />
                    <%--<input type="date" id="txtfecEntrada" class="form-control" placeholder="dd/mm/yyyy">--%>
                    
                </div>
            </div>
            <div class="col-xs-3">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbHoraE" runat="server" Text="Hora de entrada:" CssClass="input-group-addon"></asp:Label>
                    <asp:DropDownList ID="ddHoraE" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddHoraE_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
                </div>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-xs-2">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbFecCita" runat="server" Text="Fecha de Salida:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox  ID="txtfecSalida" runat="server" CssClass="form-control" placeholder="dd/mm/yy" AutoPostBack="true" OnTextChanged="txtfecSalida_OnTextChanged" TabIndex="3" />
                    <%--<input type="date" id="FecCita" class="form-control" placeholder="dd/mm/yyyy">--%>
                    
                </div>
            </div>
            <div class="col-xs-3">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbHoraS" runat="server" Text="Hora de salida:" CssClass="input-group-addon"></asp:Label>
                    <asp:DropDownList ID="ddHoraS" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddHoraS_SelectedIndexChanged" TabIndex="4"></asp:DropDownList>
                </div>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-xs-2">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbAreaTrab" runat="server" Text="Área a trabajar:" CssClass="input-group-addon"></asp:Label>
                    <asp:DropDownList ID="ddAreaTrab" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddAreaTrab_SelectedIndexChanged" TabIndex="5"></asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-3">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbTipoTrab" runat="server" Text="Tipo de Trabajo:" CssClass="input-group-addon"></asp:Label>
                    <asp:DropDownList ID="ddTipTrab" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddTipTrab_SelectedIndexChanged" TabIndex="6"></asp:DropDownList>
                </div>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-xs-2">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbVista" runat="server" Text="Visita a:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox ID="txtVisita" runat="server" required="true" CssClass="form-control" OnSelectedIndexChanged="ddSala_SelectedIndexChanged" TabIndex="7" MaxLength="100">
                    </asp:TextBox>
                </div>
            </div>
            <div class="col-xs-3">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbEmpresa" runat="server" Text="Empresa:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control" required="true" OnSelectedIndexChanged="ddHoraE_SelectedIndexChanged" TabIndex="8" MaxLength="100"></asp:TextBox>
                </div>
            </div>
        </div>

        <br />
        <div class="row">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
                <div class="form-group">
                    <label for="txtDesc">Describir el trabajo a realizar:</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDesc" TextMode="multiline" Rows="3" required="true" TabIndex="9"></asp:TextBox>
                </div>
            </div>
        </div>

        <br />

        <div class="row">
            <div class="col-xs-2">
            </div>
            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbPersona" runat="server" Text="Persona:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox ID="txtPersona" runat="server" CssClass="form-control" TabIndex="10" MaxLength="100"></asp:TextBox>
                </div>
            </div>

            <div class="col-xs-4">
                <div class="input-group">
                    <asp:Label ID="lbImss" runat="server" Text="IMSS:" CssClass="input-group-addon"></asp:Label>
                    <asp:TextBox ID="txtImss" runat="server" CssClass="form-control" TabIndex="11" MaxLength="100"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-1">
                <div class="input-group">
                    <asp:ImageButton ID="imbtAgregar" runat="server" ImageUrl="~/Media/Add.png" Style="width: 30px; height: 30px;" OnClick="imbtAgregar_Click" TabIndex="12" />
                </div>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-xs-3">
            </div>
            <div class="col-xs-6">
                <asp:GridView ID="gvPersEx" runat="server" ShowHeader="true"
                    AutoGenerateColumns="false" ShowFooter="false"
                    HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="white" CssClass="table table-bordered bs-table"
                    HeaderStyle-BackColor="#336699"
                    OnRowDeleting="gvPersEx_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Persona" HeaderText="Persona" ItemStyle-Width="120" />
                        <asp:BoundField DataField="IFE" HeaderText="IFE" ItemStyle-Width="120" />
                        <asp:TemplateField ItemStyle-Width="1px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDeleteA" runat="server" CommandName="Delete" ImageUrl="~/Media/delete.png" Style="width: 30px; height: 30px; margin-left: 30%;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#EEEEEE"
                        ForeColor="Black" />
                </asp:GridView>
            </div>
        </div>


        <br />

        <div class="row">
            <div class="col-xs-4">.</div>
            <div class="col-xs-8">
                <ul class="list-inline">
                    <li><asp:Button ID="btnGuardarPTP" Text="Guardar Datos" runat="server" CssClass="btn btn-default col-xs-offset-5" OnClick="btnGuardarDatosPTP" /></li>
                    <li>.</li>
                    <li><asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger col-xs-offset-5" OnClientClick="if ( !confirm('Desea cancelar la captura de la cita?')) return false;" OnClick="btnCancelar_Click"/></li>
                </ul>
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
                $("#<%= txtfecEntrada.ClientID %>").datepicker();
                $("#<%= txtfecSalida.ClientID %>").datepicker();
            });
        </script>
    </form>
</asp:Content>
