<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Reportes_Productos.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Reportes_Productos" Title="Reporte Productos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" language="javascript">
                function calendarShown(sender, args){
                    sender._popupBehavior._element.style.zIndex = 10000005;
                }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
                <div id="Div_Contenido" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                    <tr>
                       <td class="label_titulo">Reporte de Productos
                       </td>
                     </tr>
                     <tr> <!--Bloque del mensaje de error-->
                        <td>
                           <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                              <table style="width:100%;">
                                <tr>
                                    <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                        <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                        Width="24px" Height="24px" />
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                        <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" />
                                    </td>
                                </tr> 
                            </table>                   
                          </div>
                        </td>
                     </tr>
                     <tr class="barra_busqueda">
                        <td style="width:20%;">
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                             ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                             Width="24px" CssClass="Img_Button" 
                             AlternateText="Imprimir PDF" 
                             ToolTip="Exportar PDF" 
                             OnClick="Btn_Imprimir_Click"/>  
                             <asp:ImageButton ID="Btn_Imprimir_Excel" 
                             runat="server" 
                             ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                             Width="24px" CssClass="Img_Button" 
                             AlternateText="Imprimir Excel"
                              ToolTip="Exportar Excel" 
                              OnClick="Btn_Imprimir_Excel_Click" />  
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                CssClass="Img_Button" 
                                ToolTip="Salir" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                         </td>                            
                     </tr>
                     <tr>
                        
                     </tr>
                     <tr>
                        <td>
                             <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">
                                <tr >
                                    <td width="170px">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Partida_Especifica" runat="server" 
                                            OnCheckedChanged="Ckb_Partida_Especifica_CheckedChanged" Text="Partida Especifica" 
                                            Checked="false" Enabled="true"
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Partidas_Especificas" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Modelo" runat="server" 
                                            OnCheckedChanged="Ckb_Modelo_CheckedChanged" Text="Modelo" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Modelos" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Marca" runat="server" 
                                            OnCheckedChanged="Ckb_Marca_CheckedChanged" Text="Marca" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Marcas" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                  <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Proveedor" runat="server" 
                                            OnCheckedChanged="Ckb_Proveedor_CheckedChanged" Text="Proveedor" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True" onselectedindexchanged="Cmb_Proveedores_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                  <tr>
                                    <td width="170px" > 
                                         &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="Ckb_Productos_Stock" runat="server" 
                                            Enabled="true" Text="Stock/Transitorios" 
                                            AutoPostBack="True" oncheckedchanged="Ckb_Productos_Stock_CheckedChanged"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Stock_Transitorios" runat="server" Enabled="False" Width="400px">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="TRANSITORIOS">&lt;TRANSITORIOS&gt;</asp:ListItem>
                                            <asp:ListItem Value="STOCK">&lt;STOCK&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                     </td>
                                </tr>
                                <tr>
                                    <td width="170px" >
                                        &nbsp;&nbsp;&nbsp; 
                                        <asp:CheckBox ID="Ckb_Descripcion_A_Z"
                                        runat="server" OnCheckedChanged="Ckb_Descripcion_A_Z_CheckedChanged" 
                                        Text="Descripción de la A-Z" Checked="false" 
                                        AutoPostBack="True"  />
                                    </td>
                                    <td > De:
                                        <asp:TextBox ID="Txt_Letra_Inicial" runat="server" 
                                        Enabled="False" Width="80px" MaxLength="1"></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Inicial" runat="server" 
                                            Enabled="True" FilterType="Custom"  InvalidChars="0,1,2,3,4,5,6,7,8,9,&lt;,&gt;,',!," 
                                            TargetControlID="Txt_Letra_Inicial" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ">
                                      </cc1:FilteredTextBoxExtender>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A:
                                        <asp:TextBox ID="Txt_Letra_Final" runat="server"  
                                        Enabled="False" Width="80px" MaxLength="1"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Final" runat="server" 
                                            Enabled="True" FilterType="Custom"  InvalidChars="0,1,2,3,4,5,6,7,8,9,&lt;,&gt;,',!," 
                                            TargetControlID="Txt_Letra_Final" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                 </tr>
                               
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
              </div>
          </ContentTemplate>
       </asp:UpdatePanel>
</asp:Content>

