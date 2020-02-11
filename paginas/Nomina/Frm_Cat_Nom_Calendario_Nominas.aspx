<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Calendario_Nominas.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Calendario_Nominas" Title="Catálogo Calendario Nómina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../javascript/Js_Cat_Nom_Calendario_Nomina.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Calendario_Nominas" runat="server"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calendario_Nominas" style="background-color:#ffffff; width:99%; height:100%;">
            
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td colspan="4" class="label_titulo">
                        Catálogo de Calendario de Nominas
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    </td>
                </tr>
                <tr class="barra_busqueda" align="right">
                    <td colspan = "2" align = "left">
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                            CssClass="Img_Button" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            onclick="Btn_Nuevo_Click"  CausesValidation="false"/>
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                            CssClass="Img_Button" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click" CausesValidation="false"/> 
                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                            CssClass="Img_Button" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"                                 
                            CausesValidation="false" OnClientClick="return confirm('¿Está seguro de eliminar el calendario de nomina seleccionado?');" 
                            onclick="Btn_Eliminar_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                            CssClass="Img_Button" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click" CausesValidation="false"/>
                    </td>
                    <td colspan="2">Busqueda
                        <asp:TextBox ID="Txt_Busqueda_Calendario_Nomina" runat="server" MaxLength="100"  Width="200px"
                            TabIndex="5"  ToolTip = "Buscar por No Nomina"/>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Requistos_Empleado" 
                            runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Ingrese Nombre>" 
                            TargetControlID="Txt_Busqueda_Calendario_Nomina" />
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Requistos_Empleado" 
                            runat="server" TargetControlID="Txt_Busqueda_Calendario_Nomina" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                            ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                        <asp:ImageButton ID="Btn_Buscar_Calendario_Nomina" runat="server" TabIndex="6"
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" onclick="Btn_Buscar_Calendario_Nomina_Click"
                             />
                    </td>                        
                </tr>
            </table>
                            
            <table width="100%">
                <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;width:20%;">
                       Nomina ID
                    </td>
                    <td  style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Nomina_ID" runat="server" Width="88%"/>
                    </td>
                    <td style="text-align:left;width:20%;">
                       &nbsp;&nbsp;*Año
                    </td>
                    <td  style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Anio_Calendario_Nomina" runat="server" Width="98%"/>
                        <span id="Mensaje_Anio" class="watermarked"></span> 
                        <cc1:FilteredTextBoxExtender ID="FTxt_Anio_Calendario_Nomina" 
                            runat="server" TargetControlID="Txt_Anio_Calendario_Nomina" FilterType="Numbers"/>
                    </td>                        
                </tr>
                <tr>
                    <td style="text-align:left;width:20%;">
                       &nbsp;&nbsp;*Fecha Inicio
                    </td>
                    <td style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server"  Width="88%"/>
                        <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                            TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha Final"/>
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Inicio" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Fecha_Inicio" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Mee_Txt_Fecha_Inicio" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Inicio"
                            ControlExtender="Mee_Txt_Fecha_Inicio" 
                            EmptyValueMessage="La Fecha Inicial es obligatoria"
                            InvalidValueMessage="Fecha Inicial Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                        
                    </td>
                    <td style="text-align:left;width:20%;">
                       *Fecha Fin
                    </td>
                    <td style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="88%" />
                        <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Fin_FilteredTextBoxExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                            TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                        <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha Inicial"/>   
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Fin" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Fecha_Fin" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Mee_Txt_Fecha_Fin" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Fin"
                            ControlExtender="Mee_Txt_Fecha_Fin" 
                            EmptyValueMessage="La Fecha Final es obligatoria"
                            InvalidValueMessage="Fecha Final Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Final"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                
                    </td>       
                </tr>
               <tr>
                    <td style="text-align:left;width:20%;">                          
                    </td>
                    <td style="text-align:left;width:30%;">     
                    </td>
                    <td style="text-align:left;width:20%;">
                    </td>
                    <td style="text-align:left;width:30%;">
                        <asp:Button ID="Btn_Ver_Catorcenas" runat="server" Text="Ver Fechas de Catorcenas" OnClick="Btn_Ver_Catorcenas_Click" Width="100%"
                            CssClass="button" ToolTip="Catorcena" CausesValidation="false"/>
                    </td>       
                </tr>
                <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>                    
            </table>
               
                
            <cc1:TabContainer ID="Contenedor" runat="server" Width="100%" ActiveTabIndex="0" >
                <cc1:TabPanel HeaderText="Nominas" ID="Tab_Nominas" runat="server">
                    <HeaderTemplate>
                        Calendario Nomina
                    </HeaderTemplate>
                      <ContentTemplate>
                            <table width="100%">
                                <tr align="center">
                                    <td colspan="4">
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                            <asp:GridView ID="Grid_Calendario_Nominas" runat="server" 
                                                AutoGenerateColumns="False"  GridLines="None"  AllowPaging="True" PageSize="1000"
                                                onpageindexchanging="Grid_Calendario_Nominas_PageIndexChanging" 
                                                onselectedindexchanged="Grid_Calendario_Nominas_SelectedIndexChanged" 
                                                AllowSorting="True" OnSorting="Grid_Calendario_Nominas_Sorting" 
                                                class="filterable" HeaderStyle-CssClass="tblHead">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar" CausesValidation="false"
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="15%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Nomina_ID" HeaderText="Nomina ID" SortExpression="NOMINA_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha Inicio" 
                                                        DataFormatString="{0:dd/MMM/yyyy}" SortExpression="Fecha_Inicio">
                                                        <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="33%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha_Fin" HeaderText="Fecha Fin" 
                                                        DataFormatString="{0:dd/MMM/yyyy}" SortExpression="Fecha_Fin">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderText="" />
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>     
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Catorcenas" ID="TPnl_Catorcenas" runat="server">
                        <HeaderTemplate>
                            Catorcenas 
                        </HeaderTemplate>                    
                            <ContentTemplate>                    
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;width:20%;">
                                            Dias
                                        </td>
                                        <td style="text-align:left;width:30%;">
                                            <asp:DropDownList ID="Cmb_Dias_Periodo_Nominal" runat="server" Width="100%" />                           
                                        </td>
                                        <td style="text-align:left;width:20%;">
                                            N&uacute;mero Periodos
                                        </td> 
                                        <td style="text-align:left;width:30%;">
                                           <asp:TextBox ID="Txt_Numero_Periodos_Generar" runat="server"  Width="98%"/>   
                                        </td>                                                                                                  
                                    </tr>                                                        
                                </table>
                                
                                <asp:Panel ID="Pnl_Operaciones" runat="server">                           
                                    <table width="100%">       
                                        <tr>
                                            <td style="width:100%;" align="right">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width:70%;">
                                                            <asp:Button ID="Btn_Agregar_Periodos" runat="server" Text="Agregar Periodos" CssClass="button_autorizar" Width="100%"
                                                                OnClick="Btn_Agregar_Periodos_Click"/>                                                         
                                                        </td>
                                                        <td style="width:30%;">
                                                            <asp:Button ID="Btn_Limpiar_Periodos" runat="server" CssClass="button_autorizar" Text="Limpiar Periodos Generados" 
                                                                OnClick="Btn_Limpiar_Periodos_Click" Width="100%"/>                                                                                                 
                                                        </td>
                                                    </tr>                                    
                                                </table>                           
                                            </td>
                                        </tr>                        
                                    </table>                        
                                </asp:Panel>                                  
                                <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                    <asp:GridView ID="Grid_Catorcenas" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False"  GridLines="None" onrowdatabound="Grid_Catorcenas_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Estatus" ControlStyle-BackColor="ActiveBorder">
                                                <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Estatus_Catorcena" runat="server" Checked="true"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NO_NOMINA" HeaderText="No Nomina" 
                                                Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%"   Font-Bold="true" ForeColor="Black" Font-Size="12px"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_INICIO" HeaderText="Inicia (Dia/Mes/Año)" Visible="True" DataFormatString="{0:dd MMMM yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Separador" HeaderText="Dias Periodo" Visible="True" >
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_FIN" HeaderText="Fecha Pago (Dia/Mes/Año)" Visible="True" DataFormatString="{0:dd MMMM yyyy}">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%"  Font-Bold="true" ForeColor="Black" Font-Size="12px"/>
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>                        
                                </div>
                            </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>                                         
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>        
</asp:Content>

