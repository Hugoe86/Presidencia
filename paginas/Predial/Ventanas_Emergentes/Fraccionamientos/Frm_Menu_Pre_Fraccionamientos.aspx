<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Fraccionamientos.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Fraccionamientos_Frm_Menu_Pre_Fraccionamientos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>COSTOS</title>
    <%--<link href="../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />--%>
    <base target="_self" />
    <style type="text/css">
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
    <form id="frm_menu_pre_fraccionamientos" runat="server"> 
    <asp:ScriptManager ID="SCM_Impuesto_Fraccionamiento" runat="server" />
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
        <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >

                                <table width="100%">
                                    <tr class="barra_busqueda">
                        <td align="center" colspan="4">
                            <asp:ImageButton ID="Btn_Regresar" runat="server" 
                                AlternateText="Regresar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Regresar_Click" Width="24px" />
                        </td>
                        </tr>
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                           Identificador
                                        </td>
                                        <td style="width:35%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_Identificador" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Identificador" runat="server" 
                                                FilterType="CUSTOM, LowercaseLetters, UppercaseLetters" ValidChars=" "
                                                TargetControlID="Txt_Busqueda_Identificador"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Identificador" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Identificador" WatermarkText="Búsqueda por Identificador" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Año
                                        </td>     
                                        <td style="width:85%;text-align:left;">
                                           <asp:TextBox ID="Txt_Busqueda_Año" runat="server" Width="95%" MaxLength="4" />
                                           <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Año_FilteredTextBoxExtender" runat="server" 
                                                FilterType="Numbers" TargetControlID="Txt_Busqueda_Año"/>
                                            <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Año_TextBoxWatermarkExtender" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Año" WatermarkText="Búsqueda por Año" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            Descripción
                                        </td>
                                        <td style="width:35%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:TextBox ID="Txt_Busqueda_Descripcion" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Descripcion" runat="server" 
                                                FilterType="CUSTOM, LowercaseLetters, UppercaseLetters" ValidChars=" "
                                                TargetControlID="Txt_Busqueda_Descripcion"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Descripcion" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Descripcion" WatermarkText="Búsqueda por Descripción" 
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
                                               <asp:Button ID="Btn_Busqueda_Costos" runat="server"  Text="Buscar Costos" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Costos_Click" /> 
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
                                            <asp:GridView ID="Grid_Costos" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                OnPageIndexChanging="Grid_Costos_PageIndexChanging" 
                                                OnSelectedIndexChanged="Grid_Costos_SelectedIndexChanged" PageSize="5" 
                                                Width="100%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="FRACCIONAMIENTO_ID" HeaderText="FRACCIONAMIENTO_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0%" />
                                                        <ItemStyle Font-Size="0pt" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IMPUESTO_FRACCIONAMIENTO_ID" HeaderText="IMPUESTO_FRACCIONAMIENTO_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0%" />
                                                        <ItemStyle Font-Size="0pt" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                        <HeaderStyle HorizontalAlign="Center" Width="45%" />
                                                        <ItemStyle Width="45%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MONTO" HeaderText="Monto">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
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
                            </td>
                        </tr>
                       
                     </table>
                   </div>
    </form>
</body>
</html>
