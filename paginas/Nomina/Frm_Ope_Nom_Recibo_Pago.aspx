<%@ Page Title="Recibos de Pago" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Recibo_Pago.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Recibo_Pago" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        
        function Cerrar_Modal_Popup() {
            $find('Busqueda').hide();
            return false;
        }          
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="SM_Recibos_Empleados" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
   <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>  
    
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>       
     
    <div id="Div_Ope_Nom_Recibo_Pago_Botones" style="background-color:#ffffff; width:100%; height:100%;">                
        <table id="Tbl_Comandos" style="width: 98%;">                        
            <tr>
                <td colspan="2" class="label_titulo">
                    Recibo de Pago de Sueldo
                </td>
            </tr>                            
                           
            <tr class="barra_busqueda">
                <td style="width:50%">
                    <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Width="24px" CssClass="Img_Button" 
                    AlternateText="Imprimir" onclick="Btn_Imprimir_Click" OnClientClick="return confirm('¿Esta seguro de Imprimir los recibos seleccionados? \n La impresion se mandara a la Impresora predeterminada');"/>
                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                    CssClass="Img_Button"
                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"/>
                </td>
                <td align="right" style="width:50%">
                    <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                        ToolTip="Avanzada" onclick="Btn_Busqueda_Avanzada_Click">
                        B&uacute;squeda 
                    </asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="220px"
                        ToolTip="Buscar" TabIndex="1" />
                    <asp:ImageButton ID="Btn_Busqueda" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                        TabIndex="2" onclick="Btn_Busqueda_Click"/>
                    <cc1:textboxwatermarkextender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                        WatermarkText="<Buscar por Número de Empleado>" TargetControlID="Txt_Busqueda"/>
                </td>                        
            </tr>                            
        </table>
     </div>
      
<div id="Div_Ope_Nom_Recivos_Pago" runat="server" style="background-color:#ffffff; width:100%; height:100%;">        
    <table id="Datos Generales_Inner" style="width: 98%;">                            
        <tr>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Recibo_ID" runat="server" Text="Recibo ID"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Recibo_ID" runat="server" Width="98%"/>
            </td>
            <td style="width:20%; text-align:left;">
                &nbsp;
            </td>
            <td style="width:30%; text-align:left;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Empleado_ID" runat="server" Text="No Empleado"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Empleado_ID" runat="server" Width="98%" />
            </td>
            <td style="width:20%; text-align:left;">
                &nbsp;&nbsp;<asp:Label ID="Lbl_RFC" runat="server" Text="R.F.C."/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_RFC" runat="server" Width="98%"/>
            </td>
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre del Empleado"/>
            </td>
            <td style="width:30%; text-align:left;" colspan="3">
                <asp:TextBox ID="Txt_Nombre" runat="server" Width="99.5%"/>                                           
            </td>                                
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">                                    
                <asp:Label ID="Lbl_CURP" runat="server" Text="CURP"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Curp" runat="server" Width="98%"/>
            </td>
            <td style="width:20%; text-align:left;">
                &nbsp;&nbsp;<asp:Label ID="Lbl_No_Afiliacion" runat="server" Text="No Afiliación"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_No_Afiliacion" runat="server" Width="98%"/>
            </td>
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Departamento" runat="server" Text="Departamento al que pertenece"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Departamento" runat="server" Width="98%"/>
            </td>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Categoria" runat="server" Text="Categoria"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Categoria" runat="server" Width="98%"/>
            </td>
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">                                    
                <asp:Label ID="Lbl_Periodo" runat="server" Text="Periodo"/>                                    
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Periodo" runat="server" Width="98%"/>
            </td>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Codigo" runat="server" Text="Código Programático"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Codigo" runat="server" Width="98%"/>
            </td>
        </tr>
        <tr>
            <td style="width:20%; text-align:left;">
                <asp:Label ID="Lbl_Dias" runat="server" Text="Dias Trabajados"/>
            </td>
            <td style="width:30%; text-align:left;">
                <asp:TextBox ID="Txt_Dias" runat="server" Width="98%"/>
            </td>
            <td style="width:20%; text-align:left;">
                &nbsp;
            </td>
            <td style="width:30%; text-align:left;">
                &nbsp;
            </td>
        </tr>
    </table>                            
    
    <br />
         
    <div style="overflow:auto;height:330px;width:97%;vertical-align:top;border-style:outset;border-color:Silver;" >
        <asp:GridView ID="Grid_Recibos_Pago" runat="server"
            AutoGenerateColumns="False" CssClass="GridView_1" 
            EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none"                                         
            Style="white-space:normal" Width="96%" 
            AllowSorting="True"
            onselectedindexchanged="Grid_Recibos_Pago_SelectedIndexChanged" 
            onsorting="Grid_Recibos_Pago_Sorting">
            
            <RowStyle CssClass="GridItem" />
            <PagerStyle CssClass="GridHeader" />
            <SelectedRowStyle CssClass="GridSelected" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle CssClass="GridAltItem" />      
                  
            <Columns>
                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                    <ItemStyle Width="3%" />
                </asp:ButtonField>
                <asp:BoundField DataField="RECIBO_NO" HeaderText="No. Recibo">
                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    <ItemStyle HorizontalAlign="Left" Width="10%" />                                        
                </asp:BoundField>
                <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Empleado">
                    <HeaderStyle HorizontalAlign="Left" Width="35%" />
                    <ItemStyle HorizontalAlign="Left" Width="35%" />                                                                            
                </asp:BoundField>                                                
                <asp:BoundField DataField="PERIODO" HeaderText="Periodo">
                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DIAS_TRABAJADOS" HeaderText="Laborados">
                    <HeaderStyle HorizontalAlign="Left" Width="10%"/>
                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Imprimir" HeaderStyle-HorizontalAlign="Center" 
                    ItemStyle-HorizontalAlign="Center" SortExpression="Imprimir">
                    <ItemTemplate>                                            
                        <asp:CheckBox ID="Chk_Seleccionado" runat="server" Style="position: static" 
                            ItemStyle-HorizontalAlign="Center" EnableViewState="true"/>
                    </ItemTemplate>                
                    <HeaderStyle Width="10%" ForeColor="OrangeRed" HorizontalAlign="Center"/>                            
                </asp:TemplateField>
            </Columns>
        </asp:GridView>     
        </div>      
     </div>     
    
   </ContentTemplate>
   <Triggers>
        <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar_Busqueda_Av" EventName="Click"/>
   </Triggers>
