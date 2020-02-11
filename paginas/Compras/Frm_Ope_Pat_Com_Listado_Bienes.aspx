<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Listado_Bienes.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pat_Com_Listado_Bienes" Title="Operación - Listado de Bienes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Traslado" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Listado de Bienes</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td>
                            &nbsp;
                        </td>                        
                    </tr>
                </table>   
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales"  ID="Tab_Panel_Datos_Generales"  Width="100%"  >
                        <HeaderTemplate> Datos Generales </HeaderTemplate>
                        <ContentTemplate>
                            <div style="border-style:dotted; width:100%;" >
                                <table width="100%">
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Inventario_Anterior" runat="server" Text="Inventario Anterior" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Inventario_Anterior" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Inventario_Anterior" runat="server" TargetControlID="Txt_Inventario_Anterior" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            &nbsp;&nbsp;     
                                            <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                       <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inventario" runat="server" TargetControlID="Txt_Numero_Inventario" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Producto" runat="server" Text="Producto" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Producto" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Producto" runat="server" TargetControlID="Txt_Producto" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+!#$%&_-" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">   
                                            &nbsp;&nbsp;     
                                            <asp:Label ID="Lbl_Exacto_Aproximado" runat="server" Text="Filtro" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Exacto_Aproximado" runat="server" Width="100%" >
                                                <asp:ListItem Text="EXACTO" Value="EXACTO"></asp:ListItem>
                                                <asp:ListItem Text="APROXIMADO" Value="APROXIMADO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Tipo" runat="server" Text="Tipo Bien" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Text="ANIMAL" Value="CEMOVIENTE"></asp:ListItem>
                                                <asp:ListItem Text="BIEN MUEBLE" Value="BIEN_MUEBLE"></asp:ListItem>
                                                <asp:ListItem Text="VEHÍCULO" Value="VEHICULO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            &nbsp;&nbsp;     
                                            <asp:Label ID="Lbl_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">   
                                            <asp:DropDownList ID="Cmb_Marca" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Tipo_Cemoviente" runat="server" Text="Tipo Animal" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Tipo_Cemoviente" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            &nbsp;&nbsp;     
                                            <asp:Label ID="Lbl_Raza" runat="server" Text="Raza" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">   
                                            <asp:DropDownList ID="Cmb_Raza" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Modelo" runat="server" Width="99%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo" runat="server" TargetControlID="Txt_Modelo" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+!#$%&_-" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%; text-align:left; ">   
                                            &nbsp;&nbsp;     
                                            <asp:Label ID="Lbl_Factura" runat="server" Text="Factura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Factura" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factura" runat="server" TargetControlID="Txt_Factura" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+!#$%&_-" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Numero_Serie" runat="server" Text="No. Serie" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Numero_Serie" runat="server" Width="99%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Serie" runat="server" TargetControlID="Txt_Numero_Serie" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+!#$%&_-" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>
                                     <tr>
                                         <td colspan="4">
                                            <hr />    
                                         </td>
                                     </tr>
                                     <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" AlternateText="Consultar" ImageUrl="~/paginas/imagenes/paginas/Listado.png" CausesValidation="False" OnClick="Btn_Buscar_Datos_Click" ToolTip="Ver Listado" Width="24px" />
                                            <asp:ImageButton ID="Btn_Generar_Reporte_Datos" runat="server" AlternateText="Consultar" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" CausesValidation="False" OnClick="Btn_Generar_Reporte_Datos_Click" ToolTip="Generar Reporte" Width="24px" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" ToolTip="Limpiar Filtros" onclick="Btn_Limpiar_Filtros_Buscar_Datos_Click" Width="24px"/>  
                                            &nbsp;&nbsp;                                 
                                        </td>
                                </table>
                            </div>
                        </ContentTemplate>
                    
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes"  ID="Tab_Panel_Resguardantes"  Width="100%"  >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>
                            <div style="border-style:dotted; width:100%; height:150px;" >
                                  <table width="100%">    
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;&nbsp;             
                                        </td>
                                    </tr>                           
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_No_Empleado_Resguardante" runat="server" Text="No. Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_No_Empleado_Resguardante" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleado_Resguardante" runat="server" TargetControlID="Txt_No_Empleado_Resguardante" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; "> 
                                            &nbsp;&nbsp;                                 
                                            <asp:Label ID="Lbl_RFC_Resguardante" runat="server" Text="R. F. C. " CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_RFC_Resguardante" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_RFC_Resguardante" runat="server" TargetControlID="Txt_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Resguardantes_Dependencias" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Resguardantes_Dependencias_SelectedIndexChanged">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; SELECCIONE &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr>    
                                     <tr>
                                         <td colspan="4">
                                            <hr />    
                                         </td>
                                     </tr>
                                     <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/Listado.png" ToolTip="Ver Listado" OnClick="Btn_Buscar_Resguardante_Click" Width="24px"/>                                      
                                            <asp:ImageButton ID="Btn_Generar_Reporte_Resguardante" runat="server" AlternateText="Consultar" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" CausesValidation="False" OnClick="Btn_Generar_Reporte_Resguardante_Click" ToolTip="Generar Reporte" Width="24px" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Click" Width="24px"  />   
                                            &nbsp;&nbsp;                                 
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;&nbsp;             
                                        </td>
                                    </tr>                                   
                                </table>
                            </div>
                        </ContentTemplate>
                </cc1:TabPanel>
                </cc1:TabContainer>  
                <br />
                    <asp:Label ID="Lbl_Numero_Resultados" runat="server" Text=" " style="text-align:center; font-style:italic; border-style:outset; color:Blue;" Width="97%"></asp:Label>
                <br />
                <br />
                <div style="width:97%; height:350px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <asp:GridView ID="Grid_Listado_Bienes" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="false" CssClass="GridView_1"
                            PageSize="100" GridLines="Vertical" 
                            Width="99.5%" onpageindexchanging="Grid_Listado_Bienes_PageIndexChanging">
                            <RowStyle CssClass="GridItem" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <Columns>
                                <asp:BoundField DataField="BIEN_ID" HeaderText="BIEN_ID" SortExpression="BIEN_ID" />
                                <asp:BoundField DataField="INVENTARIO_ANTERIOR" HeaderText="No. Inventario" SortExpression="INVENTARIO_ANTERIOR" >
                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="90px" />
                                </asp:BoundField >
                                <asp:BoundField DataField="INVENTARIO_NUEVO" HeaderText="Inv. [SIAS]" SortExpression="INVENTARIO_NUEVO" >
                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="50px" />
                                </asp:BoundField >
                                <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" SortExpression="DEPENDENCIA" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField >
                                <asp:BoundField DataField="PRODUCTO" HeaderText="Nombre" SortExpression="PRODUCTO" >
                                    <ItemStyle  Font-Size="X-Small"  />
                                </asp:BoundField >
                                <asp:BoundField DataField="CARACTERISTICAS" HeaderText="Caracteristicas" SortExpression="CARACTERISTICAS">
                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                </asp:BoundField >
                                <asp:BoundField DataField="IMPORTE" HeaderText="Importe" SortExpression="IMPORTE" DataFormatString="{0:c}">
                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="90px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                    <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="50px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                        </asp:GridView>
                    </center>   
                </div>                                             
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>