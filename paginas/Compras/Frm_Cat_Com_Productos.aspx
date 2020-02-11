<%@ Page Title="Catálogo de Productos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Productos.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Productos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
// <!CDATA[
function Hr3_onclick() {
}
// ]]>85180811
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Productos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                
            <div id="Div_Productos" style="background-color:#ffffff; width:100%; height:100%;">          
                <table  id="Tabla_Generar" width="100%" class="estilo_fuente"> 
                 <tr>
                  <td>
                       <table width="100%" class="estilo_fuente">
                            <tr align="center">
                                <td class="label_titulo">
                                    Catálogo de Productos
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"/>&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                </td>
                            </tr>
                       </table>
            
                       <table width="98%"  border="0" cellspacing="0">
                                 <tr align="center">
                                     <td>                
                                         <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                              <table style="width:100%;height:28px;">
                                                <tr>
                                                  <td align="left" style="width:59%;">  
                                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="20"
                                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                        <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" 
                                                            AlternateText="Consultar" CssClass="Img_Button" 
                                                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                                            onclick="Btn_Exportar_Excel_Click" ToolTip="Exportar Excel" />
                                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                                  </td>
                                                  
                                                 </tr>
                                              </table>
                                            </div>
                                     </td>
                                 </tr>
                        </table>  
                 </td>
                </tr>                                
                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Producto" runat="server" 
                WatermarkCssClass="watermarked"
                WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Nombre_Producto_B" />
                 <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Producto" 
                runat="server" TargetControlID="Txt_Nombre_Producto_B" 
                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ "/>
                
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                WatermarkCssClass="watermarked"
                WatermarkText="<Ingrese Descripción>" TargetControlID="Txt_Descripcion_Producto_B" />
                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                runat="server" TargetControlID="Txt_Descripcion_Producto_B" 
                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ "/>
                
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                WatermarkCssClass="watermarked"
                WatermarkText="<Ingrese Clave>" TargetControlID="Txt_Clave_B" />
                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                runat="server" TargetControlID="Txt_Clave_B" 
                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ "/>
                
                
                 <div id="Div_Busqueda_Av" runat="server" style="width: 98%; height: 100%;"> 
                <tr>
                     <td>             
                             <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                                <tr>                         
                                    
                                     <td align="left" style="width:30%;">
                                         &nbsp;&nbsp;&nbsp; <asp:TextBox ID="Txt_Nombre_Producto_B" runat="server"  MaxLength="50" Width="230px"></asp:TextBox>
                                         &nbsp;&nbsp;
                                    <asp:TextBox ID="Txt_Descripcion_Producto_B" runat="server"  MaxLength="50" Width="280px"></asp:TextBox>
                                        &nbsp;&nbsp;
                                     <asp:TextBox ID="Txt_Clave_B" runat="server"  MaxLength="10" Width="120px" ></asp:TextBox>
                                        &nbsp;&nbsp;<asp:ImageButton ID="Btn_Buscar_Producto" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        onclick="Btn_Buscar_Producto_Click" ToolTip="Buscar Producto" TabIndex="2"/>
                                        &nbsp;<asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                        onclick="Btn_Limpiar_Click" ToolTip="Limpiar"  />
                                    </td>
                                </tr>
                              
                             </table>
                     </td>
                </tr>
                <tr>
                    <td>
                        <hr ID="Hr1" runat="server" onclick="return Hr3_onclick() " style="width:95%; text-align:left;" />
                    </td>
                </tr>
            </div >
                
                
                
                    <caption>
                        <br />
                        <asp:HiddenField ID="Txt_Producto_ID" runat="server" />
                        <asp:HiddenField ID="Hdn_Txt_Descripcion" runat="server" />
                        <tr>
                            <td>
                                <table class="estilo_fuente" width="100%">
                                  <tr>
                                        
                                        <td>
                                            <table class="estilo_fuente" width="100%">
                                                <!-- Campos datos de producto -->
                                                <tr>
                                                    <td style="text-align:left;width:15%">
                                                        <asp:Label ID="Lbl_Txt_Clave" runat="server" AssociatedControlID="Txt_Clave" 
                                                            Text="Clave" ></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;">
                                                        <asp:TextBox ID="Txt_Clave" runat="server" BackColor="#F3F3F3" ReadOnly="True" style="width:246px;" 
                                                            TabIndex="7" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave" runat="server" 
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                            TargetControlID="Txt_Clave" ValidChars="ÑñáéíóúÁÉÍÓÚ-" />
                                                    </td>
                                                    <td style="text-align:left;width:15%;">
                                                        <asp:Label ID="Lbl_Tipo" runat="server" 
                                                            AssociatedControlID="Cmb_Tipo" Text="*Tipo"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;">
                                                        <asp:DropDownList ID="Cmb_Tipo" runat="server" style="width:260px;" 
                                                            TabIndex="8">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="Lbl_Txt_Nombre_Producto" runat="server" 
                                                            AssociatedControlID="Txt_Nombre_Producto" Text="*Nombre"></asp:Label>
                                                    </td>
                                                    <td colspan="3" style="text-align:left;">
                                                        <asp:TextBox ID="Txt_Nombre_Producto" runat="server" MaxLength="100" 
                                                            style="width:99%" TabIndex="9" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Producto" runat="server" 
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                            TargetControlID="Txt_Nombre_Producto" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ " />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="Lbl_Txt_Descripcion" runat="server" 
                                                            AssociatedControlID="Txt_Descripcion" Text="*Descripción"></asp:Label>
                                                    </td>
                                                    <td colspan="3" style="text-align:left;width:30%;" >
                                                        <asp:TextBox ID="Txt_Descripcion" runat="server" Height="95" MaxLength="3600" 
                                                            style="width:99%;" TabIndex="10" TextMode="MultiLine" />
                                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Descripcion" runat="server" 
                                                            TargetControlID="Txt_Descripcion" WatermarkCssClass="watermarked" 
                                                            WatermarkText="Límite de Caractes 3600">
                                                        </cc1:TextBoxWatermarkExtender>
                                                        <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                            runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                            TargetControlID="Txt_Descripcion" 
                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="Lbl_Chk_Stock" runat="server" Text="*Stock"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;">
                                                        <asp:DropDownList ID="Cmb_Stock" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="Cmb_Stock_SelectedIndexChanged" style="width:250px" 
                                                            TabIndex="11">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="text-align:left;width:15%;">
                                                        <asp:Label ID="Lbl_Cmb_Estatus" runat="server" 
                                                            AssociatedControlID="Cmb_Estatus" Text="Estatus"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;">
                                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" style="width:260px;" 
                                                            TabIndex="12">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="Lbl_Txt_Costo" runat="server" AssociatedControlID="Txt_Costo" 
                                                            Text="*Costo"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;">
                                                        <asp:TextBox ID="Txt_Costo" runat="server" AutoPostBack="true" MaxLength="9" 
                                                            ontextchanged="Txt_Costo_TextChanged" style="width:246px;" TabIndex="13" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo" runat="server" 
                                                            FilterType="Custom, Numbers" TargetControlID="Txt_Costo" ValidChars=".," />
                                                    </td>
                                                    <td style="text-align:left;width:15%;">
                                                        <asp:Label ID="Lbl_Txt_Costo_Promedo" runat="server" 
                                                            AssociatedControlID="Txt_Costo" Text="Costo promedio" Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;">
                                                        <asp:TextBox ID="Txt_Costo_Promedio" runat="server" MaxLength="15" Enabled="true" Visible="false"
                                                             style="width:256px;" TabIndex="14" BackColor="#F3F3F3"  />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo_Promedio" runat="server" 
                                                            FilterType="Custom, Numbers" TargetControlID="Txt_Costo_Promedio" 
                                                            ValidChars="." />
                                                    </td>
                                                </tr>
                                                <!-- Combos tipo de producto-->
                                                <tr>
                                                    <td style="text-align:left;">
                                                        *Unidad</td>
                                                    <td style="text-align:left;width:30%" colspan="3">
                                                        <asp:DropDownList ID="Cmb_Unidad" runat="server"  Width="99%" 
                                                            TabIndex="15">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td style="text-align:left;">
                                                        *<asp:Label ID="Lbl_Capitulo" runat="server" Text="Capítulo"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="Cmb_Capitulo" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="Cmb_Capitulo_SelectedIndexChanged" 
                                                            TabIndex="16" 
                                                            Width="99%">
                                                        </asp:DropDownList>
                                                    </td>                                                                                                                                                                                                                
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;width:15%;">
                                                        <asp:Label ID="Lbl_Concepto" runat="server" Text="*Concepto"></asp:Label>
                                                    </td>
                                                    <td style="text-align:left;width:30%;" colspan="3">
                                                        <asp:DropDownList ID="Cmb_Conceptos" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="Cmb_Conceptos_SelectedIndexChanged" TabIndex="17" 
                                                            Width="99%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;">
                                                        
                                                        <asp:Label ID="Lbl_Partida_General" runat="server" Text="*Partida Generica"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="Cmb_Partida_General" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="Cmb_Partida_General_SelectedIndexChanged" TabIndex="18" 
                                                            Width="99%">
                                                        </asp:DropDownList>
                                                    </td>                                                    
                                                </tr>
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="Lbl_Partida_Especifica" runat="server" Text="*Partida Especifica"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="Btn_Descripcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_advice.png" onclick="Btn_Descripcion_Click" ToolTip="Descripción" Visible="false" />
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" AutoPostBack="True" onselectedindexchanged="Cmb_Partida_Especifica_SelectedIndexChanged" TabIndex="19" 
                                                            Width="99%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <div ID="Div_Datos_Especificos" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                                                        <tr>
                                                            <td colspan="4">
                                                                <hr ID="Hr3" runat="server" onclick="return Hr3_onclick() " style="width:95%; text-align:left;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Impuesto
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:DropDownList ID="Cmb_Impuesto" runat="server" AutoPostBack="true" onselectedindexchanged="Cmb_Impuesto_SelectedIndexChanged" style="width:250px;" TabIndex="20">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="text-align:left; width:15%;">
                                                                Impuesto 2
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:DropDownList ID="Cmb_Impuesto_2" runat="server" style="width:260px;" TabIndex="21">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <!-- Campos existencia de producto -->
                                                        <tr style="display:none;">
                                                            <td>
                                                                <asp:Label ID="Lbl_Txt_Existencia" runat="server" AssociatedControlID="Txt_Existencia" Text="*Existencia"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Existencia" runat="server" AutoPostBack="true" BackColor="#F3F3F3" MaxLength="15" ontextchanged="Txt_Existencia_TextChanged" style="width:246px;" 
                                                                    TabIndex="22" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Existencia" runat="server" FilterType="Numbers" TargetControlID="Txt_Existencia" />
                                                            </td>
                                                            <td style="text-align:left;width:15%;">
                                                                <asp:Label ID="Lbl_Txt_Minimo" runat="server" AssociatedControlID="Txt_Minimo" Text="*Mínimo"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Minimo" runat="server" AutoPostBack="true" BackColor="#F3F3F3" MaxLength="15" ontextchanged="Txt_Minimo_TextChanged" style="width:256px;" TabIndex="23" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Minimo" runat="server" FilterType="Numbers" TargetControlID="Txt_Minimo" />
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td style="text-align:left;">
                                                                <asp:Label ID="Lbl_Txt_Comprometido" runat="server" AssociatedControlID="Txt_Comprometido" Text="Comprometido"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Comprometido" runat="server" BackColor="#F3F3F3" Enabled="false" MaxLength="22" ReadOnly="true" style="width:246px;" TabIndex="24" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comprometido" runat="server" FilterType="Numbers" TargetControlID="Txt_Comprometido" />
                                                            </td>
                                                            <td style="text-align:left;width:15%;">
                                                                <asp:Label ID="Lbl_Txt_Maximo" runat="server" AssociatedControlID="Txt_Maximo" Text="*Máximo"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Maximo" runat="server" AutoPostBack="true" BackColor="#F3F3F3" MaxLength="15" ontextchanged="Txt_Maximo_TextChanged" style="width:256px;" TabIndex="25" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Maximo" runat="server" FilterType="Numbers" TargetControlID="Txt_Maximo" />
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td style="text-align:left;">
                                                                <asp:Label ID="Lbl_Txt_Disponible" runat="server" AssociatedControlID="Txt_Disponible" Text="Disponible"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Disponible" runat="server" BackColor="#F3F3F3" Enabled="false" MaxLength="15" ReadOnly="true" style="width:246px;" TabIndex="26" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Disponible" runat="server" FilterType="Numbers" TargetControlID="Txt_Disponible" />
                                                            </td>
                                                            <td style="text-align:left;width:15%;">
                                                                <asp:Label ID="Lbl_Txt_Reorden" runat="server" AssociatedControlID="Txt_Reorden" Text="Reorden"></asp:Label>
                                                            </td>
                                                            <td style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Reorden" runat="server" BackColor="#F3F3F3" Enabled="false" MaxLength="10" ReadOnly="true" style="width:256px;" TabIndex="27" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Reorden" runat="server" FilterType="Numbers" TargetControlID="Txt_Reorden" />
                                                            </td>
                                                        </tr>
                                                        <!-- Campo Ubicacion de producto -->
                                                        <tr style="display:none;">
                                                            <td style="text-align:left;">
                                                                <asp:Label ID="Lbl_Txt_Ubicacion" runat="server" AssociatedControlID="Txt_Ubicacion" Text="Ubicación"></asp:Label>
                                                            </td>
                                                            <td colspan="3" style="text-align:left;width:30%;">
                                                                <asp:TextBox ID="Txt_Ubicacion" runat="server" BackColor="#F3F3F3" MaxLength="255" style="width:658px;" TabIndex="28" />
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ubicacion" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" TargetControlID="Txt_Ubicacion" 
                                                                    ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                            </td>
                                                        </tr>
                                                    </div>
                                                    <tr align="center">
                                                        <td colspan="4">
                                                          <div style="overflow:auto;height:210px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                                                            <asp:GridView ID="Grid_Productos" runat="server" AllowPaging="false" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                                onpageindexchanging="Grid_Productos_PageIndexChanging" onselectedindexchanged="Grid_Productos_SelectedIndexChanged" Style="white-space:normal" Width="100%">
                                                                <Columns>
                                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                        <ItemStyle HorizontalAlign="Center" Width="7%" />
                                                                    </asp:ButtonField>
                                                                    <asp:BoundField DataField="Producto_ID" HeaderText="Producto ID" Visible="False">
                                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True">
                                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Nombre" HeaderText="Producto" Visible="True">
                                                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="25%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True">
                                                                        <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="35%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MODELOS_NOMBRE" HeaderText="Modelo">
                                                                        <ItemStyle Font-Size="X-Small" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Stock" HeaderText="Stock" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Disponible" HeaderText="Disponible" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="GridSelected" />
                                                                <PagerStyle CssClass="GridHeader" />
                                                                <HeaderStyle CssClass="GridHeader" />
                                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                            </asp:GridView>
                                                          </div>  
                                                        </td>
                                                    </tr>
                                                </tr>
                                                
                                                
                                                
                                                
                                            </table>
                                        </td>
                                   </tr>
                                   
                                  <tr>
                                     <td >
                                               
                                    </td>
                                 </tr>
                                   
                                  
                               
                                <caption>
                                     <br />
                                    <tr>
                                        <td>
                                            <table border="0" cellspacing="0" class="estilo_fuente" width="99%">
                                               
                                            </table>
                                        </td>
                                    </tr>
                               </caption>
                                </table>
                            </td>
                        </tr>
                    </caption>
             </table> 
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Exportar_Excel" />
        </Triggers>
    </asp:UpdatePanel>
    
     <asp:Panel ID="Pnl_Foto_Producto" runat="server" Width="320px" Height="90px"
       style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none;">
       <center>
            <table width="100%" class="estilo_fuente">
              <tr  align="center" class="barra_delgada" >
                        <td align="center" > 
                            <asp:Label ID="Lbl_Foto_Producto" runat="server"  Text=" ¿Guardar Foto del Producto?" Width="100%"/>
                       </td>
              </tr>
              <tr>
                    <td>
                    <br />
                    </td>
              </tr>
             <tr>
                <td>
                    <center>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Aceptar_Guardar_Foto" runat="server" CssClass="button" ToolTip="Guardar Foto" 
                            onclick="Btn_Aceptar_Guardar_Foto_Click" Text="Aceptar" Width="100px"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Cancelar_Guardar_Foto" runat="server" Text="Cancelar" Width="100px" 
                        CssClass="button" onclick="Btn_Cancelar_Guardar_Foto_Click" ToolTip="Cancelar"/>
                    </center>
                    </td>
            </tr>
        </table>
       </center>
    </asp:Panel>
    
         <!-- Modal Popup Foto Producto-->
          <asp:UpdatePanel ID="Udp_Modal_Popup_Foto" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Foto_Producto" runat="server"
                            TargetControlID="Btn_MP_Login"
                            PopupControlID="Pnl_Foto_Producto"                      
                            CancelControlID="Btn_Cancelar_Guardar_Foto"
                            DropShadow="True"
                            BackgroundCssClass="progressBackgroundFilter"/>
                    <asp:Button ID="Btn_MP_Login" runat="server" Text="Button" style="display:none;"/>
              </ContentTemplate>          
         </asp:UpdatePanel>
         
         <!-- Modal Popup Modificar Foto-->
          <asp:UpdatePanel ID="Udp_Modal_Popup_Modificar_Foto" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Modificar_Foto" runat="server"
                            TargetControlID="Btn_MP_Mod_Foto"
                            PopupControlID="Pnl_Modificar_Foto"                      
                            CancelControlID="Btn_Cancelar_Modificar_Foto"
                            DropShadow="True"
                            BackgroundCssClass="progressBackgroundFilter"/>
                    <asp:Button ID="Btn_MP_Mod_Foto" runat="server" Text="Button" style="display:none;"/>
              </ContentTemplate>          
         </asp:UpdatePanel>


       <asp:Panel ID="Pnl_Modificar_Foto" runat="server" Width="320px" Height="90px"
           style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none;">
               <center>
                <table width="100%" class="estilo_fuente">
                  <tr  align="center" class="barra_delgada" >
                            <td align="center" > 
                                <asp:Label ID="Lbl_Modificar_Foto" runat="server"  Text=" ¿Modificar Foto del Producto?" Width="100%"/>
                           </td>
                  </tr>
                  <tr>
                        <td>
                        <br />
                        </td>
                  </tr>
                 <tr>
                    <td>
                        <center>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Aceptar_Modificar_Foto" runat="server" CssClass="button" ToolTip="Modificar Foto" 
                                onclick="Btn_Aceptar_Modificar_Foto_Click" Text="Aceptar" Width="100px"/>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Cancelar_Modificar_Foto" runat="server" Text="Cancelar" Width="100px" 
                            CssClass="button" onclick="Btn_Cancelar_Modificar_Foto_Click" ToolTip="Cancelar" />
                        </center>
                        </td>
                </tr>
            </table>
       </center>
    </asp:Panel>



          
</asp:Content>

