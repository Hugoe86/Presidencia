<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Respuesta_Oficios.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Respuesta_Oficios" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Respuesta a Oficios"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 59%;
        }
        .style2
        {
            width: 423px;
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
                           Respuesta de Oficios
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
                                    <td align="left" class="style2">
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            AlternateText="Salir" onclick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:50%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                
                                                <td align="left" class="style1">
                                                    Búsqueda:
                                                    <asp:DropDownList ID="Cmb_Filtro_Busqueda" runat="server" TabIndex="4" 
                                                       >
                                                        <asp:ListItem Text="FOLIO DE RECEPCION" Value="FOLIO_RECEPCION"></asp:ListItem>
                                                        <asp:ListItem Text="FOLIO DE RESPUESTA" Value="FOLIO_RESPUESTA"></asp:ListItem>
                                                        <asp:ListItem Text="DEPARTAMENTO" Value="DEPARTAMENTO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="4"  ToolTip = "Buscar" Width="120px" style="text-transform:uppercase"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" 
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" 
                                                        TabIndex="6" ToolTip="Buscar" />
                                                </td>
                                                
                                                
                                                
                                                
                                                
                                                <%--<td style="width:55%;">
                                                    Búsqueda:
                                                 <asp:DropDownList ID="Cmb_Filtro_Busqueda" runat="server" TabIndex="4" 
                                                        Width="25%">
                                                        <asp:ListItem Text="FOLIO" Value="FOLIO"></asp:ListItem>
                                                        <asp:ListItem Text="DEPARTAMENTO" Value="DEPARTAMENTO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="4"  ToolTip = "Buscar" Width="180px" style="text-transform:uppercase"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>--%>
                                              <%--  <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="5"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                        AlternateText="Buscar" onclick="Btn_Buscar_Click" />
                                                </td>--%>
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
                            <asp:TextBox  runat="server" ID="Txt_Fecha_Oficio"  Width="98%" TabIndex="6"
                                Style="float: left"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Oficio" runat="server" 
                                TargetControlID="Txt_Fecha_Oficio" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Oficio" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px"/>
                            <cc1:CalendarExtender ID="Dtp_Fecha_Oficio" runat="server" TargetControlID="Txt_Fecha_Oficio" PopupButtonID="Btn_Fecha_Oficio" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Hora oficio</td>
                    <td>
                    <asp:DropDownList ID="Cmb_Hora" runat="server" Width="70px"/>&nbsp;:&nbsp;
                    <asp:DropDownList ID="Cmb_Minutos" runat="server" Width="70px"/>&nbsp;:&nbsp;
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
                   <td>*No Oficio Recepción</td>
                        <td><asp:TextBox  runat="server" ID="Txt_No_Oficio_Recepcion"  Width="98%" TabIndex="7"
                                Style="float: left"/>
                        </td>
                    </tr>
                    <tr>
                    <td>Asunto</td>
                    <td colspan="3">
                    <asp:TextBox ID="Txt_Descripcion" runat="server" TextMode="MultiLine" Rows="3" Width="99%"/>
                    </td>
                    </tr>
                    <tr>
                    <td>*No Oficio Respuesta</td>
                        <td><asp:TextBox  runat="server" ID="Txt_No_Oficio_Respuesta"  Width="98%" TabIndex="7"
                                Style="float: left" MaxLength="30"/>
                        </td>
                    </tr>
                    <tr>
                    <td style="text-align:left;width:20%;">
                        *Fecha Respuesta
                    </td>
                    
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Respuesta_Oficio" runat="server" Style="float: left" 
                                TabIndex="6" Width="98%" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Respuesta" runat="server" 
                                Enabled="True" TargetControlID="Txt_Fecha_Respuesta_Oficio" 
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" />
                            <asp:ImageButton ID="Btn_Fecha_Respuesta" runat="server" Height="18px" 
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;" />
                            <cc1:CalendarExtender ID="Dtp_Fecha_Respuesta" runat="server" 
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Respuesta" 
                                TargetControlID="Txt_Fecha_Respuesta_Oficio">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Hora Respuesta
                        </td>
                    <td>
                        <asp:DropDownList ID="Cmb_Hora_Respuesta" runat="server" Width="70px" />&nbsp;:&nbsp;
                        <asp:DropDownList ID="Cmb_Minutos_Respuesta" runat="server" Width="70px" />&nbsp;:&nbsp;
                        <asp:DropDownList ID="Cmb_Segundos_Respuesta" runat="server" Width="70px" />
                    </td>
                    
                        <tr>
                            <td>
                                <asp:HiddenField ID="Hdf_No_Oficio" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table class="estilo_fuente" width="98%">
                                    <tr>
                                        <td style="text-align:left;">
                                            <asp:GridView ID="Grid_Oficios" runat="server" AllowPaging="true" 
                                                AllowSorting="true" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                HeaderStyle-CssClass="tblHead" 
                                                onpageindexchanging="Grid_Oficios_PageIndexChanging" 
                                                onselectedindexchanged="Grid_Oficios_SelectedIndexChanged" PageSize="10" 
                                                style="white-space:normal;" Width="100%">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png" Text="Button">
                                                        <HeaderStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_OFICIO" HeaderText="Id" Visible="false">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="NO_OFICIO_RECEPCION" HeaderText="Folio de Recepción" >
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="FECHA_RECEPCION" DataFormatString="{0:dd-MMM-yyyy}" 
                                                        HeaderText="Fecha de Recepción">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HORA_RECEPCION" HeaderText="Hora de Recepción">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_OFICIO_RESPUESTA"   HeaderText="Folio de Respuesta" >
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_RESPUESTA" DataFormatString="{0:dd-MMM-yyyy}" 
                                                        HeaderText="Fecha Respuesta">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HORA_RESPUESTA" HeaderText="Hora de Respuesta">
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
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                                        Visible="false">
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
                    
                    </tr>
                    </table>        
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>