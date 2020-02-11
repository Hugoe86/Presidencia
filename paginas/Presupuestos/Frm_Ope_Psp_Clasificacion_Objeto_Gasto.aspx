<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Psp_Clasificacion_Objeto_Gasto.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Psp_Clasificacion_Objeto_Gasto" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.treegrid.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Ope_Psp_Clasificacion_Objeto_Gasto.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>    
    <div>
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                 <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>
                <center>
                  <div id="Div_Encabezado" runat="server">
                    <table style="width:99%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Clasificación por Objeto del Gasto
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" style="display:none"/>
                                <asp:Label ID="Lbl_Encanezado_Error" runat="server" Text="Favor de:" ForeColor="#990000" style="display:none"></asp:Label><br />
                                <asp:Label ID="Lbl_Error" runat="server" ForeColor="#990000" style="display:none"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Salir" />
                            </td>
                            <td colspan="2"> &nbsp; </td>
                        </tr>
                    </table>
                </div>
                 <div id="Div1" style="clear:both;">&nbsp;</div>
                  <div id="Div_COG" runat="server">
                   <center>
                        <div>
                            <table style="width:99%;" border="0" cellspacing="0">
                                <tr align="left">
                                    <td>
                                        <asp:DropDownList ID="Cmb_Anios" runat="server" Width="150px"></asp:DropDownList>
                                        &nbsp;
                                        <asp:Button ID="Btn_Generar" runat="server" CssClass="button" Text="Generar" ToolTip="Generar" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>&nbsp;</div>
                        <div style="text-align:center">
                            <table id="Grid_COG"></table>
                        </div>
                   </center>
                </div>
               </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

