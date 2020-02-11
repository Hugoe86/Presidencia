<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Cajas_Empleados.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Cajas_Empleados" Title="Asignación de la Caja al Empleado" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Cajas_Empleados" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Cajas_Empleados" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>
            <div id="Div_IMSS" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Asignacion de la Caja al Empleado</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                        CssClass="Img_Button" TabIndex="1"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                        onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio"
                                                        CssClass="Img_Button" TabIndex="3"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td align="right" style="width:41%;"></td>       
                                    </tr>         
                                </table>                   
                            </div>
                        </td>
                    </tr>
                </table>
                <br /> 
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Empleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>          
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales del Empleado" Width="97%" BackColor="White">              
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                                <tr>
                                    <td style="text-align:left;width:20%;">No Empleado</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_No_Empleado" runat="server" ReadOnly="True" Width="98%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">&nbsp;&nbsp;Estatus</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Estatus_Empleado" runat="server" ReadOnly="True" Width="98%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">Nombre</td>
                                    <td colspan="3" style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="True" Width="99%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Caja" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Caja" runat="server" GroupingText="Datos de la Caja" Width="97%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                                <tr>
                                    <td style="text-align:left;width:20%;">*Módulo</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Modulos_Caja" runat="server" Width="98%" AutoPostBack="true" TabIndex="4" 
                                            onselectedindexchanged="Cmb_Modulos_Caja_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align:left;width:20%;">&nbsp;&nbsp;*Caja</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Cajas_Modulos" runat="server" Width="98%" TabIndex="5">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                         </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

