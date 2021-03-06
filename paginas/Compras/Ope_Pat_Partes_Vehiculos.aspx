﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Ope_Pat_Partes_Vehiculos.aspx.cs" Inherits="paginas_Compras_Ope_Pat_Partes_Vehiculos" Title="Partes de Vehículos" %>

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

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpD_Panel_General" runat="server" >
        <ContentTemplate>
            <asp:UpdateProgress ID="UpP_Panel_General" runat="server" AssociatedUpdatePanelID="UpD_Panel_General" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Partes</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Width="24px" Height="24px" Enabled="false"/>
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
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server"  ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button"  AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"  AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Lnk_Busqueda_Avanzada" runat="server" ForeColor="White" ToolTip="Avanzada" OnClick="Lnk_Busqueda_Avanzada_Click" >Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;
                            </div>
                        </td>                                          
                </table>  
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="99%" ActiveTabIndex="0" CssClass="Tab">
                                <cc1:TabPanel runat="server" HeaderText="Generales" ID="Tab_Partes_Generales" Width="100%">
                                    <HeaderTemplate>Generales</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="99%">
                                            <caption style="color:Blue; font-weight:bolder;">Datos Generales</caption>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:HiddenField ID="Hdf_Producto_ID" runat="server" />
                                                    <asp:HiddenField ID="Hdf_Parte_ID" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Nombre_Parte" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Nombre_Parte" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Parte" runat="server" TargetControlID="Txt_Nombre_Parte" InvalidChars="<,>,&,',!," 
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                                    </cc1:FilteredTextBoxExtender>  
                                                    <asp:ImageButton ID="Btn_Lanzar_Buscar_Producto" runat="server" AlternateText="Buscar Producto Parte" 
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Lanzar_Buscar_Producto_Click"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left;">
                                                    <asp:Label ID="Lbl_Numero_Inventario_Parte" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:TextBox ID="Txt_Numero_Inventario_Parte" runat="server" Width="97%" Enabled="false" ></asp:TextBox>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Numero_Inventario_Parte" runat="server" TargetControlID ="Txt_Numero_Inventario_Parte"  WatermarkText="<-- AUTOMATICO -->" WatermarkCssClass="watermarked" Enabled="True"/>     
                                                                                         
                                                </td>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Marca_Parte" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:DropDownList runat="server" ID="Cmb_Marca_Parte" Width="100%" Enabled="false">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                    </asp:DropDownList>  
                                                </td>  
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Modelo_Parte" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Modelo_Parte" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo_Parte" runat="server" TargetControlID="Txt_Modelo_Parte"
                                                        InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!%&/" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>     
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Material_Parte" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:DropDownList runat="server" ID="Cmb_Material_Parte" Width="100%">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                    </asp:DropDownList>    
                                                </td>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Color_Parte" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:DropDownList runat="server" ID="Cmb_Color_Parte" Width="100%">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                    </asp:DropDownList>    
                                                </td>
                                            </tr>                        
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Costo_Parte" runat="server" Text="Costo [$]" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:TextBox ID="Txt_Costo_Parte" runat="server" Width="50%" ></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="REV_Txt_Costo_Parte" runat="server" ErrorMessage="[No Valido Verificar]" ControlToValidate="Txt_Costo_Parte" ValidationExpression="^\d+(\.\d\d)?$" Font-Size="X-Small" ></asp:RegularExpressionValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo_Parte" runat="server" Enabled="True" FilterType="Numbers, Custom" ValidChars="." TargetControlID="Txt_Costo_Parte">
                                                    </cc1:FilteredTextBoxExtender>   
                                                </td>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Fecha_Adquisicion_Parte" runat="server" Text="Adquisición" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:TextBox ID="Txt_Fecha_Adquisicion_Parte" runat="server" Width="90%" Enabled="False"
                                                        MaxLength="20" ></asp:TextBox>
                                                    <asp:ImageButton ID="Btn_Fecha_Adquisicion_Parte" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion_Parte" runat="server" 
                                                        TargetControlID="Txt_Fecha_Adquisicion_Parte" 
                                                        PopupButtonID="Btn_Fecha_Adquisicion_Parte" Format="dd/MMM/yyyy" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>                           
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Estado_Parte" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:DropDownList ID="Cmb_Estado_Parte" runat="server" Width="100%">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        <asp:ListItem Value="BUENO">BUENO</asp:ListItem>
                                                        <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                                        <asp:ListItem Value="MALO">MALO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Estatus_Parte" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:DropDownList ID="Cmb_Estatus_Parte" runat="server" Width="100%" Enabled="False">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        <asp:ListItem Value="VIGENTE" Selected="True">VIGENTE</asp:ListItem>
                                                        <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                        <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>                                      
                                            <tr>
                                                <td style="text-align:left; vertical-align:top;">
                                                    <asp:Label ID="Lbl_Comentarios_Parte" runat="server" Text="Comentarios"  CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Comentarios_Parte" runat="server" Width="99%"  Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Parte" runat="server"  TargetControlID="Txt_Comentarios_Parte" InvalidChars="<,>,&,',!,"  
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!%&/" Enabled="True">   
                                                    </cc1:FilteredTextBoxExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Parte" runat="server" TargetControlID ="Txt_Comentarios_Parte"  WatermarkText="Límite de Caractes 500" WatermarkCssClass="watermarked" 
                                                        Enabled="True"/>     
                                                </td>
                                            </tr>                                    
                                            <tr>
                                                <td style="text-align:left; vertical-align:top;">
                                                    <asp:Label ID="Lbl_Motivo_Baja_Parte" runat="server" Text="Motivo Baja"  CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Motivo_Baja_Parte" runat="server" Width="99%"  Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Baja_Parte" runat="server"  TargetControlID="Txt_Motivo_Baja_Parte" InvalidChars="<,>,&,',!,"  
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!%&/" Enabled="True">   
                                                    </cc1:FilteredTextBoxExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Motivo_Baja_Parte" runat="server" TargetControlID ="Txt_Motivo_Baja_Parte"  WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked" Enabled="True"/>     
                                                </td>
                                            </tr>  
                                        </table>
                                        <br />
                                        <table width="100%">
                                            <caption style="color:Blue; font-weight:bolder;">Vehículo</caption>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:HiddenField ID="Hdf_Vehiculo_ID" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Nombre_Vehiculo" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Nombre_Vehiculo" runat="server" Width="93%" Enabled="false"></asp:TextBox>
                                                    <asp:ImageButton ID="Btn_Lanzar_Buscar_Vehiculo" runat="server"  AlternateText="Buscar Producto Vehículo" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  onclick="Btn_Lanzar_Buscar_Vehiculo_Click"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Dependencia_Vehiculo" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList runat="server" ID="Cmb_Dependencia_Vehiculo" Width="100%" Enabled="false">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                    </asp:DropDownList>  
                                                </td>  
                                            </tr>
                                            <tr>  
                                                <td style="width:15%; text-align:left; ">
                                                    <asp:Label ID="Lbl_Numero_Inventario_Vehiculo" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                                                </td>
                                                <td style="width:35%">
                                                    <asp:TextBox ID="Txt_Numero_Inventario_Vehiculo" runat="server" Width="90%" Enabled="false"></asp:TextBox>  
                                                </td>
                                            </tr>
                                        </table>                                   
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Resguardos"  ID="Tab_Partes_Resguardos"  Width="100%" >
                                    <HeaderTemplate>Resguardos</HeaderTemplate>
                                    <ContentTemplate>
                                        <br />
                                        <asp:GridView ID="Grid_Resguardos" runat="server" 
                                            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                            GridLines="None" AllowPaging="True" Width="98%"
                                            PageSize="5" 
                                            onpageindexchanging="Grid_Resguardos_PageIndexChanging" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
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
                            </cc1:TabContainer>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="Btn_Comodin_MPE_Busqueda_Productos" runat="server" Text="Btn_Comodin_MPE_Busqueda_Productos" style="display:none;" />
            <asp:Button ID="Btn_Comodin_MPE_Busqueda_Vehiculos" runat="server" Text="Btn_Comodin_MPE_Busqueda_Vehiculos" style="display:none;" />
            <asp:Button ID="Btn_Comodin_MPE_Busqueda_Partes" runat="server" Text="Btn_Comodin_MPE_Busqueda_Partes" style="display:none;" />
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Productos" runat="server" TargetControlID="Btn_Comodin_MPE_Busqueda_Productos" PopupControlID="Pnl_Busqueda_Productos" DropShadow="true"
                BackgroundCssClass="progressBackgroundFilter" CancelControlID="Btn_MPE_Productos_Cancelar" PopupDragHandleControlID="Pnl_Interno_Busqueda_Productos">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Vehiculo" runat="server" TargetControlID="Btn_Comodin_MPE_Busqueda_Vehiculos" PopupControlID="Pnl_Busqueda_Vehiculos" CancelControlID="Btn_MPE_Vehiculos_Cerrar" PopupDragHandleControlID="Pnl_Busqueda_Vehiculos_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Partes" runat="server" TargetControlID="Btn_Comodin_MPE_Busqueda_Partes" PopupControlID="Pnl_Busqueda_Partes" CancelControlID="Btn_MPE_Partes_Cerrar" PopupDragHandleControlID="Pnl_Busqueda_Partes_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Grid_Listado_Productos" EventName="SelectedIndexChanged"  />
            <asp:AsyncPostBackTrigger ControlID="Grid_Listado_Vehiculos" EventName="SelectedIndexChanged"  />
            <asp:AsyncPostBackTrigger ControlID="Grid_Listado_Partes" EventName="SelectedIndexChanged"  />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Busqueda_Productos" runat="server" CssClass="drag"  HorizontalAlign="Center"  style="display:none;border-style:outset;border-color:Silver;width:760px;" >                         
        <asp:Panel ID="Pnl_Interno_Busqueda_Productos" runat="server" CssClass="estilo_fuente" style="cursor:move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
            <table class="estilo_fuente">
                <tr>
                    <td style="color:Black;font-size:12;font-weight:bold;">
                       <asp:Image ID="Img_Barra_Informacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                         B&uacute;squeda de Productos
                    </td>
                </tr>
            </table>            
         </asp:Panel>
           <center>            
            <asp:UpdatePanel ID="UpPnl_Busqueda_Productos" runat="server" UpdateMode="Conditional"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="Upg_Busqueda_Productos" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda_Productos" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                 
                </asp:UpdateProgress>
                <table width="98%"  class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                          <div id="Div_MPE_Productos_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_MPE_Productos_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Enabled="false" Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_MPE_Productos_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_MPE_Productos_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div> 
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <asp:Label ID="Lbl_Clave_Producto" runat="server" Text="Clave"></asp:Label>
                        </td>
                        <td width="35%">
                            <asp:TextBox ID="Txt_Clave_Producto" runat="server" Width="98%"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:Label ID="Lbl_Nombre_Producto" runat="server" Text="Nombre"></asp:Label>
                        </td>
                        <td width="35%">
                            <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <asp:Label ID="Lbl_Marca_Producto" runat="server" Text="Marca"></asp:Label>
                        </td>
                        <td width="35%">
                            <asp:DropDownList ID="Cmb_Marca_Producto" runat="server" Width="99%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td width="15%">
                            <asp:Label ID="Lbl_Modelo_Producto" runat="server" Text="Modelo"></asp:Label>
                        </td>
                        <td width="35%">
                            <asp:DropDownList ID="Cmb_Modelo_Producto" runat="server" Width="99%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:right;">
                            <asp:ImageButton ID="Btn_MPE_Productos_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" AlternateText="Buscar" onclick="Btn_MPE_Productos_Buscar_Click" />
                            <asp:ImageButton ID="Btn_MPE_Productos_Limpiar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" AlternateText="Limpiar Filtros" Width="24px" onclick="Btn_MPE_Productos_Limpiar_Click" />
                           <asp:ImageButton ID="Btn_MPE_Productos_Cancelar" ImageUrl="~/paginas/imagenes/paginas/delete.png" runat="server" AlternateText="Cerrar"/>&nbsp;&nbsp;   
                        </td>
                    </tr>
                </table>
                <div style="width:98%">
                    <center>
                        <caption>
                            <br />
                            <asp:GridView ID="Grid_Listado_Productos" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                PageSize="5" Width="98%" 
                                onpageindexchanging="Grid_Listado_Productos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Listado_Productos_SelectedIndexChanged" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="PRODUCTO_ID" HeaderText="PRODUCTO_ID" SortExpression="PRODUCTO_ID" />
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE" />
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" />
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" />
                                </Columns>                                
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </caption>
                        <br />
                        <br />
                    </center>   
                </div>                                                  
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </asp:Panel> 
    
    <asp:Panel ID="Pnl_Busqueda_Vehiculos" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Vehiculos_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda" runat="server"  UpdateMode="Conditional"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="Upg_Busqueda_Vehiculos" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="Tab_Panel_Datos_Generales_Busqueda" Width="100%" Height="400px">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Titulo_Busqueda_Vehiculos" runat="server" Text="Búsqueda de Vehículo"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" Width="200px" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Economico" runat="server" Text="Número Económico"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Economico" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Modelo_Busqueda" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo_Busqueda" runat="server" TargetControlID="Txt_Modelo_Busqueda"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                                     
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Marca" runat="server"  Width="200px">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Text="Tipo Vehículo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Width="200px">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Combustible" runat="server" Text="Tipo Combustible" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Combustible" runat="server"  Width="200px">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Text="Año Fabricación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Width="200px" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Color" runat="server" Width="200px">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Zonas" runat="server" Text="Zonas" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Zonas" runat="server" Width="200px">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Estatus" runat="server" Width="200px" Enabled="false" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            <asp:ListItem Value="VIGENTE" Selected="True">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Dependencias" runat="server" Text="Unidad Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Dependencias" runat="server" Width="490px">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                            <asp:ImageButton ID="Btn_MPE_Vehiculos_Buscar_Datos" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                ToolTip="Buscar Contrarecibos" OnClick="Btn_MPE_Vehiculos_Buscar_Datos_Click" />
                                            <asp:ImageButton ID="Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Datos" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" OnClick="Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Datos_Click" />                                      
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
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_RFC_Resguardante" runat="server" Text="RFC Reguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_RFC_Resguardante" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Text="Unidad Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Width="300px" OnSelectedIndexChanged="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Text="Nombre Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Width="80%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="Btn_MPE_Vehiculos_Buscar_Resguardante" runat="server"  CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_MPE_Vehiculos_Buscar_Resguardante_Click" />                                      
                                            <asp:ImageButton ID="Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Resguardante" runat="server" ausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" OnClick="Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Resguardante_Click" />   
                                        </td>
                                    </tr>                                       
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:97%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Vehiculos" runat="server" AllowPaging="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                OnPageIndexChanging="Grid_Listado_Vehiculos_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Vehiculos_SelectedIndexChanged"
                                Width="98%" PageSize="100" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="VEHICULO_ID" HeaderText="VEHICULO_ID" SortExpression="VEHICULO_ID" />
                                    <asp:BoundField DataField="VEHICULO" HeaderText="Vehículo" SortExpression="VEHICULO" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO">
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO">
                                        <ItemStyle Width="70px" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" />
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
                            <asp:Button ID="Btn_MPE_Vehiculos_Cerrar" runat="server" TabIndex="202" Text="Cerrar" Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel>     
    
    <asp:Panel ID="Pnl_Busqueda_Partes" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Partes_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda_Partes" runat="server"  UpdateMode="Conditional"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="Upg_Busqueda_Partes" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda_Partes" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                <cc1:TabContainer ID="Tab_Contenedor_Busqueda_Partes" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="TabPanel1" Width="100%" Height="400px">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Busqueda_Partes" runat="server" Text="Búsqueda de Parte"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_No_Inventario_Parte" runat="server" Text="Inv. (Parte)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_MPE_Busqueda_Partes_No_Inventario_Parte" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Busqueda_Partes_No_Inventario_Parte" runat="server" TargetControlID="Txt_MPE_Busqueda_Partes_No_Inventario_Parte" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_No_Inventario_Vehiculo" runat="server" Text="Inv. (Vehículo)"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo" runat="server" TargetControlID="Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Marca" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_MPE_Busqueda_Partes_Modelo" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Busqueda_Partes_Modelo" runat="server" TargetControlID="Txt_MPE_Busqueda_Partes_Modelo"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                                     
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Material" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Material" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Color" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Fecha_Adquisicion" runat="server" Text="Adquisición" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_MPE_Busqueda_Partes_Fecha_Adquisicion" runat="server" Width="85%" Enabled="False"
                                                MaxLength="20" ></asp:TextBox>
                                            <asp:ImageButton ID="Btn_MPE_Busqueda_Partes_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_MPE_Busqueda_Partes_Fecha_Adquisicion" runat="server" TargetControlID="Txt_MPE_Busqueda_Partes_Fecha_Adquisicion" OnClientShown="Mostrar_Calendar"
                                                PopupButtonID="Btn_MPE_Busqueda_Partes_Fecha_Adquisicion" Format="dd/MMM/yyyy" >
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Dependencia" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Dependencia" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                         
                                        </td>      
                                    </tr>    
                                    <tr>       
                                        <td style="text-align:right;" colspan="4">                                    
                                            <asp:ImageButton ID="Btn_MPE_Partes_Buscar_Datos" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                ToolTip="Buscar Contrarecibos" OnClick="Btn_MPE_Partes_Buscar_Datos_Click" />
                                            <asp:ImageButton ID="Btn_MPE_Partes_Limpiar_Filtros_Buscar_Datos" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" OnClick="Btn_MPE_Partes_Limpiar_Filtros_Buscar_Datos_Click" />                                      
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes" ID="TabPanel2" Width="100%" >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Busqueda_Parte_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_RFC_Resguardante" runat="server" Text="RFC" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_MPE_Busqueda_Partes_RFC_Resguardante" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_MPE_Busqueda_Partes_RFC_Resguardante" runat="server" TargetControlID="Txt_MPE_Busqueda_Partes_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Dependencia_Resguardante" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3"> 
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_MPE_Busqueda_Partes_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">   
                                            <asp:DropDownList ID="Cmb_MPE_Busqueda_Partes_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                              
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="text-align:right;" colspan="4">   
                                            <asp:ImageButton ID="Btn_MPE_Partes_Buscar_Resguardante" runat="server"  CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_MPE_Partes_Buscar_Resguardante_Click" />                                      
                                            <asp:ImageButton ID="Btn_MPE_Partes_Limpiar_Filtros_Buscar_Resguardante" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" OnClick="Btn_MPE_Partes_Limpiar_Filtros_Buscar_Resguardante_Click" /> 
                                            &nbsp;&nbsp;&nbsp;  
                                        </td>
                                    </tr>                                       
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:97%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Partes" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                PageSize="100" OnPageIndexChanging="Grid_Listado_Partes_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Partes_SelectedIndexChanged"
                                Width="98%" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="PARTE_ID" HeaderText="Parte ID" SortExpression="PARTE_ID" />
                                    <asp:BoundField DataField="PARTE_CLAVE" HeaderText="Clave" SortExpression="PARTE_CLAVE"  >
                                        <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PARTE_NOMBRE" HeaderText="Nombre" SortExpression="PARTE_NOMBRE"  >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA_NOMBRE" HeaderText="Marca" SortExpression="MARCA_NOMBRE" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO_NOMBRE" HeaderText="Modelo" SortExpression="MODELO_NOMBRE" >
                                        <ItemStyle Width="100px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                    </center>   
                </div>                                                  
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="95%">
                <tr>
                    <td style="width:100%">
                        <center>
                            <asp:Button ID="Btn_MPE_Partes_Cerrar" runat="server" TabIndex="202" Text="Cerrar" Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel> 
    
</asp:Content>

