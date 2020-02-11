<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Dependencias.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Dependencias" Title="Catálogo Unidades Responsables" Culture="en-Us" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Cat_Sap_Dependencias.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function pageLoad(){
            jQuery.fx.interval = 100;
            
            $('#Btn_Ctrl_Informacion').click(function(e){
                e.preventDefault();
                $("#Div_Presupuesto_Sueldos").toggle(1000);
            });
        }    
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Dependencias" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">    
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
           
            
            <div id="Div_Dependencias" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Cat&aacute;logo de Unidad Responsable
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
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
                                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('¿Está seguro de eliminar la unidad responsable seleccionada?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Dependencia" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Nombre"
                                                            Width="180px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Dependencia" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Dependencia" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Dependencia" 
                                                            runat="server" TargetControlID="Txt_Busqueda_Dependencia" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>                                                    
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Consultar" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Dependencia_Click" />                                        
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
                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                    <tr>
                        <td style="text-align:left;width:20%;"></td>
                        <td style="text-align:left;width:30%;"></td>
                        <td style="text-align:left;width:20%;"></td>
                        <td style="text-align:left;width:30%;"></td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Clave
                        </td>
                        <td style="text-align:left;width:30%;"> 
                            <asp:TextBox ID="Txt_Clave_Dependecia" runat="server" Width="97%" MaxLength="5" TabIndex="9"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Clave_Dependecia" 
                                runat="server" TargetControlID="Txt_Clave_Dependecia" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"/>                                   
                            <span id="Mensaje" class="watermarked"></span> 
                        </td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Dependencia" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td style="text-align:left;width:20%;">
                            Nombre
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre_Dependencia" runat="server" MaxLength="100" TabIndex="10" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Dependencia" 
                                runat="server" TargetControlID="Txt_Nombre_Dependencia" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Dependencia
                        </td>
                        <td style="text-align:left;width:30%;">
                           <asp:DropDownList ID="Cmb_Grupo_Dependencia" runat="server" Width="100%" TabIndex="8"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;Área Funcional
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Area_Funcional" runat="server" Width="100%" TabIndex="8"/>
                        </td>
                        
                    </tr>                    
                   
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Dependencia" runat="server" TabIndex="9" MaxLength="250"
                                TextMode="MultiLine" Width="99.5%"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Dependencia" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Dependencia" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Dependencia" 
                                runat="server" TargetControlID="Txt_Comentarios_Dependencia" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>      
                        </td>
                    </tr>
                    <%-- parte oculta es la unidad responsable id ---%>
                    <tr>
                        <td style="text-align:left;width:20%;">
                           <asp:Label ID="Lbl_Unidad_Responsable_ID" runat="server" Visible="false"></asp:Label>
                        </td>
                       
                        <td style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Dependencia_ID" runat="server" ReadOnly="True" Width="98%" visible="false"/>
                        </td>
                    </tr>
                </table>  
                
                <br />
                
                <cc1:TabContainer ID="TPnl_Contenedor" runat="server" ActiveTabIndex="0" Width="98%">
                    <cc1:TabPanel ID="Pnl_Dependencias" runat="server" HeaderText="Dependencias">
                        <HeaderTemplate>
                            Dependencias
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Grupo_Dependencias" runat="server" style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;"  visible="true">
                                <asp:GridView ID="Grid_Dependencias" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                    onselectedindexchanged="Grid_Dependencias_SelectedIndexChanged" 
                                    onpageindexchanging="Grid_Dependencias_PageIndexChanging"
                                    AllowSorting="True" OnSorting="Grid_Dependencias_Sorting" HeaderStyle-CssClass="tblHead">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="7%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Dependencia_ID" HeaderText="Unidad ID" 
                                            Visible="True" SortExpression="Dependencia_ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                            <ItemStyle HorizontalAlign="Left" Width="23%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre" HeaderText="Unidad Responsable" Visible="True" SortExpression="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView> 
                            </div>                       
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Pnl_Fuente_Financiamiento" runat="server" HeaderText="Fuente Financiamiento">
                        <HeaderTemplate>
                            Fuente Financiamiento
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="text-align:left; width:25%;">
                                        Fuente Financiamiento
                                    </td>
                                    <td style="text-align:left; width:45%;">
                                        <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Width="100%"/>
                                    </td>
                                    <td style="text-align:left; width:30%;">
                                        <asp:Button ID="Btn_Agregar_Fte_Financiamiento" runat="server" Text="Agregar Fte. Financiamiento" Width="100%"
                                            CssClass="button_agregar" OnClick="Btn_Agregar_Fte_Financiamiento_Click"/>
                                    </td>
                                </tr>
                            </table>
                            
                            <hr />
                            <div id="Div_Fuente_Financiamiento" runat="server" style="overflow:auto;height:225px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;"  visible="true">
                                <asp:GridView ID="Grid_Fuentes_Financiamiento" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    OnPageIndexChanging="Grid_Fuentes_Financiamiento_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="FUENTE_FINANCIAMIENTO_ID" HeaderText=""/>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                            <ItemStyle HorizontalAlign="Left" Width="80%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Fte_Financiamiento" runat="server" CommandName="Eliminar_Fte_Financiamiento" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                    OnClientClick="return confirm('¿Está seguro de eliminar la Fte. Financiamiento?');" 
                                                    CommandArgument='<%# Eval("FUENTE_FINANCIAMIENTO_ID") %>' OnClick="Btn_Eliminar_Fte_Financiamiento_Click"/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />                                                        
                                        </asp:TemplateField>                                        
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>  
                            </div>                               
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    <cc1:TabPanel ID="Pnl_Programas" runat="server" HeaderText="Programas">
                        <HeaderTemplate>
                            Programas
                        </HeaderTemplate>
                        <ContentTemplate>    
                            <table width="100%">
                                <tr>
                                    <td style="text-align:left; width:25%;">
                                        Programa
                                    </td>
                                    <td style="text-align:left; width:45%;">
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" Width="100%"/>
                                    </td>
                                    <td style="text-align:left; width:30%;">
                                        <asp:Button ID="Btn_Agregar_Programa" runat="server" Text="Agregar Programa" Width="100%"
                                            CssClass="button_agregar" OnClick="Btn_Agregar_Programa_Click"/>
                                    </td>
                                </tr>
                            </table>
                            
                            <hr />     
                            <div id="Div_Programas" runat="server" style="overflow:auto;height:225px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;"  visible="true">
                                <asp:GridView ID="Grid_Programas" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                    OnPageIndexChanging="Grid_Programas_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="PROYECTO_PROGRAMA_ID" HeaderText=""/>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                            <ItemStyle HorizontalAlign="Left" Width="80%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Programa" runat="server" CommandName="Eliminar_Programa" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                    OnClientClick="return confirm('¿Está seguro de eliminar el Programa seleccionado?');" 
                                                    CommandArgument='<%# Eval("PROYECTO_PROGRAMA_ID") %>' OnClick="Btn_Eliminar_Programa_Click"/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />                                                        
                                        </asp:TemplateField>                                         
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>  
                            </div>                                                        
                        </ContentTemplate>
                    </cc1:TabPanel>  
                    <cc1:TabPanel ID="Pnl_Puestos" runat="server" HeaderText="Puestos">
                        <HeaderTemplate>
                            Puestos
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:25%;">
                                        Tipo Plaza
                                    </td>
                                    <td style="width:45%;">
                                        <asp:DropDownList ID="Cmb_Tipo_Plaza" runat="server" Width="100%">
                                            <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                            <asp:ListItem>BASE-SUBSEMUN-SUBROGADOS</asp:ListItem>
                                            <asp:ListItem>EVENTUAL</asp:ListItem>
                                            <asp:ListItem>ASIMILABLE</asp:ListItem>
                                            <asp:ListItem>PENSIONADO</asp:ListItem>
                                            <asp:ListItem>DIETA</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:30%;">
                                    </td>                                                                      
                                </tr>                            
                                <tr>
                                    <td style="width:25%;">
                                        Puestos
                                    </td>
                                    <td style="width:45%;">
                                        <asp:DropDownList ID="Cmb_Puestos" runat="server" Width="100%"/>
                                    </td>
                                    <td style="width:30%;">
                                        <asp:Button ID="Btn_Agregar_Puestos" runat="server" Text="Agregar" Width="100%"
                                            OnClick="Btn_Agregar_Puesto_Click"/>
                                    </td>                                                                      
                                </tr>
                            </table>
                            <hr />
                            
                            <div align="right"><input type="button" id="Btn_Ctrl_Informacion" value="Mostrar/Ocultar" /></div>
                            <div id='Div_Presupuesto_Sueldos' >                            
                                <table style="width: 98%">
                                    <thead>
                                        <tr>
                                            <th style="width: 50%">
                                                Sueldos
                                            </th>
                                            <th style="width: 50%">
                                                Previs&oacute;n Social M&uacute;ltiple
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:Literal ID="Ltr_Inf_Presupuestal_Sueldos" runat="server"></asp:Literal>
                                            </td>
                                            <td style="width: 50%; vertical-align: top;">
                                                <asp:Literal ID="Ltr_Inf_Presupuestal_PSM" runat="server"></asp:Literal>
                                            </td>                                        
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
             
                            <div id="Div_Puesto" runat="server" style="overflow:auto;height:275px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;"  visible="true">
                                <asp:GridView ID="Grid_Puestos" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="PUESTO_ID" HeaderText="">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="0px"/>
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="0px"/>
                                        </asp:BoundField>                                                
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" Font-Bold="true" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Width="30%" Font-Bold="true" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SALARIO_MENSUAL" HeaderText="Salario Mensual" DataFormatString="{0:c}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS_PUESTO" HeaderText="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" Font-Bold="true" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Bold="true" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left"  Width="10%" Font-Bold="true" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Bold="true" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="TIPO_PLAZA" HeaderText="Tipo Plaza">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Bold="true" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Bold="true" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Puesto" runat="server" CommandName="Eliminar_Puesto" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                    OnClientClick="return confirm('¿Está seguro de eliminar el Puesto seleccionado?');" 
                                                    CommandArgument='<%# String.Format("{0} - {1}", Eval("PUESTO_ID"), Eval("CLAVE")) %> ' OnClick="Btn_Eliminar_Puesto_Click"/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />                                                        
                                        </asp:TemplateField>                                         
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView> 
                            </div>      
                        </ContentTemplate>
                    </cc1:TabPanel>                                           
                </cc1:TabContainer>                                                       
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

