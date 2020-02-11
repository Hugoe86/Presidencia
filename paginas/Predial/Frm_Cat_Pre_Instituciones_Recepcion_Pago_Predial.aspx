<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Instituciones_Recepcion_Pago_Predial.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Instituciones_Recepcion_Pago_Predial" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css" >
.Tabla_Comentarios 
{
    border-collapse:collapse;
    margin-left:25px;
    color: #25406D;
    font-family: Verdana,Geneva,MS Sans Serif;
    font-size: small;
    text-align: left;
}
.Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
{
    border: 1px solid #999999;
    padding: 2px 10px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" 
                AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress> 

                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Instituciones con Recepción de Pago Predial
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                <tr class="barra_busqueda" align="right">
                    <td colspan="2">                
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server"  
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" 
                                            onclick="Btn_Eliminar_Click"
                                            OnClientClick="return confirm('Sólo se cambiará el estatus a BAJA. ¿Confirma que desea proceder?');" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        B&uacute;squeda:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" TabIndex="5" MaxLength="100" Width="180"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="6" 
                                            onclick="Btn_Buscar_Institucion_Click" />
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
                </table> 
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Institución
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Institucion" runat="server" Width="96.4%" 
                            TabIndex="7" MaxLength="50" style="text-transform:uppercase;" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="8">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Caja
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Caja" runat="server" Width="99%" TabIndex="9">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Convenio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Convenio" runat="server" Width="96.4%" TabIndex="10" MaxLength="20"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Convenio" runat="server" 
                                TargetControlID="Txt_Convenio" 
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers"/>
                            <asp:TextBox ID="Txt_Id" runat="server" Width="96.4%" Visible="false"/>
                        </td>
                    </tr>
                    <%------------------ Líneas de captura ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Líneas de captura
                        </td>
                    </tr>
                    <%--<tr>--%>
                       <%-- <td style="text-align:left;width:20%;">
                            Mes
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Mes" runat="server" Width="99%" TabIndex="11">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>--%>
                        <%--<td colspan="2">
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Líneas Captura Enero
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Lineas_Captura_Enero" runat="server" Width="99%" TabIndex="11">
                                <asp:ListItem Text="<SELECCIONE>" Value = "SELECCIONE"></asp:ListItem>
                                  <asp:ListItem Text="BAJIO" Value="BAJIO" />
                                 <asp:ListItem Text="BANAMEX" Value="BANAMEX" />
                                  <asp:ListItem Text="BANCOMER" Value="BANCOMER" />
                                  <asp:ListItem Text="BANORTE" Value="BANORTE" />
                                  <asp:ListItem Text="HSBC" Value="HSBC" />
                                  <asp:ListItem Text="OXXO / SUPERBARA" Value="OXXO / SUPERBARA" />
                                  <asp:ListItem Text="SANTANDER" Value="SANTANDER" />
                                  <asp:ListItem Text="SCOTIABANK" Value="SCOTIABANK" />
                             
                            </asp:DropDownList>
                        </td>
                         <td style="text-align:right;width:20%;">
                            Líneas Captura Febrero
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Lineas_Captura_Febrero" runat="server" Width="99%" TabIndex="12">
                                <asp:ListItem Text="<SELECCIONE>" Value = "SELECCIONE"></asp:ListItem>
                                  <asp:ListItem Text="BAJIO" Value="BAJIO" />
                                 <asp:ListItem Text="BANAMEX" Value="BANAMEX" />
                                  <asp:ListItem Text="BANCOMER" Value="BANCOMER" />
                                  <asp:ListItem Text="BANORTE" Value="BANORTE" />
                                  <asp:ListItem Text="HSBC" Value="HSBC" />
                                  <asp:ListItem Text="OXXO / SUPERBARA" Value="OXXO / SUPERBARA" />
                                  <asp:ListItem Text="SANTANDER" Value="SANTANDER" />
                                  <asp:ListItem Text="SCOTIABANK" Value="SCOTIABANK" />
                                 
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    
                    
                    <%--<tr>
                        <td style="text-align:left;width:20%;">
                            Texto
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Texto" runat="server" Width="96.4%" TabIndex="10"/>
                        </td>
                        <td colspan="2">
                            <asp:ImageButton ID="Img_Agregar_Texto" runat="server" 
                                ToolTip="Agregar" 
                                TabIndex="11" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                Height="22px" Width="22px" style="float:left" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Campos
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Campos" runat="server" Width="99%" TabIndex="12">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" style="width:20%;">
                            <asp:ImageButton ID="Img_Agregar_Campos" runat="server" 
                                ToolTip="Agregar" 
                                TabIndex="13" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                Height="22px" Width="22px" style="float:left" />
                        </td>
                    </tr>--%>
                    
                </table>
                <br />
                <%--
                border = "1" class = "Tabla_Comentarios"--%>
                <table width="98%" cellspacing="0" >
                    <%--<tr>
                        <td style="width:25%;">
                            Mes
                        </td>
                        <td>Campos que conforman la línea de captura </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr><td></td></tr>--%>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Instituciones" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                style="white-space:normal;" 
                                onpageindexchanging="Grid_Instituciones_PageIndexChanging" 
                                onselectedindexchanged="Grid_Instituciones_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="INSTITUCION_ID" HeaderText="Id" HeaderStyle-Width="0%" visible="false"/>
                                    <asp:BoundField DataField="INSTITUCION" HeaderText="Institución" HeaderStyle-Width="20%" />
                                    <asp:BoundField DataField="CAJA_ID" HeaderText="Caja" />
                                    <asp:BoundField DataField="CLAVE" HeaderText="Caja" HeaderStyle-Width="15%"/>
                                    <asp:BoundField DataField="LINEA_CAPTURA_ENERO" HeaderText="Línea Captura Enero" HeaderStyle-Width="20%"/>
                                    <asp:BoundField DataField="LINEA_CAPTURA_FEBRERO" HeaderText="Línea Captura Febrero" HeaderStyle-Width="20%"  />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="15%" />
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
                <%--<asp:ImageButton ID="Btn_Agregar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                    CssClass="Img_Button" ToolTip="Agregar datos de trámite" TabIndex="15" style="float:right;margin:5px 10px 0 0;" />--%>
                <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>

