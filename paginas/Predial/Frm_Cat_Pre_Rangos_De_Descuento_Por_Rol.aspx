<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Rangos_De_Descuento_Por_Rol.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Rangos_De_Descuento_Por_Rol" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            return false;
        }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel_Padron_Predios" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel_Padron_Predios" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
                <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td class="label_titulo" colspan="2">
                                        Catálogo de Descuentos Por Empleado</td>
                                </tr>
                                <tr>
                                    <div id="Div_Contenedor_error" runat="server">
                                    <td colspan="2">
                                        <asp:Image ID="Img_Error" runat="server" 
                                            ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                        <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        <br />
                                        <asp:Label ID="Lbl_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                    </td>
                                    </div>
                                </tr>
                                <tr class="barra_busqueda">
                                    <td style="width:50%">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" onclick="Btn_Nuevo_Click1"/>
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" onclick="Btn_Modificar_Click1"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        CssClass="Img_Button" onclick="Btn_Salir_Click1"/>
                                    </td></td><td align="right" style="width:50%">
                                    <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" 
                                            CausesValidation="false" Height="24px" 
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                            OnClientClick="javascript:return Abrir_Modal_Popup();" TabIndex="23" 
                                            ToolTip="Busqueda Avanzada" Width="24px" />
                                            <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                                            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                            CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>
                                            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                                            <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
                                            
                                        Búsqueda:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                        ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            TabIndex="2" onclick="Btn_Buscar_Rango_De_Descuento_Click"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                    </td>
                                </tr>
                            </table>
                    <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                        <tr>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                        </tr>
                        <tr>
                        <td style="width:18%">
                                *Empleado</td>
                            <td style="width:32%">
                               <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="92%" Text="" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:18%">
                                *Estatus</td>
                            <td style="width:32%">
                                <asp:DropDownList ID="Cmb_Estatus" Width="94%" runat="server">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                *Porcentaje máximo</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Porcentaje_Maximo" runat="server" Width="92%" Text=""></asp:TextBox>
                            </td>
                            <td style="width:18%">*Tipo</td>
                            <td style="width:32%">
                                <asp:DropDownList ID="Cmb_Tipo" Width="94%" runat="server">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="PREDIAL" Value="PREDIAL" />
                                <asp:ListItem Text="TRASLADO" Value="TRASLADO" />
                                <asp:ListItem Text="MULTAS" Value="MULTAS" />
                                <asp:ListItem Text="FRACCIONAMIENTO" Value="FRACC" />
                                <asp:ListItem Text="DERECHOS DE SUPERVISIÓN" Value="DER.SUP." />
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                Comentarios</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" Width="97%" Height="60px" Style="text-transform: uppercase"
                                    TextMode="MultiLine" ></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                <asp:TextBox ID="Txt_Id_Empleado" runat="server" Width="92%" Text="" Visible="false"></asp:TextBox></td>
                            <td style="width:32%">
                            <asp:TextBox ID="Txt_id" runat="server" Width="92%" Text="" Visible="false"></asp:TextBox>
                                </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Rango_De_Descuentos_Por_Rol" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                                    PageSize="5" Style="white-space:normal" Width="96%" 
                                    onpageindexchanging="Grid_Rangos_De_Descuentos_Por_Rol_PageIndexChanging" 
                                    
                                    onselectedindexchanged="Grid_Rangos_De_Descuentos_Por_Rol_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="RANGOS_DE_DESCUENTO_POR_ROL_ID" HeaderText="Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Empleado">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PORCENTAJE" HeaderText="Descuento máximo (%)">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                                        <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                                                <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                    </table>
                    
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
    
    
    
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Click"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">
                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           No Empleado 
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" style="text-transform:uppercase"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">                                            
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">                                         
                                        </td>                                         
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" style="text-transform:uppercase"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                     
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" style="text-transform:uppercase"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="nombre(s), apellido paterno y apellido materno" 
                                                WatermarkCssClass="watermarked"/>                                                                                               
                                        </td>                                         
                                    </tr>
                                    <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                                    PageSize="5" Style="white-space:normal" Width="96%"
                                    onpageindexchanging="Grid_Empleados_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Empleados_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                 <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="200px"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>                                                                   
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>                                                      
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>
    
    
    
    
    
    
    
    
    
    
    
    
</asp:Content>