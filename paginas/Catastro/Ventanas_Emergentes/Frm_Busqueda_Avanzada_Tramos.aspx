<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Tramos.aspx.cs" Inherits="paginas_Catastro_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Tramos" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Búsqueda Avanzada de Tramos" Culture="es-MX"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 838px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type='text/javascript'>
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
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"  EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos" DisplayAfter="0">
            <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calles" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Búsqueda avanzada de Tramos de Calle</td>
                    </tr>
                    <tr>
                        <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <%--<asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />--%>
<%--                            <asp:ImageButton ID="Btn_Eliminar" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick = "Btn_Eliminar_Click" />--%>
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                        <td align="right" class="style1">Búsqueda:
                            <%--<asp:DropDownList ID="Cmb_Filtro_Busqueda" runat="server" Width="25%" TabIndex="5">
                                <asp:ListItem Text="CALLE" Value="CALLE"></asp:ListItem>
                                <asp:ListItem Text="COLONIA" Value="COLONIA"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="Txt_Busqueda_Calles" runat="server"
                                Width="130px" TabIndex="6" style="text-transform:uppercase"></asp:TextBox>
                                --%>
                            <asp:ImageButton ID="Btn_Buscar_Calles" runat="server" 
                                CausesValidation="false" TabIndex="7"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick = "Btn_Buscar_Click" /> 
                        </td>                        
                    </tr>
                </table>   
                <br />
                        <center>
                        <table width="98%" class="estilo_fuente">
                        <tr>
                        <td style="text-align:left;width:20%;">
                            Calle
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="txt_Calle" runat="server" Width="98%" 
                                TabIndex="9" Enabled="true"></asp:TextBox>
                        </td>
                        <td style="text-align:Right;width:20%;">
                            Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="txt_Colonia" runat="server" Width="98%" 
                                TabIndex="9" Enabled="true"></asp:TextBox>
                        </td>
                        </tr>
                    <div id="Div_Grid_Calles" runat="server">
                    <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Calles
                                </td>
                            </tr>
                            <tr>
                                <td align="center" >
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Calles" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Calles_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Calles_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CALLE_ID" HeaderStyle-Width="15%" HeaderText="Calle ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="CLAVE" HeaderStyle-Width="15%" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre Calle">
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="COLONIA" HeaderText="Colonia">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="15%" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                            </div>
                            <div id="Div_Grid_Tramos" runat="server" visible="false">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos de la calle
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;
                            </td>
                            </tr>
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_Calle_Id" runat="server" />
                        </td>
                        </tr>
                    <tr>
                    <td>
                    <br />
                    <br />
                    </td>
                    </tr>
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Tramos
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:HiddenField ID="Hdf_Tramo_Id" runat="server" />
                    <br />
                    </td>
                    </tr>
                            <%--<tr>
                            <td>
                            Tramo
                            </td>
                            <td colspan="3" style="text-align: left;">
                            <asp:TextBox ID="Txt_Tramo" TextMode="MultiLine" Rows="3" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" Enabled="false"/>
                            <cc1:TextBoxWatermarkExtender ID="TBWME_Txt_Tramo" runat="server" TargetControlID="Txt_Tramo" WatermarkText="LÍMITE DE CARACTERES 250" WatermarkCssClass="watermarked" Enabled="true"/>
                            </td>
                            </tr>--%>
                            <%--<tr>
                            <td>
                            <br />
                            </td>
                            </tr>--%>
                            <tr>
                            <td colspan="4">
                            <asp:GridView ID="Grid_Tramos" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="10" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Tramo_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Tramo_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>

                                                    <asp:BoundField DataField="TRAMO_ID" HeaderStyle-Width="15%" HeaderText="Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="TRAMO_DESCRIPCION" HeaderStyle-Width="15%" HeaderText="Tramo de calle">
                                                    <HeaderStyle HorizontalAlign="Center" Width="90%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="90%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ACCION" HeaderStyle-Width="15%" HeaderText="Accion">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            <tr>
                                            <td>
                                            <br />
                                            </td>
                                            </tr>
                                            </div>
                                            <div id="Div_Grid_Tabla_Valores" runat="server" visible="false">
                                            <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Tabla de Valores
                        </td>
                    </tr>
                    <%--<tr>
                    <td>
                    <asp:HiddenField ID="Hdf_Anio" runat="server" />
                    <asp:HiddenField ID="Hdf_Valor_M2" runat="server"/>
                    <br />
                    </td>
                    </tr>--%>
                    <%--<tr>
                            <td style="text-align:left;width:20%;text-align:left;">
                            *Año
                            </td>
                            <td style="text-align:left;width:20%;text-align:left;">
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" MaxLength="4"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio" TargetControlID="Txt_Anio" />
                            </td>
                            <td style="text-align:right;width:20%;text-align:right;">
                            *Valor M2
                            </td>
                            <td style="text-align:right;width:20%;text-align:right;">
                            <asp:TextBox ID="Txt_Valor_M2" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" AutoPostBack="true" OnTextChanged="Txt_Valor_M2_TextChanged" MaxLength="12"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FTB_Txt_Valor_M2" TargetControlID="Txt_Valor_M2" ValidChars="1234567890.,"/>
                            </td>
                            </tr>
                    
                                            <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            <asp:ImageButton ID="Btn_Agregar" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                OnClick="Btn_Agregar_Click"
                                TabIndex="10" ToolTip="Agregar" Width="24px" />
                                &nbsp; &nbsp; 
                            <asp:ImageButton ID="Btn_Actualizar" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" 
                                OnClick="Btn_Actualizar_Click"
                                TabIndex="10" ToolTip="Actualizar" Width="24px" />
                                &nbsp; &nbsp;
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                OnClick="Btn_Eliminar_Click"
                                TabIndex="10" ToolTip="Eliminar" Width="24px" />
                            </td>
                            </tr>--%>
                            <tr>
                            <td colspan="4">
                            <asp:GridView ID="Grid_Valores_Tramos" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="10" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Valores_Tramos_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Valores_Tramos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>

                                                    <asp:BoundField DataField="VALOR_TRAMO_ID" HeaderStyle-Width="15%" HeaderText="Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="ANIO" HeaderStyle-Width="15%" HeaderText="Año">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="VALOR_TRAMO" HeaderStyle-Width="15%" HeaderText="Valor de M2" DataFormatString="{0:c2}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ACCION" HeaderStyle-Width="15%" HeaderText="Accion" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                            </div>
                            </table>
                </table>

                        </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>