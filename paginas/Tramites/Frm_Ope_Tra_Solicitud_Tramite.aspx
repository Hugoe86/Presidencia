<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Solicitud_Tramite.aspx.cs" Inherits="paginas_Tramites_Frm_Ope_Tra_Solicitud_Tramite" Title="Solicitud de Trámite" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 100%;">       
         <ContentTemplate>
           <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
         <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                            
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
                
            <div style="width: 95%;">  
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
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            OnClick="Btn_Modificar_Click" Visible="False" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            
                                            OnClientClick="return confirm('¿Está seguro de eliminar el asunto seleccionado?');" 
                                            Visible="False" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right">
                                        Búsqueda por:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Solicitud" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Ingrese folio de solicitud" TargetControlID="Txt_Busqueda">
                                        </cc1:TextBoxWatermarkExtender>                                                                                                                                     
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            TabIndex="1" OnClick="Btn_Buscar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
                <table width="100%">
                    <tr>
                        <td style="width:15%">
                            Folio
                        </td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="50%" Enabled="False"></asp:TextBox>
                        </td> 
                        
                        <td style="width:15%" align="right">
                            Estatus
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="90%" Enabled="False">
                            </asp:DropDownList>
                        </td>
                       
                       
                    </tr>

                    
                    <tr>
                        <td style="width:15%">
                            Tr&aacute;mite
                        </td>
                        <td style="width:35%" colspan="3">
                            <asp:DropDownList ID="Cmb_Tramite" runat="server" Width="96%" OnSelectedIndexChanged="Cmb_Tramite_SelectedIndexChanged"
                                AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                
                <asp:Panel ID="Pnl_Datos_Solicitud" runat="server" GroupingText="Datos del tramite">
                    <table width="100%">
                        <tr>
                            <td style="width:15%">
                                Avance 
                            </td>
                            <td style="width:18%">
                                <asp:TextBox ID="Txt_Avance" runat="server" Enabled="False" Width="60%"></asp:TextBox>
                                &nbsp;(%)
                            </td>
                            <td style="width:15%" align="right">
                                Costo
                            </td>
                            
                            <td style="width:18%">
                                <asp:TextBox ID="Txt_Costo" runat="server" Enabled="False" Width="45%"></asp:TextBox>
                            </td>
                            
                            <td style="width:15%" align="right">
                                Tiempo Estimado 
                            </td>
                            
                            <td style="width:19%" >
                                <asp:TextBox ID="Txt_Tiempo_Estimado" runat="server" Enabled="False" 
                                    Width="45%"></asp:TextBox>&nbsp;(dias)
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
                
                <asp:Panel ID="Pnl_Datos_Solicitante" runat="server" GroupingText="Datos del solicitante">
                    <table width="100%">
                        
                        <tr>
                            <td style="width:15%">
                                Nombre
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%" Enabled="False" onkeyup='this.value = this.value.toUpperCase();'
                                   ></asp:TextBox><%----%>
                            </td> 
                            <td style="width:15%" align="right">
                                Apellido Paterno
                            </td>
                            <td style="width:35%">
                                 <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="90%" onkeyup='this.value = this.value.toUpperCase();'
                                      Enabled="False"></asp:TextBox><%----%>
                            </td>              
                        </tr>
                     
                        <tr>
                        
                            <td>
                                Apellido Materno
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="90%" onkeyup='this.value = this.value.toUpperCase();'
                                     Enabled="False"></asp:TextBox><%----%>
                            </td>
                            <td align="right">
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
                        </tr>
                    </table>
                </asp:Panel>
            
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
                                ForeColor="#006600"></asp:Label>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center">
                            <div id="Div_Grid_Datos_Tramite" runat="server" style="overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:none">
                                <asp:GridView ID="Grid_Datos" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    Width="97%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Nombre" HeaderText="Datos" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                            <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Descripción">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="<%# 400 %>"></asp:TextBox>
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
                                ForeColor="#006600"></asp:Label>
                         
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center">
                            <div id="Div_Grid_Documentos" runat="server" style="overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:none">
                                <asp:GridView ID="Grid_Documentos" runat="server"  AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="97%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" 
                                            ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="25%" Font-Size="13px"/>
                                            <ItemStyle HorizontalAlign="Left" Width="25%" Wrap = "true" Font-Size="12px"/>
                                        </asp:BoundField>
                                       <asp:TemplateField HeaderText="URL"  
                                            HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="65%"
                                            ItemStyle-Font-Size="12px" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="65%"> 
                                                <ItemTemplate>
                                                    <cc1:AsyncFileUpload ID="FileUp" runat="server" ErrorBackColor="Red"
                                                         CompleteBackColor="LightSteelBlue" UploadingBackColor="LightBlue"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                            
                            <div id="Div_Grid_Documentos_Modificar" runat="server" style="overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:none">
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
    </div>
</asp:Content>

