<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Proveedores.master" AutoEventWireup="true" CodeFile="Frm_Apl_Login_Proveedores.aspx.cs" Inherits="paginas_Frm_Apl_Login" Title="Página sin título" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Cph_Area_Trabajo2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <div id="tabla" style="position:absolute; width:200px; height:300px;  top:237px;right:80px">           
        <table>
            <tr>    
                <td align="right">
                    <asp:TextBox ID="Txt_Usuario" runat="server" Columns="15" MaxLength="15"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_Txt_Usuario" runat="server" ErrorMessage="*" ControlToValidate="Txt_Usuario"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Txt_Password" runat="server" Columns="15" MaxLength="15" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_Txt_Password" runat="server" ErrorMessage="*" ControlToValidate="Txt_Password"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                        AlternateText="Iniciar sesión..." 
                        
                        ImageUrl="../imagenes/paginas/sias_entrar.png" onclick="Btn_Aceptar_Click"/>
                </td>
            </tr>
        </table>
    </div>
    <div id="mensaje" style="position:absolute; width:450px; height:300px;  top:340px;right:60px" >
        <asp:ImageButton ID="Btn_Img_Mensaje" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" style="visibility: hidden"/>
        <asp:Label ID="Lbl_Mensaje" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>

