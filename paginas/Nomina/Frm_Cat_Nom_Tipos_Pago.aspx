<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Tipos_Pago.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Tipos_Pago" Title="Catálogo Tipos de Pago" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sc_Cat_Nom_Tipos_Pago" runat="server" AsyncPostBackTimeout="360000" ScriptMode="Release"/>
<asp:UpdatePanel ID="UPnl_Cat_Nom_Tipos_Pago" runat="server">
    <ContentTemplate>
      <asp:UpdateProgress ID="UPgs_Cat_Nom_Tipos_Pago" runat="server" 
            AssociatedUpdatePanelID="UPnl_Cat_Nom_Tipos_Pago" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>                
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <%--mensajes de erro--%>
        <div id="Div_Tipos_Pago" style="background-color:#ffffff; width:100%; height:100%;">
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td colspan="4" class="label_titulo">
                        C&aacute;talogo de Tipos de Pagos
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    </td>
                </tr>
            </table>             
           <%-- botones --%>
            <table width="100%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div  align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">  
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                            OnClientClick="return confirm('Desea eliminar este Elemento. ¿Confirma que desea proceder?');"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                    </td>
                                    <td align="right" style="width:41%;">
                                         <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="30" TabIndex="5"  
                                                        ToolTip = "Buscar Nombre" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" 
                                                        TargetControlID="Txt_Busqueda" ValidChars="ÑñáéíóúÁÉÍÓÚ."
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  />
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                                        onclick="Btn_Buscar_Click" />
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
             
            <%--zona de datos--%>
                <table width="100%">
                    <tr>
                        <td style="width: 15%;" align="left"> 
                            <asp:Label ID="Lbl_Tipos_Pago_ID" runat="server" Text="Tipos Pago ID" Width="100%"></asp:Label>
                        </td>
                        <td style="width: 40%;">
                            <asp:TextBox ID="Txt_Tipos_Pago_ID" runat="server" Width="35%" ReadOnly="true"></asp:TextBox>
                         </td>
                        <td style="width: 25%;"></td>
                        <td style="width: 20%;"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre" Width="100%"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            <%--zona del grid--%>
            <div id="Div_Grid_Tipos_Pago" runat="server" style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;">
                <table width="100%">
                    <tr>
                        <td width="100%">
                            <asp:GridView ID="Grid_Tipos_Pago" runat="server"  CssClass="GridView_1" Width="100%" 
                                AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" 
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                EmptyDataText="No se encuentra ningun Tipo de Pago"
                                OnSelectedIndexChanged="Grid_Tipos_Pago_SelectedIndexChanged"
                                OnSorting="Grid_Tipos_Pago_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Tipo_Pago_Id" HeaderText="ID" Visible="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Center" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" Visible="True" SortExpression="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="93%" />
                                        <ItemStyle HorizontalAlign="Left" Width="93%" />
                                    </asp:BoundField>
                                </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>