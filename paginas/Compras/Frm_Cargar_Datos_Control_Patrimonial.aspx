<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cargar_Datos_Control_Patrimonial.aspx.cs" Inherits="paginas_Compras_Frm_Cargar_Datos_Control_Patrimonial" Title="Untitled Page" %>

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
                    <td colspan ="2" class="label_titulo">Cargar Datos a BD de Control Patrimonial</td>
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
                        <asp:Button ID="Btn_Aseguradoras" runat="server" Text="Subir Aseguradoras" 
                            Width="30%" onclick="Btn_Aseguradoras_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Zonas" runat="server" Text="Subir Zonas" 
                            Width="30%" onclick="Btn_Zonas_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Tipos_Vehiculo" runat="server" Text="Subir Tipos Vehiculo" 
                            Width="30%" onclick="Btn_Tipos_Vehiculo_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Tipos_Siniestros" runat="server" Text="Subir Tipos Siniestros" 
                            Width="30%" onclick="Btn_Tipos_Siniestros_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Tipos_Combustibles" runat="server" Text="Subir Tipos Combustibles" 
                            Width="30%" onclick="Btn_Tipos_Combustibles_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Tipos_Animales" runat="server" Text="Subir Tipos Animales" 
                            Width="30%" onclick="Btn_Tipos_Animales_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Tipos_Alimentacion" runat="server" Text="Subir Tipos Alimentación" 
                            Width="30%" onclick="Btn_Tipos_Alimentacion_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Colores" runat="server" Text="Subir Colores" 
                            Width="30%" onclick="Btn_Colores_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Materiales" runat="server" Text="Subir Materiales" 
                            Width="30%" onclick="Btn_Materiales_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Btn_Procedencias" runat="server" Text="Subir Procedencias" 
                            Width="30%" onclick="Btn_Procedencias_Click"/>
                    </td>
                </tr>
         </table>

    </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Content>