﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pat_Com_Clasificaciones.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pat_Com_Clasificaciones" Title="Catalogo de Clasificaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Clasificaciones" runat="server" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Catálogo de Tipos de Activos</td>
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
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
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
                        <td align="left" style="width:50%;">
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
                        <td>Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" AlternateText="Consultar"
                                onclick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkText="<- Clave ó Descripción ->" TargetControlID="Txt_Busqueda" 
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
                <center>
                    <table width="100%" class="estilo_fuente">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Clasificacion_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:13%; text-align:left;">
                                <asp:Label ID="Lbl_Clasificacion_ID" runat="server" Text="Tipo ID" ></asp:Label></td>
                            <td style="width:37%; text-align:left;">
                                <asp:TextBox ID="Txt_Clasificacion_ID" runat="server" Width="50%" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>  
                        <tr>
                            <td style="width:13%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Clave" runat="server" Text="* Clave" ></asp:Label>
                            </td>
                            <td style="width:37%; text-align:left; ">
                                 <asp:TextBox ID="Txt_Clave" runat="server" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave" runat="server" 
                                    TargetControlID="Txt_Clave" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>     
                            </td>
                            <td style="width:13%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" ></asp:Label>
                            </td>
                            <td style="width:37%; text-align:left; ">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                    <asp:ListItem Text ="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                    <asp:ListItem Text ="OBSOLETO" Value="OBSOLETO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>                                  
                        <tr>
                            <td style="width:13%; text-align:left; ">
                               <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripción" ></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99.5%" MaxLength="50"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion" runat="server" 
                                    TargetControlID="Txt_Descripcion" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!#$%">
                                </cc1:FilteredTextBoxExtender>   
                            </td>
                        </tr>
                    </table>
                    <br />                                
                    <asp:GridView ID="Grid_Clasificaciones" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="10" Width="99%"
                        GridLines= "None"
                        onselectedindexchanged="Grid_Clasificaciones_SelectedIndexChanged" 
                        onpageindexchanging="Grid_Clasificaciones_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="CLASIFICACION_ID" HeaderText="Tipo ID" SortExpression="CLASIFICACION_ID" >
                                <ItemStyle Width="110px" HorizontalAlign="Center" Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE" >
                                <ItemStyle Width="110px" HorizontalAlign="Center" Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION" >
                                <ItemStyle Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                SortExpression="ESTATUS" >
                                <ItemStyle Width="110px" HorizontalAlign="Center" Font-Size="X-Small" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />                                
                        <AlternatingRowStyle CssClass="GridAltItem" />       
                    </asp:GridView>
                </center>        
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>