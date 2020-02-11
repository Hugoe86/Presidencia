<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
CodeFile="Frm_Cat_Ort_Inspectores.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Inspectores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
           <%--Div de Contenido --%>
           <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width = "98%" border="0" cellspacing="0">                     
                    <tr>
                        <td colspan ="4" class="label_titulo">Catálogo Peritos de proyectos y obras Y/O Corresponsales</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="6">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="true">
                            <table style="width:100%;" class="estilo_fuente">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                    </td>            
                                </tr>
                                <tr>
                                    <td style="width:10%;">              
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <%--<asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />--%>
                                    </td>
                                </tr>          
                            </table>                   
                        </div>
                        </td>
                    </tr>
                     <%--Manejo de la barra de busqueda--%>
                    <tr class="barra_busqueda">
                    <td colspan = "2" align = "left" >
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            onclick="Btn_Nuevo_Click"/>
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            onclick="Btn_Modificar_Click"/>
                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            onclick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?');"/>
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            onclick="Btn_Salir_Click"/>
                    </td>
                    <td colspan="2" align = "right">Busqueda
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                            onclick="Btn_Buscar_Click" />
                    </td> 
                    </tr>
                </table>
                
                <%--Div oculto de id --%>
                <div id="Div_Plantilla_id" runat="server" style="display:none">
                    <table width="98%">
                        <tr>
                            <td >
                                <asp:HiddenField ID="Hdf_Elemento_ID" runat="server" />
                            </td>
                        </tr>
                    </table>
                     <table width="98%">
                        <tr>
                            <td rowspan="5">
                             </td>
                        </tr>
                    </table>
                </div>
                
                 <%--Div de datos de la formatos --%>
                <div id="Div_Datos_Formato" runat="server" style="display: block">
                    <table width="98%">
                        <tr>
                            <td style="width:15%" align="left">
                                 *Nombre             
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="150" Width="95%" ></asp:TextBox>              
                            </td>
                             <td  style="width:15%" align="right">
                                *Tipo de perito              
                            </td>
                             <td  style="width:35%" align="left">
                                <asp:DropDownList ID="Cmb_Tipo_Perito" runat="server" Width="95%" >
                                    <asp:ListItem Selected="True">&lt; Seleccione &gt;</asp:ListItem>
                                    <asp:ListItem Value="PROYECTOS">PROYECTOS</asp:ListItem>
                                    <asp:ListItem Value="OBRAS">OBRAS</asp:ListItem>
                                    <asp:ListItem Value="PROYECTOS Y OBRAS">PROYECTOS Y OBRAS</asp:ListItem>
                                    <asp:ListItem Value="CORRESPONSAL">CORRESPONSAL</asp:ListItem>
                                </asp:DropDownList>              
                            </td>
                        </tr>
                    </table> 
                        
                    <table width="98%">
                        <tr>
                            <td style="width:15%" align="left">
                                 *Cedula profesional              
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Cedula_Profesional" runat="server" MaxLength="50" Width="95%" ></asp:TextBox>              
                            </td>
                        
                            <td style="width:15%" align="right">
                                 *Titulo            
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Titulo" runat="server" MaxLength="100" Width="95%" ></asp:TextBox>              
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%" align="left">
                                 *Afiliado al              
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Afiliado" runat="server" MaxLength="100" Width="95%" ></asp:TextBox>              
                            </td>
                             <td style="width:15%" align="left">       
                            </td>
                            <td  style="width:35%" align="left">            
                            </td>
                        </tr>
                     </table>    
                        
                    <table width="98%">
                        <tr>
                            <td style="width:15%" >  
                                *Colonia Oficina
                            </td>
                             <td style="width:85%" >  
                                  <asp:DropDownList ID="Cmb_Colonias" runat="server"  width="95%" 
                                    AutoPostBack="true" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle"
                                    OnSelectedIndexChanged="Cmb_Colonias_OnSelectedIndexChanged" />  
                                    
                                <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Colonia_Click" />
                                <%--<asp:TextBox ID="Txt_Colonia" runat="server" Enabled="false" Width="95%"></asp:TextBox>--%>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="width:15%" align="left">
                                 *Calle de la oficina             
                            </td>
                            <td  style="width:85%" align="left">
                                 <asp:DropDownList ID="Cmb_Calle" runat="server" width="90%" 
                                    AutoPostBack="false" 
                                    DropDownStyle="DropDown" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle"  /> 
                                 <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                        OnClick="Btn_Buscar_Calles_Click" />           
                            </td> 
                        </tr>
                    </table>  
                      
                    <table width="98%">
                        <tr>       
                            <td style="width:15%" align="left">
                                 *Numero de la oficina            
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Numero_Oficina" runat="server" MaxLength="8" Width="95%" ></asp:TextBox> 
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Oficina" runat="server" 
                                                FilterType="Numbers" TargetControlID="Txt_Numero_Oficina" Enabled="True"></cc1:FilteredTextBoxExtender>                                
                            </td>
                             <td style="width:15%" align="right">
                                 *Telefono de la oficina            
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Telefono_Oficina" runat="server" MaxLength="20" Width="95%" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Telefono_Oficina" runat="server" 
                                                FilterType="Numbers" TargetControlID="Txt_Telefono_Oficina" Enabled="True" ValidChars="0987654321-"></cc1:FilteredTextBoxExtender>                                   
                            </td>
                        </tr>
                         <tr>       
                            <td style="width:15%" align="left">
                                 *Email          
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Email" runat="server" MaxLength="150" Width="95%" 
                                onblur="this.value = (this.value.match(/^[A-Za-z]{1}([-\.]?\w)+@([A-Za-z]{1}[A-Za-z0-9_\-]{1,63})(\.[A-Za-z]{2,4}){1}((\.[A-Za-z]{2}){1})?$/))? this.value: '';"/>                            
                            </td>
                             <td style="width:15%" align="right">
                                *Telefono Celular         
                            </td>
                            <td  style="width:35%" align="left">
                                  
                                <asp:TextBox ID="Txt_Celular" runat="server" Width="95%" ></asp:TextBox>  
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                        FilterType="Numbers" TargetControlID="Txt_Celular" Enabled="True" ValidChars="0987654321"></cc1:FilteredTextBoxExtender>                                   
                            </td>
                        </tr>
                    </table>
                    
                      <table width="98%">
                        <tr>
                            <td style="width:15%" >  
                                *Colonia particular
                            </td>
                             <td style="width:85%" >  
                                  <asp:DropDownList ID="Cmb_Colonias_Particular" runat="server"  width="95%"
                                    AutoPostBack="true" 
                                    DropDownStyle="DropDownList" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle"
                                    OnSelectedIndexChanged="Cmb_Colonias_Particular_SelectedIndexChanged" />  
                                    
                                <asp:ImageButton ID="Btn_Buscar_Colonia_Particular" runat="server" ToolTip="Seleccionar Colonia"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Colonia_Particular_Click" />
                                <%--<asp:TextBox ID="Txt_Colonia" runat="server" Enabled="false" Width="95%"></asp:TextBox>--%>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="width:15%" align="left">
                                 *Calle Particular             
                            </td>
                            <td  style="width:85%" align="left">
                                 <asp:DropDownList ID="Cmb_Calle_Particular" runat="server" width="90%" 
                                    AutoPostBack="false" 
                                    DropDownStyle="DropDown" 
                                    AutoCompleteMode="SuggestAppend" 
                                    CaseSensitive="False" 
                                    CssClass="WindowsStyle"  /> 
                                 <asp:ImageButton ID="Btn_Buscar_Calle_Particular" runat="server" ToolTip="Seleccionar Calle"
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                        OnClick="Btn_Buscar_Calle_Particular_Click" />           
                            </td> 
                        </tr>
                    </table>  
                      
                    <table width="98%">
                        <tr>       
                            <td style="width:15%" align="left">
                                 *Numero             
                            </td>
                            <td  style="width:35%" align="left">
                                 <asp:TextBox ID="Txt_Numero_Casa" runat="server" MaxLength="10" Width="95%" ></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Casa" runat="server" 
                                        FilterType="Numbers" TargetControlID="Txt_Numero_Casa" Enabled="True"></cc1:FilteredTextBoxExtender>                                                        
                            </td>
                             <td style="width:15%" align="right">
                                 *Codigo postal            
                            </td>
                            <td  style="width:35%" align="left">
                                <asp:TextBox ID="Txt_Codigo_Postal" runat="server" MaxLength="8" Width="95%" ></asp:TextBox> 
                                     <cc1:FilteredTextBoxExtender ID="Fte_Txt_Codigo_Postal" runat="server" 
                                        FilterType="Numbers" TargetControlID="Txt_Codigo_Postal" Enabled="True"></cc1:FilteredTextBoxExtender>  
                            </td>
                        </tr>
                         <tr>       
                            <td style="width:15%" align="left">
                                 *Telefono            
                            </td>
                            <td  style="width:35%" align="left">
                                 <asp:TextBox ID="Txt_Telefono_Particular" runat="server" Width="95%" ></asp:TextBox>  
                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                        FilterType="Numbers" TargetControlID="Txt_Telefono_Particular" Enabled="True" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>                              
                            </td>
                             <td style="width:15%" align="left">
                                  
                            </td>
                            <td  style="width:35%" align="left">
                                
                            </td>
                        </tr>
                    </table>
                    
                    <table width="98%">
                        <tr>       
                            <td style="width:15%" align="left">
                                 *Especialidad             
                            </td>
                            <td  style="width:35%" align="left">
                                 <asp:TextBox ID="Txt_Especialidad" runat="server"  Width="95%" MaxLength="100" ></asp:TextBox>                                 
                            </td>
                             <td style="width:15%" align="left">
                                <%-- Numero registro--%>            
                            </td>
                            <td  style="width:35%" align="left">
                                <%--<asp:TextBox ID="Txt_Numero_Registro" runat="server" MaxLength="150" Width="95%" ></asp:TextBox>   --%>
                            </td>
                        </tr>
                         <tr>       
                            <td style="width:15%" align="left">
                                 Comentarios             
                            </td>
                            <td  style="width:35%" align="left" colspan="3">
                                 <asp:TextBox ID="Txt_Comentarios" runat="server"  Width="95%" MaxLength="200" Rows="3" TextMode="MultiLine" ></asp:TextBox>                                 
                            </td>
                            
                        </tr>
                    </table>
                    
                    <table width="98%">
                        <tr>       
                            <td style="width:15%" align="left">
                                *Documentacion entregada             
                            </td>
                            <td  style="width:15%" align="left">
                                <asp:CheckBox ID="Chk_Titulo_Profesional" runat="server"  Text="Titulo profesional" ></asp:CheckBox>                                 
                            </td>
                             <td style="width:15%" align="left">
                                <asp:CheckBox ID="Chk_Cedula_Profesional" runat="server"  Text="Cedula profesional" ></asp:CheckBox>         
                            </td>
                            <td  style="width:15%" align="left">  
                                <asp:CheckBox ID="Chk_Curriculum" runat="server"  Text="Curriculum vitae" ></asp:CheckBox>  
                            </td>
                             <td  style="width:15%" align="left">  
                                <asp:CheckBox ID="Chk_Constancia" runat="server"  Text="Constancia de colegio" ></asp:CheckBox>  
                            </td>
                            <td  style="width:15%" align="left">  
                                <asp:CheckBox ID="Chk_Refrendo" runat="server"  Text="Refrendo" ></asp:CheckBox>  
                            </td>
                            <td  style="width:10%" align="left">  
                                
                            </td>
                        </tr> 
                         <tr>       
                            <td style="width:15%" align="left">
                                            
                            </td>
                            <td  style="width:15%" align="left">
                                <asp:CheckBox ID="Chk_Acreditacion" runat="server"  Text="Acreditacion de especialidad" ></asp:CheckBox>                              
                            </td>
                             <td style="width:15%" align="left">
                                <asp:CheckBox ID="Chk_Conformidad_Finaza" runat="server"  Text="Conformidad para autorizar fianza" ></asp:CheckBox>         
                            </td>
                            <td  style="width:15%" align="left">  
                                <asp:CheckBox ID="Chk_Curso" runat="server"  Text="Asistencia a curso promovido por direccion" ></asp:CheckBox>  
                            </td>
                             <td  style="width:15%" align="left">  
                            </td>
                            <td  style="width:15%" align="left">   
                            </td>
                            <td  style="width:10%" align="left">   
                            </td>
                        </tr>                        
                    </table>
                </div>
                
                 <table width="98%">
                        <tr>
                            <td rowspan="5">
                             </td>
                        </tr>
                    </table>
                
                 <%-- Manejo del Grid View--%>
                <div id="Div_Grid_Formatos" runat="server" style="display:block">
                    <table width="98%">
                        <tr>
                            <td style="width:100%;text-align:center;vertical-align:top;">
                                <center>
                                    <div id="Div_Avance_Obra" runat="server" 
                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                        <asp:GridView ID="Grid_Inspector" runat="server" AutoGenerateColumns="False" 
                                            CssClass="GridView_1" Width="100%"  
                                            EmptyDataText="No se encontraron datos"
                                            onselectedindexchanged="Grid_Inspector_SelectedIndexChanged" 
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:ButtonField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="INSPECTOR_ID" HeaderText="ID" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>  
                                                <%-- 2 --%>  
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="55%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="55%" />
                                                </asp:BoundField> 
                                                <%-- 3 --%>   
                                                <asp:BoundField DataField="TIPO_PERITO" HeaderText="Tipo Perito" Visible="True">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="40%" />
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
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



