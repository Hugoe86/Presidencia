<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Postores_Almonedas.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_PAE_Frm_Postores_Almonedas" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>POSTORES</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .style2
        {
            width: 56px;
        }
        .style3
        {
            width: 163px;
        }
    </style>
</head>
<body>
    <form id="Frm_Postores_Alomedas" method="post" runat="server">
        <%--<cc1:ToolkitScriptManager ID="Tsm_Postores" runat="server" EnableScriptGlobalization="true">
        </cc1:ToolkitScriptManager>--%>
        <asp:ScriptManager ID="Tsm_Postores" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Uprg_Reporte" runat="server"
                    AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0" >
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" 
                                src="../../../imagenes/paginas/Updating.gif" / height="50px" width="50px"></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
                <div id="Div_Contenedor_Msj_Error" style="width:99%;" runat="server" visible="false">
                    <table style="width:99%;">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                            </td>            
                        </tr>       
                    </table>                   
              </div>                          
            <div>    
               <table  style="width:99%;" border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda">
                        <td align="left">
                            <asp:UpdatePanel ID="Upd_Panel_Botones" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                                        AlternateText="Aceptar" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                        onclick="Btn_Aceptar_Click" Width="24px" />
                                    <asp:ImageButton ID="Btn_Regresar" runat="server" 
                                        AlternateText="Regresar" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                        onclick="Btn_Regresar_Click" Width="24px" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                       </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: #6699FF">
                            <asp:Label ID="Lbl_Title" runat="server" Text="Gastos de Ejecucion" Font-Bold="True" 
                                ForeColor="White"></asp:Label>
                        </td>
                    </tr>    
               </table>        
            </div>
            <div>
                <asp:Panel ID="Pnl_Gastos" runat="server" Width="99%" Height="99%">
                    <table style="width:99%;" border="0" cellspacing="0">
                   <%----------------------------------- Notifiaciones---------------------------------------------------------%>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre postor"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Lbl_Deposito" runat="server" Text="Depósito" />
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="Txt_Deposito" runat="server" Width="94%" Enabled="false"/>
                                <%--<cc1:FilteredTextBoxExtender ID="Fte_Txt_Deposito" runat="server" TargetControlID="Txt_Deposito"
                                FilterType="Numbers,Custom" ValidChars="."/>--%>                               
                            </td>
                            <td class="style2">
                                <asp:Label ID="Lbl_Porcentaje" runat="server" Text="Porciento"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Porcentaje" runat="server" Width="98%" MaxLength="3" OnTextChanged="Txt_Porcentaje_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Porcentaje" runat="server" TargetControlID="Txt_Porcentaje"
                                FilterType="Numbers,Custom"/>
                            </td>
                        </tr> 
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Lbl_Domicilio" runat="server" Text="Domicilio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Domicilio" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                            </td>
                        </tr>                                             
                        <tr>                                
                            <td class="style1" >
                                <asp:Label ID="Lbl_Telefono" runat="server" Text="Teléfono" />                                 
                            </td>
                            <td colspan="3">                                        
                                <asp:TextBox ID="Txt_Telefono" runat="server" Width="99%"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Telefono" runat="server" TargetControlID="Txt_Telefono"
                                FilterType="Numbers,Custom"/>
                                
                            </td>
                        </tr>
                        <tr>                                
                            <td class="style1" >
                                <asp:Label ID="Lbl_RFC" runat="server" Text="RFC" />                                 
                            </td>
                            <td colspan="3">                                        
                                <asp:TextBox ID="Txt_RFC" runat="server" Width="99%" onkeyup='this.value=this.value.toUpperCase();'/>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_RFC" runat="server"
                                    TargetControlID="Txt_RFC" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                    ValidChars=" " Enabled="True"/>   
                            </td>
                        </tr>
                        <tr>                                
                            <td class="style1" >
                                <asp:Label ID="Lbl_IFE" runat="server" Text="No.IFE" />                                 
                            </td>
                            <td colspan="3">                                        
                                <asp:TextBox ID="Txt_IFE" runat="server" Width="99%"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_IFE" runat="server" TargetControlID="Txt_IFE"
                                    FilterType="Numbers,Custom" />   
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Lbl_Sexo" runat="server" Text="Sexo" />
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Sexo" runat="server" TabIndex="7" Width="99%">
                                    <asp:ListItem><- Seleccionar -></asp:ListItem>
                                    <asp:ListItem>MASCULINO</asp:ListItem>
                                    <asp:ListItem>FEMENINO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Lbl_Estado_Civil" runat="server" Text="Estado civil" />
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Estado_Civil" runat="server" TabIndex="7" Width="99%">
                                    <asp:ListItem><- Seleccionar -></asp:ListItem>
                                    <asp:ListItem>SOLTERO(A)</asp:ListItem>
                                    <asp:ListItem>CASADO(A)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Postores" runat="server"
                                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                    HeaderStyle-CssClass="tblHead" PageSize="2" Style="white-space: normal;"
                                    Width="100%" 
                                    onselectedindexchanged="Grid_Postores_SelectedIndexChanged" 
                                    onpageindexchanging="Grid_Postores_PageIndexChanging">
                                    <Columns>
                                    <asp:BoundField DataField="NOMBRE_POSTOR" HeaderText="Nombre">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>                                  
                                    <asp:BoundField DataField="TELEFONO" HeaderText="Telefono">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SEXO" HeaderText="Sexo">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO_CIVIL" HeaderText="Estado Civil">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DEPOSITO" HeaderText="Deposito">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PORCENTAJE" HeaderText="Porcentaje">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Eliminar_Publicacion" runat="server" Height="20px" 
                                                ImageUrl="~/paginas/imagenes/paginas/delete.png" TabIndex="10"
                                                CommandName="Select"
                                                ToolTip="Eliminar" Width="20px"/>
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>--%>
                                </Columns>
                                    <HeaderStyle CssClass="tblHead" />
                                </asp:GridView>
                            </td>                                                
                        </tr>
                   </table>
                   <asp:HiddenField ID="Hdn_No_Det_Etapa" runat="server" />
                   <asp:HiddenField ID="Hdn_Avaluo" runat="server" />
                </asp:Panel>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Btn_Regresar" />
                <asp:PostBackTrigger ControlID="Btn_Aceptar" />                
            </Triggers>        
    </asp:UpdatePanel>
    </form>
</body>
</html>