<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Psp_Asignar_Tope_Presupuestal.aspx.cs" Inherits="paginas_presupuestos_Frm_Ope_Psp_Asignar_Tope_Presupuestal" Title="Asignar límite presupuestal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                 <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                               Asignación de Límite Presupuestal y Capítulos 
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
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" ToolTip="Nuevo" onclick="Btn_Nuevo_Click"  />   
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" 
                                    onclick="Btn_Modificar_Click"  />                                                                     
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" onclick="Btn_Salir_Click"  />
                            </td>
                            <td colspan="2">
                                <div id="Div_Busqueda" runat="server" style="width:98%;">
                                    <asp:Label ID="Lbl_Leyenda_Busqueda" runat="server" Text="Busqueda" Style="font-weight:bolder; color:White;"></asp:Label>
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" MaxLength="4"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar"  
                                        AlternateText="Consultar" onclick="Btn_Buscar_Click" />
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<Año>" TargetControlID="Txt_Busqueda" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>    
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" FilterType="Numbers" >
                                    </cc1:FilteredTextBoxExtender>  
                                 </div>  
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                  <div id="Div_Listado_Parametros" runat="server" style="width:100%;height:auto; max-height:500px; overflow:auto; vertical-align:top;">
                    <table style="width: 100%;">
                         <tr>
                            <tr>
                                <td><asp:HiddenField ID="Hdf_Parametro_ID" runat="server" /></td>
                            </tr>
                            <td>
                                <asp:GridView ID="Grid_Listado_Parametros" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False"  Width="96%" GridLines= "None" 
                                    onselectedindexchanged="Grid_Listado_Parametros_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="PARAMETRO_ID" HeaderText="PARAMETRO_ID" SortExpression="PARAMETRO_ID" >
                                            <ItemStyle HorizontalAlign ="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO_PRESUPUESTAR" HeaderText="Año" SortExpression="ANIO_PRESUPUESTAR" >
                                            <ItemStyle HorizontalAlign ="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_APERTURA" HeaderText="Fecha de Apertura" SortExpression="FECHA_APERTURA" DataFormatString="{0:dd/MMM/yyyy}" >
                                            <ItemStyle HorizontalAlign ="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_CIERRE" HeaderText="Fecha de Cierre" SortExpression="FECHA_CIERRE" DataFormatString="{0:dd/MMM/yyyy}" >
                                            <ItemStyle HorizontalAlign ="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                            <ItemStyle HorizontalAlign ="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FTE_FINANCIAMIENTO_ID">
                                            <ItemStyle HorizontalAlign ="Center"/>
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </td>
                         </tr>
                    </table>
                  </div>
                <%--Div listado de requisiciones--%>
                  <div id="Div_Listado_Requisiciones" runat="server" style="width:100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:20%; text-align:left;">
                                Año Presupuestal
                            </td>
                            <td  style="width:25%; text-align:left;">
                                <asp:TextBox ID="Txt_Anio_Presupuestal" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width:20%; text-align:left;">
                                &nbsp;&nbsp;Fecha límite para captura
                            </td>
                            <td style="width:25%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Limite" runat="server" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left">
                                <asp:Label ID="Lbl_Fuente_Financiamiento" runat="server" Text="Fuente Financiamiento"></asp:Label>
                            </td>
                            <td style=" text-align:left;" colspan="3">
                               <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Width="100%" Enabled ="false">
                                    <asp:ListItem Value="NADA">&lt;--SELECCIONE--&gt;</asp:ListItem>
                                </asp:DropDownList> 
                            </td>                                                    
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left;">
                                * Unidad Responsable
                            </td>
                            <td colspan="3" style=" text-align:left;">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Programas_SelectedIndexChanged"></asp:DropDownList>
                            </td>                                                    
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left">
                                <asp:Label ID="Lbl_Programa" runat="server" Text="* Programa" ></asp:Label>
                            </td>
                            <td style=" text-align:left;" colspan="3">
                               <asp:DropDownList ID="Cmb_Programa" runat="server" Width="100%" >
                                    <asp:ListItem Value="NADA">&lt;--SELECCIONE--&gt;</asp:ListItem>
                                </asp:DropDownList> 
                            </td>                                                    
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left;">
                                * Límite Presupuestal
                            </td>
                            <td style="width:25%; text-align:left;">
                                <asp:TextBox ID="Txt_Tope_Presupuestal" runat="server" Width="100%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTxb_PU" runat="server" 
                                    FilterType="Custom,Numbers"
                                    TargetControlID="Txt_Tope_Presupuestal" ValidChars=".,">
                                </cc1:FilteredTextBoxExtender>                                
                            </td>
                            <td colspan="2">&nbsp;</td>                            
                        </tr>
                    </table>
                    <asp:Panel ID="Pnl_Capitulos" runat="server"  GroupingText="* Capítulos" Width="99%" >
                      <div style="height:auto; max-height:300px; overflow:auto; width:100%; vertical-align:top;">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:100%;">
                                    <asp:GridView ID="Grid_Capitulos" runat="server" style="white-space:normal"
                                        CssClass="GridView_1"
                                        AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        EmptyDataText="No se encontraron capítulos"
                                        DataKeyNames="CAPITULO_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate >
                                                    <asp:CheckBox ID="Chk_Capitulo" runat="server" />                                                
                                                </ItemTemplate >
                                                <ControlStyle Width="12px"/>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>    
                                            <asp:BoundField DataField="CAPITULO_ID" HeaderText="Clave" >
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>                                                                          
                                            <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True" SortExpression="Clave">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion" HeaderText="Capítulo" Visible="True" SortExpression="Descripcion">
                                                <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                                <ItemStyle HorizontalAlign="Left" Width="80%" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>                                
                                </td>
                            </tr>
                        </table>
                       </div>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Unidades_Responsables" runat="server"  GroupingText="Unidades Responsables Asignadas" Width="99%" >
                        <div style="height:auto; max-height:300px; overflow:auto; width:100%; vertical-align:top;">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:100%;">
                                        <asp:GridView ID="Grid_Unidades_Responsables" runat="server" style="white-space:normal"
                                            CssClass="GridView_1"
                                            AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            onselectedindexchanged="Grid_Unidades_Responsables_SelectedIndexChanged"
                                            EmptyDataText="No se han asignado Unidades Responsables">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                                    <ItemStyle Width="3%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Clave" HeaderText="Clave"  SortExpression="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_NOMBRE" HeaderText="Descripción" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="61%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="61%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="Descripción" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="LIMITE_PRESUPUESTAL" HeaderText="Limite Presupuestal" 
                                                 SortExpression="LIMITE_PRESUPUESTAL" DataFormatString="{0:c}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="16%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="16%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Unidad" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                            OnClick="Btn_Eliminar_Unidad_Click" CommandArgument='<%# Eval("DEPENDENCIA_ID") %>' ToolTip="Eliminar"
                                                            OnClientClick="return confirm('¿Esta seguro que desea elimina el registro?');"/>
                                                    </ItemTemplate>                                            
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FTE_FINANCIAMIENTO_ID" /> 
                                                <asp:BoundField DataField="PROYECTO_PROGRAMA_ID" />                                         
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>                                  
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>                    
                  </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

