<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Polizas_Stock.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Polizas_Stock" Title="Polizas Stock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>
              </ContentTemplate>  
          </asp:UpdatePanel> 
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                               Pólizas de Stock para SAP
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" onclick="Btn_Salir_Click"  />
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" 
                                    CssClass="Img_Button" ToolTip="Guardar" onclick="Btn_Guardar_Click" Visible="false" />   
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
              
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Salidas" runat="server">
                    <table>
                        <tr>
                            <td style="width: 8%;">
                                Fecha
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                            </td>
                            <td style="width: 10%; text-align:right;">
                                Contabilizada
                            </td>
                            <td style="width: 13%;">                                
                                <asp:DropDownList ID="Cmb_Contabilizada" runat="server" Width="45px" 
                                    onselectedindexchanged="Cmb_Contabilizada_SelectedIndexChanged" AutoPostBack="true" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    ToolTip="Consultar" onclick="Btn_Buscar_Click"/>                                
                            </td>
                            <td style="width: 13%; text-align:right;">
                                <asp:LinkButton ID="Lnk_Buscar_Polizas_Anteriores" runat="server" 
                                    ForeColor="Blue" onclick="Lnk_Buscar_Polizas_Anteriores_Click">Buscar Pólizas Generadas</asp:LinkButton>
                            </td>
                        </tr> 
                     </table>  

                     <hr class="linea" />                                   
                     <table>   
                        <tr>
                            <td style="width:80%;" colspan="2">
                                <asp:CheckBox ID="Chk_Seleccionar_Todo" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Seleccionar_Todo_CheckedChanged"
                                    Text="Seleccionar todas las salidas" />                                
                            </td>
                            <td style="width:20%;" colspan="2">
                                <asp:LinkButton ID="Lnk_Mostrar" runat="server" ForeColor="OrangeRed" 
                                    onclick="Lnk_Mostrar_Click" Font-Size="Larger" Font-Bold="true">Mostrar póliza</asp:LinkButton>                                 
                            </td>                            
                            <td align="right" style="text-align: right; width:20%;" colspan="2">                                
                                <asp:Button ID="Btn_Generar_Poliza" runat="server" Text="Generar póliza" OnClick="Btn_Generar_Poliza_Click"
                                    CssClass="button" />
                            </td>
                        </tr>
                    </table>                
                  <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Salidas" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="100%" AllowPaging="false" 
                                    DataKeyNames="NO_SALIDA,FECHA_CREO,TOTAL"
                                    HeaderStyle-CssClass="tblHead" 
                                    EmptyDataText="No se encontraron salidas de stock">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate >
                                                <asp:CheckBox ID="Chk_Salida" runat="server" />                                                
                                            </ItemTemplate >
                                            <ControlStyle Width="12px"/>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>                                      
                                        <asp:BoundField DataField="NO_SALIDA" HeaderText="No. Salida" Visible="True" SortExpression="No_Requisicion">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD_RESP" HeaderText="Unidad Responsable" Visible="True"
                                            SortExpression="UNIDAD_RESP">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:c}"
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" />
                                        </asp:BoundField>  
                                        <asp:BoundField DataField="CONTABILIZADO" HeaderText="Contabilizada" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
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
                  <table >
                    <tr align="right">
                        <asp:HiddenField ID="Hdn_Lista_Salidas" runat="server" />
                        <asp:HiddenField ID="Hdn_Importe" runat="server" />
                    </tr>
                  </table>
                </div>
                                
                <%--Div listado de requisiciones--%>
              
                <div id="Div_Polizas_Stock_Sap" runat="server">
                    <table>
                        <tr>
                            <td style="width: 7%;">
                                Fecha
                            </td>
                            <td style="width: 40%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial_Poliza" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftb_Txt_Fecha_Inicial_Poliza" runat="server" TargetControlID="Txt_Fecha_Inicial_Poliza" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_Poliza_Calendar" runat="server" TargetControlID="Txt_Fecha_Inicial_Poliza" PopupButtonID="Btn_Fecha_Inicial_Poliza" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial_Poliza" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;                                
                                <asp:TextBox ID="Txt_Fecha_Final_Poliza" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftb_Txt_Fecha_Final_Poliza" runat="server" TargetControlID="Txt_Fecha_Final_Poliza" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Final_Poliza_Calendar" runat="server" TargetControlID="Txt_Fecha_Final_Poliza" PopupButtonID="Btn_Fecha_Final_Poliza" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final_Poliza" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                                      
                            </td>
                            <td style="width: 75%; text-align:left;">                                
                                <asp:ImageButton ID="Btn_Buscar_Polizas_Stock_Sap" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    ToolTip="Consultar" onclick="Btn_Buscar_Polizas_Stock_Sap_Click" />                                
                            </td>                                                        
                        </tr> 
                     </table>    
                     <hr class="linea" />                                 
                  <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Polizas_Stock_Sap" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="100%" AllowPaging="false"                                     
                                    HeaderStyle-CssClass="tblHead" 
                                    EmptyDataText="No se encontraron salidas de stock">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns> 
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Imprimir_Requisicion" runat="server" CssClass="Img_Button"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                                    OnClick="Btn_Imprimir_Requisicion_Click"                                                    
                                                    CommandArgument='<%# Eval("NO_POLIZA_STOCK") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        </asp:TemplateField>                                                                        
                                        <asp:BoundField DataField="NO_POLIZA_STOCK" HeaderText="No. Póliza" Visible="True" SortExpression="NO_POLIZA_STOCK">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HORA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HORA" HeaderText="Hora" Visible="True" DataFormatString="{0:hh:mm:ss tt}"
                                            SortExpression="HORA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IMPORTE" HeaderText="Total" DataFormatString="{0:c}"
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" Width="16%" />
                                        </asp:BoundField>     
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Realizó..." 
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left"  />
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
                  <table >
                    <tr align="right">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                    </tr>
                  </table>
                </div>                
                

    </div>
</asp:Content>

