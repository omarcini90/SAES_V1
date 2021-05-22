<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="taldi.aspx.cs" Inherits="SAES_v1.taldi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Admisiones/curriculum.png" style="width: 30px;" /><small>Catálogo de Campus</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end">
            <li class="nav-item">
                <a class="nav-link " href="tpers.aspx">Datos Generales</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="taldi.aspx">Dirección</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talte.aspx">Teléfono</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="talco.aspx">Correo</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tadmi.aspx">Solicitud</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="tredo.aspx">Documentos</a>
            </li>
        </ul>
        <asp:UpdatePanel ID="upd_tladi" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_taldi" runat="server">
                    <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_matricula" class="form-label">Matrícula</label>
                            <asp:TextBox ID="txt_matricula"  runat="server" CssClass="form-control"></asp:TextBox> <!--Configurar BackEnd la longitud de la BD-->
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_txt_nombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_tipo_direccion" class="form-label">Tipo de Dirección</label>
                            <asp:DropDownList ID="ddl_tipo_direccion" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_estatus" class="form-label">Estatus</label>
                            <asp:DropDownList ID="ddl_estatus" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="w-100"></div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_pais" class="form-label">Pais</label>
                            <asp:DropDownList ID="ddl_pais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_estado" class="form-label">Estado</label>
                            <asp:DropDownList ID="ddl_estado" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_ddl_delegacion" class="form-label">Delegación-Municipio</label>
                            <asp:DropDownList ID="ddl_delegacion" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="w-100"></div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_txt_zip" class="form-label">Codigo Postal</label>
                            <asp:TextBox ID="txt_zip" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_txt_colonia" class="form-label">Colonia</label>
                            <asp:TextBox ID="txt_colonia" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="ContentPlaceHolder1_txt_ciudad" class="form-label">Ciudad</label>
                            <asp:TextBox ID="txt_ciudad" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="w-100"></div>
                        <div class="col-md-10">
                            <label for="ContentPlaceHolder1_txt_dirección" class="form-label">Calle</label>
                            <asp:TextBox ID="txt_dirección" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_taldi" runat="server">
                    <div class="col-md-4" style="text-align: center; margin-top: 15px;">
                        <asp:Button ID="btn_cancel" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" />
                        <asp:Button ID="btn_save" runat="server" CssClass="btn btn-round btn-success" Text="Agregar" />
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" />
                    </div>
                </div>
                <div id="table_tladi">
                    <asp:GridView ID="GridDireccion" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small">
                        <%--<Columns>
                            <asp:ButtonField ButtonType="image" ImageUrl="~/Images/Generales/hacer-clic.png" ControlStyle-Height="24px" ControlStyle-Width="24px" CommandName="select" HeaderText="Seleccionar" ItemStyle-CssClass="button_select" />
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" />
                            <asp:BoundField DataField="NOMBRE" HeaderText="Periodo" />
                            <asp:BoundField DataField="OFICIAL" HeaderText="Oficial" />
                            <asp:BoundField DataField="FECHA_INI" HeaderText="Fecha Inicial" />
                            <asp:BoundField DataField="FECHA_FIN" HeaderText="Fecha Final" />
                            <asp:BoundField DataField="C_ESTATUS" HeaderText="Estatus_code">
                                <HeaderStyle CssClass="ocultar" />
                                <ItemStyle CssClass="ocultar" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha Registro" />
                        </Columns>--%>
                        <SelectedRowStyle CssClass="selected_table" />
                        <HeaderStyle BackColor="#2a3f54" ForeColor="white" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
