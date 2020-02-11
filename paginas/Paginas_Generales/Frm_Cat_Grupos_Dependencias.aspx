<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Grupos_Dependencias.aspx.cs"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Paginas_Generales_Frm_Cat_Grupos_Dependencias" %>

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
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <%--Primer UpdatePanel --%>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                 <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                 <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>--%>
            </ProgressTemplate>
           </asp:UpdateProgress>
           <div id="Div_Contenido" style="width: 97%; height: 100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
           
            <tr>
                <td colspan ="4" class="label_titulo">Grupos Dependencias</td>
            </tr>
             <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                    <tr>
                        <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        </td>            
                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                        </td>
                    </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila 3 Renglon de barra de Busqueda--%>
            <tr class="barra_busqueda">
                <td style="width:50%;">
                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                        OnClientClick="return confirm('¿Está seguro de cambiar el estatus a INACTIVO al grupo de dependencia seleccionada?');"/>
                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                
                </td>
                
                <td align="right" colspan="3" style="width:50%;">
                        <div id="Div_Busqueda" runat="server">
                        Busqueda
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese una Clave>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                        onclick="Btn_Buscar_Click" />
                        </div>
                </td> 
                
            </tr>            
            <tr>
                
                <td colspan="4">
                    <%--Div de Grid_Grupo_Dependencias --%>
                    <div id="Div_Contenido_Grupo_Dependencias" runat="server" style="width:100%;font-size:9px;" visible="true">
                    <table width="99%">
                        <tr>
                            <td style="width:20%"></td>
                            <td style="width:30%"></td>
                            <td style="width:20%"></td>
                            <td style="width:30%"></td>
                        </tr>
                        <tr>
                            <td > Clave</td>
                            <td >
                                <asp:TextBox ID="Txt_Clave" runat="server" Width="85%" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                    TargetControlID="Txt_Clave" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Clave&gt;" />
                            </td>   
                            <td align="center">
                                Estatus
                            </td>
                            <td >
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%">
                                    <asp:ListItem>&lt; SELECCIONE ESTATUS &gt;</asp:ListItem>
                                    <asp:ListItem>ACTIVO</asp:ListItem>
                                    <asp:ListItem>INACTIVO</asp:ListItem>
                                </asp:DropDownList>
                            </td>                         
                        </tr>
                       
                        <tr>
                            <td> Dependencia</td>
                            <td colspan="3"> 
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                    TargetControlID="Txt_Nombre" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Nombre&gt;" />
                            </td>                            
                        </tr>
                        <tr>
                            <td> Comentarios</td>
                            <td colspan="3"><asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="10" 
                                    TextMode="MultiLine" Width="100%" MaxLength="250"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                    TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Ingrese algun Comentario&gt;" /> 
                            </td>                            
                        </tr>
                        
                    </table>
                    
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td colspan="4">
                    <%--Div de Grid_Grupo_Dependencias --%>
                    <div id="Div_Grupo_Dependencias" runat="server" style="overflow:auto;height:325px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;"  visible="true">
                        <asp:GridView ID="Grid_Grupos_Dependencias" runat="server"   Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                DataKeyNames="Grupo_Dependencia_ID" AllowSorting="True"
                                EmptyDataText= "No se encontraron datos"
                                onsorting="Grid_Grupos_Dependencias_Sorting" 
                        onselectedindexchanged="Grid_Grupos_Dependencias_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Grupo_Dependencia_ID" HeaderText="Grupo_Dependencia_ID" 
                                        Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True" SortExpression="Clave">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" Visible="True" SortExpression="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
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
           
           