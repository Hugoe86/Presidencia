<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master" AutoEventWireup="true" 
CodeFile="Frm_Cat_Ven_Parametros_Correo.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ven_Parametros_Correo" 
Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Tipos" runat="server" />
     <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
              <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>    

        <div id="Div_Requisitos" style="background-color:#ffffff; width:99%; height:100%;">      
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo">
                        Parametros Correo
                    </td>
                </tr>
                <tr>
                    <td runat="server" id="Td_Error">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server"   CssClass="estilo_fuente_mensaje_error"/>
                            <br />
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server"  CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
           </table>          
           
            <table width="100%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td>                
                             <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">  
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                         OnClick = "Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                         OnClick = "Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        OnClick="Btn_Salir_Click" />
                                      </td>
                                      <td align="right" style="width:41%;">&nbsp;</td>
                                     </tr> 
                                  </table>
                                </div>
                         </td>
                     </tr>
            </table>         
            <center>
            <table width="97%">
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td style="width:5%;text-align:left;">
                        Puerto
                    </td>
                    <td style="width:45%;text-align:left;">
                        <asp:TextBox ID="Txt_Puerto" runat="server" MaxLength="6" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:15%; text-align:left;">
                        &nbsp;&nbsp;&nbsp;Servidor 
                    </td>
                    <td style="width:35%;text-align:left;">
                       <asp:TextBox ID="Txt_Servidor" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td style="width:5%; text-align:left;">
                       Correo
                    </td>
                    <td style="text-align:left;">
                       <asp:TextBox ID="Txt_Correo" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:15%; text-align:left;">
                       &nbsp;&nbsp;&nbsp;Password Correo
                    </td>
                    <td style="text-align:left;">
                       <asp:TextBox ID="Txt_Password" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
        </center>
    </div>
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

