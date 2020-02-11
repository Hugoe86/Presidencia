<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Registro_Contratos.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Registro_Contratos" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        function pageLoad() 
        { 
            Contar_Caracteres();
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr()
        {
            document.getElementById("<%=Txt_No_Compromiso_PopUp.ClientID%>").value="";
            document.getElementById("<%=Txt_Cuenta_Contable_PopUp.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Financiamiento.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Area_Funcional.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Proyectos.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Partida.ClientID%>").value="";
            return false;
        }  
        function Abrir_Modal_Popup() 
        {
            //Limpiar_Ctlr();
            $find('Busqueda_Compromisos').show();
            return false;
        }
        function Contar_Caracteres()
        {
            $('textarea[id$=Txt_Concepto]').keyup(function() {
                var Caracteres =  $(this).val().length;
                
                if (Caracteres > 100) {
                    this.value = this.value.substring(0, 100);
                    $(this).css("background-color", "Yellow");
                    $(this).css("color", "Red");
                }else{
                    $(this).css("background-color", "White");
                    $(this).css("color", "Black");
                }
                
                $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 100 ]');
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" >
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Compromisos" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Registro de Pre-Compromisos</td>
                    </tr>
                    <tr>
                        <td >&nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>                         
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                                </ContentTemplate>                                
                            </asp:UpdatePanel>                                      
                        </td>
                    </tr> 
                </table>

                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                        CssClass="Img_Button" TabIndex="1"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                        onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                        CssClass="Img_Button" TabIndex="3"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el Compromiso seleccionado?');" 
                                                        onclick="Btn_Eliminar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="4"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="width:100%;vertical-align:top;" align="right">
                                                        B&uacute;squeda 
                                                        <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada" TabIndex="23" 
                                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                            OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
                                                        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Compromisos" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Compromisos"
                                                            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                                            CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                                        <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                                                        <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />                                                                                                    
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
                
                <asp:UpdatePanel ID="Upnl_Generales_Compromisos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>             
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos del Monto Comprometido" Width="98%" BackColor="White">
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                <td width="20%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                <td width="10%"></td>
                                </tr>
                                <tr>
                                    <td>No. Pre-Compromiso</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_No_Compromiso" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false" Visible="true"></asp:TextBox> 
                                    </td>
                                    <td colspan="2">*Estatus</td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" TabIndex="5" Width="98%" Enabled="false">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>PENDIENTE</asp:ListItem>
                                            <asp:ListItem>COMPROMETIDO</asp:ListItem>
                                            <asp:ListItem>CANCELADO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tipo de Beneficiario</td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Beneficiario_Tipo" runat="server" TabIndex="5" 
                                            Width="99%" Enabled="false" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Beneficiario_Tipo_SelectedIndexChanged">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>CONTRATISTA</asp:ListItem>
                                            <asp:ListItem>EMPLEADO</asp:ListItem>
                                            <asp:ListItem>PROVEEDOR</asp:ListItem>
                                            <asp:ListItem>OTRO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">Beneficiario</td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Beneficiario_Nombre" runat="server" TabIndex="5" Width="98%" Enabled="false" Visible="true" AutoPostBack="true"></asp:DropDownList>
                                        <asp:TextBox ID="Txt_Beneficiario_Nombre" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false" Visible="false"></asp:TextBox> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        *Cuenta Contable</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Cuenta_Contable" runat="server" AutoPostBack="true" 
                                            ontextchanged="Txt_Cuenta_Contable_TextChanged" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Cuenta_Contable" FilterType="Custom" ValidChars="1234567890"></cc1:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Txt_Cuenta_Contable_ID" runat="server" Width="98%" Visible="false"></asp:TextBox>
                                    </td>
                                    <td colspan="2">*Monto</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Monto_Comprometido" runat="server" Width="98%" Enabled="false" 
                                            AutoPostBack="True" ontextchanged="Txt_Monto_Comprometido_TextChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Monto_Comprometido" FilterType="Custom" ValidChars="1234567890."></cc1:FilteredTextBoxExtender>                                    
                                        <asp:TextBox ID="Txt_Monto_Disponible" runat="server" Width="98%" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*Concepto</td>
                                    <td colspan="8">
                                        <asp:TextBox ID="Txt_Concepto" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TWM_Txt_Concepto" runat="server" WatermarkCssClass="watermarked" 
                                            WatermarkText ="< Carácteres Permitidos 100 >" TargetControlID="Txt_Concepto"/>
                                        <span id="Contador_Caracteres_Concepto" class="watermarked"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Unidad Responsable</td>
                                    <td colspan="3">Programa</td>
                                    <td colspan="3">Fuente de Financiamiento</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" width="98%" Enabled = "false"
                                            onselectedindexchanged="Cmb_Unidad_Responsable_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" width="98%" Enabled = "false"
                                            onselectedindexchanged="Cmb_Programa_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Fte_Financiamiento" runat="server" width="98%" Enabled = "false"
                                            onselectedindexchanged="Cmb_Fte_Financiamiento_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Area Funcional</td>
                                    <td colspan="2">Partida</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="Cmb_Area_Funcional" runat="server" width="98%" 
                                            AutoPostBack="true" Enabled = "false" 
                                            onselectedindexchanged="Cmb_Area_Funcional_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Partida" runat="server" width="98%" AutoPostBack="true" Enabled = "false"></asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <p></p>
                            <%--********************************************************
                            ********************************************************
                            ********************************************************--%>
                            <asp:GridView ID="Grid_Compromisos" runat="server" AllowPaging="True" CssClass="GridView_1" 
                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="98%"
                                onpageindexchanging="Grid_Compromisos_PageIndexChanging" AutoPostBack="true"
                                HeaderStyle-CssClass="tblHead" onselectedindexchanged="Grid_Compromisos_SelectedIndexChanged1">
                                <Columns>         
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>                       
                                    <asp:BoundField DataField="No_Compromiso" HeaderText="No Pre-Compromiso" 
                                        Visible="True" SortExpression="No_Compromiso">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Codigo_Programatico" HeaderText="Codigo Programatico" 
                                        Visible="True" SortExpression="Codigo_Programatico">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                                        Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cuenta_Contable" HeaderText="Cuenta Contable" 
                                        Visible="True" SortExpression="Cuenta_Contable">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="left" Width="20%" />
                                    </asp:BoundField>                                   
                                    <asp:BoundField DataField="Monto" HeaderText="Monto" 
                                        Visible="True" SortExpression="Monto">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="left" Width="10%" />
                                    </asp:BoundField> 
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>                            
                        
                    </ContentTemplate>        
                </asp:UpdatePanel>
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
                     B&uacute;squeda: Compromisos
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Click"/>  
                </td>
            </tr>
        </table>            
        
    <%--********************************************************
        ********************************************************
        ********************************************************--%></asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Compromisos" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Compromisos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Compromisos" DisplayAfter="0">
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
                                           No de Compromiso
                                        </td>
                                        <td style="width:40%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_No_Compromiso_PopUp" runat="server" Width="98%" MaxLength="5"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Compromiso_PopUp" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_No_Compromiso_PopUp"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Compromiso_PopUp" runat="server" 
                                                TargetControlID ="Txt_No_Compromiso_PopUp" WatermarkText="Busqueda por No. de Compromiso" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                     
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Estatus
                                        </td>
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">   
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>                                         
                                                <asp:ListItem>PENDIENTE</asp:ListItem>
                                                <asp:ListItem>COMPROMETIDO</asp:ListItem>
                                            </asp:DropDownList>                                          
                                        </td>             
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Cuenta Contable
                                        </td>
                                        <td style="width:40%;text-align:left;">
                                            <asp:TextBox ID="Txt_Cuenta_Contable_PopUp" runat="server" Width="98%" MaxLength="20" AutoPostBack="true"
                                            ontextchanged="Txt_Cuenta_Contable_PopUp_TextChanged"/>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Cuenta_Contable_PopUp"/>  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>              
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Busqueda_Dependencia_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Proyectos y Programas
                                        </td>              
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Proyectos" runat="server" Width="100%" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Busqueda_Proyectos_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fuente de Financiamiento
                                        </td>              
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Financiamiento" runat="server" Width="100%" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Busqueda_Financiamiento_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Area Funcional
                                        </td>              
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Area_Funcional" runat="server" Width="100%"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Partida
                                        </td>              
                                        <td style="width:40%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Partida" runat="server" Width="100%"></asp:DropDownList>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Compromiso_Popup" runat="server"  Text="Busqueda de Polizas" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Compromiso_Popup_Click" Width="200px"/> 
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