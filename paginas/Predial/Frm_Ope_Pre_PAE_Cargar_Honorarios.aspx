<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_PAE_Cargar_Honorarios.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_PAE_Cargar_Honorarios" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">    
    <style type="text/css">
    body
    {
        font: normal 12px auto "Trebuchet MS", Verdana;    
        background-color: #ffffff;
        color: #4f6b72;       
    }

    .popUpStyle
    {        
        background-color: #000;   
        -moz-opacity: 0.50;
        filter: alpha(opacity=50);
    }
    
    .drag
    { 
        background-color: #FFFFFF;  
        border: solid 2px #5D7B9D;
        padding: 10px 10px 10px 10px;
    }
    
    .link
    {
    	color:Black;
    }
    .Label
    {
            width: 163px;
    }
    .TextBox
     {
        TEXT-ALIGN: right
     }
     
        </style>
    
      <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";
        $(function(){
           
           $('.pantalla').click(function(){
               alert('Hola');
           
           });
         
        });

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
        //Codigo de sergio
        //registra los eventos para la página
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded);
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);

        //procedimientos de evento
        function beginRequest(sender, args) {
            $(function() {
                $('.Detalles_Gastos').click(function() {
                    var id = $(this).attr('id_cuenta');       
                    Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Detalles_Gastos_De_Ejecucion.aspx?Cuenta_Predial='+id, 'center:yes;resizable:yes;status:no;dialogWidth:290px;dialogHeight:225px;dialogHide:true;help:no;scroll:no');
                });
            });           
        }
        function PageLoaded(sender, args) {}
        function endRequestHandler(sender, args) {
            $(function() {
                $('.Detalles_Gastos').click(function() {
                    var id = $(this).attr('id_cuenta');
                    Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Detalles_Gastos_De_Ejecucion.aspx?Cuenta_Predial=' + id, 'center:yes;resizable:yes;status:no;dialogWidth:290px;dialogHeight:225px;dialogHide:true;help:no;scroll:no');
                });
            });  
        } 
        
        function SelectAllCheckboxes(spanChk)
        {
            // Added as ASPX uses SPAN for checkbox 
            var oItem = spanChk.children;
            var theBox= (spanChk.type=="checkbox") ? spanChk : spanChk.children.item[0];
            xState=theBox.checked;
            elm=theBox.form.elements;

           for(i=0;i<elm.length;i++)
             if(elm[i].type=="checkbox" && elm[i].id!=theBox.id)
             {
               if(elm[i].checked!=xState)
                 elm[i].click();
             }
         }
         function HighlightRow(chkB)
        {
            var IsChecked = chkB.checked;           
            if(IsChecked)
              {
                   chkB.parentElement.parentElement.style.backgroundColor='#228b22'; 
                   chkB.parentElement.parentElement.style.color='white';
              }else
              {
                   chkB.parentElement.parentElement.style.backgroundColor='white';
                   chkB.parentElement.parentElement.style.color='black';
              }
        }
        
        function SelectAllCheckboxesMoreSpecific(spanChk)
        {       
           var IsChecked = spanChk.checked;
           var Chk = spanChk;
              Parent = document.getElementById("<%=Grid_Generadas.ClientID%>");                                            
              for(i=0;i< Parent.rows.length;i++)
              {     
                  var tr = Parent.rows[i];
                 //var td = tr.childNodes[0];                                  
                  var td = tr.firstChild;         
                  var item =  td.firstChild;                     
                  if(item.id != Chk && item.type=="checkbox")
                  {           
                      if(item.checked!= IsChecked)
                      {    
                          item.click();    
                      }
                  }
              }           
          }
//          function SelectAll(  tb, chk ) 
//          {
//            var check = chk.checked;
//            $("#" + tb + " tr td :checkbox").each(function(){
//                this.checked = check;
//            });
//          }
 
    </script>

<%--<script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>--%>
<%--<script language="javascript" type="text/javascript">
    function Selecciona_Todos() {
        $().ready(function() {
            $("#<%=Grid_Generadas.ClientID%> :checkbox").click(function() {
                var seleccionado = false;
                $("#<%=Grid_Generadas.ClientID%> :checkbox").each(function() {
                    if (this.checked)
                        seleccionado = true;
                });

                var botondeshabilitar = $("#<%=Btn_Guardar.ClientID%>");
                if (seleccionado)
                    botondeshabilitar.removeAttr('disabled');
                else
                    botondeshabilitar.attr('disabled', 'disabled');
            })
        });
    }
