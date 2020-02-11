<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Archivo_Excel.aspx.cs" Inherits="paginas_Compras_Frm_Archivo_Excel" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">

   
   
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>--%>
                    </ProgressTemplate>                     
        </asp:UpdateProgress>
        
        
        <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                <tr>
                    <td colspan ="2" class="label_titulo">Cargar Datos a BD</td>
                </tr>
                <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td colspan ="2">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                </td>            
                                </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                </td>
                            </tr> 
                        </table>                   
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Capitulos" runat="server" Text="Subir Capítulos" 
                            Width="30%" onclick="Btn_Capitulos_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Conceptos" runat="server" Text="Subir Conceptos" 
                            Width="30%" onclick="Btn_Conceptos_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Partidas_Generales" runat="server" Text="Subir Partidas Generales" 
                            Width="30%" onclick="Btn_Partidas_Generales_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_partidas_Especificas" runat="server" Text="Subir Partidas Especificas" 
                            Width="30%" onclick="Btn_partidas_Especificas_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Subir_Proveedores" runat="server" Text="Subir Proveedores" 
                        onclick="Btn_Subir_Proveedores_Click" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Subir_Servicios" runat="server" Text="Subir Servicios" 
                            Width="30%" onclick="Btn_Subir_Servicios_Click"/>
                    </td>
                </tr>
                
                 <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Subir_Modelos" runat="server" Text="Subir Modelos" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar los Modelos?');"                                
                        onclick="Btn_Subir_Modelos_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Subir_Productos" runat="server" Text="Subir Productos" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar los Productos?');"                                
                        onclick="Btn_Subir_Productos_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Subir_Unidades" runat="server" Text="Subir Unidades" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar las Unidades?');"                                                       
                        onclick="Btn_Subir_Unidades_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Dependencias" runat="server" Text="Subir dependencias" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar las Dependencias?');"                                                       
                        onclick="Btn_Dependencias_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Programas" runat="server" Text="Subir Programas" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar los Programas?');" 
                            onclick="Btn_Programas_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Presupuestos" runat="server" Text="Actualizar Presupuestos" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Presupuestos_Click"/>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Cuentas_Contables" runat="server" Text="Subir Cuentas Contables" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Cuentas_Contables_Click"/>
                    </td>
                </tr>      
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Elementos_PEP" runat="server" Text="Actualizar Elemnto PEP" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Elementos_PEP_Click"/>
                    </td>
                </tr>    
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Crear_Areas" runat="server" Text="Crear Areas" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Crear_Areas_Click"/>
                    </td>
                </tr>  
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Menus" runat="server" Text="Actualizar Menús" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Menus_Click"/>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Actualizar_Password" runat="server" Text="Password" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Actualizar_Password_Click"/>
                    </td>
                </tr>        
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Actualizar_Descripcion_Productos" runat="server" Text="Actualizar Descripción Productos" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Actualizar_Descripcion_Productos_Click"/>
                    </td>
                </tr>   
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Ingresos" runat="server" Text="Conceptos de Ingreso" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Ingresos_Click"/>
                    </td>
                </tr>   
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Inventario_inicial" runat="server" Text="Inventario_Inicial" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Inventario_inicial_Click"/>
                    </td>
                </tr>     
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Actualizar_Clave_UR" runat="server" Text="Actualizar Clave UR" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Actualizar_Clave_UR_Click"/>
                    </td>
                </tr>   
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Actualizar_Programas" runat="server" Text="Btn_Actualizar_Programas" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Actualizar_ProgramasX_Click"/>
                    </td>
                </tr>     
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" Text="Btn_Actualizar_Programas >> ur" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Actualizar_Relacion_Programas_UR_Click"/>
                    </td>
                </tr>       
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button2" runat="server" Text="Psp 2012" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Psp_2012_UR_Click"/>
                    </td>
                </tr>      
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Crear_Requisa" runat="server" Text="Nueva requisa" Width="30%"
                        OnClientClick="return confirm('¿Está seguro?');" 
                            onclick="Btn_Nueva_Requisa_Click"/>
                    </td>
                </tr>                                                                                                                                                                                                                                                                 
         </table>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

