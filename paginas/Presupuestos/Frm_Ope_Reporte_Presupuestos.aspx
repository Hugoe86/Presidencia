<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Reporte_Presupuestos.aspx.cs" Inherits="paginas_presupuestos_Frm_Ope_Reporte_Presupuestos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
               <%-- <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>--%>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
         <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
        <tr>
                <td colspan ="4" class="label_titulo">Reporte de Presupuestos</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                            </td>
                        </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila de Busqueda y Botones Generales --%>
            <tr class="barra_busqueda">
                    <td style="width:20%;">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                            
                        </ContentTemplate>
                    </td>
                   <td align="right" colspan="3" style="width:99%;">
                       <asp:ImageButton ID="Btn_Limpiar" runat="server" 
                           ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                           onclick="Btn_Limpiar_Click" ToolTip="Limpiar" />
                       <asp:ImageButton ID="Btn_Buscar" runat="server" 
                           ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" 
                           ToolTip="Consultar" />
                    </td>
            </tr>
            <%--Div_Principal --%>
            <tr>
                <td colspan="4">
                    <table width="100%">
                        <tr>
                            <td style="width:20%">
                                Unidad Responsable
                            </td>
                            <td style="width:80%">
                                <asp:DropDownList ID="Cmb_U_Responsable" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                            
                       </tr>
                       <tr>
                            <td style="width:20%">
                                Programa
                            </td>
                            <td style="width:80%">
                                <asp:DropDownList ID="Cmb_Programas" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                            
                       </tr>
                       <tr>
                            
                            <td style="width:20%">
                                Fuente de Financiamiento
                            </td>
                            <td style="width:80%">
                                <asp:DropDownList ID="Cmb_Fte_Financiamiento" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Capitulo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Capitulo" runat="server" Width="99%">
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                Concepto
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Concepto" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                Partida Generica
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Partida_Generica" runat="server" Width="99%">
                                </asp:DropDownList>
                                </td>
                                
                       </tr>
                       <tr>
                            <td>
                                Partida Especifica
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Width="99%">
                                </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                Año
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" Width="20%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                TargetControlID="Txt_Anio"  
                                FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9"
                                Enabled="True" InvalidChars="<,>,&,',!,">   
                                </cc1:FilteredTextBoxExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            <table width="100%">
                             
                                <tr>
                                    <td colspan="2">
                                    
                                    
                                        <asp:GridView ID="Grid_Presupuestos" runat="server" AllowSorting="true" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Both" 
                                            HeaderStyle-Height="0%" Width="100%">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="UR" HeaderText="" SortExpression="UR" 
                                                    Visible="false">
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FUENTE" HeaderText="Fte.Financiamiiento" 
                                                    SortExpression="FUENTE" Visible="true">
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" 
                                                    SortExpression="PROGRAMA" Visible="True">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="19%" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PARTIDA" HeaderText="Partida" 
                                                    SortExpression="PARTIDA" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="28%" Wrap="true" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ASIGNADO" DataFormatString="{0:n}" 
                                                    HeaderText="Asignado" SortExpression="ASIGNADO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AMPLIACION" DataFormatString="{0:n}" 
                                                    HeaderText="Ampliacion" SortExpression="AMPLIACION" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="REDUCCION" DataFormatString="{0:n}" 
                                                    HeaderText="Reducción" SortExpression="REDUCCION" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MODIFICADO" DataFormatString="{0:n}" 
                                                    HeaderText="Modificado" SortExpression="MODIFICADO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DEVENGADO" DataFormatString="{0:n}" 
                                                    HeaderText="Devengado" SortExpression="DEVENGADO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PAGADO" DataFormatString="{0:n}" HeaderText="Pagado" 
                                                    SortExpression="PAGADO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DISPONIBLE" DataFormatString="{0:n}" 
                                                    HeaderText="Disponible" SortExpression="DISPONIBLE" Visible="true">
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COMPROMETIDO" DataFormatString="{0:n}" 
                                                    HeaderText="Comprometido" SortExpression="COMPROMETIDO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EJERCIDO" DataFormatString="{0:n}" 
                                                    HeaderText="Ejercido" SortExpression="EJERCIDO" Visible="true">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="8" />
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            
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