<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    CodeFile="Frm_Cat_Ven_Registrar_Ciudadano.aspx.cs" Inherits="paginas_Ventanilla_Frm_Cat_Ven_Registrar_Ciudadano" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        function whichButton(event) {
            if (event.button == 2)//RIGHT CLICK
            {
                alert("Opcion no disponible!");
            }
        }
        function noCTRL(e) {
            var code = (document.all) ? event.keyCode : e.which;
            var msg = "Opcion no disponible.";
            if (parseInt(code) == 17) //CTRL
            {
                alert(msg);
                window.clep
                window.event.returnValue = false;
            }
        }
        function borrarPortapapeles() {
            window.clipboardData.setData('text', '');
            //setInterval("window.clipboardData.setData('text','')", 100)
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
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
            <div id="Div_General" runat="server" style="background-color: #ffffff; width: 98%;
                height: 100%;">
                <%--Fin del div General--%>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Registrar usuario
                        </td>
                    </tr>
                    <tr>
                        <!--Bloque del mensaje de error-->
                        <td colspan="2">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            <td>
                    </tr>
                </table>
                <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales"
                    Width="99%" Style="font-size: 8pt; color: Navy; font-weight: bold;">
                    <table style="width: 100%; text-align: center;">
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre(s)" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Apellido_Paterno" runat="server" Text="* Apellido Paterno" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Apellido_Materno" runat="server" Text="* Apellido Materno" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Sexo" runat="server" Text="* Sexo" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="98%">
                                    <asp:ListItem Value="MASCULINO">Masculino</asp:ListItem>
                                    <asp:ListItem Value="FEMENINO">Femenino</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Estado_Nacimiento" runat="server" Text="* Estado" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Estado_Nacimiento" runat="server" Width="98%">
                                    <asp:ListItem Text="AGUASCALIENTES" Value="AGUASCALIENTES" />
                                    <asp:ListItem Text="BAJA CALIFORNIA" Value="BAJA CALIFORNIA" />
                                    <asp:ListItem Text="BAJA CALIFORNIA SUR" Value="BAJA CALIFORNIA SUR" />
                                    <asp:ListItem Text="CAMPECHE" Value="CAMPECHE" />
                                    <asp:ListItem Text="COAHUILA" Value="COAHUILA" />
                                    <asp:ListItem Text="COLIMA" Value="COLIMA" />
                                    <asp:ListItem Text="CHIAPAS" Value="CHIAPAS" />
                                    <asp:ListItem Text="CHIHUAHUA" Value="CHIHUAHUA" />
                                    <asp:ListItem Text="DISTRITO FEDERAL" Value="DISTRITO FEDERAL" />
                                    <asp:ListItem Text="DURANGO" Value="DURANGO" />
                                    <asp:ListItem Text="GUANAJUATO" Value="GUANAJUATO" Selected="True" />
                                    <asp:ListItem Text="GUERRERO" Value="GUERRERO" />
                                    <asp:ListItem Text="HIDALGO" Value="HIDALGO" />
                                    <asp:ListItem Text="JALISCO" Value="JALISCO" />
                                    <asp:ListItem Text="MEXICO" Value="MEXICO" />
                                    <asp:ListItem Text="MICHOACAN" Value="MICHOACAN" />
                                    <asp:ListItem Text="MORELOS" Value="MORELOS" />
                                    <asp:ListItem Text="NAYARIT" Value="NAYARIT" />
                                    <asp:ListItem Text="NUEVO LEON" Value="NUEVO LEON" />
                                    <asp:ListItem Text="OAXACA" Value="OAXACA" />
                                    <asp:ListItem Text="PUEBLA" Value="PUEBLA" />
                                    <asp:ListItem Text="QUERETARO" Value="QUERETARO" />
                                    <asp:ListItem Text="QUINTANA ROO" Value="QUINTANA ROO" />
                                    <asp:ListItem Text="SAN LUIS POTOSI" Value="SAN LUIS POTOSI" />
                                    <asp:ListItem Text="SINALOA" Value="SINALOA" />
                                    <asp:ListItem Text="SONORA" Value="SONORA" />
                                    <asp:ListItem Text="TABASCO" Value="TABASCO" />
                                    <asp:ListItem Text="TAMAULIPAS" Value="TAMAULIPAS" />
                                    <asp:ListItem Text="TLAXCALA" Value="TLAXCALA" />
                                    <asp:ListItem Text="VERACRUZ" Value="VERACRUZ" />
                                    <asp:ListItem Text="YUATAN" Value="YUATAN" />
                                    <asp:ListItem Text="ZACATECAS" Value="ZACATECAS" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Fecha_Nacimiento" runat="server" Text="* Fecha Nacimiento" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Anio" runat="server" Width="31%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_SelectedIndexChanged" />
                                <asp:DropDownList ID="Cmb_Mes" runat="server" Width="33%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Mes_SelectedIndexChanged">
                                    <asp:ListItem Text="Enero" Value="01" />
                                    <asp:ListItem Text="Febrero" Value="02" />
                                    <asp:ListItem Text="Marzo" Value="03" />
                                    <asp:ListItem Text="Abril" Value="04" />
                                    <asp:ListItem Text="Mayo" Value="05" />
                                    <asp:ListItem Text="Junio" Value="06" />
                                    <asp:ListItem Text="Julio" Value="07" />
                                    <asp:ListItem Text="Agosto" Value="08" />
                                    <asp:ListItem Text="Septiembre" Value="09" />
                                    <asp:ListItem Text="Octubre" Value="10" />
                                    <asp:ListItem Text="Noviembre" Value="11" />
                                    <asp:ListItem Text="Diciembre" Value="12" />
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Dia" runat="server" Width="31%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Dia_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Curp" runat="server" Text="CURP" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Curp" runat="server" Width="95%" MaxLength="18"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Curp_FTE" runat="server" TargetControlID="Txt_Curp"
                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" />
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Rfc" runat="server" Text="* RFC" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_Rfc" runat="server" Width="95%" MaxLength="13"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Email" runat="server" Text="* Email" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%;">
                                <asp:TextBox ID="Txt_Email" runat="server" Width="95%" MaxLength="50" onblur="this.value = (this.value.match(/^[A-Za-z]{1}([-\.]?\w)+@([A-Za-z]{1}[A-Za-z0-9_\-]{1,63})(\.[A-Za-z]{2,4}){1}((\.[A-Za-z]{2}){1})?$/))? this.value : '';" />
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Confirmar_Email" runat="server" Text="* Confirmar Email" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_Confirmar_Email" runat="server" Width="95%" MaxLength="50" onkeydown="return borrarPortapapeles()"
                                    onMouseDown="whichButton(event)" onblur="this.value = (this.value.match(/^[A-Za-z]{1}([-\.]?\w)+@([A-Za-z]{1}[A-Za-z0-9_\-]{1,63})(\.[A-Za-z]{2,4}){1}((\.[A-Za-z]{2}){1})?$/))? this.value : '';">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Password" runat="server" Text="* Password" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_Registrar_Password" runat="server" Width="95%" MaxLength="20"
                                    TextMode="Password"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Pregunta_Secreta" runat="server" Text="* Pregunta clave" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Pregunta_Secreta" runat="server" Width="97%">
                                    <asp:ListItem> - Selecciona - </asp:ListItem>
                                    <asp:ListItem>Lugar de nacimiento de la madre</asp:ListItem>
                                    <asp:ListItem>Mejor amigo</asp:ListItem>
                                    <asp:ListItem>Nombre de tu primera mascota</asp:ListItem>
                                    <asp:ListItem>Libro favorito</asp:ListItem>
                                    <asp:ListItem>Equipo favorito</asp:ListItem>
                                    <asp:ListItem>Nombre de la primaria a la que asististe</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Respuesta_Secreta" runat="server" Text="* Respuesta clave" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Respuesta_Secreta" runat="server" Width="95%" MaxLength="100" />
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Edad" runat="server" Text="* Edad" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Edad" runat="server" Width="95%" MaxLength="2"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom,Numbers"
                                    TargetControlID="Txt_Edad" ValidChars="0123456789" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Pnl_Datos_Direccion" runat="server" GroupingText="Domicilio" Width="99%"
                    Style="font-size: 8pt; color: Navy; font-weight: bold;">
                    <table style="width: 100%; text-align: center;">
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Estado" runat="server" Text="* Estado" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:DropDownList ID="Cmb_Estado" runat="server" Width="97%" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Estado_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Ciudad" runat="server" Text="* Ciudad" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Ciudad" runat="server" Width="97%" AutoPostBack="false"
                                    DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                    CssClass="WindowsStyle" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Colonia" runat="server" Text="* Colonia" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:DropDownList ID="Cmb_Colonias" runat="server" Width="90%" AutoPostBack="true"
                                    DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                    CssClass="WindowsStyle" OnSelectedIndexChanged="Cmb_Colonias_SelectedIndexChanged" />
                                <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Colonia_Click" />
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Calle" runat="server" Text="* Calle" Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:DropDownList ID="Cmb_Calle" runat="server" Width="90%" AutoPostBack="false"
                                    DropDownStyle="DropDown" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                    CssClass="WindowsStyle" />
                                <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Calles_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Numero" runat="server" Text="* Número" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Numero_Calle" runat="server" Width="95%" MaxLength="20"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_CP" runat="server" Text="* C.P." Style="font-size: 8pt; color: Navy;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_CP" runat="server" Width="95%" MaxLength="5"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom,Numbers"
                                    TargetControlID="Txt_CP" ValidChars="0123456789" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Telefono_Casa" runat="server" Text="* Teléfono Casa" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; cursor: default; width: 35%">
                                <asp:TextBox ID="Txt_Telefono_Casa" runat="server" Width="95%" MaxLength="15" Text=""></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom,Numbers"
                                    TargetControlID="Txt_Telefono_Casa" ValidChars="0123456789" />
                            </td>
                            <td style="text-align: left; width: 15%; cursor: default">
                                <asp:Label ID="Lbl_Telefono_Celular" runat="server" Text="Teléfono Celular" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 35%; cursor: default">
                                <asp:TextBox ID="Txt_Telefono_Celular" runat="server" Width="95%" MaxLength="15"
                                    Text=""></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom,Numbers"
                                    TargetControlID="Txt_Telefono_Celular" ValidChars="0123456789" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
