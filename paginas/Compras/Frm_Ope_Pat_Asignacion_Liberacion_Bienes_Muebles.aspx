﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Asignacion_Liberacion_Bienes_Muebles.aspx.cs" Inherits="paginas_Control_Patrimonial_Frm_Ope_Pat_Asignacion_Liberacion_Bienes_Muebles" Title="Bienes Sin Inventario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    
    <asp:ScriptManager ID="ScriptManager_Control_Bienes_Secundarios" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True">
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
                        <td class="label_titulo">Control de Bienes Secundarios [Bienes Muebles]</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" Enabled="false" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Width="24px" Height="24px" />
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
                <br />
                <table width="98%" border="0" cellspacing="0">
                    <tr class="barra_busqueda">       
                        <td style="width:30%; text-align:left;">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" 
                                CssClass="Img_Button" ToolTip="Modificar" AlternateText="Modificar" 
                                OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" ToolTip="Salir" AlternateText="Salir" 
                                OnClick="Btn_Salir_Click" />
                            &nbsp;&nbsp;
                        </td>
                        <td style="width:70%; text-align:right;">
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White" ToolTip="Busqueda Avanzada" OnClick="Lnk_Busqueda_Avanzada_Click">Busqueda</asp:LinkButton>
                                    &nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Busqueda_Anterior" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Anterior" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="< - Inventario Anterior - >"
                                    TargetControlID="Txt_Busqueda_Anterior" />
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="< - Inventario SIAS - >"
                                    TargetControlID="Txt_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  ToolTip="Buscar" onclick="Btn_Buscar_Click" />
                            </div>
                        </td>   
                    </tr> 
                </table>
                <table width="98%" class="estilo_fuente">                                      
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Bien_Mueble_ID" runat="server" />
                                <asp:HiddenField ID="Hdf_Bien_Parent_ID" runat="server" />
                                <asp:HiddenField ID="Hdf_Lanza_Busqueda" runat="server" />
                            </td>
                        </tr>                                      
                        <tr>
                            <td style="text-align:left; vertical-align:top; width:13%">
                                <asp:Label ID="Lbl_Nombre_Producto" runat="server" Text="Nombre Producto" ></asp:Label>
                            </td>
                            <td style="width:37%;">
                                <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="97%" Enabled="False" ></asp:TextBox>
                            </td>
                            <td style="text-align:left; vertical-align:top; width:13%">
                                <asp:Label ID="Lbl_Resguardo_Recibo" runat="server" Text="Operación"></asp:Label>
                            </td>
                            <td style=" width:37%;">
                                <asp:TextBox ID="Txt_Resguardo_Recibo" runat="server" Width="97%" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>                                 
                        <tr>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Invenario_Anterior" runat="server" Text="No. Inventario"></asp:Label>
                            </td>
                            <td style="width:37%">
                                <asp:TextBox ID="Txt_Invenario_Anterior" runat="server" Width="97%" Enabled="False"></asp:TextBox>                                         
                            </td>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="Inventario Nuevo"></asp:Label>
                            </td>
                            <td style="width:37%">
                                <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="97%" Enabled="False"></asp:TextBox>                                         
                            </td>
                        </tr>                    
                        <tr>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Dependencia" runat="server" Text="U. Responsable"></asp:Label>
                            </td>
                            <td colspan="3">
                                 <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" Enabled="False" >
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>                                          
                        <tr>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Zona" runat="server" Text="Zona"></asp:Label>
                            </td>
                             <td colspan="3">
                                 <asp:DropDownList ID="Cmb_Zonas" runat="server" Width="100%" Enabled="False" >
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>                    
                        <tr>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Marca" runat="server" Text="Marca"></asp:Label>
                            </td>
                            <td style="width:37%">
                                 <asp:DropDownList ID="Cmb_Marca" runat="server" Width="99%" Enabled="False" >
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align:left;width:13%;">
                                <asp:Label ID="Lbl_Numero_Serie" runat="server" Text="No. Serie"></asp:Label>
                            </td>
                            <td style="width:37%">
                                <asp:TextBox ID="Txt_Numero_Serie" runat="server" Width="97%" MaxLength="49" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>                                     
                        <tr>
                            <td  width="13%">
                                <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo"></asp:Label>
                            </td>
                            <td colspan="3">
                                 <asp:TextBox ID="Txt_Modelo" runat="server"  Width="99%" MaxLength="149" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>                                  
                        <tr>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Material" runat="server" Text="Material"></asp:Label>
                            </td>
                            <td style="width:37%">
                                <asp:DropDownList ID="Cmb_Materiales" runat="server" Width="99%" Enabled="False" >
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align:left; ">
                                <asp:Label ID="Lbl_Color" runat="server" Text="Color"></asp:Label>
                            </td>
                            <td style="width:37%">
                                <asp:DropDownList ID="Cmb_Colores" runat="server" Width="100%" Enabled="False" >
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>         
                        <tr>
                            <td style="text-align:left; vertical-align:top; width:13%">
                                <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" ></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" Rows="4" TextMode="MultiLine" Font-Size="X-Small" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>  
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr>
                        <tr>
                            <td style="width:13%; text-align:left;">
                                <asp:Label ID="Lbl_Tipo_Parent" runat="server" Text="Tipo Parent" ></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Tipo_Parent" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_Parent_SelectedIndexChanged">
                                    <asp:ListItem Value="">NINGUNA</asp:ListItem>
                                    <asp:ListItem Value="BIEN_MUEBLE">BIEN MUEBLE</asp:ListItem>
                                    <asp:ListItem Value="VEHICULO">VEHíCULO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr>
                    </table>
                <br />
                <div id="Div_Vehiculo_Parent" runat="server" style="width:98%;">
                    <table width="98%">
                        <caption class="label_titulo">Asignada al Vehículo</caption>
                        <tr align="right" class="barra_delgada">
                            <td align="center" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Vehiculo_Nombre" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Vehiculo_Nombre" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar_Vehiculo" runat="server" 
                                    AlternateText="Buscar Vehículo" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Buscar_Vehiculo_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Vehiculo_Dependencia" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Vehiculo_Dependencia" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Vehiculo_No_Inventario" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_No_Inventario" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left;">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Vehiculo_Numero_Serie" runat="server" Text="No. Serie" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Numero_Serie" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Vehiculo_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Marca" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Vehiculo_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Modelo" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Vehiculo_Tipo" runat="server" Text="Tipo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Tipo" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left;">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Vehiculo_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Color" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Vehiculo_Numero_Economico" runat="server" Text="No. Economico" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Numero_Economico" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left;">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Vehiculo_Placas" runat="server" Text="Placas" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Vehiculo_Placas" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Bien_Mueble_Parent" runat="server" style="width:98%;">
                    <table width="98%">
                        <caption class="label_titulo">Asignada al Bien Mueble</caption>
                        <tr align="right" class="barra_delgada">
                            <td align="center" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Bien_Mueble_Nombre" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Bien_Mueble_Nombre" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar_Bien_Mueble" runat="server" 
                                    AlternateText="Buscar Bien Mueble" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Buscar_Bien_Mueble_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Bien_Mueble_Dependencia" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Bien_Mueble_Dependencia" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Bien_Mueble_Inventario_Anterior" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Inventario_Anterior" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Bien_Mueble_Inventario_SIAS" runat="server" Text="Inv. SIAS" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Inventario_SIAS" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Bien_Mueble_Numero_Serie" runat="server" Text="No. Serie" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Numero_Serie" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left;">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Bien_Mueble_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Marca" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Bien_Mueble_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Bien_Mueble_Modelo" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Bien_Mueble_Material" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Material" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left;">
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Bien_Mueble_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Bien_Mueble_Color" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
             <div id="Div_Resguardos" runat="server" style="width:98%;">
                <table width="98%">
                    <caption class="label_titulo">Resguardos</caption>
                    <tr align="right" class="barra_delgada">
                        <td align="center">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width:100%; height:150px; overflow:auto; background-color:White;">
                                <center>
                                    <asp:GridView ID="Grid_Listado_Resguardantes" runat="server"
                                        AutoGenerateColumns="False" CssClass="GridView_1" PageSize="100"
                                        GridLines="Both" Width="98%">
                                        <RowStyle CssClass="GridItem" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID" SortExpression="EMPLEADO_ID" >
                                                <ItemStyle Width="3px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO" >
                                                <ItemStyle Width="110px" HorizontalAlign="Center" />
                                            </asp:BoundField>   
                                            <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre del Empleado" SortExpression="NOMBRE_EMPLEADO" >
                                                <ItemStyle  HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                    </asp:GridView>
                                </center>   
                            </div>                                            
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Bien_Mueble" runat="server" UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Bien_Mueble" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Bien_Mueble" runat="server" TargetControlID="Btn_Comodin_Busqueda_Bien_Mueble" 
                BackgroundCssClass="progressBackgroundFilter" CancelControlID="Btn_Cerrar_Busqueda_Bien_Mueble" 
                PopupControlID="Pnl_Busqueda_Bien_Mueble" PopupDragHandleControlID="Pnl_Busqueda_Bien_Mueble_Interno"
                DropShadow="True" />
        </ContentTemplate>           
    </asp:UpdatePanel>  
    
    <asp:Panel ID="Pnl_Busqueda_Bien_Mueble" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                <%-- runat="server" CssClass="drag" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-color:White;">                         --%>
        <center>
            <asp:Panel ID="Pnl_Busqueda_Bien_Mueble_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table class="estilo_fuente" width="100%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;width:90%; border-color:Black;">
                           <asp:Image ID="Img_Encabezado_Busqueda_Bienes_Muebles" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             Busqueda de Bienes Muebles
                        </td>
                        <td align="right">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Bien_Mueble" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                        </td>
                    </tr>
                </table>   
            </asp:Panel>
            <asp:UpdatePanel ID="UpPnl_Busqueda_Bien_Mueble" runat="server"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPrg_Busqueda_Bien_Mueble" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda_Bien_Mueble" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda_Bien_Mueble" runat="server" ActiveTabIndex="0" CssClass="Tab" Width="100%">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="Tab_Panel_Datos_Generales_Busqueda_Bien_Mueble" Width="100%" Height="400px" BackColor="White">
                        <HeaderTemplate>Datos Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>                                           
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Numero_Inventario" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Numero_Inventario" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Numero_Inventario" runat="server" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Numero_Inventario" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Numero_Inventario_SIAS" runat="server" Text="No. Inventario SIAS" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS" runat="server" Width="95%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS" runat="server" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS" InvalidChars="<,>,&,',!,"  
                                                FilterType="Numbers"  Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                    </tr>                                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Producto" runat="server" Text="Nombre Producto" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Producto" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Producto" runat="server" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Producto" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                    </tr>                                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Bien_Mueble_Dependencias" runat="server" Width="99%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Modelo" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Modelo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Modelo" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# " Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                    </tr>                                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Bien_Mueble_Marca" runat="server" Width="98%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Factura" runat="server" Text="No. Factura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Factura" runat="server" Width="95%"></asp:TextBox>    
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Factura" runat="server" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Factura" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                    
                                        </td>
                                    </tr>                   
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Bien_Mueble_Estatus" runat="server" Width="98%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Numero_Serie" runat="server" Text="No. Serie" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_Numero_Serie" runat="server" Width="95%"></asp:TextBox>      
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_Numero_Serie" runat="server" 
                                                TargetControlID="Txt_Busqueda_Bien_Mueble_Numero_Serie" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                   
                                                                              
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Datos_Bien_Mueble" runat="server" 
                                                OnClick="Btn_Buscar_Datos_Bien_Mueble_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False"  ToolTip="Buscar" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos_Bien_Mueble" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Bien_Mueble_Click" 
                                                ToolTip="Limpiar Filtros"  Width="20px"/>  
                                            &nbsp;&nbsp;  
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>                     
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Busqueda_Bien_Mueble_Reguardantes" ID="Tab_Panel_Busqueda_Bien_Mueble_Resguardantes_Busqueda" Width="100%" >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_RFC_Resguardante" runat="server" Text="RFC" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_RFC_Resguardante" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_Bien_Mueble_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_No_Empleado" runat="server" Text="No. Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Bien_Mueble_No_Empleado" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bien_Mueble_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_Bien_Mueble_No_Empleado" InvalidChars="<,>,&,',!,"  FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias_SelectedIndexChanged">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Bien_Mueble_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr>    
                                     <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante_Bien_Mueble" runat="server" 
                                                OnClick="Btn_Buscar_Resguardante_Bien_Mueble_Click"
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante_Bien_Mueble" runat="server"  
                                                CausesValidation="False" OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Bien_Mueble_Click" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros"  />   
                                            &nbsp;&nbsp;                                 
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:99%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Bienes_Bien_Mueble" runat="server"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"  
                                OnPageIndexChanging="Grid_Listado_Bienes_Bien_Mueble_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Bienes_Bien_Mueble_SelectedIndexChanged"
                                AllowPaging="true" PageSize="100">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_MUEBLE_ID" HeaderText="BIEN_MUEBLE_ID" SortExpression="BIEN_MUEBLE_ID" />
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="Inv. Anterior" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO" HeaderText="Inv. SIAS" SortExpression="NO_INVENTARIO" >
                                        <ItemStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre" SortExpression="NOMBRE_PRODUCTO"  >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLOR" HeaderText="Color" SortExpression="COLOR"  >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
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
        </center>
    </asp:Panel>    
    
    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Vehiculo" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Vehiculo" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Vehiculo" runat="server" TargetControlID="Btn_Comodin_Busqueda_Vehiculo" 
                PopupControlID="Pnl_Busqueda_Vehiculo" CancelControlID="Btn_Cerrar_Busqueda_Vehiculo" PopupDragHandleControlID="Pnl_Busqueda_Vehiculo_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Busqueda_Vehiculo" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >
        <center>
            <asp:Panel ID="Pnl_Busqueda_Vehiculo_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table class="estilo_fuente" width="100%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                           <asp:Image ID="Img_Encabezado_Busqueda_Vehiculos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             Busqueda de Vehículos
                        </td>
                        <td align="right">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Vehiculo" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                        </td>
                    </tr>
                </table>   
            </asp:Panel>
            <asp:UpdatePanel ID="UpPnl_Busqueda_Vehiculo" runat="server"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPrg_Busqueda_Vehiculo" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda_Vehiculo" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda_Vehiculo" runat="server" Width="100%" ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales_Busqueda_Vehiculo" ID="Tab_Panel_Datos_Generales_Busqueda_Vehiculo" Width="100%" Height="400px" BackColor="White">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="4">&nbsp;</td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Economico" runat="server" Text="Número Económico"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Economico" InvalidChars="<,>,&,',!,"  FilterType="Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Modelo" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Modelo" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Modelo"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Marca" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Text="Tipo Vehículo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Combustible" runat="server" Text="Combustible" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Combustible" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Text="Año Fabricación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Width="97%" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Color" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Zonas" runat="server" Text="Zonas" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Zonas" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Dependencias" runat="server" Width="85%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                            <asp:ImageButton ID="Btn_Buscar_Datos_Vehiculo" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                OnClick="Btn_Buscar_Datos_Vehiculo_Click"
                                                ToolTip="Buscar Contrarecibos"/>
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo" runat="server" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click"
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" />                                      
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes_Busqueda_Vehiculo" ID="Tab_Panel_Resguardantes_Busqueda_Vehiculo" Width="100%" BackColor="White">
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">&nbsp;</td>
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
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_No_Empleado" runat="server" Text="No. Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_No_Empleado" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_No_Empleado" InvalidChars="<,>,&,',!,"  FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged" >
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td colspan="4" style="text-align:right">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante_Vehiculo" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                OnClick="Btn_Buscar_Resguardante_Vehiculo_Click"
                                                ToolTip="Buscar Listados"/>                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo" runat="server"  
                                                CausesValidation="False"  OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" />   
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>                                      
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:99%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Busqueda_Vehiculo" runat="server" AllowPaging="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                OnPageIndexChanging="Grid_Listado_Busqueda_Vehiculo_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged"
                                Width="98%" PageSize="100" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="VEHICULO_ID" HeaderText="VEHICULO_ID" SortExpression="VEHICULO_ID" />
                                    <asp:BoundField DataField="NUMERO_INVENTARIO" HeaderText="No. Inven." SortExpression="NUMERO_INVENTARIO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NUMERO_ECONOMICO" HeaderText="No. Eco." SortExpression="NUMERO_ECONOMICO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
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
                                        <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
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
        </center>
    </asp:Panel>  
      
    <asp:UpdatePanel ID="UpPnl_Aux_Multiples_Resultados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Multiples_Resultados" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="Mpe_Multiples_Resultados" runat="server" 
                TargetControlID="Btn_Comodin_Mpe_Multiples_Resultados" PopupControlID="Pnl_Multiples_Resultados" 
                CancelControlID="Btn_Cerrar_Mpe_Multiples_Resultados" PopupDragHandleControlID="Pnl_Multiples_Resultados_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        </ContentTemplate>
    </asp:UpdatePanel>   
    
    <asp:Panel ID="Pnl_Multiples_Resultados" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;">                
    <asp:Panel ID="Pnl_Multiples_Resultados_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                    Seleccione el Bien
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Multiples_Resultados" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Multiples_Resultados" runat="server"> 
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpPgr_Multiples_Resultados" runat="server" AssociatedUpdatePanelID="UpPnl_Multiples_Resultados" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                            <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                        </ProgressTemplate>                     
                    </asp:UpdateProgress>
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <asp:Panel ID="Pnl_Grid_Resultados_Multiples" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                            Width="100%" BorderColor="#3366FF" Height="370px"> 
                            <asp:GridView ID="Grid_Resultados_Multiples" runat="server" HeaderStyle-CssClass="tblHead"
                                EmptyDataText="No se encontrarón Registros"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"  AllowPaging="true" PageSize="100"
                                OnSelectedIndexChanged="Grid_Resultados_Multiples_SelectedIndexChanged" 
                                OnPageIndexChanging="Grid_Resultados_Multiples_PageIndexChanging"
                                Width="100%">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_MUEBLE_ID" HeaderText="BIEN_MUEBLE_ID" SortExpression="BIEN_MUEBLE_ID" />
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="Inv. Anterior" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO" HeaderText="Inv. SIAS" SortExpression="NO_INVENTARIO" >
                                        <ItemStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre" SortExpression="NOMBRE_PRODUCTO"  >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLOR" HeaderText="Color" SortExpression="COLOR"  >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                       </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel>  
    
</asp:Content>

