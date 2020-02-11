<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master" 
AutoEventWireup="true" CodeFile="Frm_Rpt_Ven_Consultar_Tramites.aspx.cs" Inherits="paginas_Ventanilla_Frm_Rpt_Ven_Consultar_Tramites" 
Title="Consultar Trámites"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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

        <script type="text/javascript" language="javascript">
            //Abrir una ventana modal
            function Abrir_Ventana_Modal(Url, Propiedades)
            {
                window.showModalDialog(Url, null, Propiedades);
            }

            function Abrir_Resumen(Url, Propiedades) {
                window.open(Url, 'Resumen_Predio', Propiedades);
            }
            </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
   
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"/>

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
            
                <div id="Div_General" runat="server"  style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr align="center">
                            <td  colspan="2" class="label_titulo">Consultar Tr&aacutemites
                            </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td colspan="2" >
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                        
                        <tr class="barra_busqueda" align="right" >
                            <td  align="left">
                               <asp:ImageButton ID="Btn_Modificar" runat="server"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                            AlternateText="Modificar" onclick="Btn_Modificar_Click" Visible="false"/>
                                            
                                <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                            OnClick="Btn_Generar_Reporte_Click" ToolTip="Vista previa" Width="24px" Height="24px"
                                            Style="cursor: hand;" Visible="false"/>
                                            
                                <asp:ImageButton ID="Btn_Salir" runat="server" 
                                    CssClass="Img_Button" 
                                    ToolTip="Salir"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                    onclick="Btn_Salir_Click"/>
                                
                               <asp:ImageButton ID="Btn_Copiar" runat="server" 
                                      ImageUrl="~/paginas/imagenes/paginas/icono_copiar.jpeg" Width="25px" 
                                      Height="25px" OnClientClick="return confirm('¿Esta seguro que desea generar una nueva solicitud a partir de esta?');" 
                                      onclick="Btn_Copiar_Click" ToolTip="Copiar Tramite" /> 
                             </td> 
                             
                             <td>
                               
                             <td>                                                   
                        </tr>
                    </table>
                    
                    <div id="Div_Busqueda_Sin_Usuario_Registrado" runat="server" style="display:none">
                        <table class="estilo_fuente" width="100%">  
                            <tr>
                                <td style="width:20%">
                                    <asp:Label ID="Lbl_Clave_Solicitud" runat="server" Text="Clave de la solicitud" ></asp:Label>
                                </td>
                                 <td style="width:80%">
                                    <asp:TextBox ID="Txt_Clave_Solicitud" runat="server"  Width="95%" MaxLength="12" TabIndex="1"
                                        ></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Consulta_Clave" runat="server" ToolTip=""
                                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Height="22px" Width="22px"
                                                    OnClick="Btn_Buscar_Click" TabIndex="2"/>    
                                                    
                                    <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                                    <asp:HiddenField ID="Hdf_Dependencia_ID" runat="server" />
                                    <asp:HiddenField ID="Hdf_Reporte_Orden_Pago" runat="server" />
                                    <asp:HiddenField ID="Hdf_Propietario_Cuenta_Predial" runat="server" />
                                                    
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="Div_Filtros" runat="server" style="display:none">
                       
                        <table class="estilo_fuente" width="100%">  
                           
                            <tr>  
                                <td style="width:15%"  align ="left">
                                        <asp:Label ID="Lbl_Unidad_Responsable_Filtro" runat="server"  Text="U. Responsable" Width="100%"></asp:Label> 
                                </td>
                                <td style="width:85%">
                                        <asp:DropDownList  ID="Cmb_Unidad_Responsable_Filtro" runat="server" Width="96%" 
                                            AutoPostBack="true" 
                                            DropDownStyle="DropDownList" 
                                            AutoCompleteMode="SuggestAppend" 
                                            CaseSensitive="False" 
                                            CssClass="WindowsStyle" 
                                            OnSelectedIndexChanged="Cmb_Unidad_Responsable_Filtro_SelectedIndexChanged"
                                            ItemInsertLocation="Append"/>
                                        <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Dependencia_Click" /> 
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Nombre_Tramite" runat="server" Text="Nombre del tramite" ></asp:Label>
                                </td>
                                 <td style="width:85%">
                                    <asp:TextBox ID="Txt_Nombre_Tramite" runat="server"  Width="95%"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Buscar" runat="server"  ToolTip="Buscar solicitud"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    OnClick="Btn_Buscar_Click" />
                                    
                                </td>
                            </tr>
                        </table>      
                        <table class="estilo_fuente" width="100%">  
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" ></asp:Label>
                                </td>
                                 <td style="width:85%">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="96%" AutoPostBack="true" 
                                        OnSelectedIndexChanged="Cmb_Estatus_SelectedIndexChanged">  
                                        <asp:ListItem Value="" Text="< SELECCIONE >" Selected="True" />                                     
                                        <asp:ListItem Value="PENDIENTE" Text="PENDIENTE"/>
                                        <asp:ListItem Value="PROCESO" Text="PROCESO"  />
                                         <asp:ListItem Value="DETENIDO" Text="DETENIDO" />
                                         <asp:ListItem Value="CANCELADO" Text="CANCELADO" />
                                         <asp:ListItem Value="TERMINADO" Text="TERMINADO" />
                                    </asp:DropDownList>
                                  <%--  <asp:ImageButton ID="Btn_Buscar_Estatus" runat="server"  ToolTip="Buscar solicitud"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    OnClick="Btn_Buscar_Click" />--%>
                                   
                                </td>
                              
                            </tr>
                        </table> 
                        <table class="estilo_fuente" width="100%">     
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio" ></asp:Label>
                                </td>
                                 <td style="width:35%">
                                    <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="80%" ></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Inicio" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                            ValidChars="/_" />
                                        <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy" />
                                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />  
                                   
                                </td>
                                
                                <td style="width:15%" align ="left">
                                        <asp:Label ID="Lbl_Fecha_Fin" runat="server"  Text="Fecha Fin" Width="100%"></asp:Label> 
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="81%" ></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Fin" runat="server" 
                                            TargetControlID="Txt_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                            ValidChars="/_" />
                                        <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender1" runat="server" 
                                            TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                                        <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />  
                                        
                                        <asp:ImageButton ID="Btn_Buscar_Fechas" runat="server"  ToolTip="Buscar solicitud"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            OnClick="Btn_Buscar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                     
                    <div id="Div_Grid" runat="server" style="display:block"> 
                        <asp:Panel ID="Pnl_Solicitud_Proceso" runat="server" GroupingText="Trámites en proceso"> 
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;">
                                        <center>
                                            <div id="Div_Tramites_Proceso" runat="server" 
                                                style="overflow:auto;height:450px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                                <asp:GridView ID="Grid_Consulta_Tramites" runat="server" Width="97%" 
                                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                                    GridLines="None"   AllowPaging="false"
                                                    AutoGenerateColumns="False" 
                                                    OnRowDataBound="Grid_Consulta_Tramites_RowDataBound"
                                                    OnSelectedIndexChanged="Grid_Consulta_Tramites_SelectedIndexChanged"
                                                    EmptyDataText="No se encuentra ningun trámite"  >
                                                     <Columns>
                                                        <%-- 0 --%>
                                                        <asp:BoundField DataField="solicitud_id" HeaderText="solicitud_id"
                                                              HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="12px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>                                                        
                                                        <%-- 1 --%> 
                                                        <asp:TemplateField  HeaderText= ""  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                            <ItemTemplate>
                                                                    <asp:ImageButton ID="Btn_Consulta_Grid_Select" OnClick="Btn_Consulta_Grid_Selected_Click"
                                                                        ButtonType="Image" runat="server"  Width="16px" Height="16px"/> 
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                            <ItemStyle HorizontalAlign="center" Width="7%" />
                                                        </asp:TemplateField>                                                                                                                
                                                        <%-- 2 --%>
                                                        <asp:BoundField DataField="clave_solicitud" HeaderText="Clave"
                                                             HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="11px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                        </asp:BoundField>                                                        
                                                        <%-- 3 --%>  
                                                        <asp:BoundField DataField="nombre" HeaderText="Nombre del trámite"
                                                            HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="11px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                        </asp:BoundField>                                                        
                                                        <%-- 4 --%>
                                                        <asp:BoundField DataField="estatus" HeaderText="Estatus"
                                                             HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>                                                        
                                                        <%-- 5 --%> 
                                                         <asp:BoundField DataField="porcentaje_avance" HeaderText="Avance %" 
                                                             HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="11px">
                                                            <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="6%" />
                                                        </asp:BoundField>
                                                        <%-- 6 --%>
                                                        <asp:BoundField DataField="FECHA_TRAMITE" HeaderText="Fecha Solicitud" 
                                                             HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="11px" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}">
                                                            <HeaderStyle HorizontalAlign="Center" Width="17%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                        </asp:BoundField>
                                                        <%-- 7 --%>
                                                        <asp:BoundField DataField="COMPLEMENTO" HeaderText="COMPLEMENTO" 
                                                             HeaderStyle-Font-Size="13px"  ItemStyle-Font-Size="11px" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}">
                                                            <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                       
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                        
                    <%--**************************** INICIO campos ocultos  ****************************--%>
                    <div  id="Div_Campos_Ocultos" runat="server"  style="display:none">
                        <table width="100%">
                            <tr>
                             <td style="width:15%;">
                                    <asp:Label ID="Lbl_Detalle_Nombre_Solicitante" runat="server" Text="Nombre Solicitante" Width="100%"></asp:Label>
                                </td>
                                 <td style="width:35%;">
                                    <asp:TextBox ID="Txt_Detalle_Nombre_Solicitante" runat="server" Width="95%"></asp:TextBox>
                                </td>
                                </td>
                                <td style="width:15%;">
                                    <asp:Label ID="Lbl_Detalle_Email" runat="server" Text="Email Solicitante" Width="100%"></asp:Label>
                                </td>
                                 <td  style="width:35%;">
                                    <asp:TextBox ID="Txt_Detalle_Email" runat="server" Width="90%"></asp:TextBox>
                                </td>
                                
                            </tr>
                        </table>
                    </div>
                    <%--**************************** FIN campos ocultos  ****************************--%>
                        
                    <div id="Div_Detalles_Solicitud" runat="server" style="display:none">
                        <asp:Panel ID="Pnl_Datos_Generales_Solicitud" runat="server" GroupingText="Datos Generales">
                            
                            <table width="100%">
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Clave_Solicitud_Detalle" runat="server" Text="Clave" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Detalle_Clave" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                    </td>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Fecha_Inicio" runat="server" Text="Fecha Trámite" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                         <asp:TextBox ID="Txt_Detalle_Fecha_Inicio" runat="server" Width="90%" Font-Size="12px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="DTP_Txt_Detalle_Fecha_Inicio" runat="server" 
                                                TargetControlID="Txt_Detalle_Fecha_Inicio" Format="dd/MMM/yyyy" Enabled="false" />
                                            <asp:ImageButton ID="Btn_Detalle_Fecha_Inicio" runat="server"
                                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                Height="18px" CausesValidation="false" Enabled="true"/> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Estatus" runat="server" Text="Estatus" Width="100%"></asp:Label>  
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                    </td>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Porcentaje" runat="server" Text="Avance" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Detalle_Porcentaje" runat="server" Width="90%" Font-Size="12px" style="text-align:right"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Nombre" runat="server" Text="Trámite" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;" colspan="3">
                                        <asp:TextBox ID="Txt_Detalle_Nombre" runat="server" Width="96%" Font-Size="12px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Cantidad" runat="server" Text="Cantidad" Width="100%"></asp:Label> 
                                    </td>
                                     <td style="width:35%;">
                                         <asp:TextBox ID="Txt_Detalle_Cantidad" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Detalle_Cantidad"
                                                FilterType="Numbers,Custom, UppercaseLetters, LowercaseLetters" ValidChars="$0123456789. ">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Costo" runat="server" Text="Costo" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Detalle_Costo" runat="server" Width="90%" Font-Size="12px" Text="En base a la ley vigente"></asp:TextBox>
                                            <asp:HiddenField ID="Hdf_Costo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Detalle_Tiempo_Estimado" runat="server" Text="Tiempo Estimado" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                       <asp:TextBox ID="Txt_Detalle_Tiempo_Estimado" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                    </td>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Fecha_Entraga" runat="server" Text="Fecha entrega" Width="100%"></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Fecha_Entrega" runat="server" Width="90%" Font-Size="12px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Fecha_Entrega_CalendarExtender1" runat="server" 
                                                    TargetControlID="Txt_Fecha_Entrega" Format="dd/MMM/yyyy" Enabled="false" />
                                                <asp:ImageButton ID="Btn_Detalle_Fecha_Entrega" runat="server"
                                                    ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                    Height="18px" CausesValidation="false" Enabled="true"/> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%;">
                                        <asp:Label ID="Lbl_Actividad" runat="server" Text="Actividad Actual" Width="100%"></asp:Label>  
                                    </td>
                                     <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Detalle_Actividad" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                    </td>
                                    <td style="width:15%;">
                                          <asp:Label ID="Lbl_Siguiente_Subproceso" runat="server" Text="Siguiente Actividad" ></asp:Label>
                                    </td>
                                     <td style="width:35%;">
                                         <asp:TextBox ID="Txt_Siguiente_Subproceso" runat="server" Width="90%" Enabled="False" Font-Size="12px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                         <div id="Div_Centa_Predial" runat="server" style="display: none">
                            <asp:Panel ID="Pnl_Cuenta_Predial" runat="server" GroupingText="Datos adicionales" Style="display: block;">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 15%">
                                            Cuenta Predial
                                        </td>
                                        <td style="width: 85%" colspan="3">
                                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96%" Enabled="false" Font-Size="12px"
                                                MaxLength="20" ></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="15px"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Propietario
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Propietario_Cuenta_Predial" runat="server" Width="95%" Font-Size="12px"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            Colonia
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Direccion_Predio" runat="server" Width="90%" >
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Calle
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Calle_Predio" runat="server" Width="95%" >
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            Numero
                                        </td>
                                        <td style="width: 35%" colspan="2">
                                            <asp:TextBox ID="Txt_Numero_Predio" runat="server" Width="90%" MaxLength="9" >
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Manzana
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Manzana_Predio" runat="server" Width="95%" >
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            Lote
                                        </td>
                                        <td style="width: 35%" colspan="2">
                                            <asp:TextBox ID="Txt_Lote_Predio" runat="server" Width="90%" >
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Otros
                                        </td>
                                        <td colspan="3" style="width: 85%">
                                            <asp:TextBox ID="Txt_Otros_Predio" runat="server" TextMode="MultiLine" MaxLength="1000" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Perito
                                        </td>
                                        <td style="width: 85%" colspan="3">
                                            <asp:DropDownList ID="Cmb_Perito" runat="server" Width="96%" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width: 15%">
                                            
                                        </td >
                                        <td style="width: 35%">
                                            <asp:LinkButton ID="Btn_Link_Catastro" runat="server" ForeColor="Blue" Visible="false" Font-Size="12px"
                                                OnClick="Btn_Link_Catastro_Click"></asp:LinkButton>  
                                        </td>
                                        <td style="width: 15%">
                                            
                                        </td >
                                        <td style="width: 35%">
                                            <asp:LinkButton ID="Btn_Link_Catastro2" runat="server" ForeColor="Blue" Visible="false" Font-Size="12px"
                                                OnClick="Btn_Link_Catastro2_Click"></asp:LinkButton>  
                                        </td>
                                    </tr>
                                    
                                </table>
                            </asp:Panel>
                        </div>
                        
                        <table id="Tabla_Condicion" runat="server" width="100%" style="display:block"> 
                             <tr>
                                <td style="width: 15%;">
                                  
                                </td>
                                <td  style="width: 85%">
                                   
                                </td>
                            </tr>
                        </table>
                        
                        <asp:Panel ID="Pnl_Historial_Actividades" runat="server" GroupingText="Historial de actividades">
                            <table width="100%">
                                <tr>
                                    <td style="width:100%">
                                        <center>
                                            <div id="Div_Grid_Historial" runat="server"   style="overflow: auto; height: 150px; width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                                <asp:GridView ID="Grid_Historial_Actividades" runat="server" CssClass="GridView_1"
                                                    EmptyDataText="No se encuentran informacion de las actividades realizadas"
                                                    AutoGenerateColumns="False" Width="96%" GridLines= "None" >
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Actividad" HeaderText="Actividad" SortExpression="Actividad" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%"
                                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"/>
                                                        <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" SortExpression="Comentarios" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="45%"
                                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="45%"/>
                                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" 
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="15%"
                                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%"/>
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}"
                                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="20%"
                                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"/>
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
                        </asp:Panel>
                        
                        <asp:Panel ID="Pnl_Grid_Datos" runat="server" GroupingText="Datos" Style="display: block;">   
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <div id="Div_Grid_Datos_Tramite" runat="server" style="overflow: auto; height: 150px;width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                            <asp:GridView ID="Grid_Datos" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                OnRowDataBound="Grid_Datos_RowDataBound"
                                                EmptyDataText="No se encuentran datos de la solicitud"
                                                CssClass="GridView_1" GridLines="None" Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <%-- 0 --%>
                                                    <asp:BoundField DataField="ope_dato_id" HeaderText="ope_dato_id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%"   Font-Size="13px"/>
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" Wrap="true" Font-Size="12px"/>
                                                    </asp:BoundField>
                                                    <%-- 1 --%>
                                                    <asp:BoundField DataField="Dato_id" HeaderText="Dato_id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%"  Font-Size="13px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" Wrap="true" Font-Size="12px"/>
                                                    </asp:BoundField>
                                                    <%-- 2 --%>
                                                    <asp:BoundField DataField="nombre_Dato" HeaderText="Nombre Dato" Visible="True">
                                                        <HeaderStyle HorizontalAlign="Left" Width="25%" Font-Size="13px"/>
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" Font-Size="12px"/>
                                                    </asp:BoundField>
                                                    <%-- 3 --%>
                                                    <asp:TemplateField HeaderText="Descripción">
                                                        <HeaderStyle HorizontalAlign="Left" Width="75%" Font-Size="13px"/>
                                                        <ItemStyle HorizontalAlign="Left" Width="75%" Font-Size="12px"/>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="90%"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- 4 --%>
                                                    <asp:BoundField DataField="Valor" HeaderText="Datos" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%"  Font-Size="13px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" Wrap="true"  Font-Size="12px"/>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>                        
                        </asp:Panel>
                        
                        <asp:Panel ID="Pnl_Documentos_Anexados" runat="server" GroupingText="Documentos anexados" Style="display: block;">   
                            <table width="100%">
                                <tr>
                                    <td>
                                     <div id="Div_Documentos_Anexados" runat="server" style="border-style: solid; border-color: Silver; display: block">  
                                             <asp:GridView ID="Grid_Documentos_Tramite" runat="server" CssClass="GridView_1"
                                                onrowdatabound="Grid_Documentos_Tramite_RowDataBound"
                                                AutoGenerateColumns="False" Width="100%" GridLines= "None" 
                                                EmptyDataText="No se encuentran documentos anexados en la solicitud">
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <%-- 0 --%>
                                                    <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="DOCUMENTO_ID" 
                                                        SortExpression="OPE_DOCUMENTO_ID" />
                                                    <%-- 1 --%>
                                                    <asp:BoundField DataField="DOCUMENTO" HeaderText="Documento" SortExpression="DOCUMENTO" >
                                                        <ItemStyle HorizontalAlign = "Left"/>
                                                        <HeaderStyle HorizontalAlign = "Left"/>
                                                    </asp:BoundField>
                                                    <%-- 2 --%>    
                                                    <asp:TemplateField HeaderText="Ruta del archivo" ItemStyle-HorizontalAlign="Left"
                                                        ItemStyle-Font-Size="12px" ItemStyle-Width="25%" HeaderStyle-Font-Size="13px"
                                                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                                                <div id="Div2" class="progressBackgroundFilter"></div>
                                                                <div  class="processMessage" id="div3">
                                                                    <img alt="" src="../imagenes/paginas/Updating.gif" />
                                                                </div>
                                                            </asp:Label>
                                                            <cc1:AsyncFileUpload ID="FileUp" runat="server" Width="450px" PersistFile="true"
                                                                UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen" 
                                                                ThrobberID="Throbber" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- 3 --%>
                                                    <asp:TemplateField HeaderText="Ver">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                                onclick="Btn_Ver_Documento_Click" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign = "Center"/>
                                                        <ItemStyle HorizontalAlign = "Right"/>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>           
                        </asp:Panel>
                        
                        
                        <%--<asp:Panel ID="Pnl_Documentos_Seguimiento" runat="server" GroupingText="Documentos Creados en el Seguimiento">
                            <table width="100%">
                                <tr>
                                    <td style="width:100%">
                                        <div id="Div_Grid_Documentos_Seguimiento" runat="server" 
                                            style="overflow:auto;height:150px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                            <asp:GridView ID="Grid_Documentos_Seguimiento" runat="server" CssClass="GridView_1"
                                                EmptyDataText="No se encuentran documentos creados durante las fases del tramite"
                                                onrowdatabound="Grid_Documentos_Seguimiento_RowDataBound"
                                                AutoGenerateColumns="False" Width="98%" GridLines= "None"  >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento" SortExpression="NOMBRE_DOCUMENTO" 
                                                        HeaderStyle-Font-Size="13px" HeaderStyle-Width="90%" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-Font-Names="12px" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="URL" HeaderText="URL" SortExpression="URL" />
                                                    <asp:TemplateField HeaderText="Ver"
                                                        HeaderStyle-Font-Size="13px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Font-Names="12px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Ver_Documento_Seguimiento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                                onclick="Btn_Ver_Documento_Seguimiento_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>--%>
                        
                        
                        
                        
                         <div id="Div_Observaciones" runat="server" style="display:none">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Observaciones">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:TextBox ID="TextBox1" runat="server" 
                                                    Width="95%" Height="48px" TextMode="MultiLine" ></asp:TextBox>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        
                            
                    </div>
                    
                </div>
             </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>