</script>--%>
     
 </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Cargar_Honorarios" runat="server"  AsyncPostBackTimeout="3600" EnableScriptGlobalization="true"/>    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0" >
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Cargar honorarios </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">                               
                                            <asp:ImageButton ID="Btn_Busca_Cuentas" runat="server" ToolTip="Buscar" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" OnClick="Btn_Busca_Cuentas_Click" />
                                            <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" OnClick="Btn_Guardar_Click"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"/>
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Folio>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
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
                <div id="Div_Generadas" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <caption>
                            <%---------------- Generadas ----------------%>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Cuenta predial</td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Numero_Cuenta" runat="server" Width="84%" onkeyup='this.value=this.value.toUpperCase();'/>
                                    <asp:ImageButton ID="Btn_Busca_Cuenta" runat="server" Height="22px" 
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="22px" OnClick="Btn_Busca_Cuenta_Click"/>
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Etapa
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Etapa" runat="server" Width="99%">
                                    <asp:ListItem><-- SELECCIONE --></asp:ListItem>
                                    <asp:ListItem>DETERMINACION</asp:ListItem>
                                    <asp:ListItem>REQUERIMIENTO</asp:ListItem>
                                    <asp:ListItem>EMBARGO</asp:ListItem>
                                    <asp:ListItem>REMOCION</asp:ListItem>
                                    <asp:ListItem>ALMONEDA</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Folio inicial
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Folio_Inicial" runat="server" Width="96.4%"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Inicial" runat="server" TargetControlID="Txt_Folio_Inicial"
                                        FilterType="Numbers" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Folio final
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Folio_Final" runat="server" Width="96.4%" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Final" runat="server" TargetControlID="Txt_Folio_Final"
                                        FilterType="Numbers" />
                                </td>
                            </tr>     
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Despacho
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Asignado_a" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Estatus
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%">
                                        <asp:ListItem Text="&lt;--SELECCIONE--&gt;" Value="0" />
                                        <asp:ListItem Text="NOTIFICACION" Value="1" />
                                        <asp:ListItem Text="NO DILIGENCIADO" Value="2" />
                                        <asp:ListItem Text="ILOCALIZABLE" Value="3" />
                                        <asp:ListItem Text="PENDIENTE" Value="4" />   
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Colonia
                                    </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Colonia" runat="server" Width="87%" ReadOnly="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Seleccionar_Colonia" runat="server" ToolTip="Seleccionar Calle y Colonia"
                                        TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                        Width="22px" OnClick="Btn_Seleccionar_Colonia_Click" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Calle
                                    </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Calle" runat="server" Width="87%" ReadOnly="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Seleccionar_Calle" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                        TabIndex="10" ToolTip="Seleccionar Calle y Colonia" Width="22px" OnClick="Btn_Seleccionar_Calle_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center; width: 20%;">
                                    <asp:Label ID="Lbl_Msg_Col" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Generadas" runat="server" 
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                        CssClass="GridView_1" HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;"
                                        DataKeyNames="NO_DETALLE_ETAPA" OnRowDataBound="Grid_Generadas_OnRowDataBound"
                                        Width="100%" OnPageIndexChanging="Grid_Generadas_PageIndexChanging">
                                        <Columns> 
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="Chk_Todos" ToolTip="Seleccionar Todos" onclick="javascript:SelectAllCheckboxes(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="Chk_Uno" ToolTip="Cargar Honorarios"  onclick="javascript:HighlightRow(this);" AutoPostBack="true" OnCheckedChanged="Chk_Uno_CheckedChanged"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cuenta">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="Btn_Link_Cuenta" class="Detalles_Gastos" id_cuenta='<%# Eval("CUENTA") %>' Text='<%# Eval("CUENTA") %>' ForeColor="#000000"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ADEUDO" HeaderText="Adeudo" DataFormatString="{0:C2}">
                                                <ItemStyle HorizontalAlign="right"/>
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ENTREGA" HeaderText="Entrega" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ETAPA" HeaderText="Etapa" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HONORARIOS_PAGAR" HeaderText="Honorarios a pagar">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OMITIDA" HeaderText="Omitida" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CONVENIO" HeaderText="Convenio" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                            <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </caption>
                    </table>
                    <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                    <br /><br /><br />
                    <br /><br /><br />
                    <br /><br /><br />
                    <br /><br /><br />
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
