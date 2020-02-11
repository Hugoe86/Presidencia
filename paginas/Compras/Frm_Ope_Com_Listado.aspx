<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Listado.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Listado" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <!--SCRIPT PARA LA VALIDACION QUE NO EXPiRE LA SESSION-->  
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
    function pageLoad() {
        Contar_Caracteres();
    }

    function Contar_Caracteres() {
        $('textarea[id$=Txt_Justificacion]').keyup(function() {
            var Caracteres = $(this).val().length;
            if (Caracteres > 3000) {
                this.value = this.value.substring(0, 3000);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            } else {
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 3000 ]');
        });
    }    
            
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
            
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    <ContentTemplate>
       <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                  <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
           </asp:UpdateProgress>
        
        <%--Div Contenido--%>        
        <div id="Div_Contenido" style="width:100%;">
            <table border="0" cellspacing="0" class="estilo_fuente" width="98%">
                <tr>
                    <td colspan ="4" class="label_titulo">Listado de Almacen</td>
                </tr>
                <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td colspan ="2">
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
                <%--Renglon de barra de Busqueda--%>
                <tr class="barra_busqueda">
                    <td style="width:20%">
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                        onclick="Btn_Salir_Click"/>
                    </td>
                    <td align="right" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">Busqueda
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                onclick="Btn_Avanzada_Click" ToolTip="Avanzada" style="display:none">Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" />
                        </div>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                       
                    </td>
                </tr>                
                <tr>
                    <td colspan="2" > 
                        <%--Div Grid Listado--%>
                        <div id="Div_Grid_Listado" runat="server" style="width:100%;" >
                            <asp:GridView ID="Grid_Listado" runat="server" Width="100%" 
                                AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None"
                                OnSelectedIndexChanged="Grid_Listado_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Listado_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Folio">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True" SortExpression="Fecha_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Partida" HeaderText="Partida" Visible="True" SortExpression="Partida">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" Visible="True" SortExpression="Total">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" Width="15%" Font-Size="X-Small"/>
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
                    <td colspan="2">
                    <%--Div Datos_Generales--%>
                    <div id="Div_Datos_Generales" runat="server" style="width:100%;">
                        <table  border="0" cellspacing="0" class="estilo_fuente" width="100%"> 
                            <tr >
                                <td colspan="4">
                                    &nbsp;</td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center"></td>
                            </tr>
                            <tr>
                                <td style="width:10%;">Folio</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%"></asp:TextBox>
                                </td>
                                <td style="width:10%;">Fecha</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="97%"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>Estatus</td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>Tipo</td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="100%" 
                                        onselectedindexchanged="Cmb_Tipo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                
                            </tr>
                                            <tr>
                    <td>
                        Partida
                    </td>
                    <td>
                        <asp:DropDownList ID="Cmb_Partida" runat="server" Enabled="True" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Partida_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <tr>
                           
                            <td>
                                Comentarios</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="250" TabIndex="10" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                    TargetControlID="Txt_Comentario" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                            </td>
                            <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentario" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                            </cc1:FilteredTextBoxExtender>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        Productos</td>
                                                </tr>
                                                <tr align="right" class="barra_delgada">
                                                    <td align="center" colspan="4">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Producto</td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Producto" runat="server" Width="97%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="Btn_Filtro_Productos" runat="server" 
                                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                                        onclick="Btn_Filtro_Productos_Click" />
                                                                </td>
                                                                <td align="right">
                                                                    Cantidad</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        &nbsp;<asp:TextBox ID="Txt_Cantidad" runat="server" Height="22px" Width="82%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            TargetControlID="Txt_Cantidad" ValidChars="1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:ImageButton ID="Btn_Agregar" runat="server" 
                                                            ImageUrl="~/paginas/imagenes/paginas/accept.png" onclick="Btn_Agregar_Click" 
                                                            Width="24px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <div ID="Div_Grid_Productos" runat="server" style="overflow:auto;height:500px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                                            <center>
                                                                Lista</center>
                                                            <asp:GridView ID="Grid_Productos" runat="server"
                                                                AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                                Enabled="false" GridLines="None" HeaderStyle-CssClass="tblHead" 
                                                                onselectedindexchanged="Grid_Productos_SelectedIndexChanged" 
                                                                OnSorting="Grid_Productos_Sorting" Width="100%">
                                                                <RowStyle CssClass="GridItem" />
                                                                <Columns>
                                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png" Text="Quitar">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:ButtonField>
                                                                    <asp:BoundField DataField="Producto_ID" HeaderText="Producto_ID" 
                                                                        Visible="false">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" 
                                                                        Visible="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="5%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Producto_Nombre" HeaderText="Producto" 
                                                                        SortExpression="Producto_Nombre" Visible="True">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                                        SortExpression="Descripcion" Visible="True">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" 
                                                                        SortExpression="Unidad" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Existencia" HeaderText="Existencia" 
                                                                        SortExpression="Existencia" Visible="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Reorden" HeaderText="Punto de Reorden" 
                                                                        Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad Solicitada" 
                                                                        SortExpression="Cantidad" Visible="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Precio_Unitario" HeaderText="Precio Unitario" 
                                                                        SortExpression="Precio_Unitario" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Importe" HeaderText="Importe Total" 
                                                                        SortExpression="Importe" Visible="True">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Monto_IVA" HeaderText="Monto_IVA" Visible="false">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Monto_IEPS" HeaderText="Monto_IEPS" Visible="false">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Porcentaje_IVA" HeaderText="Porcentaje_IVA" 
                                                                        Visible="false">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Porcentaje_IEPS" HeaderText="Porcentaje_IEPS" 
                                                                        Visible="false">
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
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
                                                    <td colspan="4">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        Total
                                                        <asp:TextBox ID="Txt_Total" runat="server" style="text-align:right" 
                                                            Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr align="right" class="barra_delgada">
                                                    <td colspan="4">
                                                    </td>
                                                </tr>
                                                <%--Div_Comentarios --%>
                                                <tr>
                                                    <td colspan="4">
                                                        <div ID="Div_Comentarios" runat="server" visible="false">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        Observaciones</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:GridView ID="Grid_Comentarios" runat="server" 
                                                                            AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                                                            GridLines="None" HeaderStyle-CssClass="tblHead" 
                                                                            onselectedindexchanged="Grid_Comentarios_SelectedIndexChanged" 
                                                                            OnSorting="Grid_Comentarios_Sorting" Width="95%">
                                                                            <RowStyle CssClass="GridItem" />
                                                                            <Columns>
                                                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver">
                                                                                    <ItemStyle Width="5%" />
                                                                                </asp:ButtonField>
                                                                                <asp:BoundField DataField="Comentario" HeaderText="Comentarios" Visible="True">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                                                                                    SortExpression="Estatus" Visible="True">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" 
                                                                                    SortExpression="Fecha_Creo" Visible="True">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" 
                                                                                    SortExpression="Usuario_Creo" Visible="True">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                                                </asp:BoundField>
                                                                            </Columns>
                                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                                            <PagerStyle CssClass="GridHeader" />
                                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                        </table>
                        
                    </div>
                    </td>
                </tr>
                </table>
        </div>
        <%--Termina Div Contenido--%>   
     </ContentTemplate>
     <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Realizar_Busqueda" EventName="Click"/>
          </Triggers>
    </asp:UpdatePanel>
    
    
    <asp:UpdatePanel ID="UPnl_Busqueda" runat ="server" UpdateMode="Conditional" >
        <ContentTemplate>               
                        <cc1:ModalPopupExtender ID="Modal_Productos" runat="server"
                            TargetControlID="Btn_Comodin_2"
                            PopupControlID="Pnl_Busqueda_Contenedor"              
                            Enabled="True"         
                            CancelControlID="Btn_Comodin_Close"
                            PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                            DynamicServicePath="" 
                            DropShadow="True"
                            BackgroundCssClass="progressBackgroundFilter"/>   
                        <asp:Button ID="Btn_Comodin_2" runat="server" Text="Button" style="display:none;" />   
                        <asp:Button ID="Btn_Comodin_Close" runat="server" Text="Button" style="display:none;" />                     
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="Upnl_Busqueda_Avanzada" runat ="server" UpdateMode="Conditional" >
        <ContentTemplate>
         
            <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
            TargetControlID="Btn_Comodin"
            PopupControlID="Pnl_Busqueda"                      
            CancelControlID="Btn_Comodin_Close2"
            PopupDragHandleControlID="Pnl_Cabecera_Bus_Avanzada" 
            DynamicServicePath="" 
            DropShadow="True"
            BackgroundCssClass="progressBackgroundFilter"/>
            <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/>
            <asp:Button ID="Btn_Comodin_Close2" runat="server" Text="Button" style="display:none;" />
         </ContentTemplate>
    </asp:UpdatePanel>
    
    <%-- Panel del ModalPopUp--%>
    
       <asp:Panel ID="Pnl_Busqueda" runat="server" Width="100%" 
        style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;display:none">
            <asp:Panel ID="Pnl_Cabecera_Bus_Avanzada" runat="server" Width="99%" 
                style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                        <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                            Busqueda Avanzada
                        </td>
                        <td align="right">
                        <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Click"/>  
                        </td>
                    </tr>
                </table>
         </asp:Panel>              
           <center>
           <asp:UpdatePanel ID="pnlPanel" runat="server">
           <ContentTemplate>
              <table class="estilo_fuente" width="99%">
              <tr>
                    <td colspan="4">
                        <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                    </td>
              </tr>
              <tr>
                    <td align="left" width="20%">
                        <asp:CheckBox ID="Chk_Fecha" runat="server" Text="Fecha" 
                        oncheckedchanged="Chk_Fecha_CheckedChanged" AutoPostBack="true"/>
                        &nbsp;De</td>
                    <td align="left" width="35%">
                        <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="80%" 
                        Enabled="False"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                            Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                            PopupButtonID="Btn_Fecha_Inicio" TargetControlID="Txt_Fecha_Inicial" />
                    </td>
                    <td align="left" width="10%">
                        Al</td>
                    <td align="left" width="35%">
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="80%" Enabled="False"></asp:TextBox>
                       <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Fin_Calendar" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Fecha_Final" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Estatus" AutoPostBack="true"
                        oncheckedchanged="Chk_Estatus_CheckedChanged"/></td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="50%" 
                        Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left" colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <center>
                        <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" 
                        onclick="Btn_Aceptar_Click" CssClass="button"/>
                        &nbsp;&nbsp;&nbsp;
                        </center>
                    </td>
                </tr>
                </table>
                </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Avanzada" EventName="Click"/>
          </Triggers>    
        </asp:UpdatePanel> 
        </center>
        </asp:Panel>
        
        
        
        
