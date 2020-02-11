<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Identificadores_Predio.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Identificadores_Predio" Title="Catálogo de Identificadores de Predios"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

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
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">

    <script type="text/javascript" language="javascript">

        //Metodos para abrir los Modal PopUp's de la página
        function Abrir_Busqueda_Cuentas_Predial() {
            $find('Busqueda_Cuentas_Predial').show();
            return false;
        }
        function Abrir_Detalle_Cuenta_Predial() {
            $find('Detalle_Cuenta_Predial').show();
            return false;
        }

    </script>

    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"  EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                           Catálogo de Identificadores de Predios
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                 
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td>                
                        <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            AlternateText="Salir" onclick="Btn_Salir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
               
                <table width="98%" class="estilo_fuente">
                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos del predio
                                </td>
                            </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Cuenta Predial
                        </td>
                        <td  style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" AutoPostBack="true" 
                                MaxLength="20" TabIndex="9" Width="80%" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" 
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Cuenta_Predial" />
                            &nbsp; 
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" 
                                runat="server" Height="24px" 
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                onclick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click" TabIndex="3" 
                                ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Tipo de Predio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Tipo_Predio" runat="server" Width="95%" 
                                style="text-transform:uppercase" ReadOnly="True" Enabled="false" MaxLength="5" TabIndex="4"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Propietario
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="98%" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                    <td style="text-align:Left;width:20%;">
                            Calle
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="98%" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                    
                    <td style="text-align:left;width:20%;">
                    
                    <asp:Label ID="Lbl_Colonia" runat="server"   
                        Text="Colonia" Width="20"
                        ></asp:Label>
                        </td>
                    
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="98%" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                    <td style="text-align:left;width:20%;">
                            No. exterior
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="98%" Enabled="false"/>
                        </td>
                        </tr>
                        <tr>
                        <td style="text-align:left;width:20%;">
                            No. interior
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="98%" Enabled="false"/>
                        </td>
                        
                        
                   </tr>
                   <tr>
                        <td style="text-align:left;width:20%;">
                            Superficie de Predio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Superficie_Predio" runat="server" Width="98%" 
                                style="text-transform:uppercase" Enabled="false" MaxLength="13" TabIndex="4"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            Superficie Construida
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Superficie_Construida" runat="server" Width="95%" 
                                style="text-transform:uppercase" Enabled="false" MaxLength="13" TabIndex="4"/>
                        </td>
                        
                   </tr>
                   
                    <div id="Div_Identificadores_Predio" style="width:98%;" runat="server" visible="true">
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Identificadores del predio
                        </td>
                   </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Región
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Region" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="5" TabIndex="4"/>
                        </td>
                        <td style="text-align:left;width :20%;">
                            *Manzana
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Manzana" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="5" TabIndex="5"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Lote
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Lote" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="5" TabIndex="6"/>
                        </td>
                        <td style="text-align:right;width:20%;">
                            
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                        </td>
                   </tr>
                   </div>
                   
                   <div id="Div_Coordenadas" style="width:98%;" runat="server" visible="false">
                   <tr >
                        <td style="text-align:left;width:20%;">
                        Coordenadas Cartograficas o UTM
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Coordenadas" runat="server" Width="96.4%" TabIndex="10" Enabled="true" OnSelectedIndexChanged="Cmb_Coordenadas_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="GEOGRAFICAS" Value="CART"></asp:ListItem>
                                <asp:ListItem Text="UTM" Value="UTM"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                   </tr>
                   <div id="Div_Cartograficas" runat="server" visible="false" style="width:98%;">
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Coordenadas X
                        </td>
                   </tr>
                   <tr>
                       
                        <td style="text-align: left;width:20% ;">
                        Grados (°)
                        </td>
                        <td style="text-align: left;width:30% ;">
                            <asp:TextBox ID="Txt_X_Horas" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="3" TabIndex="7" OnTextChanged="Txt_X_Horas_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID ="Ftbe_Txt_X_Horas" FilterType = "Numbers" runat ="server" TargetControlID  = "Txt_X_Horas">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                        Minutos (')
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_X_Minutos" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="3" TabIndex="8" OnTextChanged="Txt_X_Minutos_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID ="Ftbe_Txt_X_Minutos" FilterType = "Numbers" runat ="server" TargetControlID  = "Txt_X_Minutos">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        </tr>
                        <tr >
                        <td style="text-align:left;width:20%;">
                        Segundos (")
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_X_Segundos" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="6" TabIndex="9" OnTextChanged="Txt_X_Segundos_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID ="Ftbe_Txt_X_Segundos" FilterType = "Numbers, Custom" runat ="server" TargetControlID  = "Txt_X_Segundos" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                        Orientación
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Latitud" runat="server" Width="96.4%" TabIndex="10" Enabled="false">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="ESTE" Value="E"></asp:ListItem>
                                <asp:ListItem Text="OESTE" Value="O"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                   </tr>
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Coordenadas Y
                        </td>
                   </tr>
                   <tr>
                   <td style="text-align: left;width:20% ;">
                        Grados (°)
                        </td>
                        <td style="text-align: left;width:30%;">
                            <asp:TextBox ID="Txt_Y_Horas" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="3" TabIndex="10" OnTextChanged="Txt_Y_Horas_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID ="Ftbe_Txt_Y_Horas" FilterType = "Numbers" runat ="server" TargetControlID  = "Txt_Y_Horas">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                        Minutos (')
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Y_Minutos" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="3" TabIndex="11" OnTextChanged="Txt_Y_Minutos_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID ="Ftbe_Txt_Y_Minutos" FilterType = "Numbers" runat ="server" TargetControlID  = "Txt_Y_Minutos">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        </tr>
                        <tr >
                        <td style="text-align:left;width:20%;">
                        Segundos (")
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Y_Segundos" runat="server" Width="96.4%" style="text-transform:uppercase" MaxLength="6" TabIndex="12"  OnTextChanged="Txt_Y_Segundos_TextChanged" AutoPostBack="true"/>
                            <cc1:FilteredTextBoxExtender ID = "Ftbe_Txt_Y_Segundos" FilterType = "Numbers, Custom" runat = "server" TargetControlID = "Txt_Y_Segundos" ValidChars="0123456789.">
                           </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                        Orientación
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Longitud" runat="server" Width="96.4%" TabIndex="13" Enabled="false">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="NORTE" Value="N"></asp:ListItem>
                                <asp:ListItem Text="SUR" Value="S"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                   </tr>
                   </div>
                   <div id="Div_UTM" runat="server" visible="false" style="width:98%;">
                   <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Coordenadas UTM
                        
                   </tr>
                   <tr>
                   </td>
                        <td style="text-align:left;width:20%;">
                            Coordenadas X
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Coordenadas_UTM" runat="server"  Width="96.4%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            Coordenadas Y
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Coordenadas_UTM_Y" runat="server"  Width="96.4%"/>
                        </td>
                   </tr>
                   </div>
                     </div>
                    </table>        
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>