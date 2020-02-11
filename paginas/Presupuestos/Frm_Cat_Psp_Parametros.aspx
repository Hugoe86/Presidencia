<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Psp_Parametros.aspx.cs" Inherits="paginas_presupuestos_Frm_Cat_Psp_Parametros" Title="Parametros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Parametros" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Parametros</td>
                    </tr>
                    <tr>
                        <td colspan ="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px" Enabled="false"/>
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
                    <tr>
                        <td colspan ="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Nuevo" ToolTip="Nuevo" 
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" 
                                onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Salir" ToolTip="Salir" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">
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
                <br />
                <center>
                <table width="98%">
                    <tr>
                        <td style="width:100%;">
                            <div id="Div_Listado_Parametros" runat="server" style="width:100%; height:auto; max-height:500px; overflow:auto;vertical-align:top;">
                                <asp:GridView ID="Grid_Listado_Parametros" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" Width="96%"
                                    GridLines= "None" 
                                    onselectedindexchanged="Grid_Listado_Parametros_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="PARAMETRO_ID" HeaderText="PARAMETRO_ID" SortExpression="PARAMETRO_ID" >
                                            <ItemStyle HorizontalAlign ="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO_PRESUPUESTAR" HeaderText="Año" SortExpression="ANIO_PRESUPUESTAR" >
                                            <ItemStyle HorizontalAlign ="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_APERTURA" HeaderText="Fecha de Apertura" SortExpression="FECHA_APERTURA" DataFormatString="{0:dd/MMM/yyyy}" >
                                            <ItemStyle HorizontalAlign ="Center"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_CIERRE" HeaderText="Fecha de Cierre" SortExpression="FECHA_CIERRE" DataFormatString="{0:dd/MMM/yyyy}" >
                                            <ItemStyle HorizontalAlign ="Center"/>
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
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;">
                            <div id="Div_Campos" runat="server" style="width:98%;">
                                <table width="100%">
                                    <tr>
                                        <td colspan="4" style="text-align:center;">
                                            <asp:HiddenField ID="Hdf_Parametro_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:25%; text-align:left">
                                            <asp:Label ID="Lbl_Anio" runat="server" Text="Año a Presupuestar" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:25%; text-align:left;">
                                            <asp:TextBox ID="Txt_Anio" runat="server" Width="97%" MaxLength="4" AutoPostBack="true"  ontextchanged="Txt_Anio_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" TargetControlID="Txt_Anio" FilterType="Numbers" >
                                            </cc1:FilteredTextBoxExtender>   
                                        </td>
                                        <td style="width:25%; text-align:left">&nbsp;
                                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:25%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Value="NADA">&lt;--SELECCIONE--&gt;</asp:ListItem>
                                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                              
                                        </td>         
                                    </tr>
                                    <tr>
                                        <td style="width:25%; text-align:left">
                                            <asp:Label ID="Lbl_Fuente_Financiamiento" runat="server" Text="Fuente Financiamiento" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style=" text-align:left;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Width="100%">
                                                <asp:ListItem Value="NADA">&lt;--SELECCIONE--&gt;</asp:ListItem>
                                            </asp:DropDownList> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:25%; text-align:left;">
                                            <asp:Label ID="Lbl_Fecha_Apertura" runat="server" Text="Fecha Apertura para Captura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:25%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Apertura" runat="server" Width="87%" Enabled="false" ></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Txt_Fecha_Apertura" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Apertura" runat="server" TargetControlID="Txt_Fecha_Apertura" PopupButtonID="Btn_Txt_Fecha_Apertura" Format="dd/MMM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Apertura" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Apertura" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Apertura" runat="server" ControlToValidate="Txt_Fecha_Apertura" ControlExtender="MEE_Txt_Fecha_Apertura" 
                                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha de Apertura no valida" IsValidEmpty="false" 
                                                                    TooltipMessage="Ingresar Fecha de Apertura" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>        
                                        </td>
                                        <td style="width:25%; text-align:left;">&nbsp;
                                            <asp:Label ID="Lbl_Fecha_Cierre" runat="server" Text="Fecha Cierre Captura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:25%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Cierre" runat="server" Width="87%" Enabled="false" ></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Txt_Fecha_Cierre" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Cierre" runat="server" TargetControlID="Txt_Fecha_Cierre" PopupButtonID="Btn_Txt_Fecha_Cierre" Format="dd/MMM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Cierre" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Cierre" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Cierre" runat="server" ControlToValidate="Txt_Fecha_Cierre" ControlExtender="MEE_Txt_Fecha_Cierre" 
                                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha de Apertura no valida" IsValidEmpty="false" 
                                                                    TooltipMessage="Ingresar Fecha de Cierre" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align:center;">
                                            <asp:Panel ID="Pnl_Grid_Partidas_Stock" runat="server"  Width="99%" GroupingText="Partidas para Stock" BorderColor="BlueViolet">
                                                <table width="99%">
                                                    <tr>
                                                        <td style="width:17%"></td>
                                                        <td style="width:83%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:17%"></td>
                                                        <td style="width:83%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:17%; text-align:left;">
                                                            <asp:Label ID="Lbl_Capitulo" runat="server" Text="Capitulo" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="width:83%">
                                                             <asp:DropDownList ID="Cmb_Capitulo" runat="server" Width="100%" AutoPostBack="true"
                                                             OnSelectedIndexChanged="Cmb_Capitulo_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left; width:17%;">
                                                            <asp:Label ID="Lbl_Partida_Especifica" runat="server" Text="Partida Especifica" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="width:83%; text-align:left;">
                                                            <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Width="100%">
                                                                <asp:ListItem Value="NADA">&lt;--SELECCIONE--&gt;</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align:right;">
                                                            <asp:ImageButton ID="Btn_Agregar_Partida" runat="server" 
                                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" Width="16px" 
                                                                onclick="Btn_Agregar_Partida_Click"  />
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div style="height:auto; max-height:350px; overflow:auto; width:100%; vertical-align:top;">
                                                    <asp:GridView ID="Grid_Partidas_Stock" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False"  Width="96%"
                                                        GridLines= "None">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="PARTIDA_ID" HeaderText="PARTIDA_ID" SortExpression="PARTIDA_ID"/>
                                                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE" >
                                                                <ItemStyle HorizontalAlign ="Center" Font-Size="X-Small" Width="90px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre de la Partida" SortExpression="FECHA_APERTURA" >
                                                                <ItemStyle HorizontalAlign ="Left" Font-Size="X-Small"/>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Quitar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Btn_Quitar_Partida" runat="server" 
                                                                        ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                        CommandArgument='<%#Eval("PARTIDA_ID")%>' onclick="Btn_Quitar_Partida_Click" 
                                                                        OnClientClick="return confirm('¿Esta seguro que desea elimina el registro?');"/>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign ="Center" Font-Size="X-Small" Width="30px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                 </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                </center>        
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

