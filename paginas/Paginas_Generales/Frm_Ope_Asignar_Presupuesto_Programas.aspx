<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Asignar_Presupuesto_Programas.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_Asignar_Presupuesto_Programas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Giro_Proveedor" runat="server" />
     <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
  <%--        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>   
--%>
        <div id="Div_Requisitos" style="background-color:#ffffff; width:99%; height:100%;">      
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo">
                        Asignar Presupuesto a Programa
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
           </table>          
           
            <table width="100%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td>                
                             <div class="barra_busqueda">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">  
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                         OnClick = "Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                         OnClick = "Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        OnClick="Btn_Salir_Click" />
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="200px"></asp:TextBox>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                                          WatermarkText="<Ingrese nombre del programa>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Img_buscar" runat="server" ToolTip="Consultar"
                                                          ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                         OnClick = "Btn_Img_buscar_Click"/>                                       
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
          
            <table width="100%">
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>            
                 <tr>
                    <td style="width:13%;text-align:left;">
                        Año de presupuesto
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Anio" runat="server" Width="97%" MaxLength="4"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTB_Anio" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Anio" 
                             ValidChars=".,"/> 
                    </td>
                    <td style="width:13%; text-align:left;">
                        &nbsp;&nbsp;&nbsp;Fecha
                    </td>
                     <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Fecha" runat="server" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td style="width:13%;text-align:left;">
                        Programa
                    </td>
                    <td style="width:53%;text-align:left;" colspan="3">
                        <asp:DropDownList ID="Cmb_Programa" runat="server" Enabled="False" Width="100%" /> 
                    </td>
                </tr>
                  <tr>
                    <td style="width:13%;text-align:left;">
                        Presupuesto
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Presupuesto" runat="server" Width="100%" AutoPostBack ="true" 
                            ontextchanged="Txt_Presupuesto_TextChanged"></asp:TextBox>
                             <cc1:FilteredTextBoxExtender ID="FTB_Presupuesto" 
                             runat="server" 
                             FilterType="Custom,Numbers"
                             TargetControlID="Txt_Presupuesto" 
                             ValidChars=".,"/>  
                    </td>
                    <td style="width:13%; text-align:left;">
                        &nbsp;&nbsp;&nbsp;Ejercido
                    </td>
                     <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Ejercido" runat="server" Width="97%"></asp:TextBox> 
                    </td>
                </tr>
                    <tr>
                    <td style="width:13%;text-align:left;">
                        Disponible
                    </td>
                    <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Disponible" runat="server" Width="100%"></asp:TextBox> 
                    </td>
                    <td style="width:13%; text-align:left;">
                        &nbsp;&nbsp;&nbsp;Comprometido
                    </td>
                     <td style="width:20%;text-align:left;">
                        <asp:TextBox ID="Txt_Comprometido" runat="server" Width="97%"></asp:TextBox> 
                    </td>
                </tr>
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>                
            </table>
            
            <table width="100%">    
                <tr>
                    <td align = "center" style="width:100%;">
                        <asp:GridView ID="Grid_Presupuesto_Programa" runat="server" AutoGenerateColumns="false" Width="100%"
                                AllowPaging="True" onselectedindexchanged="Grid_Presupuesto_Programa_SelectedIndexChanged" style="white-space:normal"
                                CssClass="GridView_1" onpageindexchanging="Grid_Presupuesto_Programa_PageIndexChanging" 
                                PageSize="5" GridLines="None">                          
                            <Columns>
                                 <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                     ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                     <ItemStyle Width="5%" />
                                 </asp:ButtonField>
                                <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderText="Programa" Visible="True">
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Anio_Presupuesto" HeaderText="Año" Visible="True">
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Monto_Presupuestal" HeaderText="Presupuesto" Visible="True">
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Monto_Disponible" HeaderText="Disponible" Visible="True">
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PROYECTO_PROGRAMA_ID" HeaderText=""/>
                                <asp:BoundField DataField="MONTO_EJERCIDO" HeaderText=""/>
                                <asp:BoundField DataField="FECHA_CREO" HeaderText=""/>
                                <asp:BoundField DataField="MONTO_COMPROMETIDO" HeaderText=""/>
                                <asp:BoundField DataField="Pres_Prog_Proy_ID" HeaderText=""/>
                            </Columns>
                            <RowStyle CssClass="GridItem" />
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />                                
                            <AlternatingRowStyle CssClass="GridAltItem" />    
                        </asp:GridView>
                   </td>
            </tr>
        </table>
    </div>
 </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

