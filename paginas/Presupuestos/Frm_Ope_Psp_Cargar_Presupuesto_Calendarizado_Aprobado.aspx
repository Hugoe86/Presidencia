<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Psp_Cargar_Presupuesto_Calendarizado_Aprobado.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Psp_Cargar_Presupuesto_Calendarizado_Aprobado" Title="Cargar presupuesto aprobado" %>
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
                               Cargar presupuesto calendarizado aprobado
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
                <div id="Div_Contenido" runat="server">
                    <table style="width: 100%; text-align: left;" border="1">
                        <caption>
                            <br />
                            <tr>
                                <td style="width: 20%">
                                    Año de presupuesto
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <asp:DropDownList ID="Cmb_Anio" runat="server" OnSelectedIndexChanged="Cmb_Anio_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2" align="center">
                                    <asp:Button ID="Btn_Cargar_Presupuesto_Calendarizad" runat="server" 
                                        Text="Cargar Presupuesto Calendarizado" CssClass="button" Width="240px"
                                        Height="45px" Style="text-align:center;" onclick="Btn_Cargar_Presupuesto_Calendarizad_Click" 
                                         />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Total Aprobado"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_Aprobado" runat="server" Text="$0.00" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                        </caption>
                    </table>

                    
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

