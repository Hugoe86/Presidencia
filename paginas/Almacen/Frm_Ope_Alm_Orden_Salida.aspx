<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Ope_Alm_Orden_Salida.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Ordenes_Compra" Title="Mostrar Órdenes Salida" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

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
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
        <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
            <ContentTemplate>
         <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
             <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
              </ProgressTemplate>
         </asp:UpdateProgress>
           <!--Div de Contenido -->
           <div id="Div_Contenido" style="width: 97%; height: 100%;">
            <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td class="label_titulo" colspan="2">Órdenes de Salida</td>
                </tr>
                <!--Bloque del mensaje de error-->
                <tr>
                    <td>
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;">
                                <tr>
                                    <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                        <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                        Width="24px" Height="24px" />
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                        <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" Width="100%"/>
                                    </td>
                                 </tr> 
                            </table>                   
                        </div>
                    </td>
                </tr>
                <!--Bloque de la busqueda-->
                <tr class="barra_busqueda">
                    <td style="width:20%;" colspan="2">
                        <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="NUEVO" 
                                 ToolTip="Exportar PDF" 
                                 OnClick="Btn_Imprimir_Click" Visible="false"/> 
                                  
                                <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                 Width="24px" CssClass="Img_Button" 
                                 AlternateText="Imprimir Excel" 
                                 OnClick="Btn_Imprimir_Excel_Click" 
                                 ToolTip="Exportar Excel" Visible="false"
                                 /> 
                                 <asp:ImageButton ID="Btn_Print_PDF_Ordenes_Salida" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="NUEVO" 
                                 ToolTip="Exportar PDF Productos Entregados" 
                                 OnClick="Btn_Print_PDF_Ordenes_Salida_Click" Visible="true"/> 
                        <asp:ImageButton ID="Btn_Salir" runat="server"
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                             AlternateText="Salir"
                             ToolTip="Salir"
                             onclick="Btn_Salir_Click"/>
                    </td>                                     
                </tr>
                
                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                     TargetControlID="Txt_No_OrdenS_Buscar" InvalidChars="<,>,&,',!," 
                     FilterType="Numbers" 
                     Enabled="True">   
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                 WatermarkText="<No. Orden Salida>"
                                TargetControlID="Txt_No_OrdenS_Buscar" />
                                
                             <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                             TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Requisición>"
                                TargetControlID="Txt_Req_Buscar" />
                
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                             TargetControlID="Txt_OrdenC_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Orden Compra>"
                                TargetControlID="Txt_OrdenC_Buscar" />


                <div id="Div_Busqueda_Av" runat="server" style="width: 97%; height: 100%;"> 
                <tr>
                   <%-- <td colspan="2">&nbsp;</td>--%>
                     <table width="96%">
                     <tr>
                            <td align="left" style="width:18%;">
                               <asp:CheckBox ID="Chk_Dependencia_B" runat="server" Text="Unidad Responsable" AutoPostBack="true"
                                  oncheckedchanged="Chk_Dependencia_B_CheckedChanged"/>
                           </td>
                            <td align="left" style="width:60%;">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="355px" AutoPostBack="true"
                                 Enabled="False" onselectedindexchanged="Cmb_Dependencia_SelectedIndexChanged" >
                                </asp:DropDownList>
                                 &nbsp;&nbsp;&nbsp;&nbsp;                                 
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_No_OrdenS_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width:10%;">
                              <asp:CheckBox ID="Chk_Area_B" runat="server" Text="Área" AutoPostBack="true" Visible="false"
                                oncheckedchanged="Chk_Area_B_CheckedChanged"/>
                             
                               <asp:CheckBox ID="Chk_Tipo_Salida_B" runat="server" Text="Tipo Requisición" AutoPostBack="true"
                                  oncheckedchanged="Chk_Tipo_Salida_B_CheckedChanged"/>
                                &nbsp;&nbsp;&nbsp;                                
                                
                           </td>
                            <td align="left" style="width:60%;">
                                <asp:DropDownList ID="Cmb_Area" runat="server" Width="355px" Visible="false"
                                    Enabled="False" >
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Tipo_Salida" runat="server" Width="355px" Enabled="False"  >
                                </asp:DropDownList>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_OrdenC_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        
                        
                         <tr>
                            <td align="left">
                                <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                    &nbsp;&nbsp;   
                             </td>                         
                             <td align="left" style="width: 10%;">
                                 &nbsp;<asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                                 <asp:ImageButton ID="Img_Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                     Enabled="False" ToolTip="Seleccione la Fecha Inicial" />
                                 &nbsp;&nbsp;&nbsp;&nbsp;
                                 <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicio_CalendarExtender" OnClientShown="calendarShown"
                                     runat="server" TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Img_Btn_Fecha_Inicio"
                                     Format="dd/MMM/yyyy">
                                 </cc1:CalendarExtender>
                                 <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                                 <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                     Enabled="False" ToolTip="Seleccione la Fecha Final" />
                                 <cc1:CalendarExtender ID="Img_Btn_Fecha_Fin_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Fin"
                                     PopupButtonID="Img_Btn_Fecha_Fin" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                                 </cc1:CalendarExtender>
                            </td>
                            <td align="left" >                                 
                                <asp:TextBox ID="Txt_Req_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>                              
                            </td>
                        </tr>
                        <tr>
                            <td colspan = "2">
                                
                            </td>
                            <td>
                                 <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                     OnClick="Btn_Buscar_Click" ToolTip="Buscar" AlternateText="CONSULTAR" />
                                 &nbsp;<asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                     OnClick="Btn_Limpiar_Click" ToolTip="Limpiar" />                              
                            </td>
                        </tr>
                     </table>
                </tr>
              </div >

                <tr>
                     <td  colspan="2"> 
                        <table  border="0" width="100%" cellspacing="0" class="estilo_fuente">                            
                           
                             <tr>
                                <td>
                                    <div id="Div_Requisiciones" runat="server" style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                                       <asp:GridView ID="Grid_Requisiciones" runat="server" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                        onpageindexchanging="Grid_Requisiciones_PageIndexChanging" 
                                        onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" Width="98%" Height="98%" 
                                           CellPadding="1" PageSize="1" style="white-space:normal;" AllowPaging="false">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="NO_ORDEN_SALIDA" HeaderText="No. Salida" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NO_REQUISICION" HeaderText="RQ">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TIPO_REQUISICION" HeaderText="Tipo" >
                                                <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" 
                                                Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AREA" HeaderText="Área" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left"  Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CODIGO_PROGRAMATICO" 
                                                HeaderText="Código Programático" Visible="False">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EMPLEADO_SURTIO" HeaderText="Entregó" Visible="false" >
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SURTIDO" HeaderText="F.Surtido" 
                                                DataFormatString="{0:dd/MMM/yyyy}" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="10%" Font-Size="X-Small" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SUBTOTAL" HeaderText="Subtotal" 
                                                Visible="false" DataFormatString="{0:c}">
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                <ItemStyle HorizontalAlign="Center" Width="11%" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IVA" HeaderText="  IVA  "  DataFormatString="{0:c}" Visible="false">
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="11%" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TOTAL" HeaderText="  Total"  DataFormatString="{0:c}" >
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                        </Columns>
                                       <%-- <PagerStyle CssClass="GridHeader" />--%>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                    </div>
                                 </td>
                               </tr> 
                             
                       </table>
                     </td>
                  </tr>
                
                    <tr>
                        <td colspan = "2">
                    
                            <div id="Div_Detalles_Requisicion" runat="server" visible="false" >
                                 <table  border="0" width="100%" cellspacing="0" class="estilo_fuente"> <%--Fin de la tabla General--%>
                                    <tr>
                                        <td colspan ="4">
                                            <center >Detalles Requisición</center>
                                        </td>
                                     </tr>
                                    <tr align="right" class="barra_delgada">
                                        <td colspan ="4" align="center"></td>
                                    </tr>
                                    <tr>
                                        <td colspan ="4">
                                         <br /> 
                                        </td>
                                      </tr>
                                    <tr>
                                     <td  style="width:20%">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Requisición
                                      </td>
                                      <td style="width:30%" >
                                            <asp:TextBox    ID="Txt_Numero_Requisicion" runat="server" Width="80%" 
                                                ReadOnly="True" Enabled="False"></asp:TextBox>
                                      </td>
                                      <td  style="width:20%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Orden Salida</td>
                                           <td style="width:30%">
                                                <asp:TextBox    ID="Txt_Orden_Salida" runat="server" Width="80%" ReadOnly="True" 
                                                    Enabled="False"></asp:TextBox>
                                            </td>
                                      
                                      
                                      
                                      
                                      
                                      
                                     </tr>
                                    <tr>
                                        <td  style="width:20%">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Unidad Responsable
                                        </td>
                                        <td style="width:30%" colspan="3">
                                            <asp:TextBox    ID="Txt_Dependencia" runat="server" Width="92%" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                     <tr style="display:none;">
                                        <td  style="width:20%">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subtotal</td>
                                        <td style="width:30%">
                                            <asp:TextBox    ID="Txt_Subtotal" runat="server" Width="80%" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>                                     
                                     </tr>
                                    <tr style="display:none;">
                                        <td style="width:20%" >
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Area" runat="server" Width="80%" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                        <td  style="width:20%">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td style="width:30%">
                                            <asp:TextBox    ID="Txt_Iva" runat="server" Width="80%" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                    <tr>
                                            <td style="width:20%" >
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Usuario&nbsp;Entregó</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="Txt_Usuario_Surtio" runat="server" Width="92%" ReadOnly="True" 
                                                    Enabled="False"></asp:TextBox>
                                            </td>
                                                                                                                                                                                                                          
                                    </tr>
                                    <tr>
                                            <td style="width:20%" >
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fecha de Entrega
                                            </td>
                                            <td>
                                                    <asp:TextBox ID="Txt_Fecha_Surtio" runat="server" Width="80%" Rows="2" 
                                                        ReadOnly="True" Enabled="False" 
                                                    ></asp:TextBox>
                                            </td>
                                            <td  style="width:20%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total
                                            </td>
                                            <td style="width:30%">
                                                <asp:TextBox    ID="Txt_Total" runat="server" Width="80%" ReadOnly="True" 
                                                    Enabled="False"></asp:TextBox>
                                            </td>                                             
                                    </tr>
                                    <tr style="display:none;">
                                        <td>
                                           <td  style="width:20%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                           <td style="width:30%">
                                                <asp:TextBox    ID="Txt_Codigo_P" runat="server" Width="80%" ReadOnly="True" 
                                                    Visible="False"></asp:TextBox>
                                            </td>                                        
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center"> 
                           <table  border="0" width="100%" cellspacing="0" class="estilo_fuente">
                             <div id="Div_Productos_Requisicion" runat="server" visible="false" >  <center > 
                                 <caption>
                                     Productos Requisición</caption>
                                 </center> 
                                <tr align="right" class="barra_delgada" >
                                    <td align="center"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                      <td >
                                        <asp:GridView ID="Grid_Productos" runat="server" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                        onpageindexchanging="Grid_Productos_PageIndexChanging" 
                                        PageSize="5" Width="98%" Height="98%" CellPadding="1" style="white-space:normal;">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" 
                                                Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" 
                                                Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" Visible="true">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CANTIDAD_SOLICITADA" HeaderText="Solicitado" 
                                                Visible="true">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" Font-Size="X-Small" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CANTIDAD_ENTREGADA" HeaderText="Entregado">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" Font-Size="X-Small" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MARCA" HeaderText="Marca" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" Visible="False" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COSTO" HeaderText="Costo" 
                                                Visible="false" DataFormatString="{0:c}">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SUBTOTAL" HeaderText="Subtotal" Visible="false"
                                                DataFormatString="{0:c}">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IMPORTE" HeaderText="Total" 
                                                Visible="True" DataFormatString="{0:c}">
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                      </td>
                                </tr>
                              </div>   
                           </table> 
                        </td>
                   </tr>
              </table>          
           </div>
            </ContentTemplate>
        </asp:UpdatePanel>
  
      
         
</asp:Content>