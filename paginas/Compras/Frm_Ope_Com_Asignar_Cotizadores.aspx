<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Asignar_Cotizadores.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Compras_Frm_Ope_Com_Asignar_Cotizadores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
         <%--Div de Contenido --%>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan ="4" class="label_titulo">Asignar Cotizadores</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                            </td>
                        </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
             <%--Fila de Busqueda y Botones Generales --%>
                <tr class="barra_busqueda">
                    <td style="width:20%;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                            ToolTip="Modificar" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                        Busqueda
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"/>
                        </div>
                    </td> 
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="Div_Listado_Cotizaciones" runat="server" style="width:100%;">
                        <asp:GridView ID="Grid_Cotizaciones" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Cotizacion" 
                            GridLines="None" onselectedindexchanged="Grid_Cotizaciones_SelectedIndexChanged"
                            onpageindexchanging="Grid_Cotizaciones_PageIndexChanging" PageSize="10">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="No_Cotizacion" HeaderText="No_Cotizacion" 
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total" 
                                    Visible="True">
                                    <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                    <ItemStyle HorizontalAlign="Right" Width="20%" />
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
                 <%--Fila de Datos Generales de Cotizaciones --%>
                <tr>
                    <td>
                        <div id="Div_Datos_Cotizaciones" runat="server" style="width:100%;">
                        <tabla style="width:98%;">
                            <tr>
                                <td colspan="4" align="center">
                                    Datos Generales
                                </td>
                            </tr>  
                            <tr>
                                <td style="width:15%;">
                                    Folio
                                </td>
                                <td style="width:35%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width:15%;">
                                    Fecha
                                </td>
                                <td style="width:35%;">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Enabled ="false" Width="97%"></asp:TextBox>
                                </td>
                                 <td style="width:15%;">
                                    Estatus
                                </td>
                                <td style="width:35%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Condiciones
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Condiciones" runat="server" TabIndex="10" Enabled="false"     
                                    TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Indica el motivo de realizar la requisición>" TargetControlID="Txt_Condiciones" />
                                </td>                            
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    Total <asp:TextBox ID="Txt_Total" runat="server" Enabled= "false" Width="30%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="barra_delgada">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Conceptos Cotizaci&oacute;n
                                </td>
                                <td >
                                    <asp:DropDownList ID="Cmb_Concepto" runat="server" Width="96%" 
                                        Enabled="false" AutoPostBack="true"
                                        onselectedindexchanged="Cmb_Concepto_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="Btn_Agregar_Cotizador" runat="server" ToolTip="Agregar Cotizador" 
                                    ImageUrl="../imagenes/paginas/icono_nuevo.png" CssClass="Img_Button" 
                                        onclick="Btn_Agregar_Cotizador_Click"></asp:ImageButton>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cotizador
                                </td>
                                <td colspan ="2">
                                    <asp:DropDownList ID="Cmb_Cotizadores" runat="server" Width="98%" Enabled="false"></asp:DropDownList>
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div id="Div_Grid_Concepto_Cotizadores" runat="server">
                                        <center>Cotizadores</center>
                                        <div style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                        visible="False">
                                        <asp:GridView ID="Grid_Concepto_Cotizadores" runat="server" AllowPaging="True" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="Concepto_ID" 
                                        GridLines="None" Width="99%" PageSize="5"
                                        OnSelectedIndexChanged="Grid_Concepto_Cotizadores_SelectedIndexChanged">
                                        <Columns>                       
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                        <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Concepto_ID" HeaderText="Concepto_ID" 
                                                Visible="False">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Clave_Concepto" HeaderText="Clave Concepto">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion_Concepto" HeaderText="Descripcion Concepto">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado_ID" Visible="false">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Empleado" HeaderText="Cotizador">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            
                        </tabla>
                        </div>     
                    </td>
                </tr>
        </table>
        </div>
        
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

