<%@ Page Language="C#" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" 
CodeFile="Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx.cs" 
Inherits="paginas_Compras_Frm_Ope_Com_Alm_Generar_Inventario_Stock" 
Title="Generar Inventario Stock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
   </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
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
           
         <div id="Div_General" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
            <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" 
                    width="100%"> <%--Tabla Generar Inventario--%>
                    <tr style="width:98%" align="center">
                         <td colspan ="2" class="label_titulo"> <asp:Label ID="Lbl_Titulo" runat="server" Text="Generar Inventario" class="label_titulo"></asp:Label> </td>
                    </tr>
                    <tr style="width:98%">
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%">
                              <tr>
                                <td colspan="2" align="left" width="80%">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr> 
                    <tr style="width:98%">
                        <td style="width:63%" >&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" style="width:98%" >
                            <td align="left" style="width:63%" >
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                    AlternateText="NUEVO" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Guardar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Width="24px" CssClass="Img_Button" 
                                    AlternateText="Guardar" ToolTip="Guardar" 
                                    OnClientClick="return confirm('¿Está seguro de guardar el inventario?');"
                                    OnClick="Btn_Guardar_Click" Visible="False"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                     AlternateText="Salir" ToolTip="Salir" onclick="Btn_Salir_Click"/>
                            </td>                        
                            <td align="right" style="width:80%;">
                                <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                ToolTip="AVANZADA" onclick="Btn_Busqueda_Avanzada_Click"> Busqueda </asp:LinkButton>
                                &nbsp;&nbsp;
                                
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="55%" Enabled="true">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click1" 
                                        ToolTip="Buscar" />
                                </div>
                           </td> 
                        </tr>           

                        <div id="Div_Generar_Inventario" visible="false" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" >    
                            <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%">
                                <tr>
                                    <td colspan="2">
                                         <div>
                                             <br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="187px" > 
                                         <asp:CheckBox ID="Ckb_Inventario_Fisico" runat="server" Checked="false" 
                                         Enabled="true" Text="Inventario Físico" 
                                         AutoPostBack="True"
                                         oncheckedchanged="Ckb_Inventario_Fisico_CheckedChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="187px"  >
                                        <asp:CheckBox ID="Ckb_Inventario_Selectivo" runat="server" 
                                            Enabled="true" Text="Inventario Selectivo" 
                                            oncheckedchanged="Ckb_Inventario_Selectivo_CheckedChanged" 
                                            AutoPostBack="True" Width="80%"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="187px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Familia" runat="server" 
                                            OnCheckedChanged="Ckb_Familia_CheckedChanged1" Text="Partida Genérica" 
                                            Checked="false" Enabled="false"
                                            AutoPostBack="True"  />
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="Cmb_Familia" runat="server" Width="70%" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                <tr>
                                    <td width="187px"  >
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Subfamilia" runat="server" 
                                            OnCheckedChanged="Ckb_subfamilia_CheckedChanged" Text="Partida Especifica" 
                                            Checked="false" Enabled="false"
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Subfamilia" runat="server" Width="70%" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                    <tr>
                                        <td width="187px" >
                                            &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_A_Z" runat="server" 
                                                OnCheckedChanged="Ckb_A_Z_CheckedChanged" Text="Descripción de la A-Z" 
                                                Checked="false" Enabled="false"
                                                AutoPostBack="True"  />
                                        </td>
                                        <td>
                                       <span >De:</span>&nbsp;<asp:TextBox ID="Txt_Letra_Inicial" runat="server"  Width="50px" 
                                                Enabled="false" MaxLength="1" FilterType= "UppercaseLetters, LowercaseLetters" InvalidChars="<,>,&,',!," ></asp:TextBox>
                                                
                                           <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Inicial" runat="server" 
                                                Enabled="True" FilterType="Custom"  InvalidChars="0,1,2,3,4,5,6,7,8,9,&lt;,&gt;,',!," 
                                                TargetControlID="Txt_Letra_Inicial" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ">
                                            </cc1:FilteredTextBoxExtender>

                                               &nbsp;&nbsp;<span >&nbsp; A: </span>
                                                <asp:TextBox ID="Txt_Letra_Final" FilterType= "UppercaseLetters, LowercaseLetters" runat="server"  Width="50px" Enabled="false" 
                                                MaxLength="1"></asp:TextBox>
                                                
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Final" runat="server" 
                                                Enabled="True" FilterType="Custom"  InvalidChars="0,1,2,3,4,5,6,7,8,9,&lt;,&gt;,',!," 
                                                TargetControlID="Txt_Letra_Final" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="187px" >
                                            &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Marca" runat="server" OnCheckedChanged="Ckb_Marca_CheckedChanged" Text="Marca" Checked="false" Enabled="false"
                                                AutoPostBack="True" Visible="false" />
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="Cmb_Marca" runat="server" Width="70%" Enabled="False"
                                                AutoPostBack="True" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                     </tr>
                                <table border="0" cellspacing="0" class="estilo_fuente" frame="border" 
                                    width="100%">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr align="right" class="barra_delgada" style="width:98%">
                                        <td align="center" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones" 
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td width="85%">
                                            <asp:TextBox ID="Txt_Observaciones" runat="server" Enabled="False" 
                                                Height="60px" MaxLength="250" Rows="2" TextMode="MultiLine" Visible="False" 
                                                Width="98%"></asp:TextBox>
                                                
                                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" 
                                                Enabled="True" TargetControlID="Txt_Observaciones" 
                                                WatermarkCssClass="watermarked" WatermarkText="Límite de Caracteres 250" />
                                        </td>
                                    </tr>
                                </table>
                            </table>
                       </div>             
                        <table width="100%">       
                           <tr>
                               <%-- <td> 
                                &nbsp;
                                </td>--%>
                           </tr>
                           <tr>
                                <td>
                                    <asp:Label ID="Lbl_Inventarios" runat="server" CssClass="estilo_fuente" 
                                        Visible="False" >Inventarios</asp:Label>
                                </td> 
                            </tr>  
                            <tr>
                                <td>
                                    <asp:GridView ID="Grid_Inventario_Stock" runat="server" AllowPaging="True" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        ForeColor="#333333" GridLines="None" Height="98%" style="white-space:normal;" 
                                        OnPageIndexChanging="Grid_Inventario_Stock_PageIndexChanging" 
                                        OnSelectedIndexChanged="Grid_Inventario_Stock_SelectedIndexChanged" 
                                        Width="98%" PageSize="20">
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
                           </table>
                        <tr>                   
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_No_Inventario" runat="server" CssClass="estilo_fuente" 
                                            Visible="False" />
                                        <br />
                                    </td>
                                </tr>
                            </table> 
                        </tr>    
                            
                        <asp:GridView ID="Grid_Productos_Inventario" runat="server" style="white-space:normal;" 
                                AutoGenerateColumns="False" CellPadding="1" ForeColor="#333333" 
                                GridLines="None" AllowPaging="True" Width="98%"
                                OnPageIndexChanging="Grid_Productos_Inventario_PageIndexChanging" 
                                Visible="False" PageSize="15" CssClass="GridView_1" 
                                Height="98%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" 
                                        SortExpression="PRODUCTO_ID" Visible="False">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                        SortExpression="CLAVE" >
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PRODUCTO" 
                                        HeaderText="Producto">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EXISTENCIA" HeaderText="Existencia" 
                                        SortExpression="EXISTENCIA" >
                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                        <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SUBFAMILIA" HeaderText="Partida Especifica" >
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FAMILIA" HeaderText="Partida Genérica" 
                                        SortExpression="FAMILIA" Visible="False" >
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" 
                                        SortExpression="MODELO" Visible="False" >
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                             </asp:GridView>

                         </table> <%--Fin de la tabla Generar Inventario--%>
                       </table>    <%--Fin de la tabla General--%>
                     </div> <%--Fin del div Generar inventario--%>
                    </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>
                 </Triggers>   
             </asp:UpdatePanel>        
             
             <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar_Panel" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
            </asp:UpdatePanel>  
             
             <%-- Panel del ModalPopUp--%>
           <asp:Panel ID="Pnl_Busqueda" runat="server" Width="500px"  Height="210px"
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
                      <td colspan="4" class="barra_busqueda" align="center" width="100%"> Búsqueda 
                          Avanzada</td>
                  </tr>
                  <tr>
                      <td colspan="4"></td>
                  </tr>
                  <tr>
                        <td align="left">
                            <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                            oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                            &nbsp;&nbsp; &nbsp;Del</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Inicial_B" runat="server" Width="130px" 
                            Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Inicial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Inicial" />
                            <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicial_B" PopupButtonID="Btn_Calendar_Fecha_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td align="left">
                            Al</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Final_B" runat="server" Width="130px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Final" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Final" />
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                            TargetControlID="Txt_Fecha_Final_B" PopupButtonID="Btn_Calendar_Fecha_Final" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="Chk_Estatus_B" runat="server" Text="Estatus" AutoPostBack="true"
                            oncheckedchanged="Chk_Estatus_B_CheckedChanged"/></td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" 
                            Enabled="False" >
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
                            <center>
                                <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" 
                                onclick="Btn_Aceptar_Click" CssClass="button"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancelar_Panel" runat="server" Text="Cancelar" Width="100px" 
                                CssClass="button" onclick="Btn_Cancelar_Panel_Click"/>
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

