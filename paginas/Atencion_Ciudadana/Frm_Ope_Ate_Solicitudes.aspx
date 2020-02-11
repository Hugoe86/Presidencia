<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Ate_Solicitudes.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Atención_Solicitudes"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <%--JavaScript para posicionar el calendarExtender por encima de los demas componentes--%>
       <script type="text/javascript" language="javascript">
        function mostrar_calendario(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
<%--Region de la bandeja de Peticiones--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
          <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressTemplate"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="Div_Atencion_solicitudes" style="background-color:#ffffff; width:97%;">
            <table style="width: 100%;">
                <tr>
                    <td class="label_titulo">
                        Atención a Solicitudes
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="Img_Warning" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                        
                        <asp:Label ID="Lbl_Warning" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                </table>                
            <asp:UpdatePanel ID="Udp_Combos" runat="server">
            <ContentTemplate>            
            
            <div style="border-style: solid; border-width: 1px; border-color: #0F2543; width: 100%; height:auto">
                <table align="left" width="100%">
                <tr class="barra_busqueda" align="left">
                        <td colspan="4">Opciones de busqueda
                        </td>
                </tr>
                <tr>
                    <td style="width:15%" align="left">
                        Folio</td>
                    <td style="width:35%" align="left">
                        <asp:TextBox ID="Txt_Folio" runat="server" ToolTip="Folio" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width:25%" align="left">
                        Dependencia</td>
                    <td align="right" style="width:25%">
                        <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="205px" 
                            AutoPostBack="True" 
                            onselectedindexchanged="Cmb_Dependencia_SelectedIndexChanged" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:15%" align="left">
                        Estatus</td>
                    <td style="width:35%" align="left">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="205px" TabIndex="3">
                            <asp:ListItem Value="0">&lt;&lt;SELECCIONE&gt;&gt;</asp:ListItem>
                            <asp:ListItem Value="PENDIENTE">Pendiente</asp:ListItem>
                            <asp:ListItem Value="PROCESO">Proceso</asp:ListItem>
                            <asp:ListItem Value="POSITIVO">Positivo</asp:ListItem>
                            <asp:ListItem Value="NEGATIVO">Negativo</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                    <td style="width:25%" align="left">
                        Área</td>
                    <td align="right" style="width:25%">
                        <asp:DropDownList ID="Cmb_Area" runat="server" Width="205px" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                    <tr>
                        <td align="left" style="width:15%">
                            Fecha Inicio</td>
                        <td align="left" style="width:35%; z-index:100;">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="200px"></asp:TextBox>
                            <cc1:CalendarExtender ID="Txt_Fecha_inicio_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="Txt_Fecha_inicio" OnClientShown="mostrar_calendario"
                                Format="dd/MMM/yyyy"  >
                            </cc1:CalendarExtender>
                        </td>
                        <td align="left" style="width:25%">
                            Fecha Término</td>
                        <td align="right" style="width:25%" width="200">
                            <asp:TextBox ID="Txt_Fecha_Termino" runat="server" Width="200px"></asp:TextBox>
                            <cc1:CalendarExtender ID="Txt_Fecha_Termino_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="Txt_Fecha_Termino" Format="dd/MMM/yyyy"
                                OnClientShown="mostrar_calendario">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width:15%">
                            &nbsp;</td>
                        <td align="left" style="width:35%">
                            &nbsp;</td>
                        <td align="right" style="width:50%" colspan="2">
                        
                            <asp:ImageButton ID="Btn_Buscar_Peticion" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Buscar" onclick="Btn_Buscar_Peticion_Click2" 
                                ToolTip="Consultar" TabIndex="1"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Inicio" onclick="Btn_Salir_Click" ToolTip="Inicio"/>
                            </td>
                    </tr>
                </table>
                </div>
                </div>
                </center>
                </ContentTemplate>
            </asp:UpdatePanel>   
            <div style="z-index:1;">              
                <table align="left" width="100%">                
                
                <tr>
                    <td colspan="3">
                        <asp:LinkButton ID="Btn_Bandeja" runat="server" onclick="Btn_Bandeja_Click">Bandeja 
                        de entrada</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width:100%;">
                        <asp:GridView  
                            ID="Grid_Peticiones" 
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="GridView_1" 
                            onselectedindexchanged="Grid_Peticiones_SelectedIndexChanged" PageSize="5" 
                            AllowPaging="True" AllowSorting="True" 
                            onpageindexchanging="Grid_Peticiones_PageIndexChanging"
                            EmptyDataText="No se encontraron peticiones">
                            <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="FOLIO" HeaderText="Folio" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ASUNTO" HeaderText="Asunto" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha de Petición" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="NIVEL_IMPORTANCIA" HeaderText="Nivel de importancia" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />                                        
                                    </asp:BoundField>                                    
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr style="height:115px;">
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table> 
            </div>  
          </div>                     
        </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>