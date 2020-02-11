<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Migrar_Bienes_Control_Patrimonial.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Migrar_Bienes_Control_Patrimonial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SUBIR INFORMACIÓN...GtApCkO</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
        <asp:Button ID="Btn_Subir_Bienes_Muebles" runat="server" Text="SUBIR BIENES MUEBLES" onclick="Btn_Subir_Bienes_Muebles_Click" Visible="false" />
        <asp:Label ID="Lbl_Contador" runat="server" Text="Registros Leidos: 0"></asp:Label>
        <br />
        <br />
        <hr />
        <asp:Button ID="Btn_Subir_Vehiculos" runat="server" Text="SUBIR VEHICULOS" onclick="Btn_Subir_Vehiculos_Click" />
        <br />
        <br />
        <hr />
        <asp:Button ID="Btn_Subir_Animales" runat="server" Text="SUBIR ANIMALES" onclick="Btn_Subir_Animales_Click" />
        <br />
        <br />
        <hr />
        <asp:Button ID="Btn_Subir_Bienes_Muebles_2" runat="server" 
            Text="SUBIR BIENES MUEBLES" onclick="Btn_Subir_Bienes_Muebles_2_Click"  />
        <br />
        <br />
        <hr />
        <asp:Button ID="Btn_Subir_Resguardos_Bienes_Muebles" runat="server" 
            Text="SUBIR RESGUARDOS BIENES MUEBLES" 
            onclick="Btn_Subir_Resguardos_Bienes_Muebles_Click" />
        <br />
        <br />
        <hr />
    </div>
    </form>
</body>
</html>