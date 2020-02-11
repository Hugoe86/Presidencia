<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ort_Pendientes.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Pendientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
   
    <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
        <center>
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" align="left">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                            Width="24px" Height="24px" />
                        <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                    </td>
                    <td style="width: 90%; text-align: left;" valign="top">
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <br />
    <table style="width: 100%;">
        <tr class="barra_busqueda">
            <td style="font-size:large; text-align:center;">
                Mis Pendientes
            </td>
        </tr>
    </table>
    
    <center>
        <table style="width: 100%; height:230px; background-image:url('../imagenes/master/esquina_derecha.png'); background-position:right; background-repeat:no-repeat">
            <tr style="height:50%";>
                <td style="width: 20%;" rowspan="4">
                </td>
                <td></td>
                <td></td>
                <td style="width: 20%;" rowspan="4">
                </td>
            </tr>
            <tr style="height:25%";>
                <td align="left" style="width: 45%;">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="DarkBlue" Text="Actividades de Tramites pendientes por atender:"></asp:Label>
                    <asp:Label ID="Lbl_Solicitudes_Pendientes" runat="server" Font-Bold="true" ForeColor="OrangeRed" style="font-size:medium;"></asp:Label>
                </td>
                <td align="left" style="width: 15%;">
                    <asp:Button ID="Btn_Solicitudes_Pendientes" runat="server" Text="Revisar" OnClick="Btn_Solicitudes_Pendientes_Click" />
                </td>
            </tr>
            <tr id="Tr_Proceso" runat="server" style="height:25%";>
                <td align="left" style="width: 45%;">
                    <asp:Label ID="Lbl_Solicitudes_Proceso" runat="server" Font-Bold="true" ForeColor="DarkBlue" Text="Actividades de Tramites de la direccion por atender:"></asp:Label>
                    <asp:Label ID="Lbl_No_Solicitudes_Proceso" runat="server" Font-Bold="true" ForeColor="OrangeRed" style="font-size:medium;"></asp:Label>
                </td>
                <td align="left" style="width: 15%;">
                    <asp:Button ID="Btn_Solicitudes_Proceso" runat="server" Text="Revisar" OnClick="Btn_Solicitudes_Proceso_Click" />
                </td>
            </tr>
            <tr style="height:25%";>
                <td align="left" style="width: 45%;">
                    <asp:Label ID="Label2" runat="server" Font-Bold="true" ForeColor="DarkBlue" Text="Solicitudes de Opinion pendientes de responder:"></asp:Label>
                    <asp:Label ID="Lbl_Fichas_Pendientes" runat="server" Font-Bold="true" ForeColor="OrangeRed" style="font-size:medium;"></asp:Label>
                </td>
                <td align="left" style="width: 15%;">
                    <asp:Button ID="Btn_Fichas_Pendientes" runat="server" Text="Revisar" OnClick="Btn_Fichas_Pendientes_Click" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
