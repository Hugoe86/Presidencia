<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Cat_Tra_Plantillas.aspx.cs" Inherits="paginas_Tramites_Frm_Cat_Tra_Plantillas_Subprocesos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 202px;
        }
    </style>
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
                <table width = "95%" border="0" cellspacing="0">                     
                    <tr>
                        <td colspan ="4" class="label_titulo">Plantillas</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="6">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;" class="estilo_fuente">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
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
                            onclick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar la plantilla seleccionada?');"/> 
                        <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" ImageUrl="~/paginas/imagenes/paginas/sias_download.png"
                            OnClick="Btn_Ver_Documento_Click" />
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
                                 Plantilla ID            
                            </td>
                            <td >
                                <asp:TextBox ID="Txt_Plantilla_ID" runat="server" Width="280px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <%--Div de datos de la plantilla --%>
                <div id="Div_Datos_Plantilla" runat="server" style="display: block">
                    <table width="98%">
                        <tr>
                            <td style="width:15%" align="left">
                                 Nombre (Plantilla)               
                            </td>
                            <td  style="width:85%" align="left">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="100" Width="85%" ></asp:TextBox>              
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%" align="left">
                                Nombre (Archivo)
                            </td>
                            <td style="width:85%" align="left">
                                <asp:TextBox ID="Txt_Nombre_Archivo" runat="server" Width="85%" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%" align="left">
                                Subir Archivo
                                </td>
                            <td style="width:85%" align="left">
                                <cc1:AsyncFileUpload ID="Btn_Subir_Archivo" runat="server" Width="100%"
                                    UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen" />
                            </td>
                        </tr>
                        <tr>    
                            <td colspan="2"> 
                                 
                            </td>
                        </tr>  
                    </table>
                </div>
                    
                <%-- Manejo del Grid View--%>
                <div id="Div1" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Tramites_Generales" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Plantillas" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" Width="97%"  
                                            EmptyDataText="No se encontraron datos"
                                            onpageindexchanging="Grid_Plantillas_PageIndexChanging"
                                            onselectedindexchanged="Grid_Plantillas_SelectedIndexChanged" 
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:ButtonField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="Plantilla_ID" HeaderText="ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 2 --%>  
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre de la plantilla" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="45%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="45%" />
                                                </asp:BoundField>   
                                                <%-- 3 --%> 
                                                <asp:BoundField DataField="Archivo" HeaderText="Nombre del Archivo" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="50%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="50%" />
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

