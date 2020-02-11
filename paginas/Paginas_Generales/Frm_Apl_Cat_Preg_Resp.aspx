<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Apl_Cat_Preg_Resp.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Cat_Preg_Resp" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="SM_Preguntas_Respuestas" runat="server" />
    <asp:UpdatePanel ID="UPnl_Preguntas_Respuestas" runat="server">
        <ContentTemplate>        
        
            <asp:UpdateProgress ID="Uprg_Preguntas_Respuestas" runat="server" AssociatedUpdatePanelID="UPnl_Preguntas_Respuestas" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Preguntas_Respuestas" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Preguntas y Respuestas</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                </table>      
                
                <table width="98%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td colspan="2">                
                             <div align="right" class="barra_busqueda">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">                                                  
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="3" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                     B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Preguntas_Respuestas" runat="server" MaxLength="100"  TabIndex="21"
                                                        ToolTip = "Busquedad de Preguntas_Respuestas" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Preguntas_Respuestas" 
                                                        runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Pregunta>" 
                                                        TargetControlID="Txt_Busqueda_Preguntas_Respuestas" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Preguntas_Respuestas" 
                                                        runat="server" TargetControlID="Txt_Busqueda_Preguntas_Respuestas" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    <asp:ImageButton ID="Btn_Busqueda_Preguntas_Respuestas" runat="server" TabIndex="22"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                        onclick="Btn_Busqueda_Preguntas_Respuestas_Click"
                                                         />                                      
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
                
                <table width="98%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            Para hacer una pregunta acerca del sistema, presione el boton NUEVO, ingrese su pregunta <br />
                            y presione el boton de GUARDAR, se le dara respuesta lo mas pronto posible <br />
                            Gracias!!
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Identificador
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Pregunta_Respuesta_ID" runat="server" Width="78%" TabIndex="0"/>
                        </td> 
                         <td style="text-align:left;width:20%;">
                        </td>
                        <td style="text-align:left;width:50%;">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Pregunta
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Pregunta" runat="server" Width="99.5%" TabIndex="1"
                                TextMode="MultiLine" Height="50px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Pregunta" runat="server" TargetControlID ="Txt_Pregunta" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
                        </td> 
                    </tr> 
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Respuesta
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Respuesta" runat="server" Width="99.5%" TabIndex="2"
                                TextMode="MultiLine" Height="50px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Respuesta" runat="server" TargetControlID ="Txt_Respuesta" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
                        </td> 
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>

                <asp:GridView ID="Grid_Preguntas_Respuesta" runat="server" CssClass="GridView_1" Width="98%"
                     AutoGenerateColumns="False"  GridLines="None" HeaderStyle-CssClass="tblHead" AllowPaging="true" PageSize="10"
                     OnSelectedIndexChanged="Grid_Preguntas_Respuesta_SelectedIndexChanged"
                     OnPageIndexChanging="Grid_Preguntas_Respuesta_PageIndexChanging">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="7%"  HorizontalAlign="Center"/>
                                <HeaderStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:ButtonField>
                            <asp:BoundField DataField="PREG_RESP_ID" HeaderText="">
                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="USUARIO_CREO" HeaderText="USUARIO">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>                            
                            <asp:BoundField DataField="PREGUNTA" HeaderText="PREGUNTA">
                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="RESPUESTA" HeaderText="RESPUESTA">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                </asp:GridView>   
                    <br /><br /><br /><br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

