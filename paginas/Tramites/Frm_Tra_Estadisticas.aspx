<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Tra_Estadisticas.aspx.cs" Inherits="paginas_Tramites_Frm_Tra_Estadisticas" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
     <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
        
          //Abrir una ventana modal
            function Abrir_Ventana_Modal(Url, Propiedades)
            {
                window.showModalDialog(Url, null, Propiedades);
            }
            
            //  revisara que el estatus no sea mayor al 100%
            function Comparar_Estatus(ctrl) {
                var Valor = parseFloat(ctrl.value);

                if (Valor > 100) {
                    $('input[id$=Txt_Avance]').val('');
                    alert('El Valor no puede ser mayor a 100%!!');
                }
            }
        </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            
        <%--Div de Contenido --%>
        <table width="100%">
        <tr>
            <td colspan ="6" class="label_titulo"> Estadisticas de Tramites</td>
        </tr>
        <%--Fila de div de Mensaje de Error --%>
        <tr>
            <td colspan ="6">
                <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                <table style="width:100%;">
                    <tr>
                        <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                        </td>            
                    </tr>
                    <tr>
                        <td style="width:10%;">              
                        </td>            
                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                        
                        </td>
                    </tr> 
                </table>                   
                </div>
            </td>
        </tr>
        <%--Fila 3 Renglon de barra de Busqueda--%>
        <tr class="barra_busqueda" align="right">
            <td colspan = "6" align="left">
                <asp:ImageButton ID="Btn_Graficar" runat="server" ToolTip="Graficar" 
                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_graficar.png" 
                onclick="Btn_Graficar_Click"/>
                <asp:ImageButton ID="Btn_Limpiar" runat="server" ToolTip="Limpiar formulario" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                onclick="Btn_Limpiar_Click"/>
                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                onclick="Btn_Salir_Click"/>
            </td>
        </tr>
    </table>
    
    <table width="100%">
        <tr>
            <td style="width:100%" align="left">
                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="Blue" 
                    onclick="Btn_Busqueda_Avanzada_Click">Búsqueda Avanzada Tramites</asp:LinkButton>
            </td>
        </tr>
    </table>
    
      <div id="Div_Tramites" runat="server" >
            <asp:Panel ID="Pnl_Tramites" runat ="server" GroupingText="Trámites">
                <table width="100%">
                    <tr> 
                        <td></td>        
                    </tr>
                    <tr> 
                        <td align="left">
                            <asp:CheckBox ID="Chk_Todos" runat="server" Text = "Todos" 
                                oncheckedchanged="Chk_Todos_CheckedChanged" AutoPostBack="True"/></td>        
                    </tr>
                    <%--Grid View--%>
                    <tr>
                        <td>
                        <%--<div style="overflow:auto; width:100%; height:150px">--%>
                       <div id="Div_Grid_Tramites" runat="server" 
                                    style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                            <asp:GridView ID="Grid_Tramites" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" GridLines="None" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Selecciona" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-Font-Size="12px"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderStyle-Font-Size="13px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:CheckBox ID="Chk_Tramite" runat="server" AutoPostBack="True" 
                                                    oncheckedchanged="Chk_Tramite_CheckedChanged" />
                                            </center>
                                        </ItemTemplate>
                                        <ControlStyle />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="90%" />
                                        <ItemStyle HorizontalAlign="Left" Width="90%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </div>
                        </td>
                    </tr>
                    <%--<tr>
                       <td>
                         <hr class="linea"/>        
                       </td>  
                    </tr>--%>   
                </table>
            </asp:Panel>
        
        </div>
        
        
        <%--
        <tr>
            <td align = "center">
            De
            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
            TargetControlID="Txt_Fecha_Inicio" Format ="dd/MMM/yyyy">
            </cc1:CalendarExtender>
            &nbsp;&nbsp; Al&nbsp;&nbsp;
            <asp:TextBox ID="Txt_Fecha_Fin" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
            TargetControlID="Txt_Fecha_Fin" Format ="dd/MMM/yyyy">
            </cc1:CalendarExtender>
            </td> 
        </tr> --%>   
        
        <table width="100%">
            <tr>
                <td  style="width:15%">
                    <asp:CheckBox ID="Chk_Estatus"  Text = "Estatus" runat="server" Width="100%"
                        AutoPostBack = "true" oncheckedchanged="Chk_Estatus_CheckedChanged" />
                </td>
                <td style="width:85%"> 
                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width = "90%" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td  style="width:15%">
                    <asp:CheckBox ID="Chk_Avance" runat="server" Text="Avance (%)" 
                        oncheckedchanged="Chk_Avance_CheckedChanged" AutoPostBack="true"/>
                </td>
                <td  style="width:85%">
                    <asp:TextBox ID="Txt_Avance" runat="server" MaxLength="3" Width="20%"
                        name="<%=Txt_Avance.ClientID %>"  style="text-align:right"
                        >
                    </asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        TargetControlID="Txt_Avance" ValidChars="1234567890">
                        </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
        </table>
        
        
        </table>
            <div id="Div_Fechas" runat="server" >
                <asp:Panel ID="Pnl_Fechas" runat ="server" GroupingText="Fecha">
                    <table width="100%">
                        <tr>
                            <td style="width:15%">
                                <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text ="Inicio"></asp:Label>
                            </td>
                             <td style="width:20%"> 
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" MaxLength="11" Enabled="false" 
                                    Width="75%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Inicio" 
                                        PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                    <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                             </td>
                             <td style="width:15%" align="right">
                                <asp:Label ID="Lbl_Fecha_Fin" runat="server" Text ="Fin"></asp:Label>
                             </td>
                             <td style="width:20%" align="left">
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" MaxLength="11" Enabled="false"
                                    Width="75%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Fin" Format ="dd/MMM/yyyy"
                                        PopupButtonID="Btn_Fecha_Fin">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                             </td>
                             <td style="width:30%">
                             </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
