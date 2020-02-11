<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ejecutar_Query.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ejecutar_Query" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
            
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    <ContentTemplate>
       <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
           </asp:UpdateProgress>
        <div id="Div_Contenido" style="width:99%;">
        <table width="99%">
            <tr>
                <td style="width:20%;">
                    Sentencia a Ejecutar
                </td>
                <td align="left">
                    <asp:CheckBox ID="Chk_Select" Text="Sentencias Select" runat="server" />
                </td>
                
                
            <tr>
                
                <td colspan="2">
                    <asp:TextBox ID="Txt_Sentencia" runat="server" TextMode="MultiLine" Width="100%" Height="300px"></asp:TextBox>
                </td>
            
            </tr>
            
            <tr>
                <td>
                    <asp:ImageButton ID="Btn_Limpiar_Busqueda_Avanzada" runat="server" ToolTip="Limpiar"
                    ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                        onclick="Btn_Limpiar_Busqueda_Avanzada_Click"/>
                </td>
                <td>
                    <asp:Button ID="Btn_Ejecutar_Query" runat="server" 
                        Text="Ejecutar Sentencia Oracle" Width ="30%" 
                        onclick="Btn_Ejecutar_Query_Click" />    
                </td>
                
                
            </tr>
                
             <tr>
                
                <td colspan="2">
                    <div ID="Div_1" runat="server" visible="false"
                    style="overflow:auto;height:900px;width:900px;vertical-align:top;border-style:outset;border-color:Silver;">
                    <asp:GridView ID="Grid_Consulta_ORACLE" runat="server" Width="99%">
                    </asp:GridView>
                    </div>
                </td>
            
            </tr>
        </table>
        
        
        
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
        
