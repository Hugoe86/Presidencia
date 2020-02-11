<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Cat_Parametros.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Cat_Parametros" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <asp:UpdatePanel ID="Upd_Panel_Parametros" runat="server">    
     <ContentTemplate>
        <%-- Configuracion del Panel  --%>
        <asp:UpdateProgress ID="Upr_Parametros" runat="server" AssociatedUpdatePanelID="Upd_Panel_Parametros" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressTemplate"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <%-- Tabla principal con los componentes --%>
        <div id="Div_Apl_Cat_Parametros" style="background-color:#ffffff; width:97%;">
        <table width = "100%" class="estilo_fuente" border="0" cellspacing="0">
            <tr>
                <td colspan = "4" class ="label_titulo">
                    Parametros
                </td>
            </tr>
            <tr>
                <td colspan = "4" >
                    <%--Contenedor del Mensaje de Error--%> 
                    <div id="Div_Contenedor_Msj_Error" style="width:100%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                </td>            
                            </tr>
                            <tr>
                                <td style="width:10%;">              
                                </td>            
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                </td>
                            </tr>          
                        </table>                   
                    <br />
                    </div>
                </td>
            </tr>
            
            <%--Manejo de la barra de busqueda--%>
            <tr class="barra_busqueda">
                <td align = "left" style=" width:20%" >
                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                        onclick="Btn_Modificar_Click"/>
                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                        onclick="Btn_Salir_Click"/>
                </td>
                    
                <td colspan="3" align = "right">&nbsp;</td> 
            </tr>
        
            <tr>
                <td colspan = "4">
                </td>
            </tr> 
            <tr>
                <td>
                    Correo Saliente
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="Txt_Correo_Saliente" runat="server" Width="98%"></asp:TextBox>                        
                </td>
                
            </tr>
            <tr>
                <td>
                    Servidor de Correo
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="Txt_Servidor_Correo" runat="server" Width="98%"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                            TargetControlID="Txt_Servidor_Correo" ValidChars="1234567890.">
                        </cc1:FilteredTextBoxExtender>
                    
                </td>
                
            </tr>
            <tr>
                <td>
                    Usuario de Correo
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="Txt_Usuario_Correo" runat="server" Width="98%"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td>
                    Password de Correo
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="Txt_Password_Correo" runat="server" Width="98%" TextMode="Password"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Confirmar Password
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="Txt_Confirmar_Password" runat="server" Width="98%" TextMode="Password"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td colspan="4">
                
                </td>
            </tr>
            
            <tr>
                <td colspan="4" align = "center" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan = "4">
                </td>
            </tr>
        </table>
        </div>
     </ContentTemplate> 
     </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
</asp:Content>
