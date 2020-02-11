<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Ort_Administracion_Urbana.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Rpt_Ort_Administracion_Urbana" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Ficha_Revision" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <div id="Div_Area_Trabajo" style="background-color:#ffffff; width:100%; height:100%;">
                <center>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Reporte de Administración Urbana</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" >
                            <asp:ImageButton ID="Btn_Reporte" runat="server" CssClass ="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                ToolTip="Generar Reporte" onclick="Btn_Reporte_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Salir" />
                        </td>                     
                    </tr>
                </table>
                </center>
            </div>
            
            <div id="Div_Campos" runat="server" style="width:100%;">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Administracion_Urbana_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                        </td>
                    </tr>
                </table>
                <center>
                <br />
                    <asp:GridView ID="Grid_Listado" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False"  Width="99%"
                        GridLines= "None" EmptyDataText="No hay registros."  >
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField  ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                <ItemStyle Width="5%" HorizontalAlign="Center"/>
                            </asp:ButtonField>
                            <asp:BoundField DataField="ADMINISTRACION_URBANA_ID" HeaderText="ADMINISTRACION_URBANA_ID" SortExpression="ADMINISTRACION_URBANA_ID" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TRAMITE_ID" HeaderText="TRAMITE_ID" SortExpression="TRAMITE_ID" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID" SortExpression="SOLICITUD_ID" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SUBPROCESO_ID" SortExpression="SUBPROCESO_ID" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE_TRAMITE" HeaderText="Clave del Tramite" SortExpression="CLAVE_TRAMITE" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave de Solicitud" SortExpression="CLAVE_SOLICITUD" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Subproceso" SortExpression="NOMBRE" NullDisplayText="-" >
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />                                
                        <AlternatingRowStyle CssClass="GridAltItem" />       
                    </asp:GridView>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

