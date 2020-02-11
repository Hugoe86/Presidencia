<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Frm_Rpt_Alm_Consumo_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Rpt_Alm_Consumo_Stock" Title="Reporte Consumo Stock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript">
    function pageLoad() {
        $("input[id$=Btn_Exportar]").bind("click", function() {
            Exportar_Tabla_Excel($('table[id$=Grid_Consumo_Stock]').clone().get(0), 'Consumo Stock');
        });
    }
    /// <summary>
    /// Nombre: Exportar_Tabla_Excel
    /// 
    /// Descripción: Método que exporta una tabla HTML a excel.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete
    /// Fecha Creo: 25 Noviembre 2013 18:38 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    var Exportar_Tabla_Excel = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
    , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
    , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            $(table).find('.datagrid-view1').remove();
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))
        }
    })();    
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="Sm_Consultar_Consumo_Stock" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />

    <asp:UpdatePanel ID="Upd_Consumo_Stock" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Consumo_Stock" runat="server" AssociatedUpdatePanelID="Upd_Consumo_Stock" DisplayAfter="0">
               <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif"/></div>
                </ProgressTemplate>                    
            </asp:UpdateProgress> 
            
            <div id="Div_Consumo_Stock" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Listado de Consumo de Stock</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="4" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td colspan="3">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%;">
                            &nbsp;
                        </td>
                        <td align="right" style="width:50%;">
                            &nbsp;
                        </td> 
                    </tr>                                     
                </table>  
                <br /> 
                <table width="98%" class="estilo_fuente">                                                                    
                    <tr>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="80%" MaxLength="20"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Inicial" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" Enabled="True" ClearMaskOnLostFocus="false"/>
                            <cc1:MaskedEditValidator  
                            ID="Mev_Mee_Txt_Fecha_Inicial" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Inicial"
                            ControlExtender="Mee_Txt_Fecha_Inicial" 
                            EmptyValueMessage="La Fecha Inicial es obligatoria"
                             InvalidValueMessage="Fecha Inicial Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="80%" MaxLength="20"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Final_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Final" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Final" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" Enabled="True" ClearMaskOnLostFocus="false"/>
                            <cc1:MaskedEditValidator  
                            ID="Mev_Txt_Fecha_Final" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Final"
                            ControlExtender="Mee_Txt_Fecha_Final" 
                            EmptyValueMessage="La Fecha Final es obligatoria"
                            InvalidValueMessage="Fecha Final Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                        </td>
                    </tr>    
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Departamento" runat="server" Text="Departamento"/>
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:DropDownList ID="Cmb_Departamentos" runat="server" Width="100%" />
                        </td>
                    </tr>                                                                   
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Productos" runat="server" Text="Productos"/>
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:DropDownList ID="Cmb_Productos" runat="server" Width="100%" />
                        </td>
                    </tr> 
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Partidas" runat="server" Text="Partidas"/>
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:DropDownList ID="Cmb_Partidas" runat="server" Width="100%" />
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="4" style="text-align:center;text-align:right;">
                            <asp:Button ID="Btn_Consultar_Consumo_Stock" Text="Consultar" style="
                                border-style:ridge; 
                                background-color:Window;
                                cursor:pointer;
                                font-family:Consolas;
                                font-size:small;
                                font-weight:bold;
                                width:200px;
                                "
                                title="Consultar el consumo de stock"
                                OnClick="Btn_Consultar_Consumo_Stock_Click"
                                runat="server"
                                />
                            <asp:Button ID="Btn_Exportar" Text="Exportar Tabla" style="
                                border-style:ridge; 
                                background-color:Window;
                                cursor:pointer;
                                font-family:Consolas;
                                font-size:small;
                                font-weight:bold;
                                width:200px;
                                "
                                title="Exportar a excel el listado de consumo de stock"
                                runat="server"
                                />
                            <asp:Button ID="Btn_Reporte_Excel" Text="Reporte" style="
                                border-style:ridge; 
                                background-color:Window;
                                cursor:pointer;
                                font-family:Consolas;
                                font-size:small;
                                font-weight:bold;
                                width:200px;
                                "
                                title="Exportar a excel el listado de consumo de stock"
                                runat="server"
                                OnClick="Btn_Reporte_Excel_Click"
                                />
                        </td> 
                    <tr>
                        <td colspan="4" style="text-align:center;text-align:right;">
                            <div Id="_Div_Consumo_Stock" runat="server" style="overflow:auto;height:400px;width:100%;vertical-align:top" >
                                <asp:GridView ID="Grid_Consumo_Stock" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False"  GridLines="Both" EmptyDataText ="No se encontraron registros en la búsqueda">
                                         <Columns>
                                             <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                 <HeaderStyle HorizontalAlign="Left" Width="40px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="40px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Producto" HeaderText="Producto">
                                                 <HeaderStyle HorizontalAlign="Left" Width="150px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="150px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>                                         
                                             <asp:BoundField DataField="Unidad" HeaderText="Unidad">
                                                 <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Center" Width="60px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Codigo_Programatico" HeaderText="Codigo Programatico">
                                                 <HeaderStyle HorizontalAlign="Left" Width="150px" Font-Size="XX-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="150px" Font-Size="XX-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Departamento" HeaderText="Departamento" >
                                                 <HeaderStyle HorizontalAlign="Left" Width="200px" Font-Size="XX-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="200px" Font-Size="XX-Small" Font-Bold="true"/>
                                             </asp:BoundField>                                                                                                                           
                                             <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                                                  <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                                  <ItemStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:c}">
                                                  <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Size="X-Small" Font-Bold="true"/>
                                                  <ItemStyle HorizontalAlign="Center" Width="80px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>                                           
                                         </Columns>
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <HeaderStyle CssClass="GridHeader" />
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>                                                                       
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Reporte_Excel"/>
        </Triggers>
    </asp:UpdatePanel>           
</asp:Content>

