<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Resolucion.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Resolucion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Detalle de la Cuenta</title>
    <%--<link href="../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />--%>
    <base target="_self" />
    <style type="text/css">
.Ventana
{
    height:420px;
    width:700px;	
}    
.GridView_1
{
    font-family:Verdana, Geneva, MS Sans Serif;
    font-size:8px;
    color:#2F4E7D;
    font-weight:normal;
    padding: 3px 6px 3px 6px;
    vertical-align:middle;
    white-space:nowrap;
    background-color:White;
    border-style:none;
    border-width:5px;
    width:98%;    
    text-align:left;
	margin-left: 0px;
}
*
{
	font-family:Arial;
	font-size:small;
	text-align: left;
}
.GridHeader
 {
     font-weight:bold;
     background-color:#2F4E7D;
     color:#ffffff;
     text-align:left;
     position:relative;
     height:23px;
 }

.GridItem
{
 background-color:white;
 color:#25406D;
}

.GridAltItem
{
 background-color:#E6E6E6;/*#E6E6E6 #E0F8F7*/
 color:#25406D;
}

.GridSelected
{
 background-color:#A4A4A4; /*#A9F5F2;*/
 color:#25406D;
}

.renglon_botones
{
   vertical-align:middle; 
   height:40px;
}

        </style>
</head>
<body>
<script type="text/javascript" language="javascript">
	//Abrir una ventana modal
	function Abrir_Ventana_Modal(Url, Propiedades)
	{
		window.showModalDialog(Url, null, Propiedades);
	}

    //Metodos para abrir los Modal PopUp's de la página
    function Abrir_Busqueda_Colonias() {
        $find('Busqueda_Colonias').show();
        Window_Resize();
        return false;
    }

    function Abrir_Busqueda_Calles() {
        $find('Busqueda_Calles').show();
        Window_Resize();
        return false;
    }
    
    function Window_Resize()
    {
        var myWidth = 0;
        var myHeight = 0;
        var yWithScroll = 0;
        var xWithScroll = 0;

        //Obtiene las dimensiones de la ventana emergente en su alto y ancho.
        if( typeof( window.innerWidth ) == 'number' ) 
        {
            //Non-IE
            myWidth = window.innerWidth+18;
            myHeight = window.innerHeight+77;
        }
        else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
        {
            //IE 6+ in 'standards compliant mode'
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
        }
        else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
        {
            //IE 4 compatible
            myWidth = document.body.clientWidth+18;
            myHeight = document.body.clientHeight+77;
        }
        //Aplica un alto más grande a la ventana
        window.resizeTo(myWidth,myHeight/2);

        //Obtiene los máximos en desplazamiento de scroll vertical y horizontal
        if( typeof( window.innerWidth ) == 'number' ) 
        {
            //Non-IE
            myWidth = window.innerWidth+18;
            myHeight = window.innerHeight+77;
        }
        else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
        {
            //IE 6+ in 'standards compliant mode'
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
        }
        else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
        {
            //IE 4 compatible
            myWidth = document.body.clientWidth+18;
            myHeight = document.body.clientHeight+77;
        }
        if (window.scrollMaxY > 0 || window.scrollMaxX > 0)
        {
            // Firefox 
            yWithScroll = window.scrollMaxY; 
            xWithScroll = window.scrollMaxX; 
        }
        else if (document.body.scrollHeight > document.body.offsetHeight
                || document.body.scrollWidth > document.body.offsetWidth)
        { 
            // all but Explorer Mac 
            yWithScroll = document.body.scrollHeight; 
            xWithScroll = document.body.scrollWidth; 
        }
        else if (document.documentElement.scrollHeight > document.documentElement.offsetHeight
                || document.documentElement.scrollWidth > document.documentElement.offsetWidth)
        { 
            // Explorer 
            yWithScroll = document.documentElement.scrollHeight; 
            xWithScroll = document.documentElement.scrollWidth; 
        }
        
        //Redimensiona el alto y ancho de la ventana para ajustar el contenido y no se visualice la barra del scroll
        window.resizeTo(myWidth+xWithScroll,myHeight+yWithScroll);
    }

</script>
    <form id="Frm_Ope_Pre_Resolucion" method="post" runat="server">
      <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
        <table style="width:100%;">
          <tr>
            <td colspan="2" align="left">
              <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                Width="24px" Height="24px"/>
                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
            </td>            
          </tr>
          <tr>
            <td style="width:10%;">              
            </td>          
            <td style="width:90%;text-align:left;" valign="top" >
              <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
            </td>
          </tr>          
        </table>                   
      </div>                          
    <div>
    <asp:ScriptManager ID="ScptM_Contribuyentes" runat="server" />
