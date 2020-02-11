<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_TUrnos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Turnos" Title="Catalogo de Turnos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 49%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Estados_Predio" runat="server" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Turno" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Catálogo de Turnos</td>
                    </tr>
                    <tr>
                        <td>
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
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td>Busqueda:
                            <asp:TextBox ID="Txt_Busqueda_Turno" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Turno" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Turno_Click" />
                           <%-- <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Turno" runat="server" 
                                WatermarkText="<Descripcion>" TargetControlID="Txt_Busqueda_Turno" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>  
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Turno" runat="server" TargetControlID="Txt_Busqueda_Turno" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>      --%>  
                        </td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Turno_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_ID_Turno" runat="server" Text="Turno ID" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="Txt_ID_Turno" runat="server" Width="150px" MaxLength="5" Enabled="False"></asp:TextBox>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>  
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="150px" MaxLength="100" Enabled="False"></asp:TextBox>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr> 
                        <tr>
                            <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Hora_Inicio" runat="server" Text="Hora Inicio" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="text-align:left;" class="style1">
                            <asp:TextBox ID="Txt_Hora_Inicio" runat="server" Width="150px"  Enabled = "false"/>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server"
                            TargetControlID="Txt_Hora_Inicio" 
                            Mask="99:99"
                            MessageValidatorTip="true"
                            OnFocusCssClass="MaskedEditFocus"
                            OnInvalidCssClass="MaskedEditError"
                            MaskType="Time"
                            AcceptAMPM="True"
                            ErrorTooltipEnabled="True" /></td>
                            <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Hora_Fin" runat="server" Text="Hora Fin" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="text-align:left;" class="style1">
                            <asp:TextBox ID="Txt_Hora_Fin" runat="server" Width="150px"  Enabled = "false"/>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                            TargetControlID="Txt_Hora_Fin" 
                            Mask="99:99"
                            MessageValidatorTip="true"
                            OnFocusCssClass="MaskedEditFocus"
                            OnInvalidCssClass="MaskedEditError"
                            MaskType="Time"
                            AcceptAMPM="True"
                            ErrorTooltipEnabled="True" />
                        </tr>                                   
                        <tr>
                            <td style="width:18%; text-align:left; ">
                               <asp:Label ID="Lbl_Comentarios_Turnos" runat="server" 
                                    Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left; ">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" Width="98%" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />                                
                    <asp:GridView ID="Grid_Turnos" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="96%"
                        GridLines= "None"
                        onselectedindexchanged="Grid_Turnos_SelectedIndexChanged" 
                        onpageindexchanging="Grid_Turnos_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="TURNO_ID" HeaderText="Turno ID" 
                                SortExpression="TURNO_ID" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" />
                            <asp:BoundField DataField="HORA_INICIO" HeaderText="Estado Inicio" 
                                SortExpression="HORA_INICIO" />
                            <asp:BoundField DataField="HORA_FIN" HeaderText="Hora Fin" 
                                SortExpression="HORA_FIN" />     
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                                SortExpression="COMENTARIOS" />
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />                                
                        <AlternatingRowStyle CssClass="GridAltItem" />       
                    </asp:GridView>
                </center>        
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>