<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Claves_Catastrales.aspx.cs" 
Inherits="paginas_Catastro_Frm_Cat_Cat_Claves_Catastrales" MasterPageFile ="~/paginas/Paginas_Generales/MasterPage.master" 
Title="Catálogo de Claves Catastrales"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <script type='text/javascript'>
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
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="3" class="label_titulo">
                            Cat&aacute;logo de Claves Catastrales
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    </table>
                    
                    <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td >
                            <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                            Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
		                                    Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
		                                    Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
						            </td>
						        <td align="right" style="width:41%;">
						    <table style="width:100%;height:28px;">
                                <tr>
                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                    <td style="width:55%;">
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="4"  ToolTip = "Buscar" Width="180px" style="text-transform:uppercase"/>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                            runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                    </td>
                                    <td style="vertical-align:middle;width:5%;" >
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                            AlternateText="Buscar" onclick="Btn_Buscar_Click" />
                                    </td>
                                 </tr>
                             </table>
                            </td>
                            </tr>
                            </table>
                            <caption>
                                <br />
                            </caption>
		                </div>
               </td>
               </table>     
                   </br>
                        
               <center>
                    <table width="98%" class="estilo_fuente">
                        <div id="Div_Grid_Claves_Catastrales" runat="server" visible="true">
                        </div>
                        
                        <tr style="background-color: #3366CC" align="center">
                            <td style="text-align: center; font-size: 17px; color: #FFFFFF;" colspan="4" >
                                Cláves Catastrales
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:HiddenField ID="hdf_Claves_Catastrales_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Identificador</td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" TabIndex="9" Enabled="false"
                                    Style="text-transform: uppercase" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: Left;">
                                *Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                    <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: Left;">
                                    *Tipo
                             </td>
                             <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="ACC" Value="ACC"></asp:ListItem>
                                <asp:ListItem Text="MCC" Value="MCC"></asp:ListItem>
                                <asp:ListItem Text="AC" Value="AC"></asp:ListItem>
                                <asp:ListItem Text="MC" Value="MC"></asp:ListItem>
                                </asp:DropDownList>
                             </td>
                        </tr>
                        <tr>
                            <td>
                                <td>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Claves_Catastrales" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                    HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_PageIndexChanging" 
                                    OnSelectedIndexChanged="Grid_SelectedIndexChanged" PageSize="20" 
                                    Style="white-space: normal;" Width="100%">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CLAVES_CATASTRALES_ID" HeaderText="Id" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador" >
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Center"  />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo">
                                            <HeaderStyle HorizontalAlign="Center"  />
                                            <ItemStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <div ID="Div_Grid_Tab_Val" runat="server" visible="false">
                        </div>
                        
                    </table>
            </center>
            
            <caption>
                <br />
                <br />
            </caption>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

