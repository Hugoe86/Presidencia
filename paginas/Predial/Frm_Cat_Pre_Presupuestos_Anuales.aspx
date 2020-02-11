<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Cat_Pre_Presupuestos_Anuales.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Presupuestos_Anuales" title="Presupuesto Anual"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
    </Script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"  EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%>
            </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            <tr>
                                <td class="label_titulo" colspan="2">
                                    Catálogo de Presupuestos Anuales</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div ID="Div_Contenedor_Error" runat="server">
                                        <tr>
                                            <asp:Image ID="Img_Error" runat="server" 
                                                ImageUrl="../imagenes/paginas/sias_warning.png" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" 
                                                CssClass="estilo_fuente_mensaje_error" Text="" />
                                            <caption>
                                                <br />
                                                <asp:Label ID="Lbl_Error" runat="server" CssClass="estilo_fuente_mensaje_error" 
                                                    TabIndex="0" Text=""></asp:Label>
                                            </caption>
                                        </tr>
                                    </div>
                                </td>
                            </tr>
                                </div>
                            </tr>
                            <tr class="barra_busqueda">
                                <td style="width:50%">
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                                </td>
                                <td align="right" style="width:50%">
                                    Búsqueda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                    ToolTip="Buscar" TabIndex="1"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        TabIndex="2" OnClick="Btn_Buscar_Click"/>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Clave de ingreso>" TargetControlID="Txt_Busqueda"/>
                                </td>                        
                            </tr>
                        </table>
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:32%">&nbsp;</td>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:32%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            Clave de Ingreso</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Clave_Ingreso" runat="server" Width="92%" ReadOnly="true" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width:18%">
                        <asp:HiddenField ID="Hdf_Clave_Ingreso_Id" runat="server" />
                        <asp:HiddenField ID="Hdf_Presupuesto_Id" runat="server" />
                        <asp:HiddenField ID="Hdf_Anio" runat="server" />
                        <asp:HiddenField ID="Hdf_Importe" runat="server" />
                        </td>
                        <td style="width:32%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">Año</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="92%" MaxLength="4"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Anio" 
                                runat="server" FilterType="Numbers" 
                                TargetControlID="Txt_Anio" ValidChars="0123456789"/>
                        </td>
                        <td style="width:18%" align="right">Presupuesto Anual</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Presupuesto" runat="server" Width="92%" OnTextChanged="Txt_Monto_Presupuesto_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNumbers" 
                                runat="server" FilterType="Numbers, Custom" 
                                TargetControlID="Txt_Monto_Presupuesto" ValidChars="0123456789.,">
                            </cc1:FilteredTextBoxExtender>
                         </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            <asp:ImageButton ID="Btn_Agregar_Presupuesto" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/sias_add.png" OnClick="Btn_Agregar_Presupuesto_Click"/> &nbsp; &nbsp; 
                                    <asp:ImageButton ID="Btn_Actualizar_Presupuesto" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_Actualizar.png" 
                                onclick="Btn_Actualizar_Presupuesto_Click" /></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            
                            <asp:GridView ID="Grid_Presupuestos_Anuales" runat="server" AllowPaging="true" 
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" PageSize="5"                                 
                                Style="white-space:normal" Width="96%" 
                                onselectedindexchanged="Grid_Presupuestos_Anuales_SelectedIndex_Changed" 
                                onpageindexchanging="Grid_Presupuestos_Anuales_PageIndex_Changing" 
                                onrowcommand="Grid_Presupuestos_Anuales_RowCommand" >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>                                    
                                    <asp:BoundField DataField="PRESUPUESTO_ID" HeaderText="Id" SortExpression="CAJA_ID" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IMPORTE" HeaderText="Importe" DataFormatString="{0:c2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                        <ItemStyle HorizontalAlign="Center" Width="30%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Presupuesto" runat="server" Height="20px" 
                                                            ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  TabIndex="10" 
                                                            OnClientClick = "return confirm('Se eliminará de forma permanente, ¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Presupuesto Anual" Width="20px"  CommandName="Borrar_Presupuesto" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                    <td colspan="4">
                                        <asp:GridView ID="Grid_Claves_Ingreso" runat="server" AllowPaging="True" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" 
                                            EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="None" 
                                            PageSize="5" Style="white-space:normal" Width="96%"
                                            OnSelectedIndexChanged = "Grid_Claves_Ingreso_SelectedIndexChanged"
                                            OnPageIndexChanging = "Grid_Claves_Ingreso_PageIndexChanging">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RAMA" HeaderText="Rama">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GRUPO" HeaderText="Grupo">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>     
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>                     
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                    </tr>
                
                
                </table> 
            </div>
                        
            </ContentTemplate>                        
    </asp:UpdatePanel>
    


</asp:Content>