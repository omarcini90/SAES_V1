<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tsimu.aspx.cs" Inherits="SAES_v1.tsimu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .icon_regresa{
            width:100%;
            text-align:center;
            border-color:#FFF !important;
        }
        .icon_regresa:hover{
            background-color:#fff !important;
            color: #26b99a;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="x_title">
        <h2>
            <img src="Images/Admisiones/lucro.png" style="width: 30px;" /><small>Simulador de Cuotas</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end">
            <li class="nav-item">
                <a class="nav-link " href="tpers.aspx">Datos Generales</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="taldi.aspx">Dirección</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talte.aspx">Teléfono</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talco.aspx">Correo</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="tadmi.aspx">Solicitud</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tredo.aspx">Documentos</a>
            </li>
        </ul>

        <asp:UpdatePanel ID="upd_simulador" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_tadmi" runat="server">
                    <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                        <div class="col-md-2" style="margin:auto;text-align:center;">
                            <asp:LinkButton ID="back" runat="server" CssClass=" icon_regresa  btn-outline-secondary" OnClick="back_Click"><i class="fas fa-arrow-circle-left fa-2x"></i></asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_txt_matricula" class="form-label">Matrícula</label>
                            <asp:TextBox ID="txt_matricula" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox><!--Configurar BackEnd la longitud de la BD-->
                        </div>
                        <div class="col-md-5">
                            <label for="ContentPlaceHolder1_txt_nombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="w-100"></div>  
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_periodo" class="form-label">Periodo</label>
                            <asp:DropDownList ID="ddl_periodo" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>                      
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_Campus" class="form-label">Campus</label>
                            <asp:DropDownList ID="ddl_Campus" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_Programa" class="form-label">Programa</label>
                            <asp:DropDownList ID="ddl_Programa" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>
                        <div class="w-100"></div>  
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_tipo_ingreso" class="form-label">Tipo de ingreso</label>
                            <asp:DropDownList ID="ddl_tipo_ingreso" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_tasa_f" class="form-label">Tasa Financiera</label>
                            <asp:DropDownList ID="ddl_tasa_f" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_plan_beca" class="form-label">Plan de Cobro/Beca</label>
                            <asp:DropDownList ID="ddl_plan_beca" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>    
                        <asp:Label ID="lbl_id_pers" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_consecutivo" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
                <div id="table_tsimu">
                    <%--<asp:GridView ID="GridSimulador" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small">
                         <Columns>
                            <asp:ButtonField ButtonType="image" ImageUrl="~/Images/Generales/hacer-clic.png" ControlStyle-Height="24px" ControlStyle-Width="24px" CommandName="select" HeaderText="Seleccionar" ItemStyle-CssClass="button_select" />
                            <asp:BoundField DataField="ID_NUM" HeaderText="Id_Num">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CLAVE" HeaderText="Matrícula" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" />
                             <asp:BoundField DataField="PERIODO" HeaderText="Periodo" />
                            <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TURNO" HeaderText="Turno" />
                            <asp:BoundField DataField="CAMPUS" HeaderText="Campus" />
                            <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" />
                            <asp:BoundField DataField="TIIN" HeaderText="Tipo de Ingreso" />
                             <asp:BoundField DataField="TASA" HeaderText="Tasa Financiera" />
                             <asp:BoundField DataField="E_PRO" HeaderText="E_PRO">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                             <asp:BoundField DataField="PROMEDIO" HeaderText="Promedio" />
                             <asp:BoundField DataField="C_ESTATUS" HeaderText="C_ESTATUS">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha Registro" />
                        </Columns>
                        <SelectedRowStyle CssClass="selected_table" />
                        <HeaderStyle BackColor="#2a3f54" ForeColor="white" />
                    </asp:GridView>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
