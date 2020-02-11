<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Ver_Registros_Proveedores.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Ver_Registros_Proveedores" Title="Autorizar Pago de Proveedor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
function pageLoad(){
   $('input[id$=Txt_Buscar_Empleado_No_Empleado]').live("blur", function(){
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
}    
    
function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }     
</script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScrMng_Ver_Registros_Proveedores" runat="server"/>

<asp:UpdatePanel ID="UPnl_Ver_Registros_Proveedores" runat="server" UpdateMode="Conditional">
  <ContentTemplate>  
       
       <asp:UpdateProgress ID="Uprg_Ver_Registros_Proveedores" runat="server" AssociatedUpdatePanelID="UPnl_Ver_Registros_Proveedores" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>                     
        </asp:UpdateProgress>  
        
        <center>
        <table width="95%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr align="center">
                <td class="label_titulo" >
                   <table style="width:98%;">
                        <tr>
                            <td style="height:55px;background-image:url(../imagenes/paginas/Escudo_icon.jpg); background-repeat: no-repeat; background-position:left;width:20%; padding-left:-200px;">
                            
                            </td>
                            <td style="width:60%;">
                                <span style="font-family:Arial; font-size:larger; font-weight:bold; font-style:italic; vertical-align:bottom;">Consulta y Autorizaci&oacute;n de Cr&eacute;ditos FONACOT</span>
                            </td>
                            <td style="height:55px;background-image:url(../imagenes/paginas/Fonacot_V1.png); background-repeat: no-repeat; background-position:right;width:20%;">
                                
                            </td>                                                                
                        </tr>
                   </table>
                </td>
            </tr>
        </table>           
        
        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td>
                  <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                    <table style="width:100%;">
                      <tr>
                        <td colspan="2" align="left">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Width="24px" Height="24px"/>
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
        </table>   
            
        <asp:Panel ID="Pnl_Ver_Registros_Proveedores" runat="server" HorizontalAlign="Left" Width="95%"
            style="border-style:none;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">    
                                                           
                <div style="cursor:default;width:100%">
                    <table width="100%" style="background-color:#ffffff;"> 
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr id="Tr_Periodos_Fiscales" runat="server">
                            <td colspan="4" style="width:100%">
                                <table width="100%">
                                    <tr>
                                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                                            *Nomina
                                        </td>
                                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true"
                                                onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                        </td>             
                                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                                            &nbsp;&nbsp;*Periodo
                                        </td>
                                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server"
                                                Width="100%" />
                                        </td>                                                                                        
                                    </tr>
                                </table>                   
                            </td>
                        </tr>                                               
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Buscar_Empleado" runat="server" Text="Buscar Empleado"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:TextBox ID="Txt_Buscar_Empleado_No_Empleado" runat="server" MaxLength="6" Width="25%" OnTextChanged="Txt_Buscar_Empleado_No_Empleado_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Buscar_Empleado_No_Empleado" runat="server" TargetControlID="Txt_Buscar_Empleado_No_Empleado" WatermarkText="No. del Empleado" WatermarkCssClass="watermarked">                         
                                </cc1:TextBoxWatermarkExtender>
                                <asp:TextBox ID="Txt_Buscar_Empleado_Nombre" runat="server" Width="65%" OnTextChanged="Txt_Buscar_Empleado_Nombre_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Buscar_Empleado_Nombre" runat="server" TargetControlID="Txt_Buscar_Empleado_Nombre" WatermarkText="Nombre del Empleado" WatermarkCssClass="watermarked">                         
                                </cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" style="cursor:hand;"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Buscar_Empleado_Click" />
                            </td>
                        </tr>
                       <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Seleccionar_Empleado" runat="server" Text="Empleado"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:DropDownList ID="Cmb_Seleccionar_Empleado" runat="server" Width="100%">
                                    <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>                                                     
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>   
                        <tr>
                            <td colspan="4" style="text-align:right;" >
                                <asp:Button ID="Btn_Cargar_Datos" runat="server" Text=" --> Dar Click para cargar descuentos aplicar en el periodo indicado <-- " 
                                    Width="99.5%" CssClass="button_autorizar" 
                                    style=" color:Black; border-style:outset;background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right;" 
                                    onclick="Btn_Cargar_Datos_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width:100%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width:100%">
                                <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Detalles_Proveedores" runat="server" 
                                    OnRowDataBound="Grid_Detalles_Proveedoresr_RowDataBound"
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Both">
                                        <Columns>
                                            <asp:BoundField DataField="NO_MOVIMIENTO_DETALLE" HeaderText="NO_MOVIMIENTO_DETALLE">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="0%" Font-Size="XX-Small" Font-Bold="true"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="ESTATUS">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="15%" Font-Size="XX-Small" Font-Bold="true"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROVEEDOR" HeaderText="Proveedor">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="25%" Font-Size="XX-Small" Font-Bold="true"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Deducción" >
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="25%" Font-Size="XX-Small" Font-Bold="true"/>
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="NO_CREDITO" HeaderText="No Credito">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15%" Font-Size="11px" Font-Bold="true"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15%" Font-Size="11px" Font-Bold="true"/>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Autorizar">
                                                <ItemTemplate>
                                                    <asp:Button ID="Btn_Autorizar" runat="server" 
                                                        onmouseout="this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';" 
                                                        onmouseover="this.style.backgroundColor='#FFFFCC';this.style.cursor='hand';this.style.color='DarkBlue';this.style.borderStyle='none';this.style.borderColor='Silver';" 
                                                        style="border-style:none;background-color:#FFFFFF" Text="Autorizar" 
                                                        onclick="Btn_Autorizar_Click" />
                                                    <asp:Button ID="Btn_Rechazar" runat="server" 
                                                        onmouseout="this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';" 
                                                        onmouseover="this.style.backgroundColor='#FFFFCC';this.style.cursor='hand';this.style.color='DarkBlue';this.style.borderStyle='none';this.style.borderColor='Silver';" 
                                                        style="border-style:none;background-color:#FFFFFF;" Text="Cancelar" 
                                                        onclick="Btn_Rechazar_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="25%" Font-Size="XX-Small"/>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </td>
                            
                        </tr>
                       <tr>
                            <td style="width:100%" colspan="4">
                                <hr />
                            </td>
                        </tr>      
                       <tr>
                            <td style="width:100%" colspan="4">
                                <asp:Label ID="Lbl_Autorizar" runat="server" />
                            </td>
                        </tr>  
                    </table>          
                </div>                 
            </asp:Panel>  
            </center>
       </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>

