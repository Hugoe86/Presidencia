<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Cotizacion_Manual_PULL.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Cotizacion_Manual_PULL" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPiRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);        
    //-->
   </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
               <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td  colspan="4" class="label_titulo">Cotizacion Manual</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="font-size:9px;width:90%;text-align:left; width:95%;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                            </td>
                        </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila de Busqueda y Botones Generales --%>
            <tr class="barra_busqueda">
                    <td style="width:20%;" colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            ToolTip="Cotizar" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click" Width="24px"/>
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" style="display:none"
                                onclick="Btn_Imprimir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

            </tr>
             <tr>
              
                <td colspan="4">
                <div id="Div_Busqueda" style="width:100%;" runat="server">
                    <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="width:20%">
                               Requisicion 
                            </td>
                            <td style="width:40%">
                                <asp:TextBox ID="Txt_Requisicion_Busqueda" runat="server" Width="80%" MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                TargetControlID="Txt_Requisicion_Busqueda"  
                                FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9"
                                Enabled="True" InvalidChars="<,>,&,',!,">   
                                </cc1:FilteredTextBoxExtender>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"/>
                            </td>
                            <td style="width:40%" align="right">
                                  
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Fecha_Elaboracion" runat="server" 
                                    Text="Fecha de Elaboracion" AutoPostBack="true" style="display:none"
                                    oncheckedchanged="Chk_Fecha_Elaboracion_CheckedChanged"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Busqueda_Fecha_Elaboracion_Ini" runat="server" Width="80%" style="display:none"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Elaboracion_Ini" runat="server" style="display:none"
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Busqueda_Fecha_Elaboracion_Ini" TargetControlID="Txt_Busqueda_Fecha_Elaboracion_Ini"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Busqueda_Fecha_Elaboracion_Fin" runat="server" Width="80%" style="display:none"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Elaboracion_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" style="display:none"
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Busqueda_Fecha_Elaboracion_Fin" TargetControlID="Txt_Busqueda_Fecha_Elaboracion_Fin"/>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Vigencia_Propuesta" runat="server" style="display:none"
                                    Text="Vigencia de la Propuesta" 
                                    oncheckedchanged="Chk_Vigencia_Propuesta_CheckedChanged"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Busqueda_Vigencia_Propuesta_Ini" runat="server" Width="80%" style="display:none"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Busqueda_Vigencia_Propuesta_Ini" runat="server" style="display:none"
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender7" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Busqueda_Vigencia_Propuesta_Ini" TargetControlID="Txt_Busqueda_Vigencia_Propuesta_Ini"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Busqueda_Vigencia_Propuesta_Fin" runat="server" Width="80%" style="display:none"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Busqueda_Vigencia_Propuesta_Fin" runat="server" style="display:none"
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender8" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Busqueda_Vigencia_Propuesta_Fin" TargetControlID="Txt_Busqueda_Vigencia_Propuesta_Fin"/>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                            
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="Cmb_Cotizador" runat="server" Width="90%" style="display:none">
                                </asp:DropDownList>
                                
                            </td>
                            
                        </tr>
                <tr>
                <td colspan="4">
                    <div ID="Div_Grid_Requisiciones" runat="server" style="overflow:auto;height:500px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Requisiciones" runat="server"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" 
                                    Width="99%" Enabled ="False" DataKeyNames="No_Requisicion"
                                    AllowSorting="True" OnSorting="Grid_Requisiciones_Sorting" 
                                    HeaderStyle-CssClass="tblHead" >
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver Requisicion" HeaderText="Ver"
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="Proveedor_ID" HeaderText="Proveedor_ID" Visible="false">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="true" SortExpression="Folio" ItemStyle-Wrap="true">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tipo_Articulo" HeaderText="Tipo Articulo" Visible="True" SortExpression="Tipo_Articulo" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="20%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                                                Visible="True" SortExpression="Estatus" ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="20%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Nombre_Proveedor" HeaderText="Proveedor" 
                                                Visible="True" SortExpression="Nombre_Proveedor" ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="20%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha Solicitud" 
                                                Visible="True" SortExpression="Fecha" ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="20%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                         </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                    </div>
                </td>
            </tr>
            </table>
            </div>
            </td>
            </tr>
            <tr>
            <td colspan="4">
               <div ID="Div_Detalle_Requisicion" runat="server" style="width:100%;font-size:9px;" 
                    visible="false">
                    <table width="99%">
                        <tr>
                            <td align="center" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width:17%">
                                Unidad responsable</td>
                            <td style="width:83%" colspan="3">
                                <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="False" Width="100%"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td style="width:17%">
                                Concepto</td>
                            <td style="width:83%" colspan="3">
                                <asp:TextBox ID="Txt_Concepto" runat="server" Enabled="False" Width="100%"></asp:TextBox>
                            </td>

                        </tr>                        
                        <tr>
                            <td>
                                Folio
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Folio" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">
                                Fecha Generación</td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Enabled="False" 
                                Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo</td>
                            <td>
                                <asp:TextBox ID="Txt_Tipo" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">
                                Tipo Articulo
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Tipo_Articulo" runat="server" Enabled="false" 
                                Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estatus
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="false" 
                                Width="99%"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">
                                Compra especial
                            </td>                            
                            <td >
                                <asp:CheckBox ID="Chk_Verificacion" runat="server" Enabled="false" Visible="false"
                                Text="Verificar las características, garantías y pólizas" />
                                <asp:TextBox ID="Txt_Compra_Especial" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Justificación
                            <br />
                                de la Compra</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="true" Height="70px" ReadOnly="true"
                                TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                Especificaciones
                            <br />
                                Adicionales</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Especificacion" runat="server" Enabled="False" 
                                TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                TargetControlID="Txt_Especificacion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Especificaciones de los productos&gt;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="barra_delgada">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                Detalle de Cotización
                            </td>
                        
                        </tr>
                        <tr>
                            <td>
                                Proveedor
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Proveedor" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                No. Padrón
                            </td>
                            <td >
                                <asp:TextBox ID="Txt_Reg_Padron_Prov" runat="server" Width="99%" MaxLength="20" Visible="true" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                TargetControlID="Txt_Reg_Padron_Prov"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/ "
                                Enabled="True" InvalidChars="'"></cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align:right;">
                                Estatus Propuesta
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus_Propuesta" runat="server" Width="98%">
                                    <asp:ListItem Value="COTIZADA">COTIZADA</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="EN CONSTRUCCION">EN CONSTRUCCION</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *Vigencia de propuesta                                
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Vigencia" runat="server" Width="80%" Enabled="false" Visible="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Vigencia" />
                            </td>
                            <td style="text-align:right;">
                                *Fecha Elaboración</td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Elaboracio" runat="server" Width="99%" Enabled="false" Visible="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                *Garantía 
                            </td>

                            <td>
                                <asp:TextBox ID="Txt_Garantia" runat="server" Width="99%" Visible="true"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="Tbw_Garantia" runat="server" 
                                TargetControlID="Txt_Garantia" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Indicar la Garantia&gt;" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                TargetControlID="Txt_Garantia"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/ "
                                Enabled="True" InvalidChars="'"></cc1:FilteredTextBoxExtender>
                            </td>
                            
                            <td class="style1" style="text-align:right;">
                                *Tiempo de Entrega
                            </td>
                            
                            <td class="style1">
                                 <asp:TextBox ID="Txt_Tiempo_Entrega" runat="server" Width="50%" Enabled="false" Visible="true" MaxLength="4"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                TargetControlID="Txt_Tiempo_Entrega"  
                                FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9,."
                                Enabled="True" InvalidChars="<,>,&,',!,">   
                                </cc1:FilteredTextBoxExtender>Días Hábiles
                                
                            </td>
                        
                        </tr>
                        <tr >
                            <td>
                                Firma cotización
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Elaboro_Cotizacion" runat="server"  
                                 Width="99%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                TargetControlID="Txt_Elaboro_Cotizacion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Nombre de quién elabora cotización&gt;" />
                            </td>
                        </tr>                           
                        <tr>
                            <td colspan="4">
                             
                            </td>
                        </tr>
                        <tr>
                            <td colspan = "4">
                            <asp:GridView ID="Grid_Productos" runat="server"
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Both" 
                            Width="99%" DataKeyNames="Ope_Com_Req_Producto_ID"
                            AllowSorting="True" OnSorting="Grid_Productos_Sorting" 
                            HeaderStyle-Font-Size="X-Small">
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <HeaderStyle CssClass="tblHead" />
                            <AlternatingRowStyle CssClass="GridAltItem" />  
                                                      
                            <Columns>
                            <asp:BoundField DataField="Ope_Com_Req_Producto_ID" HeaderText="Ope_Com_Req_Producto_ID" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Porcentaje_Impuesto" HeaderText="Porcentaje_Impuesto" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Subtotal_Cotizado" HeaderText="Subtotal_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                             <asp:BoundField DataField="Prod_Serv_ID" HeaderText="Prod_Serv_ID" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="IVA_Cotizado" HeaderText="IVA_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="IEPS_Cotizado" HeaderText="IVA_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Precio_U_Con_Imp_Cotizado" HeaderText="Precio_U_Con_Imp_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                  
                            <asp:BoundField DataField="Clave" HeaderText="Clave" HeaderStyle-Font-Size="X-Small" ItemStyle-VerticalAlign="Top"
                            SortExpression="Clave" Visible="True">
                            
                            <HeaderStyle HorizontalAlign="Left" Width="5%" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Left" Width="5%" Font-Size="X-Small"/>
                            </asp:BoundField>
                                                        
 <%--                           <asp:BoundField DataField="Nombre" HeaderText="Producto/Servicio" 
                            SortExpression="Nombre" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                            </asp:BoundField>--%>
                            
                            <asp:BoundField DataField="Nombre_Descripcion" HeaderText="Descripción" ItemStyle-VerticalAlign="Top"
                            SortExpression="Descripcion" Visible="true">
                            <HeaderStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Descripcion_Producto_Cot" HeaderText="Descripcion" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Cantidad" HeaderText="Ctd" 
                            SortExpression="Cantidad" Visible="True">
                            <HeaderStyle HorizontalAlign="Center" Width="6%" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Center" Width="6%" Font-Size="X-Small" VerticalAlign="Top"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Marca" HeaderText="Marca" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                            </asp:BoundField>
                            

                            
                            <asp:BoundField DataField="Precio_U_Sin_Imp_Cotizado" HeaderText="Precio_U_Sin_Imp_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Marca" ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Marca" runat="server" Width="98%" MaxLength="100" Font-Size="X-Small" Height="45px"></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                TargetControlID="Txt_Marca"  
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/ "
                                Enabled="True" InvalidChars="'">   
                                </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                               <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripcion Producto Cotizado" ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Descripcion_Producto_Cot" runat="server" Width="98%" Font-Size="X-Small" TextMode="MultiLine" MaxLength="200" Height="45px"></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" 
                                TargetControlID="Txt_Descripcion_Producto_Cot"  
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/ "
                                Enabled="True" InvalidChars="'">   
                                </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Total_Cotizado" HeaderText="Importe" 
                            SortExpression="Monto_Total" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small" VerticalAlign="Top"/>
                            </asp:BoundField>                            
                            <asp:TemplateField HeaderText="$ Unitario S/I" ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Precio_Unitario" runat="server" Width="98%" MaxLength="20" Font-Size="X-Small" Style="text-align:right; vertical-align:bottom;" ></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                TargetControlID="Txt_Precio_Unitario"  
                                FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9."
                                Enabled="True" InvalidChars="<,>,&,',!,">   
                                </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                                <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:TemplateField>
                            
                            </Columns>

                            </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            
                            <td colspan="2" align="right">
                                <asp:Button ID="Btn_Calcular_Precios_Cotizados" runat="server" 
                                    Text="Calcular Precios Cotizados" CssClass="button" Width="200px" 
                                    onclick="Btn_Calcular_Precios_Cotizados_Click"/>
                            </td>
                            <td align="right" colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        Subtotal Cotizado
                                    </td>
                                
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_SubTotal_Cotizado_Requisicion" runat="server" Style="text-align:right;" Enabled="false" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                            <td colspan="2">
                                
                            </td>
                            <td colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        IVA Cotizado
                                    </td>
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_IVA_Cotizado" runat="server" Enabled="false"  Style="text-align:right;" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                            <td colspan="2">
                                
                            </td>
                            <td colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        IEPS Cotizado
                                    </td>
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_IEPS_Cotizado" runat="server" Enabled="false"  Style="text-align:right;" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width:50%">
                                            Total Cotizado
                                        </td>
                                        <td align="right" style="width:50%">
                                            <asp:TextBox ID="Txt_Total_Cotizado_Requisicion" runat="server"  Style="text-align:right;" Enabled="false" 
                                                Width="99%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            </tr>
                        </table>
                        </div>
                        </td>
                        </tr>
        </table>
        </div>
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>