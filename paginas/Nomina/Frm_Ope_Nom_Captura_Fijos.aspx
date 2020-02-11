<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Captura_Fijos.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Captura_Fijos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Captura de Fijos</title>
<link href="../estilos/TabContainer.css" rel="stylesheet" type="text/css" />
<link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
<link href="../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />

<script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
<script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="../../easyui/ddaccordion.js" type="text/javascript"></script>
<script src="../../easyui/jquery_jclock.js" type="text/javascript"></script>
<script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
<script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>        
    
 <script type="text/javascript" language="javascript">
    function calendarShown(sender, args){
        sender._popupBehavior._element.style.zIndex = 10000005;
    }

    function Abrir_Modal_Popup() {
        $find('Busqueda_Empleados').show();
        return false;
    } 
</script>

<script type="text/javascript" language="javascript">
function pageLoad(){
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
    
    $('table[id$=Grid_Tipo_Nomina_Deducciones] tbody > tr > td :input').keyup(function(){
            var clase =  "input[title=" + $(this).attr('title') + "]";
            
            if(clase != undefined){
                if(clase != 'input[title=]'){
                    $(clase).val($(this).val()); 
                }   
            }
            
         $(this).keyup(function(){
            $(this).removeClass('CajaTextoDefault');
            $(this).addClass('CajaTextoCantidad');
            $(this).css('background-color', '#FFFF99');
         });
         
         $(this).blur(function(){
            $(this).removeClass('CajaTextoCantidad');
            $(this).addClass('CajaTextoDefault');
            $(this).css('background-color', 'White');
         });
    });    
    
    $('table[id$=Grid_Tipo_Nomina_Percepciones] tbody > tr > td :input').bind("keyup click", function(){         
         $(this).click(function(){ $(this).focus().select(); });
         
         $(this).keyup(function(){
            $(this).removeClass('CajaTextoDefault');
            $(this).addClass('CajaTextoCantidad');
            $(this).css('background-color', '#FFFF99');
         });
         
         $(this).blur(function(){
            $(this).removeClass('CajaTextoCantidad');
            $(this).addClass('CajaTextoDefault');
            $(this).css('background-color', 'White');
         });
    });      
}    
    
function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }   
</script> 
<style type="text/css">
.GridView_Nested
{
    font-family: Arial, Sans-Serif;
    font-size:small;
    table-layout: inherit;
    border-collapse: collapse;
    border:#666 2px solid;
}
.GridHeader_Nested
 {
    background-image: url(../imagenes/gridview/HeaderChrome.jpg);
    background-position:center;
    background-repeat:repeat-x;
    background-color:#1d1d1d;
	border-bottom: #1d1d1d 1px solid;
	color:White;
 }

.GridAltItem_Nested
{
 background-color: #f0f0f0;
}

