<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Con_Cuentas_Fijas.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Cuentas_Fijas"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Div_General">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <%--<asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>--%>
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Cuentas Contables Fijas
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">                          
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" ToolTip="Modificar" onclick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" onclick="Btn_Salir_Click" />                                      
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Contenido" runat="server">
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td style="width:25%; height:20px;" colspan="2">                          
                        </td>                 
                    <tr>                
                        <td style="width:25%;">
                            IVA Por acreditar
                        </td>
                        <td style="width:75%;">
                            <asp:DropDownList ID="Cmb_Cuentas_Iva_Acreditable_Compras" runat="server" 
                                Width="35%">
                            </asp:DropDownList>
                        </td>
                    <tr>
                        <td>
                            Compras Almacén
                        </td>                  
                        <td>
                            <asp:DropDownList ID="Cmb_Cuentas_Compras_Almacen" runat="server" Width="35%">
                            </asp:DropDownList>
                        </td>
                                                                                 
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

