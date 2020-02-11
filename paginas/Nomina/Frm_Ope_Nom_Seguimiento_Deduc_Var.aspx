<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Seguimiento_Deduc_Var.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Seguimiento_Deduc_Var" Title="Seguimiento de Deducciones Variables" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Deducciones_Variables" runat="server"/>

<asp:UpdatePanel ID="UPnl_Autorizacion_Deducciones_Variables" runat="server" UpdateMode="Conditional">
  <ContentTemplate>       

    <asp:HiddenField ID="Hf_No_Deduccion" runat="server" />
    <asp:HiddenField ID="Hf_Deduccion" runat="server" />
    <asp:HiddenField ID="Hf_Estatus" runat="server" />
    
    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td class="label_titulo">Autorizaci&oacute;n Deducciones Variables</td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
            </td>
        </tr>
    </table>  
    
    <center>
        <asp:Panel ID="Pnl_Gral_Autirizar_Deducciones_Variables" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px"
            style="border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
            <asp:Panel ID="Pnl_Header_Autorizar_Deducciones_Variables" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                           <asp:Image ID="Img_Informacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             Informacion: Autorizaci&oacute;n de Deducciones Variables
                        </td>
                        <td align="right" style="width:10%;">
                        </td>
                    </tr>
                </table>            
                
            </asp:Panel>                                                                       
                <div style="cursor:default;width:99%">
                            <table width="100%" style="background-color:#ffffff;">
                               <tr>
                                    <td style="width:100%" colspan="4" align="right">
                                        <asp:ImageButton ID="Btn_Clear_Ctlr" runat="server" OnClick="Btn_Clear_Ctlr_Click"
                                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles"/>                         
                                    </td>
                                </tr>            
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Empleado ID
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Autorizacion_Empleado_ID" runat="server" Width="98%" MaxLength="10" TabIndex="11"/>
                                    </td>     
                                    <td style="width:20%;text-align:left;">
                                        No Empleado
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_autorizacion_No_Empleado" runat="server" Width="98%" MaxLength="10" TabIndex="11"/>
                                    </td>                                                                                           
                                </tr>    
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Nombre
                                    </td>
                                     <td style="width:80%;text-align:left;" colspan="3">
                                        <asp:TextBox ID="Txt_Autorizacion_Nombre_Empleado" runat="server" Width="99.5%" MaxLength="10" TabIndex="11"/>
                                    </td>                                    
                                </tr>                  
                                <tr>
                                    <td style="width:10%;text-align:left;">
                                       Comentarios
                                    </td>
                                    <td style="width:90%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Autorizacion_Comentarios_Estatus" runat="server" Width="99.5%" MaxLength="100" 
                                                TextMode="MultiLine" TabIndex="5"/>
                                            <cc1:FilteredTextBoxExtender ID="FTxt_Autorizacion_Comentarios_Estatus" runat="server" 
                                                TargetControlID="Txt_Autorizacion_Comentarios_Estatus"
                                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                            <cc1:TextBoxWatermarkExtender ID="TTxt_Autorizacion_Comentarios_Estatus" runat="server" 
                                                TargetControlID ="Txt_Autorizacion_Comentarios_Estatus" WatermarkText="Límite de Caractes 250" 
                                                WatermarkCssClass="watermarked"/>                              
                                    </td>                                                           
                                </tr>  
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>                    
                               <tr>
                                    <td style="width:100%" colspan="4" align="center">
                                        <asp:Button ID="Btn_Autorizar" runat="server" Text="Guardar Comentario" 
                                            style="border-style:none;background-color:White;"  TabIndex="16" 
                                            ToolTip="Guardar el Estatus" OnClick="Btn_Autorizar_Click"/>    
                                        <asp:Button ID="Btn_Cancelar" runat="server" Text="Salir" 
                                            style="border-style:none;background-color:White;"  TabIndex="16" 
                                            ToolTip="Salir" OnClick="Btn_Cancelar_Click"/>                                                                           
                                    </td>
                                </tr>                  
                               <tr>
                                    <td style="width:100%" colspan="4" >
                                        <hr />
                                    </td>
                                </tr>  
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Emplados_Autorizar" runat="server" CssClass="GridView_1"
                                             AutoGenerateColumns="False"  GridLines="None" 
                                             OnSelectedIndexChanged="Grid_Emplados_Autorizar_OnSelectedIndexChanged"
                                             OnRowDataBound="Grid_Emplados_Autorizar_RowDataBound"
                                             >
                                                <Columns>           
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select"
                                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                                <ItemStyle Width="5%"  HorizontalAlign="Left"/>
                                                                <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                                                        </asp:ButtonField>                                                                               
                                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Clave">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COMENTARIOS_ESTATUS" HeaderText="Comentarios">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>  
                                                        <asp:TemplateField HeaderText="Autorizar">
                                                            <ItemTemplate>
                                                                <asp:Button ID="Btn_Autorizar_Deduccion_Variable" runat="server" Text="Autorizar"
                                                                    OnClick="Btn_Autorizar_Deduccion_Variable_Click" style="border-style:none;background-color:#FFFFFF"
                                                                    onmouseover="this.style.backgroundColor='#FFFFCC';this.style.cursor='hand';this.style.color='DarkBlue';this.style.borderStyle='none';this.style.borderColor='Silver';"
                                                                    onmouseout="this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';"
                                                                    />
                                                                <asp:Button ID="Btn_Rechazar_Deduccion_Variable" runat="server" Text="Rechazar"
                                                                    OnClick="Btn_Rechazar_Deduccion_Variable_Click" style="border-style:none;background-color:#FFFFFF;"
                                                                    onmouseover="this.style.backgroundColor='#FFFFCC';this.style.cursor='hand';this.style.color='DarkBlue';this.style.borderStyle='none';this.style.borderColor='Silver';"
                                                                    onmouseout="this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';"                                                                    
                                                                    />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="25%" /> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Información">
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel ID="UPnl_Informacion" runat="server" >
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="Lbl_Informacion" runat="server" CssClass="Informacion_Incidencia" Width="98%"/>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>                                                                
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="25%"  Font-Size="10px"/> 
                                                        </asp:TemplateField>                                                                                                                                                                                                                               
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView> 
                                        </div>
                                    </td>
                               </tr>      
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                               </tr>  
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <asp:Label ID="Lbl_Autorizar" runat="server" />
                                    </td>
                                </tr>                                                                                        
                            </table>          
                </div>                 
        </asp:Panel>  
    </center>
       </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>

