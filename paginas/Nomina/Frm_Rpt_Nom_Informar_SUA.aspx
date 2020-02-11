<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Informar_SUA.aspx.cs" 
Inherits="paginas_Nomina_Frm_Rpt_Nom_Informar_SUA" Title="Reporte SUA" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
function Limpiar_Controles(){

    $('input[id$=Txt_Busqueda_No_Empleado]').val('');
    $('input[id$=Txt_Busqueda_RFC]').val('');
    $('input[id$=Txt_Busqueda_Nombre_Empleado]').val('');
    $('select[id$=Cmb_Busqueda_Dependencia]').val('');
    $('table[id$=Grid_Busqueda_Empleados]  tr:not(:first)').remove();
    
    return false;
}
</script>

<script type="text/javascript" language="javascript">
function pageLoad(){
     $('input[id$=Txt_Busqueda_No_Empleado]').css('background-image', 'url(../imagenes/paginas/empleado.png)');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('background-position', 'left');    
     $('input[id$=Txt_Busqueda_No_Empleado]').css('border-style', 'Solid');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('font-family', 'Tahoma');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('font-size', '12px');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('font-weight', 'bold');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('color', '#2F4E7D');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('height', '24px');
     $('input[id$=Txt_Busqueda_No_Empleado]').css('text-align', 'center');   
     $('input[id$=Txt_Busqueda_No_Empleado]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Busqueda_No_Empleado]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Busqueda_No_Empleado]').blur(function(){$(this).css('background-color', 'White');});
     
     $('input[id$=Txt_Registro_Patronal]').css('background-image', 'url(../imagenes/paginas/imss.bmp)');
     $('input[id$=Txt_Registro_Patronal]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Registro_Patronal]').css('background-position', 'left');    
     $('input[id$=Txt_Registro_Patronal]').css('border-style', 'Solid');
     $('input[id$=Txt_Registro_Patronal]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Registro_Patronal]').css('font-family', 'Tahoma');
     $('input[id$=Txt_Registro_Patronal]').css('font-size', '12px');
     $('input[id$=Txt_Registro_Patronal]').css('font-weight', 'bold');
     $('input[id$=Txt_Registro_Patronal]').css('color', '#2F4E7D');
     $('input[id$=Txt_Registro_Patronal]').css('height', '24px');
     $('input[id$=Txt_Registro_Patronal]').css('text-align', 'center');   
     $('input[id$=Txt_Registro_Patronal]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Registro_Patronal]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Registro_Patronal]').blur(function(){$(this).css('background-color', 'White');});     

   $('input[id$=Txt_Busqueda_No_Empleado]').live("blur", function(){
        if(isNumber($(this).val())){
            var Ceros = "";
            if($(this).val() != undefined){
                if($(this).val() != ''){
                    for(i=0; i<(6-$(this).val().length); i++){
                        Ceros += '0';
                    }
                    $(this).val(Ceros + $(this).val());
                    Ceros = "";
                }else $(this).val('');
            }
        }
    }); 
    
   $('input[id$=Btn_Busqueda_Empleados]').hover(
     function(e){
         e.preventDefault();
         $(this).css("background-color", "#2F4E7D");
         $(this).css("color", "#FFFFFF");
     },
    function(e){
         e.preventDefault();
             $(this).css("background-color", "#f5f5f5");
             $(this).css("color", "#565656");
    }
   );
}    
    
