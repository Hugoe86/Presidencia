<%@ Page Title="Actualizar Movimientos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="~/paginas/Compras/Frm_Ope_Pat_Actualizar_Movimientos.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Actualizar_Movimientos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="ScptM_Clases_Activos" runat="server" EnableScriptLocalization="true" EnableScriptGlobalization="true"  AsyncPostBackTimeout="36000" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Actualizar Movimientos</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%;">&nbsp;</td>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>   
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Button ID="Btn_Actualizar_Vehiculos" runat="server" Text="Vehiculos" 
                                onclick="Btn_Actualizar_Vehiculos_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Button ID="Btn_Actualizar_Animales" runat="server" Text="Animales" 
                                onclick="Btn_Actualizar_Animales_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Button ID="Btn_Actualizar_Bienes_Muebles" runat="server" Text="Bienes Muebles" 
                                onclick="Btn_Actualizar_Bienes_Muebles_Click" />
                        </td>
                    </tr>
                </table>
                <hr />
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Button ID="Btn_Actualizar_Obs_Est_Dep" runat="server" Text="Actualizar Observaciones, Estado, Unidad Responsable" 
                                onclick="Btn_Actualizar_Obs_Est_Dep_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Button ID="Btn_Actualizar_Dep_Archivo" runat="server" Text="Actualizar Observaciones, Estado, Unidad Responsable Archivo" 
                                onclick="Btn_Actualizar_Dep_Archivo_Click" />
                        </td>
                    </tr>
                </table>
                <br />     
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

