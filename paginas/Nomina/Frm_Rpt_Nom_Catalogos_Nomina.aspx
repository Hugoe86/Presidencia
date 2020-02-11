<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Catalogos_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Catalogos_Nomina" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>

<asp:UpdatePanel ID="UPnl_Captura_Masiva_Proveedores_Fijas" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Captura_Masiva_Proveedores_Fijas"
        DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter">
            </div>
            <div class="processMessage" id="div_progress">
                <img alt="" src="../Imagenes/paginas/Updating.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
        <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td>
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                        Visible="false" />&nbsp;
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error" />
                </td>
            </tr>
        </table>
        
        <table width="98%" border="0" cellspacing="0">
        <tr align="center">
            <td colspan="2">
                <div align="right" class="barra_busqueda">
                    <table style="width: 100%; height: 28px;">
                        <tr>
                            <td align="left" style="width: 59%;">
                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                    TabIndex="20" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    CausesValidation="false" />
                            </td>
                            <td align="right" style="width: 41%;">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    
    <center>
        <table style="width:78%;">
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width:20%; text-align:left; cursor:default;">
                    Tablas
                </td>
                <td style="width:80%; text-align:left; cursor:default;" colspan="3">
                    <asp:DropDownList ID="Cmb_Tablas_Nomina" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Tablas_Nomina_SelectedIndexChanged"
                        AutoPostBack="true" />
                </td>    
            </tr>
            <tr>                   
                <td style="width:20%; text-align:left; cursor:default;">
                    Campos
                </td>
                <td style="width:80%; text-align:left; cursor:default;" colspan="3">
                    <asp:DropDownList ID="Cmb_Campos_Por_Tabla" runat="server" Width="100%"/>
                </td>                   
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="Btn_Agregar_Campo" runat="server" Text="Agregar Campo" CssClass="button_autorizar" style="width:100%; cursor:hand;"
                        OnClick="Btn_Agregar_Campo_Click"/>
                </td>
            </tr>            
        </table>
    
        <table style="width:78%;">
            <tr>
                <td colspan="4" align="right">
                    <asp:GridView ID="Grid_Campos_Mostrar_Reporte" runat="server" CssClass="GridView_1" Width="50%"
                         AutoGenerateColumns="False"  GridLines="None" >
                            <Columns>                                             
                                <asp:BoundField DataField="NOMBRE_CAMPO" HeaderText="Campo">
                                    <HeaderStyle HorizontalAlign="Left" Width="80%" Font-Size="XX-Small" Font-Bold="true"/>
                                    <ItemStyle HorizontalAlign="Left" Width="80%" Font-Size="XX-Small" Font-Bold="true"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btn_Eliminar_Campo" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                            OnClick="Btn_Eliminar_Campo_Click" CommandArgument='<%# Eval("NOMBRE_CAMPO") %>'/>                                                    
                                    </ItemTemplate>
                                    <ItemStyle  HorizontalAlign="Center" Width="20%" Font-Size="XX-Small"/>
                                    <HeaderStyle  HorizontalAlign="Center" Width="20%" Font-Size="XX-Small"/>                                                  
                                </asp:TemplateField>                                                                                                                                                                                             
                            </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>                                 
                </td>
            </tr>
        </table>   
    </center>     
    

    </ContentTemplate>
</asp:UpdatePanel>

<center>
    <table style="width:78%;">       
        <tr>
            <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
                        <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                             OnClick="Button1_Click" ToolTip="Generar Reporte en EXCEL" Width="32px" Height="32px" style="cursor:hand;"/>
            </td>                
        </tr>   
        <tr>
            <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                <hr />
            </td>                
        </tr>                                                                         
    </table>
</center>

</asp:Content>

