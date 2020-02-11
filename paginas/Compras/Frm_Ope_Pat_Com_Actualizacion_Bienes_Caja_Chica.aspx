<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Actualizacion_Bienes_Caja_Chica.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Bienes_Caja_Chica" Title="Caja Chica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Mostrar_Calendar(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Bienes_Caja_Chica" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Bienes Caja Chica</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="4" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" colspan="3">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
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
                        <td align="left">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Modificar" OnClick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick="Btn_Salir_Click"/>
                        </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                onclick="Btn_Avanzada_Click" ToolTip="Avanzada">Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Número de Inventario>"
                                TargetControlID="Txt_Busqueda" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" 
                                                            TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                                            FilterType="Numbers" 
                                                            Enabled="True">   
                            </cc1:FilteredTextBoxExtender>  
                            <asp:ImageButton ID="Btn_Buscar" runat="server" AlternateText="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click"  />
                        </div>
                    </td>                                      
                </table>  
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Bienes_Detalles"  ID="Tab_Bienes_Detalles"  Width="100%">
                        <HeaderTemplate>Datos Generales</HeaderTemplate>
                        <ContentTemplate>
                                <table width="100%">                                      
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Bien_ID" runat="server" />
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Bien_ID" runat="server" Text="Bien ID"  
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Bien_ID" runat="server" Width="39%" Enabled="False" ></asp:TextBox>
                                        </td>
                                    </tr>                                         
                                    <tr>
                                        <td style="text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="99%" Enabled="False" ></asp:TextBox>
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="Dependencia"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia" runat="server" Width="99%" Enabled="False" ></asp:TextBox>
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" 
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="96%" Enabled="False"></asp:TextBox>                                         
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Cantidad" runat="server" Text="Cantidad" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Cantidad" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad" runat="server" TargetControlID="Txt_Cantidad" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>      
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Material" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Material" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Color" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList> 
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Marca" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Modelo" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList> 
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Costo" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Costo" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MEE_Txt_Costo" runat="server" 
                                                TargetControlID="Txt_Costo" Mask="9,999,999.99" MaskType="Number" 
                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="Fecha Adquisición" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="98%" 
                                                MaxLength="20" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" 
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" >
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Estado" runat="server" Text="Estado" 
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estado" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                <asp:ListItem>BUENO</asp:ListItem>
                                                <asp:ListItem>REGULAR</asp:ListItem>
                                                <asp:ListItem>MALO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Motivo_Baja" runat="server" Text="Motivo Baja"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Baja" runat="server" 
                                                                            TargetControlID="Txt_Motivo_Baja" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                           
                                    <tr>
                                        <td style="text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Comentarios_Generales" runat="server" 
                                                Text="Comentarios Generales"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Comentarios_Generales" runat="server" Width="99%" Rows="2" 
                                                TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Generales" runat="server" 
                                                                            TargetControlID="Txt_Comentarios_Generales" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " 
                                                                            Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>            
                                </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Resguardos"  ID="Tab_Resguardos"  Width="100%"  >
                        <HeaderTemplate>Resguardos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados" 
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Cometarios" runat="server" TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cometarios" runat="server" TargetControlID ="Txt_Cometarios" 
                                                WatermarkText="Límite de Caractes 150" WatermarkCssClass="watermarked" Enabled="True"/>                                                        
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Resguardante" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" OnClick="Btn_Agregar_Resguardante_Click"/>
                                            <asp:ImageButton ID="Btn_Quitar_Resguardante" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" OnClick="Btn_Quitar_Resguardante_Click"/>
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                            </center>
                            <asp:GridView ID="Grid_Resguardantes" runat="server" 
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                GridLines="None" AllowPaging="True" Width="98%"
                                OnPageIndexChanging="Grid_Resguardantes_PageIndexChanging"
                                PageSize="2" >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" SortExpression="EMPLEADO_ID">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" SortExpression="NOMBRE_EMPLEADO" />
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Historial_Resguardos"  ID="Tab_Historial_Resguardos"  Width="100%" >
                        <HeaderTemplate>Historial Resguardos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Empleado_Resguardo" runat="server" Text="Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Empleado_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Inicial_Resguardo" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Inicial_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Final_Resguardo" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Final_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Historial_Comentarios_Resguardo" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Comentarios_Resguardo" runat="server" TextMode="MultiLine" Rows="3" Width="98%" Enabled="false"></asp:TextBox>                                                     
                                        </td>
                                    </tr>             
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Historial_Resguardantes" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Historial_Resguardantes_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Historial_Resguardantes_SelectedIndexChanged"
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" 
                                            SortExpression="BIEN_RESGUARDO_ID">
                                            <ItemStyle Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EMPLEADO_ID" 
                                            HeaderText="Empleado ID" SortExpression="EMPLEADO_ID">
                                            <ItemStyle Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" 
                                            SortExpression="NOMBRE_EMPLEADO" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Archivos" runat="server" HeaderText="Tab_Archivos">
                        <HeaderTemplate>Archivos</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">              
                                <tr>
                                    <td style="text-align:left; vertical-align:top; width:15%">
                                        <asp:Label ID="Lbl_Archivo" runat="server" Text="Archivo"  CssClass="estilo_fuente"></asp:Label>
                                        <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                            <div id="Div1" class="progressBackgroundFilter"></div>
                                            <div  class="processMessage" id="div2">
                                                <img alt="" src="../imagenes/paginas/Updating.gif" />
                                            </div>
                                        </asp:Label>  
                                    </td>
                                    <td colspan="3">                      
                                        <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server"  Width="600px" 
                                            ThrobberID="Throbber" ForeColor="White" Font-Bold="True" 
                                            CompleteBackColor="LightBlue" UploadingBackColor="LightGray" 
                                            FailedValidation="False" />
                                    </td>
                                </tr>   
                            </table>
                            <br />
                            <asp:GridView ID="Grid_Archivos" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" AllowPaging="True" Width="98%" PageSize="5" 
                                OnPageIndexChanging="Grid_Archivos_PageIndexChanging" onrowdatabound="Grid_Archivos_RowDataBound"
                                >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="ARCHIVO_BIEN_ID" HeaderText="ARCHIVO_BIEN_ID" SortExpression="ARCHIVO_BIEN_ID">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ARCHIVO" HeaderText="ARCHIVO" SortExpression="ARCHIVO" />
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION" 
                                        NullDisplayText="ARCHIVO NORMAL" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Ver_Archivo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                Width="24px" CssClass="Img_Button" AlternateText="Ver Archivo" 
                                                OnClick="Btn_Ver_Archivo_Click"/>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView> 
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>                       
            </div>
            <br />
            <br />
            <br />
            <br />
    <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/> 
    <cc1:ModalPopupExtender ID="MPE_Busqueda_Bien_Caja_Chica" runat="server" TargetControlID="Btn_Comodin" 
        PopupControlID="Pnl_Busqueda_Bien_Caja_Chica" CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
        DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>           
    </asp:UpdatePanel>  
    <asp:Panel ID="Pnl_Busqueda_Bien_Caja_Chica" runat="server" HorizontalAlign="Center" Width="800px" style="border-style:outset;border-color:Silver;background-repeat:repeat-y;display:none;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda" runat="server"  UpdateMode="Conditional"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda" runat="server" Width="98%" 
                        ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="Tab_Panel_Datos_Generales_Busqueda" Width="100%" Height="400px">
                        <HeaderTemplate>Datos Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Titulo_Busqueda" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Nombre" runat="server" Text="Nombre"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre" runat="server" Width="99%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Nombre" runat="server" TargetControlID="Txt_Busqueda_Nombre"  InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Material" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Busqueda_Material" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Busqueda_Color" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Busqueda_Marca" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Busqueda_Modelo" runat="server" Width="98%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="98%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Fecha_Aquisicion" runat="server" Text="Fecha Adquisición" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Busqueda_Fecha_Aquisicion" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Busqueda_Fecha_Aquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Aquisicion" runat="server" TargetControlID="Txt_Busqueda_Fecha_Aquisicion" PopupButtonID="Btn_Busqueda_Fecha_Aquisicion" Format="dd/MMM/yyyy" OnClientShown="Mostrar_Calendar">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>                
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="Dependencia" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="85%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList> 
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                OnClick="Btn_Buscar_Datos_Click" ToolTip="Buscar Contrarecibos" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" 
                                                onclick="Btn_Limpiar_Filtros_Buscar_Datos_Click" Width="20px"/>                                      
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes" ID="Tab_Panel_Resguardantes_Busqueda" Width="100%" >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Busqueda_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_RFC_Resguardante" runat="server" Text="RFC Reguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC_Resguardante" runat="server" Width="300px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Resguardantes_Dependencias" runat="server" Text="Dependencias" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Resguardantes_Dependencias" runat="server" Width="200px" OnSelectedIndexChanged="Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Nombre_Resguardante" runat="server" Text="Nombre Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Nombre_Resguardante" runat="server" Width="80%" >
                                                <asp:ListItem Text="&lt; SELECCIONE &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>     
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_Buscar_Resguardante_Click"/>                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante" runat="server"  
                                                CausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Click"  />   
                                        </td>
                                    </tr>                                       
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:98%">
                    <center>
                        <caption>
                            <br />
                            <asp:GridView ID="Grid_Listado_Bienes" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                PageSize="5" OnSelectedIndexChanged="Grid_Listado_Bienes_SelectedIndexChanged"
                                Width="98%" OnPageIndexChanging="Grid_Listado_Bienes_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_ID" HeaderText="BIEN_ID" SortExpression="BIEN_ID" />
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE_PRODUCTO" />
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA"  >
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </caption>
                    </center>   
                </div>                                                  
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="95%">
                <tr>
                    <td style="width:100%">
                        <center>
                            <asp:Button ID="Btn_Cerrar" runat="server" TabIndex="202" Text="Cerrar" Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel>    
</asp:Content>

