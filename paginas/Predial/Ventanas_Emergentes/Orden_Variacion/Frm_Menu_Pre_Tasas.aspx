<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Tasas.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Tasas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tasas Predial Anuales</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        .mayus {
	        text-transform: uppercase;	
            }
    </style>

    <script type="text/javascript" language="javascript">
        window.onerror = new Function("return true");
        function Limpiar_Controles_Busqueda_Notario()
        {            
            if(document.getElementById("<%=IBtn_Imagen_Error.ClientID%>") != null )
            {
            document.getElementById("<%=IBtn_Imagen_Error.ClientID%>").style.visibility = "hidden";
            }
            document.getElementById("<%=Lbl_Ecabezado_Mensaje.ClientID%>").innerHTML="";
            document.getElementById("<%=Lbl_Mensaje_Error.ClientID%>").innerHTML="";
            
            document.getElementById("<%=Txt_Tasa_ID.ClientID%>").value="";
            document.getElementById("<%=Txt_Ano_Tasa.ClientID%>").value="";      
            document.getElementById("<%=Txt_Identificador.ClientID%>").value="";
            document.getElementById("<%=Txt_Descripcion.ClientID%>").value="";
            return false;            
        }
    </script>
</head>
<body bgcolor="White">
    
    <form id="Frm_Pre_Tasas_Predial_Anual" method="post" runat="server">   
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../../../imagenes/paginas/Sias_Roler.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
    <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
        <table width="99%" cellpadding="2" cellspacing="0">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;" colspan='4'>
                            <asp:Image ID="Img_Icono_Busqueda_Notarios" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Tasas Predial
                        </td>                        
                    </tr>
                    <tr class="barra_busqueda">
                        <td align="left" colspan="3"> 
                            <asp:ImageButton ID="Btn_Regresar" runat="server" 
                                AlternateText="Regresar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Regresar_Click" Width="24px" />                            
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Controles_Busqueda_Notario();"
                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>
                        </td>
            </tr>
                    <tr>
                        <td style="width:100%" colspan="2" align="left">                        
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" Height="24px" 
                                ImageUrl="../../../imagenes/paginas/sias_warning.png" Width="24px" />
                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" 
                                CssClass="estilo_fuente_mensaje_error" Text="" />                        
                        </td>
                        <td style="width:100%" colspan="2" align="right">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td style="width:10%;">              
                        </td>          
                        <td style="width:90%;text-align:left;" valign="top" colspan="3">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                        </td>
                    </tr>
                        
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                           Tasa ID&nbsp;</td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Tasa_ID" runat="server" Width="98%" MaxLength="10" AutoPostBack="true" OnTextChanged="Txt_Tasa_ID_TextChanged"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tasa_ID" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Tasa_ID"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Tasa_ID" runat="server" 
                                                TargetControlID ="Txt_Tasa_ID" WatermarkText="Búsqueda por ID" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            A&ntilde;o
                                        </td>
                                        <td style="width:38%;text-align:left;">
                                            <asp:TextBox ID="Txt_Ano_Tasa" runat="server" Width="98%" MaxLength="4" AutoPostBack="true" OnTextChanged="Txt_Ano_Tasa_TextChanged" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Ano_Tasa" runat="server" 
                                                FilterType="Numbers" 
                                                TargetControlID="Txt_Ano_Tasa"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Ano_Tasa" runat="server" 
                                                TargetControlID ="Txt_Ano_Tasa" WatermarkText="Búsqueda por Año" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            Identificador
                                        </td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="15" CssClass="mayus" AutoPostBack="true" OnTextChanged="Txt_Identificador_TextChanged"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Identificador" runat="server" 
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_Identificador"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Identificador" runat="server" 
                                                TargetControlID ="Txt_Identificador" WatermarkText="Búsqueda por Identificador" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            &nbsp;</td>
                                        <td style="width:38%;text-align:left;">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            Descripci&oacute;n
                                        </td>     
                                        <td style="width:88%;text-align:left;" colspan="3">
                                            <hr />
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99.5%" MaxLength="95" CssClass="mayus" AutoPostBack="true" OnTextChanged="Txt_Descripcion_TextChanged"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Descripcion" runat="server" 
                                                FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters" 
                                                TargetControlID="Txt_Descripcion" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Descripcion" runat="server" 
                                                TargetControlID ="Txt_Descripcion" WatermarkText="Búsqueda por Descripción" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Tasas" runat="server"  Text="Buscar Tasas" CssClass="button"  
                                                CausesValidation="false" Width="200px" onclick="Btn_Busqueda_Tasas_Click"/> 
                                            </center>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Tasas" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                PageSize="5" 
                                                Width="98%" onpageindexchanging="Grid_Tasas_PageIndexChanging" 
                                                onselectedindexchanged="Grid_Tasas_SelectedIndexChanged" >
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="TASA_ANUAL_ID" HeaderText="No.">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador">
                                                        <ItemStyle Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion">
                                                        <ItemStyle Width="50%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderText="Año" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TASA_ANUAL" HeaderText="TASA" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                    </table>
                                    </div>        
        </ContentTemplate>
    </asp:UpdatePanel>                            
</form>
</body>
</html>