</asp:UpdatePanel>
            
<asp:UpdatePanel ID="UPnl_Busqueda_Avanzada" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:Button ID="Btn_Comodin_Busqueda" runat="server" Text="Button" style="display:none;"/> 
    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" TargetControlID="Btn_Comodin_Busqueda"  BehaviorID="Busqueda"
        PopupControlID="Pnl_Buscar_Avanzada" CancelControlID="Btn_Cerrar_Ventana_Autorizacion" PopupDragHandleControlID="Pnl_Buscar_Avanzada_Interno"
        DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>     
    </ContentTemplate>  
</asp:UpdatePanel>  


<asp:Panel ID="Pnl_Buscar_Avanzada" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="550px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Buscar_Avanzada_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Recibos Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Autorizacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>            
    <center>            
            
<asp:UpdatePanel ID="UPnl_Busqueda_Recibos_Empleados" runat="server" UpdateMode="Conditional">
    <ContentTemplate>        
        <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="UPnl_Busqueda_Recibos_Empleados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressTemplateInner"></div>
                <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress> 
                    
        <table width="100%" class="estilo_fuente">
            <tr>                                
                <td colspan="4">
                    <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png" />
                    <br />
                    <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"/>
                </td>                                
            </tr>                           
            <tr id="Tr_Periodos_Fiscales" runat="server">
                <td colspan="4" style="width:100%">                                                                 
                    <asp:Panel id="Pnl_Nomina_Periodo_Incapacidad" runat="server" GroupingText="Calendario Nomina">
                        <table width="98%">
                            <tr>
                                <td class="button_autorizar" style="width:20%;text-align:left;">
                                    *Nomina
                                </td>
                                <td class="button_autorizar" style="width:30%;text-align:left;">
                                    <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                        TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                </td>             
                                <td class="button_autorizar" style="width:20%;text-align:left;">
                                    *Periodo
                                </td>
                                <td class="button_autorizar" style="width:30%;text-align:left;">
                                    <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                        Width="100%" TabIndex="6" />
                                </td>                                                                                        
                            </tr>
                        </table>
                    </asp:Panel>      
                    <br />                     
                </td>
            </tr> 
        </table>    
                             
        <table width="100%">                                     
            <tr>
                <td class="button_autorizar" colspan="4" align="center" style="width:100%">
                    <hr />
                </td>
            </tr> 
            <tr>
                <td class="button_autorizar" align="left" style="width:20%; text-align:left; cursor:default;">
                    <asp:Label ID="Lbl_Tipos_Nomina" runat="server" Text="*Tipos Nominas"/>                                                                
                    &nbsp;&nbsp;&nbsp;
               </td>
                <td class="button_autorizar" colspan="3" style="width:30%; text-align:left; cursor:default;">
                    <asp:DropDownList ID="Cmb_Tipos_Nominas" runat="server" Width="90%"/>
                </td>
            </tr>                
            <tr>
                <td class="button_autorizar" align="left" style="width:20%; text-align:left; cursor:default;">
                    <asp:Label ID="Label1" runat="server" Text="No. Empleado"/>                                                                
                    &nbsp;&nbsp;&nbsp;
               </td>
               <td class="button_autorizar" colspan="3" style="width:30%; text-align:left; cursor:default;">
                    <asp:TextBox ID="Txt_Empleado_ID_Busqueda" runat="server" Width="90%"/>
               </td>
            </tr>
            <tr>                         
                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                <asp:Label ID="Lbl_Rfc_Busqueda" runat="server" Text="R.F.C."/>
                </td>
                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;" colspan="3">
                     <asp:TextBox ID="Txt_Rfc_Busqueda" runat="server" Width="90%"/>
                </td>
            </tr>
            <tr>
                <td class="button_autorizar"  style="width:20%; text-align:left; cursor:default;">
                    <asp:Label ID="Lbl_Curp_Busqueda" runat="server" Text="CURP"/>
                </td>
                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;" colspan="3">
                    <asp:TextBox ID="Txt_Curp_Busqueda" runat="server" Width="90%"/>
                </td>
            </tr>
            <tr>
                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                    <asp:Label ID="Lbl_Departamento_Busqueda" runat="server" Text="Unidad Responsable"/>                                
                </td>
                <td class="button_autorizar" colspan="3" style="width:30%; text-align:left; cursor:default;">                           
                    <asp:DropDownList ID="Cmb_Departamento" runat="server" Width="92%"/>
                </td>                        
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width:20%; text-align:left; cursor:default;" colspan="4">
                    <asp:Button ID="Btn_Aceptar_Busqueda_Av" runat="server" CssClass="button_autorizar" 
                        Text="Buscar Recibos" Width="100%" onclick="Btn_Aceptar_Busqueda_Av_Click"
                        style="color:Black;border-style:outset;"/>
                </td>
            </tr>
        </table>      
    </ContentTemplate>                        
</asp:UpdatePanel>       
    </center>        
  </asp:Panel>      
  
</asp:Content>

