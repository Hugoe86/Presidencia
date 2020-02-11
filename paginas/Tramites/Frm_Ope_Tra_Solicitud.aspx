<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Solicitud.aspx.cs" Inherits="paginas_Tramites_Frm_Ope_Tra_Solicitud"
    Title="Solicitud de Trámite" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
        <div style="width: 100%;">
            <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
            <div id="Div_General" runat="server"  style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                
            <div style="width: 100%;">  
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td align="center" class="label_titulo">
                            Solicitud de Tr&aacute;mite
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" ></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" 
                                            onclick="Btn_Guardar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right">
                                      <%--  Búsqueda por:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Solicitud" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Ingrese folio de solicitud" TargetControlID="Txt_Busqueda">
                                        </cc1:TextBoxWatermarkExtender>                                                                                                                                     
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            TabIndex="1" OnClick="Btn_Buscar_Click" />--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
                <div id="Div_Link_Busqueda_Tramite" runat="server" style="display:block">
                    <table width="100%">
                        <tr>
                            <td style="width:20%">
                                <asp:LinkButton ID="Btn_Busqueda_Tramite" runat="server" ForeColor="Blue"
                                OnClick="Btn_Link_Busqueda_Tramite_Click">Búsqueda Trámite</asp:LinkButton>  
                            </td>
                            <td style="width:20%">
                                <asp:HiddenField ID="Hdf_Ciudadano_ID" runat="server" />
                            </td>
                            <td style="width:60%">
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- elementos comentados porque se agrego una ventana emergente para la busqueda de los tramites
                <div id="Div_Consultar_Tramite" runat="server" style="color: #5D7B9D;display:none" > 
                    <asp:Panel ID="Pnl_Consultar_Tramite" runat="server" GroupingText="Buscar trámite" Width="98%" > 
                        
                        <table width="100%">
                            <tr style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                                
                                <td style="width:100%" align="right">
                                
                                     <asp:ImageButton ID="Btn_Buscar_Tramite_Filtro" runat="server"
                                        CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                        OnClick="Btn_Buscar_Tramite_Filtro_Click" />
                                        
                                    <asp:ImageButton ID="Btn_Limpiar_Busqueda_Avanzada" runat="server" ToolTip="Limpiar"
                                        ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                                        OnClientClick="javascript:return Limpiar_Filtros_Busqueda();" />
                                        
                                    <asp:ImageButton ID="Btn_Cerrar_Busqueda_Avanzada" runat="server" ToolTip="Cerrar"
                                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"  
                                        onclick="Btn_Cerrar_Busqueda_Avanzada_Click"   /> 
                                </td>
                            </tr>
                        </table>
                        
                        <table width="99%">   
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Nombre_Tramite_Filtro" runat="server"  Text="Nombre del trámite" Width="100%"></asp:Label> 
                                </td>
                                <td style="width:80%">
                                    <asp:TextBox ID="Txt_Nombre_Tramite_Filtro" runat="server" Width="98%"></asp:TextBox>
                                </td> 
                                <td style="width:5%" rowspan="2" align="center">
                                   
                                </td>
                            </tr> 
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Clave_Tramite_Filtro" runat="server"  Text="Clave del trámite" Width="100%"></asp:Label> 
                                </td>
                                <td style="width:80%">
                                    <asp:TextBox ID="Txt_Clave_Tramite_Filtro" runat="server" Width="35%" MaxLength="5" onkeyup='this.value = this.value.toUpperCase();'> </asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Unidad_Responsable_Filtro" runat="server"  Text="U. Responsable" Width="100%"></asp:Label> 
                                </td>
                                <td style="width:80%">
                                    <cc1:ComboBox ID="Cmb_Unidad_Responsable_Filtro" runat="server" width="450px"
                                        AutoPostBack="False" 
                                        DropDownStyle="DropDownList" 
                                        AutoCompleteMode="SuggestAppend" 
                                        CaseSensitive="False" 
                                        CssClass="WindowsStyle" 
                                        ItemInsertLocation="Append"/> 
                                </td>
                            </tr>
                        </table>
                        
                        <table class="estilo_fuente" width="100%">      
                            <tr>
                                <td style="width:100%;text-align:center;vertical-align:top;">
                                    <center>
                                        <div id="Div_Tramites_Generales" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">                              
                                            <asp:GridView ID="Grid_Tramites_Generales" runat="server" CssClass="GridView_1"
                                                AutoGenerateColumns="False"  Width="98%"
                                                GridLines= "None"
                                                EmptyDataText="No se encuentra ningun trámite "
                                                onselectedindexchanged="Grid_Tramites_Generales_SelectedIndexChanged" >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="TRAMITE_ID" HeaderText="Tramite ID"
                                                        SortExpression="TRAMITE_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CLAVE_TRAMITE" HeaderText="Clave Tramite" 
                                                        SortExpression="Clave_Tramite">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Size="13px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="12px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                        SortExpression="Nombre">
                                                        <HeaderStyle HorizontalAlign="Left" Width="75%" Font-Size="13px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="75%" Font-Size="12px" />
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
                    </asp:Panel>
                </div> 
                --%>

                <div id="Div_Tramite_ID" runat="server" style="display:none">
                    <table width="100%">
                            <tr>
                                <td style="width:100%">
                                    <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                                </td>
                            </tr>
                    </table>
                    
                    <table width="100%">
                        <tr>
                            <td style="width:15%">
                                Tr&aacute;mite
                            </td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Tramite" runat="server" Width="96%" OnSelectedIndexChanged="Cmb_Tramite_SelectedIndexChanged"
                                    AutoPostBack="True" Enabled="False">
                                </asp:DropDownList>
                            </td>
                            <td style="width:15%" align="right"></td>
                            <td style="width:35%"></td>
                        </tr>
                    </table>
                </div>
                    
                <div id="Div_Combo_Tramites" runat="server" style="display:block">  
                </div>
                
                
                
                
                <table align="center" width="97%">                                 
                    <tr>
                        <td  align="left" style="width:15%;">
                           <asp:Label ID="Lbl_Clave_Tramite" runat="server" Text="*Clave Trámite" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td align="left" style="width:35%;">
                            <asp:TextBox ID="Txt_Clave_Tramite" runat="server" Width="90%"
                                style="text-transform:uppercase" Enabled="false" />
                        </td>
                        <td  align="left" style="width:15%;">
                            <asp:Label ID="Lbl_Costo" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  align="left" style="width:35%;">
                            <asp:TextBox ID="Txt_Costo" runat="server" Enabled="False" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
               
                    <tr>
                        <td style="width:15%; text-align:left;">
                           <asp:Label ID="Lbl_Nombre_Tramite" runat="server" Text="*Nombre" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%; text-align:left " >
                            <asp:TextBox ID="Txt_Nombre_Tramite" runat="server" Width="90%" Rows="2"  TextMode="MultiLine" MaxLength="400" Enabled="false" />
                        </td>
                         <td style="width:15%; text-align:left ">
                            <asp:Label ID="Lbl_Tiempo_Estimado" runat="server" Text="*Tiempo Estimado (d&iacute;as)" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%; text-align:left ">
                            <asp:TextBox ID="Txt_Tiempo_Estimado" runat="server" Enabled="False" Width="90%"></asp:TextBox>
                        </td>
                    </tr> 
                </table>
                <table align="center" width="97%">     
                    <tr>
                        <td style="width:15%; text-align:left; vertical-align:top;">
                            <asp:Label ID="Lbl_Descripcion" runat="server" 
                                Text="*Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:85%; text-align:left ">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="2" TextMode="MultiLine" Width="96%" Enabled="false" />
                        </td>                            
                    </tr>
                </table>
                
               
                
                <asp:Panel ID="Pnl_Datos_Solicitud" runat="server" GroupingText="Datos de Solicitud"  style="display: none ">
                      <table width="100%">
                        <tr>
                            <td style="width:15%; text-align:left ">
                                Folio
                            </td>
                            <td style="width:35%; text-align:left ">
                                <asp:TextBox ID="Txt_Folio" runat="server" Width="95%" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width:15%; text-align:left ">
                            </td>
                            <td style="width:35%; text-align:left ">
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                Avance 
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Avance" runat="server" Enabled="False" Width="35%"></asp:TextBox>
                                &nbsp;(%)
                            </td>
                            <td style="width:15%" align="right">
                                Estatus
                            </td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="87%" Enabled="False" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <table width="100%">
                    <tr>
                        <td rowspan="3">
                        </td>
                    </tr>
                </table>
                
                
                
                 <div id="Div_Link_Buscar_Ciudadano" runat="server" style="display:none">
                    <table width="100%">
                        <tr>
                            <td style="width:100%">
                                <asp:LinkButton ID="Btn_Buscar_Ciudadano" runat="server" ForeColor="Blue">
                                    Buscar Ciudadano
                                </asp:LinkButton>  
                            </td>
                        </tr>
                    </table>
                </div>
                
                 <table width="100%">
                    <tr>
                        <td >
                            <asp:LinkButton ID="Btn_Link_Busqueda_Ciudadano" runat="server" ForeColor="Blue"
                                onclick="Btn_Link_Busqueda_Ciudadano_Click">Búsqueda Ciudadano</asp:LinkButton>  
                        </td> 
                    </tr>
                </table>
                    
                <asp:Panel ID="Pnl_Datos_Solicitante" runat="server" GroupingText="Datos Generales">
                    <table width="100%">
                        <tr>
                            <td style="width:15%">
                                Nombre
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%" Enabled="False" 
                                    ontextchanged="Txt_Nombre_TextChanged" AutoPostBack="True" ></asp:TextBox>
                                <asp:HiddenField ID="Hdf_Usuario_ID" runat="server" />
                            </td> 
                            <td style="width:15%" align="left">
                                Apellido Paterno
                            </td>
                            <td style="width:35%">
                                 <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="85%" 
                                    ontextchanged="Txt_Apellido_Paterno_TextChanged" AutoPostBack="True"
                                      Enabled="False"></asp:TextBox><%----%>
                            </td>              
                        </tr>
                     
                        <tr>
                        
                            <td>
                                Apellido Materno
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="90%" 
                                    ontextchanged="Txt_Apellido_Materno_TextChanged" AutoPostBack="True"
                                     Enabled="False"></asp:TextBox><%----%>
                            </td>
                            <td align="left">
                                Entidad federativa
                            </td>
                            <td>
                               <asp:DropDownList ID="Cmb_Estado" runat="server" Width="87%" Enabled="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Estado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           <%-- --%>
                                               
                        </tr>
                        <tr>  
                            <td>
                                Sexo
                            </td>
                            <td>
                                <asp:DropDownList id="Cmb_Sexo" runat = "server" Width="92%" Enabled="False">
                                    <asp:ListItem Value="FEMENINO">Femenino</asp:ListItem>
                                    <asp:ListItem Value = "MASCULINO">Masculino</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                Fecha Nacimiento
                            </td>
                            <td>
                                 <asp:TextBox id="Txt_Fecha_Nacimiento" runat = "server" Width="85%" MaxLength="18"
                                     ontextchanged="Txt_Fecha_Nacimiento_TextChanged" AutoPostBack="True"/>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Nacimiento_FilteredTextBoxExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Nacimiento" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                        ValidChars="/_" />
                                    <cc1:CalendarExtender ID="Txt_Fecha_Nacimiento_CalendarExtender" runat="server" 
                                        TargetControlID="Txt_Fecha_Nacimiento" PopupButtonID="Btn_Fecha_Nacimiento" Format="dd/MMM/yyyy" />
                                    <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha de nacimiento" />
                            </td>
                        </tr>
                        <tr> 
                            <td align="left">
                                E-mail
                            </td>
                            <td>
                                 <asp:TextBox ID="Txt_Email" runat="server" Width="90%"  
                                     Enabled="False"></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="Ftbe_Email" runat="server" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Email" ValidChars=".@_">
                                    </cc1:FilteredTextBoxExtender>
                            </td>
                            <td> Edad</td>
                            <td>
                                <asp:TextBox id="Txt_Edad" runat = "server" Width="85%" MaxLength="2" Enabled="False"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                     runat="server" 
                                     FilterType="Custom,Numbers"
                                     TargetControlID="Txt_Edad" 
                                     ValidChars="0123456789"/>
                            </td>
                        </tr>
                         <tr> 
                            <td>RFC</td>
                            <td> 
                                <asp:TextBox id="Txt_Rfc" runat = "server" MaxLength="20" Width="90%" 
                                    onkeyup='this.value = this.value.toUpperCase();' Enabled="False"></asp:TextBox>    
                            </td>
                             <td align="left">CURP</td>
                            <td> 
                                <asp:TextBox id="Txt_Curp" runat = "server" MaxLength="20" Width="85%" onkeyup='this.value = this.value.toUpperCase();' 
                                    Enabled="False"></asp:TextBox>    
                            </td>
                        </tr> 
                        
                        <tr>
                            <td align="left"> 
                                Telefono
                            </td>
                            <td>
                              <asp:TextBox id="Txt_Telefono_Casa" runat = "server" Width="90%" MaxLength="10" Text="Irapuato"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                     runat="server" 
                                     FilterType="Custom,Numbers"
                                     TargetControlID="Txt_Telefono_Casa" 
                                     ValidChars="0123456789"/>
                            </td>                            
                        </tr> 
                            
                    </table>
                </asp:Panel>
                            
                       
                <asp:Panel ID="Pnl_Datos_Ciudadano_Domicilio" runat="server" GroupingText="Domicilio" >
                    <div id="Div_Direccion_Seperada" runat="server" style="display:block;"> 
                        <table width="100%">
                            <tr>    
                                <td style="width:15%">Colonia</td>
                                <td style="width:85%">
                                    <asp:DropDownList ID="Cmb_Colonias" runat="server"  width="95%"
                                            AutoPostBack="true" 
                                            DropDownStyle="DropDownList" 
                                            AutoCompleteMode="SuggestAppend" 
                                            CaseSensitive="False" 
                                            CssClass="WindowsStyle"
                                            OnSelectedIndexChanged="Cmb_Colonias_SelectedIndexChanged"
                                            /> 
                                            <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Colonia_Click" />
                                </td>
                            </tr>
                            <tr>      
                                <td style="width:15%" align="left">Calle</td>
                                <td style="width:85%">
                                    <asp:DropDownList ID="Cmb_Calle" runat="server" width="95%"
                                            AutoPostBack="false" 
                                            DropDownStyle="DropDown" 
                                            AutoCompleteMode="SuggestAppend" 
                                            CaseSensitive="False" 
                                            CssClass="WindowsStyle"  /> 
                                        <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                            OnClick="Btn_Buscar_Calles_Click" />
                                </td>
                            </tr>
                        </table> 
                        
                        <table width="100%">      
                            <tr>
                                <td style="width:15%">Numero</td>
                                <td style="width:35%">
                                       <asp:TextBox id="Txt_Numero" runat = "server" Width="90%" MaxLength="50" 
                                        Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                         runat="server" 
                                         FilterType="Custom,Numbers"
                                         TargetControlID="Txt_Numero" 
                                         ValidChars="0123456789"/>
                                </td>
                                <td style="width:15%" align="left">CP</td>
                                <td style="width:35%">
                                     <asp:TextBox id="Txt_CP" runat = "server" Width="85%" MaxLength="5" Enabled="False" 
                                        onkeyup='this.value = this.value.toUpperCase();'></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                         runat="server" 
                                         FilterType="Custom,Numbers"
                                         TargetControlID="Txt_CP" 
                                         ValidChars="0123456789"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                            
                    <div id="Div_Direccion_Completa" runat="server" style="display:none">   
                         <table width="100%">   
                            <tr>
                                <td style="width:15%">Calle</td>
                                <td style="width:85%">
                                     <asp:TextBox ID="Txt_Direccion_Completa" runat="server" Width="95%"/> 
                                </td>
                                 
                            </tr>
                            <tr>
                                <td style="width:15%" >Colonia</td>
                                <td style="width:85%">
                                     <asp:TextBox ID="Txt_Colonia" runat="server" Width="95%"/> 
                                </td>
                               
                            </tr>
                        </table>
                    </div>
                        
                    <table width="100%"> 
                        <tr>
                            <td style="width:15%">Ciudad</td>
                            <td style="width:35%">
                                <asp:TextBox id="Txt_Ciudad" runat = "server" Width="90%" MaxLength="50" Enabled="False" 
                                    Text="IRAPUATO" onkeyup='this.value = this.value.toUpperCase();'>
                                </asp:TextBox>
                            </td>
                            <td style="width:15%" align="left">Estado</td>
                            <td style="width:35%">
                                <asp:TextBox id="Txt_Estado" runat = "server" Width="85%" MaxLength="50" Enabled="False" 
                                    Text="GUANAJUATO"
                                    onkeyup='this.value = this.value.toUpperCase();'></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <asp:Panel ID="Pnl_Cuenta_Predial" runat="server" GroupingText="Datos del inmueble" Visible="false" style="margin-top:8px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%">
                                    Cuenta Predial
                                    <asp:HiddenField ID="Hdf_Cuenta_Predial" runat="server" />
                                    <asp:HiddenField ID="Hdf_Dependencia_ID" runat="server" />
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="90%" Enabled="true" AutoPostBack="true"
                                        MaxLength="20" OnTextChanged="Txt_Cuenta_Predial_TextChanged" ToolTip="Ingrese la cuenta predial"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                        FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                                    <asp:UpdatePanel ID="Upnl_Cuenta_Predial" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="Btn_Buscar_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px"
                                                 Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 15%" align="left">
                                </td>
                                <td style="width:35%">
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    Propietario
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Propietario_Cuenta_Predial" runat="server" Width="90%" Enabled="false">
                                    </asp:TextBox>
                                </td>
                                <td style="width:15%">
                                    Colonia
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Direccion_Predio" runat="server" Width="90%" >
                                    </asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%">
                                    Calle
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Calle_Predio" runat="server" Width="90%" >
                                    </asp:TextBox>
                                </td>
                                <td style="width:15%">
                                    Numero
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Numero_Predio" runat="server" Width="90%" >
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    Manzana
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Manzana_Predio" runat="server" Width="90%" >
                                    </asp:TextBox>
                                </td>
                                <td style="width:15%">
                                    Lote
                                </td>
                                <td style="width:35%">
                                    <asp:TextBox ID="Txt_Lote_Predio" runat="server" Width="90%" >
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    Otros
                                </td>
                                <td colspan="3" style="width:85%">
                                    <asp:TextBox ID="Txt_Otros_Predio" runat="server" TextMode="MultiLine" MaxLength="1000" Width="100%"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Otros_Predio_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                InvalidChars="&lt;,&gt;,&amp;,',!," TargetControlID="Txt_Otros_Predio" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                     </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    Perito&nbsp;
                                </td>
                                <td colspan="2" style="width:50%">
                                    <asp:DropDownList ID="Cmb_Perito" runat="server" Width="70%">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="Btn_Buscar_Perito" runat="server" ToolTip="Seleccionar un Perito"
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                        OnClick="Btn_Buscar_Perito_Click"  />
                                </td>
                                <td >
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
             </div>
             </div>
        
        
            <div style="width: 100%;">   
                <table width="100%">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center">
                            
                            <asp:Label ID="Lbl_Datos_Requeridos" runat="server" Font-Bold="True" 
                                Font-Size="Small" Text="Datos Requeridos" Visible="False" 
                                ForeColor="#FF8FBC8F"></asp:Label>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center">
                            <div id="Div_Grid_Datos_Tramite" runat="server" 
                                style="overflow:auto;height:150px;width:95%;vertical-align:top;border-style:solid;border-color:Silver;display:none">
                                <asp:GridView ID="Grid_Datos" runat="server" AllowPaging="false" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    Width="97%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Nombre" HeaderText="Datos" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Descripción">
                                            <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                            <ItemStyle HorizontalAlign="Right"  Width="70%"/>
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="<%# 500 %>"></asp:TextBox>
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
                    <tr>
                        <td>
                            &nbsp;
                            </td>
                    </tr>
                    <tr>
                        <td align="center">
                         
                            <asp:Label ID="Lbl_Documentos_Requeridos" runat="server" Font-Bold="True" 
                                Font-Size="Small" Text="Documentos Requeridos" Visible="False" 
                                ForeColor="#FF8FBC8F"></asp:Label>
                         
                        </td>
                    </tr>
                    
                     <tr>
                        <td align="left">
                         
                            <asp:Label ID="Lbl_Mensaje_Documentos" runat="server" Font-Bold="True" 
                                Font-Size="Small" Text=" Archivos en formato PDF, JPG ó JPEG </br> Nota: Las filas que estan marcadas con color Azul son opcionales a subir dependiendo de la situación." Visible="False" 
                                ForeColor="#FF8FBC8F"></asp:Label>
                         
                        </td>
                    </tr>
                   
                    
                    <tr>
                        <td align="center">
                            <div id="Div_Grid_Documentos" runat="server" 
                                style="overflow:auto;height:150px;width:95%;vertical-align:top;border-style:solid;border-color:Silver;display:none">
                                <asp:GridView ID="Grid_Documentos" runat="server"  AutoGenerateColumns="False" 
                                    onrowdatabound="Grid_Documentos_RowDataBound"
                                    CssClass="GridView_1" GridLines="None" Width="97%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" Font-Size="13px"/>
                                            <ItemStyle HorizontalAlign="Left" Width="30%" Wrap = "true" Font-Size="12px"/>
                                        </asp:BoundField>
                                       <asp:TemplateField HeaderText="Ruta del archivo"  
                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70%"
                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70%"> 
                                                <ItemTemplate>
                                                
                                                    <cc1:AsyncFileUpload ID="FileUp" runat="server" PersistFile="true" Width="90%"
                                                        UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen"/>
                                                         
                                                    <asp:TextBox ID="Txt_Url" runat="server" Width="98%" ></asp:TextBox>
                                                    
                                                    <asp:ImageButton ID="Btn_Acutalizar_Documento" runat="server" AlternateText="Ver" 
                                                            ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" Width="24px" Height="24px"
                                                            onclick="Btn_Actualizar_Documento_Click" />
                                                                
                                                    <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" 
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Width="24px" Height="24px" 
                                                            onclick="Btn_Ver_Documento_Click" />   
                                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <%-- 3 --%>                                           
                                            <asp:BoundField DataField="DOCUMENTO_REQUERIDO" HeaderText="" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" Wrap="true" />
                                            </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                            
                            <div id="Div_Grid_Documentos_Modificar" runat="server" 
                                style="overflow:auto;height:150px;width:95%;vertical-align:top;border-style:solid;border-color:Silver;display:none">
                                <asp:GridView ID="Grid_Documentos_Modificar" runat="server"  AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="50%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="90%"/>
                                            <ItemStyle HorizontalAlign="Left" Wrap = "true" Width="90%"/>
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
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>                                               
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        </div>
</asp:Content>