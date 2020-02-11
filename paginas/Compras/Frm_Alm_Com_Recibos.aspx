<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Alm_Com_Recibos.aspx.cs" Inherits="paginas_Compras_Frm_Alm_Com_Recibos" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>            
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Generar Recibo</td>
                    </tr>
                    <tr align="left">
                        <td colspan="2" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" onclick="Btn_Salir_Click" />
                                </td>
                                <td align = "right"><!--
                                    Búsqueda por:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Impuesto_ID" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Ingrese nombre del impuesto>" TargetControlID="Txt_Busqueda" />
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="1" />-->
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
                    <tr>
                        <td>No Requisici&oacute;n</td>
                        <td><asp:TextBox ID="Txt_No_Requisicion" runat="server" Width="150px" Enabled="false"></asp:TextBox></td>
                    </tr>
                        <tr>
                            <td>Dependencia</td>
                            <td><asp:TextBox ID="Txt_Dependencia" runat="server" Width="150px" Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>&Aacute;rea</td>
                            <td><asp:TextBox ID="Txt_Area" runat="server" Width="150px" Enabled="false"></asp:TextBox></td>
                        </tr>
                    <tr>
                        <td>Fecha</td>
                        <td><asp:TextBox ID="Txt_Fecha" runat="server" Enabled="false" Width="150px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Comentarios</td>
                        <td>
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Width="605px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Comentarios_TextBoxWatermarkExtender" runat="server" 
                                TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Límite de Caracteress 250&gt;" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "></cc1:FilteredTextBoxExtender>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="2" align="center">
                            <asp:GridView ID="Grid_Requisiciones_Detalles" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" AllowPaging="True" PageSize="5" 
                                GridLines="None" Width = "98%" 
                                onpageindexchanging="Grid_Requisiciones_Detalles_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MONTO" HeaderText="Costo" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="MONTO_TOTAL" HeaderText="Total" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                                                        
                            </asp:GridView>                            
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>