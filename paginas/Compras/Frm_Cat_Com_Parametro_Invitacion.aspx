<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Parametro_Invitacion.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Parametro_Invitacion" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <%-- <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                 <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>--%>
            </ProgressTemplate>
           </asp:UpdateProgress>
           <div id="Div_Contenido" style="width:97%;height:100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td class="label_titulo">Parametro Invitacion a Cotizar</td>
                </tr>
                <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td>
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error"  runat="server" ForeColor="Red" Visible="true"/>
                            </td>
                        </tr> 
                        </table>
                        </div>
                    </td>
                </tr>
                <tr class="barra_busqueda" align="right">
                    <td align="left" valign="middle">
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                        CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" 
                            onclick="Btn_Modificar_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server" 
                        CssClass="Img_Button" 
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                        TabIndex="3" onclick="Btn_Salir_Click"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="98%" class="estilo_fuente">
                            <tr>
                                <td style="width:30%">
                                    Invitacion a Proveedores    
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Texto_Invitacion" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" >
                                    <asp:Label ID="lbl_Nota" ForeColor="red" runat="server" Text="Nota: Este texto es el que aparecera en la invitacion a los proveedores al enviar el correo."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
           </table>
           
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>