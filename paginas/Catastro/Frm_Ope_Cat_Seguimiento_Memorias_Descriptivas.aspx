<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Seguimiento_Memorias_Descriptivas.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Ope_Cat_Seguimiento_Memorias_Descriptivas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <style type="text/css">
        body
        {
            font: normal 12px auto "Trebuchet MS" , Verdana;
            background-color: #ffffff;
            color: #4f6b72;
        }
        .link
        {
            color: Black;
        }
        .Label
        {
            width: 163px;
        }
        .TextBox
        {
            text-align: right;
        }
        a.enlace_fotografia:link, a.enlace_fotografia:visited
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: normal;
            padding:0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding:0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
        .style2
        {
            width: 39%;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script type="text/javascript" language="javascript">
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
        //        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tool_ScriptManager" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" >
                             Seguimiento de Memorias Descriptivas
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Aceptar" runat="server" ToolTip="Aceptar Trámite" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/accept.png" OnClick="Btn_Aceptar_Click" 
                            AlternateText="Aceptar"/> 
                             <asp:ImageButton ID="Btn_Rechazar" runat="server" ToolTip="Rechazar Trámite" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/delete.png" OnClick="Btn_Cancelar_Click" 
                            AlternateText="Rechazar" Height="24px"/>     
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir" TabIndex="2"/>
                                        </td>
                                        <td align="right" style="width: 41%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Div_Grid_Datos_Peritos" runat="server" visible="true">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_No_Mem_Descript" runat="server" />
                            <asp:HiddenField ID="Hdf_Anio" runat="server" />
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Perito_Externo_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Cantidad_Cobro1" runat="server" />
                            <asp:HiddenField ID="Hdf_Cantidad_Cobro2" runat="server" />
                        </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos del Perito Externo
                                </td>
                        </tr>
                        <tr>
                    <td style="width:20%; text-align: left;">
                          Perito Externo</td>
                        <td style="width:30%; text-align: left;">

                            
      
                            <asp:TextBox ID="Txt_Perito_Externo" runat="server" Enabled="false" 
                                MaxLength="50" TabIndex="3" Width="98%" Style="text-transform:uppercase"></asp:TextBox>
                    </td>
                    </tr>
                        <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos del Tramite
                                </td>
                        </tr>
                        <tr>
                    <td style="width:20%; text-align: left;">
                          *Fraccionamiento</td>
                        <td style="width:30%; text-align: left;">

                            
      
                            <asp:TextBox ID="Txt_Fraccionamiento" runat="server" Enabled="false" 
                                MaxLength="50" TabIndex="3" Width="98%" Style="text-transform:uppercase"></asp:TextBox>
                    </td>
                    <td style="width:20%">
                    *Estatus
                    </td>
                    <td style="width:30%">
                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%">
                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                    <asp:ListItem Text="BAJA" Value="BAJA" />
                    <asp:ListItem Text="AUTORIZADA" Value="AUTORIZADA" />
                    <asp:ListItem Text="RECHAZADA" Value="RECHAZADA" />
                    <asp:ListItem Text="PAGADA" Value="PAGADA" />
                    </asp:DropDownList>
                    </td>
                    </tr>
                      <tr>
                      
                      
                            <td style="width:20%;">
                                *Propietario/Solicitante
                            </td>
                        <td style="width:30%;">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" MaxLength="50" 
                                 style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Tipo</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Tipo" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged" Width="98%">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCCIONE" />
                                <asp:ListItem Text="RC" Value="RC" />
                                <asp:ListItem Text="MR" Value="MR" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuent_Predial" runat="server" Width="98%" TabIndex="3" Style="text-transform: uppercase"
                                    Enabled="false"  MaxLength="12" AutoPostBack="true"  ></asp:TextBox>
                                
                            </td>
                            <td style="text-align: left; " class="style3">
                                *Horientación
                            </td>
                            <td style="text-align: left; " class="style4">
                                <asp:DropDownList ID="Cmb_Horientacion" runat="server" Width="98%" AutoPostBack="true" >
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCCIONE" />
                                    <asp:ListItem Text="HORIZONTAL" Value="HORIZONTAL" />
                                    <asp:ListItem Text="VERTICAL" Value="VERTICAL" />
                                    
                                </asp:DropDownList>
                            </td>
                        
                        
                        </tr>
                      <tr>
                     
                      
                            <td style="text-align:left;width:20%;">
                                *No. Memorias Descriptivas
                            </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_No_Memorias_Descriptivas" runat="server" Width="98%" 
                                TabIndex="3" Enabled="false" MaxLength="4"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftb_Txt_No_Memorias_Descriptivas" runat="server" FilterType="Numbers" TargetControlID="Txt_No_Memorias_Descriptivas" />
                        </td>
                        
                  
                    </tr>
                    <tr>
                     <td style="text-align:left;width:20%;">
                                *Ubicación
                            </td>
                        <td style="text-align:left;width:30%;" colspan="3">
                            <asp:TextBox ID="Txt_Ubicación" runat="server" Width="98%" 
                                TabIndex="3" Enabled="false" MaxLength="100" col></asp:TextBox>
                                
                        </td>
                    
                    </tr>
                    
                     <tr style="background-color: #36C;">
                     <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Observaciones
                                </td>
                     </tr>
                     <tr>
                    <td  style="text-align:left;width:20%;">
                                *Observaciones
                                </td>
                        <td style="text-align:left;width:30%;" colspan="3">
                               
                                <asp:TextBox ID="Txt_Observacion" runat="server" Enabled="false" Height="77px" 
                                    MaxLength="250" TabIndex="3" Width="97%" Style="text-transform:uppercase" ></asp:TextBox>
                        </td>
                    
                    </tr>
                    </table>
                    </div>
                <div id="Div_Detalles" runat="server" visible="true">
                    <table width="98%" class="estilo_fuente">
                            <tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Documentos
                                </td>
                            </tr>
                            <tr ID="Tr_Fila_Fotografias_Bien" runat="server" >
                                <td style="text-align: left;width:20% " class="style1">
                                    Nombre del Documento
                                </td>
                                <td style="text-align: left; width:30%" class="style1" >
                                    <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%" />
                                </td>
                                <td style="text-align: left; width:20%" class="style2">
                                    Documento
                                    </td>
                                    <td style="width:30%" class="style2">
                                    <asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="16"/>
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                            </tr>
                            <tr>
                            <td style="width:20%"></td>
                            <td style="width:30%"></td>
                            <td style="width:20%"></td>
                            <td style="width:30%"><asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                        OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px" TabIndex="17"
                                    /></td>
                            </tr>
                              <tr>
                            <td style="text-align: left; width: 20%" class="style2">
                                Importe del Tramite
                            </td>
                            <td style="width: 30%" class="style2">
                                <asp:TextBox ID="Txt_Calculo_Valores_Memorias" runat="server" Width="98%" TabIndex="3"
                                    Enabled="false" MaxLength="20" AutoPostBack="true" OnTextChanged="Txt_Calculo_Valores_Memorias_TextChanged"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%" class="style2">
                                No. Recibo
                            </td>
                            <td style="width: 30%" class="style2">
                                <asp:TextBox ID="Txt_No_Recibo" runat="server" Width="98%" Enabled="false" ></asp:TextBox>
                            </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: left; width: 20%;">
                                    <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" 
                                        AllowSorting="True" AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" 
                                        OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged" PageSize="20" 
                                        OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO_DOCUMENTO" HeaderText="Año documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="REGIMEN_CONDOMINIO_ID" HeaderText="Perito Id" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DOCUMENTO" HeaderText="Nombre Documento">
                                            <ItemStyle HorizontalAlign="Left" Width="40%"/>
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="tblHead" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="4">
                            <asp:GridView ID="Grid_Memorias_Descriptivas" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Memorias_Descriptivas_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Memorias_Descriptivas_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_MEM_DESCRIPT" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderStyle-Width="15%" HeaderText="Año" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="CANTIDAD_MEM_DESCRIPT" HeaderStyle-Width="30%" HeaderText="Cantidad de Memorias Descriptivas">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="TIPO" HeaderStyle-Width="30%" HeaderText="Tipo de Trámite">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="40%">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="40%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    
                                                    
                                                    
                                                    <asp:BoundField DataField="FRACCIONAMIENTO" HeaderText="Fraccionamiento" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                   <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PERITO_EXTERNO_ID" HeaderText="Perito_Externo_Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UBICACION" HeaderText="Ubicacion" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_HORIENTACION" HeaderText="Horientacion" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>


                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />

                                            </asp:GridView>
                            </td>
                            </tr>
                    </table>
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>