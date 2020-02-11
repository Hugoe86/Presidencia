<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_PAE_Remtes.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_PAE_Remtes" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Remates</title>
<script type="text/javascript" src="../../jquery/jquery-1.5.js"></script>
    <link href="../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/TabContainer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="TSM_Remates" runat="server"></cc1:ToolkitScriptManager>
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <center>
            <table width="1024px" style="background-color:White">
                <tr>
                    <td colspan="4"><img alt="encabezado" src="../imagenes/master/encabezado.png" /></td>
                </tr>
                <tr>
                        <td colspan="4" style="text-align: left">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table width="1024px" style="background-color:White">
                <tr>
                    <td colspan="4" align="center"><H1>Remates</H1></td>
                </tr>                
                <tr>
                    <td colspan="4" align="justify">
                        <p>Bienvenido a la página oficial del municipio de Irapuato Guanajuato.</p>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <p>Aquí podrás encontrar todo tipo bienes  de muy buena calidad en remates, 
                           selecciona el tipo de bien que quisieras ofertar en los remates y obtendrás una lista de bienes y fechas para que acudas a los remates.</p>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Selecciona el tipo de bien que quieres buscar:&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="Cmb_Tipos_Bienes" runat="server" Width="250px" AutoPostBack="true"
                            onselectedindexchanged="Cmb_Tipos_Bienes_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="border-bottom-style:groove; border-left-style:groove; border-right-style:ridge; border-top-style:ridge;">
                        <asp:Label ID="Lbl_Bienes" runat="server" Width="300px" Text="Bienes para Rematar"/>
                    
                     <asp:GridView ID="Grid_Generadas" runat="server" 
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;"
                            Width="100%" OnPageIndexChanging="Grid_Generadas_PageIndexChanging" DataKeyNames="NO_BIEN" OnSelectedIndexChanged="Grid_Generadas_SelectedIndexChanged">
                            <Columns>       
                                <asp:BoundField DataField="LUGAR_REMATE" HeaderText="Lugar de Remante" DataFormatString="{0:C2}">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>                                            
                                <asp:BoundField DataField="FECHA_HORA_REMATE" HeaderText="Fecha y Hora del Remate">
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" >
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="VALOR" HeaderText="Valor" DataFormatString="{0:C2}" >
                                    <ItemStyle HorizontalAlign="right"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText=" Ver Imagenes">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Detalle" runat="server" Height="20px" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="10"                         
                                                CommandName="Select" 
                                                ToolTip="Agregar Gastos Ejecucion" Width="20px"                                                                                                                        
                                                OnClick="Btn_Detalle_Click"/>                                                            
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%"/>
                                </asp:TemplateField>
                            </Columns>
                           <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <HeaderStyle CssClass="tblHead" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
           </table>
           </center>                      
       </ContentTemplate>    
    </asp:UpdatePanel>
    </form>
</body>
</html>
