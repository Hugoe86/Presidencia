<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Rpt_Com_Tipo_Compra.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Com_Tipo_Compra" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="Upd_Panel" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
        </asp:UpdateProgress>
        <%--Contenido del Formulario--%>
        <div id="Div_Contenido" style="width:99%;">
            <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                <tr>
                    <td colspan ="2" class="label_titulo">Reporte de Compras</td>
                </tr>
                <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td colspan ="2">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                </td>            
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                </td>
                            </tr> 
                        </table>                   
                        </div>
                    </td>
                </tr>
                <%--Renglon de barra de Busqueda--%>
                <tr class="barra_busqueda">
                    <td style="width:20%" colspan="4">
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" 
                            /><%--onclick="Btn_Salir_Click"--%>
                        <asp:ImageButton ID="Btn_Limpiar" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                            onclick="Btn_Limpiar_Click" ToolTip="Limpiar Formulario" />
                        <asp:ImageButton ID="Btn_Exportar_PDF" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                            ToolTip="Exportar PDF" onclick="Btn_Generar_Reporte_Click" AlternateText="Consultar"/>
                        <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                            ToolTip="Exportar PDF" onclick="Btn_Exportar_Excel_Click" AlternateText="Consultar"/>
                    </td>
                </tr>
                <%--Contenido del reporte--%>
                <tr>
                    <td colspan="2">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:15%">
                                Tipo de Compra
                            </td>
                            <td style="width:30%">
                                <asp:DropDownList ID="Cmb_Tipo_Compra" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                            <td style="width:15%">
                                 
                            </td>
                            <td style="width:30%">
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                 Tipo Articulo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo_Articulo" runat="server" Width="99%" 
                                    onselectedindexchanged="Cmb_Tipo_Articulo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Requisici&oacute;n
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Requisiciones" runat="server" Width="99%">
                                </asp:DropDownList>                                
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td>
                                Proveedor
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                           
                        </tr>
                        <tr>
                             <td>
                                Cotizador
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Cotizador" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Producto/Servicio</td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Producto" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Fecha Cotizaci&oacute;n 
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="150px"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" Enabled="true" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Inicio" TargetControlID="Txt_Fecha_Inicial" />
                                    &nbsp;&nbsp;&nbsp;Al&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="150px"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" Enabled="true" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Final_CalendarExtender" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Fecha_Final" />
                            </td>
                           
                        </tr>
                        <%--<tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Exportar PDF" 
                                    CssClass="button" Width ="20%" onclick="Btn_Generar_Reporte_Click"/>  
                            </td>
                            <td colspan="2" align="center">
                                <asp:Button ID="Btn_Exporzxtar_Excel" runat="server" Text="Exportar Excel" 
                                    CssClass="button" Width ="20%" onclick="Btn_Exportar_Excel_Click"/>  
                            </td>
                        </tr>--%>
                    
                    </table>
                    </td>
                </tr> 
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>