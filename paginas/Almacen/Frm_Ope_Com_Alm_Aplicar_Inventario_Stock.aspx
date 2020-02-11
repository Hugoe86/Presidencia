<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Alm_Aplicar_Inventario_Stock" Title="Aplicar Inventario Stock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
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
                <table  border="0" cellspacing="0" class="estilo_fuente" frame="border"  
                    width="100%"> <%-- Tabla General--%>
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Aplicar Inventario</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td style="font-size:9px;width:70%;text-align:left;" valign="top">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="font-size:9px;width:70%;text-align:left;" valign="top"> 
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />      
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
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
                        <td align="left" style="width:20%;">
                             <asp:ImageButton ID="Btn_Aplicar" runat="server" 
                                 ImageUrl="~/paginas/imagenes/gridview/grid_docto.png" Width="24px" CssClass="Img_Button"
                              ToolTip="Aplicar Inventario"   AlternateText="NUEVO" 
                                 OnClientClick="return confirm('¿Está seguro de aplicar el inventario?');"
                                 OnClick="Btn_Aplicar_Click" Visible="false"/>   
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                AlternateText="Salir"
                                ToolTip= "Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                Width="24px" 
                                CssClass="Img_Button"
                                onclick="Btn_Salir_Click"/>
                                
                                 <asp:ImageButton ID="Btn_Cancelar_Proceso" runat="server" Visible="false" 
                                ToolTip= "Cancelar Proceso"
                                ImageUrl="~/paginas/imagenes/paginas/delete.png" 
                                Width="24px" 
                                CssClass="Img_Button"
                                OnClientClick = "return confirm('¿Está seguro de terminar el proceso?');"
                                onclick="Btn_Cancelar_Proceso_Click"/>
                                
                        </td>
                        <td align="right" style="width:80%;">
                             <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                              ToolTip="AVANZADA" onclick="Btn_Busqueda_Avanzada_Click">B&uacute;squeda</asp:LinkButton>
                              &nbsp;&nbsp;
                              <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="150px"></asp:TextBox>
                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                         TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                         FilterType="Numbers" 
                                        Enabled="True">   
                               </cc1:FilteredTextBoxExtender>
                               <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el No. Inventario>"
                                TargetControlID="Txt_Busqueda" />
                               <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" ToolTip="Buscar" />
                           </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="Div_Inventarios_Capturados" visible="true" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" >    
                               <table width="99%"> 
                                  <tr>
                                        <td> 
                                        &nbsp;<asp:Label ID="Lbl_Inventarios" runat="server" CssClass="estilo_fuente" 
                                                Visible="False" >Inventarios</asp:Label>
                                        &nbsp;</td>
                                   </tr>
                                  <tr>
                                        <td >
                                            <asp:GridView ID="Grid_Inventario_Capturados" runat="server" AllowPaging="True" style="white-space:normal;"
                                         AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                         ForeColor="#333333" GridLines="None" 
                                         OnSelectedIndexChanged="Grid_Inventario_Capturados_SelectedIndexChanged" 
                                                Width="99%" 
                                                onpageindexchanging="Grid_Inventario_Capturados_PageIndexChanging" 
                                                PageSize="8">
                                         <RowStyle CssClass="GridItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver"/>
                                             <asp:BoundField DataField="NO_INVENTARIO" HeaderText="No. Inventario" 
                                                 SortExpression="NO_INVENTARIO">
                                                 <ItemStyle Width="110px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                                 HeaderText="Fecha" SortExpression="FECHA">
                                                 <ItemStyle Width="110px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario Creó" 
                                                 SortExpression="USUARIO_CREO" />
                                             <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                                 SortExpression="ESTATUS" />
                                             <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" 
                                                 SortExpression="OBSERVACIONES" Visible="False" />
                                         </Columns>
                                         <PagerStyle CssClass="GridHeader" />
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <HeaderStyle CssClass="GridHeader" />
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                     </asp:GridView>
                                            </td> 
                                    </tr>  
                                  <tr>
                                     <td>
                                     &nbsp;</td>
                                   </tr>
                               </table>
                            </div>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                             <div id="Div_Productos_Inventario" runat="server" visible="false">  <%--Fin Div Capturar inventario--%>
                                 <table  border="0" width="99%" cellspacing="0" class="estilo_fuente"> <%--Fin de la tabla General--%>
                                   <tr>
                                        <td>
                                         &nbsp;
                                        </td>
                                    </tr>
                                   <tr>
                                        <td  style="width:191px">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Fecha
                                        </td>
                                        <td >
                                            <asp:TextBox    ID="Txt_Fecha_Creo" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                   <tr>
                                      <td style="width:191px" >
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Usuario
                                      </td>
                                      <td>
                                            <asp:TextBox ID="TxtUsuario_Creo" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                       </td>
                                    </tr>
                                   <tr>
                                        <td style="width:191px" >
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            Estatus&nbsp;
                                        </td>
                                        <td >
                                            <asp:TextBox ID="Txt_Estatus" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                   </tr>
                                   <tr>
                                        <td style="width:191px" >
                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                       Observaciones
                                        </td>
                                         <td>
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Width="400px" Rows="2" 
                                                TextMode="MultiLine" ReadOnly="True" Height="48px" Enabled="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                        
                                    <table width="99%"> <%-- Fin del Div General--%>
                                       <tr>
                                            <td>
                                            &nbsp
                                            </td>
                                        </tr>
                                       <tr align="right" class="barra_delgada">
                                            <td align="center"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lbl_No_Inventario" runat="server" CssClass="estilo_fuente" 
                                                    Visible="False" />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                          <td>
                                               <div id="Div_DataGrid" visible="true" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" > 
                                                 <table width="100%"> <%-- Fin del Div General--%>
                                                    <tr>
                                                        <td>
                                                             <asp:GridView ID="Grid_Aplicar_Inventario" runat="server" style="white-space:normal;"
                                                                AutoGenerateColumns="False" CellPadding="1" ForeColor="#333333" 
                                                                GridLines="None" Width="99%" 
                                                                RowStyle-HorizontalAlign="Center" CssClass="GridView_1" AllowPaging="True" 
                                                                onpageindexchanging="Grid_Aplicar_Inventario_PageIndexChanging" >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_Id" 
                                                        SortExpression="PRODUCTO_ID" Visible="False">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                                        SortExpression="CLAVE" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PRODUCTO" 
                                                        HeaderText="Producto">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTADOS_SISTEMA" HeaderText="C. Sistema" 
                                                        SortExpression="CONTADOS_SISTEMA" ItemStyle-HorizontalAlign="Right">
                                                        <FooterStyle CssClass="textcantidades" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle HorizontalAlign="Right" Width="80px"  CssClass="textcantidades"/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTADOS_USUARIO" HeaderText="C.  Usuario" 
                                                        SortExpression="CONTADOS_USUARIO" FooterStyle-CssClass="textcantidades" 
                                                        ItemStyle-HorizontalAlign="Right">
                                                    <FooterStyle CssClass="textcantidades"></FooterStyle>

                                                        <HeaderStyle HorizontalAlign="Center" />

                                                        <ItemStyle HorizontalAlign="Right" Width="80px" CssClass="textcantidades" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DIFERENCIA" HeaderText="Diferencia" 
                                                        SortExpression="DIFERENCIA" FooterStyle-CssClass="textcantidades" ItemStyle-HorizontalAlign="Right">
                                                      <FooterStyle CssClass="textcantidades"></FooterStyle>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" CssClass="textcantidades" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UNIDAD" HeaderText="Unidad">
                                                        <ItemStyle Width="20px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SUBFAMILIA" HeaderText="Partida Especifica">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="70px" HorizontalAlign="Left" />
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
                                              </div> <%--Fin Div Capturar inventario--%>
                                         </td>
                                       </tr>
                                    </table> <%--Fin de la tabla General--%>
                                </table> <%-- Fin del Div General--%>
                             </div> 
                         </td>
                    </tr>
                    <tr>
                        <td >
                            <div id="Div_Justificación" visible="false" runat="server" style="overflow:auto;vertical-align:top;border-style:none;border-color:Silver;" >    
                               <table width ="98%">
                                  <tr align="center" class="barra_delgada">
                                       <td  align="center" colspan="4"> 
                                       <asp:Label ID="lbl_Justificacion" runat="server"  Text="*Observaciones" />
                                        </td>
                                  </tr>
                                  <tr>
                                    <td align="left">
                                        <asp:TextBox ID="Txt_Justificacion" runat="server" Height="133px" 
                                            MaxLength="2000" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                            
                                            <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                            runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                            TargetControlID="Txt_Justificacion" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                            </cc1:FilteredTextBoxExtender>
                                            
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Justificacion" 
                                            WatermarkText="Límite de Caracteres 2000" WatermarkCssClass="watermarked" Enabled="True"/> 
                                    </td>
                                  </tr>
                                 </table>            
                            </div> 
                         </td>
                    </tr>
               </table>    <%--Fin de la tabla General--%>
           </div> <%-- Fin del Div General--%>
                <br />
                <br />
                <br />
                <br />
      </ContentTemplate>
          <Triggers>
             <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar_Busqueda" EventName="Click"/>
          </Triggers> 
    </asp:UpdatePanel>
    
     <asp:Panel ID="Pnl_Login" runat="server" Width="340px" Height="200px"
       style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none;">
       <center>
            <table width="100%" class="estilo_fuente">
              <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="Btn_Img_Error_Login" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px" Visible="false"/>
                        <asp:Label ID="Lbl_Error_login" runat="server" ForeColor="Red" Width="100%" />
                    </td>
              </tr>
              <tr  align="center" class="barra_delgada" >
                        <td   align="center" colspan="2" > 
                       <asp:Label ID="Label1" runat="server"  Text="Autentificación" Width="100%"/>
                       </td>
              </tr>
              <tr>
                    <td colspan="2">
                    <br />
                    </td>
              </tr>
              <tr>
                <td style="width:180px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Nombre Usuario </td>
                <td ><asp:TextBox ID="Txt_Login" runat="server" Width="120px" MaxLength="10"></asp:TextBox></td>
              </tr>
              <tr>
                <td style="width:180px" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Contraseña</td>
                <td><asp:TextBox ID="Txt_Password" runat="server" TextMode="Password" Width="120px" Columns="10"></asp:TextBox></td>
              </tr>
              <tr>
                <td style="width:180px" >
                 &nbsp;
                </td>
             </tr>
             <tr>
                <td colspan="2">
                    <center>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Aceptar_Login" runat="server" CssClass="button" 
                            onclick="Btn_Aceptar_Login_Click" Text="Aceptar" Width="100px"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Cancelar_Login" runat="server" Text="Cancelar" Width="100px" 
                        CssClass="button" onclick="Btn_Cancelar_Login_Click"/>
                    </center>
                    </td>
            </tr>
        </table>
       </center>
    </asp:Panel>
    
                                   <!-- Modal Popup Busqueda Avanzada-->
     <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar_Busqueda" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
     </asp:UpdatePanel>  



                                      <!-- Modal Popup Login-->
                  <asp:UpdatePanel ID="Udp_Modal_Popup_Login" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                            <cc1:ModalPopupExtender ID="Modal_Login" runat="server"
                                    TargetControlID="Btn_MP_Login"
                                    PopupControlID="Pnl_Login"                      
                                    CancelControlID="Btn_Cancelar_Login"
                                    DropShadow="True"
                                    BackgroundCssClass="progressBackgroundFilter"/>
                            <asp:Button ID="Btn_MP_Login" runat="server" Text="Button" style="display:none;"/>
                      </ContentTemplate>          
                 </asp:UpdatePanel> 
                 
                <%-- Panel del ModalPopUp--%>
      <asp:Panel ID="Pnl_Busqueda" runat="server" Width="500px" Height="200px" 
            style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none; ">
           <center>
           <asp:UpdatePanel ID="pnlPanel" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
              <table width="95%" class="estilo_fuente">
                  <tr>
                     <td colspan="4">
                            <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                      </td>
                  </tr>
                  <tr align="center" >
                      <td colspan="4" class="barra_busqueda" align="center" width="100%"> Búsqueda Avanzada</td>
                  </tr>
                  <tr>
                      <td colspan="4"> <br /></td>
                  </tr>
                  <tr>
                        <td align="left">
                            <asp:CheckBox ID="Chk_Fecha_Abanzada" runat="server" Text="Fecha" 
                            oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                            &nbsp;&nbsp; &nbsp;</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Inicial_B" runat="server" Width="150px" 
                            Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Inicial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Inicial" />
                            <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicial_B" PopupButtonID="Btn_Calendar_Fecha_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td align="left">
                         </td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Final_B" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Final" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Final" />
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Txt_Fecha_Final_B" PopupButtonID="Btn_Calendar_Fecha_Final" OnClientShown="calendarShown" Format="dd/MMM/yyyy" >
                            </cc1:CalendarExtender>
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
                                <asp:Button ID="Btn_Aceptar_Busqueda" runat="server" Text="Aceptar" Width="100px" 
                                onclick="Btn_Aceptar_Busqueda_Click" CssClass="button" Enabled="False"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancelar_Busqueda" runat="server" Text="Cancelar" Width="100px" 
                                CssClass="button" onclick="Btn_Cancelar_Busqueda_Click"/>
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
</asp:Content>
