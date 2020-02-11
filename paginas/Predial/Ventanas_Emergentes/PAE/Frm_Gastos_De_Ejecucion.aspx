<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Gastos_De_Ejecucion.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Gastos_De_Ejecucion" Culture="es-MX" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GASTOS DE EJECUCIÓN</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        
.style2
{
    width: 126px;
}
    </style>
</head>
<body>
    <form id="Frm_Motivo_Omision" method="post" runat="server">
    <%--<cc1:ToolkitScriptManager ID="Tsm_Gastos_Ejecucion" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>--%>
    <asp:ScriptManager ID="Tsm_Gastos_Ejecucion" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
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
                    <td colspan="4" align="left">
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
                        <td colspan="4" align="left">
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
                        <td colspan="4" align="center" style="background-color: #6699FF">
                            <asp:Label ID="Lbl_Title" runat="server" Text="Gastos de Ejecucion" Font-Bold="True" 
                                ForeColor="White"></asp:Label>
                        </td>
                    </tr>    
               </table>        
            </div>
            <div>
            <asp:Panel ID="Pnl_Gastos" runat="server" Width="99%" Height="99%">
                <table style="width:99%;" border="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha" />
                         </td>
                         <td>
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="60%" AutoPostBack="true" OnTextChanged="Txt_Fecha_TextChanged"/>
                            <cc1:CalendarExtender ID="CE_Fecha" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                            PopupButtonID="Btn_Fecha" TargetControlID="Txt_Fecha" />                      
                             <asp:ImageButton ID="Btn_Fecha" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                 Style="vertical-align: top;" />
                         </td>
                         <td>
                            <asp:Label ID="Lbl_Hora" runat="server" Text="Hora"></asp:Label>
                         </td>
                         <td>
                            <asp:TextBox ID="Txt_Hora" runat="server" Width="98%" AutoPostBack="true" OnTextChanged="Txt_Hora_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Hora" runat="server" TargetControlID="Txt_Hora"
                            FilterType="Numbers,Custom" ValidChars=":.aAMmPp " />
                         </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Estatus_Ejecucion" runat="server" TabIndex="7" Width="99%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Notificador" runat="server" Text="Notificador"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Notificador" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Recibio" runat="server" Text="Recibió"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Recibio" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                        </td>
                    </tr>                                
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Acuse" runat="server" Text="Acuse de recibo"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Acuse" runat="server" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Acuse" runat="server" TargetControlID="Txt_Acuse"
                            FilterType="Numbers,Custom" ValidChars="." />
                        </td>
                        <td>                                     
                            <asp:Label ID="Lbl_Folio" runat="server" Text="Folio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Folio" runat="server"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Folio" runat="server" TargetControlID="Txt_Folio"
                                FilterType="Numbers,Custom" ValidChars="." />
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4" style="text-align:right; border-bottom-style:solid;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Lbl_Gastos_Eje" runat="server" Text="Gastos de Ejecucion"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Gastos_Ejecucion" runat="server" TabIndex="7" 
                                Width="99%" AutoPostBack="True" 
                                onselectedindexchanged="Cmb_Gastos_Ejecucion_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="Lbl_Costo" runat="server" Text="Costo"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Costo" runat="server" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Costo" runat="server" TargetControlID="Txt_Costo"
                            FilterType="Numbers,Custom" ValidChars="." />                                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:right;">
                            <asp:ImageButton ID="Btn_Agregar_Gasto" runat="server" Height="20px" 
                            ImageUrl="~/paginas/imagenes/paginas/sias_add.png" TabIndex="10"
                            ToolTip="Agregar Costo" Width="20px" onclick="Btn_Agregar_Gasto_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="3">
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
                        <td colspan="4" style="text-align:right; border-bottom-style:solid;">
                            &nbsp;</td>
                    </tr>
                    <tr>                                
                        <td >
                            <asp:Label ID="Lbl_Publicacion" runat="server" Text="Publicación"  />                                 
                        </td>
                        <td colspan="3">                                        
                            <asp:TextBox ID="Txt_Publicacion" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Fecha_Publicacion" runat="server" Text="Fecha" />
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Fecha_Publicacion" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="Txt_Fecha_Publicacion_TextChanged"/>
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Publicacion" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                            PopupButtonID="Btn_Fecha_Publicacion" TargetControlID="Txt_Fecha_Publicacion" />                                    
                            <asp:ImageButton ID="Btn_Fecha_Publicacion" runat="server" Height="18px" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Tomo" runat="server" Text="Tomo" />
                       </td>
                       <td>
                            <asp:TextBox ID="Txt_Tomo" runat="server" Width="98%" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tomo" runat="server" TargetControlID="Txt_Tomo"
                                FilterType="Numbers,Custom" ValidChars="." />
                       </td>                                    
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Parte" runat="server" Text="Parte" />
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Parte" runat="server" Width="100px" onkeyup='this.value=this.value.toUpperCase();' />
                        </td>
                        <td>                                        
                            <asp:Label ID="Lbl_Foja" runat="server" Text="Foja" />
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Foja" runat="server" Width="98%"  onkeyup='this.value=this.value.toUpperCase();'/>
                        </td>
                    </tr>                                
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Pagina" runat="server" Text="Pagina" />
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Pagina" runat="server" Width="100px" onkeyup='this.value=this.value.toUpperCase();' />
                        </td>
                        <td colspan="2" style="text-align:right">
                        <asp:ImageButton ID="Btn_Agregar_Publicacion" runat="server" Height="22px" 
                            ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                onclick="Btn_Agregar_Costo_Publicacion_Click" />
                        </td>
                    </tr>
                    <tr> 
                        <td colspan="4">
                            <asp:GridView ID="Grid_Publicacion" runat="server"
                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                HeaderStyle-CssClass="tblHead" PageSize="2" Style="white-space: normal;"
                                Width="100%" 
                                onselectedindexchanged="Grid_Publicacion_SelectedIndexChanged" 
                                onpageindexchanging="Grid_Publicacion_PageIndexChanging">
                                <Columns>
                                <asp:BoundField DataField="MEDIO_PUBLICACION" HeaderText="Publicación">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_PUBLICACION" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>                                  
                                <asp:BoundField DataField="PARTE" HeaderText="Parte">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOMO" HeaderText="Tomo">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="FOJA" HeaderText="Foja">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="PAGINA" HeaderText="Pagina">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btn_Eliminar_Publicacion" runat="server" Height="20px" 
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