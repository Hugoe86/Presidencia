<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Seguimiento_Avaluos_Urbanos_Re.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Seguimiento_Avaluos_Urbanos_Re" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Seguimiento: Avalúo Urbano Regularizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <style type="text/css">
        body
        {
            font: normal 12px auto "Trebuchet MS" , Verdana;
            background-color: #ffffff;
            color: #4f6b72;
        }
        .link
        {
            color: Black;
        }
        .Label
        {
            width: 163px;
        }
        .TextBox
        {
            text-align: right;
        }
        a.enlace_fotografia:link, a.enlace_fotografia:visited
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: normal;
            padding: 0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding: 0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
        .style2
        {
            width: 39%;
        }
        .style3
        {
            width: 20%;
            height: 15px;
        }
        .style4
        {
            width: 30%;
            height: 15px;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script type="text/javascript" language="javascript">
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

        function Abrir_Busqueda_Oficios() {
          $find('Busqueda_Oficios').show();
        return false;
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
                  <%--  <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%>
            </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Principal" runat="server" visible="true">
            <div id="Div_Calles" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Seguimiento: Aval&uacute;os Urbanos Regularización</td>
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
                        <td align="left" class="style1" colspan="2">
                            <%--<asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>--%>
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
                        <td>
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
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" >
                                    Avalúos Urbanos
                                </td>
                            </tr>
                            <tr>
                                <td align="center" >
                                    <tr align="center">
                                        <td>
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
                            <asp:HiddenField ID="Hdf_Perito_Interno_Id" runat="server" />
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
                            &nbsp; Mzna.<asp:TextBox ID="Txt_Manzana" runat="server" Width="10%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
                            &nbsp; Lote<asp:TextBox ID="Txt_Lote" runat="server" Width="10%" TabIndex="11" Enabled="false" style="text-transform:uppercase"/>
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
                        
                        <td style="text-align:left;width:20%;">
                            Oficio
                        </td>
                        <td style="text-align:left;width:30%;">
                             <asp:TextBox ID="Txt_Oficio"  runat="server" Enabled="false" MaxLength="50" Style="float: left;
                                  text-transform: uppercase" Width="85%" />
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Recepcion_Oficios" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Oficios();"
                                TabIndex="10" ToolTip="Búsqueda Avanzada Oficios" Width="24px" />
                                            
                         </td>                       
                    </tr>
                    <tr>
                    <asp:HiddenField ID="Hdf_No_Oficio" runat="server" />
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
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;"  colspan="4">
                                    II.I Clasificación de
                                    de la Zona
                                </td>
                                </tr>
                                <tr>
                                <td colspan="4">
                                <asp:GridView ID="Grid_Clasificacion_Zona" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                                    HeaderStyle-CssClass="tblHead" 
                                        OnDataBound="Grid_Clasificacion_Zona_DataBound" PageSize="20" 
                                                    style="white-space:normal;" Width="100%" >
                                                    <Columns>
                                                        <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_A" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_A.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_B" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_B.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField><asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_C" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_C.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField><asp:BoundField DataField="COLUMNA_D" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_D" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_D.ClientID %>"  Width="98%"></asp:CheckBox>
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




                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                    </td>
                                </tr>
                                <tr>
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;"  colspan="4">
                                    II.II Servicios
                                    de la Zona
                                </td>
                                </tr>
                                <tr>
                                <td colspan="4">
                                <asp:GridView ID="Grid_Servicios_Zona" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                                    HeaderStyle-CssClass="tblHead" 
                                        OnDataBound="Grid_Servicios_Zona_DataBound" PageSize="20" 
                                                    style="white-space:normal;" Width="100%" >
                                                    <Columns>
                                                        <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_A" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_A.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_B" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_B.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField><asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_C" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_C.ClientID %>"  Width="98%"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField><asp:BoundField DataField="COLUMNA_D" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Columna_D" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Chk_Columna_D.ClientID %>"  Width="98%"></asp:CheckBox>
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




                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;" colspan="4">
                                        II.III Construcción Dominante
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="4">
                                <asp:GridView ID="Grid_Construccion_Dominante" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                                    HeaderStyle-CssClass="tblHead" 
                                        OnDataBound="Grid_Construccion_Dominante_DataBound" PageSize="20" 
                                                    style="white-space:normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="Rdb_Columna_A" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                    name="<%=Rdb_Columna_A.ClientID %>"  Width="98%"></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="Rdb_Columna_B" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                    name="<%=Rdb_Columna_B.ClientID %>"  Width="98%"></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="Rdb_Columna_C" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                    name="<%=Rdb_Columna_C.ClientID %>"  Width="98%"></asp:RadioButton>
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


                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        II.IV Vías de Acceso
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Buenas" runat="server" GroupName="Rdb_Vias_Acceso"
                                            Text="Buenas" />
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Regulares" runat="server" GroupName="Rdb_Vias_Acceso" 
                                            Text="Regulares" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        
                                        <asp:RadioButton ID="Rdb_Malas" runat="server" GroupName="Rdb_Vias_Acceso" 
                                            Text="Malas" />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                        II.V Fotografía
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Plana" runat="server" GroupName="Rdb_Foto" 
                                            Text="Plana" />
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Pendiente" runat="server" GroupName="Rdb_Foto" 
                                            Text="Pendiente" />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; font-size:15px; color:#FFFFFF;background-color: #555555;">
                                       II.VI  Dens. de Construcción
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
                                        <asp:RadioButton ID="Rdb_Nueva" runat="server" 
                                            GroupName="Rdb_Tipo_Construccion" Text="Nueva" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Ampliacion" runat="server" 
                                            GroupName="Rdb_Tipo_Construccion" Text="Ampliación" />
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Remodelacion" runat="server" 
                                            GroupName="Rdb_Tipo_Construccion" Text="Remodelación" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Rentada" runat="server" 
                                            GroupName="Rdb_Tipo_Construccion" Text="Rentada" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;width:20%;">
                                        <asp:Label ID="Lbl_Calidad_Proy" runat="server" Text="Calidad del Proyecto" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Buena" runat="server" 
                                            GroupName="Rdb_Calidad_Proy" Text="Buena" />
                                    </td>
                                    <td style="text-align:right;width:20%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Mala" runat="server" 
                                            GroupName="Rdb_Calidad_Proy" Text="Mala" />
                                    </td>
                                    <td style="text-align:right;width:30%;">
                                        <asp:RadioButton ID="Rdb_Calidad_Regular" runat="server" 
                                            GroupName="Rdb_Calidad_Proy" Text="Regular" />
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
                                            <asp:BoundField DataField="ELEMENTOS_CONSTRUCCION_ID" HeaderStyle-Width="5%" 
                                                    HeaderText="Id" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ELEMENTOS_CONSTRUCCION" HeaderStyle-Width="5%" 
                                                    HeaderText="Referencia">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="A">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_A" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_A.ClientID %>" 
                                                            style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="B">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_B" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_B.ClientID %>" 
                                                            style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="C">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_C" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_C.ClientID %>" 
                                                            style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="D">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_D" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_D.ClientID %>" 
                                                            style="text-transform:uppercase" Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="E">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_E" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_E.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="F">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_F" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_F.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="G">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_G" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_G.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="H">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_H" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_H.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="I">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_I" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_I.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="J">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_J" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_J.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="K">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_K" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_K.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="L">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_L" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_L.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="M">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_M" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_M.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="N">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_N" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_N.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="O">
                                                    <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_O" runat="server" CssClass="text_cantidades_grid" 
                                                            Font-Size="X-Small" name="<%=Txt_O.ClientID %>" 
                                                            style="text-transform:uppercase" Width="90%"></asp:TextBox>
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
                                                        <asp:TemplateField HeaderText="Valor Tramo Id">
                                                            <HeaderStyle HorizontalAlign="right" Width="0%" />
                                                            <ItemStyle HorizontalAlign="right" Width="0%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tramo_Id" runat="server" Font-Size="X-Small" 
                                                                    name="<%=Txt_Tramo_Id.ClientID %>" Width="0%"></asp:TextBox>
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
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Superficie Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Terreno_Superficie_Total" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
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
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Superficie Total
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Construccion_Superficie_Total" runat="server" 
                                            Enabled="false" style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
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
                                        VIII. MEDIDAS Y COLINDANCIAS: SEGUN PLANO DE LOTIFICACION
                                    </td>
                                </tr>
                                <%--<tr id="Tr1" runat="server">
                                            <td style="text-align: left; width: 20%" class="style1">
                                                Medidas y Colindancias
                                            </td>
                                            <td colspan="3" style="width: 80%">
                                                <asp:TextBox ID="Txt_Medida_Colindancia" Style="text-transform: uppercase" runat="server"
                                                    TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%">
                                            </td>
                                            <td style="width: 30%">
                                            </td>
                                            <td style="width: 20%">
                                            </td>
                                            <td style="width: 30%">
                                                <asp:ImageButton ID="Btn_Agregar_Med_Col" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                    OnClick="Btn_Agregar_Med_Col_Click" ToolTip="Agregar Medidas y Colindancias"
                                                    Width="20px" TabIndex="17" />
                                            </td>
                                        </tr>--%>
                                        
                                       <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Colindancias" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                    OnSelectedIndexChanged="Grid_Colindancias_SelectedIndexChanged" OnPageIndexChanging="Grid_Colindancias_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="NO_COLINDANCIA" HeaderStyle-Width="15%" HeaderText="ID"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MEDIDA_COLINDANCIA" HeaderStyle-Width="96.5%" HeaderText="MEDIDAS Y COLINDANCIAS">
                                                            <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                            <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                        </asp:ButtonField>--%>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>
                                        
                                        
                                         <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                       IX. ARCHIVOS DEL AVALÚO
                                    </td>
                                </tr>
                                <tr ID="Tr_Fila_Fotografias_Bien" runat="server" >
                                <td style="text-align: left;width:20% " class="style1">
                                    Nombre del Documento
                                </td>
                                <td style="text-align: left; width:30%" class="style1" >
                                    <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%">
                                    <asp:ListItem Text="FOTO" Value="FOTO" />
                                    <asp:ListItem Text="CROQUIS" Value="CROQUIS" />
                                    <asp:ListItem Text="AUTOCAD" Value="AUTOCAD" />
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left; width:20%" class="style2">
                                    Documento
                                    </td>
                                    <td style="width:30%" class="style2">
                                    <asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="16"/>
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                            </tr>
                            <tr>
                            <td style="width:20%"></td>
                            <td style="width:30%"></td>
                            <td style="width:20%"></td>
                            <td style="width:30%"><asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                        OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px" TabIndex="17"
                                    /></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: left; width: 20%;">
                                    <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" 
                                        AllowSorting="True" AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" 
                                        OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged" PageSize="20" 
                                        OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO_DOCUMENTO" HeaderText="Anio Documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DOCUMENTO" HeaderText="Nombre Documento">
                                            <ItemStyle HorizontalAlign="Left" Width="40%"/>
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Eliminar_Fotos" runat="server" CommandName="Select" 
                                                        Height="20px" ImageUrl="~/paginas/imagenes/paginas/delete.png" 
                                                        ToolTip="Eliminar" Width="20px" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="2%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="tblHead" />
                                    </asp:GridView>
                                </td>
                            </tr>
                                        
                                        
                                        
                                        
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        X. VALOR TOTAL DEL PREDIO
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
                                    <td style="text-align:left;width:20%;">
                                        I.N.P.C. Actual
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Inpa" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                        <asp:HiddenField ID="Hdf_Valor_Inpa_Id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        I.N.P.C. Referido
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Inpr" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                        <asp:HiddenField ID="Hdf_Valor_Inpr_Id" runat="server" />
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
                                        V.R.
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Vr" runat="server" Enabled="false" 
                                            style="text-transform:uppercase" TabIndex="11" Width="96.4%" />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                       XI. IMPORTE DEL AVALÚO
                                    </td>
                                </tr>
                                <tr>
                                     <td style="text-align: left; width: 20%">
                                                Pagar Avaluo
                                            </td>
                                            <td>
                                            <asp:CheckBox ID="Chk_Pago_Avaluo" runat="server" Font-Size="X-Small"
                                                                    Width="98%" />
                                            </td>
                                    <td style="text-align:left; width:20%">
                                    Importe del Avalúo
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Precio_Avaluo" runat="server" Enabled="false" 
                                            Width="96.4%" />
                                        <asp:HiddenField ID="Hdf_Factor_Cobro1" runat="server" />
                                        <asp:HiddenField ID="Hdf_Factor_Cobro2" runat="server" />
                                        <asp:HiddenField ID="Hdf_Base_Cobro" runat="server" />
                                        <asp:HiddenField ID="Hdf_Solicitud_Id" runat="server" />
                                    </td>
                                </tr>
                                
                            </table>
                            </div>
                        
                    <div id="Div_Observaciones" runat="server" visible="false">
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Observaciones: Motivos del rechazo
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;
                            </td>
                            </tr>
                            <tr>
                        <td style="text-align:left;width:20%;">
                            Motivos de Rechazo.
                        </td>
                        <td style="text-align:left;width:70%;" colspan="3">
                            <asp:DropDownList ID="Cmb_Motivos_Rechazo" runat="server" Width="98%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                            <td style="width:30%">
                            <asp:HiddenField ID="Hdf_No_Seguimiento" runat="server" />
                            <asp:HiddenField ID="Hdf_Motivo_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Estatus" runat="server" />
                            </td>
                            <td style="width:20%;text-align:right">
                            &nbsp;
                            </td>
                            <td style="width:30%;text-align:left">
                            <asp:ImageButton ID="Btn_Agregar_Observacion" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                OnClick="Btn_Agregar_Observacion_Click"
                                TabIndex="10" ToolTip="Agregar Observación" Width="24px" />
                                &nbsp; &nbsp; 
                            <%--<asp:ImageButton ID="Btn_Actualizar_Observacion" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" 
                                OnClick="Btn_Actualizar_Observacion_Click"
                                TabIndex="10" ToolTip="Actualizar Observacion" Width="24px" />
                                &nbsp; &nbsp;--%>
                            <asp:ImageButton ID="Btn_Eliminar_Observacion" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                OnClick="Btn_Eliminar_Observacion_Click"
                                TabIndex="10" ToolTip="Eliminar Observacion" Width="24px" />
                            </td>
                            <td style="width:20%;text-align:left">
                            </td>
                            </tr>
                    <tr>
                    <td colspan="4">
                    <asp:GridView ID="Grid_Observaciones" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="10" style="white-space:normal;" 
                                                Width="100%" OnPageIndexChanging="Grid_Observaciones_PageIndexChanging"
                                                OnSelectedIndexChanged="Grid_Observaciones_SelectedIndexChanged">
                                                <Columns>

                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>

                                                    <asp:BoundField DataField="NO_SEGUIMIENTO" HeaderStyle-Width="5%" HeaderText="no seguimiento" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="MOTIVO_ID" HeaderStyle-Width="5%" HeaderText="Motivo Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="MOTIVO_DESCRIPCION" HeaderStyle-Width="93%" HeaderText="Motivo de Rechazo">
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
<cc1:ModalPopupExtender ID="Mpe_Busqueda_Oficios" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Oficios"
        PopupControlID="Pnl_Busqueda_Contenedor" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Oficios"
        CancelControlID="Btn_Comodin_Close_Busqueda_Oficios" DropShadow="true" DynamicServicePath=""
        Enabled="True" />
    <asp:Button ID="Btn_Comodin_Close_Busqueda_Oficios" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Open_Busqueda_Oficios" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <%--Ventana modal de búsqueda de Oficios--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel1" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Oficios
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
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Numero de Oficio
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_No_Oficio" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Dependencia
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Oficios" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Oficios_SelectedIndexChanged" OnPageIndexChanging="Grid_Oficios_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_OFICIO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderStyle-Width="15%" HeaderText="Descripcion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEPENDENCIA" HeaderStyle-Width="15%" HeaderText="Dependencia">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_OFICIO_RECEPCION" HeaderText="No oficio Recepcion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_RECEPCION" HeaderText="Fecha Recepcion" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
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
                                                <asp:Button ID="Btn_Busqueda_Oficios" runat="server" Text="Busqueda Oficios" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Oficios_Click" />
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
