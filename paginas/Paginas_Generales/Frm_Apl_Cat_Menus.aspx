<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Cat_Menus.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Cat_Menus" Title="Catálogo de Menus" UICulture="es-MX" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
function switchViews(obj,row) 
    { 
        var div = document.getElementById(obj); 
        var img = document.getElementById('img' + obj); 
         
        if (div.style.display=="none") 
            { 
                div.style.display = "inline"; 
                if (row=='alt') 
                    { 
                        img.src="../imagenes/paginas/stocks_indicator_down.png";
                    } 
                else 
                    { 
                        img.src="../imagenes/paginas/stocks_indicator_down.png";
                    } 
                img.alt = "Close to view other customers"; 
            } 
        else 
            { 
                div.style.display = "none"; 
                if (row=='alt') 
                    { 
                        img.src="../imagenes/paginas/add_up.png";
                    } 
                else 
                    { 
                        img.src="../imagenes/paginas/add_up.png";
                    } 
                img.alt = "Expand to show orders"; 
            } 
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="SM_Menus" runat="server" />
    <asp:UpdatePanel ID="UPnl_Menus" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="UPnl_Menus"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                   
                </asp:UpdateProgress>              
                
            <div id="Div_Menus" style="background-color:#ffffff; width:98%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Catálogo de Menus</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>              
            
                <table width="98%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td colspan="2">                
                             <div align="right" class="barra_busqueda">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">                                                  
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                OnClientClick="return confirm('¿Está seguro de eliminar menú. Si eliges eliminar el menú el mismo será eliminado y todas sus relaciones?');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                    B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Menus" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar Menu"
                                                        Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Menus" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Ingrese el Menu>" TargetControlID="Txt_Busqueda_Menus" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Menus" runat="server" TargetControlID="Txt_Busqueda_Menus"
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    <asp:ImageButton ID="Btn_Buscar_Menus" runat="server" TabIndex="6" ToolTip="Consultar"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Menus_Click" />                                  
                                                </td>
                                            </tr>                                                                          
                                        </table>                                    
                                       </td>       
                                     </tr>         
                                  </table>                      
                                </div>
                         </td>
                     </tr>
                </table>                      

                <br />               

                <table width="98%">
                    <tr>
                        <td style="width:20%; text-align:left;">
                            Menu ID
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Menu_ID" runat="server" Width="98%" Enabled="false"/>
                        </td>
                        <td style="width:20%; text-align:left;">
                            &nbsp;&nbsp;Modulo
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Modulo" runat="server" Width="100%"/>
                        </td>                        
                    </tr>                    
                    <tr>
                        <td style="width:20%; text-align:left;">
                            *Nombre
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Menu" runat="server" MaxLength="100" TabIndex="7" Width="99.5%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Menu" runat="server" TargetControlID="Txt_Nombre_Menu"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left;">
                            *Tipo
                        </td>                        
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Tipo_Menu" runat="server" Width="100%" TabIndex="8" AutoPostBack="True"
                                onselectedindexchanged="Cmb_Tipo_Menu_SelectedIndexChanged">
                                <asp:ListItem><- Seleccione -></asp:ListItem>
                                <asp:ListItem>SUBMENU</asp:ListItem>
                                <asp:ListItem>MENU</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left;">
                            *Orden
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Orden_Menu" runat="server" TabIndex="9" MaxLength="4" Width="98%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Orden_Menu" runat="server" 
                                TargetControlID="Txt_Orden_Menu" FilterType="Custom, Numbers" />
                        </td>
                    <tr>    
                        <td style="width:20%; text-align:left;">
                            Clasificaci&oacute;n
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Clasificacion_Menu" runat="server" Width="100%" TabIndex="10">
                                <asp:ListItem><- Seleccione -></asp:ListItem>
                                <asp:ListItem>CATALOGO</asp:ListItem>
                                <asp:ListItem>OPERACION</asp:ListItem>
                                <asp:ListItem>REPORTE</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left;">
                            Men&uacute;
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Menus" runat="server" Width = "100%" TabIndex="12" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left;">
                           URL
                        </td>
                        <td style="width:80%; text-align:left;" colspan="3">
                             <asp:TextBox ID="Txt_Url_Menu" runat="server" MaxLength="100" TabIndex="11" Width="99%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ur_Menu" 
                                runat="server" TargetControlID="Txt_Url_Menu" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="./?:_ " />                       
                        </td>
                    </tr>
                </table>                      

                <br />                               
            
                    <asp:GridView ID="Grid_Menus" runat="server" DataKeyNames="MENU_ID"
                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                        onselectedindexchanged="Grid_Menus_SelectedIndexChanged"
                        AllowSorting="True" OnSorting="Grid_Menus_Sorting" HeaderStyle-CssClass="tblHead"
                        OnRowDataBound="Grid_Menus_RowDataBound"
                        OnRowCreated="Grid_Menus_RowCreated">
                        
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <HeaderStyle CssClass="GridHeader" ForeColor="White" />
                        <AlternatingRowStyle CssClass="GridAltItem" />                                
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png">
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:TemplateField> 
                                <ItemTemplate> 
                                    <a href="javascript:switchViews('div<%# Eval("MENU_ID") %>', 'one');"> 
                                        <img id="imgdiv<%# Eval("MENU_ID") %>" alt="Click to show/hide orders" border="0" src="../imagenes/paginas/add_up.png" /> 
                                    </a> 
                                </ItemTemplate> 
                                <AlternatingItemTemplate> 
                                    <a href="javascript:switchViews('div<%# Eval("MENU_ID") %>', 'alt');"> 
                                        <img id="imgdiv<%# Eval("MENU_ID") %>" alt="Click to show/hide orders" border="0" src="../imagenes/paginas/add_up.png" /> 
                                    </a> 
                                </AlternatingItemTemplate> 
                            </asp:TemplateField>                                      
                            <asp:BoundField DataField="Menu_ID" 
                                Visible="True">
                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                <ItemStyle HorizontalAlign="Left" Width="0%" ForeColor="Transparent" Font-Size="0px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Menu_Descripcion" HeaderText="Menus" Visible="True" SortExpression="Menu_Descripcion">
                                <FooterStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" Width="90%" />
                                <ItemStyle HorizontalAlign="Left" Width="90%" />
                            </asp:BoundField>
                             <asp:TemplateField>
                                <ItemTemplate>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td colspan="100%">
                                            <div id="div<%# Eval("MENU_ID") %>" style="display:none;position:relative;left:25px;" >                                                     
                                                <asp:GridView ID="Grid_Submenus" runat="server"
                                                    AutoGenerateColumns="False" GridLines="None" CssClass="GridView_Nested"
                                                     HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Submenus_SelectedIndexChanged"
                                                     OnRowCreated="Grid_Submenus_RowCreated"
                                                    >
                                                    
                                                     <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                     <PagerStyle CssClass="GridHeader_Nested" />
                                                     <HeaderStyle CssClass="GridHeader_Nested" />
                                                     <AlternatingRowStyle CssClass="GridAltItem_Nested" /> 
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                            ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png">
                                                            <ItemStyle Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="Menu_ID" 
                                                            Visible="True">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" ForeColor="Transparent" Font-Size="0px"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Menu_Descripcion" HeaderText="Submenus" Visible="True" >
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="35%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="URL_LINK" HeaderText="Link" Visible="True" >
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60%"/>
                                                            <ItemStyle HorizontalAlign="Left" Width="60%" Font-Size="11px"/>
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <br /><br /><br /><br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>

