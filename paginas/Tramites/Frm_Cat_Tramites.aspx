<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Tramites.aspx.cs" Inherits="paginas_tramites_Frm_Cat_Tramites"
    Title="Catalogo de Tramites" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        //  para buscar que el porcentaje no se pase del 100%
        function on(ctrl) {
            var Valor = parseFloat(ctrl.value);
            var Suma;
            var Diferencia;

            if (Valor > 100) {
                $('input[id$=Txt_Valor_Subproceso]').val('');
                alert('El Valor no puede ser mayor a 100%!!');
            }
            else if (Valor == 0) {
                $('input[id$=Txt_Valor_Subproceso]').val('');
                alert('El Valor no puede 0%!!');
            }
            else {
                if (document.getElementById("<%=Txt_Valor_Subproceso_Acumulado.ClientID%>").value <= 100) {
                    var Valor_Acumulado = parseFloat(document.getElementById("<%=Txt_Valor_Subproceso_Acumulado.ClientID%>").value);
                    Suma = Valor + Valor_Acumulado;

                    if (Suma > 100) {
                        Diferencia = (Suma - 100);
                        $('input[id$=Txt_Valor_Subproceso]').val('');
                        alert('El Valor ' + Valor + ' exece con ' + Diferencia + ' al 100%!!');
                    }
                }
            }
        }
        //  consultar la clave por si se encuentra repetida no dejar que la guarde en la base de datos
        function Consultar_Clave(ctrl) {
            var Clave_Ingresada = ctrl;
            var Clave_Id = (document.getElementById("<%=Txt_ID_Tramite.ClientID%>").value);
            var cadena = "Accion=Consultar_Clave&id=" + Clave_Ingresada + "&";

            if ((Clave_Ingresada.length > 0)) {
                if (Clave_Id.length == 0) {
                    $.ajax({
                        url: "Frm_Cat_Tramites.aspx?" + cadena,
                        type: 'POST',
                        async: false,
                        cache: false,
                        success: function(Datos) {
                            if (Datos == "Repetido") {
                                alert(" La clave " + ctrl + " ya se encuentra registrada.");
                                $('input[id$=Txt_Clave_Tramite]').val('');
                            }
                        }
                    });
                }
            }
        }
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
               
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Tramites" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">
                            Catálogo de Tr&aacute;mites
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" style="font-family:Arial; font-size:10px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                           
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                            <asp:ImageButton ID="Btn_Ver_Requisitos" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                Width="24px" Height="24px" OnClick="Btn_Ver_Requisitos_Click" CausesValidation="false" />
                            <asp:ImageButton ID="Btn_Ver_Formato" runat="server" ImageUrl="~/paginas/imagenes/paginas/report.png"
                                Width="22px" Height="22px" OnClick="Btn_Ver_Formato_Click" Visible="False" CausesValidation="False" />
                        </td>
                        <td>
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" OnClick="Btn_Busqueda_Avanzada_Click">Búsqueda Avanzada</asp:LinkButton>
                            <%--<asp:TextBox ID="Txt_Busqueda_Tramite" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Tramite" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Busqueda_Tramite_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Tramite" runat="server" WatermarkText="<Nombre>" TargetControlID="Txt_Busqueda_Tramite" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>--%>
                        </td>
                    </tr>
                </table>
                <%-- pestaña de tramites generales --%>
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1" Width="100%">
                        <HeaderTemplate>
                            Tramites</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Tramite_id" runat="server" style="display: none">
                                    <table width="98%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_ID_Tramite" runat="server" Text="Tramite ID" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%">
                                                <asp:TextBox ID="Txt_ID_Tramite" runat="server" Width="98%" MaxLength="5" Enabled="False"></asp:TextBox>
                                                <asp:HiddenField ID="Hdf_Clave_Tramite" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Clave_Tramite" runat="server" Text="*Clave Trámite" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%;" align="left">
                                            <asp:TextBox ID="Txt_Clave_Tramite" runat="server" Width="95%" MaxLength="20" name="<%=Txt_Clave_Tramite.ClientID %>"
                                                Style="text-transform: uppercase"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Estatus_Tramite" runat="server" Text="Estatus" CssClass="estilo_fuente"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:DropDownList ID="Cmb_Estatus_Tramite" runat="server" Width="80%" DropDownStyle="DropDownList"
                                                MaxLength="0" AutoPostBack="True">
                                                <asp:ListItem Text="Activo" Value="ACTIVO" />
                                                <asp:ListItem Text="Baja" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="*U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 84%;" align="left">
                                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="96%" DropDownStyle="DropDownList"
                                                MaxLength="0" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Dependencias_SelectedIndexChanged" />
                                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Dependencia_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Areas" runat="server" Text="*Area" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 84%;" align="left">
                                            <asp:DropDownList ID="Cmb_Areas" runat="server" Width="96%" DropDownStyle="DropDownList"
                                                AutoPostBack="True" MaxLength="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 84%;" align="left">
                                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="95%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Descripcion" runat="server" Text="*Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 84%; text-align: left; vertical-align: top;">
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Div_tipo_Tramite" runat="server" style="display: none">
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 15%; text-align: left;">
                                                <asp:Label ID="Lbl_Tipo_Tramite" runat="server" Text="* Tipo Tramite" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 35%">
                                                <asp:DropDownList ID="Cmb_Tipo_Tramite" runat="server" Width="99%">
                                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                    <asp:ListItem Text="FIJO" Value="FIJO" />
                                                    <asp:ListItem Text="VARIABLE" Value="VARIABLE" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:HiddenField runat="server" ID="Hdf_Cuenta_Contable_ID" />
                                            <asp:Label ID="Lbl_Cuenta" runat="server" Text="*Cuenta" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:DropDownList ID="Cmb_Cuenta" runat="server" Width="90%" Enabled="False" AutoPostBack="True"
                                                OnSelectedIndexChanged="Cmb_Cuenta_SelectedIndexChanged" />
                                            <asp:ImageButton ID="Btn_Busqueda_Avanzada_Cuenta_Contable" runat="server" ToolTip="Seleccionar Cuenta contable"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Busqueda_Avanzada_Cuenta_Contable_Click" />
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Costo" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%;">
                                            <asp:TextBox ID="Txt_Costo" runat="server" Width="95%" MaxLength="15" onblur="$('input[id$=Txt_Costo]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Filt_Txt_Costo" runat="server" TargetControlID="Txt_Costo"
                                                FilterType="Custom, Numbers" ValidChars=",." Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Tiempo_Estimado" runat="server" Text="*Tiempo Estimado (d&iacute;as)"
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Tiempo_Estimado" runat="server" Width="95%" MaxLength="3"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tiempo_Estimado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Tiempo_Estimado" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Documento" runat="server" Text="Documento" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <cc1:AsyncFileUpload ID="FileUp" runat="server" CompleteBackColor="LightSteelBlue"
                                                UploadingBackColor="LightBlue" FailedValidation="False" Width="95%" />
                                        </td>
                                    </tr>
                                </table>
                                <%--<table width="98%" style="display: block">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Parametro1" runat="server" Text="Parametro 1" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                           
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Parametro2" runat="server" Text="Parametro 2" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Operador1" runat="server" Text="Operador " CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                           
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Operador2" runat="server" Text="Operador parametros" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            
                                        </td>
                                    </tr>
                                </table>--%>
                                <br />
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width: 100%; text-align: center; vertical-align: top;">
                                            <center>
                                                <div id="Div_Tramites_Generales" runat="server" style="overflow: auto; height: 200px;
                                                    width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                    <asp:GridView ID="Grid_Tramites_Generales" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                                        Width="96%" GridLines="None" EmptyDataText="No se encuentra ningun registro favor de seleccionar Búsqueda Avanzada"
                                                        OnSelectedIndexChanged="Grid_Tramites_Generales_SelectedIndexChanged" OnPageIndexChanging="Grid_Tramites_Generales_PageIndexChanging">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="TRAMITE_ID" HeaderText="Tramite ID" SortExpression="TRAMITE_ID">
                                                                <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                                <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CLAVE_TRAMITE" HeaderText="Clave Tramite" SortExpression="Clave_Tramite">
                                                                <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Size="13px" />
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="12px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="Nombre">
                                                                <HeaderStyle HorizontalAlign="Left" Width="75%" Font-Size="13px" />
                                                                <ItemStyle HorizontalAlign="Left" Width="75%" Font-Size="12px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                     <cc1:TabPanel runat="server" HeaderText="TabPnl_Parametros" ID="TabPnl_Parametros" Width="100%">
                        <HeaderTemplate>Parametros</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Parametros" runat="server" style="display: block">
                                    <table width="98%">
                                        <tr>
                                            <td style="width:5%">
                                                <asp:Label ID="Lbl_Costo_Parametro" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Operador1" runat="server" Text="Operador parametro 1" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Parametro1" runat="server" Text="[ (Parametro 1" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Operador2" runat="server" Text="Operador parametro 2" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:15%">
                                                <asp:Label ID="Lbl_Parametro2" runat="server" Text="Parametro 2" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                             <td style="width:15%">
                                                <asp:Label ID="Lbl_Operador3" runat="server" Text="Operador parametro 3" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:20%">
                                                <asp:Label ID="Lbl_Parametro3" runat="server" Text="Parametro 3) ]" CssClass="estilo_fuente"></asp:Label>
                                            </td>
 					                    </tr>
 					                    <tr>
 					                        <td style="width:5%">
                                                <asp:Label ID="Lbl_Parametro_Costo" runat="server" Text="$" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:15%">
                                                <asp:TextBox ID="Txt_Operador1" runat="server" Width="90%" MaxLength="1" style="text-align:center"></asp:TextBox>
                                            </td>
                                            <td style="width:17%">
                                                <asp:TextBox ID="Txt_Parametro1" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width:15%">
                                                <asp:TextBox ID="Txt_Operador2" runat="server" Width="90%" MaxLength="1" style="text-align:center"></asp:TextBox>
                                            </td>
                                            <td style="width:16%">
                                                <asp:TextBox ID="Txt_Parametro2" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
                                            </td>
                                             <td style="width:15%">
                                                <asp:TextBox ID="Txt_Operador3" runat="server" Width="90%" MaxLength="1" style="text-align:center"></asp:TextBox>
                                            </td>
                                            <td style="width:17%">
                                                <asp:TextBox ID="Txt_Parametro3" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
                                            </td>
 					                    </tr>
                                    </table>
                                </div>
 			                </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="Tab_Matriz_Costo" Width="100%"
                        Enabled="true" Visible="true">
                        <HeaderTemplate>
                            Matriz de costo</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="98%" style="display: block">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Matriz_Tipo" runat="server" Text="Tipo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Matriz_Tipo" runat="server" Width="95%" MaxLength="200"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Matriz_Costo" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Matriz_Costo" runat="server" Width="85%"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Matiz" runat="server" ImageUrl="~/paginas/imagenes/gridview/add_grid.png"
                                                Width="20px" Height="20px" AlternateText="Agregar elemento a la matriz" OnClick="Btn_Matiz_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%" style="display: block">
                                    <tr>
                                        <td style="width: 100%; text-align: left;">
                                            <div id="Div_Grid_Matriz_Costo" runat="server" style="overflow: auto; height: 200px;
                                                width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                <asp:GridView ID="Grid_Matriz_Costo" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                                    Width="96%" GridLines="None" EmptyDataText="No se encuentra ningun registro"
                                                    OnSelectedIndexChanged="Grid_Tramites_Generales_SelectedIndexChanged" OnPageIndexChanging="Grid_Tramites_Generales_PageIndexChanging">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <%-- 0 --%>
                                                        <asp:BoundField DataField="MATRIZ_ID" HeaderText="MATRIZ_ID" SortExpression="MATRIZ_ID">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                                        </asp:BoundField>
                                                        <%-- 1 --%>
                                                        <asp:BoundField DataField="TRAMITE_ID" HeaderText="TRAMITE_ID" SortExpression="TRAMITE_ID">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                                        </asp:BoundField>
                                                        <%-- 2 --%>
                                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" SortExpression="TIPO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="40%" Font-Size="13px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                                        </asp:BoundField>
                                                        <%-- 3 --%>
                                                        <asp:BoundField DataField="COSTO_BASE" HeaderText="Costo" SortExpression="COSTO_BASE">
                                                            <HeaderStyle HorizontalAlign="Left" Width="40%" Font-Size="13px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                                        </asp:BoundField>
                                                        <%-- 4 --%>
                                                        <asp:TemplateField HeaderText="Quitar" HeaderStyle-Font-Size="12px" ItemStyle-Font-Size="11px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Img_Quitar" OnClick="Btn_Img_Quitar_Click" ButtonType="Image"
                                                                    runat="server" Width="16px" Height="16px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                            <ItemStyle HorizontalAlign="center" Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2" Width="100%" Enabled="false"
                        Visible="false">
                        <HeaderTemplate>
                            Autorizaci&oacute;n de Tramite</HeaderTemplate>
                        <ContentTemplate>
                            *
                            <center>
                                <table width="98%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField ID="Hdf_Detalle_Autorizar_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Autorizar" runat="server" Text="Autoriza" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Cmb_Autorizadores" runat="server" Width="99%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Perfil" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                AlternateText="Agregar Autorizador" OnClick="Btn_Agregar_Perfil_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Perfil" runat="server" ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png"
                                                AlternateText="Modificar Autorizador" OnClick="Btn_Modificar_Perfil_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Perfil" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                                                AlternateText="Quitar Autorizador" OnClick="Btn_Quitar_Perfil_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Perfiles" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                    AllowPaging="True" PageSize="5" Width="96%" OnPageIndexChanging="Grid_Perfiles_PageIndexChanging"
                                    GridLines="None">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" />
                                        <asp:BoundField DataField="DETALLE_AUTORIZACION_ID" HeaderText="DETALLE_AUTORIZACION_ID"
                                            SortExpression="DETALLE_AUTORIZACION_ID" />
                                        <asp:BoundField DataField="PERFIL_ID" HeaderText="Perfil ID" SortExpression="PERFIL_ID" />
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3" Width="100%">
                        <HeaderTemplate>
                            Informacion Requerida</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Ocultar_Id_Requisitos" runat="server" style="display: none">
                                    <table width="98%">
                                        <tr>
                                            <td colspan="4">
                                                <asp:HiddenField ID="Hdf_Dato_Tramite_ID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Dato_Tramite_ID" runat="server" Text="Dato Tramite ID" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%">
                                                <asp:TextBox ID="Txt_Dato_Tramite_ID" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Nombre_Dato" runat="server" Text="*Nombre" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" colspan="2">
                                            <asp:TextBox ID="Txt_Nombre_Dato" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Descripcion_Dato" runat="server" Text="*Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left" colspan="2">
                                            <asp:TextBox ID="Txt_Descripcion_Dato" runat="server" Rows="3" TextMode="MultiLine"
                                                Width="99%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion_Dato" runat="server" TargetControlID="Txt_Descripcion_Dato"
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Dato_Requerido" runat="server" Text="Requerido" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:CheckBox ID="Chk_Dato_Requerido" runat="server" Text="(Solo si es Obligatorio)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Dato_Inicial" runat="server" Text="Inicial" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:CheckBox ID="Chk_Dato_Inicial" runat="server" Text="(Si se requiere un campo en la solicitud)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Dato_Final" runat="server" Text="Final" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:CheckBox ID="Chk_Dato_Final" runat="server" Text="(Si se requiere un campo de resolucion)" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 85%;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 15%;" align="right">
                                            <asp:ImageButton ID="Btn_Agregar_Dato" runat="server" Width="20px" Height="20px"
                                                ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Dato"
                                                OnClick="Btn_Agregar_Dato_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Dato" runat="server" Width="20px" Height="20px"
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" AlternateText="Modificar Dato"
                                                OnClick="Btn_Modificar_Dato_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Dato" runat="server" Width="20px" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                AlternateText="Quitar Dato" OnClick="Btn_Quitar_Dato_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width: 95%; text-align: center; vertical-align: top;">
                                            <center>
                                                <div id="Div_Grid_Datos_Tramite" runat="server" style="overflow: auto; height: 200px;
                                                    width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                    <asp:GridView ID="Grid_Datos_Tramite" runat="server" Width="96%" CssClass="GridView_1"
                                                        GridLines="None" AutoGenerateColumns="False" OnPageIndexChanging="Grid_Datos_Tramite_PageIndexChanging"
                                                        OnSelectedIndexChanged="Grid_Datos_Tramite_OnSelectedIndexChanged" EmptyDataText="No se encuentra ningun requisito">
                                                        <Columns>
                                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="DATO_ID" HeaderText="Dato ID" SortExpression="DATO_ID">
                                                                <HeaderStyle Height="0%" HorizontalAlign="Left" />
                                                                <ItemStyle Height="0%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                                <HeaderStyle Font-Size="13px" Width="65%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Width="65%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TIPO_DATO" HeaderText="Tipo de dato">
                                                                <HeaderStyle Font-Size="13px" Width="15%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Width="15%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ORDEN" HeaderText="Orden">
                                                                <HeaderStyle Font-Size="13px" Width="15%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Width="15%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </div>
                                            </center>
                                        </td>
                                        <td style="width: 5%" align="center">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="Btn_Subir_Dato" runat="server" Width="20px" Height="20px" ImageUrl="~/paginas/imagenes/paginas/subir.png"
                                                            OnClick="Btn_Subir_Dato_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="Btn_Bajar_Dato" runat="server" Width="20px" Height="20px" ImageUrl="~/paginas/imagenes/paginas/bajar.png"
                                                            OnClick="Btn_Bajar_Dato_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4" Width="100%">
                        <HeaderTemplate>
                            Requisitos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="98%">
                                    <tr>
                                        <td colspan="3">
                                            <asp:HiddenField ID="Hdf_Detalle_Documento" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Documento_Tramite" runat="server" Text="Documento" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 70%; text-align: left;" align="left">
                                            <asp:DropDownList ID="Cmb_Documentos_Tramites" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%;">
                                          
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Documento_Requerido" runat="server" Text="Requerido" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 75%" align="left">
                                            <asp:CheckBox ID="Chk_Documento_Requerido" runat="server" Text="(Solo si es Obligatorio)" />
                                        </td>
                                        <td style="width: 15%;">
                                             <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Width="20px" Height="20px"
                                                ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Documento"
                                                OnClick="Btn_Agregar_Documento_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Documento" runat="server" Width="20px" Height="20px"
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" AlternateText="Modificar Documento"
                                                OnClick="Btn_Modificar_Documento_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Documento" runat="server" Width="20px" Height="20px"
                                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" AlternateText="Quitar Documento"
                                                OnClick="Btn_Quitar_Documento_Click" />
                                        </td>
                                    </tr>
                                </table>
                                
                                <br />
                                
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width: 100%; text-align: center; vertical-align: top;">
                                            <center>
                                                <div id="Div2" runat="server" style="overflow: auto; height: 200px; width: 99%; vertical-align: top;
                                                    border-style: solid; border-color: Silver; display: block">
                                                    <asp:GridView ID="Grid_Documentos_Tramite" runat="server" Width="96%" CssClass="GridView_1"
                                                        GridLines="None" AutoGenerateColumns="False" OnPageIndexChanging="Grid_Documentos_Tramite_PageIndexChanging"
                                                        EmptyDataText="No se encuentra ningun documento">
                                                        <Columns>
                                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                <ItemStyle Width="5%" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="DETALLE_DOCUMENTO_ID" HeaderText="DETALLE_DOCUMENTO_ID"
                                                                SortExpression="DETALLE_DOCUMENTO_ID" />
                                                            <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Documento ID" SortExpression="DOCUMENTO_ID" />
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="Nombre">
                                                                <HeaderStyle Font-Size="13px" Height="85%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Height="85%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="DOCUMENTO_REQUERIDO" HeaderText="Requerido" SortExpression="DOCUMENTO_REQUERIDO">
                                                                <HeaderStyle Font-Size="13px" Height="10%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Height="10%" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel5" Width="100%">
                        <HeaderTemplate>
                            Flujo del Tramite</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Ocultar_id_Flujo" runat="server" style="display: none">
                                    <table width="98%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="Hdf_SubProceso_ID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Subproceso_ID" runat="server" Text="Subproceso ID" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%">
                                                <asp:TextBox ID="Txt_Subproceso_ID" runat="server" Width="98%" MaxLength="5" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Nombre_Subproceso" runat="server" Text="Actividad" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <%-- <asp:TextBox ID="Txt_Nombre_Subproceso" runat="server" Width="99%" 
                                                MaxLength="100"></asp:TextBox>--%>
                                            <asp:DropDownList ID="Cmb_Nombre_Actividad" runat="server" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Tipo_Actividad" runat="server" Text="Tipo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:DropDownList ID="Cmb_Tipo_Actividad" runat="server" Width="98%" AutoPostBack="true"
                                                OnSelectedIndexChanged="Cmb_Tipo_Actividad_SelectedIndexChanged">
                                                <asp:ListItem>&lt; SELECCIONE &gt;</asp:ListItem>
                                                <asp:ListItem>AUTORIZADO</asp:ListItem>
                                                <asp:ListItem>ELABORAR</asp:ListItem>
                                                <asp:ListItem>COBRO</asp:ListItem>
                                                <asp:ListItem>CONDICION</asp:ListItem>
                                                <asp:ListItem>TERMINADO</asp:ListItem>
                                                <asp:ListItem>VALIDACION</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Condicion_Si" runat="server" Text="(No Actividad) Condicion SI"
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%" align="left">
                                            <asp:TextBox ID="Txt_Condicion_Si" runat="server" Width="95%" MaxLength="3" Enabled="False"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FEE_Txt_Condicion_Si" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Condicion_Si" Enabled="True" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 15%; text-align: right;">
                                            <asp:Label ID="Lbl_Condicion_No" runat="server" Text="Condicion NO" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%" align="left">
                                            <asp:TextBox ID="Txt_Condicion_No" runat="server" Width="95%" MaxLength="3" Enabled="False"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FEE_Txt_Condicion_No" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Condicion_No" Enabled="True" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Valor_Subproceso" runat="server" Text="Valor" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:TextBox ID="Txt_Valor_Subproceso" runat="server" Width="45%" MaxLength="3" name="<%=Txt_Valor_Subproceso.ClientID %>"
                                                onkeyup="javascript:on(this);" Style="text-align: right"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Subproceso" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Valor_Subproceso" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            %
                                        </td>
                                        <td style="width: 15%; text-align: right;">
                                            <asp:Label ID="Lbl_Valor_Acumulado" runat="server" Text="Acumulado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:TextBox ID="Txt_Valor_Subproceso_Acumulado" runat="server" Width="45%" name="<%=Txt_Valor_Subproceso_Acumulado.ClientID %>"
                                                Style="text-align: right" MaxLength="3" Enabled="False"></asp:TextBox>
                                            %
                                        </td>
                                        <td style="width: 40%; text-align: right;">
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Descripcion_Subproceso" runat="server" Text="Descripción" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 85%;">
                                            <asp:TextBox ID="Txt_Descripcion_Subproceso" runat="server" Rows="3" TextMode="MultiLine"
                                                Width="99%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion_Subproceso" runat="server"
                                                TargetControlID="Txt_Descripcion_Subproceso" WatermarkText="Límite de Caractes 100"
                                                WatermarkCssClass="watermarked" Enabled="True">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Pnl_Detalle_Tramite" runat="server" GroupingText="Plantilla">
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 80%;" align="left">
                                                <asp:DropDownList ID="Cmb_Platillas" runat="server" Width="75%" DropDownStyle="DropDownList"
                                                    AutoCompleteMode="SuggestAppend" CssClass="WindowsStyle" MaxLength="0" />
                                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Plantillas" runat="server" ToolTip="Seleccionar plantilla"
                                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                    OnClick="Btn_Busqueda_Avanzada_Plantillas_Click" />
                                            </td>
                                            <td style="width: 20%;" align="center">
                                                <asp:ImageButton ID="Btn_Agregar_Plantilla" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Subproceso"
                                                    OnClick="Btn_Agregar_Plantilla_Click" />
                                                <asp:ImageButton ID="Btn_Quitar_Plantilla" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" AlternateText="Quitar Subproceso"
                                                    OnClick="Btn_Quitar_Plantilla_Click" />
                                                <asp:HiddenField ID="Hdf_Orden_Subporceso" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 100%">
                                                <center>
                                                    <div id="Div_Grid_Detalle_Plantilla" runat="server" style="overflow: auto; height: 100px;
                                                        width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                        <asp:GridView ID="Grid_Detalle_Plantilla" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                                            Width="96%" GridLines="None">
                                                            <RowStyle CssClass="GridItem" />
                                                            <Columns>
                                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="5%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="5%" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SubProceso ID" SortExpression="SUBPROCESO_ID" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="Nombre">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="85%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="85%" />
                                                                </asp:BoundField>
                                                                <%--<asp:BoundField DataField="ORDEN" HeaderText="Orden"
                                                                    SortExpression="Orden">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="10%" />
                                                                </asp:BoundField>--%>
                                                            </Columns>
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                        </asp:GridView>
                                                    </div>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Pnl_Detalle_Formato" runat="server" GroupingText="Formato">
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 80%;" align="left">
                                                <asp:DropDownList ID="Cmb_Formato" runat="server" Width="75%" DropDownStyle="DropDownList"
                                                    AutoCompleteMode="SuggestAppend" CssClass="WindowsStyle" MaxLength="0" />
                                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Formatos" runat="server" ToolTip="Seleccionar formato"
                                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                    OnClick="Btn_Busqueda_Avanzada_Formatos_Click" />
                                            </td>
                                            <td style="width: 20%;" align="center">
                                                <asp:ImageButton ID="Btn_Agregar_Formato" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Subproceso"
                                                    OnClick="Btn_Agregar_Formato_Click" />
                                                <asp:ImageButton ID="Btn_Quitar_Formato" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" AlternateText="Quitar Subproceso"
                                                    OnClick="Btn_Quitar_Formato_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 100%">
                                                <center>
                                                    <div id="Div_Grid_Detalle_Formato" runat="server" style="overflow: auto; height: 100px;
                                                        width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                        <asp:GridView ID="Grid_Detalle_Formato" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                                            Width="96%" GridLines="None">
                                                            <RowStyle CssClass="GridItem" />
                                                            <Columns>
                                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="5%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="5%" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SubProceso ID" SortExpression="SUBPROCESO_ID" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="Nombre">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="85%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="85%" />
                                                                </asp:BoundField>
                                                                <%-- <asp:BoundField DataField="ORDEN" HeaderText="Orden"
                                                                    SortExpression="Orden">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="10%" />
                                                                </asp:BoundField>--%>
                                                            </Columns>
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                        </asp:GridView>
                                                    </div>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table width="98%">
                                </table>
                                <asp:Panel ID="Pnl_SubProceso" runat="server" GroupingText="Actividad">
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 85%;">
                                                &nbsp;
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:ImageButton ID="Btn_Agregar_Subproceso" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Subproceso"
                                                    OnClick="Btn_Agregar_Subproceso_Click" />
                                                <asp:ImageButton ID="Btn_Modificar_Subproceso" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" AlternateText="Modificar Subproceso"
                                                    OnClick="Btn_Modificar_Subproceso_Click" />
                                                <asp:ImageButton ID="Btn_Quitar_Subproceso" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" AlternateText="Quitar Subproceso"
                                                    OnClick="Btn_Quitar_Subproceso_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 95%; text-align: center; vertical-align: top;">
                                                <center>
                                                    <div id="Div1" runat="server" style="overflow: auto; height: 200px; width: 99%; vertical-align: top;
                                                        border-style: solid; border-color: Silver; display: block">
                                                        <asp:GridView ID="Grid_Subprocesos_Tramite" runat="server" CssClass="GridView_1"
                                                            AutoGenerateColumns="False" Width="96%" OnPageIndexChanging="Grid_Subprocesos_Tramite_PageIndexChanging"
                                                            GridLines="None">
                                                            <RowStyle CssClass="GridItem" />
                                                            <Columns>
                                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="5%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="5%" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SubProceso ID" SortExpression="SUBPROCESO_ID" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="Nombre">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="85%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="85%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ORDEN" HeaderText="Orden" SortExpression="Orden">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="10%" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                        </asp:GridView>
                                                    </div>
                                                </center>
                                            </td>
                                            <td style="width: 5%; vertical-align: middle;">
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="Btn_Subir_Orden_Subproceso" runat="server" Width="20px" Height="20px"
                                                                    ImageUrl="~/paginas/imagenes/paginas/subir.png" OnClick="Btn_Subir_Orden_Subproceso_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="Btn_Bajar_Orden_Subproceso" runat="server" Width="20px" Height="20px"
                                                                    ImageUrl="~/paginas/imagenes/paginas/bajar.png" OnClick="Btn_Bajar_Orden_Subproceso_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
