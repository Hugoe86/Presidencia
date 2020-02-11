<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Peritos_Externos.master" AutoEventWireup="true" CodeFile="Frm_Apl_Login_Peritos_Externos.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Login_Peritos_Externos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Cph_Area_Trabajo2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <div id="tabla" style="position:absolute; width:200px; height:300px;  top:237px;right:80px">
        <table>
            <tr>    
                <td align="right">
                    <asp:TextBox ID="Txt_Usuario" runat="server" Columns="15" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_Txt_Usuario" runat="server" ErrorMessage="*" ControlToValidate="Txt_Usuario"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Txt_Password" runat="server" Columns="15" MaxLength="20" TextMode="Password"></asp:TextBox>
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
            <tr>
                <td>
                <asp:HyperLink ID="Hlk_Registro" runat="server" ForeColor="Blue" Visible="true" 
                        Text="Registrarse"
                        NavigateUrl="../../paginas/Catastro/Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx"></asp:HyperLink>
                </td>
                </tr>
        </table>
    </div>
    <div id="mensaje" style="position:absolute; width:450px; height:300px;  top:380px;right:60px" >
        <asp:ImageButton ID="Btn_Img_Mensaje" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" style="visibility: hidden"/>
        <asp:Label ID="Lbl_Mensaje" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />                
        
    </div>
    
</asp:Content>