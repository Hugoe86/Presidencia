<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Con_Auxiliar_Cuentas.aspx.cs" Inherits="paginas_Contabilidad_Frm_Cat_Con_Auxiliar_Cuentas" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <%--<asp:UpdateProgress ID="Uprg_Reporte" runat="server" 
        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <asp:ScriptManager ID="ScriptManager_Auxiliar_Cuentas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <div id="Div_Auxiliar_Cuentas" >
                <table width="99.5%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Auxiliar de Cuentas Contables</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%"></td> 
                    </tr>
                </table>
                <table width="99.5%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td width="25%">Buscar cuentas por aproximaci&oacute;n.</td>
                        <td width="30%">
                            <asp:TextBox ID="Txt_Buscar_Cuentas" runat="server" Width="98%" AutoPostBack="true"
                                ontextchanged="Txt_Buscar_Cuentas_TextChanged"></asp:TextBox>
                        </td>
                        <td width="1%"></td>
                        <td width="10%">Año</td>
                        <td width="30%">
                            <asp:DropDownList ID="Cmb_Anio" runat="server" Width="50%" AutoPostBack="true"
                                onselectedindexchanged="Cmb_Anio_SelectedIndexChanged">
                                <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                <asp:ListItem>2010</asp:ListItem>
                                <asp:ListItem>2011</asp:ListItem>
                                <asp:ListItem>2012</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                                <asp:ListItem>2018</asp:ListItem>
                                <asp:ListItem>2019</asp:ListItem>
                                <asp:ListItem>2020</asp:ListItem>
                                <asp:ListItem>2021</asp:ListItem>
                                <asp:ListItem>2022</asp:ListItem>
                                <asp:ListItem>2023</asp:ListItem>
                                <asp:ListItem>2024</asp:ListItem>
                                <asp:ListItem>2025</asp:ListItem>
                                <asp:ListItem>2026</asp:ListItem>
                                <asp:ListItem>2027</asp:ListItem>
                                <asp:ListItem>2028</asp:ListItem>
                                <asp:ListItem>2029</asp:ListItem>
                                <asp:ListItem>2030</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </table>
                            <cc1:TabContainer ID="Tbc_Meses_Contables" runat="server" ActiveTabIndex="0"  Width="99.5%" 
                               OnActiveTabChanged="Tbc_Meses_Contables_ActiveTabChanged" AutoPostBack="True" Font-Size="Medium" >
                                <cc1:TabPanel ID="Tbp_Enero" runat="server" HeaderText="ENE">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                             <asp:Panel ID="Pnl_enero" runat="server" GroupingText="Cuentas" Width="99%" BackColor="White">
                                            <asp:GridView ID="Grid_Cuentas_Enero" runat="server" AllowPaging="True"
                                                onpageindexchanging="Grid_Cuentas_Enero_PageIndexChanging" PageSize="15"  
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None">
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" SortExpression="Cuenta">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                        SortExpression="Descripcion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" 
                                                        SortExpression="Saldo_Final">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView> 
                                            </asp:Panel>
                                           </td>
                                          </tr> 
                                       </table>    
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Febrero" runat="server" HeaderText="FEB">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Febrero" runat="server" AllowPaging="True" PageSize="15" 
                                              onpageindexchanging="Grid_Cuentas_Febrero_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Marzo" runat="server" HeaderText="MAR">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Marzo" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Marzo_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>   
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Abril" runat="server" HeaderText="ABR">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Abril" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Abril_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table> 
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Mayo" runat="server" HeaderText="MAY">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Mayo" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Mayo_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>   
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Junio" runat="server" HeaderText="JUN">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Junio" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Junio_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Julio" runat="server" HeaderText="JUL">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                        <asp:GridView ID="Grid_Cuentas_Julio" runat="server" AllowPaging="True" PageSize="15" 
                                        onpageindexchanging="Grid_Cuentas_Julio_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table> 
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Agosto" runat="server" HeaderText="AGO">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                    <asp:GridView ID="Grid_Cuentas_Agosto" runat="server" AllowPaging="True" PageSize="15" 
                                    onpageindexchanging="Grid_Cuentas_Agosto_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>                           
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Septiembre" runat="server" HeaderText="SEPT">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Septiembre" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Septiembre_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table> 
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Octubre" runat="server" HeaderText="OCT">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Octubre" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Octubre_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            </td>
                                            </tr>
                                            </table>                            
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Noviembre" runat="server" HeaderText="NOV">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                       <asp:GridView ID="Grid_Cuentas_Noviembre" runat="server" AllowPaging="True" PageSize="15" 
                                       onpageindexchanging="Grid_Cuentas_Noviembre_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView> 
                                            </td>
                                            </tr>
                                            </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Diciembre" runat="server" HeaderText="DIC">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Diciembre" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Diciembre_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                    SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" 
                                                    SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView> 
                                            </td>
                                            </tr>
                                            </table>                           
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tbp_Trece" runat="server" HeaderText="MES 13">
                                    <ContentTemplate>
                                    <table width="100%">
                                        <tr align="center">
                                            <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Trece" runat="server" AllowPaging="True" PageSize="15" 
                                            onpageindexchanging="Grid_Cuentas_Trece_PageIndexChanging"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" Visible="True" SortExpression="Cuenta">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Saldo_Final" HeaderText="Saldo Final" Visible="True" SortExpression="Saldo_Final">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView> 
                                            </td>
                                            </tr>
                                            </table>    
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

