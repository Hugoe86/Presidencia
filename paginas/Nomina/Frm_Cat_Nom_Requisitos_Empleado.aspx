<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Requisitos_Empleado.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Requisitos_Empleado" Title="Catálogo Requisitos Empleado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Requisitos_Empleados" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Requisitos_Empleados" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Requisitos Empleado
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan = "2" align = "left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click"  />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click"  /> 
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" 
                                
                                OnClientClick="return confirm('¿Está seguro de eliminar el requisito seleccionado?');" 
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click" />
                        </td>
                        <td colspan="2">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Requistos_Empleado" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Nombre"
                                Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Requistos_Empleado" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Requistos_Empleado" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Requistos_Empleado" 
                                runat="server" TargetControlID="Txt_Busqueda_Requistos_Empleado" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                            <asp:ImageButton ID="Btn_Buscar_Requisitos_Empleado" runat="server" TabIndex="6"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" onclick="Btn_Buscar_Requisitos_Empleado_Click" 
                                 />
                        </td>                        
                    </tr>
                </table>
                                
                <table width="100%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Requisito ID
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Requisito_ID" runat="server" ReadOnly="True" Width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre_Requisito" runat="server" MaxLength="100" TabIndex="8" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Nombre_Requisito" 
                                runat="server" TargetControlID="Txt_Nombre_Requisito" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Requisito" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Archivo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Aplica_Archivo" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>S</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                
                <table width="100%">
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Requistos" runat="server" CssClass="GridView_1" Width="100%"
                                AutoGenerateColumns="false"  GridLines="None"  PageSize="5" AllowPaging="true"
                                onpageindexchanging="Grid_Requistos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Requistos_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Requistos_Sorting" HeaderStyle-CssClass="tblHead" 
                                >
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Requisito_ID" HeaderText="Requisito ID" 
                                        Visible="True" SortExpression="Requisito_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" Visible="True" SortExpression="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="33%"/>
                                        <ItemStyle HorizontalAlign="Left" Width="33%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                        <ItemStyle HorizontalAlign="Left" Width="33%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Archivo" HeaderText="Archivo" Visible="True" SortExpression="Archivo">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                </table>                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


