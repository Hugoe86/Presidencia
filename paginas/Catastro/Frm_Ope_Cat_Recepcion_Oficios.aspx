<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Recepcion_Oficios.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Recepcion_Oficios" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Recepción de Oficios"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

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
                           Recepción de Oficios
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
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            AlternateText="Nuevo" onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            AlternateText="Salir" onclick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td align="right" class="style1">
                                                    Búsqueda:
                                                    <asp:DropDownList ID="Cmb_Filtro_Busqueda" runat="server" TabIndex="4" 
                                                        Width="25%">
                                                        <asp:ListItem Text="OFICIO" Value="OFICIO"></asp:ListItem>
                                                        <asp:ListItem Text="DEPARTAMENTO" Value="DEPARTAMENTO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="Txt_Busqueda_Oficio" runat="server" 
                                                        Style="text-transform: uppercase" TabIndex="5" Width="130px"></asp:TextBox>
                                                    <asp:ImageButton ID="Btn_Buscar_oficio" runat="server" CausesValidation="false" 
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" 
                                                        TabIndex="6" ToolTip="Buscar" />
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
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Fecha Oficio
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox  runat="server" ID="Txt_Fecha_Oficio"  Width="92%" TabIndex="6"
                                Style="float: left"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Oficio" runat="server" 
                                TargetControlID="Txt_Fecha_Oficio" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Oficio" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px"/>
                            <cc1:CalendarExtender ID="Dtp_Fecha_Oficio" runat="server" TargetControlID="Txt_Fecha_Oficio" 
                                PopupButtonID="Btn_Fecha_Oficio" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Hora oficio</td>
                    <td>
                    <asp:DropDownList ID="Cmb_Hora" runat="server" Width="70px"/>&nbsp;:&nbsp;<asp:DropDownList ID="Cmb_Minutos" runat="server" Width="70px"/>&nbsp;:&nbsp;
                    <asp:DropDownList ID="Cmb_Segundos" runat="server" Width="70px"/>
                    </td>
                    </tr>
                    <tr>
                    <td>*Dependencia</td>
                    <td><asp:TextBox  runat="server" ID="Txt_Dependencia"  Width="98%" TabIndex="6"
                                Style="float: left"/></td>
                    <td>*Departamento de Catastro</td>
                    <td>
                    <asp:DropDownList ID="Cmb_Dep_Catastro" runat="server"  Width="98%" >
                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                    <asp:ListItem Text="Actualización de valor" Value="ACTUALIZACION DE VALOR" />
                    <asp:ListItem Text="Inconformidades" Value="INCONFORMIDADES" />
                    <asp:ListItem Text="Regularización" Value="REGULARIZACION" />
                    <asp:ListItem Text="Autorización" Value="AUTORIZACION" />
                    <asp:ListItem Text="Cartografía" Value="CARTOGRAFIA" />
                    <asp:ListItem Text="Dirección" Value="DIRECCION" />
                    </asp:DropDownList>
                    </tr>
                    <tr>
                        <td>*No Oficio Recepcion</td>
                        <td><asp:TextBox  runat="server" ID="Txt_No_Oficio_Recepcion"  Width="98%" TabIndex="7"
                                Style="float: left"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Asunto</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" TextMode="MultiLine" Rows="3" Width="99%" TabIndex="8"/>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:HiddenField ID="Hdf_No_Oficio" runat="server" />
                    </td>
                    </tr>
                    <tr>
                    <td colspan="4">
                    <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;">
                            <asp:GridView ID="Grid_Oficios" runat="server" AllowSorting="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                HeaderStyle-CssClass="tblHead" style="white-space:normal;" Width="100%" 
                                onpageindexchanging="Grid_Oficios_PageIndexChanging" 
                                onselectedindexchanged="Grid_Oficios_SelectedIndexChanged" PageSize="10" AllowPaging="true">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png" Text="Button">
                                        <HeaderStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_OFICIO" HeaderText="Id" Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_OFICIO_RECEPCION"   HeaderText="Folio de Recepción" >
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_RECEPCION" HeaderText="Fecha del Oficio" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HORA_RECEPCION" HeaderText="Hora de recepción">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DEP_CATASTRO" HeaderText="Dirigido al Departamento">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>