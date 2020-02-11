<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Ort_Ficha_Revision.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Ficha_Revision" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Solicitud', Propiedades);
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Ficha_Revision" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Area_Trabajo" style="background-color:#ffffff; width:100%; height:100%;">
                <center>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Catalogo Ficha de Revisión</td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                        <td align="left" style="width:50%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" 
                                ToolTip="Nueva Ficha de Revision" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Modificar"
                                ToolTip="Modificar Ficha de Revision" OnClick="Btn_Modificar_Click" />                           
                            
                            <asp:ImageButton ID="Btn_Reporte" runat="server" CssClass ="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                ToolTip="Generar Reporte" onclick="Btn_Reporte_Click" />
                                
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" /> 
                        </td>
                        <td style="width:50%;"><%--B&uacute;squeda--%>
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" Visible="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Busqueda_Directa" runat="server" Visible="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" AlternateText="Buscar" 
                                ToolTip="Buscar" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkText="<No. Tarjeta>" TargetControlID="Txt_Busqueda" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>                                 
                        </td>                        
                    </tr>
                </table>
                </center>
            </div>
            
            <div id="Div_Campos" runat="server" style="width:100%;">
                <center>
                <br />
                <asp:Panel ID="Pnl_Ficha_Revision" runat="server" Width="99%" GroupingText="Datos de la ficha">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Solicitud_Interna_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Solicitud_Interna_ID" runat="server" Text="No Ficha de Revisión"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Solicitud_Interna_ID" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Fecha_Solicitud" runat="server" Text="Fecha Solicitud"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Fecha_solicitud" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Solicitud_ID" runat="server" Text="Número de Solicitud"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Solicitud_ID" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Solicitud" runat="server" ToolTip="Seleccionar Solicitud"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" 
                                Width="22px" onclick="Btn_Buscar_Solicitud_Click"/>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Fecha_Respuesta" runat="server" Text="Fecha Respuesta"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Fecha_Respuesta" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Zona" runat="server" Text="Zona"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Zona" Width="87%" runat="server" 
                                onselectedindexchanged="Cmb_Zona_SelectedIndexChanged" 
                                AutoPostBack="True" />
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Area" runat="server" Text="Área"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Area" Width="87%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Observacion" runat="server" Text="Observacion"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Observacion" MaxLength="200" Width="94%" runat="server" 
                                TextMode="MultiLine" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observacion" runat="server" 
                                WatermarkText="   Limite de 200 Caracteres  " TargetControlID="Txt_Observacion" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Respuesta" runat="server" Text="Respuesta"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Respuesta" MaxLength="200" Width="94%" runat="server" 
                                TextMode="MultiLine" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Respuesta" runat="server" 
                                WatermarkText="   Limite de 200 Caracteres  " TargetControlID="Txt_Respuesta" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:15%;" colspan="2" align="center">
                            <asp:LinkButton ID="Btn_Link_Datos_Solicitud" runat="server" Text="Datos de la solicitud"
                                OnClick="Btn_Link_Datos_Solicitud_Click" ForeColor="Blue" Visible="false"></asp:LinkButton>
                        </td>
                        <td style="width:15%;" colspan="2" align="center">
                            <asp:LinkButton ID="Btn_Link_Datos_Inspeccion" runat="server" Text="Datos de la inspeccion" ForeColor="Blue"  
                                OnClick="Btn_Link_Datos_Inspeccion_Click"  Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br />
                 <div id="Div_Grid_Listado" runat="server" style="overflow: auto; height: 200px; width: 95%; 
                        vertical-align: top; border-style: solid; border-color: Silver; display: block">
                    <asp:GridView ID="Grid_Listado" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False"  Width="97%"
                        GridLines= "None" EmptyDataText="No hay Fichas de Revision." 
                        OnRowDataBound="Grid_Listado_RowDataBound"
                            onselectedindexchanged="Grid_Listado_SelectedIndexChanged" >
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField  ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                <ItemStyle Width="5%" HorizontalAlign="Center"/>
                            </asp:ButtonField>
                            <asp:BoundField DataField="SOLICITUD_INTERNA_ID" HeaderText="Solicitud" SortExpression="SOLICITUD_INTERNA_ID" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave solicitud" SortExpression="CLAVE_SOLICITUD">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="20%"/>
                                <HeaderStyle Font-Size="X-Small" HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>
                             <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="FOLIO" >
                                 <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="23%"/>
                                <HeaderStyle Font-Size="X-Small" HorizontalAlign="Left" Width="23%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FECHA_SOLICITUD" HeaderText="Fecha de Solicitud" SortExpression="FECHA_SOLICITUD" NullDisplayText="-" DataFormatString="{0:D}">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="25%"/>
                                <HeaderStyle Font-Size="X-Small" HorizontalAlign="Left" Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FECHA_RESPUESTA" HeaderText="Fecha de Respuesta" SortExpression="FECHA_RESPUESTA" NullDisplayText="-" DataFormatString="{0:D}">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="25%"/>
                                <HeaderStyle Font-Size="X-Small" HorizontalAlign="Left" Width="25%" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />                                
                        <AlternatingRowStyle CssClass="GridAltItem" />       
                    </asp:GridView>
                 </div> 
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
