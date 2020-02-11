<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pat_Com_Donadores.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Pat_Com_Donadores" Title="Donadores" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

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
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  AsyncPostBackTimeout="36000" EnableScriptLocalization="true" EnableScriptGlobalization="true"/> 
    
        <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>
            <div id="Div_General" style="background-color:#ffffff; width:98%; height:100%;"> <%--Div General--%>
                <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%"> <%-- Tabla General--%>
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Donadores</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td align="left" colspan="4">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;" >              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                             </table>                   
                          </div>                          
                        </td>
                    </tr> 
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" >
                        <td align="left" style="width:20%;" >
                             <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button"
                                 AlternateText="Imprimir PDF" 
                                 ToolTip="Exportar PDF"
                                 OnClick="Btn_Imprimir_Click" Visible="false"/>   
                                 
                             <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                               ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                               AlternateText="Imprimir Excel" 
                               ToolTip="Exportar Excel"
                               OnClick="Btn_Imprimir_Excel_Click" Visible="false"/>  
                               
                             <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" 
                                ToolTip="Salir"
                                onclick="Btn_Salir_Click"/>
                        </td>
                         <td align="right" style="width:90%;">
                            <div id="Div_Busqueda" runat="server">
                            <asp:Label ID="Lbl_Busqueda" runat="server" Text="Búsqueda "></asp:Label>
                            <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="150px"></asp:TextBox>
                            
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Inicial" runat="server" 
                                        Enabled="True" FilterType="Custom"  InvalidChars="&lt;,&gt;,',!," 
                                        TargetControlID="Txt_Busqueda" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789">
                            </cc1:FilteredTextBoxExtender>
                                    
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el Nombre del Donador>"
                                TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" ToolTip="Consultar" />
                           </div>
                        </td> 
                    </tr>
                    <!-- Modal popup-->
<%--                    <div>
                        <tr>
                            <td>
                                <cc1:ModalPopupExtender ID="Modal_Login" runat="server"
                                    TargetControlID="Btn_MP_Login"
                                    PopupControlID="Pnl_Login"                      
                                    CancelControlID="Btn_Cancelar_Login"
                                    DropShadow="True"
                                    BackgroundCssClass="progressBackgroundFilter"/>
                                <asp:Button ID="Btn_MP_Login" runat="server" Text="Button" style="display:none;"/>
                            </td>
                        </tr>                    
                    </div>--%>
                    <tr>
                         <td colspan="2">
                           <div id="Div_Donadores" visible="true" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" >    
                              <table width="100%">     
                                <tr>
                                    <td > 
                                    <asp:Label ID="Lbl_Donadores" runat="server" CssClass="estilo_fuente" 
                                            Visible="true" >Listado de Donadores</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                 <asp:GridView ID="Grid_Donadores" runat="server" AllowPaging="True" 
                                 AutoGenerateColumns="False" CellPadding="4" CssClass="GridView_1" 
                                 ForeColor="#333333" GridLines="None" Height="98%" style="white-space:normal;"
                                 OnSelectedIndexChanged="Grid_Donadores_SelectedIndexChanged" 
                                 PageSize="5" Width="98%" Visible="False" 
                                        onpageindexchanging="Grid_Donadores_PageIndexChanging">
                                 <RowStyle CssClass="GridItem" />
                                 <Columns>
                                     <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                         ImageUrl="~/paginas/imagenes/gridview/blue_button.png" />
                                     <asp:BoundField DataField="DONADOR_ID" HeaderText="Donador" 
                                         SortExpression="DONADOR_ID">
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle Width="110px" HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="NOMBRE_COMPLETO" 
                                         HeaderText="Nombre" SortExpression="NOMBRE_COMPLETO">
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle Width="180px" HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="CURP" HeaderText="CURP" SortExpression="CURP" >
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" >
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="DIRECCION" HeaderText="Dirección" 
                                         SortExpression="DIRECCION" >
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="CIUDAD" HeaderText="Ciudad" 
                                         SortExpression="CIUDAD" >
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Left" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="ESTADO" HeaderText="Estado" 
                                         SortExpression="ESTADO" >
                                         <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Left" />
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
                         </td>
                     </tr>
                    <tr>
                        <td colspan="4">
                           <div id="Div_Productos_Donados" runat="server" visible="false" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;">  <%--Div Productos Donados--%>
                              <table  border="0" width="100%" cellspacing="0" class="estilo_fuente"> <%--Tabla Datos Geenrales--%>
                                <tr>
                                    <td class="label_titulo" colspan="4">Datos Generales</td>
                                </tr>
                                <tr align="right" class="barra_delgada" >
                                    <td colspan="4" ></td>
                                </tr>
                                <tr>
                                   <td> &nbsp; </td>
                                </tr>
                                <tr >
                                    <td style="width: 15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Donador</td>
                                    <td colspan="3">
                                        <asp:TextBox    ID="Txt_Nombre_Donador" runat="server" Width="99%" 
                                         ReadOnly="True" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Dirección</td>
                                    <td colspan="3">
                                        <asp:TextBox    ID="Txt_Direccion" runat="server" Width="99%" 
                                            ReadOnly="True" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Ciudad</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Ciudad" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Estado</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Estado" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                               </tr>
                                <tr>
                                    <td style="width: 15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CURP</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Curp" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RFC</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_RFC" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                               </tr>
                                <tr>
                                    <td style="width: 15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Teléfono&nbsp;
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Telefono" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%" >
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Celular</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Celular" runat="server" Width="98%" ReadOnly="True" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                               </tr>
                            
                                  <table width="100%"> <%-- Tabla Detalles--%>
                                    <tr>
                                        <td>
                                        &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_titulo">Detalles</td>
                                    </tr>
                                    <tr align="right" class="barra_delgada">
                                        <td align="center"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                     </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Productos" runat="server" CssClass="estilo_fuente" 
                                                Visible="true"> Productos Donados</asp:Label>   
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="Grid_Productos_Donados" runat="server" 
                                            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                            GridLines="None" Width="98%" style="white-space:normal;" Height="98%"
                                            PageSize="100" 
                                                RowStyle-HorizontalAlign="Center" CssClass="GridView_1" 
                                                AllowPaging="True" 
                                                onpageindexchanging="Grid_Productos_Donados_PageIndexChanging" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="NUMERO_INVENTARIO" HeaderText="No. Inventario" 
                                                    SortExpression="NUMERO_INVENTARIO" FooterStyle-CssClass="text_cantidades_grid" 
                                                    ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="110px"  />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FECHA_ADQUISICION" HeaderText="Fecha Adquisición" 
                                                    SortExpression="FECHA_ADQUISICION"  DataFormatString="{0:dd/MMM/yyyy}" >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"  Width="120px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Descripción del Bien" 
                                                    SortExpression="NOMBRE">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"  />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />                                
                                            <AlternatingRowStyle CssClass="GridAltItem" />                                
                                        </asp:GridView>     
                                        </td>
                                     </tr>
                                  </table> <%--Fin de la tabla Detalles--%>
                              </table> <%-- Fin Tabla Datos Generales--%>
                           </div>  <%--Fin Div Productos Donados--%>
                         </td>
                    </tr>
                </table>    <%--Fin de la tabla General--%>
            </div> <%-- Fin del Div General--%>
            <br />
            <br />
            <br />
            <br />
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

