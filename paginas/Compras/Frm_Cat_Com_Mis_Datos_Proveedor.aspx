<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Proveedores.master" CodeFile="Frm_Cat_Com_Mis_Datos_Proveedor.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Mis_Datos_Proveedor" %>

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
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="<%= Page.ResolveUrl("~/paginas/imagenes/paginas/Updating.gif") %>" /></div>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
        <tr>
                <td  colspan="4" class="label_titulo">Mis Datos</td>
            </tr>
            <tr>
                <td colspan="4" class="barra_delgada">
                </td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td>
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
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
            <tr>
                <td colspan="4">
                    <div ID="Div_Mis_Datos_Proveedor" runat="server" style="width:100%;font-size:9px;" visible="true">
                    <table width="100%">
                        <tr>
                            <td style="text-align:left;width:20%;">Padron de Proveedor</td>
                            <td style="text-align:left;width:30%;"><asp:TextBox ID="Txt_Proveedor_ID" runat="server" Enabled="false" 
                            ReadOnly="true" Width="97%" ></asp:TextBox></td>
                            <td>&nbsp;</td><td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;width:20%;">Nombre Comercial</td>
                            <td colspan="3" style="text-align:left;width:80%;">
                                <asp:TextBox ID="Txt_Compania" runat="server" Enabled="false" MaxLength="250" Width="99%" TextMode="SingleLine" TabIndex="6" ></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:left;width:20%;">Razón Social</td>
                            <td colspan="3" style="text-align:left;width:80%;">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="200" Width="99%" Enabled="false" TabIndex="7"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:left;width:20%;">Contacto</td>
                            <td colspan="3" style="text-align:left;width:80%;">
                                <asp:TextBox ID="Txt_Contacto" runat="server" MaxLength="100" Width="99%" TabIndex="8" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:left;width:20%;">RFC</td>
                            <td style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_RFC" runat="server" MaxLength="20" Width="97%" 
                                    TabIndex="9" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="text-align:right;width:20%;">Estatus</td>
                            <td style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="97%" 
                                    TabIndex="9" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr><td colspan="4"><hr /></td></tr>
                        
                        <tr>
                            <td style="text-align:left;width:20%;">Calle y número</td>
                            <td colspan="3" style="text-align:left;width:80%;">
                                <asp:TextBox ID="Txt_Direccion" runat="server" Enabled="false" MaxLength="250" Width="99%" TabIndex="11"></asp:TextBox>
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:left;width:20%;">Colonia</td>
                            <td style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_Colonia" runat="server" MaxLength="100" Width="97%" TabIndex="12" Enabled="false"></asp:TextBox>
                                
                            </td>
                            <td style="text-align:right;width:20%;">Ciudad</td>
                            <td style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_Ciudad" runat="server" MaxLength="100" Width="97%" TabIndex="13" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Estado</td>
                            <td>
                                <asp:TextBox ID="Txt_Estado" runat="server" MaxLength="100" Width="97%" TabIndex="14" Enabled="false"></asp:TextBox>
                                
                            </td>
                            <td style="text-align:right;">CP</td>
                            <td>
                                <asp:TextBox ID="Txt_CP" runat="server" MaxLength="5" Width="97%" TabIndex="15" Enabled="false"></asp:TextBox>
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Tel&eacute;fono 1</td>
                            <td>
                                <asp:TextBox ID="Txt_Telefono_1" runat="server" MaxLength="20" Width="97%" TabIndex="16" Enabled="false"></asp:TextBox>
                                
                            </td>
                            <td style="text-align:right;">Tel&eacute;fono 2</td>
                            <td>
                                <asp:TextBox ID="Txt_Telefono_2" runat="server" MaxLength="20" Width="97%" TabIndex="17" Enabled="false"></asp:TextBox>
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Nextel</td>
                            <td>
                                <asp:TextBox ID="Txt_Nextel" runat="server" MaxLength="20" Width="97%" TabIndex="18"  Enabled="false"></asp:TextBox>
                                
                            </td>
                            
                            <td style="text-align:right;">Fax</td>
                            <td>
                                <asp:TextBox ID="Txt_Fax" runat="server" MaxLength="20" Width="97%" TabIndex="19"  Enabled="false"></asp:TextBox>
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Correo Electr&oacute;nico</td>
                            <td>
                                <asp:TextBox ID="Txt_Correo_Electronico" runat="server" MaxLength="100" 
                                    TabIndex="20" Width="97%"  Enabled="false"></asp:TextBox>
                                
                            </td>
                            <td style="text-align:right;">D&iacute;as Cr&eacute;dito</td>
                            <td>
                                <asp:TextBox ID="Txt_Dias_Credito" runat="server" MaxLength="3" Width="97%" TabIndex="23" Enabled="false"></asp:TextBox>
                                </td>
                            
                        </tr>
                        
                        <tr style="display:none;">
                            <td>Tipo Pago</td>
                            <td>
                                <asp:TextBox ID="Txt_Tipo_Pago" runat="server" MaxLength="3" Width="97%" TabIndex="22" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">Cuenta</td>
                            <td><asp:TextBox ID="Txt_Cuenta" runat="server" MaxLength="250" Width="97%" TabIndex="25" Enabled="false"></asp:TextBox></td>
                            
                        </tr>
                        
                        <tr style="display:none;">
                            <td>Forma de Pago</td>
                            <td>
                                <asp:TextBox ID="Txt_Forma_Pago" runat="server" Width="97%" TabIndex="25" Enabled="false"></asp:TextBox>
                            </td>
                            
                                
                        </tr>
                        <tr style="display:none;">
                            <td>Comentarios</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" MaxLength="250" Width="99%" Enabled="false" TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="Txt_Comentarios_TextBoxWatermarkExtender" runat="server" TargetControlID="Txt_Comentarios" 
                                WatermarkCssClass="watermarked" WatermarkText="<Algun Comentario Extra>" />
                            </td>
                        </tr>
                        
                        
                        <tr>
                        
                            <td align="center" style="text-align:right; width:100%; vertical-align:top;" colspan="4" >
                            <asp:GridView ID="Grid_Conceptos_Proveedor" runat="server" Style="white-space:normal"
                                AutoGenerateColumns="false" Width="99%" CssClass="GridView_1" PageSize="5" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="ID" Visible="false">
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CONCEPTO" HeaderText="Giros ó Conceptos Registrados" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="100%"/>
                                        <ItemStyle HorizontalAlign="Left" Width="100%"/>
                                    </asp:BoundField>
                                 </Columns>
                                 <RowStyle CssClass="GridItem" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />    
                            </asp:GridView>
                       </td>
                    </tr>
                    <tr style="display:none;">
                        <td colspan="4" class="barra_delgada">                        
                        </td>
                        <caption>
                            <br />
                            <br />
                        </caption>
                    </tr>  
                    <tr style="display:none;">
                        <td colspan="4" style="height:20px;">                        
                        </td>
      
                    </tr>                                      
                    <tr style="display:none;">
                        <td>
                            Solicitud
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Solicitud" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td colspan="4" align="center">
                            <asp:Button ID="Btn_Enviar_Solicitud" runat="server" Text="Enviar Solicitud" 
                                CssClass="button" Width="300px" onclick="Btn_Enviar_Solicitud_Click"/>
                        </td>
                    </tr>
                    
                    </table>
                    </div>
                </td>
            </tr>
        
        </table>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

