<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Perfiles.aspx.cs" Inherits="paginas_tramites_Frm_Cat_Perfiles" Title="Catalogo de Perfiles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">
    
    function Mostrar_Tabla(Renglon, Imagen) {
        object = document.getElementById(Renglon);
        if (object.style.display == "none") {
            object.style.display = "block";
            document.getElementById(Imagen).src = " ../../paginas/imagenes/paginas/stocks_indicator_down.png";
        } else {
            object.style.display = "none";
            document.getElementById(Imagen).src = "../../paginas/imagenes/paginas/add_up.png";
        }
    }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScptM_Perfiles" runat="server" />

    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Cat&aacute;logo de Perfiles</td>
                    </tr>
                    <tr>
                        <td>
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
                                <td style="font-size:10px;width:90%;text-align:left;" valign="top" class="estilo_fuente_mensaje_error">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""/>
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>
        
              <table width="98%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td>                
                             <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">  
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
                                      <td align="right" style="width:41%;">
                                        Busqueda
                                        <asp:TextBox ID="Txt_Busqueda_Perfil" runat="server" Width="50%"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Buscar_Perfil" runat="server" CausesValidation="false"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                            onclick="Btn_Buscar_Perfil_Click" />
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Perfil" runat="server" WatermarkText="<Nombre perfil>" TargetControlID="Txt_Busqueda_Perfil" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>                                   
                                       </td>       
                                     </tr>         
                                  </table>                      
                                </div>
                         </td>
                     </tr>
              </table>
              <br />  
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel1"  Width="100%"  >
                        <HeaderTemplate>Perfiles</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <div id="Div_Datos_ID" runat="server" style="display:none">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="Hdf_Perfil_ID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="width:50%">
                                            <td style="width:18%; text-align:left;">
                                                <asp:Label ID="Lbl_ID_Perfil" runat="server" 
                                                    Text="Perfil ID" CssClass="estilo_fuente"></asp:Label></td>
                                            <td style="width:32%">
                                                <asp:TextBox ID="Txt_ID_Perfil" runat="server" Width="98%" MaxLength="10" 
                                                    Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr> 
                                    </table>
                                </div> 
                                    
                                <div id="Div_Datos_Generales" runat="server" style="display:block">
                                    <table width="100%"  >                                
                                        <tr>
                                            <td style="width:15%; text-align:left; ">
                                                <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" 
                                                    CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width:85%">
                                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%; text-align:left; vertical-align:top;"><asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLengt="150"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                                    WatermarkText="<Límite de Caracteress 150>" 
                                                    TargetControlID="Txt_Descripcion" Enabled="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                
                                
                                <div id="Div1" runat="server" style="display:block">
                                    <table width="100%"  >  
                                        <tr>
                                            <td style="width:100%">
                                                <center>
                                                    <div id="Div_Grid_Datos" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                                        <asp:GridView ID="Grid_Perfiles" runat="server" CssClass="GridView_1"
                                                            AutoGenerateColumns="False"  Width="96%"
                                                            GridLines= "None"
                                                            onpageindexchanging="Grid_Perfiles_PageIndexChanging">
                                                            <RowStyle CssClass="GridItem" />
                                                            <Columns>
                                                                <asp:TemplateField >
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Seleccionar_Solicitudes" runat="server" 
                                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                                        CommandArgument='<%# Eval("PERFIL_ID") %>' OnClick="Btn_Seleccionar_Solicitud_Click" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PERFIL_ID" HeaderText="Perfil ID" SortExpression="PERFIL_ID" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                                        SortExpression="Nombre">
                                                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="95%" />
                                                                        <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="95%" />
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
                                </div>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel2"  ID="TabPanel2"  Width="100%"  >
                        <HeaderTemplate>Perfiles - Actividades</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                            <div style="overflow:auto;height:300px;width:98%;vertical-align:top;border-style:solid;border-color:Silver;display:block" >                           
                                    <asp:GridView ID="Grid_Subprocesos" runat="server" HorizontalAlign="Left"
                                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                        DataKeyNames="TRAMITE_ID"  
                                        OnRowDataBound="Grid_Subprocesos_RowDataBound"
                                        GridLines="None" Width="96%">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image ID="Img_Btn_Expandir" runat="server"
                                                        ImageUrl="~/paginas/imagenes/paginas/add_up.png" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="2%" />
                                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TRAMITE_ID" 
                                                HeaderText="TRAMITE_ID" SortExpression="TRAMITE_ID">                                                
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>                                                
                                            <asp:BoundField DataField="NOMBRE_TRAMITE" HeaderText="Tramite" 
                                                SortExpression="Nombre_Tramite">                                            
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="68%" />
                                                <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="68%" />
                                            </asp:BoundField>    
                                             <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Resp." 
                                                SortExpression="Nombre_Tramite">                                            
                                                <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="30%" />
                                                <ItemStyle Font-Size="10px" HorizontalAlign="Left" Width="30%" />
                                            </asp:BoundField>                                            
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_Actividades" runat="server" 
                                                        Text='<%# Bind("TRAMITE_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Literal ID="Ltr_Inicio" runat="server" 
                                                        Text="&lt;/td&gt;&lt;tr id='Renglon_Grid' style='display:none;position:static'&gt;&lt;td colspan='3';left-padding:30px;&gt;" />
                                                    <asp:GridView ID="Grid_Detalles_Actividades" runat="server" AllowPaging="False" 
                                                        OnRowDataBound="Grid_Detalles_Actividades_RowDataBound"
                                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="95%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Habilitar"
                                                                HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="5%"
                                                                ItemStyle-Font-Size="12px"   ItemStyle-HorizontalAlign="Center"   ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chck_Activar" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SUBPROCESO_ID" 
                                                                SortExpression="SUBPROCESO_ID" />  
                                                            <asp:BoundField DataField="ORDEN_SUBPROCESO" HeaderText="No." 
                                                                SortExpression="ORDEN_SUBPROCESO" 
                                                                HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="5%"
                                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="12px" ItemStyle-Width="5%"/>  
                                                            <asp:BoundField DataField="NOMBRE_SUBPROCESO" HeaderText="Actividad" 
                                                                SortExpression="NOMBRE_SUBPROCESO" 
                                                                HeaderStyle-Font-Size="13px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90%"
                                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="12px" ItemStyle-Width="90%"/>
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                    <asp:Literal ID="Ltr_Fin" runat="server"  Text="&lt;/td&gt;&lt;/tr&gt;" /> 
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />                                
                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                    </asp:GridView>                                
                                </div>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>    
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>