<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Migrar_Bienes.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Migrar_Bienes" Title="Migración de Bienes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <center>
                <asp:Button ID="Btn_Cargar_Actualizacion" runat="server" Text="CARGAR ACTUALIZACIÓN DE BIENES" onclick="Btn_Cargar_Actualizacion_Click" />
                </center>    
                <div id="Div_Encontrados" style="background-color:#ffffff; width:100%; height:250px; overflow:auto;">
                    <asp:GridView ID="Grid_Encontrados" runat="server">
                    </asp:GridView>
                </div>    
                <div id="Div_No_Encontrados" style="background-color:#ffffff; width:100%; height:250px; overflow:auto;">
                    <asp:GridView ID="Grid_No_Encontrados" runat="server">
                    </asp:GridView>
                </div>    
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

