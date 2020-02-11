<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Bandeja_Tramites.aspx.cs" Inherits="paginas_Tramites_Ope_Bandeja_Tramites" Title="Bandeja de Tramites" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScptM_Bandeja_Tramites" runat="server" />

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
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Bandeja de Trámites</td>
                        </tr>
                        <tr>
                            <td>
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
                                    <td style="font-size:10px;width:90%;text-align:left;" valign="top" class="estilo_fuente_mensaje_error">
                                      <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"/>
                                    </td>
                                  </tr>          
                                </table>                   
                              </div>                          
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>                        
                        </tr>
                    </table>
            
                      <table width="98%"  border="0" cellspacing="0">
                             <tr align="center">
                                 <td>                
                                     <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left">  
                                            <asp:ImageButton ID="Btn_Guardar_Avance_Solucion" 
                                                runat="server" 
                                                onclick="Btn_Evaluar_Click" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_save.png" 
                                                CssClass="Img_Button" 
                                                ToolTip="Guardar avance y/o solución"
                                                Visible="false"/>
                                            <asp:ImageButton ID="Btn_Cancela" runat="server" onclick="Btn_Cancela_Click" 
                                                CssClass="Img_Button" Visible="false"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_cancelar.png" ToolTip="Cancelar" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server"  CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                                  CausesValidation="False" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right">Busqueda por Estatus
                                            <asp:DropDownList ID="Cmb_Buscar_Solicitudes_Estatus" runat="server">
                                                <asp:ListItem Value="DETENIDO" Text="DETENIDOS" />    
                                                <asp:ListItem Value="PENDIENTE_PROCESO" Text="PENDIENTES Y EN PROCESO" Selected="True" />
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="Btn_Buscar_Solicitudes_Estatus" runat="server" CausesValidation="false"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                onclick="Btn_Buscar_Solicitudes_Estatus_Click" />
                                           </td>       
                                         </tr>         
                                      </table>                      
                                    </div>
                                 </td>
                             </tr>
                      </table>
                </center>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel1"  Width="96%"  >
                        <HeaderTemplate>Bandeja de Trámites</HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">
                                <tr>
                                    <td style="width:15%">
                                        <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                                        <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label ID="Lbl_Clave_Solicitud" runat="server" Text="Clave Solicitud"></asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:TextBox ID="Txt_Clave_Solicitud" runat="server" 
                                            Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%" align="right">
                                        <asp:Label ID="Lbl_Porcentaje_Avance" runat="server" Text="% Fase"></asp:Label>
                                    </td>
                                    <td style="width:18%"> 
                                        <asp:TextBox ID="Txt_Porcentaje_Actual_Proceso" runat="server" 
                                            Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%" align="right">
                                        <asp:Label ID="Lbl_Porcentaje_Acumulado" runat="server" Text="% Acumulado"></asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:TextBox ID="Txt_Porcentaje_Avance" runat="server" 
                                            Width="98%" Enabled="False"></asp:TextBox>
                                       
                                    </td>
                                    <td style="width:1%">
                                    </td>
                                </tr>
                            </table>
                               
                            <table width="98%">
                                  <tr>
                                    <td style="width:15%"><asp:Label ID="Lbl_Nombre_Tramite" runat="server" Text="Tramite"></asp:Label></td>
                                    <td colspan="3" style="width:85%"><asp:TextBox ID="Txt_Nombre_Tramite" 
                                            runat="server" Width="99%" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width:15%"><asp:Label ID="Lbl_Solicito" runat="server" Text="Solicito"></asp:Label></td>
                                    <td colspan="3" style="width:85%"><asp:TextBox ID="Txt_Solicito" runat="server" 
                                            Width="99%" Enabled="False"></asp:TextBox></td>
                                </tr>
                                                                
                              <tr>
                                    <td style="width:15%"><asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label></td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Estatus" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%" align="right"><asp:Label ID="Lbl_Fecha_Solicitud" runat="server" Text="Fecha Solicitud"></asp:Label></td>
                                    <td style="width:35%"><asp:TextBox ID="Txt_Fecha_Solicitud" runat="server" 
                                            Width="98%" Enabled="False"></asp:TextBox></td>
                                </tr>                           
                            </table>
                            
                            <table width="98%">                            
                                <tr>
                                    <td style="width:15%; vertical-align: top;"><asp:Label ID="Lbl_Subproceso" runat="server" Text="Actividad"></asp:Label></td>
                                    <td style="width:80%">
                                        <asp:TextBox ID="Txt_Subproceso" runat="server" 
                                           TextMode="MultiLine" Width="98%" Height="48px" Enabled="False"></asp:TextBox> 
                                    </td>
                                    <td style="width:5%" align="center">                                                                           
                                        <asp:ImageButton ID="Btn_Dar_Solucion" 
                                            runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/sias_aceptarplan.png" 
                                            ToolTip="Dar Solución"
                                            onclick="Btn_Dar_Solucion_Click"
                                            Enabled = "False"/>
                                    </td>
                                </tr>
                            </table>
                            
                            <div id="Div_Dar_Solucion" runat="server" style="display:none">   
                                <asp:Panel ID="Pnl_Dar_Solucion" runat="server" GroupingText="Dar Solucion">
                                    <table width="98%">
                                        <tr>                                        
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Evaluacion" runat="server" Text="Soluci&oacute;n">
                                                </asp:Label>
                                            </td>
                                            <td width="85%">
                                                <asp:DropDownList ID="Cmb_Evaluacion" runat="server" Width="98%" >
                                                    <asp:ListItem Text="APROBAR" Value="APROBAR" Selected="True" /> 
                                                    <asp:ListItem Text="DETENER" Value="DETENER" /> 
                                                    <asp:ListItem Text="CANCELAR" Value="CANCELAR" /> 
                                                </asp:DropDownList> </td>
                                             
                                        </tr>
                                        <tr>
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Comentarios_Evaluacion" runat="server" Text="Comentarios"></asp:Label></td>
                                            <td width="85%">
                                                <asp:TextBox ID="Txt_Comentarios_Evaluacion" runat="server" Width="98%" TextMode="MultiLine" Rows="3" ></asp:TextBox>
                                            </td>
                                        </tr> 
                                    </table>
                               </asp:Panel>
                            </div>
                            
                            <table width="98%">
                                <tr>
                                    <td rowspan="5"></td>
                                </tr>
                            </table>
                            
                            <div id="Div_Grid_Bandeja_Entrada" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">
                                <table width="98%">                                                                    
                                    <tr>
                                        <td style="width:100%">
                                            <center>
                                                <asp:GridView ID="Grid_Bandeja_Entrada" runat="server" CssClass="GridView_1"
                                                    AutoGenerateColumns="False" Width="98%"
                                                    GridLines= "None" 
                                                    onpageindexchanging="Grid_Bandeja_Entrada_PageIndexChanging" 
                                                    onselectedindexchanged="Grid_Bandeja_Entrada_SelectedIndexChanged"
                                                    EmptyDataText="No se encuentra ningun tramite en espera">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                             <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID" 
                                                            SortExpression="SOLICITUD_ID"   >
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave Solicitud"
                                                            SortExpression="CLAVE_SOLICITUD"   >
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="14%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="14%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TRAMITE" HeaderText="Tramite" 
                                                            SortExpression="TRAMITE"   >
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="28%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="28%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                                            SortExpression="ESTATUS"   >                                                           
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SOLICITO" HeaderText="Solicito" 
                                                            SortExpression="SOLICITO"   >
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha Solicitud" 
                                                            DataFormatString="{0:dd/MMM/yyyy}" SortExpression="FECHA"   >
                                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="18%" />
                                                            <ItemStyle Font-Size="12px" HorizontalAlign="Right" Width="18%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2" Width="96%" >
                        <HeaderTemplate>Datos de la solicitud</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Grid_Datos_Solicitud" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">
                                    <%--<asp:Label ID="Lbl_Mensaje_Datos_Solicitud" runat="server" Text="No hay datos para este Subproceso"/>--%>
                                         <asp:GridView ID="Grid_Datos_Tramite" runat="server" CssClass="GridView_1"
                                            AutoGenerateColumns="False" Width="98%" GridLines= "None" 
                                            EmptyDataText="No hay datos para este Subproceso">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:BoundField DataField="OPE_DATO_ID" HeaderText="OPE_DATO_ID" SortExpression="OPE_DATO_ID" />
                                                <asp:BoundField DataField="DATO_ID" HeaderText="DATO_ID" SortExpression="DATO_ID" />
                                                <asp:BoundField DataField="NOMBRE_DATO" HeaderText="Dato" SortExpression="NOMBRE_DATO" >
                                                <ItemStyle HorizontalAlign = "Left"/>
                                                <HeaderStyle HorizontalAlign = "Left"/>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Valor">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_Valor_Dato" runat="server" Width="98%" ReadOnly="true" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign = "Left"/>
                                                    <ItemStyle HorizontalAlign = "Left"/>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                </div>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>    
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel3" ID="TabPanel3" Width="96%">
                        <HeaderTemplate>Documentacion del Tramite</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <asp:Label ID="Lbl_Documentos_Anexos" runat="server" Text="Documentos Anexados" Width="98%" CssClass="label_titulo"></asp:Label>
                                
                                   <div id="Div_Grid_Documentos_Tramite" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">
                                        <%--<asp:Label ID="Lbl_Mensaje_Documentos_Anexos" runat="server" Text="No hay documentos anexos para este Subproceso"/>--%>
                                            <asp:GridView ID="Grid_Documentos_Tramite" runat="server" CssClass="GridView_1"
                                                onrowdatabound="Grid_Documentos_Tramite_RowDataBound"
                                                AutoGenerateColumns="False" Width="98%" GridLines= "None" 
                                                EmptyDataText="No hay documentos anexos para este Subproceso">
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="OPE_DOCUMENTO_ID" HeaderText="OPE_DOCUMENTO_ID" 
                                                        SortExpression="OPE_DOCUMENTO_ID" />
                                                    <asp:BoundField DataField="DETALLE_DOCUMENTO_ID" HeaderText="DETALLE_DOCUMENTO_ID" 
                                                        SortExpression="DETALLE_DOCUMENTO_ID" />
                                                    <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento" 
                                                        HeaderStyle-Font-Size="13px" HeaderStyle-Width="90%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Font-Names="12px" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="NOMBRE_DOCUMENTO" >
                                                        <ItemStyle HorizontalAlign = "Left"/>
                                                        <HeaderStyle HorizontalAlign = "Left"/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="URL" HeaderText="URL" SortExpression="URL" />
                                                    <asp:TemplateField HeaderText="Ver"
                                                        HeaderStyle-Font-Size="13px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Font-Names="12px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                                onclick="Btn_Ver_Documento_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign = "Right"/>
                                                        <HeaderStyle HorizontalAlign = "Center"/>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                    </div>
                                    
                                    <asp:Label ID="Lbl_Documentos_Seguimiento" runat="server" Text="Documentos Creados en el Seguimiento" Width="98%" CssClass="label_titulo"></asp:Label>
                                    <%--<asp:Label ID="Lbl_Mensaje_Documentos_Seguimiento" runat="server" Text="No hay documentos de seguimiento para este Subproceso"/>--%>
                                    
                                      <div id="Div_Grid_Documentos_Seguimiento" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">
                                        <asp:GridView ID="Grid_Documentos_Seguimiento" runat="server" CssClass="GridView_1"
                                            onrowdatabound="Grid_Documentos_Seguimiento_RowDataBound"
                                            AutoGenerateColumns="False" Width="98%" GridLines= "None" 
                                            EmptyDataText="No hay documentos de seguimiento para este Subproceso">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento" 
                                                    SortExpression="NOMBRE_DOCUMENTO" 
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-Width="90%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Names="12px" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="URL" HeaderText="URL" SortExpression="URL" />
                                                
                                                <asp:TemplateField HeaderText="Ver"
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Font-Names="12px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Ver_Documento_Seguimiento" runat="server" AlternateText="Ver" 
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                            onclick="Btn_Ver_Documento_Seguimiento_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>                         
                             
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>     
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel3" ID="TabPanel4" Width="96%">
                        <HeaderTemplate>Documentos que debe elaborar para este proceso</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="98%" >
                                    <tr >
                                        <td colspan="3">
                                            <div id="Div_Grid_Plantillas" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block"> 
                                               <%--<asp:Label ID="Lbl_Mensaje_Plantillas" runat="server" Text="No hay plantillas para este Subproceso"/>--%>
                                               <asp:GridView ID="Grid_Plantillas" runat="server" CssClass="GridView_1"
                                                    OnRowDataBound="Grid_Plantillas_RowDataBound"
                                                    AutoGenerateColumns="False" Width="98%" GridLines= "None" 
                                                    EmptyDataText="No hay plantillas para este Subproceso">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <%-- 0 --%>
                                                        <asp:TemplateField HeaderText="Realizados" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-Font-Names="12px" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate >
                                                                <asp:CheckBox ID="Chk_Realizado" runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <%-- 1 --%>                                            
                                                        <asp:BoundField DataField="PLANTILLA_ID" HeaderText="Clave" SortExpression="Clave_Plantilla" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-Width="0%" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-Font-Names="12px" ItemStyle-Width="0%" ItemStyle-HorizontalAlign="Left"/>
                                                        <%-- 2 --%>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Plantilla" SortExpression="Plantilla" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-Width="75%" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-Font-Names="12px" ItemStyle-Width="75%" ItemStyle-HorizontalAlign="Left"/>
                                                         <%-- 3 --%>
                                                        <asp:BoundField DataField="ARCHIVO" HeaderText="ARCHIVO" SortExpression="Archivo" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-Width="0%" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-Font-Names="12px" ItemStyle-Width="0%" ItemStyle-HorizontalAlign="Left"/>
                                                         <%-- 4 --%>
                                                        <asp:TemplateField HeaderText="Generar"     
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-Font-Names="12px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Generar_Documento" runat="server" 
                                                                    AlternateText="Generar" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                                    onclick="Btn_Generar_Documento_Click"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>  
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>                                          
                </cc1:TabContainer>
            </div>
    <asp:Button ID="Btn_Comodin_FGC" runat="server" Text="Comodin" style="display:none;"/>
    <cc1:ModalPopupExtender ID="MPE_Crear_Plantilla" runat="server" TargetControlID="Btn_Comodin_FGC" 
        PopupControlID="Pnl_Crear_Plantilla" CancelControlID="Btn_Cancelar"    PopupDragHandleControlID="Pnl_Interno"
        DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>           
    </asp:UpdatePanel>  
    <asp:Panel ID="Pnl_Crear_Plantilla" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
            style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Plantilla" runat="server"  UpdateMode="Conditional"> 
                <ContentTemplate>
                <asp:HiddenField ID="Hdf_Plantilla_Seleccionada" runat="server" />
                    <table width="95%">
                        <tr>
                            <td style="width:100%">
                                <asp:Label ID="Lbl_Error_MPE_Crear_Plantilla" runat="server" Text="" Width="98%"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%">
                                <center>
                                    <div style="border-style:outset; height:98px; width:98%; overflow:auto;" >
                                        <asp:GridView ID="Grid_Marcadores_Platilla" runat="server" AutoGenerateColumns="False" 
                                            Width="100%" BackColor="White" BorderColor="#336666" BorderStyle="Double" 
                                            BorderWidth="3px" CellPadding="4" GridLines="Horizontal"  
                                            Font-Size="X-Small">
                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                            <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Left"/>
                                            <Columns>
                                                <asp:BoundField DataField="MARCADOR_ID" HeaderText="MARCADOR_ID" 
                                                    SortExpression="MARCADOR_ID" />
                                                <asp:BoundField DataField="NOMBRE_MARCADOR" HeaderText="Insertar" 
                                                    SortExpression="NOMBRE_MARCADOR" 
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Names="12px" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:TemplateField HeaderText="Valor"
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-Width="65%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Names="12px" ItemStyle-Width="65%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_Valor_Marcador" runat="server" Width="98%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>        
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="95%">
                <tr>
                    <td style="width:100%">
                        <center>
                            <asp:Button ID="Btn_Crear_Documento" runat="server" Text="Crear" TabIndex="202" 
                                Width="80px"  Height="26px" onclick="Btn_Crear_Documento_Click"/>
                                &nbsp;&nbsp;
                            <asp:Button ID="Btn_Cancelar" runat="server" TabIndex="202" Text="Cancelar" 
                                Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel>
</asp:Content>

