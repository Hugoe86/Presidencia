<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Psp_Autorizar_Partida_Asignada.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Psp_Autorizar_Partida_Asignada" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript">
         $(document).ready(function() {
            window.open('Frm_Ope_Psp_Autorizar_Partida_Asignada_Emergrnte.aspx', 'Autorización', 'toolbar=0,directories=0,menubar=0,status=0,scrollbars=1,resizable=NO,width=1210,height=620,left=20px, top=20px');
         });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <table width="100%" >
        <tr align="center"><td>&nbsp;</td></tr>
        <tr align="center"><td>&nbsp;</td></tr>
        <tr align="center"><td>&nbsp;</td></tr>
        <tr align="center"><td>&nbsp;</td></tr>
        <tr>
            <td align ="center">
                <asp:Image ID="Image2" runat="server" src="../imagenes/paginas/aceptar-y-rechazar.jpg" Width="300px" Height="200px" />
            </td>
        </tr>
    </table>
</asp:Content>

