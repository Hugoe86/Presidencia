<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Factores_Cobro_Memorias_Descriptivas.aspx.cs" Inherits="paginas_Catastro_Frm_Cat_Cat_Factores_Cobro_Memorias_Descriptivas" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Catálogo de Factores de Cobro para memorias Descriptivas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type='text/javascript' >
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

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
                        <td colspan="2" class="label_titulo">Catálogo de Cobro para Memorias Descriptivas</td>
                    </tr>
                    <tr>
                        <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
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
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />

                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                                              
                    </tr>
                </table>   
                <br />
                        <center>
                        <table width="98%" class="estilo_fuente">
                    <div id="Div_Grid_Valores_Cobro_Memorias_Descriptivas" runat="server" visible="true">
                   
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Tabla de Valores
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
                    </td>
                    </tr>
                            <tr>
                            <td style="text-align:left;width:20%;text-align:left;">
                            *Año
                            </td>
                            <td style="text-align:left;width:20%;text-align:left;">
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" MaxLength="4"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio" TargetControlID="Txt_Anio" />
                            </td>
                        
                            </tr>
                            <tr>
                            <td style="text-align:right;width:20%;text-align:left;">
                            *Cantidad Cobro 1
                            </td>
                            <td style="text-align:right;width:20%;text-align:right;">
                            <asp:TextBox ID="Txt_Cantidad_Cobro1" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" AutoPostBack="true" OnTextChanged="Txt_Cantidad_Cobro1_TextChanged" MaxLength="24"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FTB_Txt_Cantidad_Cobro1" TargetControlID="Txt_Cantidad_Cobro1" ValidChars="1234567890.,"/>
                            </td>
                            <td style="text-align:right;width:20%;text-align:left;">
                            *Cantidad Cobro 2
                            </td>
                            <td style="text-align:right;width:20%;text-align:right;">
                            <asp:TextBox ID="Txt_Cantidad_Cobro2" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" AutoPostBack="true" OnTextChanged="Txt_Cantidad_Cobro2_TextChanged" MaxLength="24"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FilteredTextBoxExtender5" TargetControlID="Txt_Cantidad_Cobro2" ValidChars="1234567890.,"/>
                            </td>
                            <%--<td style="text-align:right;width:20%;text-align:left;">
                            *Base Cobro
                            </td>
                            <td style="text-align:right;width:20%;text-align:right;">
                            <asp:TextBox ID="Txt_Base_Cobro" runat="server" Width="94.4%" Style="float: left; text-transform:uppercase" AutoPostBack="true" OnTextChanged="Txt_Base_Cobro_TextChanged" MaxLength="8"/>
                            <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FilteredTextBoxExtender6" TargetControlID="Txt_Base_Cobro" ValidChars="1234567890.,"/>
                            </td>--%>
                            </tr>
                            <tr>
                            <td>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                    FilterType="Numbers,Custom" TargetControlID="Txt_Anio" 
                                    ValidChars="1234567890" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                    FilterType="Numbers,Custom" TargetControlID="Txt_Cantidad_Cobro1" 
                                    ValidChars="1234567890.," />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                    FilterType="Numbers,Custom" TargetControlID="Txt_Cantidad_Cobro2" 
                                    ValidChars="1234567890.," />
                                
                            
                            <asp:HiddenField ID="Hdf_Anio" runat="server" />
                            <asp:HiddenField ID="Hdf_Cantidad_Cobro1" runat="server" />
                            <asp:HiddenField ID="Hdf_Cantidad_Cobro2" runat="server" />
                            <%--<asp:HiddenField ID="Hdf_Base_Cobro" runat="server" />--%>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            <asp:ImageButton ID="Btn_Agregar_Valor" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                OnClick="Btn_Agregar_Valor_Click"
                                TabIndex="10" ToolTip="Agregar Valor" Width="24px" />
                                &nbsp; &nbsp; 
                            <asp:ImageButton ID="Btn_Actualizar_Valor" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" 
                                OnClick="Btn_Actualizar_Valor_Click"
                                TabIndex="10" ToolTip="Actualizar Valor" Width="24px" />
                                &nbsp; &nbsp;
                            <asp:ImageButton ID="Btn_Eliminar_Valor" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                OnClick="Btn_Eliminar_Valor_Click"
                                TabIndex="10" ToolTip="Eliminar Valor" Width="24px" />
                            </td>
                            </tr>
                            <tr>
                            <td colspan="4">
                            <asp:GridView ID="Grid_Valores" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="10" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Valores_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Valores_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="ANIO" HeaderStyle-Width="20%" HeaderText="Año">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="FACTOR_COBRO_MEM_ID" HeaderStyle-Width="15%" HeaderText="Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="CANTIDAD_COBRO_1" HeaderStyle-Width="30%" HeaderText="Cantidad de Cobro 1" DataFormatString="{0:N10}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="CANTIDAD_COBRO_2" HeaderStyle-Width="30%" HeaderText="Cantidad de Cobro 2" DataFormatString="{0:N10}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                    </asp:BoundField>
                                                    
                                                    <%--<asp:BoundField DataField="BASE_COBRO" HeaderStyle-Width="30%" HeaderText="Base Cobro" DataFormatString="{0:N10}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="ACCION" HeaderStyle-Width="20%" HeaderText="Accion" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                            </table>
                            </div>
                </table>

                        </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