.GridSelected_Nested
{
 background-color: #ffff66;
}
.GridItem_Nested
{
 background-color: #c9c9c9;
}  
</style>
</head>
<body>
    <form id="form1" runat="server"  method="post" >
    <div>
    <asp:ScriptManager ID="Sm_Aplicar_Conceptos_Fijos" runat="server"/>
    <asp:UpdatePanel ID="Upnl_Aplicar_Conceptos_Fijos" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Upgrs_Aplicar_Conceptos_Fijos" runat="server" AssociatedUpdatePanelID="Upnl_Aplicar_Conceptos_Fijos" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Sias_Roler.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress> 
        
            <div id="Div_Conceptos_Fijos" style="background-color:#ffffff; width:100%; height:100%;font-size:10px;" >
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Percepciones Y Deducciones Fijas
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" >&nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>                         
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                                </ContentTemplate>                                
                            </asp:UpdatePanel>                                      
                        </td>
                    </tr> 
                </table>
                
                <table width="98%">
                         <tr align="center">
                             <td colspan="2">                
                                 <div align="right" class="barra_busqueda">                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                            <td align="left" style="width:59%;">
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>                                               
                                            </td>
                                            <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="width:100%;vertical-align:top;" align="right">
                                                            <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada" TabIndex="23" 
                                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
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
                
                <cc1:TabContainer ID="Contenedor_Datos_Personales" runat="server" Width="97%" 
                    ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel ID="Tab_Percepciones_Tipo_Nomina" HeaderText="Percepciones" runat="server">
                        <HeaderTemplate>Percepciones</HeaderTemplate>
                        <ContentTemplate>
                            <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >  
                                <center>                              
                                <asp:GridView ID="Grid_Tipo_Nomina_Percepciones" runat="server" 
                                     AutoGenerateColumns="False"  GridLines="Both" CssClass="GridView_Nested"
                                     OnRowDataBound="Grid_Tipo_Nomina_Percepciones_RowDataBound" Width="880px">
                                     
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <HeaderStyle CssClass="GridHeader_Nested" />
                                        
                                        <Columns>                                              
                                            <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Clave">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                <HeaderStyle HorizontalAlign="Left" Width="400px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="400px" Font-Size="11px" Font-Bold="true"/>
                                            </asp:BoundField>                 
                                            <asp:BoundField DataField="TIPO_ASIGNACION"  ControlStyle-CssClass="Ocultar">
                                                <HeaderStyle HorizontalAlign="Left" Width="0px" Font-Size="0px" Font-Bold="true" BorderStyle="None" CssClass="Ocultar"/>
                                                <ItemStyle HorizontalAlign="Left" Width="0px" Font-Size="0px" Font-Bold="true" BorderStyle="None" CssClass="Ocultar"/>
                                            </asp:BoundField>                                             
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="$ Tipo Nomina" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>                                             
                                           <asp:TemplateField HeaderText="Percepción Catorcenal">
                                                <ItemTemplate>
                                                      <asp:TextBox ID="Txt_Cantidad_Percepcion" runat="server" CssClass="CajaTextoDefault"  Width="90%" style="text-align:right;"
                                                            onblur="$('input[id$=Txt_Cantidad_Percepcion]').formatCurrency();"/>                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="150px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Center" Width="150px" />                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField ControlStyle-CssClass="Ocultar">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Cantidad_Importe" runat="server" Width="90%" style="text-align:right; display:none;"
                                                        onblur="$('input[id$=Txt_Cantidad_Importe]').formatCurrency();"/>                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>
                                                <ItemStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField ControlStyle-CssClass="Ocultar">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Saldo" runat="server" Width="90%" style="text-align:right; display:none;"
                                                        onblur="$('input[id$=Txt_Saldo]').formatCurrency();"/>                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>
                                                <ItemStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField ControlStyle-CssClass="Ocultar">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Cantidad_Retenida" runat="server" Width="90%" style="text-align:right; display:none;"
                                                        onblur="$('input[id$=Txt_Cantidad_Retenida]').formatCurrency();" />                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>
                                                <ItemStyle HorizontalAlign="Left" Width="0px" CssClass="Ocultar"/>                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Calendario Nómina">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width:50%;text-align:left;">
                                                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="150px" AutoPostBack="true" 
                                                                    TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_P_SelectedIndexChanged" />
                                                            </td>             
                                                            <td style="width:50%;text-align:left;">
                                                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                                                    Width="150" TabIndex="6" />
                                                            </td>                                                                                        
                                                        </tr>
                                                    </table>                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="250px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />                                                                                   
                                           </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView> 
                                    </center>
                             </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>       
                    <cc1:TabPanel ID="Tab_Deducciones_Tipo_Nomina" HeaderText="Deducciones" runat="server">
                        <HeaderTemplate>Deducciones</HeaderTemplate>
                        <ContentTemplate>
                            <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;color:White;" >
                                <center>
                                <asp:GridView ID="Grid_Tipo_Nomina_Deducciones" runat="server" CssClass="GridView_Nested"
                                     AutoGenerateColumns="False"  GridLines="None" 
                                     OnRowDataBound="Grid_Tipo_Nomina_Deduccion_RowDataBound" Width="1200px">

                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <HeaderStyle CssClass="GridHeader_Nested" />

                                        <Columns>                                              
                                            <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Clave">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                <HeaderStyle HorizontalAlign="Left" Width="600px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="600px" Font-Size="11px" Font-Bold="true"/>
                                            </asp:BoundField>                 
                                            <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo">
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                            </asp:BoundField>                                             
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Retención Catorcenal" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>                                             
                                           <asp:TemplateField HeaderText="$ Empleado">
                                                <ItemTemplate>
                                                      <asp:TextBox ID="Txt_Cantidad_Deduccion" runat="server" Width="90%" style="text-align:right;"
                                                            onblur="$('input[id$=Txt_Cantidad_Deduccion]').formatCurrency();" />                                                                                                                                                                                                                            
                                                </ItemTemplate> 
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="$ Importe">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Cantidad_Importe" runat="server" Width="90%" style="text-align:right;"
                                                        onblur="$('input[id$=Txt_Cantidad_Importe]').formatCurrency();" 
                                                        title='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'/>                                                                                                                                                                                                                          
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="$ Saldo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Saldo" runat="server" Width="90%" style="text-align:right;"
                                                        onblur="$('input[id$=Txt_Saldo]').formatCurrency();" title='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'/>                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="$ Abonada">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Cantidad_Retenida" runat="server" Width="90%" style="text-align:right;"
                                                        onblur="$('input[id$=Txt_Cantidad_Retenida]').formatCurrency();"/>                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />                                                                                   
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Calendario Nómina">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width:50%;text-align:left;">
                                                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="150px" AutoPostBack="true" 
                                                                    TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_D_SelectedIndexChanged" />
                                                            </td>             
                                                            <td style="width:50%;text-align:left;">
                                                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                                                    Width="150" TabIndex="6" />
                                                            </td>                                                                                        
                                                        </tr>
                                                    </table>                                                                                             
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="250px" Font-Size="11px" Font-Bold="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />                                                                                   
                                           </asp:TemplateField>                                                                                                                                                                                                                                                          
                                        </Columns>
                                    </asp:GridView>
                                    </center>              
                            </div>                  
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                
                <input type="button" id="Btn_Ocultar_Empleados" value="Ocultar" onclick="$('#Contenedor').toggle(1000);" 
                    class="button_autorizar" style="width:97%;border-style:outset;"/>
                
                <div id="Contenedor">
                    <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="True" CssClass="GridView_Nested"
                        AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="96.5%"
                        onpageindexchanging="Grid_Empleados_PageIndexChanging"  
                        onselectedindexchanged="Grid_Empleados_SelectedIndexChanged"
                        AllowSorting="True" OnSorting="Grid_Empleados_Sorting">
                        
                         <SelectedRowStyle CssClass="GridSelected_Nested" />
                         <PagerStyle CssClass="GridHeader_Nested" />
                         <HeaderStyle CssClass="GridHeader_Nested" />
                         <AlternatingRowStyle CssClass="GridAltItem_Nested" /> 
                         
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="5%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID" 
                                Visible="True" SortExpression="Empleado_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Empleado" HeaderText="Número Empleado" 
                                Visible="True" SortExpression="No_Empleado">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EMPLEADO" HeaderText="Nombre" 
                                Visible="True" SortExpression="Empleado">
                                <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                <ItemStyle HorizontalAlign="left" Width="75%" />
                            </asp:BoundField>                                    
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Empleados" EventName="Click"/>
        </Triggers>
    </asp:UpdatePanel>
    
 <asp:UpdatePanel ID="Upnal_Busqueda_Empleado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
            CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
        <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
        <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />                                                                                                    
    </ContentTemplate>
 </asp:UpdatePanel>

 <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;color:White;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:$find('Busqueda_Empleados').hide(); return false;"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">  
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
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" AutoPostBack="true"
                                                OnTextChanged="Txt_Busqueda_No_Empleado_TextChanged"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">                                            
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">                                         
                                        </td>                                         
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" AutoPostBack="true"
                                                OnTextChanged="Txt_Busqueda_No_Empleado_TextChanged"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" 
                                            FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ _-Ññ"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>                                                                                               
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
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="100%"
                                                style="background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right; cursor:hand;"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>                                                                   
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>                                                      
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>
    </div>
    </form>
</body>
</html>
