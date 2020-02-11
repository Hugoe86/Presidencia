<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ciudadanos.master" AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Tramite_Listado_Tramites.aspx.cs" Inherits="paginas_Tramites_Frm_Ope_Tra_Tramite_Listado_Tramites" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <%--               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
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
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio"  />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Tramites" runat="server">
                  <asp:Panel ID="Pnl_Tramites" runat="server"  GroupingText="Trámites" Width="75%" >
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%" align="left">
                            Dependencias
                            </td>                            
                            <td style="width: 60%" align="left">
                                <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="98%" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Dependencias_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width: 15%" align="left">
                            Trámites
                            </td>                            
                            <td style="width: 60%" align="left">
                                <asp:DropDownList ID="Cmb_Tramites" runat="server" Width="92%" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Tramites_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Refrescar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/refresh16.png"
                                    ToolTip = "Volver a cargar trámites"
                                    onclick="Btn_Refrescar_Click" />
                            </td>                            
                        </tr>    
                        <tr>
                            <td style="width: 15%" align="left">
                            Búsqueda
                            </td>                            

                            <td style="width: 15%" align="left">
                                <asp:Button ID="Btn_Solicitar_Tramite" runat="server" Text="Solicitar Trámite" 
                                    OnClick="Btn_Solicitar_Tramite_Click"/>
                            </td>                                                        
                        </tr>                                              
                    </table>
                  </asp:Panel>  
                  <asp:Panel ID="Pnl_Busqueda" runat="server"  GroupingText="Trámites" Width="20%">                  
                  </asp:Panel>
                </div>
                <div id="Div_Realizar_Tramite" runat="server">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

