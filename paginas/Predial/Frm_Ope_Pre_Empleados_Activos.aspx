<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Empleados_Activos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Empleados_Activos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
         
        <div id="Div_Busqueda" style="width:100%; height:100%; background-color:#ffffff">
            <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">                
                    <tr>
                        <td colspan="2" class="label_titulo">
                            Empleados Activos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                            <br />
                            <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                        </td> 
                    </tr>
                    
                    <tr class="barra_busqueda">
                        <td style="width:50%">                            
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button" onclick="Btn_Modificar_Click"/>                            
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                        </td>
                        <td align="right" style="width:50%">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                            ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                TabIndex="2"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                        </td>                        
                    </tr>                    
                </table>
                <br />
        </div>
        
        <div id="Div_Datos_Generales" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
            <table id="Tbl_Datos_Generales" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                <tr>
                    <td style="width:18%">
                    Número de Empleado
                    </td>
                    <td style="width:32%">
                        <asp:TextBox ID="Txt_No_Empleado" Width="90%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%">
                    </td>
                    <td style="width:32%">
                    </td>
                </tr>
                <tr>
                    <td style="width:18%">
                        Nombre de Empleado</td>
                    <td style="width:32%" colspan="3">
                    <asp:TextBox ID="Txt_Nombre_Empleado" Width="97%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%">
                    </td>
                    <td style="width:32%">
                    </td>
                </tr>
                </table>
                <br />
                <asp:Panel ID="Pnl_Datos_Generales" GroupingText="Datos de Movimiento"  Font-Size="8px" runat="server" style="width:98%;color:Blue;font-size:9px;">                
                <table id="Table3" border="0" cellspacing="0" style="width: 98%;">
                <tr>
                    <td style="width:18%">
                    Fecha
                    </td>
                    <td style="width:32%">
                        <asp:TextBox ID="Txt_Fecha" Width="97%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%">
                    
                    </td>
                    <td style="width:32%">                        
                    </td>
                </tr>
                <tr>
                    <td style="width:18%">
                    Número Recepción
                    </td>
                    <td style="width:32%">
                        <asp:TextBox ID="Txt_Numero_Recepcion" Width="97%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%" align="left">
                    &nbsp;&nbsp;Clave de Trámite
                    </td>
                    <td style="width:32%">
                        <asp:TextBox ID="Txt_Clave" Width="97%" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>                    
                    <td style="width:18%" align="left">
                    Notario
                    </td>
                    <td style="width:32%" colspan="3">
                    <asp:TextBox ID="Txt_Notario" Width="99%" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td style="width:18%">
                    No. Notaría
                    </td>                    
                    <td style="width:32%">
                    <asp:TextBox ID="Txt_Notaria" Width="97%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%">
                    &nbsp;&nbsp;Número Escritura
                    </td>                    
                    <td style="width:32%">
                    <asp:TextBox ID="Txt_Numero_Escritura" Width="97%" runat="server"></asp:TextBox>
                    </td>                
                </tr>
                <tr>
                    <td style="width:18%">
                    Cuenta Predial
                    </td>                    
                    <td style="width:32%">
                    <asp:TextBox ID="Txt_Cuenta_Predial" Width="97%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width:18%" align="left">
                    &nbsp;&nbsp;Estatus
                    </td>
                    <td style="width:32%">
                    <asp:TextBox ID="Txt_Estatus" Width="97%" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>                  
            </asp:Panel>
         <br />
        </div>        
         
         <div id="Div2" runat="server" style="background-color:#ffffff; width:100%;">
         <br />         
         <div id="Div1" runat="server" style="font-size:11px;font-weight:bold;text-align:left;background-color:#F0F8FF;">         
                    Tabla de Empleados Activos
         <div id="Div_Botones_Operacion" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
            <table id="Table2" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                <tr>                    
                    <td colspan="4">
                    <hr id="Hr2" runat="server" />
                    <center>
                        <asp:GridView ID="Grid_Empleados_Activos" runat="server" AllowPaging="true" 
                            AutoGenerateColumns="False" CssClass="GridView_1" 
                            EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                            onpageindexchanging="Grid_Empleados_Activos_PageIndexChanging" 
                            onselectedindexchanged="Grid_Empleados_Activos_SelectedIndexChanged" 
                            PageSize="15" Style="white-space:normal" Width="96%">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="Número empleado" 
                                    SortExpression="NO_EMPLEADO">
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" 
                                    SortExpression="NOMBRE_EMPLEADO">
                                    <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Activar/Desactivar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btn_Activar_Desactivar" runat="server" 
                                            CommandArgument='<%# Eval("NO_EMPLEADO") %>' CommandName="Activar" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_update.png" 
                                            OnClick="Btn_Activar_Desactivar_Click" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="17%" />
                                    <ItemStyle HorizontalAlign="Center" Width="17%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                    SortExpression="ESTADO">
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                        </center>
                    </td>
                </tr>
            </table>
        </div>
        </div>
        </div>
        <center>
        <div id="Div3" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
        <div id="Div_Grid_Empleados" runat="server" style="background-color:#ffffff; width:90%; height:100%;">
                    <table id="Tbl_Grid" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">
                             <tr>
                             <td style="background:#CCCCCC;">
                                <asp:Button ID="Btn_Generar_Tabla" runat="server" Text="Actualizar tabla Empleados Activos" 
                                    OnClick="Btn_Generar_Tabla_Click" CssClass="button_autorizar" 
                                    OnClientClick="return confirm('¿Esta seguro de actualizar la tabla?');" 
                                    ToolTip="Actualizar Tabla de Empleados Activos" />
                            </td>                                 
                            </tr>       
                    </table>
                    <br />
            </div>
            </div>
            </center>
            <center>
        <div id="Div_Pendientes_Titulo" runat="server" style="font-size:11px;font-weight:bold;text-align:left;background-color:#F0F8FF;">
                    Tabla de Pendientes asignados
                
        <div id="Div_Grid_Pendientes" runat="server" style="background-color:#ffffff;border-style:outset;width:99%; height:100%;">
                    <table id="Table1" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="Grid_Movimientos" runat="server" AllowPaging="true"
                                        AutoGenerateColumns="False" CssClass="GridView_1"
                                        GridLines="none"
                                        PageSize="5" Style="white-space:normal" Width="96%"
                                        onselectedindexchanged="Grid_Movimientos_SelectedIndexChanged" 
                                        onpageindexchanging="Grid_Movimientos_PageIndexChanging">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select"
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="7%"/>
                                            </asp:ButtonField>
                                            
                                            <asp:BoundField DataField="NO_RECEPCION_DOCUMENTO" HeaderText="Número de Recepción">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="NUMERO_ESCRITURA" HeaderText="Número Escritura">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="NOMBRE_NOTARIO" HeaderText="Nombre Notario">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="NUMERO_NOTARIA" HeaderText="Numero de Notaría">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                            </asp:BoundField>
                                            
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                            </tr>                        
                    </table>                    
            </div>            
            </div>
         </center>
         <center>
        <div id="Div_Reasignacion" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
        <div id="Div_Reasignacion_Inner" runat="server" style="background-color:#ffffff; width:90%; height:100%;">
                    <table id="Table4" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">                            
                             <tr>
                             <td style="background:#CCCCCC;">
                                <asp:Button ID="Btn_Reasignar" runat="server" Text="Reasignar Movimiento" 
                                    CssClass="button_autorizar" 
                                    OnClientClick="return confirm('La reasignacion será automatica \n y se le borrará este Pendiente al usuario selceccionado \n ¿Esta seguro de reasignar este movimiento?');" 
                                    ToolTip="Reasignación de movimientos" onclick="Btn_Reasignar_Click" />
                            </td>                                 
                            </tr>       
                    </table>
                    <br />
            </div>
            </div>
            </center>
        <asp:HiddenField ID="Hdn_Empleado_ID" runat="server"/>
    </ContentTemplate>    
    </asp:UpdatePanel>
</asp:Content>