<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Tab_Nom_IMSS.aspx.cs" Inherits="paginas_Nomina_Frm_Tab_Nom_IMSS" Title="Tabla de IMSS" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_IMSS" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>
            <div id="Div_IMSS" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Tabla de IMSS</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align = "left">
                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>IMSS ID</td>
                        <td>
                            <asp:TextBox ID="Txt_IMSS_ID" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>                                                      
                        </td>                             
                    </tr>
                    <tr>
                        
                        <td>*Porcentaje de Enfermedad (Maternidad en Especie)</td>
                        <td>
                            <asp:TextBox ID="Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS" runat="server" Width="150px" TabIndex="4" MaxLength="3"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS" runat="server"  TargetControlID="Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Porcentaje Enfermedad Maternidad Especie IMSS [0-100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Porcentaje_Enfermedad_Maternidad_Especie_IMSS" runat="server" TargetControlID="Cv_Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS(sender, args) {     
                                     var Porcentaje_Enfermedad_Maternidad_Especie_IMSS = document.getElementById("<%=Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.ClientID%>").value;   
                                     if ( (Porcentaje_Enfermedad_Maternidad_Especie_IMSS < 0) || (Porcentaje_Enfermedad_Maternidad_Especie_IMSS > 100) ){  
                                        document.getElementById("<%=Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                                                  
                        </td>
                    </tr>                    
                    <tr>
                        <td>*Porcentaje de Enfermedad (Maternidad en Pesos)</td>
                        <td>
                            <asp:TextBox ID="Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS" runat="server" Width="150px" TabIndex="5" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="F_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS" runat="server"  TargetControlID="Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Porcentaje Enfermedad Maternidad Pesos IMSS [0-100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS" runat="server" TargetControlID="Cv_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS(sender, args) {     
                                     var Porcentaje_Enfermedad_Maternidad_Pesos_IMSS = document.getElementById("<%=Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.ClientID%>").value;   
                                     if ( (Porcentaje_Enfermedad_Maternidad_Pesos_IMSS < 0) || (Porcentaje_Enfermedad_Maternidad_Pesos_IMSS > 100) ){  
                                        document.getElementById("<%=Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                              
                        </td>
                    </tr>
                    <tr>
                        <td>*Porcentaje de Invalidez de Vida</td>
                        <td>
                            <asp:TextBox ID="Txt_Porcentaje_Invalidez_Vida_IMSS" runat="server" Width="150px" TabIndex="6" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="F_Txt_Porcentaje_Invalidez_Vida_IMSS" runat="server"  TargetControlID="Txt_Porcentaje_Invalidez_Vida_IMSS"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_Invalidez_Vida_IMSS" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Porcentaje Invalidez Vida [0-100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_Invalidez_Vida_IMSS"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_Invalidez_Vida_IMSS"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Invalidez_Vida_IMSS" runat="server" TargetControlID="Cv_Txt_Porcentaje_Invalidez_Vida_IMSS" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_Invalidez_Vida_IMSS(sender, args) {     
                                     var Porcentaje_Invalidez_Vida_IMSS = document.getElementById("<%=Txt_Porcentaje_Invalidez_Vida_IMSS.ClientID%>").value;   
                                     if ( (Porcentaje_Invalidez_Vida_IMSS < 0) || (Porcentaje_Invalidez_Vida_IMSS > 100) ){  
                                        document.getElementById("<%=Txt_Porcentaje_Invalidez_Vida_IMSS.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                             
                        </td>
                    </tr>
                    <tr>
                        <td>*Porcentaje de Cesantia de Vejez</td>
                        <td>
                            <asp:TextBox ID="Txt_Porcentaje_Cesantia_Vejez_IMSS" runat="server" Width="150px" TabIndex="7" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="F_Txt_Porcentaje_Cesantia_Vejez_IMSS" runat="server"  TargetControlID="Txt_Porcentaje_Cesantia_Vejez_IMSS"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_Cesantia_Vejez_IMSS" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Porcentaje Cesantia Vejez IMSS [0-100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_Cesantia_Vejez_IMSS"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_Cesantia_Vejez_IMSS"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Cesantia_Vejez_IMSS" runat="server" TargetControlID="Cv_Txt_Porcentaje_Cesantia_Vejez_IMSS" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_Cesantia_Vejez_IMSS(sender, args) {     
                                     var Porcentaje_Cesantia_Vejez_IMSS = document.getElementById("<%=Txt_Porcentaje_Cesantia_Vejez_IMSS.ClientID%>").value;   
                                     if ( (Porcentaje_Cesantia_Vejez_IMSS < 0) || (Porcentaje_Cesantia_Vejez_IMSS > 100) ){  
                                        document.getElementById("<%=Txt_Porcentaje_Cesantia_Vejez_IMSS.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                              
                        </td>
                    </tr>
                    <tr>
                        <td>*Excedente SMG DF</td>
                        <td>
                            <asp:TextBox ID="Txt_Excedente_SMG_DF" runat="server" Width="150px" TabIndex="7" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Excedente_SMG_DF" runat="server"  TargetControlID="Txt_Excedente_SMG_DF"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="CV_Txt_Excedente_SMG_DF" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Excedente tiene que ser mayor a 0."
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Excedente_SMG_DF"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Excedente_SMG_DF"/>
                            <cc1:ValidatorCalloutExtender ID="VCE_CV_Txt_Excedente_SMG_DF" runat="server" TargetControlID="CV_Txt_Excedente_SMG_DF" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Excedente_SMG_DF(sender, args) {     
                                     var Excedente_SMG_DF = document.getElementById("<%=Txt_Excedente_SMG_DF.ClientID%>").value;   
                                     if ( (Excedente_SMG_DF < 0) ){  
                                        document.getElementById("<%=Txt_Excedente_SMG_DF.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                              
                        </td>
                    </tr>
                    <tr>
                        <td>*Prestaciones de Dinero</td>
                        <td>
                            <asp:TextBox ID="Txt_Prestaciones_Dinero" runat="server" Width="150px" TabIndex="7" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Prestaciones_Dinero" runat="server"  TargetControlID="Txt_Prestaciones_Dinero"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="CV_Txt_Prestaciones_Dinero" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Prestaciones de Dinero tiene que ser mayor a 0."
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Prestaciones_Dinero"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Prestaciones_Dinero"/>
                            <cc1:ValidatorCalloutExtender ID="VCE_CV_Txt_Prestaciones_Dinero" runat="server" TargetControlID="CV_Txt_Prestaciones_Dinero" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Prestaciones_Dinero(sender, args) {     
                                     var Excedente_SMG_DF = document.getElementById("<%=Txt_Prestaciones_Dinero.ClientID%>").value;   
                                     if ( (Excedente_SMG_DF < 0) ){  
                                        document.getElementById("<%=Txt_Prestaciones_Dinero.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                              
                        </td>
                    </tr>
                    <tr>
                        <td>*Gastos Medicos</td>
                        <td>
                            <asp:TextBox ID="Txt_Gastos_Medicos" runat="server" Width="150px" TabIndex="7" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Gastos_Medicos" runat="server"  TargetControlID="Txt_Gastos_Medicos"
                                FilterType="Custom, Numbers" ValidChars="."/>         
                            <asp:CustomValidator ID="CV_Txt_Gastos_Medicos" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Gastos Medicos tiene que ser mayor a 0."
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Gastos_Medicos"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Gastos_Medicos"/>
                            <cc1:ValidatorCalloutExtender ID="CVE_CV_Txt_Gastos_Medicos" runat="server" TargetControlID="CV_Txt_Gastos_Medicos" PopupPosition="Right"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Gastos_Medicos(sender, args) {     
                                     var Excedente_SMG_DF = document.getElementById("<%=Txt_Gastos_Medicos.ClientID%>").value;   
                                     if ( (Excedente_SMG_DF < 0) ){  
                                        document.getElementById("<%=Txt_Gastos_Medicos.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                              
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">Comentarios</td>
                        <td>
                            <asp:TextBox ID="Txt_Comentarios_IMSS" runat="server" TabIndex="8" MaxLength="250"
                                TextMode="MultiLine" Width="400px" AutoPostBack="True"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_IMSS" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_IMSS" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_IMSS" runat="server" TargetControlID="Txt_Comentarios_IMSS"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

