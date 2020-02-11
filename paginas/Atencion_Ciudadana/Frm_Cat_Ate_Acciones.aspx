<%@ Page Title="Acciones" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Acciones.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Acciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
    function pageLoad() {
        Contar_Caracteres();
    }

    function Contar_Caracteres() {
        $('textarea[id$=Txt_Descripcion]').keyup(function() {
            var Caracteres = $(this).val().length;

            if (Caracteres > 250) {
                this.value = this.value.substring(0, 250);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            } else {
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }

            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
        });
    }    
            
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Acciones" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    </asp:ScriptManager>
    <div id="Div_Principal" style="background-color: #ffffff; width: 100%; height: 100%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Acciones de Atención Ciudadana
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" TabIndex="0" Text="Mensajes de advertencia"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="Div_Barra_Herramientas" runat="server">
                    <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr class="barra_busqueda">
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" AlternateText="Eliminar" ToolTip="Eliminar" OnClientClick="return confirm('¿Esta seguro de eliminar el registro?');"
                                    OnClick="Btn_Eliminar_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2" align="right">
                                Búsqueda
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" ToolTip="Buscar" TabIndex="5"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., " />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Clave o Nombre>" TargetControlID="Txt_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                    OnClick="Btn_Buscar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Contenido1" runat="server" style="width: 99%;">
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td style="width: 14%;">
                                <asp:HiddenField ID="HF_ID" runat="server" />
                            </td>
                            <td style="width: 34%;">
                            </td>
                            <td style="width: 4%;">
                            </td>
                            <td style="width: 14%;">
                            </td>
                            <td style="width: 34%;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *&nbsp;Clave
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="15"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Clave" runat="server" TargetControlID="Txt_Clave"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td style="text-align: right;">
                                *&nbsp;Estatus
                            </td>
                            <td style="text-align: right;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *&nbsp;Nombre
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTB_Nombre" runat="server" TargetControlID="Txt_Nombre"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                * Tiempo estimado de solución (días)
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Tiempo_Solucion" runat="server" MaxLength="5" ToolTip="Número de días hábiles"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tiempo_Solucion" runat="server" TargetControlID="Txt_Tiempo_Solucion"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                Descripci&oacute;n
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTB_Descripción" runat="server" TargetControlID="Txt_Descripcion"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                </cc1:FilteredTextBoxExtender>
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="overflow: auto; height: 320px; width: 98.8%; vertical-align: top; border-style: outset;
                    border-color: Silver;">
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="Grid_Datos" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                    GridLines="None" Width="100%" DataKeyNames="ACCION_ID" HeaderStyle-CssClass="tblHead"
                                    EmptyDataText="No se encontraron datos para mostrar">
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar" runat="server" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                    OnClick="Btn_Seleccionar_Click" CommandArgument='<%# Eval("ACCION_ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="35px" />
                                            <ItemStyle HorizontalAlign="Center" Width="35px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ACCION_ID" HeaderText="ID" Visible="false">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    </table>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
