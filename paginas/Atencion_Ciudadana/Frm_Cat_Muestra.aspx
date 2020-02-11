<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Cat_Muestra.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Acciones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<%--<cc1:ToolkitScriptManager ID="Tsm_Acciones" runat="server"  AsyncPostBackTimeout="3600" 
    EnableScriptGlobalization="true" EnableScriptLocalization="true"/>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_Principal" style="background-color:#ffffff; width:100%; height:100%;">
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
            </asp:UpdateProgress>
            
                <table width="99.5%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Acciones de Atención Ciudadana
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>                          
                            <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" TabIndex="0" Text="Mensajes de advertencia"></asp:Label>
                        </td>
                    </tr>                    
                </table>            
              <div id="Div_Barra_Herramientas" runat="server">              
                <table width="99.5%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda">
                        <td colspan="2"> 
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                            CssClass="Img_Button" 
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button"
                            AlternateText="Modificar" ToolTip="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            CssClass="Img_Button" 
                            AlternateText="Eliminar" ToolTip="Eliminar" 
                                OnClientClick="return confirm('¿Esta seguro de eliminar el registro?');" 
                                onclick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                            CssClass="Img_Button"  
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                onclick="Btn_Salir_Click" />
                        </td>
                        <td colspan="2" align="right">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                ToolTip="Buscar" TabIndex="5" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., " />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Clave o Nombre>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"/>
                        </td>
                    </tr>
                </table>
                </div>
                <div id="Div_Contenido1" runat="server" style="width:99%; ">
                    <table width="99.5%"  border="0" cellspacing="0" >
                        <tr>
                            <td style="width:14%;">
                            </td>
                            <td style="width:34%;">
                            </td>
                            <td style="width:4%;">
                            </td>
                            <td style="width:14%;">
                            </td>
                            <td style="width:34%;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                               Clave 
                            </td>                            
                            <td>
                                <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="15"></asp:TextBox>
                                <cc1:filteredtextboxextender id="FTE_Clave" runat="server" targetcontrolid="Txt_Clave"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                    </cc1:filteredtextboxextender>                                
                            </td>
                            <td>
                            </td>
                            <td style="text-align:right;">
                                Estatus
                            </td>
                            <td style="text-align:right;">                                
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                </asp:DropDownList>                               
                            </td>
                        </tr>
                        <tr>
                            <td >
                                Nombre
                            </td>                                
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="100%" MaxLength="250"></asp:TextBox>   
                                <cc1:filteredtextboxextender id="FTB_Nombre" runat="server" targetcontrolid="Txt_Nombre"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                    </cc1:filteredtextboxextender>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Descripci&oacute;n
                            </td>                                
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <cc1:filteredtextboxextender id="FTB_Descripción" runat="server" targetcontrolid="Txt_Descripcion"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                    </cc1:filteredtextboxextender>                                                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
<%--                                <asp:GridView ID="GridView1" runat="server">
                                </asp:GridView>
--%>                            </td>
                        </tr>

                    </table>
                </div>
                
                <div style="overflow:auto; height:320px; width:98.8%; vertical-align:top; border-style:outset; border-color: Silver;" > 
                
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

