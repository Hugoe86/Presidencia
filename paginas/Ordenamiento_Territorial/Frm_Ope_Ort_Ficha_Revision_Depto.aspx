<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Ort_Ficha_Revision_Depto.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Ficha_Revision_Depto" %>

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
            window.open(Url, 'Resumen_Predio', Propiedades);
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
                        <td class="label_titulo" colspan="2">Operacion Ficha de Revisión Departamento</td>
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
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td style="width:50%;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                </center>
            </div>
            <div id="Div_Campos" runat="server" style="width:100%;">
                <center>
                <br />
                <asp:Panel ID="Pnl_Propietario" runat="server" Width="99%" GroupingText="Datos del Propietario">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Ficha_Revision_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Nombre_Propietario" runat="server" Text="Nombre Propietario:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Propietario" runat="server" ToolTip="Seleccionar Propietario"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" 
                                Width="22px" onclick="Btn_Buscar_Propietario_Click" />
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Codigo_Postal" runat="server" Text="Codigo Postal:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Codigo_Postal" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Calle_Ubicacion" runat="server" Text="Calle:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Calle_Ubicacion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Colonia_Ubicacion" runat="server" Text="Colonia:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Colonia_Ubicacion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Ciudad_Ubicacion" runat="server" Text="Ciudad:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Ciudad_Ubicacion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Estado_Ubicacion" runat="server" Text="Estado:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Estado_Ubicacion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="Pnl_Tramite" runat="server" Width="99%" GroupingText="Datos del Tramite">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Solicitud_ID" runat="server" Text="Número de Solicitud:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Solicitud_ID" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Solicitud" runat="server" ToolTip="Seleccionar Solicitud"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" 
                                Width="22px" OnClick="Btn_Buscar_Solicitud_Click" />
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Tipo_Tramite" runat="server" Text="Tipo Tramite:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Tipo_Tramite" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Tramite" runat="server" Text="Tramite:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Tramite" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width:15%;">
                            &nbsp;
                        </td>
                        <td style="width:35%;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="Pnl_Revision" runat="server" Width="99%" GroupingText="Datos de Revisión">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Documentos_Propiedad" runat="server" Text="Documentos Propiedad:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Documentos_Propiedad" Width="87%" runat="server" >
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Documentos_Dictamen" runat="server" Text="Documentos Dictamen:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Documentos_Dictamen" Width="87%" runat="server" >
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Observacion_Juridica" runat="server" Text="Observacion Juridica:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Observacion_Juridica" MaxLength="200" Width="94%" runat="server" 
                                TextMode="MultiLine" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observacion_Juridica" runat="server" 
                                WatermarkText="Limite de 200 Caracteres" TargetControlID="Txt_Observacion_Juridica" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Observacion_Tecnica" runat="server" Text="Observación Técnica:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Observacion_Tecnica" MaxLength="200" Width="94%" runat="server" 
                                TextMode="MultiLine" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observacion_Tecnica" runat="server" 
                                WatermarkText="   Limite de 200 Caracteres  " TargetControlID="Txt_Observacion_Tecnica" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Avance_Obra" runat="server" Text="Avance Obra:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Avance_Obra" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Avance_Obra" runat="server" TargetControlID="Txt_Avance_Obra" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Cumplimiento_Normas" runat="server" Text="Cumplimiento Normas:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Cumplimiento_Normas" Width="87%" runat="server" >
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Perito" runat="server" Text="Perito:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Perito" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Perito" runat="server" ToolTip="Seleccionar Perito"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" 
                                Width="22px" OnClick="Btn_Buscar_Perito_Click" />
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Ubicacion_Construccion" runat="server" Text="Ubicación Construcción:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Ubicacion_Construccion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Inicio_Permiso" runat="server" Text="Inicio Permiso:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Inicio_Permiso" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="Txt_Inicio_Permiso_CalendarExtender" runat="server" 
                                TargetControlID="Txt_Inicio_Permiso" 
                                PopupButtonID="Btn_Inicio_Permiso" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Inicio_Permiso" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                        </td>
                        <td style="width:15%;">
                            <asp:Label ID="Lbl_Fin_Permiso" runat="server" Text="Fin Permiso:"></asp:Label>
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Fin_Permiso" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="Txt_Fin_Permiso_CalendarExtender" runat="server" 
                                TargetControlID="Txt_Fin_Permiso" 
                                PopupButtonID="Btn_Fin_Permiso" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fin_Permiso" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br />
                <asp:GridView ID="Grid_Listado" runat="server" CssClass="GridView_1"
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="10" Width="99%"
                    GridLines= "None" EmptyDataText="No hay Fichas de Revision." OnSelectedIndexChanged="Grid_Listado_SelectedIndexChanged" >
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                            <ItemStyle Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="FICHA_REVISION_ID" HeaderText="Solicitud" SortExpression="FICHA_REVISION_ID" NullDisplayText="-" >
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario" SortExpression="NOMBRE_PROPIETARIO" NullDisplayText="-">
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UBICACION_CONSTRUCCION" HeaderText="Ubicacion" SortExpression="UBICACION_CONSTRUCCION" NullDisplayText="-">
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AVANCE_OBRA" HeaderText="Avance" SortExpression="AVANCE_OBRA" NullDisplayText="-">
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="INICIO_PERMISO" HeaderText="Inicio" SortExpression="INICIO_PERMISO" NullDisplayText="-" DataFormatString="{0:d}">
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FIN_PERMISO" HeaderText="Fin" SortExpression="FIN_PERMISO" NullDisplayText="-" DataFormatString="{0:d}">
                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <PagerStyle CssClass="GridHeader" />
                    <SelectedRowStyle CssClass="GridSelected" />
                    <HeaderStyle CssClass="GridHeader" />                                
                    <AlternatingRowStyle CssClass="GridAltItem" />       
                </asp:GridView>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

