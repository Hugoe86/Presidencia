<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Ordenes_Compra.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Ordenes_Compra" Title="Elaborar Orden de Compra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() {
        Contar_Caracteres();
    }

    function Contar_Caracteres() {
        $('textarea[id$=Txt_Condicion1]').keyup(function() {
            var Caracteres = $(this).val().length;

            if (Caracteres > 3600) {
                this.value = this.value.substring(0, 3600);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            } else {
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }

            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 3600 ]');
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

 <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True">
 </asp:ScriptManager>
 <div id="Div_General" style="width: 98%;" visible="true" runat="server">
      <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
           <ContentTemplate>
               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                   
                    <ProgressTemplate>
                    
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div> 
                    </ProgressTemplate>    
                                 
                </asp:UpdateProgress>
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">  
                    <table style="width: 100%;" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Elaborar Orden de Compra</td>
                    </tr>
                  
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
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle">                         
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar Orden de Compra" CssClass="Img_Button" AlternateText="Nuevo"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" onclick="Btn_Guardar_Click" Visible="false" />
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" onclick="Btn_Salir_Click" />                                    
                        </td>
                        <td>
                        </td>                         
                    </tr> 

                    </table>                    
                </div>      
                <%--Div listado--%>
               <div id="Div_Requisiciones_Para_Orden_Compra" runat="server" style="overflow: auto;
                   height: 320px; width: 99%; vertical-align: top; border-style: outset; border-color: Silver;">
                   <table style="width:100%" >
                       <tr>
                            <td style="width:10%;">
                                Cotizador(a)
                            </td>
                            <td >
                                <asp:DropDownList ID="Cmb_Cotizadores" runat="server" Width="100%"
                                 onselectedindexchanged="Cmb_Cotizadores_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                       </tr>
                       <tr>
                           <td colspan="2">
                               <asp:GridView ID="Grid_Requisiciones_Cotizadas" runat="server" AutoGenerateColumns="False"
                                   CssClass="GridView_1" GridLines="None" Width="99%" DataKeyNames="NO_REQUISICION,TOTAL_COTIZADO,TIPO_ARTICULO,FOLIO,UNIDAD_RESPONSABLE"
                                   OnSelectedIndexChanged="Grid_Requisiciones_Cotizadas_SelectedIndexChanged" PageSize="5"                                
                                   EmptyDataText="Sin registros">
                                   <RowStyle CssClass="GridItem" />
                                   <Columns>
                                       <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                           <ItemStyle Width="5%" />
                                       </asp:ButtonField>
                                       <asp:BoundField DataField="NO_REQUISICION" HeaderText="No. Requisición" Visible="False">
                                           <HeaderStyle HorizontalAlign="Left" />
                                           <ItemStyle HorizontalAlign="Left" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="NO_REQUISICION">
                                           <HeaderStyle HorizontalAlign="Left" />
                                           <ItemStyle HorizontalAlign="Left" Width="15%" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="UNIDAD_RESPONSABLE" HeaderText="Unidad Responsable" Visible="true">
                                           <HeaderStyle HorizontalAlign="Left" />
                                           <ItemStyle HorizontalAlign="Left" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="FECHA_COTIZACION" HeaderText="Fecha Cotizada" SortExpression="FECHA_COTIZACION"
                                           DataFormatString="{0:dd/MMM/yyyy}">
                                           <FooterStyle HorizontalAlign="Center" />
                                           <HeaderStyle HorizontalAlign="Center" />
                                           <ItemStyle HorizontalAlign="Center" Width="20%" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="TOTAL_COTIZADO" HeaderText="Total Cotizado" DataFormatString="{0:C}"
                                           SortExpression="TOTAL_COTIZADO">
                                           <HeaderStyle HorizontalAlign="Right" Wrap="True" />
                                           <ItemStyle HorizontalAlign="Right" Width="14%" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="TIPO_ARTICULO" HeaderText="Tipo Articulo" Visible="False">
                                           <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                           <ItemStyle HorizontalAlign="Right" Width="14%" />
                                       </asp:BoundField>
                                   </Columns>
                                   <PagerStyle CssClass="GridHeader" />
                                   <SelectedRowStyle CssClass="GridSelected" />
                                   <HeaderStyle CssClass="GridHeader" />
                                   <AlternatingRowStyle CssClass="GridAltItem" />
                               </asp:GridView>
                           </td>
                       </tr>
                   </table>
               </div>
 
                   <%--Div con los detalles de Compra--%>                              
                <div id="Div_Articulos" runat="server" visible="false">  
                  <asp:Panel ID="Pnl_Datos_Generales" runat="server"  GroupingText="Datos de Orden de Compra" Width="99%" >
                    <table style="width: 100%;">
                        <tr align="left">
                            <td colspan="4" class="label_titulo"></td>
                        </tr>      
                        <tr style="display:none;">
                            <td align="left" style="width:15%">
                                Proceso de Compra:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="Txt_Proceso_Compra" runat="server" Width="180px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>     
                        <tr>
                            <td align="left" style="width:15%">
                                No. Requisición                                
                            </td>                            
                            <td align="left">                                
                                <asp:TextBox ID="Txt_Identificador_Compra" runat="server"  Enabled="false" Width="160px"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Imprimir_Req" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" ToolTip="Ver Requisición" 
                                 OnClick="Btn_Imprimir_Req_Click"/>                                
                            </td> 
                            <td>
                                
                            </td>                           
                        </tr>     
                        <tr>
                            <td align="left" style="width:15%">
                                Unidad Responsable                                
                            </td>                            
                            <td align="left" colspan="3">                                
                                <asp:TextBox ID="Txt_Unidad_Responsable" runat="server"  Enabled="false" Width="75%"></asp:TextBox>
                            </td>                            
                        </tr>    
                        <tr>
                            <td align="left" style="width:15%" >
                                Proveedor                                
                            </td>                            
                            <td align="left" colspan="3">                                
                                <asp:TextBox ID="Txt_Proveedor" runat="server"  Enabled="false" Width="75%"></asp:TextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Importe Total
                            </td>
                            <td colspan = "3">
                                
                                <asp:TextBox ID="Txt_Total" runat="server" Enabled="false" Style="text-align:right"></asp:TextBox>
                            </td>
                        </tr>                         
                        <tr>
                            <td colspan="4">
                              <hr class="linea" />
                            </td>
                        </tr>  
                    </table>
                    <table>
                        <tr>      
                            <td align="left" style="width:35%">
                                Fecha de Entrega:&nbsp;&nbsp;
                                   
                                <asp:TextBox ID="Txt_Fecha_Entrega" runat="server" 
                                    Width="85px" TabIndex="16" MaxLength="11" Height="18px"  
                                    OnTextChanged="Txt_Fecha_Entrega_OnTextChanged" 
                                    AutoPostBack="True"/>
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Entrega" runat="server" 
                                     TargetControlID="Txt_Fecha_Entrega" WatermarkCssClass="watermarked" 
                                     WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Entrega" runat="server" 
                                     TargetControlID="Txt_Fecha_Entrega" Format="dd/MMM/yyyy" Enabled="True" 
                                     PopupButtonID="Btn_Fecha"/>
                                <asp:ImageButton ID="Btn_Fecha" runat="server"
                                     ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                     Height="18px" CausesValidation="false"/>           
                                <cc1:MaskedEditExtender 
                                     ID="Mee_Txt_Fecha_Entrega" 
                                     Mask="99/LLL/9999" 
                                     runat="server"
                                     MaskType="None" 
                                     UserDateFormat="DayMonthYear" 
                                     UserTimeFormat="None" Filtered="/"
                                     TargetControlID="Txt_Fecha_Entrega" 
                                     Enabled="True" 
                                     ClearMaskOnLostFocus="false"/>  
                                <cc1:MaskedEditValidator 
                                     ID="Mev_Txt_Fecha_Entrega" 
                                     runat="server" 
                                     ControlToValidate="Txt_Fecha_Entrega"
                                     ControlExtender="Mee_Txt_Fecha_Entrega" 
                                     EmptyValueMessage="Fecha Requerida"
                                     InvalidValueMessage="Fecha no valida" 
                                     IsValidEmpty="false" 
                                     TooltipMessage="Ingrese o Seleccione la Fecha"
                                     Enabled="true" 
                                     style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                            </td>                            
                            <td align="left" style="width:30%">
                                Días adicionales: &nbsp;&nbsp;
                                <asp:DropDownList ID="Cmb_Dias_Adicionales" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="Cmb_Dias_Adicionales_SelectedIndexChanged"/>                              
                            </td>                           
                            <td>    
                                Fecha plazo: &nbsp;&nbsp;                            
                                <asp:TextBox ID="Txt_Fecha_Plazo" runat="server" Width="85px" Enabled="false" ></asp:TextBox>           
                            </td>                            
                        </tr>                                             
                      </table>   
                   </asp:Panel>   
                                
                  <asp:Panel ID="Pnl_Condiciones_Compra" runat="server"  GroupingText="Condiciones de compra" Width="99%" >
                    <table style="width: 100%;">
                        <tr align="left">
                            <td colspan="2" style="width:100%; white-space:normal;">
                                <asp:TextBox ID="Txt_Condicion1" runat="server" Width="98%" MaxLength="3600" Height="120px" Wrap="true"
                                TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Condicion1" runat="server" TargetControlID="Txt_Condicion1" 
                                        FilterType="Custom, UppercaseLetters,Numbers, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Condicion1" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 3600 carácteres>" TargetControlID="Txt_Condicion1" />      
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>  
                                                      
                            </td>
                        </tr>      
                          <tr align="left" style="display:none;">
                            <td style="width:15%">
                                Condición
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Condicion2" runat="server" Width="98%" MaxLength="60"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Condicion2" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 60 caracteres>" TargetControlID="Txt_Condicion2" />                            
                            </td>
                        </tr>   
                        <tr align="left" style="display:none;">
                            <td style="width:15%">
                                Condición
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Condicion3" runat="server" Width="98%" MaxLength="500" Height="90px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Condicion3" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 500 caracteres>" TargetControlID="Txt_Condicion3" />                            
                            </td>
                        </tr>   
                        <tr align="left" style="display:none;">
                            <td style="width:15%">
                                Condición
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Condicion4" runat="server" Width="98%" MaxLength="60"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Condicion4" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 60 caracteres>" TargetControlID="Txt_Condicion4" />                            
                            </td>
                        </tr>   
                        <tr align="left" style="display:none;">
                            <td style="width:15%">
                                Condición
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Condicion5" runat="server" Width="98%" MaxLength="60"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Condicion5" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 60 caracteres>" TargetControlID="Txt_Condicion5" />                            
                            </td>
                        </tr>     
                        <tr align="left" style="display:none;">
                            <td style="width:15%">
                                Condición
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Condicion6" runat="server" Width="98%" MaxLength="60"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Condicion6" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., "
                                        InvalidChars="'%" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Máximo 60 caracteres>" TargetControlID="Txt_Condicion6" />                            
                            </td>
                        </tr>                                                                                                                                                             
                      </table>   
                   </asp:Panel>                                
                                                                                                                                                                                                                                                                                                  
                      <table style="width: 100%; display:none;">
                        <tr>
                            <td style="width:99%" align="center" colspan="4">
                                 <asp:GridView ID="Grid_Detalles_Compra" runat="server"
                                    AutoGenerateColumns="false" DataKeyNames="ID,NOMBRE_PROVEEDOR"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="false">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Clave" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>            
                                        <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="Proveedor" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>                                                                                    
                                        <asp:BoundField DataField="IMPORTE_TOTAL_CON_IMP_COT" 
                                            HeaderText="$ Importe" Visible="true"
                                            DataFormatString="{0:C}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="10%"/>
                                        </asp:BoundField>                                                                                                                                                                                                                                                             
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                             </td>
                        </tr>
                       
                        <tr>
                            <td colspan="4">
                              <hr class="linea" />
                            </td>
                        </tr>
                    </table>
                </div>                  
                
                
                <div id="Div_Ordenes_Generadas" runat="server" visible="false">      
                   <table style="width: 100%;">
                       <tr>
                           <td class="label_titulo">Órdenes de Compra Creadas</td>    
                       </tr> 
                       <tr>
                           <td>
                                 <asp:GridView ID="Grid_Ordenes_Creadas" runat="server"
                                    AutoGenerateColumns="false"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="false">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>                                      
                                        <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="numero" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="35px"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="Proveedor" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="55%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SUBTOTAL" HeaderText="Subtotal" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                        </asp:BoundField>            
                                        <asp:BoundField DataField="IEPS" HeaderText="IEPS" Visible="true" DataFormatString="{0:0.00}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IVA" HeaderText="IVA" Visible="true" DataFormatString="{0:0.00}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                        </asp:BoundField>              
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" Visible="true" DataFormatString="{0:0.00}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                        </asp:BoundField>                                  
                                        <asp:BoundField DataField="LISTA_REQUISICIONES" HeaderText="Lista Requisas" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                                                                                                                                                                                                                                                                                                                                   
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                           </td>
                           <td>
                               &nbsp;
                           </td>
                       </tr>

                   </table>
                </div>    <%--Area de trabajo--%>                                                                                 
           </ContentTemplate>           
      </asp:UpdatePanel>
  </div>






</asp:Content>

