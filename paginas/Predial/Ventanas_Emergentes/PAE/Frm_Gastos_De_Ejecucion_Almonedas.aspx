<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Gastos_De_Ejecucion_Almonedas.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Gastos_De_Ejecucion_Almonedas" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GASTOS DE EJECUCIÓN</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
</head>
<body>
    <form id="Frm_Gastos_Alomedas" method="post" runat="server">
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
            
                <div id="Div_Contenedor_Msj_Error" runat="server" visible="false">
                    <table width="524px">
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
               <table width="524px" border="0" cellspacing="0" class="estilo_fuente">
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
                <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="Cmb_Estatus_Ejecucion" runat="server" TabIndex="7" Width="390px" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Estatus_Ejecucion_SelectedIndexChanged">
                <asp:ListItem><-- SELECCIONE --></asp:ListItem>
                <asp:ListItem>FINCADO</asp:ListItem>
                <asp:ListItem>NOTIFICACION</asp:ListItem>
                <asp:ListItem>PUBLICACION</asp:ListItem>
                <asp:ListItem>ADQUIRIDO</asp:ListItem>
                <asp:ListItem>ADJUDICADO</asp:ListItem>
                </asp:DropDownList>
            </div>
                <%----------------------------------- Notifiaciones---------------------------------------------------------%>
                <div id="Div_Notificaciones" runat="server" visible="false">
                    <table width="524px" border="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha" runat="server" AutoPostBack="true" OnTextChanged="Txt_Fecha_TextChanged"/>
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
                                <asp:TextBox ID="Txt_Hora" runat="server" AutoPostBack="true" OnTextChanged="Txt_Hora_TextChanged"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Hora" runat="server" TargetControlID="Txt_Hora"
                                FilterType="Numbers,Custom" ValidChars=":.aAMmPp " />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Notificador" runat="server" Text="Notificador"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Notificador" runat="server" Width="370px" onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Recibio" runat="server" Text="Recibió"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Recibio" runat="server" Width="370px" onkeyup='this.value=this.value.toUpperCase();'/>
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
                    </table>
                </div>
                <%----------------------------------- Gastos de ejecucion---------------------------------------------------------%>
                <div id="Div_Gastos" runat="server" visible="false">
                    <table width="524px" border="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Lbl_Gastos_Eje" runat="server" Text="Gastos Ejecucion" Width="107px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Gastos_Ejecucion" runat="server" TabIndex="7" Width="390px"
                                    AutoPostBack="True" 
                                    OnSelectedIndexChanged="Cmb_Gastos_Ejecucion_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Lbl_Costo" runat="server" Text="Costo"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Costo" runat="server" Width="385px" />
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
                            <td></td>
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
                </div>
            <%----------------------------------- Remates ---------------------------------------------------------%>
                <div id="Div_Remates" runat="server" visible="false">
                    <table width="524px" border="0" cellspacing="0">
                        <tr>                                
                            <td >
                                <asp:Label ID="Lbl_Lugar_Remante" runat="server" Text="Lugar Remante" Width="108px"/>                                 
                            </td>
                            <td colspan="3">                                        
                                <asp:TextBox ID="Txt_Remate" runat="server" Width="385px" onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Fecha_Remate" runat="server" Text="Fecha remate" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Remate" runat="server" AutoPostBack="true" OnTextChanged="Txt_Fecha_Remate_TextChanged" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Remate" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                PopupButtonID="Btn_Fecha_Remate" TargetControlID="Txt_Fecha_Remate" />                                    
                                <asp:ImageButton ID="Btn_Fecha_Remate" runat="server" Height="18px" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Hora_Remante" runat="server" Text="Hora" />
                           </td>
                           <td>
                                <asp:TextBox ID="Txt_Hora_Remante" runat="server" AutoPostBack="true" OnTextChanged="Txt_Hora_Remante_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Hora_Remante" runat="server" TargetControlID="Txt_Hora_Remante"
                                    FilterType="Numbers,Custom" ValidChars=":.aAMmPp " />
                           </td>                                    
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Ini_Fecha" runat="server" Text="Publicar de" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Ini_Fecha" runat="server" AutoPostBack="true" OnTextChanged="Txt_Ini_Fecha_TextChanged"/>
                                <cc1:CalendarExtender ID="CE_Txt_Ini_Fecha" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                PopupButtonID="Btn_Ini_Fecha" TargetControlID="Txt_Ini_Fecha" />                                    
                                <asp:ImageButton ID="Btn_Ini_Fecha" runat="server" Height="18px" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            </td>
                            <td>                                        
                                <asp:Label ID="Lbl_Fin_Fecha" runat="server" Text="a" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fin_Fecha" runat="server" AutoPostBack="true" OnTextChanged="Txt_Fin_Fecha_TextChanged"/>
                                <cc1:CalendarExtender ID="CE_Txt_Fin_Fecha" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                PopupButtonID="Btn_Fin_Fecha" TargetControlID="Txt_Fin_Fecha" />                                    
                                <asp:ImageButton ID="Btn_Fin_Fecha" runat="server" Height="18px" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            </td>
                        </tr>                                                  
                        <tr>
                            <td colspan="4" style="text-align:right; border-bottom-style:solid;">
                            &nbsp;</td>
                        </tr>
                    </table>
                </div>
                        <%----------------------------------- Abonos ---------------------------------------------------------%>
                <div id="Div_Abonos" runat="server" visible="false">
                    <table width="524px" border="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Cuenta" runat="server" Text="Cuenta Predial" />                                 
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                            MaxLength="12" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Cuenta_Predial" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Cuenta_Predial" runat="server"
                                TargetControlID="Txt_Cuenta_Predial" WatermarkText="Proporcione su # de Cuenta Predial"
                                WatermarkCssClass="watermarked" />
                            </td>
                            <td>
                                Tipo de Pago
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo_Pago" runat="server" AutoPostBack="True" 
                                    Width="140px" onselectedindexchanged="Cmb_Tipo_Pago_SelectedIndexChanged" 
                                    >
                                    <asp:ListItem>PAGO TOTAL</asp:ListItem>
                                    <asp:ListItem>PAGO PARCIAL</asp:ListItem>
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Propietario
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Propietario" Width="99%" runat="server" Enabled="false" BorderWidth="1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Periodo_Ini" runat="server" Text="Desde el Periodo" />                                 
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Bimestre_Inicial" runat="server" BorderWidth="1" 
                                    Enabled="false" MaxLength="250" Style="text-align: center;" 
                                    TextMode="SingleLine" />
                            </td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="Txt_Anio_Inicial" runat="server" BorderWidth="1" 
                                    Enabled="false" MaxLength="250" Style="text-align: center;" 
                                    TextMode="SingleLine"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Periodo_fin" runat="server" Text="Hasta el Periodo" />                                 
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Bimestre_Final" runat="server" AutoPostBack="true" 
                                    Enabled="False" Width="99%" Style="text-align: center;" 
                                    OnSelectedIndexChanged="Cmb_Bimestre_Final_SelectedIndexChanged">
                                    <asp:ListItem Value="1">01</asp:ListItem>
                                    <asp:ListItem Value="2">02</asp:ListItem>
                                    <asp:ListItem Value="3">03</asp:ListItem>
                                    <asp:ListItem Value="4">04</asp:ListItem>
                                    <asp:ListItem Value="5">05</asp:ListItem>
                                    <asp:ListItem Value="6">06</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="Cmb_Anio_Final" runat="server" AutoPostBack="true" Style="text-align: center;" 
                                    Enabled="False" Width="152px" OnSelectedIndexChanged="Cmb_Anio_Final_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Periodo Rezago
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" Enabled="false" BorderWidth="1" />
                            </td>
                            <td>
                                Adeudo Rezago
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Enabled="false" BorderWidth="1"
                                    Style="text-align: right;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Periodo Corriente
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Periodo_Actual" runat="server" BorderWidth="1" 
                                    Enabled="false" />
                            </td>
                            <td>
                                Adeudo Corriente
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Enabled="false" BorderWidth="1"
                                    Style="text-align: right;" />
                            </td>
                         </tr>
                         <tr>
                            <td>
                                Total Recargos Ordinarios
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Enabled="false"
                                    BorderWidth="1" Style="text-align: right;" />
                            </td>
                            <td>
                                Honorarios
                            </td>
                            <td >
                                <asp:TextBox ID="Txt_Honorarios" runat="server" Enabled="false" BorderWidth="1"
                                    Style="text-align: right;" />
                            </td>
                        </tr>
                            <tr>
                                <td>
                                    Recargos Moratorios
                                </td>
                                <td >
                                    <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Enabled="false"
                                        BorderWidth="1" Style="text-align: right;" />
                                </td>
                                <td >
                                    Gastos de Ejecución
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Enabled="false"
                                        BorderWidth="1" Style="text-align: right;" />
                                </td>
                            </tr>
                            
                            
                            
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    Subtotal
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_SubTotal" runat="server" Enabled="false" BorderWidth="1"
                                        Style="text-align: right;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    Descuento Pronto Pago
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Descuento_Corriente" runat="server" Enabled="false"
                                        BorderWidth="1" Style="text-align: right;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    Total
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Total" runat="server" Enabled="false" BorderWidth="1" style="text-align:right;"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    Ajuste Tarifario
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Ajuste_Tarifario" runat="server" Enabled="false" BorderWidth="1" style="text-align:right;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <b>Total a Pagar</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Total_Pagar" runat="server" Width="98%" Style="font-weight: bolder;
                                        font-size:large; text-align: right;" ReadOnly="true" BorderWidth="1" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="Btn_Ejecutar_Pago" runat="server" Text="REALIZAR PAGO" Style="color: Black;
                                        font-weight: bolder; text-align: center; font-size: larger;" Width="98%" Height="35px"
                                        OnClick="Btn_Ejecutar_Pago_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Abono" runat="server" Text="Abono al adeudo" Width="106px" Visible="false"/>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Abono" runat="server" AutoPostBack="true" 
                                        OnTextChanged="Txt_Abono_TextChanged" Width="385px"  Visible="false"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Abono" runat="server" 
                                        FilterType="Numbers,Custom" TargetControlID="Txt_Abono" ValidChars="." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Adeudo" runat="server" Text="Adeudo restante"  Visible="false"/>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Adeudo" runat="server" Enabled="false" Width="385px"  Visible="false"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align:right; border-top:solid;">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Pasar_Etapa" runat="server" Text="Pasar a etapa" />
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="Cmb_Pasar_Etapa" runat="server" Width="390px">
                                        <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                        <asp:ListItem>DETERMINACION</asp:ListItem>
                                        <asp:ListItem>EMBARGO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    *Asignar
                                </td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Despachos" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                        onselectedindexchanged="Cmb_Despachos_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Número de entrega
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_No_Entrega" runat="server" Width="96.4%" ReadOnly="True" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Entrega" runat="server" TargetControlID="Txt_No_Entrega"
                                        FilterType="Numbers" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align:right; border-bottom-style:solid;">
                                    &nbsp;</td>
                            </tr>
                    </table>
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    <asp:HiddenField ID="Hfd_No_Descuento_Recargos" runat="server" />
                    <asp:HiddenField ID="Hfd_Tipo_Predio" runat="server" />
                </div>
                <div ID="Div_Listado_Adeudos_Predial" runat="server" style=" overflow: auto; border-width: thick; border-color: Black;">
                                        <asp:GridView ID="Grid_Listado_Adeudos" runat="server" AllowSorting="True" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" 
                                            HeaderStyle-CssClass="tblHead" 
                                            OnRowDataBound="Grid_Listado_Adeudos_RowDataBound" Style="white-space: normal;" 
                                            >
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Seleccion_Adeudo" runat="server" AutoPostBack="True" 
                                                            Checked="true" OnCheckedChanged="Chk_Seleccion_Adeudo_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NO_ADEUDO" HeaderText="NO_ADEUDO">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BIMESTRE" HeaderText="Bim.">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCUENTO" DataFormatString="{0:c}" 
                                                    HeaderText="Desc.">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CORRIENTE" DataFormatString="{0:c}" 
                                                    HeaderText="Corriente">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="REZAGOS" DataFormatString="{0:c}" 
                                                    HeaderText="Rezagos">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c}" 
                                                    HeaderText="Recargos Ordinarios">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c}" 
                                                    HeaderText="Recargos Moratorios">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HONORARIOS" DataFormatString="{0:c}" 
                                                    HeaderText="Honorarios">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MULTAS" DataFormatString="{0:c}" HeaderText="Multas">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCUENTOS" DataFormatString="{0:c}" 
                                                    HeaderText="Descuentos">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" HeaderText="Monto">
                                                    <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="tblHead" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
              <%----------------------------------- Publicaciones ---------------------------------------------------------%>
                <div id="Div_Publicaciones" runat="server" visible="false">
                    <table width="524px" border="0" cellspacing="0">
                        <tr>                                
                            <td>
                                <asp:Label ID="Lbl_Publicacion" runat="server" Text="Publicación" Width="105px" />                                 
                            </td>
                            <td colspan="3">                                        
                                <asp:TextBox ID="Txt_Publicacion" runat="server" Width="385px"
                                    onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Fecha_Publicacion" runat="server" Text="Fecha" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Publicacion" runat="server" AutoPostBack="true" OnTextChanged="Txt_Fecha_Publicacion_TextChanged"/>
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Publicacion" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                PopupButtonID="Btn_Fecha_Publicacion" TargetControlID="Txt_Fecha_Publicacion" />                                    
                                <asp:ImageButton ID="Btn_Fecha_Publicacion" runat="server" Height="18px" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Tomo" runat="server" Text="Tomo" />
                           </td>
                           <td>
                                <asp:TextBox ID="Txt_Tomo" runat="server"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tomo" runat="server" TargetControlID="Txt_Tomo"
                                    FilterType="Numbers,Custom" ValidChars="." />
                           </td>                                    
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Parte" runat="server" Text="Parte" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Parte" runat="server"  onkeyup='this.value=this.value.toUpperCase();' />
                            </td>
                            <td>                                        
                                <asp:Label ID="Lbl_Foja" runat="server" Text="Foja" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Foja" runat="server" onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>                                
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Pagina" runat="server" Text="Pagina" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Pagina" runat="server" onkeyup='this.value=this.value.toUpperCase();' />
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