<%-- Panel del ModalPopUp Pnl_Busqueda_Productos display:none;--%>
<asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center" Width="100%" Height="100%" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;" Width="99%">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Buscar Productos
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Click"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>   
           <center>
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
            <table width="99%" class="estilo_fuente">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Lbl_Mensaje_Error_Productos" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                    <tr>
                        <td>
                            Nombre Producto
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Enabled="True" 
                                Width="80%"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Realizar_Busqueda" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                OnClick="Btn_Realizar_Busqueda_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </tr>
                </table>
               </ContentTemplate> 
           </asp:UpdatePanel> 
           
           
           <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <table width="99%" class="estilo_fuente">
                <tr>
                    <td>
                    <div ID="Div_1" runat="server" 
                        style="overflow:auto;height:250px;width:98%;vertical-align:top;border-style:outset;border-color:Silver;">
                        <asp:GridView ID="Grid_Productos_Busqueda" runat="server"
                            AutoGenerateColumns="False" CssClass="GridView_1" 
                            DataKeyNames="Producto_Servicio_ID" GridLines="None" 
                            OnSelectedIndexChanged="Grid_Productos_Busqueda_SelectedIndexChanged" 
                            Width="98%"
                            AllowSorting="True" OnSorting="Grid_Productos_Busqueda_Sorting" HeaderStyle-CssClass="tblHead">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Clave" HeaderText="Clave" 
                                    Visible="True" SortExpression="Clave">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Producto_Servicio" HeaderText="Producto" 
                                    Visible="True" SortExpression="Producto_Servicio">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"
                                    Visible="True">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad"
                                    Visible="True">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Existencia" HeaderText="Existencia" Visible="True" SortExpression="Existencia">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Precio_Unitario" HeaderText="Precio Unitario" SortExpression="Precio_Unitario"
                                    Visible="True">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Producto_Servicio_ID" 
                                    HeaderText="Producto_Servicio_ID" Visible="False">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                </asp:BoundField>
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
           <Triggers>
                <asp:AsyncPostBackTrigger  ControlID="Btn_Realizar_Busqueda" EventName="Click"/>                            
                <asp:AsyncPostBackTrigger  ControlID="Btn_Filtro_Productos" EventName="Click"/>
           </Triggers>    
           </asp:UpdatePanel> 
           </center>
        </asp:Panel>  
        
        
</asp:Content>
