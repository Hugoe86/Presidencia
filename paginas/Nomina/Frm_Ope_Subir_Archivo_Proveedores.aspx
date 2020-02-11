<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Subir_Archivo_Proveedores.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Subir_Archivo_Proveedores" Title="Subir Archivo de Proveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScptM_Subir_Archivo_Proveedores" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpPnl_Subir_Archivo_Proveedores" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Subir_Archivo_Proveedores" runat="server" AssociatedUpdatePanelID="UpPnl_Subir_Archivo_Proveedores" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Width="24px" Height="24px"/>
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
                </table>   
                <br />   
                <table style="width:100%;">
                    <tr>
                        <td style="width:100%;" align="center">
                            <div id="Contenedor_Titulo" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                <table width="100%">
                                    <tr>
                                        <td></td>
                                    </tr>            
                                    <tr>
                                        <td width="100%">
                                            <font style="color: Black; font-weight: bold;">Subir Archivo de Proveedores</font>
                                        </td>    
                                    </tr>  
                                    <tr>
                                        <td></td>
                                    </tr>                                      
                                </table>    
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                    <asp:Panel ID="Pnl_Selección" runat="server" GroupingText="Selección de Archivo" Width="98%">   
                        <table style="width:100%;">
                            <tr>
                                <td width="20%" style="text-align:left;">
                                   <asp:Label ID="Lbl_Proveedores" runat="server" Text="* Proveedor"></asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="99%">
                                        <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                    </asp:DropDownList>
                                </td>                                                                                               
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="20%" style="text-align:left;">
                                   <asp:Label ID="Lbl_Nomina" runat="server" Text="* Nomina"></asp:Label>
                                </td>
                                <td width="30%" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged">
                                        <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                    </asp:DropDownList>
                                </td>             
                                <td width="20%" style="text-align:left;">
                                    <asp:Label ID="Lbl_Periodo" runat="server" Text="* Periodo"></asp:Label>
                                </td>
                                <td width="30%" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="95%" Enabled="false">
                                        <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                    </asp:DropDownList>
                                </td>                                                                                        
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <asp:Label ID="Lbl_Seleccionar_Archivo" runat="server" Text="* Archivo"></asp:Label>
                                    <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                          <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                          <div  class="processMessage" id="div_progress">
                                                <img alt="" src="../../imagenes/Updating.gif" />
                                          </div>
                                    </asp:Label>        
                                </td>
                                <td width="30%" style="text-align:left;"> 
                                    <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server" Width="98%" CompleteBackColor="LightBlue" ErrorBackColor="Red" 
                                        ThrobberID="Throbber" UploadingBackColor="LightGray" Enabled="true" />
                                </td>  
                                <td width="20%" style="text-align:left;">
                                    <asp:Label ID="Lbl_No_Periodos" runat="server" Text="* No. Periodos"></asp:Label>
                                </td>
                                <td width="30%" style="text-align:left;">
                                    <asp:TextBox ID="Txt_No_Periodos" runat="server" Width="96%" MaxLength="1"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Periodos" runat="server" FilterType="Numbers" TargetControlID="Txt_No_Periodos">
                                    </cc1:FilteredTextBoxExtender>
                                </td>    
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" width="50%">&nbsp;</td>
                                <td colspan="2" style="text-align:center;" >
                                    <asp:Button ID="Btn_Cargar_Archivo" runat="server" Text="Cargar Datos" 
                                        Width="45%" CssClass="button_autorizar" 
                                        style="background-color:#A9D0F5; color:Black; border-style:outset;" onclick="Btn_Cargar_Archivo_Click" 
                                         />
                                    <asp:Button ID="Btn_Limpiar_Datos" runat="server" Text="Limpiar" 
                                        CssClass="button_autorizar" Width="45%" 
                                        style="background-color:#A9D0F5; color:Black; border-style:outset;" 
                                        onclick="Btn_Limpiar_Datos_Click" />
                                </td>
                            </tr>
                        </table>
                     </asp:Panel>      
            <br />
            <br />
            <br />
        </ContentTemplate>    
    </asp:UpdatePanel>

</asp:Content>

