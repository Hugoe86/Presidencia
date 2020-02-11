<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Perfiles_Empleado.aspx.cs" 
Inherits="paginas_Tramites_Frm_Ope_Tra_Perfiles_Empleado" %>
    
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

     <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">
     //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
               
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
       <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
             <ContentTemplate>
             
               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                       <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
                <div id="Div_General" runat="server"  style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr align="center">
                            <td  colspan="2" class="label_titulo">Asignar perfiles a empleados  </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td colspan="2" >
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                    </table  >
                    <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
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
                                        OnClientClick="return confirm('Desea eliminar los emplados relacionados con el perfil. ¿Desea continuar?');"
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
                    
                      <div id="Div_Buscar_Perfil" runat="server" style="display:none">
                        <asp:Panel ID="Pnl_Grid_Buscar_Perfil" runat="server" GroupingText="Perfiles Encontrados" ForeColor="Blue">
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;">
                                        <center>
                                            <div id="Div1" runat="server" 
                                                style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                                <asp:GridView ID="Grid_Buscar_Perfil" runat="server"  Width="97%" 
                                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                                    GridLines="None"   AllowPaging="false"
                                                    AutoGenerateColumns="False" 
                                                    OnSelectedIndexChanged="Grid_Buscar_Perfil_SelectedIndexChanged"
                                                    OnRowDataBound="Grid_Buscar_Perfil_RowDataBound"
                                                    EmptyDataText="No se encuentra información"   >
                                                     <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select"
                                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                        </asp:ButtonField>                               
                                                        <asp:BoundField DataField="Empleado_Id" HeaderText="Perfil_id"
                                                              HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Nombre_Empleado" HeaderText="Nombre"
                                                              HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus"
                                                              HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Resp."
                                                              HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    
                    <table width="100%">
                        <tr>
                            <td style="width:15%" >
                                Empleado
                            </td>
                            <td style="width:55%" >
                                <asp:DropDownList id="Cmb_Empleado" runat="server" Width="95%" 
                                    AutoPostBack="False" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle" 
                                    ItemInsertLocation="Append"/>
                            </td>
                            <td style="width:15%" >
                                <asp:LinkButton ID="Btn_Buscar_Empleado" runat="server" ForeColor="Blue"
                                onclick="Btn_Buscar_Empleado_Click">Búsqueda Empleado</asp:LinkButton> 
                            </td>
                            <td style="width:15%" align="right">
                               
                            </td>
                        </tr>
                    </table>
                        
                    <div id="Div_Filtro_Empleados" runat="server" style="display:none">
                        <asp:Panel ID="Pnl_Filtro_Empleado" runat="server" GroupingText="Buscar Empleado" ForeColor="Blue">                            
                            <table width="100%">
                                <tr style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                                    
                                    <td style="width:100%" align="right">
                                    
                                        <asp:ImageButton ID="Btn_Buscar_Empleado_Filtro" runat="server"
                                            CssClass="Img_Button" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                            OnClick="Btn_Buscar_Empleado_Filtro_Click" />
                                        <asp:ImageButton ID="Btn_Cerrar_Busqueda_Empleado" runat="server" ToolTip="Cerrar"
                                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"  
                                            onclick="Btn_Cerrar_Busqueda_Empleado_Click"   /> 
                                    </td>
                                </tr>
                         </table>
                        
                            <table width="100%">
                                <tr>
                                    <td style="width:15%" >
                                        Empleado
                                    </td>
                                    <td style="width:30%" >
                                       <asp:TextBox id="Txt_Filtro_Nombre_Empleado" runat="server" Width="95%" />
                                    </td>
                                    <td style="width:15%" align="right">
                                        No Empleado
                                    </td>
                                    <td style="width:30%" >
                                       <asp:TextBox id="Txt_Filtro_Numero_Empleado" runat="server" Width="95%" />
                                    </td>
                                    <td  style="width:10%"  align="right" rowspan="2">
                                         <%--<asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                    OnClick="Btn_Buscar_Empleado_Click" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%" >
                                        U. Responsable
                                    </td>
                                    <td style="width:30%" colspan="4" >
                                      <cc1:ComboBox id="Cmb_Filtro_Unidad_Responsable" runat="server" Width="550px" 
                                        AutoPostBack="False" 
                                        DropDownStyle="DropDownList" 
                                        AutoCompleteMode="SuggestAppend" 
                                        CaseSensitive="False" 
                                        CssClass="WindowsStyle" 
                                        ItemInsertLocation="Append"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    
                    <table width="100%">
                        <tr>
                            <td style="width:15%" >
                                Perfil
                            </td>
                            <td style="width:55%" >
                                <asp:DropDownList id="Cmb_Perfil" runat="server" Width="95%" 
                                    AutoPostBack="False" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle" 
                                    ItemInsertLocation="Append"/>
                            </td>
                             <td style="width:15%" >  
                                <asp:ImageButton ID="Btn_Agregar_Perfil" runat="server" 
                                    ImageUrl="~/paginas/imagenes/gridview/add_grid.png" Width="20px" Height="20px"
                                    OnClick="Btn_Agregar_Perfil_Click" />
                            </td>
                            <td style="width:15%" >
                            </td>
                        </tr>
                    </table>
                   
                    <table width="100%">
                        <tr>
                            <td rowspan="5" > </td>
                        </tr>
                    </table>
                   
                  
                   
                    <table class="estilo_fuente" width="100%">      
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Tramites_Proceso" runat="server" 
                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                        <asp:GridView ID="Grid_Perfiles_Empleado" runat="server"  Width="97%" 
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                            GridLines="None"   AllowPaging="false"
                                            AutoGenerateColumns="False" 
                                            EmptyDataText="No se encuentra ningun trámite"   >
                                             <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select"
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                </asp:ButtonField>                               
                                                <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado_ID"
                                                      HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="12px">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <%-- 2 --%>
                                                <asp:BoundField DataField="PERFIL_ID" HeaderText="Perfil_ID"
                                                      HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="12px">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                 <%-- 3 --%>
                                                <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre del empleado"
                                                      HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="12px">
                                                    <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="55%" />
                                                </asp:BoundField>
                                                 <%-- 4 --%>
                                                <asp:BoundField DataField="NOMBRE_PERFIL" HeaderText="Perfil"
                                                      HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="12px">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                </asp:BoundField>
                                                <%-- 5 --%> 
                                                <asp:TemplateField  HeaderText= "Eliminar"  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                    <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Img_Quitar" OnClick="Btn_Img_Quitar_OnClick"
                                                                ButtonType="Image" runat="server"  Width="16px" Height="16px"
                                                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"/> 
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="center" Width="10%" />
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                      
                </div>
             </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>