<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/AutoFinit.Master" AutoEventWireup="true" CodeBehind="AutomatasFinitos.aspx.cs" Inherits="AutoFinit.Formulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container " role="main">
        <!-- Main jumbotron for a primary marketing message or call to action -->
        <div class="jumbotron">
            <h1>Automatas Finitos</h1>
            <br/>

            <div class="container">
              <div class="row">
                <div class="col-md-10 col-md-offset-2">

                    <h2>No Determinado</h2>
                    <br/>
                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoAFN">Estados</label>
                            <asp:Label ID="estadoAFN" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="alfabetoAFN">Alfabeto</label>
                            <asp:Label ID="alfabetoAFN" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoInicialAFN">Estado Inicial</label>
                            <asp:Label ID="estadoInicialAFN" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoFinalAFN">Estado Final</label>
                            <asp:Label ID="estadoFinalAFN" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-9 col-md-offset-3">
                        <div class="form-group"><br/> 
                            <asp:Table ID="tabla1" runat="server" Width="155px" BackColor="White" BorderColor="#0099FF" BorderStyle="Double" ForeColor="Black" GridLines="Both">
                            </asp:Table> 
                        </div>
                    </div>
                        
                    <div class="col-md-12"><br/><br/></div>

                    <h2>Determinado</h2>
                    <br/>
                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoAFD">Estados</label>
                            <asp:Label ID="estadoAFD" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="alfabetoAFD">Alfabeto</label>
                            <asp:Label ID="alfabetoAFD" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoInicialAFD">Estado Inicial</label>
                            <asp:Label ID="estadoInicialAFD" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="estadoFinalAFD">Estado Final</label>
                            <asp:Label ID="estadoFinalAFD" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <asp:Table ID="tabla2" runat="server" Width="155px" BackColor="White" BorderColor="#0099FF" BorderStyle="Double" ForeColor="Black" GridLines="Both">
                            </asp:Table>
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="GridView1">Matrix de Estados</label><br/>
                             <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="300px">
                                 <AlternatingRowStyle BackColor="White" />
                                 <EditRowStyle BackColor="#2461BF" />
                                 <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                 <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                 <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                 <RowStyle BackColor="#EFF3FB" />
                                 <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                 <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                 <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                 <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                 <SortedDescendingHeaderStyle BackColor="#4870BE" />
                             </asp:GridView>
                        </div>
                    </div>
                        
                    <div class="col-md-12"><br/><br/></div>

                    <div class="col-md-12">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="lenguaje">Validar Lenguaje</label>
                            <asp:TextBox ID="lenguaje" CssClass="form-control" runat="server"></asp:TextBox>
                            <!--<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="lenguaje"></asp:RangeValidator>-->
                        </div>
                        <div class="form-group col-md-5">
                            <label class="control-label" for="resultado">Resultado</label>
                            <asp:Label ID="resultadoAF" CssClass="form-control" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="centrado">
                        <div class="row"> 
                            <div class="col-md-3"></div>
                            <div class="col-md-2">
                                <asp:Button ID="BtnValidar" CssClass="btn btn-primary" Text="Validar" runat="server" OnClick="BtnValidar_Click"></asp:Button>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-3">
                                <asp:Button ID="BtnExportar" CssClass="btn btn-primary" Text ="Exportar XML" runat="server" OnClick="BtnExportar_Click"></asp:Button>
                            </div>
                            <div class="col-md-2"></div>
                        </div>
                    </div>
                </div>
              </div>
            </div>
        </div>
    </div>

</asp:Content>
