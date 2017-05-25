<%@ Page Title="" Language="C#" MasterPageFile="~/Citas.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebAppCitas.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("Content/cover.css")%>" />-->
<div class="site-wrapper">
    <div class="site-wrapper-inner">
        <div class="">
            <div class="masthead clearfix">
                <div class="">
                    <div class="well"><h3 class="masthead-brand">Agenda Citas <small>Skytex México</small></h3></div>
                </div>
            </div>
            <br />
            <center>
            <div class="inner cover">
                <!--<h1 class="cover-heading">Accede a la aplicacion</h1>-->
                <br />
                <div class="col-md-2">.</div>
                <div class="col-md-4">
                    <img src="Media/logo2.png" class="img-responsive left" width="500" height="500"/>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="user" class="col-sm-2 control-label">Usuario:</label>
                        <asp:DropDownList ID="ddUsuario" runat="server"  class="form-control" required>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="pass" class="col-sm-2 control-label">Contraseña:</label>
                        <input type="password" name="pass" id="pass" class="form-control" required/>
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btEntrar" runat="server" Text="Entrar" OnClick="btEntrar_Click" class="btn btn-success btn-lg"></asp:Button>
                    </div>
                </div>
            </div>
            </center>
        </div>
    </div>
</div>
</asp:Content>
