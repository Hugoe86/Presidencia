<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Reloj_Checador.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Reloj_Checador" Title="Catálogo de Reloj Checador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progre ssBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Reloj_Checador" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Catálogo de Reloj Checador</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>            
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td> 
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;">  
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                onclick="Btn_Modificar_Click"/> 
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                CssClass="Img_Button" TabIndex="3"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                
                                                OnClientClick="return confirm('¿Está seguro de eliminar el Reloj seleccionado?');" 
                                                onclick="Btn_Eliminar_Click"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                onclick="Btn_Salir_Click"/>
                                        </td>
                                        <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Reloj_Checador" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Clave" Width="180px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Reloj_Checador" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Clave>" TargetControlID="Txt_Busqueda_Reloj_Checador" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Reloj_Checador" ValidChars="ÑñáéíóúÁÉÍÓÚ. "
                                                            runat="server" TargetControlID="Txt_Busqueda_Reloj_Checador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" />
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar_Reloj_Checador" runat="server" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                            onclick="Btn_Buscar_Reloj_Checador_Click" />
                                                    </td>
                                                </tr>                                                                          
                                            </table>                                    
                                        </td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>                  
                <br />            
                <table width="98%" class="estilo_fuente">                      
                    <tr>
                        <td style="text-align:left;width:20%;">Reloj Checador ID</td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Reloj_Checador_ID" runat="server" ReadOnly="True" Width="150px"/>
                        </td>
                        <td width="20%" align ="right">*Clave</td>
                        <td align="right" width="30%">
                             <asp:TextBox ID="Txt_Clave_Reloj_Checador" runat="server" Width="150px" TabIndex="7"/>
                             <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave_Reloj_Checador" runat="server"
                                TargetControlID="Txt_Clave_Reloj_Checador" FilterType="Custom, Numbers" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">*Ubicacion</td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Ubicacion_Reloj_Checador" runat="server" MaxLength="100" TabIndex="8" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ubicacion_Reloj_Checador" runat="server" ValidChars="ÑñáéíóúÁÉÍÓÚ. "
                                TargetControlID="Txt_Ubicacion_Reloj_Checador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" />
                        </td>
                    </tr>                    
                    <tr>
                        <td style="text-align:left;width:20%;">Comentarios</td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Reloj_Checador" runat="server" TabIndex="9" MaxLength="250"
                                TextMode="MultiLine" Width="99.5%" AutoPostBack="True"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Reloj_Checador" runat="server" TargetControlID ="Txt_Comentarios_Reloj_Checador" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Reloj_Checador" 
                                runat="server" TargetControlID="Txt_Comentarios_Reloj_Checador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>  
                </table>                
                <br />                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                                      
                    <tr align="center">
                        <td>
                            <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" > 
                                <asp:GridView ID="Grid_Reloj_Checador" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" GridLines="None" Width="100%"
                                    AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                    onselectedindexchanged="Grid_Reloj_Checador_SelectedIndexChanged" 
                                    onsorting="Grid_Reloj_Checador_Sorting">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select"
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="7%" HorizontalAlign="Center"/>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="RELOJ_CHECADOR_ID" HeaderText="Reloj ID" 
                                            Visible="True" SortExpression="RELOJ_CHECADOR_ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True" SortExpression="CLAVE">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UBICACION" HeaderText="Ubicacion" Visible="True" SortExpression="UBICACION">
                                            <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                            <ItemStyle HorizontalAlign="Left" Width="75%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

