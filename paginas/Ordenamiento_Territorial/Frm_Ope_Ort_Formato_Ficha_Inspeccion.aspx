<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
CodeFile="Frm_Ope_Ort_Formato_Ficha_Inspeccion.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Formato_Ficha_Inspeccion" %>

<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"> 
<script src="../jquery/jquery-1.5.js" type="text/javascript"></script>    
    <script type="text/javascript" language="javascript">
    //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
          
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
                
            
            <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                <tr align="center">
                    <td  colspan="2" class="label_titulo">Ficha de inspeccion</td>
               </tr>
                <tr> <!--Bloque del mensaje de error-->
                    <td colspan="2" >
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    </td>      
                </tr>
            </table  >
            
             <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                <tr class="barra_busqueda" align="right">
                     <td align="left" valign="middle" colspan="2">     
                        <div>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" onclick="Btn_Nuevo_Click"
                                ToolTip="Nuevo"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" onclick="Btn_Salir_Click" 
                                CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                          <%--  <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" onclick="Btn_Modificar_Click" 
                                AlternateText="Modificar" ToolTip="Modificar" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                CssClass="Img_Button" onclick="Btn_Eliminar_Click"  
                                OnClientClick="return confirm('Desea eliminar los emplados relacionados con el perfil. ¿Desea continuar?');"
                                AlternateText="Eliminar" ToolTip="Eliminar"/>
                            
                        </div>--%>
                    </td>
                   <%-- <td colspan="2">Búsqueda
                        <asp:TextBox ID="Txt_Busqueda" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" />
                    </td>--%>
                </tr> 
            </table>
            
            
              <div id="Div_Lista_Formatos" runat="server" style="display:block">  
                <table width="98%">
                    <tr>
                        <td style="width:100%">
                            <center>
                                <div id="Div_Grid_Formatos" runat="server" 
                                    style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                    <asp:GridView ID="Grid_Formatos" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False"  Width="100%"
                                        EmptyDataText="No se encuentra ninguna solicitud en espera"
                                        OnRowDataBound="Grid_Formatos_RowDataBound"
                                        GridLines= "None">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField  HeaderText= "Capturar"  HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="12px">
                                                <ItemTemplate >
                                                    <center>
                                                        <asp:ImageButton ID="Btn_Autorizar_Formato" runat="server" 
                                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" Height="24px" 
                                                            OnClick="Btn_Autorizar_Formato_Click" 
                                                            CausesValidation="false"/> 
                                                    </center>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                <ItemStyle HorizontalAlign="center" Width="7%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID ID" 
                                                SortExpression="SOLICITUD_ID" />
                                            <asp:BoundField DataField="TRAMITE_ID" HeaderText="TRAMITE_ID ID" 
                                                SortExpression="TRAMITE_ID" />   
                                            <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SUBPROCESO_ID ID" 
                                                SortExpression="SUBPROCESO_ID" />                                                  
                                            <asp:BoundField DataField="NOMBRE_TRAMITE" HeaderText="Tramite" 
                                                SortExpression="NOMBRE_TRAMITE">
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="40%" />
                                                <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="40%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE_ACTIVIDAD" HeaderText="Actividad" 
                                                SortExpression="NOMBRE_ACTIVIDAD">
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="40%" />
                                                <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="40%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave solicitud" 
                                                SortExpression="Clave">
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="13%" />
                                                <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="13%" />
                                            </asp:BoundField>
                                           
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />                                
                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                    </asp:GridView>  
                                </div> 
                            </center>
                        </td>
                    </tr>
                </table>  
            </div> 
                        
        <div id="Div_Principal_Llenado_Formato" runat="server" style="display:none">   
            <table class="estilo_fuente" width="98%">      
                <tr>
                    <td style="width:15%" align="left">
                        Numero consecutivo
                    </td>
                    <td style="width:35%" align="left">
                        <asp:TextBox ID="Txt_Consecutivo" runat="server" Enabled="false" Width="85%"></asp:TextBox> 
                    </td>
                    <td style="width:15%" align="left">
                    </td>
                    <td style="width:35%" align="left">
                        <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width:15%" align="left">
                        Fecha Entraga
                    </td>
                    <td style="width:35%" align="left">
                        <asp:TextBox ID="Txt_Fecha_Entraga" runat="server" Enabled="false" Width="75%"></asp:TextBox> 
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Entraga" runat="server" 
                                TargetControlID="Txt_Fecha_Entraga" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                ValidChars="/_" />
                            <cc1:CalendarExtender ID="Txt_Fecha_Entraga_CalendarExtender" runat="server" 
                                TargetControlID="Txt_Fecha_Entraga" PopupButtonID="Btn_Fecha_Entrega" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Entrega" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />   
                    </td>
                    <td style="width:15%" align="right">
                        Tiempo de respuesta
                    </td>
                    <td style="width:35%" align="left">
                         <asp:TextBox ID="Txt_Tiempo_Respuesta" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                         <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tiempo_Respuesta" runat="server" 
                                TargetControlID="Txt_Tiempo_Respuesta" FilterType="Numbers"
                                 />
                    </td>
                </tr>
                <tr>
                    <td style="width:15%" align="left">
                        Fecha Inspeccion
                    </td>
                    <td style="width:35%" align="left">
                        <asp:TextBox ID="Txt_Fecha_Inspeccion" runat="server" Enabled="false" Width="75%"></asp:TextBox> 
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Inspeccion" runat="server" 
                                TargetControlID="Txt_Fecha_Inspeccion" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                ValidChars="/_" />
                            <cc1:CalendarExtender ID="Ce_Txt_Fecha_Inspeccion" runat="server" 
                                TargetControlID="Txt_Fecha_Inspeccion" PopupButtonID="Btn_Fecha_Inspeccion" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Inspeccion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />   
                    </td>
                    <td style="width:15%" align="right">
                        Inspecctor
                    </td>
                    <td style="width:35%" align="left">
                         <asp:DropDownList ID="Cmb_Inspector" runat="server" Width="95%"></asp:DropDownList> 
                    </td>
                </tr>
            </table>
            
            <table class="estilo_fuente" width="100%">      
                <tr>
                    <td rowspan="5">
                     </td>
                </tr>
            </table>         
                      
            <cc1:TabContainer ID="Tab_Contenedor_Inspeccion" runat="server" Width="98%" 
                ActiveTabIndex="0">
                
                 <cc1:TabPanel  runat="server" HeaderText=""  ID="TabPnl_Ubicacion_Inmueble"  Width="100%"  >
                    <HeaderTemplate>Datos generales</HeaderTemplate>
                    <ContentTemplate>
                    
                        <div id="Div_Ubiacion_Inmueble" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Ubiacion_Inmueble" runat="server" GroupingText="Ubicacion del inmueble" ForeColor="Blue"> 
                                <table class="estilo_fuente" width="100%">      
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Nombre
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Inmueble_Nombre_Inmueble" runat="server" Enabled="false" Width="95%" TabIndex="1"></asp:TextBox> 
                                        </td>
                                        <td style="width:15%" align="right">
                                            Telefono
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Inmueble_Telefono" runat="server" Enabled="false" Width="95%" MaxLength="20"  TabIndex="2"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Inmueble_Telefonoo" runat="server" 
                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Inmueble_Telefono" 
                                                    ValidChars="0123456789" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width:15%" >  
                                            Colonia, Fraccionamiento
                                        </td>
                                         <td style="width:85%" >  
                                            <asp:DropDownList ID="Cmb_Inmueble_Colonias" runat="server"  width="90%" TabIndex="3"
                                                AutoPostBack="true" 
                                                DropDownStyle="DropDownList" 
                                                AutoCompleteMode="SuggestAppend" 
                                                CaseSensitive="False" 
                                                CssClass="WindowsStyle"
                                                OnSelectedIndexChanged="Cmb_Inmueble_Colonias_OnSelectedIndexChanged" /> 
                                            <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Colonia_Click" />
                                        </td>
                                    </tr>
                                </table >
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width:15%" >
                                            Calle
                                        </td>
                                        <td style="width:35%" >
                                            <asp:DropDownList ID="Cmb_Inmueble_Calle" runat="server" width="90%" TabIndex="4"
                                                AutoPostBack="false" 
                                                DropDownStyle="DropDown" 
                                                AutoCompleteMode="SuggestAppend" 
                                                CaseSensitive="False" 
                                                CssClass="WindowsStyle"  />                        
                                                
                                            <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Calles_Click" />
                                        </td>
                                        <td style="width:15%" align="right">
                                            Numero
                                        </td>
                                        <td style="width:35%" >
                                            <asp:TextBox ID="Txt_Inmueble_Numero" runat="server" Enabled="false" Width="95%" MaxLength="10" TabIndex="5"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Inmueble_Numero" runat="server" 
                                                     ValidChars="0123456789"  FilterType="Numbers" TargetControlID="Txt_Inmueble_Numero" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div> 
                        
                        <table class="estilo_fuente" width="100%">      
                            <tr>
                                <td rowspan="5">
                
                                 </td>
                            </tr>
                        </table>
                        
                        <div id="Div_Datos_Solicitante" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Datos_Solicitante" runat="server" GroupingText="Datos del solicitante" ForeColor="Blue"> 
                                <table class="estilo_fuente" width="100%">      
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Nombre
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Solicitante_Nombre" runat="server" Enabled="false" Width="95%" TabIndex="6"></asp:TextBox> 
                                        </td>
                                        <td style="width:15%" align="right">
                                            Telefono
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Solicitante_Telefono" runat="server" Enabled="false" Width="95%" MaxLength="20" TabIndex="7"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Solicitante_Telefono" runat="server" 
                                                     FilterType="Custom, Numbers" TargetControlID="Txt_Solicitante_Telefono" 
                                                     ValidChars="0123456789" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width:15%" >  
                                            Colonia, Fraccionamiento
                                        </td>
                                         <td style="width:85%" >  
                                              <asp:DropDownList ID="Cmb_Solicitante_Colonia" runat="server"  width="90%" TabIndex="8"
                                                AutoPostBack="true" 
                                                DropDownStyle="DropDownList" 
                                                AutoCompleteMode="SuggestAppend" 
                                                CaseSensitive="False" 
                                                CssClass="WindowsStyle"
                                                OnSelectedIndexChanged="Cmb_Solicitante_Colonia_OnSelectedIndexChanged" /> 
                                            <asp:ImageButton ID="Btn_Buscar_Colonia_Solicitante" runat="server" ToolTip="Seleccionar Colonia"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Colonia_Solicitante_Click" />
                                        </td>
                                    </tr>
                                </table >
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width:15%" >
                                            Calle
                                        </td>
                                        <td style="width:35%" >
                                            <asp:DropDownList ID="Cmb_Solicitante_Calle" runat="server" width="90%" TabIndex="9"
                                                AutoPostBack="false" 
                                                DropDownStyle="DropDown" 
                                                AutoCompleteMode="SuggestAppend" 
                                                CaseSensitive="False" 
                                                CssClass="WindowsStyle"  />     
                                            <asp:ImageButton ID="Btn_Buscar_Calles_Solicitante" runat="server" ToolTip="Seleccionar Calle"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Calles_Solicitante_Click" />                   
                                        </td>
                                        <td style="width:15%" align="right">
                                            Numero
                                        </td>
                                        <td style="width:35%" >
                                            <asp:TextBox ID="Txt_Solicitante_Numero" runat="server" Enabled="false" Width="95%" MaxLength="10" TabIndex="10"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Solicitante_Numero" runat="server" 
                                                        FilterType="Numbers" TargetControlID="Txt_Solicitante_Numero" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel>   
                  
                
                <cc1:TabPanel runat="server" HeaderText=""  ID="TabPnl_Manifiesto_Impacto_Ambiental"  Width="100%"  >
                    <HeaderTemplate>Manifiesto</HeaderTemplate>
                    <ContentTemplate>  
                         <div id="Div_Manifiesto_Impacto_Ambiental" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Manifiesto_Impacto_Ambiental" runat="server" GroupingText="Manifiesto de impacto ambiental" ForeColor="Blue"> 
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width:15%" >
                                            Afectaciones de arboles
                                        </td>
                                        <td style="width:85%" >
                                            <asp:TextBox ID="Txt_Manifiesto_Afectacion" runat="server" Enabled="false" Width="95%" 
                                               TextMode="MultiLine" Rows="2" MaxLength="100"></asp:TextBox>      
                                        </td>
                                    </tr>
                                    <tr>      
                                        <td style="width:15%" align="left">
                                            colindancias
                                        </td>
                                        <td style="width:85%" >
                                            <asp:TextBox ID="Txt_Manifiesto_Colindancia" runat="server" Enabled="false" Width="95%" MaxLength="100"
                                                TextMode="MultiLine" Rows="2"></asp:TextBox> 
                                        </td>
                                    </tr>
                                    <tr>      
                                        <td style="width:15%" align="left">
                                            Superficie Total
                                        </td>
                                        <td style="width:85%" >
                                            <asp:TextBox ID="Txt_Manifiesto_Superficie_Total" runat="server" Enabled="true" Width="95%" 
                                                ></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                        FilterType="Numbers" TargetControlID="Txt_Manifiesto_Superficie_Total" Enabled="True" ValidChars="0123456789." >
                                                        </cc1:FilteredTextBoxExtender>
                                              
                                        </td>
                                    </tr>
                                     <tr>      
                                        <td style="width:15%" align="left">
                                            Tipo Proyecto
                                        </td>
                                        <td style="width:85%" >
                                            <asp:TextBox ID="Txt_Manifiesto_Tipo_Proyecto" runat="server" Enabled="false" Width="95%" MaxLength="100"></asp:TextBox>                                                  
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel> 
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel>  
                
                <cc1:TabPanel runat="server" HeaderText=""  ID="TabPnl_Licencia_Ambiental"  Width="100%"  >
                    <HeaderTemplate>Licencia</HeaderTemplate>
                    <ContentTemplate> 
                       <div id="Div_Licencia_Ambiental" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Licencia_Ambiental" runat="server" GroupingText="Licencia ambiental de funcionamiento" ForeColor="Blue">
                            
                                <table class="estilo_fuente" width="100%">      
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Tipo de equipo emisor
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Licencia_Equipo_Emisor" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td>
                                        <td style="width:15%" align="right">
                                            Tipo Emision
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Licencia_Tipo_Emision" runat="server" Enabled="false" Width="95%" ></asp:TextBox> 
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Hora de funcionamiento
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Licencia_Hora_Funcionamiento" runat="server" Enabled="true" Width="95%" ></asp:TextBox> 
                                                <cc1:MaskedEditExtender ID="MEE_Txt_Licencia_Hora_Funcionamiento"    runat="server"
                                                    Mask="99:99:99"
                                                    TargetControlID="Txt_Licencia_Hora_Funcionamiento"    
                                                    MaskType="Time"  
                                                    AcceptNegative="None" AcceptAMPM="true"  />
                                        </td>
                                        <td style="width:15%" align="right">
                                            Tipo de combustible
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Licencia_Tipo_Conbustible" runat="server" Enabled="true" Width="95%" ></asp:TextBox> 
                                               
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width:15%" align="left">
                                            Gastos de combustible
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Licencia_Gastos_Combustible" runat="server" Enabled="true" Width="95%" 
                                               ></asp:TextBox> 
                                                 <cc1:FilteredTextBoxExtender ID="Fte_Txt_Licencia_Gastos_Combustible" runat="server" 
                                                    TargetControlID="Txt_Licencia_Gastos_Combustible" FilterType="Numbers"
                                                     />
                                                <%--<cc1:MaskedEditExtender ID="MEE_Txt_Licencia_Gastos_Combustible" runat="server" 
                                                    TargetControlID="Txt_Licencia_Gastos_Combustible" Mask="9,999,999.99" MaskType="Number" 
                                                    InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="Left" 
                                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />--%>
                                        </td>
                                        <td style="width:15%" align="right">
                                        </td>
                                        <td style="width:35%" align="left">
                                        </td>
                                    </tr>
                                </table>
                             
                            </asp:Panel> 
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel> 
                
                 <cc1:TabPanel runat="server" HeaderText=""  ID="TabPnl_Autorizacion"  Width="100%"  >
                    <HeaderTemplate>Poda</HeaderTemplate>
                    <ContentTemplate> 
                       <div id="Div_Autorizacion" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Autorizacion" runat="server" GroupingText="Autorizacion de poda o tala" ForeColor="Blue">
                            
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Altura
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Autorizacion_Altura" runat="server" Enabled="false" Width="95%"
                                               
                                                style="text-align:left"></asp:TextBox> 
                                                 <cc1:FilteredTextBoxExtender ID="Fte_Txt_Autorizacion_Altura" runat="server" 
                                                    TargetControlID="Txt_Autorizacion_Altura" FilterType="Numbers"
                                                     />
                                                 <%--<cc1:MaskedEditExtender ID="Mee_Txt_Autorizacion_Altura" runat="server" 
                                                        TargetControlID="Txt_Autorizacion_Altura" Mask="9,999,999.99" MaskType="Number" 
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="None" 
                                                        ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />--%>
                                        </td>
                                        <td style="width:15%" align="right">
                                            Diametro de tronco
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Autorizacion_Diametro" runat="server" Enabled="false" Width="95%"
                                                                                                style="text-align:left"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Autorizacion_Diametro" runat="server" 
                                                    TargetControlID="Txt_Autorizacion_Diametro" FilterType="Numbers"
                                                     />
                                                <%--<cc1:MaskedEditExtender ID="Mee_Txt_Autorizacion_Diametro" runat="server" 
                                                        TargetControlID="Txt_Autorizacion_Diametro" Mask="9,999,999.99" MaskType="Number" 
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="None" 
                                                        ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />--%>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Diametro de fonda
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Autorizacion_Diametro_Fronda" runat="server" Enabled="false" Width="95%"
                                                                                               style="text-align:left"></asp:TextBox> 
                                                 <cc1:FilteredTextBoxExtender ID="Fet_Txt_Autorizacion_Diametro_Fronda" runat="server" 
                                                    TargetControlID="Txt_Autorizacion_Diametro_Fronda" FilterType="Numbers"
                                                     />
                                               <%--<cc1:MaskedEditExtender ID="Mee_Txt_Autorizacion_Diametro_Fronda" runat="server" 
                                                        TargetControlID="Txt_Autorizacion_Diametro_Fronda" Mask="9,999,999.99" MaskType="Number" 
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="None" 
                                                        ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />--%>
                                        </td>
                                        <td style="width:15%" align="right">
                                            Especie
                                        </td>
                                        <td style="width:35%" align="left">
                                          <asp:TextBox ID="Txt_Autorizacion_Especie" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td>
                                    </tr>
                                </table>
                                
                                 <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Condiciones del arbol
                                        </td>
                                        <td style="width:85%" align="left">
                                            <asp:TextBox ID="Txt_Autorizacion_Condiciones_Arbol" runat="server" Enabled="false" Width="98%"></asp:TextBox> 
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel> 
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel> 
                      
                      
                <cc1:TabPanel runat="server" HeaderText=""  ID="TabPanel1"  Width="100%"  >
                    <HeaderTemplate>Materiales</HeaderTemplate>
                    <ContentTemplate> 
                       <div id="Div_Materiales" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Materiales" runat="server" GroupingText="Bancos de material" ForeColor="Blue">
                            
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Permiso del instituto de ecologia
                                        </td>  
                                        <td style="width:15%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Material_Permiso_Ecologia" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>    
                                        <td style="width:15%" align="right">
                                            Permiso de uso de suelo
                                        </td>  
                                        <td style="width:15%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Material_Permiso_Suelo" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>  
                                        <td style="width:15%" align="right">
                                            Superficie Total
                                        </td>  
                                        <td style="width:25%" align="left">
                                            <asp:TextBox ID="Txt_Materiales_Superficie_Total" runat="server" Enabled="false" Width="95%"
                                               
                                                style="text-align:right"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fet_Txt_Materiales_Superficie_Total" runat="server" 
                                                    TargetControlID="Txt_Materiales_Superficie_Total" FilterType="Numbers"
                                                     />
                                                <%--<cc1:MaskedEditExtender ID="Mee_Txt_Materiales_Superficie_Total" runat="server" 
                                                    TargetControlID="Txt_Materiales_Superficie_Total" Mask="9,999,999.99" MaskType="Number" 
                                                    InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="None" 
                                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />--%>
                                        </td>     
                                    </tr>
                                </table>
                                
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Profundidad
                                        </td>  
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Materiales_Profundidad" runat="server" Enabled="false" Width="95%"
                                                style="text-align:left"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Materiales_Profundidad" runat="server" 
                                                    FilterType="Numbers" TargetControlID="Txt_Materiales_Profundidad" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>  
                                    
                                        <td style="width:15%" align="right">
                                            Inclinacion
                                        </td>  
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Materiales_Inclinacion" runat="server" Enabled="false" Width="95%"
                                                style="text-align:left"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Materiales_Inclinacion" runat="server" 
                                                    FilterType="Numbers" TargetControlID="Txt_Materiales_Inclinacion" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>  
                                    </tr>
                                    <tr>
                                        
                                         <td style="width:15%" align="left">
                                            Flora
                                        </td>  
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Materiales_Flora" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                                
                                        </td> 
                                        <td style="width:15%" align="right">
                                            Tipo de material petreo
                                        </td>  
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Materiales_Petreo" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                                
                                        </td>  
                                       
                                    </tr>
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Accesibilidad de vehiculos/tolvas
                                        </td>
                                        <td style="width:35%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Material_Accesibilidad_Vehiculo" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%--<asp:TextBox ID="Txt_Materiales_Accesibilidad" runat="server" Enabled="false" Width="95%"
                                                style="text-align:right"></asp:TextBox> 
                                                <cc1:FilteredTextBoxExtender ID="Fee_Txt_Materiales_Accesibilidad" runat="server" 
                                                    FilterType="Numbers" TargetControlID="Txt_Materiales_Accesibilidad" Enabled="True"></cc1:FilteredTextBoxExtender>--%>
                                        </td>  
                                        <td style="width:15%" align="right"> </td>
                                        <td style="width:35%" align="right">  </td>
                                    </tr>
                                    
                                </table>
                                
                            </asp:Panel> 
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel> 
               
             <cc1:TabPanel runat="server" HeaderText=""  ID="TabPnl_Aprovechamiento"  Width="100%"  >
                <HeaderTemplate>Autorizacion</HeaderTemplate>
                    <ContentTemplate> 
                       <div id="Div_Aprovechamiento" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Aprovechamiento" runat="server" GroupingText="Autorizacion de aprovechamiento ambiental" ForeColor="Blue">
                            
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Uso de suelos
                                        </td> 
                                         <td style="width:15%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Aprovechamiento_Uso_Suelo" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td> 
                                        
                                        <td style="width:15%" align="right">
                                            Area de almacen de residuos
                                        </td> 
                                         <td style="width:15%" align="left">
                                             <asp:RadioButtonList ID="RBtn_Aprovechamiento_Almacen_Residuos" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td> 
                                         <td style="width:15%" align="right">
                                            Existe separacion
                                        </td> 
                                         <td style="width:15%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Aprovechamiento_Existe_Separacion" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td> 
                                         <td style="width:10%" align="right">
                                        </td> 
                                    </tr>    
                                </table> 
                                 
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Residuos tipo
                                        </td> 
                                        <td style="width:75%" align="left">
                                          <asp:DropDownList ID="Cmb_Tipo_Residuo" runat="server"  width="90%" Enabled="false"
                                            AutoPostBack="false" 
                                            DropDownStyle="DropDownList" 
                                            AutoCompleteMode="SuggestAppend" 
                                            CaseSensitive="False" 
                                            CssClass="WindowsStyle"  />
                                            
                                          
                                        </td> 
                                        <td style="width:15%" align="center">
                                            <asp:ImageButton ID="Btn_Agregar_Tipo_Residuo" runat="server" Width="20px" Height="20px"
                                                 ImageUrl="~/paginas/imagenes/gridview/add_grid.png" 
                                                AlternateText="Agregar Documento" onclick="Btn_Agregar_Tipo_Residuo_Click" />                                                
                                                                                         
                                           
                                        </td>
                                       <%-- 
                                        <td style="width:20%" align="left">
                                            <asp:CheckBox ID="Chk_Aprovechamiento_Lista_Peligros" runat="server" Text="PELIGROSOS" />
                                        </td> 
                                        </td> 
                                        <td style="width:22%" align="left">
                                            <asp:CheckBox ID="Chk_Aprovechamiento_Lista_Manejo_Especila" runat="server" Text="DE MANEJO ESPECIAL" />
                                        </td> 
                                        <td style="width:20%" align="left">
                                            <asp:CheckBox ID="Chk_Aprovechamiento_Lista_Solidos" runat="server" Text="SOLIDOS URBANOS" />
                                        </td> 
                                        <td style="width:23%" align="left">
                                            <asp:CheckBox ID="Chk_Aprovechamiento_Lista_Biologico" runat="server" Text="BIOLOGICO-INFECCIOSOS" />
                                        </td> --%>
                                    </tr>  
                                </table> 
                                
                                <table class="estilo_fuente" width="100%">      
                                    <tr>
                                        <td style="width:100%;text-align:center;vertical-align:top;">
                                            <center>
                                                <div id="Div2" runat="server" 
                                                    style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                                    <asp:GridView ID="Grid_Tipos_Residuos" runat="server" Width="100%"
                                                        CssClass="GridView_1" 
                                                        GridLines= "None"
                                                        AutoGenerateColumns="False"   
                                                        EmptyDataText="No se encuentra ningun tipo de residuo">
                                                        <Columns>
                                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                                <ItemStyle Width="0%" />
                                                            </asp:ButtonField>
                                                            
                                                            <asp:BoundField DataField="TIPO_RESIDUO_ID" HeaderText="TIPO_RESIDUO_ID" 
                                                                SortExpression="TIPO_RESIDUO_ID"/>
                                                            
                                                            <asp:BoundField DataField="NOMBRE" HeaderText="Residuo" 
                                                                SortExpression="NOMBRE" >
                                                                 <HeaderStyle Font-Size="13px" Height="95%" HorizontalAlign="Left" />
                                                                <ItemStyle Font-Size="12px" Height="95%" HorizontalAlign="Left" />
                                                            </asp:BoundField>  
                                                            
                                                            <asp:TemplateField  HeaderText= ""  HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="12px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Btn_Quitar_Tipo_Residuo" runat="server" Width="20px" Height="20px"
                                                                        ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                        AlternateText="Quitar Documento" onclick="Btn_Quitar_Tipo_Residuo_Click" /> 
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="center" Width="5%" />
                                                                <ItemStyle HorizontalAlign="center" Width="5%" />
                                                            </asp:TemplateField> 
                                                        </Columns> 
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                                
                                      
                                <table class="estilo_fuente" width="100%">   
                                    
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Metodo de separacion de residuos
                                        </td> 
                                         <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Aprovechamiento_Metodo_Sepearacion" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td> 
                                         <td style="width:15%" align="right">
                                            Quien presenta el servicio de recoleccion
                                        </td> 
                                         <td style="width:35%" align="left">
                                           <asp:TextBox ID="Txt_Aprovechamiento_Servicio_Recoleccion" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td> 
                                    </tr>  
                                </table>     
                                <table class="estilo_fuente" width="100%">       
                                    <tr>
                                        <td style="width:25%" align="left">
                                            Se revuelven liquidos con solidos
                                        </td> 
                                         <td style="width:25%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Aprovechamiento_Revuelven_Liquidos" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td> 
                                         <td style="width:15%" align="right">
                                            Tipo de contenedor
                                        </td> 
                                         <td style="width:35%" align="left">
                                           <asp:TextBox ID="Txt_Aprovechamiento_Tipo_Contenedor" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td> 
                                    </tr>   
                                    <tr>
                                        <td style="width:25%" align="left">
                                            Conexion a drenaje
                                        </td> 
                                         <td style="width:25%" align="left">
                                            <asp:RadioButtonList ID="RBtn_Aprovechamiento_Conexion_Drenaje" runat="server">
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td> 
                                         <td style="width:15%" align="right">
                                            Letrina, fosa septica, etc
                                        </td> 
                                         <td style="width:35%" align="left">
                                           <asp:TextBox ID="Txt_Aprovechamiento_Letrina" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td> 
                                    </tr>   
                                </table>
                                <table class="estilo_fuente" width="100%">       
                                    <tr>
                                        <td style="width:15%" align="left">
                                            Tipo de ruido
                                        </td> 
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Aprovechamiento_Tipo_Ruido" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td> 
                                         <td style="width:15%" align="right">
                                            Nivel de ruido
                                        </td> 
                                         <td style="width:35%" align="left">
                                           <asp:TextBox ID="Txt_Aprovechamiento_Nivel_Ruido" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                        </td>  
                                    </tr>   
                                </table>
                                <table class="estilo_fuente" width="100%">       
                                    <tr> 
                                         <td style="width:15%" align="left">
                                            Horario de labores Inicio
                                        </td> 
                                        <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Aprovechamiento_Horario_Inicial" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                                <cc1:MaskedEditExtender ID="Mee_Txt_Aprovechamiento_Horario_Inicial"    runat="server"
                                                    Mask="99:99:99" 
                                                    TargetControlID="Txt_Aprovechamiento_Horario_Inicial"    
                                                    MaskType="Time"  
                                                    AcceptNegative="None" AcceptAMPM="true"  />
                                         </td>
                                        <td style="width:15%" align="right">
                                            Horario de labores Fin
                                        </td> 
                                         <td style="width:35%" align="left">
                                            <asp:TextBox ID="Txt_Aprovechamiento_Horario_Final" runat="server" Enabled="false" Width="95%"></asp:TextBox> 
                                                <cc1:MaskedEditExtender ID="Mee_Txt_Aprovechamiento_Horario_Final"    runat="server"
                                                    Mask="99:99:99"
                                                    TargetControlID="Txt_Aprovechamiento_Horario_Final"    
                                                    MaskType="Time"  
                                                    AcceptNegative="None" AcceptAMPM="true"  />
                                        </td>  
                                    </tr>   
                                </table>
                                <table class="estilo_fuente" width="100%">       
                                    <tr>
                                        <td style="width:16%" align="left">
                                            Dias
                                        </td> 
                                        <td style="width:17%" align="left">
                                             <asp:CheckBox ID="Chk_Dias_Labor_Lunes" runat="server" Text="Lunes" />
                                        </td> 
                                        <td style="width:17%" align="left">
                                            <asp:CheckBox ID="Chk_Dias_Labor_Martes" runat="server" Text="Martes" />
                                        </td> 
                                        <td style="width:17%" align="left">
                                            <asp:CheckBox ID="Chk_Dias_Labor_Miercoles" runat="server" Text="Miercoles" />
                                        </td> 
                                        <td style="width:17%" align="left">
                                            <asp:CheckBox ID="Chk_Dias_Labor_Jueves" runat="server" Text="Jueves" />
                                        </td> 
                                        <td style="width:16%" align="left">
                                            <asp:CheckBox ID="Chk_Dias_Labor_Viernes" runat="server" Text="Viernes" />
                                        </td> 
                                    </tr>   
                                </table>
                                 <table class="estilo_fuente" width="100%">       
                                    <tr> 
                                         <td style="width:20%" align="left">
                                            Colindancia a los lados y partes posteriores
                                        </td> 
                                        <td style="width:80%" align="left">
                                             <asp:TextBox ID="Txt_Aprovechamiento_Colindancia" runat="server" Enabled="false" Width="98%"></asp:TextBox> 
                                        </td>
                                    </tr> 
                                    <tr> 
                                         <td style="width:20%" align="left">
                                            Emisiones a la atmosfera
                                        </td> 
                                        <td style="width:80%" align="left">
                                             <asp:TextBox ID="Txt_Aprovechamiento_Emisiones_Atmosfera" runat="server" Enabled="false" Width="98%"></asp:TextBox> 
                                              <cc1:FilteredTextBoxExtender ID="Fte_Txt_Aprovechamiento_Emisiones_Atmosfera" runat="server" 
                                                     FilterType="Custom, Numbers" TargetControlID="Txt_Aprovechamiento_Emisiones_Atmosfera"  Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>   
                                </table>                                
                            </asp:Panel>
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel> 
                
                <cc1:TabPanel runat="server" HeaderText=""  ID="TabPnl_Observaciones"  Width="100%"  >
                    <HeaderTemplate>Observaciones</HeaderTemplate>
                    <ContentTemplate> 
                       <div id="Div_Observaciones" runat="server" style="display:block">
                            <asp:Panel ID="Pnl_Observaciones_Para_Inspector" runat="server" GroupingText="Observaciones para el inspector" ForeColor="Blue">
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            <asp:TextBox ID="Txt_Observaciones_Para_Inspector" runat="server" Rows="3" 
                                                TextMode="MultiLine" Width="99%"></asp:TextBox>
                                               
                                        </td> 
                                    </tr>   
                                </table>
                            </asp:Panel> 
                            
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td rowspan="5">
                    
                                     </td>
                                </tr>
                            </table>
                                
                            <asp:Panel ID="Pnl_Observaciones_Del_Inspector" runat="server" GroupingText="Observaciones del inspector" ForeColor="Blue">
                                <table class="estilo_fuente" width="100%">  
                                    <tr>
                                        <td style="width:15%" align="left">
                                            <asp:TextBox ID="Txt_Observaciones_Del_Inspector" runat="server" Rows="3" 
                                                TextMode="MultiLine" Width="99%"></asp:TextBox>
                                              
                                        </td> 
                                    </tr>   
                                </table>                      
                             </asp:Panel> 
                        </div>
                    </ContentTemplate>             
                </cc1:TabPanel>   
            </cc1:TabContainer>
        </div> 
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>