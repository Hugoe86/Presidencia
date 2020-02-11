<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Reportes.aspx.cs" Inherits="paginas_Tramites_Frm_Ope_Tra_Reportes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
     <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
        
            function selec_todo() {
                var y;
                y = $('#chkAll').is(':checked');
                var $chkBox = $("input:checkbox[id$=Chk_Tramite]");
                if (y == true) {
                    $chkBox.attr("checked", true);
                } else {
                $chkBox.attr("checked", false);
                    //        x= $('.ser').attr('checked', false);
                }
            }


            function Comparar_Estatus(ctrl) {
                var Valor = parseFloat(ctrl.value);

                if (Valor > 100) {
                    $('input[id$=Txt_Avance]').val('');
                    alert('El Valor no puede ser mayor a 100%!!');
                }
            }
            
            
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
           <div id="Div_Contenido" style="background-color: #ffffff; width: 97%; height: 100%;">
            
            <table width = "100%">
                <tr>
                    <td class="label_titulo"> Reportes de Solicitud de Tramites</td>
                </tr>
                  <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td>
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    </td>            
                                </tr>
                                <tr>
                                    <td style="width:10%;">              
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                    </td>
                                </tr>          
                            </table>                   
                        </div>
                </td>
            </tr>
        </table>
            <table width = "100%">
                <tr>
                    <td class ="barra_busqueda">
                        &nbsp;
                        <asp:ImageButton ID="Btn_Reporte" runat="server" CssClass ="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" ToolTip="Generar Reporte" 
                            onclick="Btn_Reporte_Click" />
                        <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" CssClass ="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" ToolTip="Exportar a Excel" 
                            onclick="Btn_Exportar_Excel_Click" />
                        <asp:ImageButton ID="Btn_Limpiar" runat="server" CssClass ="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                            ToolTip="Limpiar Formulario" onclick="Btn_Limpiar_Click" Width="24px"  />
                        <asp:ImageButton ID="Btn_Salir" runat="server" CssClass ="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                            onclick="Btn_Salir_Click" />
                 
                    
                    
                    <%-- --%>
                      </td>    
                </tr>
            </table>
     <%--   
        <table width="100%">
            <tr>
                <td style="width:100%" align="left">
                    <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="Blue" 
                        onclick="Btn_Busqueda_Avanzada_Click">Búsqueda Avanzada Tramites</asp:LinkButton>
                </td>
            </tr>
        </table>
        --%>
        
            <div id="Div_Fechas" runat="server" >
                <asp:Panel ID="Pnl_Fechas" runat ="server" GroupingText="Fecha" Style="display: block; margin-top: 8px; background: #E6E6E6;">
                    <table width="100%">
                        <tr>
                            <td style="width:15%">
                                <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text ="Inicio"></asp:Label>
                            </td>
                             <td style="width:35%"> 
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" MaxLength="11" Enabled="false" 
                                    Width="75%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Inicio" 
                                        PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                    <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                             </td>
                             <td style="width:15%" align="left">
                                <asp:Label ID="Lbl_Fecha_Fin" runat="server" Text ="Fin"></asp:Label>
                             </td>
                             <td style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" MaxLength="11" Enabled="false"
                                    Width="75%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Fin" Format ="dd/MMM/yyyy"
                                        PopupButtonID="Btn_Fecha_Fin">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                             </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        
        
            <asp:Panel ID="Pnl_Pendientes_Pago" runat="server" GroupingText="Pendientes de pago">
                <table width="100%" style="display:block">
                   <tr>
                        <td  style="width:15%">
                            <asp:CheckBox ID="Chk_Pendientes_Pago"  Text = "Pendientes" runat="server" Width="100%"
                              />
                        </td>
                        <td  style="width:85%">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="95%" 
                                DropDownStyle="DropDownList" 
                                AutoCompleteMode="SuggestAppend" 
                                CssClass="WindowsStyle" MaxLength="0" />
                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Dependencia_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        
        
            <asp:Panel ID="Pnl_Filtros" runat="server" GroupingText="Filtros">
                <table width="100%" style="display:none">
                    <tr>
                        <td style="width:15%">
                            <asp:CheckBox ID="Chk_Vigencia"  Text="Vigencia del" runat="server" Width="100%" />
                        </td>
                         <td style="width:35%"> 
                            <asp:TextBox ID="Txt_Vigencia_Inicio" runat="server" MaxLength="11" Enabled="false" 
                                Width="75%"></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Vigencia_Inicio" 
                                    PopupButtonID="Btn_Vigencia_Inicio" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Vigencia_Inicio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                         </td>
                         <td style="width:15%" align="left">
                            <asp:Label ID="Lbl_Vigencia_Fin" runat="server" Text ="al"></asp:Label>
                         </td>
                         <td style="width:35%" align="left">
                            <asp:TextBox ID="Txt_Vigencia_Fin" runat="server" MaxLength="11" Enabled="false"
                                Width="75%"></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_Fin_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Vigencia_Fin" Format ="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Vigencia_Fin">
                                </cc1:CalendarExtender>
                                <asp:ImageButton ID="Btn_Vigencia_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                         </td>
                    </tr>
                    <tr>
                        <td style="width:15%">
                            <asp:CheckBox ID="Chk_Vigencia_Documento"  Text="Vigencia del doc" runat="server" Width="100%" />
                        </td>
                         <td style="width:35%"> 
                            <asp:TextBox ID="Txt_Vigencia_Documento_Inicio" runat="server" MaxLength="11" Enabled="false" 
                                Width="75%"></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_Documento_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Vigencia_Documento_Inicio" 
                                    PopupButtonID="Btn_Vigencia_Documento_Inicio" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Vigencia_Documento_Inicio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                         </td>
                         <td style="width:15%" align="left">
                            <asp:Label ID="Lbl_Vigencia_Documento_Fin" runat="server" Text ="al"></asp:Label>
                         </td>
                         <td style="width:35%" align="left">
                            <asp:TextBox ID="Txt_Vigencia_Documento_Fin" runat="server" MaxLength="11" Enabled="false"
                                Width="75%"></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_Documento_Fin_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Vigencia_Documento_Fin" Format ="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Vigencia_Documento_Fin">
                                </cc1:CalendarExtender>
                                <asp:ImageButton ID="Btn_Vigencia_Documento_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                         </td>
                    </tr>
                </table>
            
                <table width="100%">
                    <tr>
                        <td style="width:15%" align="left">
                           <asp:CheckBox ID="Chk_Filtro_Dependencia"  Text = "Dependencia" runat="server" Width="100%" />
                        </td>
                        <td style="width:85%" align="left">
                            <asp:DropDownList ID="Cmb_Filtro_Dependencia" runat="server" Width="95%" 
                                DropDownStyle="DropDownList" 
                                AutoCompleteMode="SuggestAppend" 
                                CssClass="WindowsStyle" MaxLength="0" />
                            <asp:ImageButton ID="Btn_Filtro_Dependnecia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Filtro_Dependnecia_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td  style="width:15%">
                            <asp:CheckBox ID="Chk_Estatus"  Text = "Estatus" runat="server" Width="100%"  />
                        </td>
                        <td style="width:85%"> 
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width = "95%" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td  style="width:15%" >
                            <asp:CheckBox ID="Chk_Solicitud_Demorada"  Text = "Solicitud:" runat="server" Width="100%" />
                        </td>
                        <td  style="width:85%">
                            <asp:DropDownList ID="Cmb_Tipo_Estatus" runat="server" Width = "95%" >
                                <asp:ListItem>< SELECCIONAR ></asp:ListItem>
                                <asp:ListItem>Sin demora</asp:ListItem>
                                <asp:ListItem>Proximo a vencerse</asp:ListItem>
                                <asp:ListItem>Vencidos</asp:ListItem>
                            </asp:DropDownList>
                            
                            <asp:CheckBox ID="Chk_Sin_Demora"  Text = "Sin demora" runat="server" Width="100%"  style="display:none; " />
                            <asp:CheckBox ID="Chk_Proximo"  Text = "Proximo a vencerse" runat="server" Width="100%" style="display:none; " />
                            <asp:CheckBox ID="Chk_Vencido"  Text = "Vencidos" runat="server" Width="100%" style="display:none; " />
                        </td>
                    </tr>
                    <tr>
                        <td  style="width:15%">
                            <asp:CheckBox ID="Chk_Cuenta_Predial"  Text = "Cuenta Predial" runat="server" Width="100%"
                              />
                        </td>
                        
                        <td  style="width:85%">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="94%" MaxLength="12"></asp:TextBox>
                         </td>
                    </tr> 
                    <tr style="display:none">
                        <td  style="width:15%">
                            <asp:CheckBox ID="Chk_Perito"  Text = "Perito" runat="server" Width="100%"/>
                        </td>
                        <td  style="width:85%">
                               <asp:DropDownList ID="Cmb_Perito" runat="server" Width="95%">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Perito" runat="server" ToolTip="Seleccionar un Perito"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Perito_Click" />
                         </td>
                    </tr> 
                    <tr>
                        <td  style="width:15%">
                            <asp:CheckBox ID="Chk_Solicitante"  Text = "Solicitante" runat="server" Width="100%"/>
                        </td>
                        
                        <td  style="width:85%">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" Width="94%" MaxLength="150" ToolTip="Nombre Apellido_Paterno Apellido_Materno" ></asp:TextBox>
                         </td>
                    </tr> 
                </table>
            </asp:Panel>
        
            <div id="Div_Tramites" runat="server" >
                <asp:Panel ID="Pnl_Tramites" runat ="server" GroupingText="Trámites">
                    <table width="100%">
                        <tr style="display:block">
                            <td style="width:15%">
                                <asp:Label ID="Lbl_Tramites_Todos" runat="server" Text="Todos" ></asp:Label>   
                                <input type="checkbox"  id="chkAll" onclick="selec_todo();"/>
                            </td>
                            <td style="width:85%"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <div id="Div_Grid_Tramites" runat="server" 
                                        style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Tramites" runat="server" Width="97%" 
                                            OnSorting="Grid_Tramites_Sorting" AllowSorting="True"
                                            CssClass="GridView_1" GridLines="None" AutoGenerateColumns="False">
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:TemplateField HeaderText=""
                                                     HeaderStyle-Font-Size="12px" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center"
                                                     ItemStyle-Font-Size="10px" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate >
                                                        <center>
                                                            <asp:CheckBox ID="Chk_Tramite" runat="server" AutoPostBack="True" oncheckedchanged="Chk_Tramite_CheckedChanged"/>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre del tramite" SortExpression="NOMBRE">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" Font-Size="12px"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" Font-Size="12px"/>
                                                </asp:BoundField> 
                                                <%-- 2 --%>
                                                <asp:BoundField DataField="TRAMITE_ID" HeaderText="id">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField> 
                                                <%-- 3 --%>
                                                <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Resp." SortExpression="NOMBRE_DEPENDENCIA">
                                                    <HeaderStyle HorizontalAlign="Left" Width="25%" Font-Size="12px"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="10px"/>
                                                </asp:BoundField> 
                                              
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
                </asp:Panel>
            </div>
        
        
        
            <table width="100%" style="display:none">
                <tr>
                    <td  style="width:15%">
                        <asp:CheckBox ID="Chk_Avance" runat="server" Text="Avance (%)" 
                            oncheckedchanged="Chk_Avance_CheckedChanged" AutoPostBack="true"/>
                    </td>
                    <td  style="width:85%">
                        <asp:TextBox ID="Txt_Avance" runat="server" MaxLength="3" Width="20%"
                            name="<%=Txt_Avance.ClientID %>"  style="text-align:right"
                            >
                        </asp:TextBox>
                            <%--    <cc1:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" 
                            targetcontrolid="Txt_Avance" Maximum="100" Minimum="0" Width="50">
                            </cc1:NumericUpDownExtender>--%>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                            TargetControlID="Txt_Avance" ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td  style="width:15%">
                        
                    </td>
                     <td  style="width:15%">
                        <asp:Label ID="Lbl_Nombre_Propietario" runat="server" Text="Propietario"></asp:Label>
                     </td>
                    <td  style="width:70%">
                        <asp:TextBox ID="Txt_Propietario" runat="server" Width="95%" ></asp:TextBox>
                     </td>
                </tr>
            </table>  
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>