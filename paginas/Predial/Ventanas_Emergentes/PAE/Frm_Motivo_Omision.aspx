<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Motivo_Omision.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Motivo_Omision" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MOTIVO DE OMISION</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
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

        .style2
        {
            width: 126px;
        }

    </style>
</head>
<body>
    <form id="Frm_Motivo_Omision" method="post" runat="server">
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
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></cc1:ToolkitScriptManager>

            <table  style="width:98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr class="barra_busqueda">
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center">
                    <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                        AlternateText="Aceptar" CssClass="Img_Button" 
                        ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                        onclick="Btn_Aceptar_Click" Width="24px" />
                    <asp:ImageButton ID="Btn_Regresar" runat="server" 
                        AlternateText="Regresar" CssClass="Img_Button" 
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                        onclick="Btn_Regresar_Click" Width="24px" />
                </td>
            </tr>
            <tr>
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center" style="background-color: #6699FF">
                    <asp:Label ID="Lbl_Title" runat="server" Text="Introduce el motivo de omisión" Font-Bold="True" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="Txt_Motivo" runat="server" Width="250px" Height="25px"></asp:TextBox>
                </td>
            </tr>       
            </table>        
    </div>
    </form>
</body>
</html>
