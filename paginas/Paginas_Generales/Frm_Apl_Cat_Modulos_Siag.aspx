<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Cat_Modulos_Siag.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Cat_Modulos_Siag" Title="Catálogo Modulos SIAG"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div id="div_progress" class="processMessage" >
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
            </asp:UpdateProgress>
             <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Cat&aacute;logo de Modulos SIAG</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Error" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" /> &nbsp; 
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" 
                                ForeColor="#990000"></asp:Label>                       
                        </td>
                    </tr> 
                     <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" TabIndex="1" 
                                        onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" 
                                        onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" ToolTip="Eliminar" TabIndex="3" 
                                        
                                        OnClientClick="return confirm('¿Está seguro de eliminar el Modulo SIAG?. ¿Confirma que desea proceder?');" 
                                        onclick="Btn_Eliminar_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" TabIndex="4" onclick="Btn_Salir_Click" />
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
            </table>       
              
            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td width="10%"></td>
                    <td width="40%"></td>
                    <td width="15%"></td>
                    <td width="10%"></td>
                    <td width="25%"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lbl_Modulo_ID" runat="server" Text="Modulo_ID" Width="100%"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Txt_Modulo_ID" runat="server" ReadOnly="true" Width="50%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre" Width="100%"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="100%" MaxLength="100" TabIndex="5"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Nombre" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="< Longitud Máxima de Carácteres [100] >" TargetControlID="Txt_Nombre"/> 
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" 
                            TargetControlID="Txt_Nombre" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"> </cc1:FilteredTextBoxExtender> 
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div id="Div_Grid_Movimientos" runat="server" style="overflow:auto;height:300px;width:99%;vertical-align:top;border-style:solid;border-color: Silver;">
                            <asp:GridView ID="Grid_Modulos_Siag" runat="server" AutoGenerateColumns="False" GridLines="None"
                                CellPadding="4" Width="100%" EmptyDataText="No se encuentra ningun registro" 
                                AllowSorting="True" OnSorting="Grid_Modulos_Siag_Sorting"
                                onselectedindexchanged="Grid_Modulos_Siag_SelectedIndexChanged" 
                                CssClass="GridView_1">
			                      <RowStyle CssClass="GridItem" />
                                  <PagerStyle CssClass="GridHeader" />
                                  <SelectedRowStyle CssClass="GridSelected" />
                                  <HeaderStyle CssClass="GridHeader" />                                
                                  <AlternatingRowStyle CssClass="GridAltItem" /> 
                                                            
                                  <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="../imagenes/gridview/blue_button.png"  
                                            HeaderText="">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="MODULO_ID" HeaderText="Modulo_Id">
                                        <HeaderStyle Width="0%"  HorizontalAlign="Left"/>
                                        <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                        <HeaderStyle Width="95%"  HorizontalAlign="Left"/>
                                        <ItemStyle Width="95%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true"/>
                                    </asp:BoundField>
                                  </Columns>                                                      
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
                        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
                
