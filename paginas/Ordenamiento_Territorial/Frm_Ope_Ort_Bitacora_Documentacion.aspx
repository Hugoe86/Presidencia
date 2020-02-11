<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
 CodeFile="Frm_Ope_Ort_Bitacora_Documentacion.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Bitacora_Documentacion" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">
        
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
               
    </script>
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
                        <td colspan ="4" class="label_titulo">Bitácora de documentación  Faltante y Entregada</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="6">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;display:block" runat="server" visible="true" >
                            <table style="width:100%;" class="estilo_fuente">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="prueba de mensaje"
                                        ForeColor="Red" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;" />
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
                            <%--<asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click"/>--%>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                onclick="Btn_Modificar_Click"/>
                            <%--<asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                onclick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?');"/>--%>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2" align = "right">Estatus
                                <asp:DropDownList ID="Cmb_Estatus" runat="server"  >
                                    <asp:ListItem Value="FALTANTE">FALTANTE</asp:ListItem>
                                    <asp:ListItem Value="ENTREGADO">ENTREGADO</asp:ListItem>
                                </asp:DropDownList>  
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
                
                
                <table width="98%">
                    <tr>                          
                        <td style="width:15%" align="left">
                             Solicitud             
                        </td>
                        <td  style="width:85%" align="left">
                            <asp:DropDownList ID="Cmb_Solicitud" runat="server" Width="90%" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Solicitud_SelectedIndexChanged"></asp:DropDownList>  
                            <asp:ImageButton ID="Btn_Buscar_Solicitud" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Solicitud_Click"   />          
                        </td>
                    </tr>
                    <tr>                          
                        <td style="width:15%" align="left">
                             Documentacion             
                        </td>
                        <td  style="width:85%" align="left">
                            <asp:DropDownList ID="Cmb_Documentacion" runat="server" Width="90%" ></asp:DropDownList> 
                            <asp:ImageButton ID="Btn_Buscar_Documento" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Documento_Click"   />    
                            <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" 
                                ImageUrl="~/paginas/imagenes/gridview/add_grid.png" Width="20px" Height="20px"
                                OnClick="Btn_Agregar_Documento_Click" />           
                        </td>
                    </tr>
                </table>
                
                <table width="98%">            
                    <tr>                          
                        
                        <td  style="width:40%" align="center">    
                                      
                        </td>
                    </tr>
                </table>
                
                 <%-- Manejo del Grid View--%>
                <div id="Div_Grid_Formatos" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Bitacora" runat="server" 
                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Bitacoras" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" Width="100%"  AllowSorting="true"
                                            EmptyDataText="No se encontraron documentos"  
                                            OnRowDataBound="Grid_Bitacoras_RowDataBound"  
                                            OnSorting="Grid_Bitacoras_Sorting"                                          
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:BoundField DataField="BITACORA_ID" HeaderText="Bitacora_ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="SOLICITUD_ID" HeaderText="Solicitud_ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 2 --%>  
                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="Subproceso_id" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>     
                                                <%-- 3 --%>  
                                                <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Nombre" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField> 
                                                <%-- 4 --%>
                                                <asp:TemplateField HeaderText="Entregado"
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="5%"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="12px" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chck_Activar" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                            
                                                <%-- 5 --%>  
                                                <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave" Visible="True"
                                                    SortExpression="CLAVE_SOLICITUD">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField> 
                                                <%-- 6 --%>  
                                                <asp:BoundField DataField="NOMBRE_ACTIVIDAD" HeaderText="Actividad" Visible="True"
                                                      SortExpression="NOMBRE_ACTIVIDAD">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>    
                                                <%-- 7 --%>  
                                                <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento" Visible="True"
                                                      SortExpression="NOMBRE_DOCUMENTO">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>                                                 
                                                <%-- 8 --%>  
                                                <asp:BoundField DataField="FECHA_ENTREGA_DOC" HeaderText="Fecha" Visible="True"
                                                    DataFormatString="{0:dd/MMM/yyyy hh:mm:ss}"
                                                          SortExpression="FECHA_ENTREGA_DOC">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>
                                                <%-- 9 --%>  
                                                 <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True"
                                                      SortExpression="ESTATUS">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
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



