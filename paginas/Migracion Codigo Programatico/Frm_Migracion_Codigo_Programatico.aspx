<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Migracion_Codigo_Programatico.aspx.cs" Inherits="paginas_Migracion_Codigo_Programatico_Frm_Migracion_Codigo_Programatico" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptLocalization = "True" AsyncPostBackTimeout="720000" ScriptMode="Release" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
     <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0"></asp:UpdateProgress>
        <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                <tr>
                    <td class="label_titulo" style="width:99%;">Cargar Datos Codigo Programatico</td>
                </tr>
                <tr>
                    <td >
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
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
                    <td style =" text-align:center;">
                       <asp:Panel runat = "server" ID="Pnl_Datos_Generales" GroupingText="Datos Generales" Width="35%">
                         <table width="99%" style="text-align:center;">
                            <tr>
                                <td>
                                    <asp:Button ID="Btn_Fuentes_Financiamiento" runat="server" Text="Fuentes Financiamiento" Width="80%"
                                    OnClientClick="return confirm('¿Está seguro?');" onclick="Btn_Cargar_Fuentes_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="Btn_Grupo_Dependencia" runat="server" Text="Grupo Dependencia" Width="80%"
                                    OnClientClick="return confirm('¿Está seguro?');" onclick="Btn_Cargar_Gpo_Dependencia_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="Btn_Dependencias" runat="server" Text="Dependencias" Width="80%"
                                    OnClientClick="return confirm('¿Está seguro?');" onclick="Btn_Cargar_Dependencia_Click"/>
                                </td>
                            </tr>
                         </table>
                       </asp:Panel>
                    </td>
                </tr>
         </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


