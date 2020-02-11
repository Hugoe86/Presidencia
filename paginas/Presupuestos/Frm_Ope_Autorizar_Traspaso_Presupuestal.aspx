<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Autorizar_Traspaso_Presupuestal.aspx.cs" Inherits="paginas_presupuestos_Frm_Ope_Autorizar_Traspaso_Presupuestal" Title="Autorizar Movimiento Presupuestal"%>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<script runat="server">

   
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" 
        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
    </asp:UpdateProgress>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div id="div_progress" class="processMessage" >
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        
        
            <div id="Div_Movimiento_Presupuestal" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Autorizar Movimiento Presupuestal</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                           <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Movimiento_Presupuestal" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Estatus o Solicitud"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Movimiento_Presupuestal" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Solicitud>" TargetControlID="Txt_Busqueda_Movimiento_Presupuestal" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Movimiento_Presupuestal" runat="server" 
                                TargetControlID="Txt_Busqueda_Movimiento_Presupuestal" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ-. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Movimiento_Presupuestal" runat="server" 
                                ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Movimiento_Presupuestal_Click"/>
                        </td> 
                    </tr>
                </table>
                
                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    
                     <tr>
                        <td align="center" colspan="4">
                            <div id="Div_Grid" runat="server" style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                <asp:GridView ID="Grid_Movimiento_Presupuestal" runat="server"  CssClass="GridView_1" Width="100%" 
                                    AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" 
                                    AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                    onselectedindexchanged="Grid_Movimiento_Presupuestal_SelectedIndexChanged" 
                                    onsorting="Grid_Movimiento_Presupuestal_Sorting" 
                                    onpageindexchanging="Grid_Movimiento_Presupuestal_PageIndexChanging" >
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="7%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_SOLICITUD" HeaderText="Solicitud" Visible="True" SortExpression="NO_SOLICITUD">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CODIGO1" HeaderText="Partida Origen" Visible="True" SortExpression="CODIGO1">
                                            <HeaderStyle HorizontalAlign="Left" Width="26%" />
                                            <ItemStyle HorizontalAlign="Left" Width="26%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CODIGO2" HeaderText="Partida Destino" Visible="True" SortExpression="CODIGO2">
                                            <HeaderStyle HorizontalAlign="Left" Width="26%" />
                                            <ItemStyle HorizontalAlign="Left" Width="26%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IMPORTE" HeaderText="Importe" Visible="True" SortExpression="IMPORTE" DataFormatString="{0:n}">
                                            <HeaderStyle HorizontalAlign="Right" Width="14%" />
                                            <ItemStyle HorizontalAlign="Right" Width="14%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS" >
                                            <HeaderStyle HorizontalAlign="Center" Width="17%" />
                                            <ItemStyle HorizontalAlign="Center" Width="17%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                         </div>
                        </td>
                    
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Numero_Solicitud" runat="server" Width="100%" Text="Número de solicitud" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Numero_Solicitud" runat="server" Width="50%" TabIndex="6" ReadOnly="true" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Unidad_Responsable_Origen" runat="server" Width="100%" Text="Unidad Responsable Origen" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Unidad_Responsable_Origen" runat="server" ReadOnly="true" Width="95%" Visible="False"></asp:TextBox>                  
                        </td >
                        <td>
                            <asp:Label ID="Lbl_Unidad_Responsable_Destino" runat="server" Width="100%" Text="Unidad Responsable Destino" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Unidad_Responsable_Destino" runat="server" Width="98%" ReadOnly="true" Visible="False"></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Label ID="lbl_Codigo_Programatico_Origen" runat="server" Width="100%" Text="Código Programatico Origen" Visible="False"></asp:Label>
                        
                        </td>
                        <td >
                             <asp:TextBox ID="Txt_Codigo_Programatico_Origen" runat="server" TabIndex="7"  Width="95%" ReadOnly="true" Visible="False">
                              </asp:TextBox>
                        </td>
                        <td align="justify">
                        <asp:Label ID="Lbl_Codigo_Programatico_Destino" runat="server" Width="100%" Text="Código Programatico Destino" Visible="False"></asp:Label></td>
                        <td >
                            <asp:TextBox ID="Txt_Codigo_Programatico_Destino" runat="server" Width="98%" ReadOnly="true"   Visible="False"
                                TabIndex="8" >
                            </asp:TextBox>    
                        </td>
                    </tr>
                                      
                    <tr>
                        <td>
                        <asp:Label ID="Lbl_Fuente_Financiamento_Origen" runat="server" Width="100%" Text="Fuente Financiamento Origen" Visible="False"></asp:Label>
                        </td>
                        
                        <td >
                            <asp:TextBox ID="Txt_Fuente_Financiamiento_Origen" runat="server" Width="95%" Visible="False"
                                 ReadOnly="true" ></asp:TextBox>                  
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Fuente_Financiamiento_Destino" runat="server" Width="100%" Text="Fuente Financiamiento Destino" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Fuente_Financiamiento_Destino" runat="server" Width="98%" Visible="False"
                                 ReadOnly="true"></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Area_Funcional_Origen" runat="server" Width="100%" Text="Área Funcional origen" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Area_Funcional_Origen" runat="server" ReadOnly="true" Width="95%" Visible="False" ></asp:TextBox>                  
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Area_Funcional_Destino" runat="server" Width="100%" Text="Área Funcional Destino" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Area_Funcional_Destino" runat="server" Width="98%" ReadOnly="true" Visible="False"></asp:TextBox>                  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Programa_Origen" runat="server" Width="100%" Text="Programa Origen" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Programa_Origen" runat="server" ReadOnly="true" Width="95%" Visible="False"></asp:TextBox>                  
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Programa_Destino" runat="server" Width="100%" Text="Programa Destino" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Programa_Destino" runat="server" Width="98%" ReadOnly="true" Visible="False"></asp:TextBox>                  
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Partida_Origen" runat="server" Width="100%" Text="Partida Origen" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Partida_Origen" runat="server" ReadOnly="true" TextMode="MultiLine" Width="95%" Visible="False" ></asp:TextBox>                  
                        </td>
                        <td align"center">
                             <asp:Label ID="Lbl_Partida_Destino" runat="server" Width="100%" Text="Partida Destino" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Partida_Destino" runat="server" TextMode="MultiLine" Width="98%"  ReadOnly="true" Visible="False"></asp:TextBox>                  
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Monto_Traspasar" runat="server" Width="100%" Text="Monto a Traspasar" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Monto_Traspaso" runat="server" Width="95%" ReadOnly="true"  Visible="False"
                                 TabIndex="9"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Estatus_Actual" runat="server" Width="100%" Text="Estatus Actual" Visible="False"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="Txt_Estatus_Actual" runat="server" Width="95%" ReadOnly="true" Visible="False"
                                TabIndex="10"></asp:TextBox>
                        </td>
                      
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Tipo_Operacion" runat="server" Width="100%" Text="Operacion" Visible="False"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="Txt_Tipo_Operacion" runat="server" Width="95%" ReadOnly="true" Visible="False"
                                TabIndex="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Justificacion_Actual" runat="server" Width="100%" Text="Justificación del movimiento" Visible="False"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Justidicacion_Actual" runat="server" Width="98%" ReadOnly="true" Visible="False" TextMode="MultiLine"
                                TabIndex="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Label ID="Lbl_Actualizar_Estatus" runat="server" Width="100%" Text="<hr/>EVALUAR ESTATUS<hr/>" Visible="False"></asp:Label>
                            
                        </td>
                    </tr>
                    
              
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Estatus" runat="server" Width="100%" Text="*Estatus" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Tipo_Estatus" runat="server"  Width="100%" visible="false"
                                TabIndex="10" AutoPostBack="true"  
                                onselectedindexchanged="Cmb_Tipo_Estatus_Movimiento_SelectedIndexChanged">
                                <asp:ListItem>&lt; SELECCIONE ESTATUS &gt;</asp:ListItem>
                                <asp:ListItem>AUTORIZADA</asp:ListItem>
                                <asp:ListItem>RECHAZADA</asp:ListItem>
                            </asp:DropDownList>  
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Justificacion_Solicitud" runat="server" Width="100%" 
                                Text="Comentario" Visible="False"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Justificacion_Solicitud" runat="server" TabIndex="11" ReadOnly="true" Visible="false"
                                TextMode="MultiLine" Height="80px" Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Justificacion_Solicitud" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Justificacion_Solicitud" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Justificacion_Solciitud" runat="server" 
                                TargetControlID="Txt_Justificacion_Solicitud" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                   
                </table>
            </div>
            
           <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                </table>
                
                
                <div id="Div_Grid_Comentarios" runat="server" style="overflow:auto;height:100px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;">
                    <table width="100%">
                       <tr width="100%">
                            <asp:GridView ID="Grid_Comentarios" runat="server"  CssClass="GridView_1" Width="100%" 
                                AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" 
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                EmptyDataText="No se encuentra ningun comentario">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    
                                    <asp:BoundField DataField="Comentario" HeaderText="Comentario" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Right" Width="25%" />
                                    </asp:BoundField>
                                   
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                       </tr>
                    </table>
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>  