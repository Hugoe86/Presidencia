<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Derechos_Supervision.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Derechos_Supervision" Title="Catalogo de Derechos Supervision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Derechos_Supervision" runat="server" />
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
                        <td class="label_titulo" colspan="2">Catálogo de Derechos de Supervision</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
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
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                       <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td>Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Derecho_Supervision" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Derecho_Supervision" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Derecho_Supervision_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Derecho_Supervision" runat="server" WatermarkText="<Identificador>" TargetControlID="Txt_Busqueda_Derecho_Supervision" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Derecho_Supervision" runat="server" TargetControlID="Txt_Busqueda_Derecho_Supervision" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>                                  
                        </td>                        
                    </tr>
                </table>   
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel1"  Width="100%"  >
                        <HeaderTemplate>Derechos Supervisi&oacute;n</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Derecho_Supervision_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_ID_Derecho_Supervision" 
                                                runat="server" Text="Derecho Supervision ID" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_ID_Derecho_Supervision" runat="server" Width="98%" 
                                                MaxLength="10" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="* Identificador" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="10" style="text-transform:uppercase"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;"><asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%" style="text-transform:uppercase"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID ="Txt_Descripcion" 
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True"/>                                                        
                                        </td>
                                    </tr>
                                </table>
                                <br />                                
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel2"  ID="TabPanel2"  Width="100%"  >
                        <HeaderTemplate>Derechos Supervisi&oacute;n - Tasas</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Derecho_Supervision_Tasa_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;"><asp:Label ID="Lbl_Impuesto_ID" 
                                                runat="server" Text="Derecho Supervision Tasa ID" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_Impuesto_ID" runat="server" Width="98%" MaxLength="10" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Anio" runat="server" Text="* Año" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Anio" runat="server" Width="98%" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" 
                                                TargetControlID="Txt_Anio" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Tasa" runat="server" Text="* Tasa" CssClass="estilo_fuente" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Tasa" runat="server" Width="98%" AutoPostBack="true" OnTextChanged="Txt_Tasa_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Tasa" runat="server" 
                                                TargetControlID="Txt_Tasa" FilterType="Numbers, Custom" Enabled="True" ValidChars="0123456789.,"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Tasa_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Tasa_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Tasa_Click" />
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Derechos_Supervision_Tasas" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;"
                                    GridLines="None" AllowPaging="True" Width="98%" CssClass="GridView_1"
                                    OnPageIndexChanging="Grid_Derechos_Supervision_Tasas_PageIndexChanging" 
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="DERECHO_SUPERVISION_TASA_ID" 
                                            HeaderText="DERECHO_SUPERVISION_TASA_ID" SortExpression="Id_Tasa" />
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" 
                                            SortExpression="Anio" />
                                        <asp:BoundField DataField="TASA" HeaderText="Tasa" 
                                            SortExpression="Estatus" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer> 
                <table id="Tbl_Derechos_Supervision" border="0" cellspacing="0" class="estilo_fuente" style="width: 100%;">
                    
                <tr>
                <asp:GridView ID="Grid_Derechos_Supervision_Generales" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="98%"
                                    EmptyDataText="&quot;No se encontraron registros&quot;"
                                    onselectedindexchanged="Grid_Derechos_Supervision_Generales_SelectedIndexChanged" 
                                    GridLines= "None"
                                    
                        onpageindexchanging="Grid_Derechos_Supervision_Generales_PageIndexChanging" 
                        EnableModelValidation="True">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <HeaderStyle Width="2%" />
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="DERECHO_SUPERVISION_ID" HeaderText="Derecho Supervisi&oacute;n ID" 
                                            SortExpression="DERECHO_SUPERVISION_ID" >
                                        <HeaderStyle Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                            SortExpression="Descripcion" >
                                        <HeaderStyle Width="70%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                            SortExpression="Estatus" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                
                
                </tr>
                </table>
                
                          
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>