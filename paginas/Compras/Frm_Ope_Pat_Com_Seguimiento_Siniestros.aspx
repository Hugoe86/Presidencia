<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Seguimiento_Siniestros.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pat_Com_Seguimiento_Siniestros" Title="Seguimiento de Siniestros" %>

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
                        <td class="label_titulo" colspan="4">Seguimiento de Siniestros</td>
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
                            <td colspan="2">&nbsp;</td>
                        </tr> 
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Tipo_Siniestro" runat="server" Text="Tipo Siniestro" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:30%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Siniestros" runat="server" Width="100%" Enabled="false">
                                </asp:DropDownList>
                            </td>
                        </tr>                                        
                        <tr>
                            <td style="width:20%; text-align:left; ">
                               <asp:Label ID="Lbl_Bien" runat="server" Text="Vehículo" CssClass="estilo_fuente"></asp:Label>
                               
                                <asp:HiddenField ID="Hdf_Bien_ID" runat="server" />
                            </td>
                            <td colspan="3" style="text-align:left; ">
                                <asp:TextBox ID="Txt_Bien" runat="server" Width="99%" Enabled="false"></asp:TextBox>  
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
                               <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%;">
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="98%" MaxLength="20" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td  style="text-align:left; width:30%; ">
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
                            <td style="text-align:left; width:30%; " colspan="3">
                                <asp:CheckBox ID="Chk_Responsable_Municipio" runat="server" Enabled="false" />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Responsable" runat="server" Text="[Nota: Solo si el Responsable es el Municipio.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>       
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Consignado" runat="server" Text="Consignado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; " colspan="3">
                                <asp:CheckBox ID="Chk_Consignado" runat="server" Enabled="false"  />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Consignado" runat="server" Text="[Nota: Solo si fue consigando a Ministerio Público.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>   
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Pago_Danios_Sindicos" runat="server" Text="Pago a Sindicos" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; " colspan="3">
                                <asp:CheckBox ID="Chk_Pago_Danios_Sindicos" runat="server" Enabled="false"  />&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Nota_Pago_Danios_Sindicos" runat="server" Text="[Nota: Solo si hay Pago de Daño a Sindicos.]" CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>       
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Parte_Averiguacion" runat="server" Text="No. Averiguación" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%; ">
                                <asp:TextBox ID="Txt_Parte_Averiguacion" runat="server" Width="98%" MaxLength="100" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Reparacion" runat="server" Text="Reparación" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td  style="text-align:left; width:30%; ">
                                <asp:DropDownList ID="Cmb_Reparacion" runat="server" Width="100%" Enabled="false">
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
                            <td colspan="3" style="text-align:left; width:30%; ">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99%" MaxLength="50" Enabled="false"></asp:TextBox>  
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
                                    Rows="3" Width="99%"></asp:TextBox>
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
                                GridLines= "None"
                                onpageindexchanging="Grid_Observaciones_PageIndexChanging">
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
    
    <asp:UpdatePanel ID="UpPnl_Modal_Busqueda_Siniestro" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
             <asp:Button ID="Btn_Comodin_Busqueda_Siniestro" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Siniestro" runat="server" TargetControlID="Btn_Comodin_Busqueda_Siniestro" 
                PopupControlID="Pnl_MPE_Buscar_Siniestro" CancelControlID="Btn_Cerrar_MPE_Siniestros" PopupDragHandleControlID="Pnl_Interno_MPE_Buscar_Siniestro"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
      
      
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
                                AutoGenerateColumns="False" Width="99%" GridLines= "None"
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