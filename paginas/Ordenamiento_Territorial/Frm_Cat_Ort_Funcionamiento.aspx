<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
CodeFile="Frm_Cat_Ort_Funcionamiento.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Funcionamiento" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
           <%--Div de Contenido --%>
           <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width = "98%" border="0" cellspacing="0">                     
                    <tr>
                        <td colspan ="4" class="label_titulo">Catálogo Funcionamiento</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="6">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="true">
                            <table style="width:100%;" class="estilo_fuente">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                    </td>            
                                </tr>
                                <tr>
                                    <td style="width:10%;">              
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <%--<asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />--%>
                                    </td>
                                </tr>          
                            </table>                   
                        </div>
                        </td>
                    </tr>
                     <%--Manejo de la barra de busqueda--%>
                    <tr class="barra_busqueda">
                    <td colspan = "2" align = "left" >
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            onclick="Btn_Nuevo_Click"/>
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            onclick="Btn_Modificar_Click"/>
                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            onclick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?');"/>
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            onclick="Btn_Salir_Click"/>
                    </td>
                    <td colspan="2" align = "right">Busqueda
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                            onclick="Btn_Buscar_Click" />
                    </td> 
                    </tr>
                </table>
                
                <%--Div oculto de id --%>
                <div id="Div_Plantilla_id" runat="server" style="display:none">
                    <table width="98%">
                        <tr>
                            <td >
                                <asp:HiddenField ID="Hdf_Elemento_ID" runat="server" />
                            </td>
                        </tr>
                    </table>
                     <table width="98%">
                        <tr>
                            <td rowspan="5">
                             </td>
                        </tr>
                    </table>
                </div>
                
                 <%--Div de datos de la formatos --%>
                <div id="Div_Datos_Formato" runat="server" style="display: block">
                    <table width="98%">
                        <tr>
                            <td style="width:15%" align="left">
                                 Nombre             
                            </td>
                            <td  style="width:85%" align="left">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="50" Width="95%" ></asp:TextBox>              
                            </td>
                        </tr>
                    </table>
                </div>
                
                 <table width="98%">
                        <tr>
                            <td rowspan="5">
                             </td>
                        </tr>
                    </table>
                
                 <%-- Manejo del Grid View--%>
                <div id="Div_Grid_Formatos" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Avance_Obra" runat="server" 
                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Funcionamiento" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" Width="100%"  
                                            EmptyDataText="No se encontraron datos"
                                            onselectedindexchanged="Grid_Funcionamiento_SelectedIndexChanged" 
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:ButtonField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="FUNCIONAMIENTO_ID" HeaderText="ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 2 --%>  
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="95%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="95%" />
                                                </asp:BoundField>                                                  
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                        </tr> 
                    </table>
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



