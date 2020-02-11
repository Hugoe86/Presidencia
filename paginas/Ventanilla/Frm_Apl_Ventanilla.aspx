<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    AutoEventWireup="true" CodeFile="Frm_Apl_Ventanilla.aspx.cs" Inherits="paginas_Ventanilla_Frm_Apl_Ventanilla"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" media="screen">
        #<%=Div_General.ClientID%>
        {
            width: 98%;
            height: 500px;
            background: url( '../imagenes/master/esquina_derecha.png' ) no-repeat 100% 150px;
        }
        .contenedor_circulo_1
        {
            text-align: center;
            float: right;
            position: relative;
            top: 140px;
            right: 172px;
        }
        .contenedor_circulo_2
        {
            text-align: center;
            float: right;
            position: relative;
            right: 135px;
            top: 9px;
        }
        .contenedor_circulo_3
        {
            text-align: center;
            float: right;
            position: relative;
            right: 65px;
        }
        .contenedor_circulo_4
        {
            text-align: center;
            float: right;
            position: relative;
            top: 132px;
            right: 50px;
        }
        .contenedor_central
        {
            position: absolute;
            top: 325px;
            left: 52%;
        }
        .elemento_menu, .contenido_fondo
        {
            display: none;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
    <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
   </script>
    
    <script type="text/javascript">
        $(document).ready(function() {
          $("div.menu_flotante").css("background", "url( '../imagenes/master/siag_banner_componentes.png' ) no-repeat");
          $("div.menu_flotante").css("width", "290px");
        });
    </script>  
      
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"/>

    <div id="Div_General" visible="true" runat="server" class="">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <caption>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </caption>
                    </table>
                </div>
                <%-- div enlaces ventanilla única --%>
                <div id="Div_Contenedor_Enlaces" runat="server">
                    <div class="contenedor_circulo_4">
                        <asp:ImageButton ID="Btn_Portafolio" runat="server" TabIndex="4" ImageUrl="../imagenes/paginas/portafolio.png"
                            ToolTip="Portafolio de documentos" OnClick="Btn_Portafolio_Click" />
                        <br />
                        Portafolio
                    </div>
                    <div class="contenedor_circulo_3">
                        <asp:ImageButton ID="Btn_Consultar_Tramites" runat="server" TabIndex="3" ImageUrl="../imagenes/paginas/Consultar_Tramite_Ciudadano.png"
                            Width="140px" Height="140px" OnClick="Btn_Consultar_Tramites_Click" />
                        <br />
                        Consultar Trámites
                    </div>
                    <div class="contenedor_circulo_2">
                        <asp:ImageButton ID="Btn_Tramites" runat="server" TabIndex="2" ImageUrl="../imagenes/paginas/Ventanilla_Tramties.png"
                            ToolTip="Solicitud de Tramites" Width="130px" Height="130px" OnClick="Btn_Tramites_Click" />
                        <br />
                        Solicitud de Trámites
                    </div>
                    <div class="contenedor_circulo_1">
                        <asp:ImageButton ID="Btn_Atencion_Ciudadana" runat="server" TabIndex="1" ImageUrl="../imagenes/paginas/atencion_ciudadana.png"
                            ToolTip="Registro y consulta de peticiones" OnClick="Btn_Atencion_Ciudadana_Click" />
                    </div>
                </div>
                <%-- div central SIAG --%>
                <div class="contenedor_central">
                    <asp:Image ID="Img_Siag" runat="server" ImageUrl="../imagenes/overlays/escudo.jpg" Style="width:155px;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
</asp:Content>
