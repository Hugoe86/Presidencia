<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_SAP_Parametros.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_SAP_Parametros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    
        <ContentTemplate>

    <%--<asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
     
    <div id="Div_Ope_SAP_Parametros_Botones" style="background-color:#ffffff; width:100%; height:100%;">                
                        <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">                        
                            <tr>
                                <td colspan="4" class="label_titulo">
                                    Catálogo de Parámetros SAP</td>
                            </tr>
                            
                            <tr>
                                <div id="Div_Contenedor_error" runat="server">
                                <td colspan="4">
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                    <br />
                                    <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                                </td>
                                </div>
                            </tr>
                            
                            <tr class="barra_busqueda">
                                <td colspan="2" style="width:50%">
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                                </td>
                                <td colspan="2" align="right" style="width:50%">                                    
                                </td>                        
                            </tr>
                            
                        </table>
                    </div>
     
     <div id="Div_Datos" runat="server">
                 
                <div id="Div_Controles" runat="server" style="background-color:#ffffff; width:100%; height:100%;">        
                       <table id="Datos Generales_Inner" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            
                            <tr>
                                <td style="width:18%">
                                    &nbsp;
                                </td>
                                <td style="width:32%">
                                    &nbsp;
                                </td>
                                <td style="width:18%">
                                    &nbsp;
                                </td>
                                <td style="width:32%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Sociedad" runat="server" Text="Sociedad"></asp:Label></td>
                                <td style="width:32%" colspan="3">
                                    <asp:TextBox ID="Txt_Sociedad" runat="server" Width="95%" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Sociedad" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>                                
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Fondo" runat="server" Text="Fondo"></asp:Label>
                                </td>
                                <td style="width:32%" colspan="3">
                                    <asp:TextBox ID="Txt_Fondo" runat="server" Width="95%"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Fondo" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                    </td>                                
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    
                                    <asp:Label ID="Lbl_Division" runat="server" Text="Division"></asp:Label>                                    
                                </td>
                                <td style="width:32%" colspan="3">
                                    <asp:TextBox ID="Txt_Division" runat="server" Width="95%"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Division" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender></td>                                
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                            </tr>
                            </table>
                            
                    </div>            
            </div>
     
     </ContentTemplate> 
                 
    </asp:UpdatePanel>

</asp:Content>