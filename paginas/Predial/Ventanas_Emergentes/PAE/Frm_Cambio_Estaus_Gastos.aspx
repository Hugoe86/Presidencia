<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cambio_Estaus_Gastos.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Cambio_Estaus_Gastos" Culture="es-MX"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GASTOS DE EJECUCIÓN</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        
        .style3
        {
            width: 148px;
        }
    </style>
</head>
<body>
    <form id="Frm_Motivo_Omision" method="post" runat="server">
    <asp:ScriptManager ID="Script" runat="server" />
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Uprg_Reporte" runat="server"
                    AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0" >
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" 
                                src="../../../imagenes/paginas/Updating.gif" / height="50px" width="50px"></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
                <div id="Div_Contenedor_Msj_Error" style="width:99%;" runat="server" visible="false">
                    <table style="width:99%;">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                            </td>            
                        </tr>       
                    </table>                   
              </div>                          
            <div>    
               <table  style="width:99%;" border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda">
                        <td align="left">
                            <asp:UpdatePanel ID="Upd_Panel_Botones" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                                        AlternateText="Aceptar" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                        onclick="Btn_Aceptar_Click" Width="24px" />
                                    <asp:ImageButton ID="Btn_Regresar" runat="server" 
                                        AlternateText="Regresar" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                        onclick="Btn_Regresar_Click" Width="24px" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                       </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: #6699FF">
                            <asp:Label ID="Lbl_Title" runat="server" Text="Gastos de Ejecucion" Font-Bold="True" 
                                ForeColor="White"></asp:Label>
                        </td>
                    </tr>    
               </table>        
            </div>
            <div>
                <asp:Panel ID="Pnl_Gastos" runat="server" Width="99%" Height="99%">
                    <table style="width:99%;" border="0" cellspacing="0">
                   <%----------------------------------- Notifiaciones---------------------------------------------------------%>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus" />
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus_Etapa" runat="server" Width="99%">
                                    <asp:ListItem Text="&lt;--SELECCIONE--&gt;" Value="0" />
                                    <asp:ListItem Text="NOTIFICACION" Value="1" />
                                    <asp:ListItem Text="NO DILIGENCIADO" Value="2" />
                                    <asp:ListItem Text="ILOCALIZABLE" Value="3" />
                                    <asp:ListItem Text="PENDIENTE" Value="4" />   
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="Lbl_Motivo" runat="server" Text="*Motivo"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Motivo" runat="server" Width="97%" MaxLength="250" TextMode="MultiLine" AutoPostBack="True"
                                 onkeyup='this.value=this.value.toUpperCase();'/>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Motivo" runat="server" TargetControlID="Txt_Motivo"
                                    WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo" runat="server" TargetControlID="Txt_Motivo"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="Lbl_Resolucion" runat="server" Text="Resolución"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Resolucion" runat="server" Width="97%" MaxLength="250" TextMode="MultiLine" AutoPostBack="True"
                                 onkeyup='this.value=this.value.toUpperCase();'/>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Resolucion" runat="server" TargetControlID="Txt_Resolucion"
                                    WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Resolucion" runat="server" TargetControlID="Txt_Resolucion"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>                              
                        
                        <tr>
                            <td colspan="2" style="text-align:right; border-bottom-style:solid;">
                                &nbsp;</td>
                        </tr>
                        <%----------------------------------- Gastos de ejecucion---------------------------------------------------------%>
                        <tr>
                            <td align="left" class="style3">
                                <asp:Label ID="Lbl_Gastos_Eje" runat="server" Text="Gastos de Ejecucion"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Gastos_Ejecucion" runat="server" TabIndex="7" 
                                    Width="99%" AutoPostBack="True" 
                                    onselectedindexchanged="Cmb_Gastos_Ejecucion_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style3">
                                <asp:Label ID="Lbl_Costo" runat="server" Text="Costo"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Costo" runat="server" Width="97%"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Costo" runat="server" TargetControlID="Txt_Costo"
                                FilterType="Numbers,Custom" ValidChars="." />                                       
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:right;">
                                <asp:ImageButton ID="Btn_Agregar_Gasto" runat="server" Height="20px" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" TabIndex="10"
                                ToolTip="Agregar Costo" Width="20px" onclick="Btn_Agregar_Gasto_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3"></td>
                            <td>
                                <asp:GridView ID="Grid_Gastos_Ejecucion" runat="server"
                                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                    HeaderStyle-CssClass="tblHead" PageSize="2" Style="white-space: normal;"
                                    Width="100%" 
                                        onselectedindexchanged="Grid_Gastos_Ejecucion_SelectedIndexChanged" 
                                        onpageindexchanging="Grid_Gastos_Ejecucion_PageIndexChanging" >
                                    <Columns>
                                        <asp:BoundField DataField="GASTO_EJECUCION_ID" HeaderText="Tipo de gasto de ejecución" Visible="false">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_DE_GASTO" HeaderText="Tipo de gasto de ejecución">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IMPORTE" HeaderText="Costo" DataFormatString="{0:C2}">
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Costo" runat="server" Height="20px" 
                                                    ImageUrl="~/paginas/imagenes/paginas/delete.png" TabIndex="10"
                                                    CommandName="Select"
                                                    ToolTip="Eliminar" Width="20px"/>
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="tblHead" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:right; border-bottom-style:solid;">
                                &nbsp;</td>
                        </tr>
                   </table>
                </asp:Panel>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Btn_Regresar" />
                <asp:PostBackTrigger ControlID="Btn_Aceptar" />                
            </Triggers>        
    </asp:UpdatePanel>
    </form>
</body>
</html>