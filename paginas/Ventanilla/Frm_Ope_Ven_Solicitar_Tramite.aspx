<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    CodeFile="Frm_Ope_Ven_Solicitar_Tramite.aspx.cs" Inherits="paginas_Ventanilla_Frm_Ope_Ven_Solicitar_Tramite"
    Title="Solicitar Tramites" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style> 
        fieldset {border:1px solid #A4A4A4} /*this is the border color*/ 
        legend {color:Black; font-weight:bold;} /* this is the GroupingText color */ 
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
          //Abrir una ventana modal
            function Abrir_Ventana_Modal(Url, Propiedades)
            {
                window.showModalDialog(Url, null, Propiedades);
            
            }
            
            function Abrir_Resumen(Url, Propiedades) {
                window.open(Url, 'Resumen_Predio' , Propiedades);
            }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
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
                <div id="Div_General" runat="server" style="background-color: #ffffff; width: 98%;
                    height: 100%;">
                    <%--Fin del div General--%>
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                        <tr align="center">
                            <td colspan="2" class="label_titulo">
                                Solicitud de Tr&aacute;mite
                            </td>
                        </tr>
                        <tr>
                            <!--Bloque del mensaje de error-->
                            <td colspan="2">
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" style="width: 80%">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClientClick="return confirm('¿Esta seguro que desea realizar el registro de la solicitud?');"
                                    OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ToolTip="Salir"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                            OnClick="Btn_Generar_Reporte_Click" ToolTip="Vista previa" Width="24px" Height="24px"
                                            Style="cursor: hand;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 20%">
                            </td>
                        </tr>
                    </table>
                   <%-- <table width="100%">
                        <tr>
                            <td  align="center">
                                <asp:Button ID="Btn_Regrasar" runat="server" Text="Regresar al menu principal" 
                                    OnClick="Btn_Regrasar_OnClick" CssClass="button"  style="width:100%;"/>
                            </td>
                        </tr>
                    </table>--%>
                  
                    <asp:Panel ID="Pnl_Datos_Solicitud" runat="server" GroupingText="Datos del tramite">
                          <table width="100%">
                        <tr>
                            <td style="width: 15%">
                                Folio
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Folio" runat="server" Width="84%" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 15%" align="left">
                                Estatus
                            </td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="87%" Enabled="False">
                                    <asp:ListItem>PENDIENTE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="width: 15%" align="left">
                                 <asp:Label ID="Lbl_Costo" runat="server" Text="Costo" ToolTip="Costo del tramite" ></asp:Label>
                                 <asp:HiddenField ID="Hdf_Costo" runat="server" />
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Costo" runat="server" Enabled="False" Width="84%" Text="En base a la ley vigente" ></asp:TextBox>
                            </td>  
                            <td style="width: 15%" >
                                Numero
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Cantidad_Solicitud" runat="server" Enabled="true" Width="84%" MaxLength="3" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cantidad_Solicitud" runat="server" 
                                                FilterType="Numbers" TargetControlID="Txt_Cantidad_Solicitud" Enabled="True"></cc1:FilteredTextBoxExtender>
                            </td>
                           
                        </tr>
                        <tr>
                           <td style="width: 15%" align="left">
                                <asp:Label ID="Lbl_Tiempo" runat="server" Text="Tiempo Estimado" ToolTip="Duración en días" ></asp:Label>
                            </td>
                            <td style="width: 35%" >
                                <asp:TextBox ID="Txt_Tiempo_Estimado" runat="server" Enabled="False" Width="73%" ToolTip="Duración en días" Style="text-align:right" ></asp:TextBox>&nbsp;Días
                            </td>
                            <td style="width: 15%" >
                                Tr&aacute;mite
                            </td>
                            <td style="width: 35%" >
                                <asp:DropDownList ID="Cmb_Tramite" runat="server" Width="87%" Enabled="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table> 
                        
                    <%-- *********************** inicio para los campos ocultos *********************** --%>
                        <table width="100%" style="display:none">
                            <tr>
                                <td style="width: 15%">
                                    Avance
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="Txt_Avance" runat="server" Enabled="False" Width="60%"></asp:TextBox>
                                    &nbsp;(%)
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Nombre
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Nombre" runat="server" Width="95%" Enabled="False"></asp:TextBox>
                                </td>
                                <td style="width: 15%" align="left">
                                    Apellido Paterno
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="95%" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Apellido Materno
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="95%" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- *********************** Fin para los campos ocultos *********************** --%>
                    
                   
                    <asp:Panel ID="Pnl_Datos_Solicitante" runat="server" GroupingText="Datos del Solicitante"   style="">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%" align="left">
                                    Nombre
                                </td>
                                <td style="width: 85%" align="left">
                                    <asp:TextBox ID="Txt_Nombre_Completo" runat="server" Width="94%" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" align="left">
                                    E-mail
                                </td>
                                <td style="width: 85%" align="left">
                                    <asp:TextBox ID="Txt_Email" runat="server" Width="94%" Enabled="False"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Email" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                        TargetControlID="Txt_Email" ValidChars=".@_">
                                    </cc1:FilteredTextBoxExtender>
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
                                <td style="width: 80%" colspan="3">
                                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="100%" Enabled="true" AutoPostBack="true"
                                        MaxLength="20" OnTextChanged="Txt_Cuenta_Predial_OnTextChanged" ToolTip="Ingrese la cuenta predial"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                        FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                                   
                                </td>
                                <td style="width: 5%" align="center"> 
                                    <asp:UpdatePanel ID="Upnl_Cuenta_Predial" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="Btn_Buscar_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Cuenta_Predial_Click" Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Propietario
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Propietario_Cuenta_Predial" runat="server" Width="84%" MaxLength="100">
                                    </asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    Colonia
                                </td>
                                <td style="width: 35%" colspan="2">
                                    <asp:TextBox ID="Txt_Direccion_Predio" runat="server" Width="84%" MaxLength="200">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Calle
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Calle_Predio" runat="server" Width="84%" MaxLength="100">
                                    </asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    Numero
                                </td>
                                <td style="width: 35%" colspan="2">
                                    <asp:TextBox ID="Txt_Numero_Predio" runat="server" Width="84%" MaxLength="20">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Manzana
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="Txt_Manzana_Predio" runat="server" Width="84%" MaxLength="100">
                                    </asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    Lote
                                </td>
                                <td style="width: 35%" colspan="2">
                                    <asp:TextBox ID="Txt_Lote_Predio" runat="server" Width="84%" MaxLength="100">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Otros
                                </td>
                                <td colspan="3" style="width: 85%">
                                    <asp:TextBox ID="Txt_Otros_Predio" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                   <%-- <cc1:TextBoxWatermarkExtender ID="TBE_Otros_Predio" runat="server" TargetControlID="Txt_Otros_Predio"
                                         WatermarkCssClass="watermarked" WatermarkText="<>" Enabled="True"/>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    Perito
                                </td>
                                <td style="width: 80%" colspan="3">
                                    <asp:DropDownList ID="Cmb_Perito" runat="server" Width="100%">
                                    </asp:DropDownList>
                                   
                                </td>
                                <td style="width: 5%" align="center">  
                                    <asp:ImageButton ID="Btn_Buscar_Perito" runat="server" ToolTip="Seleccionar un Perito"
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                        OnClick="Btn_Buscar_Perito_Click" />
                                 </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    
                                </td>
                                <td style="width: 35%">
                                    <asp:LinkButton ID="Btn_Link_Catastro" runat="server" ForeColor="Blue" Visible="false"
                                        OnClientClick="return confirm('¿Esta seguro que desea realizar el registro de la solicitud?');"
                                        OnClick="Btn_Link_Catastro_Click"></asp:LinkButton>  
                                </td>
                                <td style="width: 15%">  
                                </td>
                                <td style="width: 35%">
                                    <asp:LinkButton ID="Btn_Link_Catastro2" runat="server" ForeColor="Blue" Visible="false"
                                        OnClientClick="return confirm('¿Esta seguro que desea realizar el registro de la solicitud?');"
                                        OnClick="Btn_Link_Catastro2_Click"></asp:LinkButton>  
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                   
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Lbl_Datos_Requeridos" runat="server" Font-Bold="True" Font-Size="Small"
                                    Text="Datos Requeridos" Visible="False" ForeColor="#006600"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="Div_Grid_Datos_Tramite" runat="server" style="overflow: auto;
                                    width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: none">
                                    <asp:GridView ID="Grid_Datos" runat="server" 
                                        AllowPaging="false" AutoGenerateColumns="False"
                                        OnRowDataBound="Grid_Datos_RowDataBound"
                                        CssClass="GridView_1" GridLines="None" Width="97%">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <%-- 0 --%> 
                                            <asp:BoundField DataField="Nombre" HeaderText="Datos" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                <ItemStyle HorizontalAlign="Left" Width="35%" Wrap="true" />
                                            </asp:BoundField>
                                            <%-- 1 --%> 
                                            <asp:TemplateField HeaderText="Descripción">
                                                <HeaderStyle HorizontalAlign="Left" Width="65%" />
                                                <ItemStyle HorizontalAlign="Left" Width="65%" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <%-- 2 --%>                                           
                                             <asp:BoundField DataField="DESCRIPCION" HeaderText="" Visible="false">
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
                            </td>
                        </tr>
                    
                    </table>
                     <table width="100%">
                         <tr>
                            <td align="center">
                                <asp:Label ID="Lbl_Documentos_Requeridos" runat="server" Font-Bold="True" Font-Size="Small"
                                    Text="Documentos Requeridos" Visible="False" ForeColor="#006600"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Lbl_Documento_Opcional" runat="server" Font-Bold="True" Font-Size="Small"
                                    Text="Nota: Las filas que estan marcadas con color Azul son opcionales a subir dependiendo de la situación." Visible="False" ForeColor="#006600"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="Div_Grid_Documentos" runat="server" style="overflow: auto; height: 200px;
                                    width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: none">
                                    <asp:GridView ID="Grid_Documentos" runat="server" 
                                        AutoGenerateColumns="False" OnRowDataBound="Grid_Documentos_RowDataBound"
                                        CssClass="GridView_1" GridLines="None" Width="97%">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                <ItemStyle HorizontalAlign="Left" Width="35%" Wrap="true" />
                                            </asp:BoundField>
                                            <%-- 1 --%>
                                            <asp:TemplateField HeaderText="Ruta del archivo" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Size="12px" ItemStyle-Width="65%" HeaderStyle-Font-Size="13px"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="65%">
                                                <ItemTemplate>
                                                    <cc1:AsyncFileUpload ID="FileUp" runat="server" Width="450px" 
                                                        UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen" />
                                                    <asp:TextBox ID="Txt_Url" runat="server" Width="98%"></asp:TextBox>
                                                 
                                                    <asp:ImageButton ID="Btn_Acutalizar_Documento" runat="server" AlternateText="Ver"
                                                        ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" Width="24px" Height="24px"
                                                        OnClick="Btn_Actualizar_Documento_Click" />
                                                    <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" name="Btn_Ver_Documento"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Width="24px" Height="24px"
                                                        OnClick="Btn_Ver_Documento_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 2 --%>
                                             <asp:BoundField DataField="DESCRIPCION" HeaderText="Documento" Visible="false" >
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" Wrap="true" />
                                            </asp:BoundField>
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
                               
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
