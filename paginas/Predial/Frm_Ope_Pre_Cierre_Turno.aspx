<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cierre_Turno.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cierre_Turno" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    
    
<script type="text/javascript" language="javascript">
$(function($) {
            var options={};
            $('.jclock').jclock(options);
        });
function switchViews(obj,row) 
    { 
        var div = document.getElementById(obj); 
        var img = document.getElementById('img' + obj); 
         
        if (div.style.display=="none") 
            { 
                div.style.display = "inline"; 
                if (row=='alt') 
                    { 
                        img.src="../imagenes/paginas/stocks_indicator_down.png";
                    } 
                else 
                    { 
                        img.src="../imagenes/paginas/stocks_indicator_down.png";
                    } 
                img.alt = "Close to view other customers"; 
            } 
        else 
            { 
                div.style.display = "none"; 
                if (row=='alt') 
                    { 
                        img.src="../imagenes/paginas/add_up.png";
                    } 
                else 
                    { 
                        img.src="../imagenes/paginas/add_up.png";
                    } 
                img.alt = "Expand to show orders"; 
            } 
    }
    
    function Sumar(){
      var Denominacion =0;
      var Total = 0;
      var Monto = 0;
      
      $('#Div_Denominaciones :input').each(function(){
        Denominacion = parseFloat(($(this).attr('title') == "") ? "0" : $(this).attr('title'));
        Monto = parseFloat((($(this).val() == "")?"0":$(this).val()));
        Monto = Monto * Denominacion;
        Total = Total + Monto;
      });
      
      $('input[id$=Txt_Total]').val(Total);
    }    
    
    function Limpiar_Ctlr(){
      $('input[id$=Txt_Total]').val('0');
      
      $('#Div_Denominaciones :input').each(function(){
        $(this).val('0');
      });
    }
