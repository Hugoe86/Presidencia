<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pat_Com_Aseguradoras.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pat_Com_Aseguradoras" Title="Catalogo de Aseguradoras" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Aseguradoras" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Aseguradoras</td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" AlternateText="Consultar"
                                onclick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkText="<- RFC ó Nombre ->" TargetControlID="Txt_Busqueda" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" 
                                TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>      
                        </td>                        
                    </tr>
                </table>   
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab" style="visibility:visible;">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Datos_Generales"  ID="Tab_Datos_Generales"  Width="100%"  >
                        <HeaderTemplate>Generales</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Aseguradora_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Aseguradora_ID" runat="server" Text="Aseguradora ID"></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Aseguradora_ID" runat="server" Width="50%" MaxLength="10" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" ></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" ></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Nombre_Fiscal" runat="server" Text="* Nombre Fiscal" ></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Nombre_Fiscal" runat="server" Width="98%" MaxLength="100"></asp:TextBox>             
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Nombre_Comercial" runat="server" Text="Nombre Comercial" ></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Nombre_Comercial" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_RFC" runat="server" Text="* RFC" ></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_RFC" runat="server" Width="98%" Rows="20"></asp:TextBox>         
                                        </td>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Cuenta_Contable" runat="server" Text="Cuenta Contable" ></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Cuenta_Contable" runat="server" Width="98%" MaxLength="25"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cuenta_Contable" runat="server" TargetControlID="Txt_Cuenta_Contable" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <br />                                
                                <asp:GridView ID="Grid_Aseguradoras_Generales" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="10" Width="99%"
                                    onselectedindexchanged="Grid_Aseguradoras_Generales_SelectedIndexChanged" 
                                    GridLines= "None"
                                    onpageindexchanging="Grid_Aseguradoras_Generales_PageIndexChanging">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="ASEGURADORA_ID" HeaderText="Aseguradora ID" SortExpression="ASEGURADORA_ID" >
                                            <ItemStyle Width="100px" Font-Size="X-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" >
                                            <ItemStyle Width="125px" Font-Size="X-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" >
                                            <ItemStyle Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                            <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Contactos"  ID="Tab_Contactos"  Width="100%"  >
                        <HeaderTemplate>Contactos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Aseguradora_Contacto_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Aseguradora_Contacto_ID" 
                                                runat="server" Text="Contacto ID"></asp:Label></td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_Aseguradora_Contacto_ID" runat="server" Width="98%" MaxLength="10" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Dato_Contacto" runat="server" Text="* Dato Contacto" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Dato_Contacto" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus_Contacto" runat="server" 
                                                Text="* Estatus" ></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus_Contacto" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Descripcion_Contacto" runat="server" Text="* Descripción" ></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Descripcion_Contacto" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Telefono_Contacto" runat="server" Text="* Teléfono" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Telefono_Contacto" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Fax_Contacto" runat="server" Text="Fax" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fax_Contacto" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>                            
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Celular_Contacto" runat="server" Text="Celular" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Celular_Contacto" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Correo_Electronico_Contacto" runat="server" Text="Correo Electronico" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Correo_Electronico_Contacto" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Contacto" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Contacto_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Contacto" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Contacto_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Contacto" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Contacto_Click" />
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Aseguradora_Contacto" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Aseguradora_Contacto_PageIndexChanging" 
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="ASEGURADORA_CONTACTO_ID" 
                                            HeaderText="ASEGURADORA_CONTACTO_ID" 
                                            SortExpression="ASEGURADORA_CONTACTO_ID" />
                                        <asp:BoundField DataField="DATO_CONTACTO" HeaderText="Dato Contacto" 
                                            SortExpression="DATO_CONTACTO" />
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                            SortExpression="DESCRIPCION" />
                                        <asp:TemplateField HeaderText="Registrado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Contacto_Registrado" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Domicilios"  ID="Tab_Domicilios"  Width="100%">
                        <HeaderTemplate>Domicilios</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Aseguradora_Domicilio_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Aseguradora_Domicilio_ID" runat="server" Text="Domicilio ID"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Aseguradora_Domicilio_ID" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Nombre_Domicilio" runat="server" Text="* Nombre Domicilio" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Nombre_Domicilio" runat="server" Width="98%" 
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus_Domicilio" runat="server" 
                                                Text="* Estatus" ></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus_Domicilio" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Descripcion_Domicilio" runat="server" 
                                                Text="* Descripción" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Descripcion_Domicilio" runat="server" Width="98%" 
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Fax_Domicilio" runat="server" Text="Fax" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fax_Domicilio" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>                              
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Calle_Domicilio" runat="server" 
                                                Text="* Calle" ></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Calle_Domicilio" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Numero_Exterior" runat="server" Text="Número Exterior" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Exterior" runat="server" Width="98%" 
                                                MaxLength="5"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Numero_Interior" runat="server" Text="Número Interior" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Interior" runat="server" Width="98%" MaxLength="5"></asp:TextBox>
                                        </td>
                                    </tr>                            
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Colonia_Domicilio" runat="server" Text="Colonia" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Colonia_Domicilio" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Codigo_Postal_Domicilio" runat="server" Text="Codigo Postal" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Codigo_Postal_Domicilio" runat="server" Width="98%" MaxLength="5"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Codigo_Postal_Domicilio" 
                                                runat="server" TargetControlID="Txt_Codigo_Postal_Domicilio" 
                                                FilterType="Numbers" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                         
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Ciudad_Domicilio" runat="server" Text="Ciudad" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Ciudad_Domicilio" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Municipio_Domicilio" runat="server" Text="Municipio"  ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Municipio_Domicilio" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Estado_Domicilio" runat="server" Text="Estado" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Estado_Domicilio" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Pais_Domicilio" runat="server" Text="País"  ></asp:Label>
                                        </td>   
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Pais_Domicilio" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Domicilio" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Domicilio_Click"/>
                                            <asp:ImageButton ID="Btn_Modificar_Domicilio" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Domicilio_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Domicilio" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Domicilio_Click" />
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Aseguradora_Domicilio" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Aseguradora_Domicilio_PageIndexChanging" 
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="ASEGURADORA_DOMICILIO_ID" 
                                            HeaderText="ASEGURADORA_DOMICILIO_ID" 
                                            SortExpression="ASEGURADORA_DOMICILIO_ID" />
                                        <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio" 
                                            SortExpression="DOMICILIO" />
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                            SortExpression="DESCRIPCION" />
                                        <asp:TemplateField HeaderText="Registrado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Domicilio_Registrado" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>      
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Bancos"  ID="Tab_Bancos"  Width="100%">
                        <HeaderTemplate>Bancos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Aseguradora_Banco_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Aseguradora_Banco_ID" 
                                                runat="server" Text="Aseguradora Banco ID" ></asp:Label></td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_Aseguradora_Banco_ID" runat="server" Width="98%" MaxLength="10" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Producto_Bancario" runat="server" Text="* Producto Bancario" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Producto_Bancario" runat="server" Width="98%" 
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Estatus_Banco" runat="server" Text="* Estatus" ></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus_Banco" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Descripcion_Banco" runat="server" Text="* Descripción Producto" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Descripcion_Banco" runat="server" Width="98%" 
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Cuenta_Banco" runat="server" Text="Cuenta" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Cuenta_Banco" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>                              
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Institucion_Bancaria" runat="server" Text="* Institución Bancaria" ></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Institucion_Bancaria" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Clabe_Institucion_Bancaria" runat="server" Text="CLABE - Institución Bancaria" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Clabe_Institucion_Bancaria" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Clabe_Plaza" runat="server" Text="CLABE - Plaza" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Clabe_Plaza" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>                            
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Clabe_Cuenta" runat="server" Text="CLABE - Cuenta" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Clabe_Cuenta" runat="server" Width="98%" 
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Clabe_Digito_Verificador" runat="server" Text="CLABE - Digito Verificador" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Clabe_Digito_Verificador" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>                         
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Clave_CIE" runat="server" Text="Clave CIE" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Clave_CIE" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Fecha_Vigencia_Banco" runat="server" Text="Fecha Vigencia" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Vigencia_Banco" runat="server" Width="85%" 
                                                MaxLength="50" Enabled="False"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Fecha_Vigencia_Banco" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Fecha_Vigencia_Banco" runat="server" 
                                                TargetControlID="Txt_Fecha_Vigencia_Banco" 
                                                PopupButtonID="Btn_Fecha_Vigencia_Banco" Format="dd/MMM/yyyy" Enabled="True">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Numero_Tarjeta" runat="server" Text="Número Tarjeta"  ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Tarjeta" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Numero_Tarjeta_Reverso" runat="server" Text="Número Tarjeta Reverso"   ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Numero_Tarjeta_Reverso" runat="server" Width="98%" 
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Banco" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Banco_Click"/>
                                            <asp:ImageButton ID="Btn_Modificar_Banco" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Banco_Click"/>
                                            <asp:ImageButton ID="Btn_Quitar_Banco" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Banco_Click"/>
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Aseguradora_Bancos" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%" PageSize="5" 
                                    onpageindexchanging="Grid_Aseguradora_Bancos_PageIndexChanging" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="ASEGURADORA_BANCO_ID" 
                                            HeaderText="ASEGURADORA_BANCO_ID" 
                                            SortExpression="ASEGURADORA_BANCO_ID" />
                                        <asp:BoundField DataField="PRODUCTO_BANCARIO" HeaderText="Producto Bancario" 
                                            SortExpression="PRODUCTO_BANCARIO" />
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                            SortExpression="DESCRIPCION" />
                                        <asp:TemplateField HeaderText="Registrado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Banco_Registrado" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>                               
                </cc1:TabContainer>       
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>