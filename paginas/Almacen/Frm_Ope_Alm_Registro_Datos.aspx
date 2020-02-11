<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Registro_Datos.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Registro_Datos" Title="Registro de Datos" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPiRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

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
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Registro_Datos" runat="server"  AsyncPostBackTimeout="3600" 
        EnableScriptGlobalization="true" EnableScriptLocalization="true"/>
    <%--<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>--%>
        <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
            <ContentTemplate>
             <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
<%--                <ProgressTemplate>
                  <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>--%>
               </asp:UpdateProgress>
     <div id="Div_General" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del Div General--%>
        <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%"> <%--Tabla General--%>
            <tr style="width:98%" align="center">
                 <td class="label_titulo"> 
                    <asp:Label ID="Lbl_Titulo" runat="server" Text="Registro de Datos" class="label_titulo"></asp:Label> 
                 </td>
            </tr>
            <tr style="width:98%">
                <td>
                  <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                    <table style="width:100%">
                      <tr>
                        <td colspan="2" align="left" >
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
        
           <tr style="width:98%">
               <td style="width:63%" >&nbsp;</td>                        
           </tr>
            <tr class="barra_busqueda" style="width:98%" >
                <td align="left" >
                    <asp:ImageButton ID="Btn_Guardar" runat="server" 
                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Width="24px" CssClass="Img_Button" 
                    AlternateText="Guardar" ToolTip="Guardar Registro Datos" 
                    OnClientClick="return confirm('¿Está seguro de guardar el registro de datos?');"
                    OnClick="Btn_Guardar_Click" Visible="False"/>
                    <asp:ImageButton ID="Btn_Salir" runat="server"
                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                     AlternateText="Salir" ToolTip="Salir" onclick="Btn_Salir_Click"/>
                </td>     
                                   
                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                         TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                         FilterType="Numbers"
                         Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                        WatermarkCssClass="watermarked"
                        WatermarkText="<No. Orden Compra>"
                        TargetControlID="Txt_Busqueda" />
                                
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                         TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                         FilterType="Numbers"
                         Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                        WatermarkCssClass="watermarked"
                        WatermarkText="<No. Requisición>"
                        TargetControlID="Txt_Req_Buscar" />
            </tr>
            
            <div id="Div_Busqueda_Av" runat="server" style="width: 98%; height: 100%;"> 
                <tr>
                     <td >             
                             <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                                <tr>                         
                                    <td align="left">
                                        <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                        oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                            &nbsp;&nbsp;   
                                     </td>
                                     <td align="left" style="width:40%;">
                                         &nbsp;<asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="110px" 
                                        Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="Img_Btn_Fecha_Inicio" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                             ToolTip="Seleccione la Fecha Inicial" />
                                         &nbsp;&nbsp;&nbsp;&nbsp;
                                        <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicio_CalendarExtender" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Img_Btn_Fecha_Inicio" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                    
                                    <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                            ToolTip="Seleccione la Fecha Final" />
                                    <cc1:CalendarExtender ID="Img_Btn_Fecha_Fin_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Fin" PopupButtonID="Img_Btn_Fecha_Fin" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                    <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                     <asp:TextBox ID="Txt_Req_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        onclick="Btn_Buscar_Click" ToolTip="Buscar" />
                                         &nbsp;<asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                        onclick="Btn_Limpiar_Click" ToolTip="Limpiar"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                             </table>
                     </td>
                </tr>
            </div >
            
            <tr>
                <td>
                    <div id="Div_Ordenes_Compra" visible="true" runat="server"   style="height:100%;width:99%"; > <%-- Div Ordenes de Compra--%>
                        <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                            <tr>
                                <td >
                                  <asp:GridView ID="Grid_Ordenes_Compra" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        ForeColor="#333333" GridLines="None"  style="white-space:normal;" 
                                        Width="98%" PageSize="1">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                             <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_OC" runat="server" 
                                                    CommandArgument='<%# Eval("NO_ORDEN_COMPRA") %>' 
                                                    CommandName="Seleccionar_Orden_Compra" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_OC_Click" ToolTip="Ver" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No. Orden Compra" 
                                                Visible="False" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FOLIO" 
                                                HeaderText="O. Compra">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="REQUISICION" HeaderText="Requisición">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_CONSTRUCCION" HeaderText="Fecha" 
                                                DataFormatString="{0:dd/MMM/yyyy}" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROVEEDOR" 
                                                HeaderText="Proveedor" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:c}" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NO_REQUISICION" HeaderText="NO_REQUISICION" Visible="False" />
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>  
                                </td>
                            </tr>
                        </table>
                    </div> <%--Fin Div Ordenes de Compra--%>
                </td>
              </tr>
            
            <tr>
                <td>
                    <div id="Div1_Datos_G_OC" visible="false" runat="server" style="height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" > <%-- Div Ordenes de Compra--%>
                    <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                        <tr>
                            <td align="left" style="width:20%;">
                                <asp:Label ID="Lbl_Orden_Compra" runat="server" Text="Orden Compra"></asp:Label>
                            </td>
                            <td align="left" style="width:20%;">
                                <asp:TextBox ID="Txt_Orden_Compra" Enabled="False" runat="server" Width="95%"></asp:TextBox>
                            </td>
                            <td align="left" style="width:20%;">
                                <asp:Label ID="Lbl_Requisicion" runat="server" Text="Requisición"></asp:Label>
                            </td>
                            <td align="left" style="width:20%;">
                                <asp:TextBox ID="Txt_Requisicion" Enabled="False" Width ="90%" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:05%;">
                                <asp:Label ID="Lbl_Factura" runat="server" Text="Factura"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Factura" Enabled="False" runat="server" Width="95%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Fecha_Factura" runat="server" Text="Fecha Recepción"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Resepcion" Enabled="False" Width ="90%"  runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:05%;">
                                <asp:Label ID="lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                            </td>
                            <td align="left" colspan="3" style="width:20%;">
                                <asp:TextBox ID="Txt_Proveedor" runat="server" Enabled="False" Width="97%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>  
                    </table>
                    </div> 
                </td>
            </tr>
             <tr>
                <td>
                   
                        <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">
                            <tr>
                                <td >
                                 <%--<asp:Panel ID="Panel1" runat="server" Width="870px" ScrollBars="Auto">--%>
                                  <asp:GridView ID="Grid_Registro_Datos" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        ForeColor="#333333" GridLines="Both"  style="white-space:normal;" 
                                        PageSize="5">
                                        <RowStyle CssClass="GridItem" Font-Size="Smaller" />
                                        <Columns>
                                            <asp:BoundField DataField="NO_INVENTARIO" HeaderText="Inv." Visible="false">
                                                <ItemStyle Font-Size="X-Small" Width="30px" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="PRODUCTO" 
                                                HeaderText="Producto" >
                                                <ItemStyle  Font-Size="X-Small" VerticalAlign="Top" Width="20%"/>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                                <ItemStyle Font-Size="X-Small" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            
                                            <asp:TemplateField HeaderText="Datos Generales" >
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_Grid_Marca" runat="server" Text="Marca"></asp:Label>
                                                    <asp:DropDownList ID="Cmb_Marca" Width="75%" runat="server" Font-Size="X-Small" OnTextChanged="Cmb_Marca_TextChanged" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    
                                                    <br/>
                                                    <asp:Label ID="Label1" runat="server" Text="* Color"></asp:Label>
                                                    <asp:DropDownList ID="Cmb_Color" Width="75%" runat="server" Font-Size="X-Small">
                                                    </asp:DropDownList> 
                                                    
                                                    <br/>
                                                    <asp:Label ID="Label2" runat="server" Text="* Material"></asp:Label>
                                                    <asp:DropDownList ID="Cmb_Material" runat="server" Width="75%" Font-Size="X-Small">
                                                    </asp:DropDownList>                                                    
                                                                
                                                    <br />
                                                    <asp:Label ID="Label3" runat="server" Text="Modelo"></asp:Label>
                                                    <asp:TextBox ID="Txt_Modelo" runat="server" Width="75%" MaxLength="140" Font-Size="X-Small"></asp:TextBox>                                                    
                                                    <cc1:FilteredTextBoxExtender ID="Txt_Modelo_FilteredTextBoxExtender" 
                                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        TargetControlID="Txt_Modelo" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
                                                    </cc1:FilteredTextBoxExtender>
                                                                 
                                                    <br />
                                                    <asp:Label ID="Label4" runat="server" Text="* No. Serie"></asp:Label>
                                                    <asp:TextBox ID="Txt_No_Serie" runat="server" Width="75%" MaxLength="49" Font-Size="X-Small"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Txt_No_Serie_FilteredTextBoxExtender" 
                                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        TargetControlID="Txt_No_Serie" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
                                                    </cc1:FilteredTextBoxExtender>
                                                                 
                                                    <br />
                                                    <asp:Label ID="Label5" runat="server" Text="Garantia"></asp:Label>
                                                    <asp:TextBox ID="Txt_Garantia" runat="server" Width="75%" MaxLength="249" Font-Size="X-Small"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Txt_Garantia_FilteredTextBoxExtender" 
                                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        TargetControlID="Txt_Garantia" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
                                                    </cc1:FilteredTextBoxExtender>                                                                                                                                                                         
                                                    
                                                    <br />
                                                    <asp:Label ID="Label6" runat="server" Text="Observaciones"></asp:Label>
                                                    <asp:TextBox ID="Txt_Observaciones_Producto" runat="server" MaxLength="248" Width="65%" Wrap="False" Font-Size="X-Small"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                    TargetControlID="Txt_Observaciones_Producto" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
                                                    </cc1:FilteredTextBoxExtender>                                                    
                                                </ItemTemplate>                                                
                                                <ItemStyle Font-Size="X-Small" Width="38%" HorizontalAlign="Right" VerticalAlign="Top"/>
                                            </asp:TemplateField>
                                      
                                            <asp:TemplateField HeaderText="" Visible="false"> 
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px"/>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px" /> 
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="" Visible="false"> 
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="" Visible="false"> 
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px" />
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="TIPO" HeaderText="Tipo" Visible="False" >
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto ID" Visible="False" >
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            
                                            <asp:TemplateField HeaderText="Observaciones" Visible="false"> 
                                                <ItemTemplate>

                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" Width="1px" />
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="False" >
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView> 
                               <%--</asp:Panel>--%>

                                </td>
                            </tr>
                        </table>
                </td>
              </tr>
        </table> <%-- Fin Tabla General--%>
     </div> <%--Fin Div General--%>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

