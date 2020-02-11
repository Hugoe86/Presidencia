<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Parametros.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Parametros" Title="Catálogo de Parámetros"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 277px;
        }
        .style2
        {
            width: 117px;
        }
        .style3
        {
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type='text/javascript' >

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

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

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
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
                           Catálogo de Parámetros
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:80%;" runat="server" visible="false">
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
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            AlternateText="Salir" onclick="Btn_Salir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                <br />
                <table width="98%" class="estilo_fuente">
                <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Decimales para redondeó
                                </td>
                            </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            * Número de decimales
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox runat="server" ID="Txt_Decimales_Redondeo" Width="98%" Style="text-transform:uppercase" MaxLength="3"/>
                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Decimales_Redondeo" runat="server" FilterType="Numbers" TargetControlID="Txt_Decimales_Redondeo" />
                        </td>
                        <td class="style2"></td>
                        <td></td>
                    </tr>
                    <tr>
                    <td style="text-align:left;width:20%;">
                    </td>
                    <td class="style1">
                    </td>
                    </tr>
                    <tr style="background-color: #3366CC">
                            <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" 
                                class="style3" >
                                Porcentaje de Incremento de valor en base a Ley de Ingresos
                            </td>
                     </tr>
                     <tr>
                     <td style="text-align:left;width:20%;">
                            * Incremento de Valor %
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox runat="server" ID="Txt_Incremento_Valor" Width="98%" Style="text-transform:uppercase" OnTextChanged="Txt_Incremento_Valor_TextChanged" AutoPostBack="true" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Incremento_Valor" runat="server" FilterType="Numbers,Custom" TargetControlID="Txt_Decimales_Redondeo" ValidChars="1234567890.,"/>
                        </td>
                        <td style="text-align:left; " class="style2">
                            * Año a Calcular 
                        </td>
                        <td style="text-align:left;">
                            <asp:TextBox runat="server" ID="Txt_Anio_Nuevo" Width="40%" Style="text-transform:uppercase" MaxLength="4"/>
                            <cc1:FilteredTextBoxExtender ID="FTB_Txt_Anio_Nuevo" runat="server" FilterType="Numbers" TargetControlID="Txt_Anio_Nuevo" ValidChars="1234567890"/>
                            <asp:ImageButton ID="Btn_Calcula_Anio" runat="server" ToolTip="Calcular" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc3.gif" 
                                AlternateText="Calcular"  
                                Width="35px" Height="21px" onclick="Btn_Calcular_Anio_Click"/>
                        </td>
                     </tr>
                     <tr style="background-color: #3366CC">
                            <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                Porcentaje de Incremento de Esquina
                            </td>
                     </tr>
                     <tr>
                     <td style="text-align:left;width:20%;">
                            * Factor de EF %
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox runat="server" ID="Txt_Factor_Ef" Width="98%" Style="text-transform:uppercase" OnTextChanged="Txt_Factor_Ef_TextChanged" AutoPostBack="true" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTB_Txt_Factor_Ef" runat="server" FilterType="Numbers,Custom" TargetControlID="Txt_Decimales_Redondeo" ValidChars="1234567890.,"/>
                        </td>
                     </tr>
                     <tr style="background-color: #3366CC">
                            <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                               //
                            </td>
                     </tr>
                      <tr>
                     <td style="text-align:left;width:20%;">
                            * Columnas
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox runat="server" ID="Txt_Columnas" Width="98%" Style="text-transform:uppercase" OnTextChanged="Txt_Columnas_TextChanged" AutoPostBack="true" MaxLength="2"/>
                            <cc1:FilteredTextBoxExtender ID="FTB_Txt_Columnas" runat="server" FilterType="Numbers" TargetControlID="Txt_Columnas" ValidChars="1234567890"/>
                        </td>
                        </tr>
                        <tr>
                        <td style="text-align:left;width:20%;">
                            * Renglones
                        </td>
                        <td class="style1" style="text-align:left; width:30%;" >
                            <asp:TextBox runat="server" ID="Txt_Renglones" Width="98%" Style="text-transform:uppercase" OnTextChanged="Txt_Renglones_TextChanged" AutoPostBack="true" MaxLength="2"/>
                            <cc1:FilteredTextBoxExtender ID="FTB_Txt_Renglones" runat="server" FilterType="Numbers" TargetControlID="Txt_Renglones" ValidChars="1234567890"/>
                        </td>
                     </tr>
                     <tr style="background-color: #3366CC">
                            <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                              // 
                            </td>
                     </tr>
                      <tr>
                     <td style="text-align:left;width:20%;">
                            * Dias de Vigencia
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox runat="server" ID="Txt_Dias_Vigencia" Width="98%" Style="text-transform:uppercase" OnTextChanged="Txt_Dias_Vigencia_TextChanged" AutoPostBack="true" MaxLength="2"/>
                            <cc1:FilteredTextBoxExtender ID="FTB_Txt_Dias_Vigencia" runat="server" FilterType="Numbers" TargetControlID="Txt_Dias_Vigencia" ValidChars="1234567890"/>
                        </td>
                        </tr>
                     <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                              Correos 
                        </td>
                     </tr>
                     <tr>
                        <td style="text-align:left; width: 20%;" >
                              * Correo General
                        </td>
                        <td class="style1" style="text-align:left; width:30%;" >
                              <asp:TextBox ID="Txt_Correo_General" runat="server" MaxLength="50" 
                              Style="text-transform:lowercase" Width="98%" />
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Correo Autorizacion
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox ID="Txt_Correo_Autorizacion" runat="server" MaxLength="50" 
                            Style="text-transform:lowercase" Width="98%" />
                        </td>
                    </tr>
                    <tr style="background-color: #3366CC">
                        <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                            Firmante
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            * Firmante
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                             <asp:TextBox ID="Txt_Firmante" runat="server" MaxLength="100" 
                             Style="text-transform:uppercase" Width="98%" />
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Puesto
                        </td>
                        <td class="style1" style="text-align:left; width:30%;">
                            <asp:TextBox ID="Txt_Puesto" runat="server" MaxLength="100" 
                            Style="text-transform:uppercase" Width="98%" />
                        </td>
                        </tr>
                            <tr>
                                <td style="text-align:left;width:20%;">
                                    * Firmante 2
                                </td>
                                <td class="style1"  style="text-align:left; width:30%;" >
                                    <asp:TextBox ID="Txt_Firmante_2" runat="server" MaxLength="100" 
                                        Style="text-transform:uppercase" Width="98%" />
                                </td>
                                <td style="text-align:left;width:20%;">
                                    * Puesto
                                </td>
                                <td class="style1" style="text-align:left; width:30%;">
                                    <asp:TextBox ID="Txt_Puesto_2" runat="server" MaxLength="100" 
                                        Style="text-transform:uppercase" Width="98%" />
                                </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                   Fundamentacion Legal
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;width:20%;">
                                    Fundamentacion Legal
                                </td>
                                <td colspan="3" style="width:99%; text-align:left;">
                                    <asp:TextBox ID="Txt_Fundamentacion_Legal" runat="server" Rows="3" 
                                    Style="text-transform:uppercase" MaxLength="250" 
                                    TextMode="MultiLine" Width="98%" />
                               </td>
                            </tr>
                        </tr>
                    </table>        
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>