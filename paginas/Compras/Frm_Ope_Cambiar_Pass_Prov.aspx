<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Proveedores.master" CodeFile="Frm_Ope_Cambiar_Pass_Prov.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Cambiar_Pass_Prov" %>

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
&nbsp;<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="<%= Page.ResolveUrl("~/paginas/imagenes/paginas/Updating.gif") %>" /></div>
                </ProgressTemplate>
        </asp:UpdateProgress>
        
         <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td  colspan="4" class="label_titulo">Cambiar Password Proveedor</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td  colspan="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:99%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="width:10%;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="width:90%;">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" Width="99%"/>
                            </td>
                        </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila de Busqueda y Botones Generales --%>
            <tr class="barra_busqueda">
                    <td style="width:20%;" colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <%--<td align="right" colspan="3" style="width:99%;">
                        <div id="Div_Busqueda" runat="server">
                        Busqueda
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png"/>
                        </div>
                    </td> --%>
            </tr>
             <table border="0" cellspacing="0" class="estilo_fuente" width="99%">
                 <tr>
                     <td colspan="4">
                         <div ID="Div_Detalle_Requisicion" runat="server" 
                             style="width:100%;font-size:9px;" visible="true">
                             <table width="99%">
                                 <tr>
                                     <td align="center" colspan="4">
                                     </td>
                                 </tr>
                                 <tr>
                                     <td style="width:15%">
                                         Numero de Padron:</td>
                                     <td style="width:85%">
                                         <asp:TextBox ID="Txt_Num_Padron" runat="server" Enabled="true" Width="20%"></asp:TextBox>
                                         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                             Enabled="True" FilterType="Custom" InvalidChars="&lt;,&gt;,&amp;,',!," 
                                             TargetControlID="Txt_Num_Padron" ValidChars="0,1,2,3,4,5,6,7,8,9">
                                         </cc1:FilteredTextBoxExtender>
                                         <asp:ImageButton ID="Btn_Buscar_Proveedor" runat="server" 
                                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                             ToolTip="Buscar Proveedor" onclick="Btn_Buscar_Proveedor_Click" />
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Razon Social</td>
                                     <td>
                                         <asp:TextBox ID="Txt_Razon_Social" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Nombre Comercial</td>
                                     <td>
                                         <asp:TextBox ID="Txt_Nombre_Comercial" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Contraseña Actual</td>
                                     <td>
                                         <asp:TextBox ID="Txt_Password_Actual" runat="server" Enabled="true" 
                                             TextMode="Password" Width="20%"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Contraseña nueva</td>
                                     <td>
                                         <asp:TextBox ID="Txt_Password_Nuevo" runat="server" Enabled="true" 
                                             TextMode="Password" Width="20%"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Confirmar contraseña nueva</td>
                                     <td>
                                         <asp:TextBox ID="Txt_Confirmar_Password" runat="server" Enabled="true" 
                                             TextMode="Password" Width="20%"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td colspan="2">
                                         <asp:Button ID="Btn_Guardar" runat="server" Text="Guardar" Width="20%" 
                                             CssClass="button" onclick="Btn_Guardar_Click"/>
                                         &nbsp;&nbsp;&nbsp;
                                         <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" Width="20%" 
                                             CssClass="button" onclick="Btn_Cancelar_Click"/>
                                     </td>
                                 </tr>
                             </table>
                         </div>
                     </td>
                 </tr>
            </table>
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
