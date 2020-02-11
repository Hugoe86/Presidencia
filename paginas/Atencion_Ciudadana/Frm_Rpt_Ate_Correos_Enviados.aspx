<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Ate_Correos_Enviados.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Rpt_Ate_Correos_Enviados"
    Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 20%;
        }
        .style2
        {
            width: 30%;
        }
        .tamanio_combos
        {
            width: 98%;
        }
        .tamanio_cajas_texto_con_boton
        {
            width: 87%;
        }
        .tamanio_cajas_texto
        {
            width: 96%;
        }
    </style>

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 97%; height: 100%;">
                <table border="0" cellspacing="0" class="estilo_fuente" style="height: auto; width: 99%;">
                    <tr>
                        <td colspan="4" align="center" class="label_titulo">
                            Reporte de correos enviados
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="4">
                            &nbsp;
                            <asp:ImageButton ID="Btn_Consultar" runat="server" ToolTip="Consultar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" OnClick="Btn_Consultar_Click" />
                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" ToolTip="Exportar a PDF" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" OnClick="Btn_Exportar_pdf_Click" />
                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" ToolTip="Exportar a Excel"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_xls.png" OnClick="Btn_Exportar_Excel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Advertencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="False" />
                            <asp:Label ID="Lbl_Informacion" runat="server" Enabled="False" ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Desde Fecha
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" CssClass="tamanio_cajas_texto_con_boton"></asp:TextBox>
                            <ajax:CalendarExtender ID="Cln_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Cln_Txt_Fecha_Inicio">
                            </ajax:CalendarExtender>
                            <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Inicio" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                        </td>
                        <td class="style1" style="text-align: right;">
                            Hasta fecha
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" CssClass="tamanio_cajas_texto_con_boton"></asp:TextBox>
                            <ajax:CalendarExtender ID="Cln_Txt_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Cln_Txt_Fecha_Fin">
                            </ajax:CalendarExtender>
                            <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Fin" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Tipo de envío
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Tipo_Envio" runat="server" CssClass="tamanio_combos">
                                <asp:ListItem Value="" Text="TODOS"></asp:ListItem>
                                <asp:ListItem Value="NOTIFICACION" Text="NOTIFICACION"></asp:ListItem>
                                <asp:ListItem Value="FELICITACION" Text="FELICITACION"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style1" style="text-align: right;">
                            Dirección de correo electrónico
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Email" runat="server" CssClass="tamanio_cajas_texto" MaxLength="100"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="Fte_Txt_Email" runat="server" TargetControlID="Txt_Email"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ._-@">
                            </ajax:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Ciudadano
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Nombre_Contribuyente" runat="server" CssClass="tamanio_cajas_texto"
                                MaxLength="100"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="Fte_Txt_Nombre_Contribuyente" runat="server" TargetControlID="Txt_Nombre_Contribuyente"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </ajax:FilteredTextBoxExtender>
                        </td>
                        <td class="style1" style="text-align: right;">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="Grid_Correos_Enviados" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                GridLines="None" Width="99%" HeaderStyle-CssClass="tblHead" Style="font-size: small;
                                white-space: normal;">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="NOMBRE_CIUDADANO" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESTINATARIO" HeaderText="Correo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MOTIVO" HeaderText="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_NOTIFICACION" HeaderText="Fecha notificación" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Font-Size="Small" />
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
