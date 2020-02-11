<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Avaluo_Urbano.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Avaluo_Urbano" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Avalúo Urbano"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 838px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type='text/javascript' >

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

        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"  EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos" DisplayAfter="0">
            <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calles" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Aval&uacute;os Urbanos</td>
                    </tr>
                    <tr>
                        <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Imprimir" OnClick = "Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                        <td align="right" class="style1">Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server"
                                Width="130px" TabIndex="6" style="text-transform:uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                CausesValidation="false" TabIndex="7"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick = "Btn_Buscar_Click" /> 
                        </td>                        
                    </tr>
                </table>   
                <br />
                        <center>
                        <table width="98%" class="estilo_fuente">
                        <tr>
                        <td>
                    <div id="Div_Grid_Avaluo" runat="server" visible="true">
                    <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Avalúos Urbanos
                                </td>
                            </tr>
                            <tr>
                                <td align="center" >
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Avaluos_Urbanos" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Avaluos_Urbanos_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Avaluos_Urbanos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_AVALUO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ANIO_AVALUO" HeaderStyle-Width="15%" HeaderText="Anio" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="AVALUO" HeaderText="Avaluo">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
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
                                            <br />
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                            </div>
                            <div id="Div_Datos_Avaluo" runat="server" visible="false">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    I. DATOS GENERALES
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;
                            </td>
                            </tr>
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_No_Avaluo" runat="server" />
                            <asp:HiddenField ID="Hdf_Calle_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Anio_Avaluo" runat="server" />
                        </td>
                        </tr>
                      <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            *Motivo del Avalúo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Motivo_Avaluo" runat="server" Width="98%" TabIndex="12" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            No. Avalúo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_No_Avaluo" runat="server" Width="96.4%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Cuenta Predial</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="80%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" 
                                runat="server" Height="24px" 
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                onclick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click" TabIndex="10" 
                                ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px"/>
                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            Clave Catastral</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave_Catastral" runat="server" Width="30%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                            <br />
                            &nbsp; Reg.<asp:TextBox ID="Txt_Region" runat="server" Width="10%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Region" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Region" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>                                
                            &nbsp; Mzna.<asp:TextBox ID="Txt_Manzana" runat="server" Width="10%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Manzana" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Manzana" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>                                
                            &nbsp; Lote<asp:TextBox ID="Txt_Lote" runat="server" Width="10%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Lote" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Lote" ValidChars="0123456789"></cc1:FilteredTextBoxExtender> 
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Ubicación del Predio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ubicacion_Predio" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia" runat="server" Width="96.4%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Localidad</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Localidad" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            Municipio</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Municipio" runat="server" Width="96.4%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Propietario</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Propietario" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:Left;width:20%;">
                            Estatus</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" width="98%">
                            <asp:ListItem Text="POR VALIDAR" Value="POR VALIDAR" />
                            <asp:ListItem Text="RECHAZADO" Value="RECHAZADO" />
                            <asp:ListItem Text="AUTORIZADO" Value="AUTORIZADO" />
                            <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                            <asp:ListItem Text="PAGADO" Value="PAGADO" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Domicilio para Notificar</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Domicilio_Not" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            Colonia</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia_Not" runat="server" Width="96.4%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Localidad</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Localidad_Not" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:Left;">
                            Municipio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Municipio_Not" runat="server" Width="96.4%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Solicitante
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
                    </td>
                    </tr>
                    <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    II. CARACTERISTICAS DEL TERRENO
                                </td>
                            </tr>
                    <tr>
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                    Clasificación de
                        </td>
                        <td style="text-align:right;width:30%;">
                            Hab de 1a. <asp:CheckBox ID="Chk_Hab_1a" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                        <td style="text-align:right;width:20%;">
                             Media <asp:CheckBox ID="Chk_Media" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                        <td style="text-align:right;width:30%;">
                            Ecónomica <asp:CheckBox ID="Chk_Economica" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                    la zona
                        </td>
                        <td style="text-align:right;width:30%;">
                            Industrial <asp:CheckBox ID="Chk_Industrial" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                        <td style="text-align:right;width:20%;">
                             Comercial <asp:CheckBox ID="Chk_Comercial" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                        <td style="text-align:right;width:30%;">
                            Campestre <asp:CheckBox ID="Chk_Campestre" runat="server" Text=""
                                TabIndex="12" Visible="true"/>
                        </td>
                    </tr>
                    <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    
                                </td>
                            </tr>
                    <tr>
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                            Servicios en
                        </td>
                        <td style="text-align:right;width:30%;">
                            Agua
                            <asp:CheckBox ID="Chk_Agua" runat="server" TabIndex="12" Text="" 
                                Visible="true" />
                        </td>
                        <td style="text-align:right;width:20%;">
                            Drenaje
                            <asp:CheckBox ID="Chk_Drenaje" runat="server" TabIndex="12" Text="" 
                                Visible="true" />
                        </td>
                        <td style="text-align:right;width:30%;">
                            Luz
                            <asp:CheckBox ID="Chk_Luz" runat="server" TabIndex="12" Text="" 
                                Visible="true" />
                        </td>
                    </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        la zona
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        Teléfono
                                        <asp:CheckBox ID="Chk_Telefono" runat="server" TabIndex="12" Text="" 
                                            Visible="true" />
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        Pavimentos
                                        <asp:CheckBox ID="Chk_Pavimentos" runat="server" TabIndex="12" Text="" 
                                            Visible="true" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        Banquetas
                                        <asp:CheckBox ID="Chk_Banquetas" runat="server" TabIndex="12" Text="" 
                                            Visible="true" />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    
                                </td>
                            </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        Construcción Dominante
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                            <asp:RadioButton ID="Rdb_Antiguas" Text="Antiguas" runat="server" GroupName="Rdb_Const_Dominante"/>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                            <asp:RadioButton ID="Rdb_Modernas" Text="Modernas" runat="server" GroupName="Rdb_Const_Dominante"/>
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                            <asp:RadioButton ID="Rdb_Mixtas" Text="Mixta" runat="server" GroupName="Rdb_Const_Dominante"/>
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    
                                </td>
                            </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        Vías de Acceso
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Buenas" Text="Buenas" runat="server" GroupName="Rdb_Vias_Acceso"/>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Regulares" Text="Regulares" runat="server" GroupName="Rdb_Vias_Acceso"/>
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        Malas
                                        <asp:RadioButton ID="Rdb_Malas" Text="Malas" runat="server" GroupName="Rdb_Vias_Acceso"/>
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    
                                </td>
                            </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        Fotografía
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Plana" Text="Plana" runat="server" GroupName="Rdb_Foto"/>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Pendiente" Text="Pendiente" runat="server" GroupName="Rdb_Foto"/>
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    
                                </td>
                            </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        Dens. de Construcción
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:TextBox ID="Txt_Dens_Construccion" runat="server" AutoPostBack="true" 
                                            MaxLength="6" OnTextChanged="Txt_Dens_Construccion_TextChanged" 
                                            style="text-transform:uppercase" Width="98.6%" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dens_Construccion" runat="server" 
                                            Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_Dens_Construccion" ValidChars="0123456789.,">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        <%--Usuario--%>
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <%--<asp:TextBox ID="TextBox1" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false"></asp:TextBox>--%>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:right;">
                                        <%--*Confirmar Contraseña--%>
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <%--<asp:TextBox ID="Txt_Password_Confirma" runat="server" Width="96.4%" TabIndex="11" Enabled="false" TextMode="Password"  MaxLength="20"/>--%>
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        III. CONSTRUCCIÓN
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Nueva" Text="Nueva" runat="server" GroupName="Rdb_Tipo_Construccion"/>
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Ampliacion" Text="Ampliación" runat="server" GroupName="Rdb_Tipo_Construccion"/>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Remodelacion" Text="Remodelación" runat="server" GroupName="Rdb_Tipo_Construccion"/>
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Rentada" Text="Rentada" runat="server" GroupName="Rdb_Tipo_Construccion"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;width:20%;">
                                        <asp:Label ID="Lbl_Calidad_Proy" runat="server" Text="Calidad del Proyecto" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Buena" Text="Buena" runat="server" GroupName="Rdb_Calidad_Proy"/>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Mala" Text="Mala" runat="server" GroupName="Rdb_Calidad_Proy"/>
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Regular" Text="Regular" runat="server" GroupName="Rdb_Calidad_Proy"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Uso
                                    </td>
                                    <td colspan="3" style="text-align:right;width:30%;">
                                        <asp:TextBox ID="Txt_Uso" runat="server" Rows="3" 
                                            Style="float: left; text-transform:uppercase" TextMode="MultiLine" 
                                            Width="96.4%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        <%--Usuario--%>
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        IV. ELEMENTOS DE CONTRUCCIÓN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align:center;">
                                        <asp:GridView ID="Grid_Elementos_Construccion" runat="server" 
                                            AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;" 
                                            HeaderStyle-CssClass="tblHead" 
                                            OnDataBound="Grid_Elementos_Construccion_DataBound" PageSize="18" 
                                            style="white-space:normal;" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="REFERENCIA" HeaderStyle-Width="5%" 
                                                    HeaderText="Referencia">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="A">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_A" runat="server" AutoPostBack="true" 
                                                            CssClass="text_cantidades_grid" Font-Size="X-Small" name="<%=Txt_A.ClientID %>" 
                                                            OnTextChanged="Txt_A_TextChanged" style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="B">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_B" runat="server" AutoPostBack="true" 
                                                            CssClass="text_cantidades_grid" Font-Size="X-Small" name="<%=Txt_B.ClientID %>" 
                                                            OnTextChanged="Txt_B_TextChanged" style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="C">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_C" runat="server" AutoPostBack="true" 
                                                            CssClass="text_cantidades_grid" Font-Size="X-Small" name="<%=Txt_C.ClientID %>" 
                                                            OnTextChanged="Txt_C_TextChanged" style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="D">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_D" runat="server" AutoPostBack="true" 
                                                            CssClass="text_cantidades_grid" Font-Size="X-Small" name="<%=Txt_D.ClientID %>" 
                                                            OnTextChanged="Txt_D_TextChanged" style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        V. OBSERVACIONES
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        *Observaciones</td>
                                    <td colspan="3" style="width:98%; text-align:left;">
                                        <asp:TextBox ID="Txt_Observaciones" runat="server" Rows="3" 
                                            Style="float: left; text-transform:uppercase" TextMode="MultiLine" 
                                            Width="96.4%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        VI. CALCULOS DEL VALOR DEL TERRENO
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <tr align="center">
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Calculos" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Calculos_DataBound" 
                                                    OnRowCommand="Grid_Calculos_RowCommand" PageSize="15" 
                                                    style="white-space:normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="SECCION" HeaderStyle-Width="5%" HeaderText="Sección">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Superficie M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Superficie_M2" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Superficie_M2.ClientID %>" 
                                                                    OnTextChanged="Txt_Superficie_M2_Cal_TextChanged" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_M2" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Superficie_M2" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_M2" runat="server" CssClass="text_cantidades_grid" 
                                                                    Enabled="false" Font-Size="X-Small" name="<%=Txt_Valor_M2.ClientID %>" 
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_M2" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Valor_M2" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Tramo Id" Visible="false">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tramo_Id" runat="server" CssClass="text_cantidades_grid" 
                                                                    Font-Size="X-Small" name="<%=Txt_Tramo_Id.ClientID %>" Width="80%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Valor_Tramo" runat="server" 
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                                                    CommandName="Cmd_Valor_M2" Height="22px" 
                                                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" TabIndex="10" 
                                                                    ToolTip="Seleccionar Valor M2" Width="22px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Cal_TextChanged" 
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factor" runat="server" Enabled="True" 
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Factor" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="F. de Ef.">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor_Ef" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Factor_Ef.ClientID %>" 
                                                                    OnTextChanged="Txt_Factor_Ef_Cal_TextChanged" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factor_Ef" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Factor_Ef" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Total" runat="server" CssClass="text_cantidades_grid" 
                                                                    Enabled="false" Font-Size="X-Small" name="<%=Txt_Total.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Grid" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Total" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Superficie Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Terreno_Superficie_Total" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:right;">
                                        Valor Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Terreno_Valor_Total" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="11" Width="96.4%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        VII. CALCULOS DEL VALOR DE LA CONSTRUCCION
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <tr align="center">
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Valores_Construccion" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                                    HeaderStyle-CssClass="tblHead" 
                                                    OnDataBound="Grid_Valores_Construccion_DataBound" PageSize="15" 
                                                    style="white-space:normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="REFERENCIA" HeaderStyle-Width="5%" HeaderText="Ref.">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Tipo">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tipo" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Tipo.ClientID %>" OnTextChanged="Txt_Tipo_Constru_TextChanged" 
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tipo" runat="server" Enabled="True" 
                                                                    FilterType="Numbers" TargetControlID="Txt_Tipo">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Con Serv">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Con_Serv" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Con_Serv.ClientID %>" 
                                                                    OnTextChanged="Txt_Con_Serv_Constru_TextChanged" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Con_Serv" runat="server" 
                                                                    Enabled="True" FilterType="Numbers" TargetControlID="Txt_Con_Serv">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Superficie M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Superficie_M2" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Superficie_M2.ClientID %>" 
                                                                    OnTextChanged="Txt_Superficie_M2_Constru_TextChanged" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_M2" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Superficie_M2" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor X M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_X_M2" runat="server" CssClass="text_cantidades_grid" 
                                                                    Enabled="false" Font-Size="X-Small" name="<%=Txt_Valor_X_M2.ClientID %>" 
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_X_M2" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Valor_X_M2" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Construccion Id" Visible="false">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_Construccion_Id" runat="server" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Valor_Construccion_Id.ClientID %>" Width="80%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" 
                                                                    CssClass="text_cantidades_grid" Font-Size="X-Small" 
                                                                    name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Constru_TextChanged" 
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Grid" runat="server" 
                                                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Factor" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Parcial">
                                                            <HeaderStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Total" runat="server" CssClass="text_cantidades_grid" 
                                                                    Enabled="false" Font-Size="X-Small" name="<%=Txt_Total.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Total" runat="server" Enabled="True" 
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Total" 
                                                                    ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Superficie Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Construccion_Superficie_Total" runat="server" 
                                            Enabled="false" style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:right;">
                                        Valor Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Construccion_Valor_Total" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="11" Width="96.4%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        VIII. VALOR TOTAL DEL PREDIO
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Valor total del Predio
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Valor_Total_Predio" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        I.N.P.A.
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Inpa" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                        <asp:HiddenField ID="Hdf_Valor_Inpa_Id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        I.N.P.R.
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Inpr" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                        <asp:HiddenField ID="Hdf_Valor_Inpr_Id" runat="server" />
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:right;">
                                        V.R.
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Vr" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="11" Width="96.4%" />
                                    </td>
                                </tr>
                            </table>
                            </div>


                        </td>
                    </tr>
                    </table>
                    </center>
                    </div>
                <div ID="Div_Observaciones" runat="server" visible="false">
                    <table border="0" cellspacing="0" class="estilo_fuente" width="98%">
                        <tr style="background-color: #3366CC">
                            <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                Observaciones: Motivos del rechazo
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Observaciones" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                    HeaderStyle-CssClass="tblHead" 
                                    OnPageIndexChanging="Grid_Observaciones_PageIndexChanging" PageSize="10" 
                                    style="white-space:normal;" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="NO_SEGUIMIENTO" HeaderStyle-Width="5%" 
                                            HeaderText="no seguimiento" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOTIVO_ID" HeaderStyle-Width="5%" 
                                            HeaderText="Motivo Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOTIVO_DESCRIPCION" HeaderStyle-Width="93%" 
                                            HeaderText="Motivo de Rechazo">
                                        <HeaderStyle HorizontalAlign="Left" Width="93%" />
                                        <ItemStyle HorizontalAlign="Left" Width="93%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="7%" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
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
            </div>
            </td>
            </tr>
                </table>
                </center>
            </div>
            </table>
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>