<%--    <asp:UpdatePanel ID="Upd_Parametros_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
--%>        
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <%--<div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >--%>
                        <div style="width:99%; "  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="center" style="width:59%;">
                                    DETALLE CÁLCULO
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                    
                <br />
                    
                    <asp:HiddenField ID="Hdn_No_Calculo" runat="server" />
                    <asp:HiddenField ID="Hdn_No_Orden" runat="server" />
                    <asp:HiddenField ID="Hdn_Anio_Orden" runat="server" />
                    <asp:HiddenField ID="Hdn_Anio_Calculo" runat="server" />
                    <asp:HiddenField ID="Hdn_Tasa_Traslado_ID" runat="server" />
                    <asp:HiddenField ID="Hdn_Tasa_Div_Lotif_ID" runat="server" />
                    <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                    <asp:HiddenField ID="Hdn_Realizo_Calculo" runat="server" />
                    
                <asp:Panel ID="Pnl_Controles" runat="server" Visible="true">
                <table width="98%" class="estilo_fuente">
                    <%------------------ Cálculos pendientes ------------------%>
                    <tr id="Tr_Titulo_Pendientes" style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Cálculos
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <br />
                        </td>
                    </tr>
                </table>
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:25%;">
                            Estatus
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Estatus" runat="server" Width="96.4%" TabIndex="7" 
                                ReadOnly="true" />
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             Folio pago
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Folio_Pago" runat="server" Width="96.4%" TabIndex="8" 
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            Cuenta Predial
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" AutoPostBack="true"
                                 TabIndex="9" MaxLength="20" ></asp:TextBox>
                        </td>
                        <td colspan="2" style="text-align:right; padding-right:35px;vertical-align:bottom;">
                            <asp:CheckBox ID="Chk_Predio_Colindante" runat="server" Text="Predio colindante"
                                TabIndex="10" OnCheckedChanged="Chk_Predio_Colindante_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            *Base del impuesto
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Base_Impuesto" runat="server" Width="96.4%" TabIndex="11" />
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             <span style="float:left;">-</span>Mínimo elevado al año
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Minimo_Elevado_Anio" runat="server" Width="96.4%" 
                                TabIndex="12" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            Base gravable T. de dominio
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Base_Gravable_Traslado" runat="server" Width="96.4%"
                             TabIndex="13" />
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             <span style="float:left;">x</span>Tasa T. de dominio
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Tasa_Traslado_Dominio" runat="server" Width="96.4%"
                             TabIndex="14" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             <span style="float:left;">=</span>Impuesto Traslado dom.
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Impuesto_Traslado_Dominio" runat="server" Width="96.4%" 
                             TabIndex="15" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                             Fecha escritura
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="96.4%" 
                            TabIndex="16" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            Tipo División o lotificación
                        </td>
                        <td style="text-align:left;width:25%;" colspan="3">
                            <asp:TextBox ID="Txt_Tipo_Division_Lotificacion" runat="server" Width="98%"
                                TabIndex="17" TextMode="SingleLine" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            *Base del impuesto
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Base_Impuesto_Div_Lotif" runat="server" Width="96.4%" 
                                TabIndex="18"  />
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             <span style="float:left;">x</span>Tasa Divi. o Lotif. (%)
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Tasa_Division_Lotificacion" runat="server" Width="96.4%" 
                             TabIndex="19" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="width:25%;text-align:right;padding-left:35px;">
                             <span style="float:left;">=</span>Impuesto Div./Lotif.
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Impuesto_Division_Lotificacion" runat="server" Width="96.4%" 
                             TabIndex="20" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:25%;text-align:right;padding-left:35px;">
                            <span style="float:left;">+</span>Constancia No adeudo
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Costo_Constancia_No_Adeudo" runat="server" Width="79%" 
                                TabIndex="21" />
                            <asp:CheckBox ID="Chk_Constancia_No_Adeudo" runat="server" Text="" Checked="true" 
                                TabIndex="22" oncheckedchanged="Chk_Constancia_No_Adeudo_CheckedChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:25%;text-align:right;padding-left:35px;">
                            <span style="float:left;">+</span>Multa
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Multa" runat="server" Width="96.4%" 
                             TabIndex="23"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:25%;text-align:right;padding-left:35px;">
                            <span style="float:left;">+</span>Recargos
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Recargos" runat="server" Width="96.4%"
                             TabIndex="24" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:25%;text-align:right;padding-left:35px;">
                            <span style="float:left;">=</span>Total
                        </td>
                        <td style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" 
                                TabIndex="25"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%;">
                            Tipo
                        </td>
                        <td style="text-align:left;width:25%;" colspan="3">
                            <asp:RadioButton ID="Opt_Tipo_Valor_Fiscal" GroupName="Opt_Tipo" runat="server" 
                                Text="Valor fiscal" TabIndex="26" /> &nbsp; &nbsp;
                            <asp:RadioButton ID="Opt_Tipo_Valor_Operacion" GroupName="Opt_Tipo" runat="server" 
                                Text="Valor operación" TabIndex="27" /> &nbsp; &nbsp;
                            <asp:RadioButton ID="Opt_Tipo_Avaluo_Predial" GroupName="Opt_Tipo" runat="server" 
                            Text="Valor registrado" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;</td>
                    </tr>
                    <tr><td style="text-align:left;width:25%;">*Fundamento </td>
                        <td colspan="3" style="text-align:left;width:25%;">
                            <asp:TextBox ID="Txt_Fundamento" runat="server" TabIndex="30" 
                                TextMode="SingleLine" Width="98%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fundamento" runat="server" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Fundamento" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:25%; vertical-align: top;">
                            <asp:Label ID="Lbl_Campo_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                        </td>
                        <td style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios_Area" runat="server" Width="98.6%" Style="text-transform:uppercase"
                                TabIndex="31" TextMode="MultiLine" MaxLength="250" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" 
                                runat="server" TargetControlID="Txt_Comentarios_Area" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                </table>
                
                <div id="Encabezado_Observaciones_Anteriores" runat="server" style="color: #25406D;display:none;">Observaciones anteriores:</div>
                <div id="Contenedor_Observaciones_Anteriores" runat="server" style="color: #25406D;display:none;"></div>
                <br />
                </asp:Panel>

    </div>
    </form>
</body>
</html>
