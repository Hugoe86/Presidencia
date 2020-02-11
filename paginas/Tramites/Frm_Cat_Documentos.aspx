<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Documentos.aspx.cs" Inherits="Frm_Cat_Documentos" Title="Catálogo de Documentos" Culture="en-Us" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Form" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
          <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            
            <div id="Div_Tabla" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Catálogo de Requisitos</td>
                    </tr>
                    <tr align="left">
                        <td style="width:100%" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr> 
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                   
                    <tr class="barra_busqueda" align="right">
                        
                        <td align="left" valign="middle" colspan="2">     
                            <%--<div>--%>
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click"
                                    ToolTip="Nuevo"/>
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" onclick="Btn_Eliminar_Click" 
                                    AlternateText="Eliminar" ToolTip="Eliminar"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server" onclick="Btn_Salir_Click" 
                                    CssClass="Img_Button" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            <%--</div>--%>
                        </td>
                        <td colspan="2">Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" />
                        </td> 
                        
                    </tr>
                </table>
                
                 <div id="Div_Documento_ID" runat="server" style="display:none">
                    <table width="98%">
                         <tr >
                            <td style="width:15%" align="left">
                                Documento ID
                            </td>
                            <td style="width:85%" align="left">
                                <asp:HiddenField ID="Hdf_Id_Documento" runat="server"/>
                                <%--<asp:TextBox ID="Txt_ID" runat="server" ReadOnly="True" Width="150px" Enabled="false"></asp:TextBox>--%>
                            </td>
                                                   
                        </tr>
                    </table>
                </div>
                
                <div id="Div_Datos_Documentos" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:15%" align="left">
                                * Nombre
                            </td>
                            <td style="width:85%" align="left">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="100" 
                                    TabIndex="1" Width="94%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Nombre_Dependencia_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Nombre" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars=". ">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Nombre" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Límite de Caracteres 100>" 
                                    TargetControlID="Txt_Nombre" />               
                            </td>
                        </tr>
                        <tr>                        
                            <td style="width:15%" align="left">
                                * Descripción
                            </td>
                            <td style="width:85%" align="left">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="4" MaxLength="4000"
                                    TextMode="MultiLine" Width="95%" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Comentarios" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Límite de Caracteres 4000>" 
                                    TargetControlID="Txt_Comentarios" />                            
                            </td>
                        </tr>
                        <tr>    
                            <td colspan="2"> 
                                 
                            </td>
                        </tr>  
                    </table>
                </div>
                         
                <div id="Div1" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <center>
                                    <div id="Div_Grid_Datos" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Datos" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1"  EmptyDataText="No se encontraron datos" 
                                            onselectedindexchanged="Grid_Datos_SelectedIndexChanged"
                                            onpageindexchanging="Grid_Datos_PageIndexChanging" 
                                            Width="96%" 
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:ButtonField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Documento ID" >
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <%-- 2 --%>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre"  >
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="45%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="45%" />
                                                </asp:BoundField>
                                                <%-- 3 --%>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="50%" />
                                                    <ItemStyle  Font-Size="12px" HorizontalAlign="Left" Width="50%" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

