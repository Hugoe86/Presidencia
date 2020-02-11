<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Indemnizacion.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Indemnizacion" Title="Catálogo de Indemnización" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { 
        Contar_Caracteres();
    }
    
    function Contar_Caracteres(){
        $('textarea[id$=Txt_Comentarios]').keyup(function() {
            var Caracteres =  $(this).val().length;
            
            if (Caracteres > 250) {
                this.value = this.value.substring(0, 250);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            }else{
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
            
            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sm_Indemnizaciones" runat="server"/>
<asp:UpdatePanel ID="UPnl_Indemnizaciones" runat="server">
    <ContentTemplate>
    
            <asp:UpdateProgress ID="Uprg_Indemnizaciones" runat="server" AssociatedUpdatePanelID="UPnl_Indemnizaciones" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenedor_Indemnizaciones" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Indemnizaci&oacute;n</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                </table>
                
                <table width="98%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td colspan="2">
                             <div align="right" class="barra_busqueda">
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="3" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="5"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                OnClientClick="return confirm('¿Está seguro de eliminar la indemnización seleccionada?');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                     B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Indemnizacion" runat="server" MaxLength="100"  TabIndex="21"
                                                        ToolTip = "Busquedad de Bancos" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="Twe_Busqueda_Indemnizacion" 
                                                        runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Nombre>" 
                                                        TargetControlID="Txt_Busqueda_Indemnizacion" />
                                                    <cc1:FilteredTextBoxExtender ID="Fte_Busqueda_Indemnizacion" 
                                                        runat="server" TargetControlID="Txt_Busqueda_Indemnizacion" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    <asp:ImageButton ID="Btn_Busqueda_Indemnizacion" runat="server" TabIndex="22"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                        onclick="Btn_Busqueda_Indemnizacion_Click"
                                                         />
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
                
                <table style="width:98%;">
                    <tr>
                        <td style="width:100%;" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left;">
                            Indemnizaci&oacute;n
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Indemnizacion" runat="server" Width="98%" TabIndex="1"/>
                        </td>
                        <td style="width:20%; text-align:left;">
                            &nbsp;&nbsp;Dias
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Dias_Indemnizacion" runat="server" Width="98%" TabIndex="2"
                                OnTextChanged="Txt_Dias_Indemnizacion_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Dias_Indemnizacion" runat="server" 
                                TargetControlID="Txt_Comentarios" FilterType="Numbers"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left;">
                            Nombre
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Indemnizacion" runat="server" Width="99.5%" TabIndex="3"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; vertical-align:top;">
                            Comentarios
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99.5%" TabIndex="4" 
                                TextMode="MultiLine" MaxLength="250" Wrap="true"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" 
                                TargetControlID ="Txt_Comentarios" WatermarkText="Límite de Caractes 250" 
                                WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" 
                                TargetControlID="Txt_Comentarios" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                
                <table style="width:98%;">
                    <tr>
                        <td style="width:100%;">
                            <asp:GridView ID="Grid_Indemnizaciones" runat="server" CssClass="GridView_1" Width="100%"
                                 AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="5"
                                 onpageindexchanging="Grid_Indemnizaciones_PageIndexChanging"
                                 OnSelectedIndexChanged="Grid_Indemnizaciones_SelectedIndexChanged"
                                 AllowSorting="True" OnSorting="Grid_Indemnizaciones_Sorting" 
                                 HeaderStyle-CssClass="tblHead">
                                 
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                            <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="INDEMNIZACION_ID" HeaderText="Indemnización" SortExpression="INDEMNIZACION_ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                        </asp:BoundField>  
                                        <asp:BoundField DataField="DIAS" HeaderText="Dias" SortExpression="DIAS">
                                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                                        </asp:BoundField>
                                    </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

