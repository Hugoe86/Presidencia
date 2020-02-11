<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Login.aspx.cs" Inherits="paginas_Frm_Apl_Login" Title="Página sin título" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Cph_Area_Trabajo2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Login" runat="server" />
    <link href="../../jquery/Modal/jquery.modaldialog.css" rel="stylesheet" type="text/css" />
    <script src="../../jquery/Modal/jquery.modaldialog.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Cambio_Cantrasena.js" type="text/javascript"></script>
    <div id="tabla" style="position:absolute; width:200px; height:300px;  top:237px;right:80px">           
        <table>
            <tr>    
                <td align="right">
                    <asp:TextBox ID="Txt_Usuario" runat="server" Columns="15" MaxLength="15"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_Txt_Usuario" runat="server" ErrorMessage="*" ControlToValidate="Txt_Usuario"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Usuario" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Usuario" 
                             ValidChars="0123456789"/>                 
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Txt_Password" runat="server" Columns="15" MaxLength="15" 
                        TextMode="Password" ontextchanged="Txt_Password_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_Txt_Password" runat="server" ErrorMessage="*" ControlToValidate="Txt_Password" >
                    </asp:RequiredFieldValidator>
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
        <table width="100%" style="text-align:center;">
            <tr>
                <td>
                <a id="Btn_Mostrar_Ventana" onclick="javascript:Btn_Cambio_Contrasena();"  style="cursor:hand;" title="Actualiza el password del empleado">Cambiar Password</a>
                </td>
            </tr>           
        </table>    
        <asp:ImageButton ID="Btn_Img_Mensaje" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" style="visibility: hidden"/>
        <asp:Label ID="Lbl_Mensaje" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>

