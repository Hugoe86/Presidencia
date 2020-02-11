<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Principal.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Principal" Title="Untitled Page" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/slide/css/modal-window.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/slide/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/slide/css/slideshow_1.css" rel="stylesheet" type="text/css" />    
    <script src="../../easyui/slide/js/modal-window.js" type="text/javascript"></script>
    <script src="../../easyui/slide/js/slideshow_1.js" type="text/javascript"></script>
   
<script type="text/javascript" >
    jQuery(function($){
      $('#slider').s3Slider({timeout:4000,fadeTime:800});
    });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<br />
<table width="100%">
    <%--<tr>
        <td align ="left">
            <asp:Image ID="Image1" runat="server" src="../imagenes/master/esquina_izquierda.png" />
        </td>
    </tr>--%>
    <tr>
        <td align ="right">
            <asp:Image ID="Image2" runat="server" src="../imagenes/master/esquina_derecha.png" />
        </td>
    </tr>    
</table>
    <div>

    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    </div>
</asp:Content>

