<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Frm_Apl_Control_Acceso_Sistema.aspx.cs" Inherits="paginas_Nomina_Frm_Apl_Control_Acceso_Sistema" 
    Title="Catálogo Roles"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="System.Web.SessionState" %>

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

   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sc_Rpt_Totales_Tipo_Nomina_Banco" runat="server" AsyncPostBackTimeout="360000" ScriptMode="Release" />
<asp:UpdatePanel ID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" runat="server"  >
    <ContentTemplate>
    
        <asp:UpdateProgress ID="UPgs_Rpt_Totales_Tipo_Nomina_Banco" runat="server" 
            AssociatedUpdatePanelID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>                
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div style="width:98%; background-color:White;">
        
            <div id="Div_Contenedor_Msj_Error" style="width:70%;font-size:9px;text-align:left;" runat="server" visible="false" >
                <table style="width:100%;">
                    <tr>
                        <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                           <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                             Width="24px" Height="24px"/>
                            Es Necesario Introducir:
                        </td>            
                    </tr>
                    <tr>
                        <td style="width:10%;">              
                        </td>            
                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>          
                </table>                   
            </div>          
        
            <table width="100%">
                 <tr align="center">
                     <td colspan="2">                
                         <div align="right" class="barra_busqueda">                        
                              <table style="width:100%;height:28px;">
                                <tr>
                                  <td align="left" style="width:59%;">  
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" ImageUrl="../imagenes/paginas/icono_nuevo.png"
                                          onclick="Btn_Nuevo_Click" CausesValidation="false"/>                
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar"
                                          CausesValidation="false" onclick="Btn_Modificar_Click" CssClass="Img_Button" ImageUrl="../imagenes/paginas/icono_modificar.png"/>
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                          CausesValidation="false" onclick="Btn_Eliminar_Click"
                                          OnClientClick="return confirm('¿Estas seguro de eliminar el rol seleccionado?');"
                                          CssClass="Img_Button" ImageUrl="../imagenes/paginas/icono_eliminar.png"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir" 
                                          CausesValidation="false" onclick="Btn_Salir_Click" CssClass="Img_Button" ImageUrl="../imagenes/paginas/icono_salir.png"/>                                                                    
                                  </td>
                                  <td align="right" style="width:41%;">
                                    <table style="width:100%;height:28px;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                            <td style="width:55%;">
                                                <asp:TextBox ID="Txt_Busqueda_Roles" runat="server" 
                                                     ToolTip="Ingrese el nombre del evento a buscar" Width="100%"/>
                                                <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Roles" runat="server" 
                                                                WatermarkCssClass="watermarked" WatermarkText ="< Ingresa el Nombre a Buscar >" 
                                                                TargetControlID="Txt_Busqueda_Roles"/>                                                                                                                                             
                                            </td>
                                            <td style="vertical-align:middle;width:5%;" >
                                                <asp:ImageButton ID="Btn_Busqueda_Roles" runat="server" CausesValidation="False" 
                                                     ImageUrl="../imagenes/paginas/busqueda.png"  ToolTip="Consultar" Height="20px"
                                                     OnClick="Btn_Busqueda_Roles_Click" />                                            
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
            
            <table width="100%">
                <tr>
                    <td  style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <hr />
                    </td>                                                                
                </tr>             
                <tr>
                    <td  style="width:20%; text-align:left; cursor:default; font-size:12px;">
                        Rol ID
                    </td>
                    <td  style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Rol_ID" runat="server" Width="98%" TabIndex="0"/>                        
                    </td>            
                    <td  style="width:20%; text-align:left; cursor:default; font-size:12px;">
                        &nbsp;&nbsp;*Grupo Roles
                    </td>
                    <td  style="width:30%; text-align:left; cursor:default;"> 
                      <asp:DropDownList ID="Cmb_Grupo_Roles" runat="server" Width="100%"/>                                     
                    </td>                                                
                </tr>             
                <tr>
                    <td  style="width:20%; text-align:left; cursor:default; font-size:12px;">
                        *Nombre
                    </td>
                    <td  style="width:30%; text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="99.5%"/>    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Nombre"
                          FilterType="Custom, LowercaseLetters, UppercaseLetters" ValidChars=" Ñ"/>                          
                    </td>                                                      
                </tr>     
                <tr>
                    <td  style="width:20%; text-align:left; cursor:default; vertical-align:top; font-size:12px;">
                        Comentarios
                    </td>
                    <td  style="width:30%; text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Width="99.5%" Height="50px"/>
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="< Longitud Máxima de Carácteres [100] >" TargetControlID="Txt_Comentarios"/>    
                    </td>                                                      
                </tr>     
                <tr>
                    <td  style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <hr />
                    </td>                                                                
                </tr>                                                
            </table>             
            
           
          <cc1:TabContainer ID="Contenedor_Roles_Setup_Access" runat="server" Width="100%" 
             ActiveTabIndex="0" >
            <cc1:TabPanel HeaderText="Lista Roles" ID="Tab_Listar_Roles" runat="server">
                <HeaderTemplate>
                    Roles
                </HeaderTemplate>
              <ContentTemplate>
                <table style="width:100%;">
                  <tr>
                    <td align="center">
                      <asp:GridView ID="Tbl_Lista_Roles_Sistema" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" Width="100%" onselectedindexchanged="Tbl_Lista_Roles_Sistema_SelectedIndexChanged" 
                        onpageindexchanging="Tbl_Lista_Roles_Sistema_PageIndexChanging" AllowPaging="True" PageSize="10"
                        CssClass="GridView_1">
			              <RowStyle CssClass="GridItem" />
                          <PagerStyle CssClass="GridHeader" />
                          <SelectedRowStyle CssClass="GridSelected" />
                          <HeaderStyle CssClass="GridHeader" />                                
                          <AlternatingRowStyle CssClass="GridAltItem" /> 
                                                    
                          <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="../imagenes/gridview/blue_button.png"  
                                    HeaderText="">
                                <ItemStyle Width="5%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="ROL_ID" HeaderText="Rol ID">
                                <HeaderStyle Width="10%"  HorizontalAlign="Left"/>
                                <ItemStyle Width="10%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                <HeaderStyle Width="90%" HorizontalAlign="Left"/>
                                <ItemStyle Width="90%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Comentarios" >
                                <HeaderStyle Width="0%" HorizontalAlign="Left"/>
                                <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="GRUPO_ROLES_ID" HeaderText="Grupo" >
                                <HeaderStyle Width="0%" HorizontalAlign="Left"/>
                                <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                            </asp:BoundField>                            
                          </Columns>                                                      
                      </asp:GridView>
                    </td>
                  </tr>
                </table>
              </ContentTemplate>
            </cc1:TabPanel> 
            <cc1:TabPanel ID="Listar_Accesos" HeaderText="Configuración Accesos" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="Grid_Menus" runat="server" CssClass="GridView_Nested"
                         AutoGenerateColumns="False"  GridLines="None"  DataKeyNames="MENU_ID"                          
                         OnRowDataBound="Grid_Menus_RowDataBound">
                         
                             <SelectedRowStyle CssClass="GridSelected_Nested" />
                             <PagerStyle CssClass="GridHeader_Nested" />
                             <HeaderStyle CssClass="GridHeader_Nested" />
                             <AlternatingRowStyle CssClass="GridAltItem_Nested" />   
                                                             
                             <Columns>
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
                                 <asp:BoundField DataField="MENU_ID" HeaderText="Menú ID">
                                     <HeaderStyle Width="0%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="true"/>
                                     <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="false"/>
                                 </asp:BoundField>
                                 <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                     <HeaderStyle Width="44%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="true"/>
                                     <ItemStyle Width="44%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="false"/>
                                 </asp:BoundField>
                                <asp:TemplateField HeaderText="Habilitar">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_Habilitar" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Habilitar_CheckedChanged" ToolTip='<%# Eval("MENU_ID") %>'/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="56%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                </asp:TemplateField>                         
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        </td>
                                     </tr> 
                                     <tr>
                                        <td colspan="100%">
                                         <div id="div<%# Eval("MENU_ID") %>" style="display:none;position:relative;left:25px;" >                                                     
                                                <asp:GridView ID="Grid_Submenus" runat="server" CssClass="GridView_Nested"
                                                     AutoGenerateColumns="False"  GridLines="None">
                                                     
                                                         <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                         <PagerStyle CssClass="GridHeader_Nested" />
                                                         <HeaderStyle CssClass="GridHeader_Nested" />
                                                         <AlternatingRowStyle CssClass="GridAltItem_Nested" />                                                         
                                                         <Columns>
                                                            <asp:BoundField DataField="MENU_ID" HeaderText="SubMenú" 
                                                                Visible="True">
                                                                <HeaderStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                                Visible="True">
                                                                <HeaderStyle  Width="48.5%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="48.5%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Habilitar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Habilitar" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Habilitar_Interno_CheckedChanged" ToolTip='<%# Eval("MENU_ID") %>'/>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:TemplateField>                                                              
                                                            <asp:TemplateField HeaderText="Alta">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Alta" runat="server"/>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:TemplateField>    
                                                            <asp:TemplateField HeaderText="Cambio">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Cambio" runat="server"/>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:TemplateField>                
                                                            <asp:TemplateField HeaderText="Eliminar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Eliminar" runat="server"/>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:TemplateField>  
                                                            <asp:TemplateField HeaderText="Consultar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Consultar" runat="server"/>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                                                <ItemStyle  Width="11%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false"/>
                                                            </asp:TemplateField>                                                                                                                                                                            
                                                         </Columns>
                                                </asp:GridView>                                                   
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                 </asp:TemplateField>
                             </Columns>
                    </asp:GridView>
              </ContentTemplate>
            </cc1:TabPanel>        
          </cc1:TabContainer>                       
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

