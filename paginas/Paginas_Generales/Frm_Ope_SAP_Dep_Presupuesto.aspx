<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_SAP_Dep_Presupuesto.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_SAP_Dep_Presupuesto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<style type="text/css">
.GridView_1 td, .GridView_1 th{
    border-width:1px;
    border-style:solid;
    padding-left:3px;
}
.GridItem td img{
    width:18px;
}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Presupuesto" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
<%--            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
                
            <div id="Div_Presupuestos" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Asignaci&oacute;n de Presupuestos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" 
                            CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div style="width:99%; background-color:#2F4E7D; color:#FFFFFF; font-weight:bold; font-style:normal; font-variant:normal; font-family:fantasy; height:32px; text-align:right;"  >                        
                                      <table style="width:100%;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                    CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                    onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                    CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                    onclick="Btn_Salir_Click" />
                                                &nbsp;
                                                <asp:ImageButton ID="Btn_Importar_Archivo_Excel" runat="server" ToolTip="Importar archivo de Excel" 
                                                    CssClass="Img_Button" TabIndex="5"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_xls.png" />
                                                <cc1:ModalPopupExtender ID="Mpe_Cargar_Archivo" runat="server" 
                                                    TargetControlID="Btn_Importar_Archivo_Excel"
                                                    PopupControlID="Pnl_Contenedor_Cargar_Archivo" 
                                                    BackgroundCssClass="popUpStyle" 
                                                    DropShadow="true" 
                                                    DynamicServicePath="" 
                                                    Enabled="True" />
                                                <asp:Button ID="Btn_Confirmar_Editar_Presupuestos" runat="server" 
                                                    Visible="true" Style="display:none; visibility:hidden;" />
                                                <cc1:ModalPopupExtender ID="Mpe_Pnl_Contenedor_Editar_Presupuesto" runat="server" 
                                                    TargetControlID="Btn_Confirmar_Editar_Presupuestos"
                                                    PopupControlID="Pnl_Contenedor_Editar_Presupuesto" 
                                                    BackgroundCssClass="popUpStyle" 
                                                    DropShadow="true" 
                                                    DynamicServicePath="" 
                                                    Enabled="True" />
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" MaxLength="100"  TabIndex="6"></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Impuesto_ID" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="< Buscar Partida >" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="7" 
                                                            onclick="Btn_Buscar_Click" />
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
                
                            <asp:HiddenField ID="Txt_Presupuesto_ID" runat="server" />
                            <asp:HiddenField ID="Txt_Monto_Presupuestal_Anterior" runat="server" />
                            <asp:HiddenField ID="Txt_Monto_Disponible_Anterior" runat="server" />
                <table width="98%" class="estilo_fuente">
                                                            
                    <tr>
                        <td style="text-align:left;width:20%">
                            *Unidad Responsable
                        </td>
                        <td style="text-align:left;width:30%">
                            <asp:DropDownList ID="Cmb_Dependencia" AutoPostBack="true" runat="server" style="width:100%;" 
                                TabIndex="8" onselectedindexchanged="Cmb_Dependencia_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%;text-align:right;">
                            *Fuente Financiamiento
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" 
                                style="width:100%;" TabIndex="9" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Programa
                        </td>
                        <td style="text-align:left;width:30%;" colspan="3" >
                            <asp:DropDownList ID="Cmb_Programa" AutoPostBack="true" runat="server" 
                                style="width:100%;" TabIndex="10" 
                                onselectedindexchanged="Cmb_Programa_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">
                            *Partida
                        </td>
                        <td style="text-align:left;width:30%;" colspan="3" >
                            <asp:DropDownList ID="Cmb_Partida" AutoPostBack="true" runat="server" 
                                style="width:100%;" TabIndex="11" onselectedindexchanged="Cmb_Partida_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr style="display:none;">
                        <td style="text-align:left;width:20%;">*A&ntilde;o</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" style="width:97.5%;" AutoPostBack="true"
                                TabIndex="12" Visible="false" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" 
                                runat="server" TargetControlID="Txt_Anio" FilterType="Numbers" />
                        </td>
                        <td style="text-align:right;width:20%;">Asignaci&oacute;n por a&ntilde;o</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Numero_Asignacion" runat="server" Enabled="false" MaxLength="10" style="width:97.5%;" 
                                TabIndex="13" Visible="false" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Asignacion" 
                                runat="server" TargetControlID="Txt_Numero_Asignacion" FilterType="Numbers" />
                        </td>
                    </tr>
                    
                    <tr><td colspan="4">
                        <asp:Label ID="Lbl_Presupuesto_Partida" runat="server" Font-Bold="True"></asp:Label>
                    </td></tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">*Monto Presupuestal</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Monto_Presupuestal" runat="server" TabIndex="14" MaxLength="15" style="width:97.5%;" 
                                AutoPostBack="true" ontextchanged="Txt_Monto_Presupuestal_TextChanged" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monto_Presupuestal" 
                                runat="server" TargetControlID="Txt_Monto_Presupuestal" FilterType="Custom, Numbers" ValidChars="." />
                        </td>
                        <td style="text-align:right;width:20%;">*Ejercido</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ejercido" runat="server" Enabled="false" MaxLength="15" 
                                style="width:97.5%;" TabIndex="15" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ejercido" 
                                runat="server" TargetControlID="Txt_Ejercido" FilterType="Custom, Numbers" ValidChars="." />
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">*Disponible</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Disponible" ReadOnly="true" Enabled="false" runat="server" MaxLength="15" style="width:97.5%;" TabIndex="16" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Disponible" 
                                runat="server" TargetControlID="Txt_Disponible" FilterType="Custom, Numbers" ValidChars="." />
                        </td>
                        <td style="text-align:right;width:20%;">*Comprometido</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Comprometido" ReadOnly="true" Enabled="false" runat="server" MaxLength="15" style="width:97.5%;" TabIndex="17" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comprometido" 
                                runat="server" TargetControlID="Txt_Comprometido" FilterType="Custom, Numbers" ValidChars="." />
                        </td>
                    </tr>
                    
                                                            <!-- Campo Comentarios de Presupuesto -->
                    <tr>
                        <td style="text-align:left;vertical-align:top;width:20%;">Comentarios</td>
                        <td style="text-align:left;width:30%;" colspan="3" >
                            <asp:TextBox ID="Txt_Comentarios" runat="server" MaxLength="250" 
                                style="width:99%;" TabIndex="18" Rows="2" TextMode="MultiLine" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" 
                                runat="server" TargetControlID="Txt_Comentarios" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                        </td>
                    </tr>
                </table>
                
                <br />
                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Presupuestos" runat="server" AllowPaging="True" style="white-space:normal" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="100%" 
                                onpageindexchanging="Grid_Presupuestos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Presupuestos_SelectedIndexChanged" >
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Presupuesto_ID" HeaderText="Presupuesto ID" >
                                        <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Clave_Y_Nombre" HeaderText="Partida" >
                                        <HeaderStyle HorizontalAlign="Left" Width="65%" />
                                        <ItemStyle HorizontalAlign="Left" Width="65%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO_DISPONIBLE" HeaderText="Disponible" 
                                        DataFormatString="{0:c}" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            
            
    <%------------------------------ Cargar archivo de Excel modal ------------------------------%>
    
            <asp:Panel ID="Pnl_Contenedor_Cargar_Archivo" runat="server" 
                    CssClass="drag" 
                    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;max-width:850px;">
                    <asp:Panel ID="Pnl_Cargar_Archivo" runat="server" 
                        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                        <table style="width:829px;">
                            <tr>
                                <td style="color:Black; text-align:center; font-size:12; font-weight:bold; vertical-align:middle;">
                                    <asp:Image ID="Img_Icono_Cargar_Archivo" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_upload.png" />
                                    Carga de presupuestos desde archivo Excel
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Contenedor_Datos" runat="server" style="max-height:450px; width:850px; overflow:scroll;" >
                    <table>
                        <tr>
                            <td colspan="2" style="width:829px;">
                                <br />
                                <asp:FileUpload ID="Fle_Cargar_Archivo" runat="server" 
                                    size="92" TabIndex="1" />
                                <asp:HiddenField ID="Hdn_Ruta_Archivo" runat="server" />
                                <asp:Button ID="Btn_Enviar_Archivo" runat="server" 
                                    Text = "Enviar" TabIndex="2"
                                    OnClick = "Btn_Enviar_Archivo_Click" />
                                <asp:Button ID="Btn_Cancelar_Sincronizacion" runat="server" 
                                    Text = "Cancelar" TabIndex="3"
                                    OnClick = "Btn_Cancelar_Sincronizacion_Click" />
                                <asp:Label ID="Contenedor_Roller" runat="server" style="display:none;" >
                                    <img align="absmiddle" alt="" src="../imagenes/paginas/Sias_Roler.gif" />
                                </asp:Label>
                                <div id="Contenedor_Mensajes_Modal" style="display:block;">
                                    <asp:Image ID="Img_Error_Modal" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                                        Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error_Modal" runat="server" Text="Mensaje" Visible="false" 
                                        CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        
                        <tr><td><hr /></td></tr>
                        
                        <tr id="Fila_Presupuestos_Alta" runat="server" style="display:none;" ><td>Presupuestos que se van a dar de alta</td></tr>
                        <tr>
                            <td>
                                <asp:GridView ID="Grid_Datos_Archivo" runat="server" 
                                    AutoGenerateColumns="false" 
                                    CssClass="GridView_1" style="white-space:normal" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="No." >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FUENTE_FINACIAMIENTO" HeaderText="Fuente de Financiamiento" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AREA_FUNCIONAL" HeaderText="Área Funcional" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD_RESPONSABLE" HeaderText="Unidad Responsable"  >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PARTIDA" HeaderText="Partida" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRESUPUESTO" HeaderText="Presupuesto" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comprometido" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EJERCIDO" HeaderText="Ejercido" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:ImageField DataImageUrlField="ESTATUS" HeaderText="" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:ImageField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        
                        <tr id="Fila_Presupuestos_Actualizar" runat="server" style="display:none;" ><td>Presupuestos que se van a actualizar:</td></tr>
                        
                        <tr>
                            <td>
                                <asp:GridView ID="Grid_Datos_Archivo_Modificar" runat="server" 
                                    AutoGenerateColumns="false" 
                                    CssClass="GridView_1" style="white-space:normal" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="No." >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FUENTE_FINACIAMIENTO" HeaderText="Fuente de Financiamiento" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AREA_FUNCIONAL" HeaderText="Área Funcional" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD_RESPONSABLE" HeaderText="Unidad Responsable"  >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PARTIDA" HeaderText="Partida" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRESUPUESTO" HeaderText="Presupuesto" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comprometido" >
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EJERCIDO" HeaderText="Ejercido" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:ImageField DataImageUrlField="ESTATUS" HeaderText="" >
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:ImageField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    
                    <br />
                    <asp:Label ID="Lbl_Comentario_Modal" runat ="server"
                        Text="*Comentario" Visible="false" ></asp:Label>
                    <br />
                    <asp:TextBox ID="Txt_Comentario_Modal" runat="server"
                        MaxLength="250"  Visible="false"
                        style="width:99%;" TabIndex="4" Rows="2" TextMode="MultiLine" />
                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Comentario_Modal" 
                        runat="server" TargetControlID="Txt_Comentario_Modal" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                    <br />
                    <asp:Label ID="Lbl_Resumen_Carga_Archivo" runat="server" ></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; 
                    <asp:Button ID="Btn_Sincronizar_Presupuestos" runat="server"
                        ToolTip="Sincronizar"
                        Text="Sincronizar"
                        Visible="false"
                        TabIndex="5"
                        onclick="Btn_Sincronizar_Presupuestos_Click" />
                    </asp:Panel>
                </asp:Panel>
                  <%------------------------------ Panel confirmar editar presupuesto existente ------------------------------%>
        
                <asp:Panel ID="Pnl_Contenedor_Editar_Presupuesto" runat="server" 
                    CssClass="drag" 
                    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;max-width:850px;">
                    <asp:Panel ID="Panel2" runat="server" 
                        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                        <table style="width:400px;">
                            <tr>
                                <td style="color:Black; text-align:center; font-size:12; font-weight:bold; vertical-align:middle;">
                                    Presupuesto existente
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Confirmar_Editar_Presupuesto" runat="server" style="width:400px;" >
                    <table>
                        <tr>
                            <td colspan="2" style="width:400px;">
                                <br />
                                <div>
                                    <asp:Image ID="Img_Error_Modal_Presupuesto_Existente" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                                        Visible="true" />&nbsp;
                                    <asp:Label ID="Lbl_Error_Modal_Presupuesto_Existente" runat="server" Text="Ya existe un presupuesto con estos datos" Visible="true" 
                                        CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                </div>
                                <br />
                                <asp:Label ID="Lbl_Mensaje_Modal_Presupuesto_Existente" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right; margin-right:5px;">
                                <asp:Button ID="Btn_Si_Modificar" runat="server"
                                    ToolTip="Actualizar Presupuesto"
                                    Text="Sí"
                                    Visible="false"
                                    TabIndex="1"
                                    onclick="Btn_Si_Modificar_Click" />
                            </td>
                            <td style="text-align:left; margin-left:5px;">
                                <asp:Button ID="Btn_No_Modificar" runat="server"
                                    ToolTip="Regresar"
                                    Text="No"
                                    Visible="false"
                                    TabIndex="2"
                                    onclick="Btn_No_Modificar_Click" />
                                <asp:Button ID="Btn_Salir_Modal_Presupuesto" runat="server"
                                    ToolTip="Salir"
                                    Text="Salir"
                                    Visible="true"
                                    TabIndex="3"
                                    onclick="Btn_Salir_Modal_Presupuesto_Click" />
                            </td>
                        </tr>
                    </table>
                    
                    </asp:Panel>
                </asp:Panel>
                
        </ContentTemplate>
    </asp:UpdatePanel>
    
    

</asp:Content>