function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }   
</script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="Sm_Reporte_Presidencia_SUA" runat="server"/>
    <asp:UpdatePanel ID="UPnl_Reporte_Presidencia_SUA" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="UPgrs_Reporte_Presidencia_SUA" runat="server" AssociatedUpdatePanelID="UPnl_Reporte_Presidencia_SUA" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenedor" style="width:98%; background-color:White;">
                <table width="100%" title="Control_Errores"> 
                    <tr>
                        <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                            Reporte para el Sistema SUA
                        </td>               
                    </tr>            
                    <tr>
                        <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>               
                    </tr>   
                </table> 
            
                  <table width="100%">
                   <tr>
                        <td style="width:100%" colspan="4" align="right">                                                                    
                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Controles();"
                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/> 
                                
                            <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                          OnClick="Btn_Generar_Reporte_Excel_Click" ToolTip="Generar Reporte en EXCEL" Width="28px" Height="28px" style="cursor:hand;"/>
                                 </ContentTemplate>
                            </asp:UpdatePanel>                                                        
                        </td>
                    </tr>     
                   <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>   
                    <tr>
                        <td style="width:20%;text-align:left;font-size:11px;">
                           No Empleado 
                        </td>              
                        <td style="width:30%;text-align:left;font-size:11px;">
                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" />
                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers" TargetControlID="Txt_Busqueda_No_Empleado"/>
                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="No Empleado" 
                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                        </td> 
                        <td style="width:20%;text-align:left;font-size:11px;">
                            
                        </td>              
                        <td style="width:30%;text-align:left;font-size:11px;">
                                                                                                                                                              
                        </td>                               
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;font-size:11px;">
                           RFC
                        </td>              
                        <td style="width:30%;text-align:left;font-size:11px;">
                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" />
                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters"
                                TargetControlID="Txt_Busqueda_RFC"/>  
                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC" 
                                WatermarkCssClass="watermarked"/>                                                                                                            
                        </td> 
                        <td style="width:20%;text-align:left;font-size:11px;">
                            &nbsp;&nbsp;&nbsp;Registro Patronal
                        </td>              
                        <td style="width:30%;text-align:left;font-size:11px;">
                           <asp:TextBox ID="Txt_Registro_Patronal" runat="server" Width="98%" />
                           <cc1:FilteredTextBoxExtender ID="Fte_Registro_Patronal" runat="server" FilterType="Numbers, UppercaseLetters"
                                TargetControlID="Txt_Registro_Patronal"/>  
                            <cc1:TextBoxWatermarkExtender ID="Twm_Registro_Patronal" runat="server" 
                                TargetControlID ="Txt_Registro_Patronal" WatermarkText="Registro Patronal" 
                                WatermarkCssClass="watermarked"/>                                                                                                                      
                        </td>                               
                    </tr>                    
                    <tr>
                        <td style="width:20%;text-align:left;font-size:11px;">
                            Nombre
                        </td>              
                        <td style="width:30%;text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" />
                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                WatermarkCssClass="watermarked"/>                                                                                               
                        </td>                                         
                    </tr>                   
                    <tr>
                        <td style="width:20%;text-align:left;font-size:11px;">
                            Unidad Responsable
                        </td>              
                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" />                                          
                        </td> 
                    </tr>                                                                
                   <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>                                    
                    <tr>
                        <td style="width:100%;text-align:left;" colspan="4">
                            <center>
                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button_autorizar"
                                CausesValidation="false"  Width="100%" OnClick="Btn_Busqueda_Empleados_Click" style="background-image:url(../imagenes/paginas/busqueda.png); 
                                   background-repeat: no-repeat; background-position:right; cursor:hand;"/> 
                            </center>
                        </td>                                                     
                    </tr>                                                                        
                  </table>   
                  <br />
                  <div id="Div_Resultados_Busqueda" runat="server" style="border-style:outset; width:99%; height: 250px; overflow:auto;">
                      <asp:GridView ID="Grid_Busqueda_Empleados" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Both" Width="100%" 
                            PageSize="100" EmptyDataText="No se encontrarón resultados para los filtros de la busqueda" 
                   
                            >
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="Número" SortExpression="NO_EMPLEADO" >
                                    <ItemStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    <HeaderStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Empleado" HeaderText="Nombre" SortExpression="NOMBRE" NullDisplayText="-" >
                                    <ItemStyle Width="200px" Font-Size="X-Small" HorizontalAlign="Left" />
                                    <HeaderStyle Width="200px" Font-Size="X-Small" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SALARIO_DIARIO_INTEGRADO" HeaderText="SDI" NullDisplayText="-" DataFormatString="{0:c}">
                                    <ItemStyle Width="100px" Font-Size="Small" HorizontalAlign="Center" Font-Names="Courier New"/>
                                    <HeaderStyle Width="100px" Font-Size="Small" HorizontalAlign="Center" />
                                </asp:BoundField>    
                                <asp:BoundField DataField="REGISTRO_PATRONAL" HeaderText="Registro Patronal" NullDisplayText="-" >
                                    <ItemStyle Width="100px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    <HeaderStyle Width="100px" Font-Size="X-Small" HorizontalAlign="Center" />
                                </asp:BoundField>                                                                
                                <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" SortExpression="DEPENDENCIA" NullDisplayText="-" >
                                    <ItemStyle Width="200px"  Font-Size="X-Small" HorizontalAlign="Left" />
                                    <HeaderStyle Width="200px" Font-Size="X-Small" HorizontalAlign="Center" />
                                </asp:BoundField>                                
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView> 
                </div>
            </div>                
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="Btn_Generar_Reporte_Excel"/>
        </Triggers>        
    </asp:UpdatePanel>
</asp:Content>

