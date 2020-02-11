<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Ajuste_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Ajuste_Stock" Title="Ajuste de stock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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


    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>
                <%--Div Encabezado--%>
                <div id="Div_Listado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td>
                                <asp:HiddenField ID="Hdn_Comprometido" runat="server" />
                            </td>
                        </tr>                    
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                               Ajustar Stock
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Guardar" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_actualizar.png"
                                    ToolTip="Guardar" onclick="Btn_Guardar_Click"  />                                  
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" onclick="Btn_Salir_Click"  />
                                 
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Ajustes" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Fecha
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                            </td>
                            <td align="right" visible="false">
                                Folio
                            </td>
                            <td visible="false">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="85%" MaxLength="13"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" ToolTip="Consultar"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;AI-000000&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Busqueda" runat="server" FilterType="Custom" 
                                    TargetControlID="Txt_Busqueda" ValidChars="aAiI-0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                              <div style="overflow:auto;height:320px;width:99%;vertical-align:top;
                                   border-style:outset;border-color: Silver;" > 
                                <asp:GridView ID="Grid_Ajustes_Inventario" runat="server" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="100%"
                                    AllowSorting="false" HeaderStyle-CssClass="tblHead"                                     
                                    EmptyDataText="Ajustes de inventario">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Inventario" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Inventario_Click"                                                    
                                                    CommandArgument='<%# Eval("NO_AJUSTE") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="25px" />
                                            <ItemStyle HorizontalAlign="Center" Width="25px" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="" Visible="true">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Imprimir_Ajuste_Inventario" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                                    OnClick="Btn_Imprimir_Ajuste_Inventario_Click"                                                    
                                                    CommandArgument='<%# Eval("NO_AJUSTE") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="35px" />
                                            <ItemStyle HorizontalAlign="Center" Width="35px" />
                                        </asp:TemplateField>                                                                                                                     
                                        <asp:BoundField DataField="NO_AJUSTE" HeaderText="No. Ajuste" Visible="True" SortExpression="NO_AJUSTE">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" 
                                            DataFormatString="{0:dd/MMM/yyyy}" Visible="True" SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Solicitó" Visible="True" SortExpression="USUARIO_CREO">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                        </asp:BoundField> 
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>                                                                               
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
                </div>
                <div id="Div_Contenido" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 18%">
                                Clave
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Clave" runat="server" Width="" MaxLength="10" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Clave" runat="server"  TargetControlID="Txt_Clave"
                                FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>                                
                                <asp:ImageButton ID="Btn_Buscar_Producto" runat="server" CssClass="Img_Button" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Producto_Click" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width: 18%">
                                Producto
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Producto" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%">
                                Descripción
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="98%" Enabled="false"
                                TextMode="MultiLine" Height="50px"></asp:TextBox>
                            </td>                                                        
                        </tr>
                        <tr>
                            <td style="width: 18%">
                                Existencia
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Existencia" runat="server" Enabled="false"></asp:TextBox>
                            </td>  
                            <td style="width: 18%">
                                $ Promedio
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Costo" runat="server" Enabled="false"></asp:TextBox>
                            </td>  
                            <td>
                            </td>                                                                                                                                                                                             
                        </tr>
                        <tr>
                            <td style="width: 18%">
                                <asp:Label ID="Lbl_Conteo_Fisico" runat="server" Text="Conteo Físico" Font-Bold="true" Font-Underline="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Conteo_Fisico" runat="server" Enabled="true" ontextchanged="Txt_Conteo_Fisico_TextChanged" 
                                 AutoPostBack="true"   MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTB_Conteo_Fisico" runat="server"  TargetControlID="Txt_Conteo_Fisico"
                                FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>                                  
                            </td>
                            <td style="width: 18%">
                                Movimiento
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Movimiento" runat="server" Width="153px" Enabled="false">
                                </asp:DropDownList>
                            </td>  
                            <td>
                            </td>                                                                                                              
                        </tr>

                        <tr>
                            <td style="width: 18%">
                                Diferencia
                            </td>
                            <td>
                                <asp:Label ID="Txt_Diferencia" runat="server" Text="" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="153px"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                $ Importe
                            </td>
                            <td>
                                <asp:Label ID="Txt_Importe" runat="server" Text="" BorderColor="GrayText"                             
                                BorderWidth="1px" Height="18px" Width="153px"></asp:Label>
                            </td>                              
                            <td>
                                <asp:Button ID="Btn_Agregar" runat="server" Text="Agregar producto" 
                                    CssClass="button" Width="145px" onclick="Btn_Agregar_Click"/>
                            </td>                              
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div style="overflow: auto; height: 220px; width: 99%; vertical-align: top; border-style: outset;
                                    border-color: Silver;">
                                    <asp:GridView ID="Grid_Productos_Ajustados" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                        GridLines="None" Width="100%" AllowSorting="false" HeaderStyle-CssClass="tblHead"
                                        EmptyDataText="No se encontraron productos">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                        OnClick="Btn_Seleccionar_Producto_Click" CommandArgument='<%# Eval("PRODUCTO_ID") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True" SortExpression="CLAVE" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE_DESCRIPCION" HeaderText="Descripción" Visible="True" SortExpression="DESCRIPCION">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DIFERENCIA" HeaderText="Ctd." Visible="True" SortExpression="DIFERENCIA">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IMPORTE_DIFERENCIA" HeaderText="Importe" Visible="True" SortExpression="DIFERENCIA">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TIPO_MOVIMIENTO" HeaderText="Mvto." Visible="True" SortExpression="DIFERENCIA">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            
                            </td>
                            <td>
                            
                            </td>
                            <td>
                                Entradas
                            </td>
                            <td>
                                Salidas
                            </td> 
                            <td>
                                Total
                            </td>                                                                                                               
                        </tr>
                        <tr>
                            <td style="width: 18%" valign="top">
                                
                            </td>
                            <td>
                                Por Producto
                            </td>    
                            <td style="width: 18%" valign="top">
                                <asp:Label ID="Lbl_Total_Entradas" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                   +
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Total_Salidas" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label> 
                                   =                                                             
                            </td>   
                            <td>
                                <asp:Label ID="Lbl_Total_Ajustes" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>                                                                
                           </td>                                                                                                                                       
                        </tr> 
                        <tr>
                            <td style="width: 18%" valign="top">
                                
                            </td>
                            <td>
                                Por Unidades
                            </td>                         
                            <td style="width: 18%" valign="top">
                                <asp:Label ID="Lbl_Entradas_Unidad" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                   +
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Salidas_Unidad" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                   =
                            </td>                                                                                                            
                            <td>
                                <asp:Label ID="Lbl_Unidades_Ajustadas" runat="server" Text="0" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>                                    
                            

                            </td>  
                        </tr>                    
                        <tr>
                            <td style="width: 18%" valign="top">
                           
                            </td>
                            <td>
                                Importe
                            </td>    
                            <td style="width: 18%" valign="top">
                                <asp:Label ID="Lbl_Importe_Entradas" runat="server" Text="0.00" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                   -
                            </td>
                            <td>
                                <asp:Label ID="Lbl_Importe_Salidas" runat="server" Text="0.00" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label> 
                                   =                             
                            </td>   
                            <td>
                                <asp:Label ID="Lbl_Importe_Saldo" runat="server" Text="0.00" BorderColor="GrayText" 
                                   BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                            </td>                                                                                                          
                        </tr>   
                        <tr>
                         
                            <td style="width: 18%" valign="top">
                       
                            </td>
                            <td>
  
                            </td>    
   
                                                                                                         
                        </tr>                                               
                        <tr>
                            <td valign="top" colspan="4">
                                Indique la Justificación para el ajuste
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="Txt_Justificacion" runat="server" Height="150px" TextMode="MultiLine" Width="98%"
                                MaxLength="3999"></asp:TextBox>
                            </td>
                            <cc1:FilteredTextBoxExtender ID="FTE_Justificacion" runat="server" TargetControlID="Txt_Justificacion" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                            </cc1:FilteredTextBoxExtender>                            
                        </tr>
                    </table>
                                  
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>






</asp:Content>

