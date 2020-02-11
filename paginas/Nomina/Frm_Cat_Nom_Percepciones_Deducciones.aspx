<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Percepciones_Deducciones.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Percepcion_Deduccion" Title="Catálogo Percepciones y Deduciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/paginas/Paginas_Generales/Pager.ascx" TagPrefix="custom" TagName="Pager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
<style type="text/css">
   .highlightRow
   {
      cursor: pointer;
      border:solid 1px Black;
   }
</style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
    function pageLoad() { 
        $('table[id$=Grid_Percepciones_Deducciones] tbody tr').mouseover(function () {$(this).addClass('highlightRow');}).mouseout(function () {$(this).removeClass('highlightRow');});
        $('input[id$=Btn_Busqueda_Percepciones_Deducciones]').hover(function(e){e.preventDefault();$(this).css("background-color", "#2F4E7D");$(this).css("color", "#FFFFFF");},function(e){e.preventDefault();$(this).css("background-color", "#f5f5f5");$(this).css("color", "#565656");});
        Contar_Caracteres();
    }
  
    function Contar_Caracteres(){
        $('textarea[id$=Txt_Comentarios]').keyup(function() {
            var Caracteres =  $(this).val().length;
            
            if (Caracteres > 250) {
                this.value = this.value.substring(0, 250);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            }else{
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
            
            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
        });
    }
    function Abrir_Modal_Popup() {$('input[id$=Txt_Busqueda_Percepciones_Deducciones]').val('');$find('Mpe_Busqueda_Conceptos').show();return false;}
    function Cerrar_Modal_Popup() {$find('Mpe_Busqueda_Conceptos').hide();return false;}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="SM_Cat_Nom_Percepciones_Deducciones" runat="server"/>
  <asp:UpdatePanel ID="Upd_Pnl_Cat_Nom_Percepciones_Deducciones" runat="server" UpdateMode="Always">
     <ContentTemplate>
     
      <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Pnl_Cat_Nom_Percepciones_Deducciones" DisplayAfter="0">
           <ProgressTemplate>
              <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
              <div  class="processMessage" id="div_progress">
                    <img alt="" src="../imagenes/paginas/Updating.gif" />
              </div>
           </ProgressTemplate>
      </asp:UpdateProgress>     
      
     <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">     
         <center>     
            <asp:Label ID="Lbl_Titulo" runat="server" Text="Catálogo de Percepciones y Deducciones" CssClass="label_titulo" />               
         </center>       
         <br />     
          <div id="Div_Contenedor_Msj_Error" style="width:70%;font-size:9px;text-align:left;" runat="server" visible="false" >
            <table style="width:100%;">
              <tr>
                <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                    Width="24px" Height="24px"/>
                    Es Necesario Introducir:
                </td>            
              </tr>
              <tr>
                <td style="width:10%;">              
                </td>            
                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error"/>
                </td>
              </tr>          
            </table>                   
            <br />
          </div>  
          <center>
            <table width="98%"  border="0" cellspacing="0">
                 <tr align="center">
                     <td colspan="2">                
                         <div align="right" class="barra_busqueda">                        
                              <table style="width:100%;height:28px;">
                                <tr>
                                  <td align="left" style="width:59%;">  
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                       CausesValidation="false" onclick="Btn_Nuevo_Click" ImageUrl="../imagenes/paginas/icono_nuevo.png"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                      CausesValidation="false" OnClick="Btn_Modificar_Click" ImageUrl="../imagenes/paginas/icono_modificar.png"/>
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" CausesValidation="false" 
                                      OnClientClick="return confirm('¿Estas seguro de eliminar la Percepción Deducción seleccionada?');"
                                      OnClick="Btn_Eliminar_Click" ImageUrl="../imagenes/paginas/icono_eliminar.png"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir" CssClass="Img_Button"
                                      CausesValidation="false" OnClick="Btn_Salir_Click" ImageUrl="../imagenes/paginas/icono_salir.png"/>                                                                   
                                  </td>
                                  <td align="right" style="width:41%;">
                                    <table style="width:100%;height:28px;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:right;width:20%;">
                                                <asp:LinkButton ID="Btn_Iniciar_Busqueda" runat="server" OnClientClick="javascript:return Abrir_Modal_Popup();" Text="Búsqueda"/>
                                            </td>
                                            <td style="width:55%;">
                                                <asp:TextBox ID="Txt_Busqueda_Percepciones_Deducciones" runat="server" 
                                                     ToolTip="Ingrese el nombre del evento a buscar" Width="100%"
                                                     onkeyup='this.value = this.value.toUpperCase();'/>
                                                <cc1:TextBoxWatermarkExtender ID="TWM_Txt_Busqueda_Percepciones_Deducciones" runat="server" 
                                                                WatermarkCssClass="watermarked" WatermarkText ="< Ingresa el Nombre a Buscar >" 
                                                                TargetControlID="Txt_Busqueda_Percepciones_Deducciones"/>                                                                                                                                             
                                            </td>
                                            <td style="vertical-align:middle;width:5%;" >
                                                <asp:ImageButton ID="Btn_Buscar_Percepcion_Deduccion" runat="server" CausesValidation="False" 
                                                     ImageUrl="../imagenes/paginas/busqueda.png"  OnClick="Btn_Buscar_Percepcion_Deduccion_Click"
                                                     ToolTip="Consultar" Height="20px" 
                                                      />                                            
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
            <asp:Panel ID="Pnl_Datos_Generales" GroupingText="Datos Generales"  Font-Size="8px" runat="server" style="width:98%;color:Blue;font-size:9px;">              
                <table style="width:100%;">
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Percepcion_Deduccion_ID" runat="server" Text="ID"
                                Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Percepcion_Deduccion_ID" runat="server" Width="95%" Font-Size="11px"/>
                        </td>                                           
                        <td style="width:20%;text-align:left;font-size:11px;">                            
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Aplica_Calculo_IMSS" runat="server" Text="Aplica IMSS" Width="100%" 
                                            CssClass="estilo_fuente" Font-Size="11px"/>                                        
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk_Aplica_Concepto_Calculo_IMSS" runat="server" Checked="false"
                                            ToolTip="Este campo indica si el concepto de PERCEPCION aplicara para el cálculo de IMSS"/>  
                                    </td>
                                    <td>     
                                                                            
                                    </td>
                                </tr>
                            </table>                                                            
                        </td>             
                        <td style="width:20%;text-align:left;">
                           <asp:Label ID="Lbl_Clave_Concepto" runat="server" Text="*Clave" Width="20%" 
                                CssClass="estilo_fuente" Font-Size="11px"/>                          
                           <asp:TextBox ID="Txt_Clave_Concepto" runat="server" Width="73%" Font-Size="11px"/>
                           <cc1:TextBoxWatermarkExtender ID="Twm_Clave_Concepto" runat="server" 
                                TargetControlID ="Txt_Clave_Concepto" WatermarkText="< Clave Ej. P001 >" 
                                WatermarkCssClass="watermarked"/>
                        </td>                                
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" Font-Size="11px"/>
                           <cc1:TextBoxWatermarkExtender ID="Twm_Nombre" runat="server" 
                                TargetControlID ="Txt_Nombre" WatermarkText="Ingresar el nombre para la percepción o deducción" 
                                WatermarkCssClass="watermarked"/>
                            <cc1:FilteredTextBoxExtender ID="FTW_Txt_Nombre" runat="server" TargetControlID="Txt_Nombre"
                                FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚ Ñ.1234567890"/>
                        </td>                                
                    </tr>          
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Tipo" runat="server" Text="*Tipo" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>    
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Tipo_Deduccion_Percepcion" runat="server" Width="97%"  AutoPostBack="true" Font-Size="11px"
                                onselectedindexchanged="Cmb_Tipo_Deduccion_Percepcion_SelectedIndexChanged">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>PERCEPCION</asp:ListItem>
                                <asp:ListItem>DEDUCCION</asp:ListItem>
                            </asp:DropDownList>
                        </td>       
                        <td style="width:20%;text-align:left;">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Gravable" runat="server" Text="Gravable" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>                    
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk_Gravable" runat="server" AutoPostBack="true"
                                            oncheckedchanged="Chk_Gravable_CheckedChanged" />  
                                    </td>
                                    <td>
                                        <asp:Label ID="Lbl_Porcentaje_Gravable" runat="server" Text="" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>  
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Cantidad_Porcentual_Gravable" runat="server" Width="85%" Font-Size="11px" 
                                MaxLength="6" ToolTip="Porcentaje Gravable" onblur="this.value = ( parseInt(this.value)< 0 ||  parseInt(this.value) > 100)? '' : this.value ;"/>
                            <asp:Label ID="Lbl_Symbol_Porcentaje" runat="server" Text="%" Font-Bold="false" ForeColor="Black" 
                                Width="10%" CssClass="estilo_fuente"/> 
                            <cc1:TextBoxWatermarkExtender ID="TWM_Txt_Cantidad_Porcentual_Gravable" runat="server"
                                WatermarkCssClass="watermarked2" WatermarkText="NO APLICA" TargetControlID="Txt_Cantidad_Porcentual_Gravable"/>  
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Cantidad_Porcentual_Gravable"
                                FilterType="Numbers, Custom" ValidChars="."/>    
                            <asp:CustomValidator ID="Cv_Txt_Cantidad_Porcentual_Gravable" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="El Porcentaje Gravable [0 - 100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Cantidad_Porcentual_Gravable"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Cantidad_Porcentual_Gravable"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Cantidad_Porcentual_Gravable" runat="server" TargetControlID="Cv_Txt_Cantidad_Porcentual_Gravable" PopupPosition="TopRight"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Cantidad_Porcentual_Gravable(sender, args) {     
                                     var v = document.getElementById("<%=Txt_Cantidad_Porcentual_Gravable.ClientID%>").value;   
                                     if ( (v < 0) || ( v > 100) ){  
                                        document.getElementById("<%=Txt_Cantidad_Porcentual_Gravable.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                                  
                        </td>                  
                    </tr>        
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Aplicar" runat="server" Text="*Aplicar" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Aplicar" runat="server" Width="97%" Font-Size="11px">
                                <asp:ListItem>&lt; Seleccionar &gt;</asp:ListItem>
                                <asp:ListItem>TODAS</asp:ListItem>
                                <asp:ListItem>PRIMERA</asp:ListItem>
                                <asp:ListItem>SEGUNDA</asp:ListItem>
                            </asp:DropDownList>
                        </td>                    
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus"
                                Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="97%" Font-Size="11px">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>                    
                    </tr>                                       
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Asignar" runat="server" Text="*Asignar" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Asignar" runat="server" Width="97%" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Asignar_SelectedIndexChanged"/>
                        </td>                    
                        <td style="width:20%;text-align:left;">
                            <asp:Label ID="Lbl_Concepto" runat="server" Text="*Concepto" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Concepto" runat="server" Width="97%" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Concepto_SelectedIndexChanged">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>TIPO_NOMINA</asp:ListItem>
                                <asp:ListItem>SINDICATO</asp:ListItem>
                            </asp:DropDownList>                            
                        </td>                      
                    </tr>
                    
                    <tr>
                        <td style="width:20%;text-align:left;vertical-align:top;">
                            <asp:Label ID="Lbl_Cuenta_Contable" runat="server" Text="Cuenta Contable" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:80%;text-align:left;" colspan="3">
                            <asp:DropDownList ID="Cmb_Cuentas_Contables" runat="server" Width="99%" Font-Size="11px"/>                                                
                        </td>                         
                    </tr>                     
                                   
                    <tr>
                        <td style="width:20%;text-align:left;vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" Width="100%" CssClass="estilo_fuente" Font-Size="11px"/>
                        </td>
                        <td style="width:50%;text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Width="98%" Height="50px"/>
                            <cc1:FilteredTextBoxExtender ID="FT_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚÑ !¡¿?#$%&/()[]{}.:;,-_" /> 
                            <cc1:TextBoxWatermarkExtender ID="TWM_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked" 
                                WatermarkText ="< Carácteres Permitidos 250 >" TargetControlID="Txt_Comentarios"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                            
                        </td>                    
                        <td style="width:30%;text-align:center;vertical-align:top;" colspan="2">
                
                        </td>       
                    </tr>                                                  
                </table>   
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="Pnl_Tbl_Percepciones_Deducciones" Width="98%" >
                <div id="Div_Titulo" style="color:Black;width:99%;font-size:12px;font-weight:bold;text-align:center;border-style:outset;border-color:Silver;background-color:#F0F8FF;">
                    Listado de Percepciones Y Deducciones
                    <hr />
                </div>
                <br />
                <asp:Panel ID="Pnl_Border_Perc_Dedu" runat="server" Width="99%">
                     <asp:GridView ID="Grid_Percepciones_Deducciones" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Width="100%" OnSelectedIndexChanged="Grid_Percepciones_Deducciones_SelectedIndexChanged"
                            AllowPaging="true" OnPageIndexChanging="Grid_Percepciones_Deducciones_PageIndexChanging"
                             AllowSorting="True" OnSorting="Grid_Percepciones_Deducciones_Sorting" HeaderStyle-CssClass="tblHead"
                            >
                              <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="../imagenes/gridview/blue_button.png"  
                                        HeaderText="">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>                         
                              </Columns>   
			                <RowStyle CssClass="GridItem" />
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />                          
                            <AlternatingRowStyle CssClass="GridAltItem" />                                                                                                                                    
                      </asp:GridView>
                      <custom:Pager ID="custPager" runat="server" OnPageChanged="custPager_PageChanged" />
                  </asp:Panel>
            </asp:Panel>      
          </center>
          <br /><br /><br /><br />
     </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Percepciones_Deducciones" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

 <asp:UpdatePanel ID="UPnl_Ventana_Busqueda"  UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Conceptos" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Mpe_Busqueda_Conceptos"
            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
            CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
        <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
        <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
    </ContentTemplate>
 </asp:UpdatePanel>


 <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Percepciones Deducciones
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:return Cerrar_Modal_Popup();"/>
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
                                           Clave
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Clave_Percepcion_Deduccion_Busqueda" runat="server" Width="98%" 
                                                onkeyup='this.value = this.value.toUpperCase();' MaxLength="20"/>
                                           <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Clave_Percepcion_Deduccion_Busqueda" runat="server" 
                                                TargetControlID ="Txt_Clave_Percepcion_Deduccion_Busqueda" WatermarkText="< Clave Ej. P001 >" 
                                                WatermarkCssClass="watermarked"/>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:DropDownList ID="Cmb_Tipo_Busqueda" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>PERCEPCION</asp:ListItem>
                                                <asp:ListItem>DEDUCCION</asp:ListItem>
                                            </asp:DropDownList>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Nombre_Busqueda" runat="server" Width="99.5%" 
                                                onkeyup='this.value = this.value.toUpperCase();'/>
                                           <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Nombre_Busqueda" runat="server" 
                                                TargetControlID ="Txt_Nombre_Busqueda" WatermarkText="< Nombre >" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Aplica
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:DropDownList ID="Cmb_Aplica_Busqueda" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>TODAS</asp:ListItem>
                                                <asp:ListItem>PRIMERA</asp:ListItem>
                                                <asp:ListItem>SEGUNDA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Concepto
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:DropDownList ID="Cmb_Concepto_Busqueda" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>TIPO_NOMINA</asp:ListItem>
                                                <asp:ListItem>SINDICATO</asp:ListItem>
                                            </asp:DropDownList>
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
                                               <asp:Button ID="Btn_Busqueda_Percepciones_Deducciones" runat="server"  Text="Busqueda de Percepciones Deducciones" CssClass="button_autorizar"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Percepciones_Deducciones_Click" Width="100%"
                                                /> 
                                            </center>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
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
</asp:Content>

