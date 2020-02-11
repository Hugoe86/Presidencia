<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Modificar_Datos_Perito_Externo.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Peritos_Externos.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Modificar_Datos_Perito_Externo" %>
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
            padding:0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding:0 5px 0 5px;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tool_ScriptManager" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" /></div></ProgressTemplate></asp:UpdateProgress><div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Modificar Registro Perito Externo
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
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
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" AlternateText="Modificar"/><asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir"/></td>
                                        <td align="right" style="width: 41%;">
                                           
                                            </table>--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Div_Grid_Datos_Peritos" runat="server" visible="true">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_Mod_Perito_Externo" runat="server" />
                        </td>
                        <tr style="background-color: #3366CC">
                            <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                Seguridad del Perito Externo
                            </td>
                        <tr>
                                <td style="text-align:left;width:20%;">
                                    *Usuario
                                </td>
                                <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Usuario" runat="server" Enabled="false" MaxLength="50" 
                                             TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                <td style="text-align:left;width:20%;text-align:left;">
                                    *Contraseña
                                </td>
                                <td style="text-align:left;width:30%;">
                                    <asp:TextBox ID="Txt_Password" TextMode="Password" runat="server" Enabled="false" MaxLength="50" 
                                         TabIndex="11" Width="98%" />
                                </td>
                        </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td style="text-align:left;width:20%;">
                                    *Confirmar Contraseña
                                </td>
                                <td style="text-align:left;width:30%;">
                                    <asp:TextBox ID="Txt_Password_Confirma" TextMode="Password" runat="server" Enabled="false" 
                                        MaxLength="50" TabIndex="9" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                        </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        Datos del Perito Externo
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Nombre
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Enabled="false" MaxLength="50" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
                                        *Apellido Paterno
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Enabled="false" 
                                            MaxLength="50" style="text-transform:uppercase" TabIndex="11" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Apellido Materno
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Enabled="false" 
                                            MaxLength="50" style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Calle
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Calle" runat="server" Enabled="false" MaxLength="50" 
                                            style="text-transform:uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
                                        *Colonia
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Colonia" runat="server" Enabled="false" MaxLength="50" 
                                            style="text-transform:uppercase" TabIndex="11" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;text-align:left">
                                        *Ciudad
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Ciudad" runat="server" Enabled="false" MaxLength="20" 
                                            style="text-transform:uppercase" TabIndex="11" Width="98%" />
                                    </td>
                                    <td style="text-align:left;width:20%;text-align:left;">
                                        *Estado
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Estado" runat="server" Enabled="false" MaxLength="20" 
                                            style="text-transform:uppercase" TabIndex="11" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        *Teléfono
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Telefono" runat="server" Enabled="false" MaxLength="10" 
                                            style="text-transform:uppercase;" TabIndex="13" Width="98%" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Anio" runat="server" 
                                            FilterType="Numbers" TargetControlID="Txt_Telefono" />
                                    </td>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        Celular
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Celular" runat="server" Enabled="false" MaxLength="10" 
                                            style="text-transform:uppercase;" TabIndex="13" Width="98%" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                            FilterType="Numbers" TargetControlID="Txt_Celular" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        *Estatus
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="false" TabIndex="12" 
                                            Width="98%">
                                            <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                            <asp:ListItem Text="VALIDAR" Value="VALIDAR"></asp:ListItem>
                                            <asp:ListItem Text="POR PAGAR" Value="POR PAGAR"></asp:ListItem>
                                            <asp:ListItem Text="PAGADO" Value="PAGADO"></asp:ListItem>
                                            <asp:ListItem Text="VALIDADO" Value="VALIDADO"></asp:ListItem>
                                            <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        *Fecha Vencimiento
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Enabled="false" TabIndex="13" 
                                            Width="98%" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="text-align:left;width:20%; vertical-align:top;">
                                        Información adicional
                                    </td>
                                    <td colspan="3" style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Informacion_Adicional" runat="server" Enabled="false" 
                                            MaxLength="250" Rows="3" style="text-transform:uppercase;" TabIndex="13" 
                                            TextMode="MultiLine" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                        </table>
                    </div>                
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>