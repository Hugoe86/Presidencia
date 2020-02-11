<%@ Page Title="Asuntos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Cat_Ate_Asuntos.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Asuntos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
    //-->
    function pageLoad() {
        Contar_Caracteres();
    }

    function Contar_Caracteres() {
        $('textarea[id$=Txt_Descripcion]').keyup(function() {
            var Caracteres = $(this).val().length;

            if (Caracteres > 500) {
                this.value = this.value.substring(0, 500);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            } else {
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }

            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 500 ]');
        });
    }    
            
   </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Acciones" runat="server"  AsyncPostBackTimeout="3600" 
    EnableScriptGlobalization="true" EnableScriptLocalization="true"/>
<%--    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>--%>
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
                            Asuntos de Atenci&oacute;n Ciudadana
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>                          
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" Text="Mensajes de advertencia"></asp:Label>
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
                                ToolTip="Buscar" ></asp:TextBox>
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
                            <td style="width:16%;">
                                <asp:HiddenField ID="HF_ID" runat="server" />
                            </td>
                            <td style="width:32%;">
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
                               &nbsp;&nbsp;Clave 
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
                                *&nbsp;Estatus
                            </td>
                            <td style="text-align:right;">                                
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                </asp:DropDownList>                               
                            </td>
                        </tr>
                        <tr>
                            <td >
                                *&nbsp;Nombre
                            </td>                                
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="99%" MaxLength="100"></asp:TextBox>   
                                <cc1:filteredtextboxextender id="FTB_Nombre" runat="server" targetcontrolid="Txt_Nombre"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars='ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿#" '>
                                    </cc1:filteredtextboxextender>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top;">
                                &nbsp;&nbsp;Descripci&oacute;n
                            </td>                                
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99%" MaxLength="500" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                <cc1:filteredtextboxextender id="FTB_Descripción" runat="server" targetcontrolid="Txt_Descripcion"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars='ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿#" '>
                                    </cc1:filteredtextboxextender> 
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                                 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *&nbsp;Unidad responsable
                            </td>                                
                            <td colspan="4">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="96%">
                                </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Dependencia_Click" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div style="overflow:auto;overflow-x: hidden; overflow-y: auto; height:320px; width:98.8%; vertical-align:top; border-style:outset; border-color: Silver;" > 
                    <table width="99.5%"  border="0" cellspacing="0" >
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="Grid_Datos" runat="server" AutoGenerateColumns="False" CssClass="GridView_1" 
                                    GridLines="None" Width="99%"
                                    DataKeyNames="ASUNTO_ID"
                                    HeaderStyle-CssClass="tblHead"                                     
                                    EmptyDataText="No se encontraron datos para mostrar">
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                    
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Click"                                                    
                                                    CommandArgument='<%# Eval("ASUNTO_ID") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="35px" />
                                            <ItemStyle HorizontalAlign="Center" Width="35px" />
                                        </asp:TemplateField>  
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ASUNTO_ID" HeaderText="ID" Visible="false" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>                                        
                                    </Columns>
                                </asp:GridView>                                
                            </td>
                        </tr>
                    </table>
                    
                    </table>                
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

