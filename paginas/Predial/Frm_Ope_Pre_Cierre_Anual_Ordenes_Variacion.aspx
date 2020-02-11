<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cierre_Anual_Ordenes_Variacion.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cierre_Anual_Ordenes_Variacion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css" >
    .Cpe_Panel
    {
        overflow: visible;
        width:98%;
    }
    .Etiqueta_Campo
    {
        display:block;
        clear:both;
        float:left;
        width:185px;
        padding-left:3px;
    }
    .Caja_Texto_Campo
    {
        float:left;
        display:block;
        width:150px;
        text-align:right;
    }
    .Combos
    {
        float:left;
        display:block;
        width:150px;
        text-align:left;
    }
    .Etiqueta_Mensaje_Campo
    {
        float:left;
        display:block;
        width:56%;
        margin:2px 0 0 15px;
    }
    .Division_Entre_Controles
    {
        margin-bottom:7px;
        width:98.8%;
    }
    .Espaciado_Floats {clear:both;width:auto;height:1px;line-height:1px;}
    .Enlace_Archivo:link{color:#3F68B6; font-weight:normal;}
    .Enlace_Archivo:hover{color:#00388C; font-weight:bold;}
    .Enlace_Archivo:active{color:#00388C;}
    .Enlace_Archivo:visited{color:#3F68B6;}
    .Enlace_Archivo{text-decoration:underline;cursor:pointer;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" >
    $(document).ready(function() {
    $("#Td_Tabla_Parametros").click(function() {
            colapsarPanel();
        });
    });
    
    function colapsarPanel() {
        var collPanel = $("#Cpe_Parametros");
        if (collPanel.get_Collapsed())
            collPanel.set_Collapsed(false);
        else
            collPanel.set_Collapsed(true);
    }
    
</script>
<!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION--> 
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Cierre_Anual" runat="server"  AsyncPostBackTimeout="800000"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="98%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Generar órdenes de variación para cuentas con cuota mínima
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>
                <br />
                
                        <%------------------ Parametros ------------------%>
                <asp:Panel ID="Pnl_Parametros" runat="server" CssClass="Cpe_Panel" >
                    <div id="Encabezado_Parametros" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Parametros_Enlace" runat="server" style="font-size:15px;">Parámetros</asp:Label>
                    </div>
                    <asp:Panel id="Pnl_Contenedor_Parametros" runat="server" EnableViewState="true">
                        <asp:Label ID="Lbl_Anio_Aplicar" runat="server" Text="Año a aplicar" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:TextBox ID="Txt_Anio_Aplicar" runat="server" 
                            CssClass="Caja_Texto_Campo" 
                            ReadOnly="false" TabIndex="5"
                            AutoPostBack="true" OnTextChanged="Txt_Anio_Aplicar_Changed" ></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Anio_Aplicar" runat="server" 
                            FilterType="Numbers" TargetControlID="Txt_Anio_Aplicar" ></cc1:FilteredTextBoxExtender>
                        <div class="Espaciado_Floats">&nbsp;</div>
                    
                        <asp:Label ID="Lbl_Cuota_Minima" runat="server" Text="Cuota mínima" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:TextBox ID="Txt_Cuota_Minima" runat="server" CssClass="Caja_Texto_Campo" TabIndex="6" ></asp:TextBox>
                        <asp:Label ID="Lbl_Msg_Cuota_Minima" runat="server" Text=" " CssClass="Etiqueta_Mensaje_Campo" ></asp:Label>
                        <div class="Espaciado_Floats">&nbsp;</div>
                        
                    </asp:Panel>
                    
                <asp:Button Style="background-color:transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />
                </asp:Panel>
                
                <asp:Panel ID="Pnl_Resultados" runat="server" CssClass="Cpe_Panel" Visible="false" >
                    <%------------------ Resultados ------------------%>
                    <div id="Encabezado_Resultados" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Encabezado_Resultados" runat="server" style="font-size:15px;">Adeudos generados</asp:Label>
                    </div>
                    
                    <asp:Label ID="Lbl_Resultado_Generacion" runat="server" Text="" ></asp:Label>
                    
                </asp:Panel>
                <br />
                <asp:Panel ID="Pnl_Errores_Generacion" runat="server" CssClass="Cpe_Panel" Visible="false" >
                    <%------------------ Listado errores ------------------%>
                    <div id="Encabezado_Errores" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Titulo_Errores" runat="server" style="font-size:15px;">Errores de generación de adeudos</asp:Label>
                    </div>
                    
                    <asp:Label ID="Lbl_Errores_Generacion" runat="server" Text="" ></asp:Label>
                    
                    <asp:GridView ID="Grid_Errores" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" Width="98%"
                        HeaderStyle-CssClass="tblHead" style="white-space:normal;"
                        PageSize="10" AllowPaging="false">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Cuenta predial" HeaderStyle-Width="15%" />
                            <asp:BoundField DataField="Value" HeaderText=" Error" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </asp:Panel>
                <asp:ImageButton ID="Btn_Generar_Ordenes" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                    CssClass="Img_Button" ToolTip="Generar órdenes" TabIndex="15" 
                    Visible="true"
                    OnClientClick="return confirm('Se van a generar las órdenes de variación para actualizar las cuentas con cuota mínima ¿Esta seguro que desea continuar?');"
                    onclick="Btn_Generar_Ordenes_Click" />
                <br />
                
                <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


