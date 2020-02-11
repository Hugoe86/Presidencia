<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
CodeFile="Frm_Ope_Ort_Bitacora_Prestamo_Expediente.aspx.cs" 
Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Bitacora_Prestamo_Expediente" %>

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
        //  abre el grid anidado
        function Mostrar_Tabla(Renglon, Imagen) {
        object = document.getElementById(Renglon);
        if (object.style.display == "none") {
            object.style.display = "block";
            document.getElementById(Imagen).src = " ../../paginas/imagenes/paginas/stocks_indicator_down.png";
        } else {
            object.style.display = "none";
            document.getElementById(Imagen).src = "../../paginas/imagenes/paginas/add_up.png";
        }
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
           <div id="Div_Contenido" runat="server" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width = "98%" border="0" cellspacing="0">                     
                    <tr>
                        <td colspan ="4" class="label_titulo">Bitácora de prestamos de expediente</td>
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
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""
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
                        <td colspan="2" align = "right">
                               
                           
                        </td> 
                    </tr>
                </table>
                
            <div id="Div_Solicitud" runat="server">
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
                </table>
                
                <table width="98%">            
                    <tr>  
                        <td  style="width:100%" align="center" rowspan="5">    
                            <asp:HiddenField ID="Hdf_Accion" runat="server" />   
                            <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />   
                            <asp:HiddenField ID="Hdf_Documento_ID" runat="server" />        
                        </td>
                    </tr>
                </table>
                
                <%-- Manejo del Grid View--%>
                    <div id="Div_Grid_Bitacora_Documentos" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Bitacora" runat="server" 
                                        style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Bitacoras" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" Width="100%"  
                                            EmptyDataText="No se encontraron documentos"  
                                            OnRowDataBound="Grid_Bitacoras_RowDataBound"                                            
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Img_Btn_Expandir" runat="server"
                                                            ImageUrl="~/paginas/imagenes/paginas/stocks_indicator_down.png" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="3%" />                                                
                                                </asp:TemplateField>
                                                <%-- 1 --%>                                                
                                                <asp:BoundField DataField="BITACORA_ID" HeaderText="Bitacora_ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 2 --%>
                                                <asp:BoundField DataField="SOLICITUD_ID" HeaderText="Solicitud_ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 3 --%>  
                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="Subproceso_id" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>     
                                                <%-- 4 --%>  
                                                <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Nombre" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField> 
                                                <%-- 5 --%> 
                                                 <asp:TemplateField HeaderText="Prestamo"
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="12px" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chck_Prestamo" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                            
                                                <%-- 6 --%>  
                                                <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField> 
                                                <%-- 7 --%>  
                                                <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="50%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="50%" />
                                                </asp:BoundField>                                                
                                                <%-- 8 --%>  
                                                <asp:TemplateField HeaderText="Realizar"
                                                    HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="12px" ItemStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Btn_Accion_Realizar" runat="server" Text="" style="font-size:12px"
                                                            OnClick="Btn_Accion_Realizar_Click"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                            
                                                 <%-- 9 --%>  
                                                <asp:BoundField DataField="ESTATUS_PRESTAMO" HeaderText="estatus del prestamo" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>                                                
                                                <%-- 10 --%>
                                                
                                                <asp:TemplateField>
                                                    <ItemTemplate> 
                                                      
                                                            <asp:Label ID="Lbl_Actividades" runat="server" 
                                                                Text='<%# Bind("DOCUMENTO_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Literal ID="Ltr_Inicio" runat="server" 
                                                                Text="&lt;/td&gt;&lt;tr id='Renglon_Grid' 
                                                                style='display:block;position:static'&gt;&lt;td colspan='4';left-padding:30px;&gt;" />
                                                              <center>
                                                                  <asp:GridView ID="Grid_Detalles_Bitacora" runat="server" AllowPaging="False"
                                                                    OnRowDataBound="Grid_Detalles_Bitacora_RowDataBound"
                                                                    OnRowCommand="Grid_Detalles_Bitacora_RowCommand"
                                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="95%">
                                                                    <Columns>
                                                                        <%-- 0 --%>
                                                                        <asp:BoundField DataField="DETALLE_BITACORA_ID" HeaderText="" Visible="false" >
                                                                        </asp:BoundField>                                                                 
                                                                        <%-- 1 --%>
                                                                        <asp:BoundField DataField="FECHA_PRESTAMO" HeaderText="Fecha prestamo" Visible="True"
                                                                            DataFormatString="{0:dd/MMM/yyyy}"
                                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%"
                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="12px" ItemStyle-Width="30%">
                                                                        </asp:BoundField> 
                                                                        <%-- 2 --%> 
                                                                        <asp:BoundField DataField="FECHA_DEVOLUCION" HeaderText="Fecha devolucion" Visible="True"
                                                                            DataFormatString="{0:dd/MMM/yyyy}"
                                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%"
                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="12px" ItemStyle-Width="30%">
                                                                        </asp:BoundField>                                                                                                 
                                                                        <%-- 3 --%> 
                                                                         <asp:BoundField DataField="USUARIO_PRESTAMO" HeaderText="Prestamo realizado" Visible="True"
                                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40%"
                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="12px" ItemStyle-Width="40%">
                                                                        </asp:BoundField> 
                                                                         <%-- 4 --%> 
                                                                        <asp:TemplateField HeaderText="Imprimir"
                                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="12px" ItemStyle-Width="12%">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="Btn_Imprimir" runat="server"  Width="22px" Height="22px"
                                                                                    CommandName="Imprimir"
                                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                                                                    OnClick="Btn_Imprimir_Click"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>                                                              
                                                                    </Columns>
                                                                    <PagerStyle CssClass="GridHeader" />
                                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                                    <HeaderStyle CssClass="GridHeader" />                                
                                                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                                                </asp:GridView> 
                                                            </center>
                                                        <asp:Literal ID="Ltr_Fin" runat="server"  Text="&lt;/td&gt;&lt;/tr&gt;" /> 
                                                    </ItemTemplate>
                                            </asp:TemplateField>
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
            </div>
            
            <asp:Panel ID="Pnl_Prestamo" runat="server" GroupingText="Prestamo" style="display:none" Width="98%">
                <div id="Div1" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:15%;">
                                * Solicitud
                            </td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Solicitud" runat="server" Width="95%" ></asp:TextBox>
                            </td>
                            <td style="width:15%;" align="right">
                                * Nombre del documento
                            </td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Nombre_Documento" runat="server" Width="95%" ></asp:TextBox>
                            </td>                        
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                * Fecha Prestamo
                            </td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Prestamo" runat="server" Width="90%" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Prestamo" runat="server" 
                                    TargetControlID="Txt_Fecha_Prestamo" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Prestamo_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Prestamo" PopupButtonID="Btn_Fecha" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />   
                            </td>
                            <td style="width:15%;" align="right">
                                * Fecha devolucion
                            </td>
                            <td style="width:35%;" >
                                <asp:TextBox ID="Txt_Fecha_Devolucion" runat="server" Width="90%" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Devolucion" runat="server" 
                                    TargetControlID="Txt_Fecha_Devolucion" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    TargetControlID="Txt_Fecha_Devolucion" PopupButtonID="Btn_Fecha_Devolucion" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Devolucion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />   
                            </td>

                        </tr>
                        <tr>
                            <td style="width:15%;" align="left">
                                * Nombre de la Persona
                            </td>
                            <td style="width:35%;" colspan="2">
                                <asp:DropDownList ID="Cmb_Nombre" runat="server" Width="95%"  ></asp:DropDownList>
                            </td> 
                                              
                        </tr>
                    </table>
                    <table width="98%">
                        <tr>
                            <td style="width:15%;">
                                * Ubicación fisico del expediente
                            </td>
                            <td style="width:85%;">
                                <asp:TextBox ID="Txt_Ubicacion" runat="server" Width="98%" MaxLength="150" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            </td>                 
                        </tr> 
                        <tr>
                            <td style="width:15%;">
                                * Observaciones
                            </td>
                            <td style="width:85%;">
                                <asp:TextBox ID="Txt_Obsevaciones" runat="server" Width="98%" MaxLength="200" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            </td>                 
                        </tr> 
                    </table>
                </div>
            </asp:Panel>
            
             <asp:Panel ID="Pnl_Encuesta" runat="server" GroupingText="Encuesta" style="display:block" Width="98%">
                <div id="Div_Encuesta" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:30%;">
                                * 1.- La informacion solicitada cumple satisfactoriamente
                            </td>
                            <td style="width:20%;">
                                <asp:RadioButtonList ID="RbtL_Satisfaccion" runat="server" Width="95%" >
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                    <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="width:30%;" align="right">
                                * 2.- Tiempo que se tardo en atenderlo
                            </td>
                            <td style="width:20%;">
                                <asp:TextBox ID="Txt_Tiempo_Espera" runat="server" Width="80%" ></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tiempo_Espera" runat="server" 
                                        FilterType="Numbers" TargetControlID="Txt_Tiempo_Espera" 
                                        ValidChars="0123456789" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        min
                            </td>                        
                        </tr>
                    </table>
                </div>
            </asp:Panel>   
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>