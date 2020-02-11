<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    CodeFile="Frm_Cat_Ort_Parametros.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Parametros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
    <!--
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }     
    //-->
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Parametros_Ordenamiento" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <div id="Div_Principal" style="background-color: #ffffff; width: 100%; height: 100%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Parámetros de Ordenamiento Territorial
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" TabIndex="0" Text="Mensajes de advertencia"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="Div_Barra_Herramientas" runat="server">
                    <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr class="barra_busqueda">
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2" align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Contenido1" runat="server" style="width: 99%;">
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Unidad Responsable de Ordenamiento Territorial
                            </td>
                            <td style="width: 70%; text-align: right;">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="93%" ToolTip="Seleccione la Unidad Responsable de Ordenamiento Territorial">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Click" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Unidad Responsable de Ordenamiento Ambiental
                            </td>
                            <td style="width: 70%; text-align: right;">
                                <asp:DropDownList ID="Cmb_Dependencia_Ambiental" runat="server" Width="93%" ToolTip="Seleccione la Unidad Responsable de Ordenamiento ambiental">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia_Ambiental" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Ambiental_Click" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Unidad Responsable de Administracion Urbana
                            </td>
                            <td style="width: 70%; text-align: right;">
                                <asp:DropDownList ID="Cmb_Dependencia_Urbana" runat="server" Width="93%" ToolTip="Seleccione la Unidad Responsable de administracion urbana">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia_Urbana" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Urbana_Click" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Unidad Responsable de Fraccionaminetos
                            </td>
                            <td style="width: 70%; text-align: right;">
                                <asp:DropDownList ID="Cmb_Dependencia_Fraccionamientos" runat="server" Width="93%" ToolTip="Seleccione la Unidad Responsable de fraccionamientos">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia_Fraccionamientos" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Fraccionamientos_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Unidad Responsable de Catastro
                            </td>
                            <td style="width: 70%; text-align: right;">
                                <asp:DropDownList ID="Cmb_Dependencia_Catastro" runat="server" Width="93%" ToolTip="Seleccione la Unidad Responsable de fraccionamientos">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia_Catastro" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Catastro_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Rol del director de ordenamiento
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:DropDownList ID="Cmb_Rol_Ordenamiento_Territorial" runat="server" Width="98%" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CssClass="WindowsStyle" MaxLength="0"
                                    ToolTip="Seleccione el rol Director">
                                </asp:DropDownList>
                                 <asp:ImageButton ID="Btn_Buscar_Rol" runat="server" ToolTip="Seleccionar el rol" Visible="false"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                     />
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Rol del director de Ambiental
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:DropDownList ID="Cmb_Rol_Ordenamiento_Ambiental" runat="server" Width="98%" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CssClass="WindowsStyle" MaxLength="0"
                                    ToolTip="Seleccione el rol Director">
                                </asp:DropDownList>
                                 <asp:ImageButton ID="Btn_Buscar_Rol_Ambiental" runat="server" ToolTip="Seleccionar el rol" Visible="false"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                     />
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Rol del director de Fraccionamientos
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:DropDownList ID="Cmb_Rol_Ordenamiento_Fraccionamientos" runat="server" Width="98%" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CssClass="WindowsStyle" MaxLength="0"
                                    ToolTip="Seleccione el rol Director">
                                </asp:DropDownList>
                                 <asp:ImageButton ID="Btn_Buscar_Rol_Fraccionamientos" runat="server" ToolTip="Seleccionar el rol" Visible="false"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                     />
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Rol del director de Urbana
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:DropDownList ID="Cmb_Rol_Ordenamiento_Urbana" runat="server" Width="98%" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CssClass="WindowsStyle" MaxLength="0"
                                    ToolTip="Seleccione el rol Director">
                                </asp:DropDownList>
                                 <asp:ImageButton ID="Btn_Buscar_Rol_Urbana" runat="server" ToolTip="Seleccionar el rol" Visible="false"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                     />
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Rol del Inspector de ordenamiento
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:DropDownList ID="Cmb_Rol_Inspector_Ordenamiento" runat="server" Width="98%" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CssClass="WindowsStyle" MaxLength="0"
                                    ToolTip="Seleccione el rol Director">
                                </asp:DropDownList>
                                 <asp:ImageButton ID="Btn_Rol_Inspector_Ordenamiento" runat="server" ToolTip="Seleccionar el rol" Visible="false"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                     />
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    
                    
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Costo por perito
                            </td>
                            <td style="width: 67%; text-align: right;">
                               <asp:TextBox ID="Txt_Costo_Perito" runat="server" Width="97%"></asp:TextBox>
                            </td>
                             <td style="width: 3%;">
                                
                            </td>
                        </tr>
                         <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                             <td style="width: 30%;">
                                 *&nbsp;Costo por Bitacora
                            </td>
                             <td style="width: 67%; text-align: right;">
                               <asp:TextBox ID="Txt_Costo_Bitacora" runat="server" Width="97%"></asp:TextBox>
                            </td>  
                            <td style="width: 3%;">
                                
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
