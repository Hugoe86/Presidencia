﻿<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cierre_Definitivo_Dia.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cierre_Definitivo_Dia" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <%--<ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>--%>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Cierre definitivo de día
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" AlternateText ="Nuevo" 
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" />
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" 
                                            CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                            onclick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                    
                <br />
                    
                <table width="98%" class="estilo_fuente">
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Cierres de día
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Cierres_Dia" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" style="white-space:normal;">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="c1" HeaderText="Módulo" />
                                    <asp:BoundField DataField="c1" HeaderText="No. Cierre" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="c1" HeaderText="Fecha" />
                                    <asp:BoundField DataField="c1" HeaderText="Estatus" />
                                    <asp:BoundField DataField="c1" HeaderText="Realizó" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Fecha cierre
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Cierre" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
                                <asp:ListItem Text="CERRADO" Value="CERRADO" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr><td colspan="4">&nbsp;</td></tr>
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Movimientos por caja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Módulo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Modulo" runat="server" Width="99%" TabIndex="7" 
                                AutoPostBack ="true"  onselectedindexchanged="Cmb_Modulo_SelectedIndexChanged">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Caja
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Caja" runat="server" Width="99%" TabIndex="7" 
                                onselectedindexchanged="Cmb_Caja_SelectedIndexChanged">
                                <%--<asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Cajero
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Cajero" runat="server" Width="96.4%" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Movimientos_Caja" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" style="white-space:normal;">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="c1" HeaderText="No. Operación" />
                                    <asp:BoundField DataField="c3" HeaderText="Cuenta predial" />
                                    <asp:BoundField DataField="c2" HeaderText="Folio" />
                                    <asp:BoundField DataField="c3" HeaderText="Tipo operación" />
                                    <asp:BoundField DataField="c3" HeaderText="Monto" />
                                    <asp:BoundField DataField="c2" HeaderText="Fecha" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

