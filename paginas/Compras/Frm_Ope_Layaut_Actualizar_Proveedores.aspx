<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"  CodeFile="Frm_Ope_Layaut_Actualizar_Proveedores.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Layaut_Actualizar_Proveedores" %>

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
                        <asp:Button ID="Btn_Actualizar_Proveedores" runat="server" 
                            Text="Actualizar Proveedores" Width="30%"
                        OnClientClick="return confirm('¿Está seguro de guardar los Productos?');" onclick="Btn_Actualizar_Proveedores_Click"                                
                       />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="Grid_Datos" runat="server" Width="99%">
                        </asp:GridView>
                    </td>
                
                </tr>
         </table>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>