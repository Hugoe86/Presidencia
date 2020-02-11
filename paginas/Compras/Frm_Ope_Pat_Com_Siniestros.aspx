<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Siniestros.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pat_Com_Siniestros" Title="Siniestros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Mostrar_Calendar(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Siniestros" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />  
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Siniestros</td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
                        <td colspan="4">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" colspan="2" style="width:50%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" OnClick="Btn_Generar_Reporte_PDF_Click" ToolTip="Generar Reporte [Para Imprimir]" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" />
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td align="right" style="width:50%;" colspan="2">
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                    ToolTip="Avanzada" onclick="Btn_Busqueda_Avanzada_Click">Busqueda</asp:LinkButton>
                                <asp:TextBox ID="Txt_Busqueda_Folio" runat="server" Width="150px" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Folio" runat="server" TargetControlID="Txt_Busqueda_Folio" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Folio" runat="server" TargetControlID="Txt_Busqueda_Folio" WatermarkText="<- No. Folio ->" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Busqueda_Folio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Busqueda_Folio_Click" />
                            </div>
                        </td>                       
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Siniestro_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Siniestro_ID" runat="server" Text="Folio Siniestro" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Siniestro_ID" runat="server" Width="97%" MaxLength="10" Enabled="False" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align:left;">
                                <asp:Label ID="Lbl_Nota_Folio_Siniestro" runat="server" Text="[Nota: El Folio del Siniestro se expide al darlo de Alta.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Tipo_Siniestro" runat="server" Text="Tipo Siniestro" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Siniestros" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>                                        
                        <tr>
                            <td style="width:20%; text-align:left; ">
                               <asp:Label ID="Lbl_Bien" runat="server" Text="* Vehículo" CssClass="estilo_fuente"></asp:Label>
                               
                                <asp:HiddenField ID="Hdf_Bien_ID" runat="server" />
                            </td>
                            <td colspan="3" style="text-align:left; ">
                                <asp:TextBox ID="Txt_Bien" runat="server" Width="94%" MaxLength="100" Enabled="false"></asp:TextBox>  
                                <asp:ImageButton ID="Btn_Lanzar_MPE_Bienes" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    AlternateText="Seleccionar Vehículo" onclick="Btn_Lanzar_MPE_Bienes_Click" />
                            </td>
                        </tr>                                      
                        <tr>
                            <td style="width:20%; text-align:left; ">
                               <asp:Label ID="Lbl_Aseguradora" runat="server" Text="Aseguradora" CssClass="estilo_fuente"></asp:Label>
                               
                                <asp:HiddenField ID="Hdf_Aseguradora" runat="server" />
                            </td>
                            <td colspan="3" style="text-align:left; ">
                                <asp:TextBox ID="Txt_Aseguradora" runat="server" Width="99%" MaxLength="100" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>         
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Fecha" runat="server" Text="* Fecha" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30% ">
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha" PopupButtonID="Btn_Fecha" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" Enabled="false">
                                    <asp:ListItem Text ="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                    <asp:ListItem Text ="PENDIENTE" Value="PENDIENTE"></asp:ListItem>
                                    <asp:ListItem Text ="BAJA" Value="BAJA"></asp:ListItem>
                                    <asp:ListItem Text ="FINALIZADO" Value="FINALIZADO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>      
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Responsable_Municipio" runat="server" Text="Mpio. Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; " colspan="3">
                                <asp:CheckBox ID="Chk_Responsable_Municipio" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Responsable" runat="server" Text="[Nota: Solo si el Responsable es el Municipio.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>    
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Consignado" runat="server" Text="Consignado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; " colspan="3">
                                <asp:CheckBox ID="Chk_Consignado" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Consignado" runat="server" Text="[Nota: Solo si fue consigando a Ministerio Público.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>   
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Pago_Danios_Sindicos" runat="server" Text="Pago a Sindicos" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; " colspan="3">
                                <asp:CheckBox ID="Chk_Pago_Danios_Sindicos" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Pago_Danios_Sindicos" runat="server" Text="[Nota: Solo si hay Pago de Daño a Sindicos.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>   
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Parte_Averiguacion" runat="server" Text="No. Averiguación" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%">
                                <asp:TextBox ID="Txt_Parte_Averiguacion" runat="server" Width="98%" MaxLength="100" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Reparacion" runat="server" Text="Reparación" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td  style="text-align:left;  width:30%">
                                <asp:DropDownList ID="Cmb_Reparacion" runat="server" Width="100%">
                                    <asp:ListItem Value="SELECCIONE">&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                    <asp:ListItem Value="ASEGURADORA">ASEGURADORA</asp:ListItem>
                                    <asp:ListItem Value="TALLER_MUNICIPAL">TALLER MUNICIPAL</asp:ListItem>
                                    <asp:ListItem Value="SIN REPARACIÓN">SIN REPARACIÓN</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Numero_Reporte" runat="server" Text="No. Reporte" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left;  width:30%">
                                <asp:TextBox ID="Txt_Numero_Reporte" runat="server" Width="98%" MaxLength="19" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>                         
                        <tr>
                            <td style="width:20%; text-align:left; ">
                               <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripción" 
                                    CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="98%" MaxLength="50"></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" 
                                    TargetControlID="Txt_Descripcion" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>  
                            </td>
                        </tr>          
                       <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                                <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" 
                                    CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td  colspan="3" style="text-align:left;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TextMode="MultiLine" 
                                    Rows="3" Width="98%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                    WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" 
                                    Enabled="True"/>     
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                    TargetControlID="Txt_Observaciones" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>                                                     
                            </td>
                        </tr>         
                    </table>               
                </center>        
            </div>        
            <br />
            <div style="width:98%">
                <center>
                    <caption>
                        <br />
                        <div style="width:99%; height:350px; overflow:auto;">
                            <asp:GridView ID="Grid_Observaciones" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" Width="96%"
                                GridLines= "None">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="OBSERVACION_ID" HeaderText="Siniestro ID" 
                                        SortExpression="OBSERVACION_ID">
                                        <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha"  DataFormatString="{0:dd/MMM/yyyy}" SortExpression="FECHA" >
                                        <ItemStyle Width="100px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AUTOR_OBSERVACION" HeaderText="Autor"  SortExpression="AUTOR_OBSERVACION" NullDisplayText="-" >
                                        <ItemStyle Width="170px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OBSERVACION" HeaderText="Observación" SortExpression="OBSERVACION" >
                                        <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />       
                            </asp:GridView>
                        </div>
                    </caption>
                </center>   
            </div>                                                
            <br />
            <br />
            <br />
        </ContentTemplate>           
    </asp:UpdatePanel>  
    
    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Vehiculo" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Vehiculo" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Vehiculo" runat="server" TargetControlID="Btn_Comodin_Busqueda_Vehiculo" 
                PopupControlID="Pnl_Busqueda_Vehiculo" CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_MPE_Buscar_Siniestro" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Siniestro" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Siniestro" runat="server" TargetControlID="Btn_Comodin_Busqueda_Siniestro" 
                PopupControlID="Pnl_MPE_Buscar_Siniestro" CancelControlID="Btn_Cerrar_MPE_Siniestros" PopupDragHandleControlID="Pnl_Interno_MPE_Buscar_Siniestro"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Busqueda_Vehiculo" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda" runat="server"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="Tab_Panel_Datos_Generales_Busqueda" Width="100%" Height="400px">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Titulo_Busqueda" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Inventario" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Economico" runat="server" Text="Número Económico"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Economico" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Economico" runat="server" TargetControlID="Txt_Busqueda_Numero_Economico" InvalidChars="<,>,&,',!,"  FilterType="Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Modelo_Busqueda" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo_Busqueda" runat="server" TargetControlID="Txt_Modelo_Busqueda"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Marca" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Vehiculo" runat="server" Text="Tipo Vehículo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Vehiculo" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Combustible" runat="server" Text="Combustible" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Combustible" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Anio_Fabricacion" runat="server" Text="Año Fabricación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Anio_Fabricacion" runat="server" Width="97%" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Anio_Fabricacion" runat="server" TargetControlID="Txt_Busqueda_Anio_Fabricacion" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Color" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Zonas" runat="server" Text="Zonas" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Zonas" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="85%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                ToolTip="Buscar Contrarecibos" OnClick="Btn_Buscar_Datos_Click" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Click" />                                      
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes" ID="Tab_Panel_Resguardantes_Busqueda" Width="100%" >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Busqueda_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_RFC_Resguardante" runat="server" Text="RFC Reguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC_Resguardante" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Resguardantes_Dependencias" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td colspan="4" style="text-align:right">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_Buscar_Resguardante_Click" />                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante" runat="server"  
                                                CausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Click" />   
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>                                      
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:97%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Vehiculos" runat="server" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                OnPageIndexChanging="Grid_Listado_Vehiculos_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Vehiculos_SelectedIndexChanged"
                                Width="98%" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="VEHICULO_ID" HeaderText="VEHICULO_ID" SortExpression="VEHICULO_ID" />
                                    <asp:BoundField DataField="NUMERO_INVENTARIO" HeaderText="No. Inv." SortExpression="NUMERO_INVENTARIO" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VEHICULO" HeaderText="Vehículo" SortExpression="VEHICULO" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO">
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO">
                                        <ItemStyle Width="70px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </caption>
                    </center>   
                </div>                                                  
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="95%">
                <tr>
                    <td style="width:100%">
                        <center>
                            <asp:Button ID="Btn_Cerrar" runat="server" TabIndex="202" Text="Cerrar" Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel>               
    
    <asp:Panel ID="Pnl_MPE_Buscar_Siniestro" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Interno_MPE_Buscar_Siniestro" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Siniestros
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_MPE_Siniestros" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Siniestros" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <asp:UpdateProgress ID="UpPgr_Mpe_Siniestros" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Siniestros" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                   </ProgressTemplate>                     
                </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 300px; background-color: White;">
                        <table width="100%">                               
                            <tr>
                                <td style="width:20%; text-align:left; ">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Numero_Inventario" runat="server" Text="Inv. [Vehículo]" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:30%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Siniestros_Numero_Inventario" runat="server" Width="97%" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Siniestros_Numero_Inventario" runat="server" TargetControlID="Txt_MPE_Siniestros_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                    </cc1:FilteredTextBoxExtender>  
                                </td>
                                <td style="width:20%; text-align:left; ">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Clave_Sistema" runat="server" Text="Folio Siniestro"  CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:30%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Siniestros_Clave_Sistema" runat="server" Width="97%" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Siniestros_Clave_Sistema" runat="server" TargetControlID="Txt_MPE_Siniestros_Clave_Sistema" FilterType="Numbers" Enabled="True">        
                                    </cc1:FilteredTextBoxExtender>  
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width:20%; text-align:left; ">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_MPE_Siniestros_Dependencias" runat="server" Width="100%">
                                        <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                    </asp:DropDownList>                                   
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width:20%; text-align:left; ">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Aseguradoras" runat="server" Text="Aseguradoras" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_MPE_Siniestros_Aseguradoras" runat="server" Width="100%">
                                        <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                    </asp:DropDownList>                                   
                                </td>
                            </tr>                    
                            <tr>
                                <td style="width:20%; text-align:left; vertical-align:top;">
                                   <asp:Label ID="Lbl_MPE_Siniestros_Fecha" runat="server" Text="Fecha" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="text-align:left; ">
                                    <asp:TextBox ID="Txt_MPE_Siniestros_Fecha" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_MPE_Siniestros_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_MPE_Siniestros_Fecha" runat="server" TargetControlID="Txt_MPE_Siniestros_Fecha" PopupButtonID="Btn_MPE_Siniestros_Fecha" Format="dd/MMM/yyyy" OnClientShown="Mostrar_Calendar">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="width:20%; text-align:left; ">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:30%; text-align:left;">
                                    <asp:DropDownList ID="Cmb_MPE_Siniestros_Estatus" runat="server" Width="100%">
                                        <asp:ListItem Text ="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                        <asp:ListItem Text ="PENDIENTE" Value="PENDIENTE"></asp:ListItem>
                                        <asp:ListItem Text ="BAJA" Value="BAJA"></asp:ListItem>
                                        <asp:ListItem Text ="FINALIZADO" Value="FINALIZADO"></asp:ListItem>
                                    </asp:DropDownList>                                                
                                </td>
                            </tr>    
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_MPE_Siniestros_Descripcion" runat="server" Text="Descripción" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="text-align:left;" colspan="3">   
                                    <asp:TextBox ID="Txt_MPE_Siniestros_Descripcion" runat="server" Width="99%" MaxLength="50"></asp:TextBox>  
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Siniestros_Descripcion" runat="server" 
                                        TargetControlID="Txt_MPE_Siniestros_Descripcion" InvalidChars="<,>,&,',!," 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                    </cc1:FilteredTextBoxExtender>                                            
                                </td>
                            </tr>  
                            <tr>
                                <td colspan="4" style="text-align:right;">                          
                                    <asp:ImageButton ID="Btn_MPE_Siniestros_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                        OnClick="Btn_MPE_Siniestros_Buscar_Click" ToolTip="Buscar Contrarecibos"  />
                                    <asp:ImageButton ID="Btn_MPE_Siniestros_Limpiar_Filtros" runat="server" OnClick= "Btn_MPE_Siniestros_Limpiar_Filtros_Click"
                                        CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" ToolTip="Limpiar Filtros" Width="20px" />
                                    &nbsp;&nbsp;&nbsp;  
                                </td>
                            </tr>                                   
                        </table>
                        <asp:Panel ID="Pnl_Listado_Bienes" runat="server" ScrollBars="Vertical" style="white-space:normal;" Width="100%" BorderColor="#3366FF" Height="145px">
                            <asp:GridView ID="Grid_Listado_Siniestros" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" Width="99%" GridLines= "None" AllowPaging="true" PageSize="100"
                                onselectedindexchanged="Grid_Listado_Siniestros_SelectedIndexChanged" 
                                onpageindexchanging="Grid_Listado_Siniestros_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="SINIESTRO_ID" HeaderText="Folio" SortExpression="SINIESTRO_ID">
                                        <ItemStyle Width="70px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}" SortExpression="FECHA">
                                        <ItemStyle Width="70px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_SINIESTRO" HeaderText="Tipo" SortExpression="TIPO_SINIESTRO" >
                                        <ItemStyle Width="100px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BIEN_ID" HeaderText="Vehículo" SortExpression="BIEN_ID" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION">
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                        <ItemStyle Width="70px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />       
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel> 
         
</asp:Content>