<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Foto_Productos.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Foto_Productos" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="ScriptManager_Productos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
           <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Foto_Producto" style="background-color:#ffffff; width:100%; height:100%;">    
              <asp:UpdatePanel ID="Upnl_Catalogo_Productos" runat="server" UpdateMode="Conditional">
                 <ContentTemplate>             
                     <asp:Panel ID="Pnl_Datos_Generales" runat="server"  GroupingText="" Width="97%">
              
              <table  id="Tabla_Generar" width="100%" class="estilo_fuente"> 
              <tr>
                <td colspan="3">
                      <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                           <table style="width:100%;height:28px;">
                                <tr>
                                      <td align="left" style="width:59%;">  
                                            <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar Imagen" CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" onclick="Btn_Guardar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Átras" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~//paginas//imagenes//paginas//icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                 </tr>
                              </table>
                       </div>
                 </td>
              </tr>
              
              
              <tr>
                  
                      <td>
                       <br />
                     </td>
                      <td style="width:10%;vertical-align:top;" align="center">
                                    <asp:Image ID="Img_Foto_Producto" runat="server"  Width="70px" Height="85px"
                                        ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px"/>           
                                    <asp:HiddenField ID="Txt_Ruta_Foto" runat="server" />         
                                    <asp:ImageButton ID="Btn_Subir_Foto" runat="server" ToolTip="Mostrar Foto del Producto"  OnClick="Subir_Foto_Click" 
                                         ImageUrl="../imagenes/paginas/Sias_Actualizar.png" style="cursor:hand;"/>
                         </td>
                        
                      <td>
                              <table class="estilo_fuente" width="98%">
                                    <!-- Campos datos de producto -->
                                    <tr>
                                        <td style="width:30%;text-align:right;">
                                            Buscar Foto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td style="width:70%;text-align:left;">
                                            <cc1:AsyncFileUpload ID="Async_Foto_Producto" runat="server"  Width="100%" 
                                                ToolTip="Buscar Foto" />                       
                                        </td>
                                    </tr>  
                               </table>   
                         </td>
                  
              </tr>
                   <tr>
                        <td colspan="3">
                            <hr ID="Hr2" runat="server" />
                        </td>
                   </tr>
               
              <tr>
                    <td>
                        <br />
                        <br />
                    </td>
              </tr>
              </table>
              
              </asp:Panel>
                          </ContentTemplate> 
                    </asp:UpdatePanel>
              
            </div>
          </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>



