<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Asignacion_Cuentas.aspx.cs"
    Title="Asignación de Cuentas" Inherits="paginas_Catastro_Frm_Ope_Cat_Asignacion_Cuentas"
    MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type='text/javascript'>

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

    function Abrir_Busqueda_Peritos_Internos() {
        $find('Busqueda_Peritos_Internos').show();
        return false;
    }

    function Abrir_Busqueda_Avaluos() {
        $find('Busqueda_Avaluos').show();
        return false;
    }

    function Abrir_Busqueda_Colonias() {
        $find('Busqueda_Colonias').show();
        Window_Resize();
        return false;
    }

    function Abrir_Busqueda_Calles() {
        $find('Busqueda_Calles').show();
        Window_Resize();
        return false;
    }

    function Abrir_Ventana_Modal(Url, Propiedades)
	{
		window.showModalDialog(Url, null, Propiedades);
	}

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Asignación de Cuentas
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" AlternateText="Modificar"
                                                OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText="Salir"
                                                OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:DropDownList ID="Cmb_Busqueda" runat="server" Width="40%">
                                                            <asp:ListItem Text="COLONIA" Value="COLONIA" />
                                                            <asp:ListItem Text="CALLE" Value="CALLE" />
                                                            <asp:ListItem Text="PERITO" Value="PERITO" />
                                                            <asp:ListItem Text="CUENTA PREDIAL" Value="CUENTA_PREDIAL" />
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="50%" Style="text-transform: uppercase" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
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
                <br />
                <div id="Div_Modificar" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Perito Interno
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Perito_Interno" Width="85%" Style="float: left;
                                    text-transform: uppercase" MaxLength="100" Enabled="false" />
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos" runat="server"
                                    Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Peritos_Internos();"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Peritos Internos" Width="24px" />
                                <asp:HiddenField ID="Hdf_Perito_Interno_Id" runat="server" />
                                <br />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Cuenta Predial
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Cuenta_Predial" Width="92%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                                <asp:HiddenField ID="Hdf_No_Asignacion" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="98%" class="estilo_fuente">
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:GridView ID="Grid_Cuentas_Asignadas" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Cuentas_Asignadas_SelectedIndexChanged" OnPageIndexChanging="Grid_Cuentas_Asignadas_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_ASIGNACION" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderText="Perito Interno id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PERITO_INTERNO" HeaderText="Perito Asignado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial Id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EXTERIOR" HeaderText="No. Ext.">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_INTERIOR" HeaderText="No. Int">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SUPERFICIE_CONSTRUIDA" HeaderText="Superficie Construcción">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SUPERFICIE_TOTAL" HeaderText="Terreno M2">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="EFECTOS" HeaderText="Efecto">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Nuevo" runat="server" visible="false">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Perito Interno
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Perito_Interno_N" Width="85%" Style="float: left;
                                    text-transform: uppercase" MaxLength="100" Enabled="false" />
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Peritos" runat="server" Height="24px"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Peritos_Internos();"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Peritos Internos" Width="24px" />
                            </td>
                            <td style="width: 20%; text-align: left;">
                            </td>
                            <td style="width: 30%; text-align: left;">
                            </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Búsqueda de Cuentas Prediales
                            
                            <asp:ImageButton ID="Btn_Buscar_Cuentas" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    OnClick="Btn_Buscar_Cuentas_Click" TabIndex="10" ToolTip="Búsqueda Avanzada de Cuentas Prediales"
                                    Width="24px" />
                                    <asp:ImageButton ID="Btn_Imprimir_Cuentas" Enabled="false" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                Width="24px" CssClass="Img_Button"  AlternateText="Imprimir" OnClick="Btn_Imprimir_Cuentas_Click"
                                ToolTip="Imprimir cuentas asignadas" />
                                <asp:ImageButton ID="Btn_Imprimir_Colonias" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                Width="24px" CssClass="Img_Button"  AlternateText="Imprimir" OnClick="Btn_Imprimir_Colonias_Click"
                                ToolTip="Imprimir Colonias por Actualizar" />
                            </td>
                            
                                
                                
                           
                        
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Cuenta Predial
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Cuenta_Predial_Busqueda" Width="92%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                               
                            </td>
                            
                        </tr>
                        <tr>
                        <td style="text-align: left; width: 20%;">
                                Propietario
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Propietario" Width="92%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Colonia
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Colonia_Busqueda" Width="80%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                                    <asp:ImageButton ID="Btn_Busqueda_Avanzada_Colonias" runat="server" AlternateText="Buscar"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Colonia_FilteredTextBoxExtender" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" "
                                TargetControlID="Txt_Colonia_Busqueda" />
                            <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Colonia_TextBoxWatermarkExtender"
                                runat="server" TargetControlID="Txt_Colonia_Busqueda" WatermarkText="Búsqueda de Colonias por Aproximación"
                                WatermarkCssClass="watermarked" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Calle
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Calle_Busqueda" Width="80%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                                    <asp:ImageButton ID="Btn_Busqueda_Avanzada_Calles" runat="server" AlternateText="Buscar"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Calle_FilteredTextBoxExtender" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" "
                                TargetControlID="Txt_Calle_Busqueda" />
                            <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Calle_TextBoxWatermarkExtender" runat="server"
                                TargetControlID="Txt_Calle_Busqueda" WatermarkText="Búsqueda de Calles por Aproximación"
                                WatermarkCssClass="watermarked" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                No. Exterior
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_No_Ext_Busqueda" Width="92%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                No. Interior
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_No_Int_Busqueda" Width="92%" Style="float: left;
                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Tipo de Predio
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:DropDownList ID="Cmb_Tipo_Predio" runat="server" Width="98%" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Efecto Año
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Efecto_Anio" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTB_Txt_Efecto_Anio" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Efecto_Anio" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Efecto Bimestre
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Efecto_Bimestre" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTB_Efecto_Bimestre" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Efecto_Bimestre" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Superficie de Terreno (>=)
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Terreno" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTB_Terreno" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Terreno" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Superficie de Terreno (<=)
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Terreno_Menor" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Terreno_Menor" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Terreno_Menor" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Superficie de Const (>=)
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Construccion" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTB_Txt_Construccion" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Construccion" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Superficie de Const (<=)
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Construccion_Menor" Width="92%" Style="float: left;" MaxLength="15"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTB_Txt_Construccion_Menor" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Construccion" />
                            </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Total Cuentas
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Total_Cuentas" Width="92%" Style="float: left;"
                                    Enabled="false" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Total Cuentas Asignadas
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox runat="server" ID="Txt_Total_Cuentas_Asignadas" Width="92%" Style="float: left;"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="98%" class="estilo_fuente">
                                    <tr>
                                        <td style="text-align: left;" colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Prediales" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnDataBound="Grid_Cuentas_Prediales_DataBound"
                                                Width="100%" OnPageIndexChanging="Grid_Cuentas_Prediales_PageIndexChanging" PageSize="200">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="Chk_Select_Todas" runat="server" Font-Size="X-Small" name="<%=Chk_Select_Todas.ClientID %>"
                                                                Width="98%" OnCheckedChanged="Chk_Select_Todas_CheckedChanged" AutoPostBack="true">
                                                            </asp:CheckBox></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chk_Cuenta_Asignada" runat="server" Font-Size="X-Small" name="<%=Chk_Cuenta_Asignada.ClientID %>"
                                                                Width="98%" OnCheckedChanged="Chk_Cuenta_Asignada_CheckedChanged" AutoPostBack="true">
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial Id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EXTERIOR" HeaderText="No. Ext.">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_INTERIOR" HeaderText="No. Int">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="SUPERFICIE_CONSTRUIDA" HeaderText="Superficie Construcción">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SUPERFICIE_TOTAL" HeaderText="Terreno M2">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="EFECTOS" HeaderText="Efecto">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Peritos_Internos" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Peritos_Internos"
        PopupControlID="Pnl_Busqueda_Contenedor" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Peritos_Internos"
        CancelControlID="Btn_Comodin_Close_Busqueda_Peritos_Internos" DropShadow="true"
        DynamicServicePath="" Enabled="True" />
    <asp:Button ID="Btn_Comodin_Close_Busqueda_Peritos_Internos" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Open_Busqueda_Peritos_Internos" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <%--Ventana modal de búsqueda de peritos internos--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel1" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Peritos Internos
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <%--                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     --%>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Nombre Perito Interno
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Peritos_Externos" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Peritos_Externos_SelectedIndexChanged" OnPageIndexChanging="Grid_Peritos_Externos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderStyle-Width="15%" HeaderText="ID_Empleado"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO" HeaderStyle-Width="15%" HeaderText="Perito Interno">
                                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda Peritos Internos"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Empleados_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
