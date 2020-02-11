<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Movimiento_Presupuestal.aspx.cs" Inherits="paginas_presupuestos_Frm_Ope_Movimiento_Presupuestal" Title="Catalogo de Solicitud de Movimiento Presupuestal"%>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<script runat="server">

   
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" 
        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
    </asp:UpdateProgress>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <div id="Div_Movimiento_Presupuestal" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Solicitud de Movimiento Presupuestal</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                
                                OnClientClick="return confirm('¿Está seguro de eliminar el movimiento Presupuestal seleccionado?');" 
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Movimiento_Presupuestal" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Descripción"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Movimiento_Presupuestal" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Descripción>" TargetControlID="Txt_Busqueda_Movimiento_Presupuestal" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Movimiento_Presupuestal" runat="server" 
                                TargetControlID="Txt_Busqueda_Movimiento_Presupuestal" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ-. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Movimiento_Presupuestal" runat="server" 
                                ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Movimiento_Presupuestal_Click"/>
                        </td> 
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="8">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="16%">Codigo Programatico</td>
                        <td width="19%"></td>
                        <td width="10%"></td>
                        <td width="5%"></td>
                        <td width="16%">Codigo Programatico</td>
                        <td width="19%"></td>
                        <td width="10%"></td>
                        <td width="5%"></td>
                        
                     </tr>
                    <tr>
                        <td colspan="3">
                                <asp:TextBox ID="Txt_Codigo_Programatico_De"  runat="server" TabIndex="6" AutoPostBack="true"
                                Width="98%" ontextchanged="Txt_Codigo_Programatico_De_TextChanged" MaxLength="29"></asp:TextBox>
                        </td>
                        
                        <td align="center">Al</td>
                        <td colspan="3" >
                            <asp:TextBox ID="Txt_Codigo_Programatico_Al" runat="server" Width="98%" AutoPostBack="true" 
                                TabIndex="7" MaxLength="29" 
                                ontextchanged="Txt_Codigo_Programatico_Al_TextChanged"></asp:TextBox>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td>Fuente Financiamento</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Fuente_Financiamiento_De" runat="server" Width="98%" 
                                TabIndex="8" ReadOnly="true" ></asp:TextBox>                  
                        </td>
                        <td>Fuente Financiamiento</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Fuente_Financiamiento_Al" runat="server" Width="98%"
                                TabIndex="9" ReadOnly=true></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>Area Funcional</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Area_Funcional_De" runat="server" ReadOnly=true Width="98%" 
                                TabIndex="10"></asp:TextBox>                  
                        </td>
                        <td>Area Funcional</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Area_Funcional_Al" runat="server" Width="98%" ReadOnly=true
                                TabIndex="11"></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>Programa</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Programa_De" runat="server" ReadOnly=true Width="98%" TabIndex="12"></asp:TextBox>                  
                        </td>
                        <td>Programa</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Programa_Al" runat="server" Width="98%" TabIndex="13" ReadOnly=true></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>Unidad Responsable</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Unidad_Responsable_De" runat="server" ReadOnly=true Width="98%"
                                TabIndex="14"></asp:TextBox>                  
                        </td >
                        <td>Unidad Responsable</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Unidad_Responsable_Al" runat="server" Width="98%" ReadOnly=true
                                TabIndex="15"></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>Partida</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Partida_De" runat="server" ReadOnly=true TextMode="MultiLine" Width="98%" TabIndex="16"></asp:TextBox>                  
                        </td>
                        <td>Partida</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Partida_Al" runat="server" TextMode="MultiLine" Width="98%" TabIndex="17" ReadOnly=true></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>Monto a Traspasar</td>
                        <td colspan="4">
                            <asp:TextBox ID="Txt_Monto_Traspaso" runat="server" Width="98%"
                                TabIndex="18"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Justificación</td>
                        <td colspan="7">
                            <asp:TextBox ID="Txt_Justificacion" runat="server" TabIndex="19"
                                TextMode="MultiLine" Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Justificacion" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Justificacion" WatermarkText="Límite de Caractes 250" >
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Justificacion" runat="server" 
                                TargetControlID="Txt_Justificacion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr >
                        <td colspan="8">
                            <hr size="2" width="98%"/>
                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:DropDownList ID="Cmb_Tipo_Estatus" runat="server"  Width="98%" TabIndex="20" AutoPostBack="true" 
                                onselectedindexchanged="Cmb_Tipo_Estatus_Movimiento_SelectedIndexChanged">
                                <asp:ListItem>&lt; SELECCIONE ESTATUS &gt;</asp:ListItem>
                                <asp:ListItem>TODOS</asp:ListItem>
                                <asp:ListItem>GENERADA</asp:ListItem>
                                <asp:ListItem>AUTORIZADA</asp:ListItem>
                            </asp:DropDownList>  
                        </td>
                        <td colspan="2" >
                        </td>
                        <td colspan="1" >Número de solicitud
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="Txt_Numero_Solicitud" runat="server" ReadOnly="true" Width="100%" > </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td colspan="8">
                              
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="8">
                            <asp:GridView ID="Grid_Movimiento_Presupuestal" runat="server"  CssClass="GridView_1" Width="98%" 
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="5" 
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                onselectedindexchanged="Grid_Movimiento_Presupuestal_SelectedIndexChanged" 
                                onsorting="Grid_Movimiento_Presupuestal_Sorting" 
                                onpageindexchanging="Grid_Movimiento_Presupuestal_PageIndexChanging" >
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_SOLICITUD" HeaderText="Numero de solicitud" Visible="True" SortExpression="NO_SOLICITUD">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CODIGO1" HeaderText="Partida Origen" Visible="True" SortExpression="CODIGO1">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CODIGO2" HeaderText="Partida Destino" Visible="True" SortExpression="CODIGO2">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IMPORTE" HeaderText="Importe" Visible="True" SortExpression="IMPORTE">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS" >
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        
                        
                        </td>
                    
                    </tr>
                </table>
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>  