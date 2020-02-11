<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Con_Cuentas_Contables.aspx.cs" Inherits="paginas_Contabilidad_Frm_Cat_Con_Cuentas_Contables" Title="Catálogo de Cuentas Contables" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <p>
        p</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
 <script type="text/javascript" language="javascript">
 function pageLoad() { 
        Contar_Caracteres();
    }
    function Contar_Caracteres(){
        $('textarea[id$=Txt_Comentarios_Cuenta_Contable]').keyup(function() {
            var Caracteres =  $(this).val().length;
            
            if (Caracteres > 250) {
                this.value = this.value.substring(0, 250);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            }else{
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
            
            $('#Txt_Comentarios_Cuenta_Contable').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
        });
    }
</script>
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" 
        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <%--<ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>--%>
    </asp:UpdateProgress>
    <asp:ScriptManager ID="ScriptManager_Cuentas_Contables" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <div id="Div_Cuentas_Contables" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Cuentas Contables</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                
                                OnClientClick="return confirm('¿Está seguro de eliminar la Cuenta Contable seleccionada?');" 
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Cuenta_Contable" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Descripción"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Cuenta_Contable" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Descripción>" TargetControlID="Txt_Busqueda_Cuenta_Contable" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Cuenta_Contable" runat="server" 
                                TargetControlID="Txt_Busqueda_Cuenta_Contable" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ-. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Descripcion_Cuenta_Contable" runat="server" 
                                ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Descripcion_Cuenta_Contable_Click"/>
                        </td> 
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td width="15%">&nbsp;</td>
                        <td width="15%"></td>
                        <td width="15%"></td>
                        <td width="15%"></td>
                        <td width="15%"></td>
                        <td width="15%"></td>
                        <td width="10%"></td>
                    </tr>
                    <tr>
                        <td>Cuenta Contable ID</td>
                        <td colspan="2">
                            <asp:TextBox ID="Txt_Cuenta_Contable_ID" runat="server" ReadOnly="True" Width="65%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*Descripcion</td>
                        <td colspan="4">
                            <asp:TextBox ID="Txt_Descripcion_Cuenta_Contable" runat="server" MaxLength="100" TabIndex="7" Width="100%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Cuenta_Contable" runat="server" TargetControlID="Txt_Descripcion_Cuenta_Contable"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>*Nivel</td>
                        <td style="text-align:left;" colspan="2">
                            <asp:DropDownList ID="Cmb_Nivel_Cuenta_Contable" runat="server" TabIndex="8" Width="65%"/>
                        </td>
                        <td>*Cuenta</td>
                        <td>
                            <asp:TextBox ID="Txt_Cuenta_Contable" runat="server" MaxLength="20" TabIndex="9" ontextchanged="Txt_Cuenta_Contable_TextChanged" Width="100%" AutoPostBack ="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Cuenta_Contable"
                                FilterType="Custom" ValidChars="1234567890-"></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>Tipo</td>
                        <td colspan="2">
                            <asp:DropDownList ID="Cmb_Tipo_Cuenta_Contable" runat ="server" Width="65%" TabIndex="11">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>BALANCE</asp:ListItem>
                                <asp:ListItem>RESULTADO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>*Cuenta Afectable</td>  
                        <td>
                            <asp:DropDownList ID="Cmb_Cuenta_Detalle" runat="server" TabIndex="10" Width="100%">
                                <asp:ListItem>NO</asp:ListItem>
                                <asp:ListItem>SI</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Partida Presupuestal</td>
                        <td colspan="4">
                            <asp:TextBox ID="Txt_Cuenta_Presupuestal" runat="server" AutoPostBack="true" Width="32.5%"
                                MaxLength="20" ontextchanged="Txt_Cuenta_Presupuestal_TextChanged" TabIndex="13"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Cuenta_Presupuestal_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom" TargetControlID="Txt_Cuenta_Presupuestal" 
                                ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="Cmb_Cuenta_Presupuestal" runat="server" 
                                TabIndex="14" AutoPostBack="true" Width="62%"
                                onselectedindexchanged="Cmb_Cuenta_Presupuestal_SelectedIndexChanged">
                                <asp:ListItem>&lt;--Seleccione--&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Comentarios</td>
                        <td colspan="4">
                            <asp:TextBox ID="Txt_Comentarios_Cuenta_Contable" runat="server" TabIndex="15" Width="100%"
                                TextMode="MultiLine"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Cuenta_Contable" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Cuenta_Contable" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Cuenta_Contable" runat="server" 
                                TargetControlID="Txt_Comentarios_Cuenta_Contable" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>                    
                    <tr align="center">
                        <td colspan="5">
                            <asp:GridView ID="Grid_Cuenta_Contable" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                onpageindexchanging="Grid_Cuenta_Contable_PageIndexChanging" 
                                onselectedindexchanged="Grid_Cuenta_Contable_SelectedIndexChanged" 
                                onsorting="Grid_Cuenta_Contable_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Cuenta_Contable_ID" HeaderText="Cuenta ID" Visible="True" SortExpression="Cuenta_Contable_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                        <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                        <ItemStyle HorizontalAlign="Left" Width="55%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nivel" HeaderText="Nivel" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
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
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger  ControlID="Txt_Cuenta_Contable"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>