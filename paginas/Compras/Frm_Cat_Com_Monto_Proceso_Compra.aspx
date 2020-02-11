<%@ Page Title="Montos para proceso de compra" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Monto_Proceso_Compra.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Monto_Proceso_Compra" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Monto_Proceso_Compra" runat="server" />
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
                        Montos para proceso de compra
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
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
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                         OnClick = "Btn_Modificar_Click"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        OnClick = "Btn_Salir_Click"/>
                                      </td>
                                     </tr>                                                                          
                                   </table>                                    
                           </td>       
                      </tr>         
             </table>                      
          </div>       
          
            <table width="98%">
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>            
                <tr>
                    <td style="width:20%;text-align:left;">
                        Tipo
                    </td>
                    <td style="width:70%;text-align:left;" colspan="3">
                        <asp:DropDownList ID="Cmb_Tipo" runat="server" Enabled="False" Width="100%" AutoPostBack="true"
                            OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        Fondo Fijo Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Fondo_Fijo_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Fondo_Fijo_Ini" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Fondo_Fijo_Inicio" 
                             ValidChars=".,"/>                                          
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Fondo Fijo Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Fondo_Fijo_Fin" runat="server" Width="100%" MaxLength="15" AutoPostBack="true"
                            ontextchanged="Txt_Fondo_Fijo_Fin_TextChanged"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Fondo_Fijo_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Fondo_Fijo_Fin" 
                             ValidChars=".,"/> 
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        Compra Directa Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Com_Directa_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Com_Directa_Ini" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Com_Directa_Inicio" 
                             ValidChars=".,"/> 
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Compra Directa Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Com_Directa_Fin" runat="server" Width="100%" AutoPostBack="true"
                            MaxLength="15" ontextchanged="Txt_Com_Directa_Fin_TextChanged"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Com_Directa_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Com_Directa_Fin" 
                             ValidChars=".,"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        Cotizaci&oacute;n Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Cotizacion_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Cotizacion_Ini" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Cotizacion_Inicio" 
                             ValidChars=".,"/>
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Cotizaci&oacute;n Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Cotizacion_Fin" runat="server" Width="100%" MaxLength="15" AutoPostBack="true"
                            ontextchanged="Txt_Cotizacion_Fin_TextChanged"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTB_Cotizacion_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Cotizacion_Fin" 
                             ValidChars=".,"/> 
                    </td>
                </tr> 
                <tr>
                    <td style="width:20%;text-align:left;">
                        Comit&eacute; Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Comite_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="FTB_Comite_Inicio" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Comite_Inicio" 
                             ValidChars=".,"/> 
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Comit&eacute; Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Comite_Fin" runat="server" Width="100%" MaxLength="15" AutoPostBack="true"
                            ontextchanged="Txt_Comite_Fin_TextChanged"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="FTB_Comite_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Comite_Fin" 
                             ValidChars=".,"/> 
                    </td>
                </tr> 
                <tr>
                    <td style="width:20%;text-align:left;">
                        Licitaci&oacute;n Restringida Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Lic_Restringida_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTB_Lic_Restringida_Ini" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Lic_Restringida_Inicio" 
                             ValidChars=".,"/>  
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Licitaci&oacute;n Restringida Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Lic_Restringida_Fin" runat="server" Width="100%" AutoPostBack="true"
                            MaxLength="15" ontextchanged="Txt_Lic_Restringida_Fin_TextChanged"></asp:TextBox> 
                        <cc1:FilteredTextBoxExtender ID="FTB_Lic_Restringida_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Lic_Restringida_Fin" 
                             ValidChars=".,"/> 
                    </td>
                </tr> 
                <tr>
                    <td style="width:20%;text-align:left;">
                        Licitaci&oacute;n Publica Inicio
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Lic_Publica_Inicio" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="FTB_Lic_Publica_Ini" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Lic_Publica_Inicio" 
                             ValidChars=".,"/> 
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;Licitaci&oacute;n Publica Fin 
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Lic_Publica_Fin" runat="server" Width="100%"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="FTB_Lic_Publica_Fin" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Lic_Publica_Fin" 
                             ValidChars=".,"/> 
                    </td>
                </tr>
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>                
            </table>
    </div>
 </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

