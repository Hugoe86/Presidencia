<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Pre_Busqueda_Notarios.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Recepcion_Documentos_Frm_Pre_Busqueda_Notarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B&uacute;squeda Avanzada de Notarios</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <script type="text/javascript" language="javascript">
        function Limpiar_Controles_Busqueda_Notario()
        {
            document.getElementById("<%=Txt_No_Notario.ClientID%>").value="";
            document.getElementById("<%=Txt_Numero_Notaria.ClientID%>").value="";
            if(document.getElementById("<%=IBtn_Imagen_Error.ClientID%>") != null )
            {
            document.getElementById("<%=IBtn_Imagen_Error.ClientID%>").style.visibility = "hidden";
            }
            document.getElementById("<%=Lbl_Ecabezado_Mensaje.ClientID%>").innerHTML="";
            document.getElementById("<%=Lbl_Mensaje_Error.ClientID%>").innerHTML="";
            
               
            document.getElementById("<%=Txt_RFC.ClientID%>").value="";
            document.getElementById("<%=Txt_Nombre_Notario.ClientID%>").value="";
            return false;            
        }
    </script>
    
    
    
</head>
<body>

    <form id="Frm_Pre_Busqueda_Notarios" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Div_Contenedor_General_Busqueda_Notarios" style="background-color:#ffffff; width:100%; height:100%;">
        
        <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;" colspan='4'>
                            <asp:Image ID="Img_Icono_Busqueda_Notarios" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Notarios
                        </td>                        
                    </tr>
                    <tr class="barra_busqueda">
                        <td align="left" colspan="3"> 
                            <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                                AlternateText="Aceptar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                onclick="Btn_Aceptar_Click" Width="24px" />
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
                                           No Notario 
                                        </td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_No_Notario" runat="server" Width="98%" MaxLength="10"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Notario" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_No_Notario"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Notario" runat="server" 
                                                TargetControlID ="Txt_No_Notario" WatermarkText="Búsqueda por No Notario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            Notaria
                                        </td>
                                        <td style="width:38%;text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Notaria" runat="server" Width="98%" MaxLength="10" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Numero_Notaria" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" 
                                                TargetControlID="Txt_Numero_Notaria"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Numero_Notaria" runat="server" 
                                                TargetControlID ="Txt_Numero_Notaria" WatermarkText="Búsqueda por No Notaria" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_RFC" runat="server" Width="98%" MaxLength="15" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" 
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_RFC" WatermarkText="Búsqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            &nbsp;</td>
                                        <td style="width:38%;text-align:left;">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>     
                                        <td style="width:88%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Nombre_Notario" runat="server" Width="99.5%" MaxLength="95" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Notario" runat="server" 
                                                FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters" 
                                                TargetControlID="Txt_Nombre_Notario" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Notario" runat="server" 
                                                TargetControlID ="Txt_Nombre_Notario" WatermarkText="Búsqueda por Nombre" 
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
                                               <asp:Button ID="Btn_Busqueda_Notarios" runat="server"  Text="Buscar Notarios" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Notarios_Click" /> 
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
                                            <asp:GridView ID="Grid_Notarios" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                PageSize="5" 
                                                Width="98%" onpageindexchanging="Grid_Notarios_PageIndexChanging" 
                                                onselectedindexchanged="Grid_Notarios_SelectedIndexChanged">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NOTARIO_ID" HeaderText="No.">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                                        <ItemStyle Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NUMERO_NOTARIA" HeaderText="Notaría" 
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
    </form>
</body>
</html>