</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%></ProgressTemplate>
            </asp:UpdateProgress>
             <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td class="label_titulo">Cierre de Turno</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td>
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
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Visible="false"
                            ToolTip="Nuevo"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <%--<td align="right" colspan="3" style="width:99%;">
                        <div id="Div_Busqueda" runat="server">
                        Busqueda
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png"/>
                        </div>
                    </td> --%>
            </tr>
            <tr>
                <td>
                    <div id="Div_Grid_Cierre_Turno" style="width:100%;height:100%;" runat="server">
                        <asp:GridView ID="Grid_Cierre_Turno" runat="server" AllowSorting="True" 
                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Turno" 
                            Enabled="true" GridLines="None" HeaderStyle-CssClass="tblHead" 
                            onselectedindexchanged="Grid_Cierre_Turno_SelectedIndexChanged" Width="99%">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" HeaderText="Ver" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver Requisicion">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="No_Turno" HeaderText="No_Turno" Visible="false">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Caja_Id" HeaderText="Caja_Id" Visible="false">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CLAVE_CAJA" HeaderText="Clave Caja" 
                                    ItemStyle-Wrap="true" SortExpression="CLAVE_CAJA" Visible="true">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NUM_CAJA" HeaderText="Numero Caja" 
                                    ItemStyle-Wrap="true" SortExpression="NUM_CAJA" Visible="True">
                                    <FooterStyle HorizontalAlign="Left" Width="15%" Wrap="true" />
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" Wrap="true" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado_ID" 
                                    Visible="false">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CAJERO" HeaderText="Cajero" ItemStyle-Wrap="true" 
                                    SortExpression="CAJERO" Visible="True">
                                    <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Hora_Apertura" HeaderText="Hora Apertura" 
                                    ItemStyle-Wrap="true" SortExpression="Hora_Apertura" Visible="True">
                                    <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Hora_Cierre" HeaderText="Hora Cierre" 
                                    ItemStyle-Wrap="true" SortExpression="Hora_Cierre" Visible="True">
                                    <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                <div ID="Div_Cierre_Caja" runat="server" style="width:100%;font-size:9px;" 
                    visible="false">
                    <table width="99%">
                        <tr>
                            <td colspan="4" align="center">Detalle Apertura</td>
                        </tr>
                        <tr>
                            <td>Cajero</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Cajero" runat="server" Width="99%" Enabled="false"></asp:TextBox></td>
                            
                        </tr>
                        <tr>
                            <td width="20%">
                            Número de Caja
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="Txt_No_Caja" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            <td width="20%">
                            Clave Caja
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="Txt_Clave_Caja" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Hora Apertura
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Hora_Apertura" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                Fecha Apertura
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Apertura" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                Recibo Inicial
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Recibo_Inicial" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                Fondo Inicial
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fondo_Inicial" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Aplicacion de Pagos
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Aplicacion_Pagos" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="right" class="barra_delgada">
                                   <td align="center" colspan="4">
                                   </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" >
                                Detalle Cierre
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Hora Cierre
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Fecha" runat="server" style="font-family:Verdana, Times New Roman; font-size:11px; font-weight:normal;" Visible="false"></asp:Label>                                                                                                                                               
                                <span class="jclock"></span>
                                <asp:TextBox ID="Txt_Hora_Cierre" runat="server" Width="99%" Enabled="false" ></asp:TextBox>
                            </td>
                            <td >
                                <asp:TextBox ID="Txt_Fecha_Cierre" runat="server" Width="99%" Enabled="false" Visible="false"></asp:TextBox>
                            </td>
                            <td rowspan="2" style="vertical-align:middle;" align="center">
                                <asp:Button ID="Btn_Cerrar_Caja" runat="server" Text="Cerrar Caja" 
                                    CssClass="button" Width="90%" Height="50px" onclick="Btn_Cerrar_Caja_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estatus
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td colspan="4">
                            
                               <asp:GridView ID="Grid_Detalle_Pago" runat="server" DataKeyNames="DEPENDENCIA_ID"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="99%"
                                    AllowSorting="True"  HeaderStyle-CssClass="tblHead" OnRowDataBound="Grid_Detalle_Pago_RowDataBound">
                                    
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                                    <Columns>
                                        <%--<asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png">
                                            <ItemStyle Width="3%" />
                                        </asp:ButtonField>--%>
                                        <asp:TemplateField> 
                                            <ItemTemplate> 
                                                <a href="javascript:switchViews('div<%# Eval("DEPENDENCIA_ID") %>', 'one');"> 
                                                    <img id="imgdiv<%# Eval("DEPENDENCIA_ID") %>" alt="Click to show/hide orders" border="0" src="../imagenes/paginas/add_up.png" /> 
                                                </a> 
                                            </ItemTemplate> 
                                            <AlternatingItemTemplate> 
                                                <a href="javascript:switchViews('div<%# Eval("DEPENDENCIA_ID") %>', 'alt');"> 
                                                    <img id="imgdiv<%# Eval("DEPENDENCIA_ID") %>" alt="Click to show/hide orders" border="0" src="../imagenes/paginas/add_up.png" /> 
                                                </a> 
                                            </AlternatingItemTemplate> 
                                            </asp:TemplateField>                                      
                                                        <asp:BoundField DataField="DEPENDENCIA_ID" 
                                                            Visible="true">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" ForeColor="Transparent" Font-Size="0px"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NO_TURNO" HeaderText="NO_TURNO" Visible="false" >
                                                            <FooterStyle HorizontalAlign="Left" />
                                                           
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CAJA_ID" HeaderText="CAJA_ID" Visible="false" >
                                                            <FooterStyle HorizontalAlign="Left" />

                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia" Visible="true" >
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left"/>
                                                            <ItemStyle HorizontalAlign="Left"/>
                                                        </asp:BoundField>
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                                    </td>
                                                </tr> 
                                                <tr>
                                                    <td colspan="100%">
                                                        <div id="div<%# Eval("DEPENDENCIA_ID") %>" style="display:none;position:relative;left:25px;" >                                                     
                                                            <asp:GridView ID="Grid_Sub_Detalle_Pago" runat="server" Width="99%"
                                                                AutoGenerateColumns="False" GridLines="None" CssClass="GridView_Nested"
                                                                 HeaderStyle-CssClass="tblHead">
                                                                 <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                                 <PagerStyle CssClass="GridHeader_Nested" />
                                                                 <HeaderStyle CssClass="GridHeader_Nested" />
                                                                 <AlternatingRowStyle CssClass="GridAltItem_Nested" /> 
                                                                <Columns>
                                                                    <asp:BoundField DataField="DEPENDENCIA_ID" 
                                                                        Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="0%" ForeColor="Transparent" Font-Size="0px"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true" >
                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" Visible="true" >
                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MONTO" HeaderText="Monto" Visible="true" >
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" Width="35%" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="35%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="NO_TURNO" HeaderText="No_Turno" Visible="false" >
                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CAJA_ID" HeaderText="CAJA_ID" Visible="false" >
                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    
                                                                    
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                         </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                Total<asp:TextBox ID="Txt_Total_Detalles" Text="0.0" runat="server" Width="20%" Enabled="false" style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1" ForeColor="Red" ></asp:TextBox>
                                &nbsp;&nbsp;
                        </tr>
                        <tr align="right" class="barra_delgada">
                                   <td align="center" colspan="4">
                                   </td>
                        </tr>
                        <tr>
                            <td colspan="4"> 
                            <table width="100%">
                                <tr>
                                    <td align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                        Total Efectivo
                                        <asp:TextBox ID="Txt_Total_Efectivo" Text="0.0" runat="server" Width="20%" 
                                            style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td align="right" align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                        Total Bancos
                                        <asp:TextBox ID="Txt_Total_Bancos" Text="0.0" runat="server" Width="20%"
                                            style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                        Total Cheques
                                        <asp:TextBox ID="Txt_Total_Cheques" Text="0.0" runat="server" Width="20%"
                                            style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td align="right" align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                        Total Transferencia
                                        <asp:TextBox ID="Txt_Total_Transferencia" Text="0.0" runat="server" Width="20%" Enabled="false" 
                                            style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="font-family:Tahoma; font-size: 13px; font-weight:bold;">
                                        Monto Total
                                        <asp:TextBox ID="Txt_Monto_Total" runat="server" Text="0.0" Width="20%" Enabled="false" 
                                            style="font-weight:bolder; font-size:medium; text-align:right;"  BorderWidth="1" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr align="right" class="barra_delgada">
                                           <td align="center" colspan="4">
                                           </td>
                                </tr>
                        </table> 
                        </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">  
                                <div id="Div_Denominaciones" style="width:48%;" visible="false" >
                                    <div id="Div_Prestamo_Datos_Empleado" style="width:98%; color:White; font-family:Comic Sans MS; font-size: 20px; font-weight:bold; text-align: center; background:#2F4E7D; vertical-align:middle;">
                                        Denominaciones                                  
                                    </div>
                                    
                                    <table style="width:98%;">
                                        <tr>
                                            <td style="width:50%; text-align:right;" align="right">
                                                <asp:Panel ID="Pnl_Monedas" runat="server" GroupingText="Moneda" Width="98%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width:100%;" colspan="6">
                                                                <hr />
                                                            </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 10
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_10_Pesos" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;"
                                                                    onkeyup="javascript:Sumar();" title="10"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_10_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_10_Pesos" FilterType="Numbers"/>  
                                                            </td>
                                                        </tr>
                                                        <tr>                                            
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 5
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_5_Pesos" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;;" 
                                                                    onkeyup="javascript:Sumar();" title="5"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_5_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_5_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr> 
                                                       <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 2
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_2_Pesos" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;"
                                                                    onkeyup="javascript:Sumar();" title="2"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_2_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_2_Pesos" FilterType="Numbers"/>
                                                            </td>
                                                        </tr>  
                                                       <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 1
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_1_Peso" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;"
                                                                    onkeyup="javascript:Sumar();" title="1"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_1_Peso" runat="server"
                                                                    TargetControlID="Txt_Denom_1_Peso" FilterType="Numbers"/>
                                                            </td>
                                                        </tr>  
                                                       <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 0.50
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_50_Cent" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;"
                                                                    onkeyup="javascript:Sumar();" title="0.50"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_50_Cent" runat="server"
                                                                    TargetControlID="Txt_Denom_50_Cent" FilterType="Numbers"/>
                                                            </td>
                                                        </tr>  
                                                       <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 0.20
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_20_Cent" runat="server" Text="0" Style="width: 40px; text-align: center; cursor:default;"
                                                                    onkeyup="javascript:Sumar();"  title="0.20"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_20_Cent" runat="server"
                                                                    TargetControlID="Txt_Denom_20_Cent" FilterType="Numbers"/>                                                  
                                                            </td>
                                                        </tr>             
                                                       <tr>
                                                            <td class="button_autorizar"  style="text-align:left; width:40%; cursor:default;">
                                                                $ 0.10
                                                            </td>
                                                            <td class="button_autorizar"  style="text-align:left; width:60%; cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_10_Cent" runat="server" Text="0" Style="width: 40px; text-align: center; border-width:1px; cursor:default;"
                                                                    onkeyup="javascript:Sumar();" title="0.10"/>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Denom_10_Cent" runat="server"
                                                                    TargetControlID="Txt_Denom_10_Cent" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>    
                                                        <tr>
                                                            <td style="width:100%;" colspan="6">
                                                                <hr />
                                                            </td>
                                                        </tr>                                                                                                                                                                                                               
                                                    </table>
                                                </asp:Panel>                                            
                                            </td>
                                            <td style="width:50%; text-align:left; vertical-align:top;" align="left">
                                                <asp:Panel ID="Pnl_Billetes" runat="server" GroupingText="Billetes" Width="98%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width:100%;" colspan="6">
                                                                <hr />
                                                            </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default; font-size:10px;">
                                                                $ 1000
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_1000_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="1000"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_1000_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_1000_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default;">
                                                                $ 500
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_500_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="500"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_500_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_500_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default;">
                                                                $ 200
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_200_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="200"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_200_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_200_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>                                                        
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default;">
                                                                $ 100
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_100_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="100"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_100_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_100_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>                                                        
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default;">
                                                                $ 50
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_50_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="50"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_50_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_50_Pesos" FilterType="Numbers"/> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="button_autorizar" style="width: 40%; text-align: left; cursor:default;">
                                                                $ 20
                                                            </td>
                                                            <td class="button_autorizar" style="width: 60%; text-align: left;cursor:default;">
                                                                <asp:TextBox ID="Txt_Denom_20_Pesos" runat="server" Text="0" Style="width: 40px;
                                                                    text-align: center; cursor:default;" onkeyup="javascript:Sumar();" title="20"/>
                                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Denom_20_Pesos" runat="server"
                                                                    TargetControlID="Txt_Denom_20_Pesos" FilterType="Numbers"/>  
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:100%;" colspan="6">
                                                                <hr />
                                                            </td>
                                                        </tr>   
                                                    </table>
                                                </asp:Panel>
                                            </td>                                            
                                        </tr>
                                    </table>
                                </div>
                                
                                <div style="text-align:right;">
                                   <asp:ImageButton ID="Btn_Limpiar" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                        ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Denominación"/> 
                                   &nbsp;&nbsp;&nbsp;&nbsp;
                                </div>                                
                                
                                <table width="48%" class="estilo_fuente">                
                                    <tr>
                                        <td style="width:98%; color:White; font-family:Comic Sans MS; font-size: 25px; text-align: right; background:#2F4E7D ; vertical-align:middle; font-weight:bold;">
                                            Total $ <asp:TextBox ID="Txt_Total" runat="server" style="border-style:solid; border-width:2px; height:30px; font-family:Comic Sans MS; font-size:25px; font-weight:bold; width:150px; text-align:center;background:#2F4E7D url(../imagenes/paginas/titleBackground.png) repeat-x top;color:Yellow;border-color:White;"/>
                                        </td>
                                    </tr>
                                </table>                
                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">

                            </td>
                        </tr>
                    </table>
                 
                </div>
                </td>
            
            </tr>
            </table>
                
                <br /><br /><br />
                </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

