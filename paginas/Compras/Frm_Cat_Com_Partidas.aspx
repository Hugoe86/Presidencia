<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Partidas.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Partidas" Title="Catálogo de Partidas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Cat&aacute;logo de Partidas</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Error" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" /> &nbsp; 
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" 
                                ForeColor="#990000"></asp:Label>                       
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" TabIndex="1" 
                                        onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" 
                                        onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" ToolTip="Eliminar" TabIndex="3" 
                                        
                                        OnClientClick="return confirm('Sólo se cambiará el estatus de la Partida a INACTIVO. ¿Confirma que desea proceder?');" 
                                        onclick="Btn_Eliminar_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" TabIndex="4" onclick="Btn_Salir_Click" />
                                </td>
                                <td align = "right">
                                    Búsqueda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" MaxLength="100"  TabIndex="5"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Impuesto_ID" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Ingrese partida>" TargetControlID="Txt_Busqueda" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="6" 
                                        onclick="Btn_Buscar_Click" />
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
                    
                    <asp:HiddenField ID="Txt_Partida_ID" runat="server" />
                    <tr>
                        <td style="text-align:left;width:20%;">*Capítulo</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Capitulo" AutoPostBack="true" runat="server" 
                                style="width:100%;" TabIndex="7" 
                                onselectedindexchanged="Cmb_Capitulo_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%;text-align:right;">*Concepto</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Concepto" AutoPostBack="true" runat="server" 
                                style="width:100%;" TabIndex="8" 
                                onselectedindexchanged="Cmb_Concepto_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">*Partida genérica</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Partida_Generica" AutoPostBack="true" runat="server" style="width:100%;" 
                                TabIndex="9" 
                                onselectedindexchanged="Cmb_Partida_Generica_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%;">*Clave</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="4" style="width:97%;" TabIndex="10" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave" 
                                runat="server" TargetControlID="Txt_Clave" FilterType="Numbers" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Estatus</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" TabIndex="11" Width="100%">
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%;">
                            &nbsp;</td>
                        <td style="text-align:left;width:30%;">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td>*Nombre</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Partida" runat="server" MaxLength="250" style="width:99%;" TabIndex="12" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Partida" 
                                runat="server" TargetControlID="Txt_Nombre_Partida" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                        </td>
                    </tr>
                    
                    <tr style="display:none;">
                        <td style="text-align:left;width:20%;">*Operación</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Operacion" runat="server"  Width="100%" TabIndex="13">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="CARGA">CARGA</asp:ListItem>
                                <asp:ListItem Value="ABONA">ABONA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%;">*Clave SAP</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave_SAP" runat="server" MaxLength="2" style="width:97%;" TabIndex="14" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Clave_SAP" 
                                runat="server" TargetControlID="Txt_Clave_SAP" FilterType="Numbers, UppercaseLetters, LowercaseLetters" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">*Cuenta</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Cuenta_SAP" runat="server" MaxLength="17" style="width:97%;" TabIndex="15" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_SAP" 
                                runat="server" TargetControlID="Txt_Cuenta_SAP" FilterType="Numbers, UppercaseLetters, LowercaseLetters" />
                        </td>
                        <td style="text-align:right;width:20%; display:none;">*Centro de aplicación</td>
                        <td style="text-align:left;width:30%;display:none;">
                            <asp:DropDownList ID="Cmb_Centro_Aplicacion_SAP" runat="server" Width="100%" TabIndex="16">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="CENTRO GESTOR">Centro gestor</asp:ListItem>
                                <asp:ListItem Value="CENTRO DE COSTO">Centro de costo</asp:ListItem>
                                <asp:ListItem Value="NINGUNO">Ninguno</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr style="display:none;">
                        <td style="text-align:left;width:20%;">*Afecta Área funcional</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Afecta_Area_Funcional" runat="server" Width="100%" TabIndex="17">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%; display:none;">*Afecta Partida</td>
                        <td style="text-align:left;width:30%;display:none;">
                            <asp:DropDownList ID="Cmb_Afecta_Partida" runat="server" Width="100%" TabIndex="18">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td style="text-align:left;width:20%;">*Afecta Elemento PEP</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Afecta_Elemento_PEP" runat="server" Width="100%" TabIndex="19">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%;">*Afecta Fondo</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Afecta_Fondo" runat="server" Width="100%" TabIndex="20">
                                <asp:ListItem>----- < SELECCIONE > -----</asp:ListItem>
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>*Descripción</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" MaxLength="250" Width="99%" TextMode="MultiLine" TabIndex="21"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Descripcion_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Descripcion" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Descripcion_TextBoxWatermarkExtender" runat="server" TargetControlID="Txt_Descripcion" 
                            WatermarkCssClass="watermarked" WatermarkText="<L&iacute;mite de Caracteres 250>" />
                        </td>
                    </tr>
                    <tr>
                        <td>*Descripción Especifica</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion_Especifica" runat="server" MaxLength="250" Width="99%" TextMode="MultiLine" TabIndex="21"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Descripcion_Especifica" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Descripcion_Especifica_TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Descripcion_Especifica" 
                            WatermarkCssClass="watermarked" WatermarkText="<L&iacute;mite de Caracteres 250>" />
                        </td>
                    </tr>
                    
                    <tr><td colspan="4"><hr /></td></tr>
                    
                    <tr align="center">
                        <td colspan="4">
                          <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                            <asp:GridView ID="Grid_Partidas" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" style="white-space:normal" AllowPaging="false" 
                                GridLines="None" Width = "100%" AllowSorting="true"
                                onpageindexchanging="Grid_Partidas_PageIndexChanging" 
                                 onsorting="Grid_Partidas_Sorting" 
                                onselectedindexchanged="Grid_Partidas_SelectedIndexChanged"> 
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="PARTIDA_ID" HeaderText="Partida ID" Visible="false" SortExpression="PARTIDA_ID">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />                                        
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />                                                                                                                
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />                                                                            
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                            </asp:GridView>
                           </div> 
                        </td>
                    </tr>
                </table>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>