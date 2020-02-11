<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Detalles_Gastos_De_Ejecucion.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Detalles_Gastos_De_Ejecucion" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DETALLE DE CUENTA</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">

.textAlign
{
    text-align: right 
    }
        .style3
        {
            width: 150px;
        }

    </style>
</head>
<body >
    <form id="Frm_Detalle_Gasto_Ejecucion" method="post" runat="server">
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
            <tr class="barra_busqueda">                
                <td align="left" colspan="2">
                    <asp:ImageButton ID="Btn_Regresar" runat="server" 
                        AlternateText="Regresar" CssClass="Img_Button" 
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                         Width="24px" onclick="Btn_Regresar_Click" />
                </td>
            </tr>
            <tr>
                
                <td align="center" style="background-color: #6699FF" colspan="2">
                    <asp:Label ID="Lbl_Title" runat="server" Text="Detalle de gastos de ejecución" Font-Bold="True" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr >
                
                <td class="style3">
                    Corriente</td>
                <td>
                    <asp:TextBox ID="Txt_Corriente" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>
            <tr >
                
                <td class="style3">Rezago</td>
                <td>
                    <asp:TextBox ID="Txt_Rezago" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>
            <tr >
                
                <td class="style3">Recargos Ordinarios</td>
                <td>
                    <asp:TextBox ID="Txt_Recargos_Ordinarios" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>       
            <tr >
               
                <td class="style3">Recargos Moratorios</td>
                <td>
                    <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>
            <tr >
                
                <td class="style3">Honorarios</td>
                <td>
                    <asp:TextBox ID="Txt_Honorarios" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>
            <tr >
                
                <td class="style3">Gastos Ejecución</td>
                <td>
                    <asp:TextBox ID="Txt_Gasto_Ejecucion" runat="server" Width="150px" CssClass="textAlign"></asp:TextBox>
                </td>
            </tr>
            </table>        
    </div>
    </form>
</body>
</html>

