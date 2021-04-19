<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="DPWALab2Periodo2.Admin" %>
<link href="Content/bootstrap.css" rel="stylesheet"> 
<link href="Content/bootstrap.min.css" rel="stylesheet"> 
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.3/jquery.min.js"></script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DPWA - Admin</title>
</head>
<body>
    <form id="form1" runat="server">
        
        <!-- - - - 1. Registro de cuentas - - - -->
        <div class="container" style="padding-top:25px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                    <b>Registro de cuentas de Banco</b>
                </div>
                
                <div class="card-body">
                    <div class="row" style="padding-left: 10px; padding-right: 10px;">
                        
                       <div class="col-xl-3 col-md-4 col-sm-12 mb-2">
                           <label>Número de cuenta:</label>
                           <asp:TextBox 
                               ID="txtNumeroCuenta" CssClass="form-control" runat="server" 
                               placeholder="0000-0000-0000-0000" MaxLength="19" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-4 col-md-6 col-sm-12 mb-2">
                           <label>Nombre:</label>
                           <asp:TextBox 
                               ID="txtNombreCuenta" CssClass="form-control" runat="server" 
                               MaxLength="32" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-2 col-md-2 col-sm-12 mb-2">
                           <label>Password:</label>
                           <asp:TextBox 
                               ID="txtPasswordCuenta" CssClass="form-control" 
                               runat="server" MaxLength="4" TextMode="Password" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-4 col-sm-12 mb-2">
                           <label>Apertura:</label>
                           <asp:Button ID="btnRegistrarCuenta" 
                               CssClass="btn btn-outline-success btn-md btn-block" runat="server" 
                               Text="Registrar cuenta" 
                               OnClientClick="return confirm
                               ('¿Está seguro que desea registrar esta cuenta?');" OnClick="btnRegistrarCuenta_Click"/> 
                        </div>
                        
                        <div class="col-xl-12 col-md-6 col-sm-12 mb-2" style="padding-top:14px;">
                            <asp:Label 
                                ID="lblAvisoRegistrarCuenta" 
                                runat="server"
                                ForeColor="#EC1354">
                            </asp:Label>
                        </div>
                                     
                    </div>   
                </div>
            </div>
        </div>

        <!-- - - - 2. Estado de cuenta - - - -->
        <div class="container" style="padding-top:25px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                   <b>Estado de cuenta</b>
                </div>
                
                <div class="card-body">
                    <div class="row" style="padding-left: 10px; padding-right: 10px;">

                        <div class="col-xl-9 col-sm-12 mb-4">
                            <label>Cuenta de banco:</label>
                            <asp:DropDownList 
                                ID="ddlCuentaDeBanco" CssClass="form-control" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-xl-3 col-sm-12 mb-4">
                           <label>Selección de cuenta:</label>
                            <asp:Button ID="btnSeleccionCuenta" 
                                runat="server" Text="Ver estado"
                                CssClass="btn btn-outline-success btn-md btn-block" 
                                OnClick="btnSeleccionCuenta_Click"/>
                        </div>

                        <!-- - - - Estado - Cuenta de banco - - - -->

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                            <label>Número de cuenta:</label>
                            <asp:TextBox 
                                ID="txtResumenNumero" CssClass="form-control" 
                                runat="server" ReadOnly="True" style="text-align:center">
                            </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                           <label>Nombre:</label>
                           <asp:TextBox 
                               ID="txtResumenNombre" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                           <label>Fecha de apertura:</label>
                           <asp:TextBox 
                               ID="txtResumenFechaApertura" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                           <label>Saldo total:</label>
                           <asp:TextBox 
                               ID="txtResumenSaldoTotal" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <!-- - - - Estado - Cuenta corriente - - - -->
                        <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                        <div class="col-xl-12 mb-4">
                           <asp:TextBox 
                               ID="txtResumenCC" CssClass="form-control" 
                               Text="Cuenta Corriente" runat="server" ReadOnly="True" 
                               style="text-align:center; font-weight:bold;">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Saldo total (CC):</label>
                           <asp:TextBox 
                               ID="txtResumenTotalCC" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Número de operaciones:</label>
                           <asp:TextBox 
                               ID="txtResumenOperacionesCC" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Estado de cuenta:</label>
                           <asp:TextBox 
                               ID="txtResumenEstadoCC" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Gestión de estado:</label>
                            <asp:Button ID="btnActivarCC" 
                                runat="server" Text="Activar cuenta corriente"
                                CssClass="btn btn-outline-success btn-md btn-block" 
                                OnClientClick="return confirm
                                ('¿Está seguro que desea activar esta cuenta?');" 
                                Enabled="False" OnClick="btnActivarCC_Click"/>
                        </div>

                        <div class="col-xl-12 mb-2" style="padding-top:5px;">
                            <asp:Label 
                                ID="lblAvisoActivacionCC" runat="server" ForeColor="#ff7733">
                            </asp:Label>
                        </div>
                        
                        <!-- - - - Estado - Cuenta de ahorro - - - -->
                        <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                        <div class="col-xl-12 mb-4">
                           <asp:TextBox 
                               ID="txtCA" CssClass="form-control" 
                               Text="Cuenta de Ahorro"
                               runat="server" ReadOnly="True" 
                               style="text-align:center; font-weight:bold;">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-2 col-md-6 col-sm-12 mb-2">
                           <label>Saldo total (CA):</label>
                           <asp:TextBox 
                               ID="txtResumenTotalCA" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Número de operaciones:</label>
                           <asp:TextBox 
                               ID="txtResumenOperacionesCA" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-2 col-md-6 col-sm-12 mb-2">
                           <label>Interés mensual:</label>
                           <asp:TextBox 
                               ID="txtResumenInteresCA" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-2 col-md-6 col-sm-12 mb-2">
                           <label>Estado de cuenta:</label>
                           <asp:TextBox 
                               ID="txtResumenEstadoCA" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Gestión de estado:</label>
                            <asp:Button ID="btnActivarCA" 
                                runat="server" Text="Activar cuenta de ahorro"
                                CssClass="btn btn-outline-success btn-md btn-block" 
                                OnClientClick="return confirm
                                ('¿Está seguro que desea activar esta cuenta?');" 
                                Enabled="False" OnClick="btnActivarCA_Click"/>
                        </div>

                        <div class="col-xl-12 mb-2" style="padding-top:5px;">
                            <asp:Label 
                                ID="lblAvisoActivacionCA" runat="server" ForeColor="#ff7733">
                            </asp:Label>
                        </div>

                        <!-- - - - Estado - Depósito a plazo - - - -->
                        <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                        <div class="col-xl-12 mb-4">
                           <asp:TextBox 
                               ID="txtDP" CssClass="form-control" 
                               Text="Depósito a Plazo" runat="server" ReadOnly="True" 
                               style="text-align:center; font-weight:bold;">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Saldo total (DP):</label>
                           <asp:TextBox 
                               ID="txtResumenTotalDP" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Núm. de depósitos activos:</label>
                           <asp:TextBox 
                               ID="txtResumenDepositosActivosDP" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Núm. de operaciones:</label>
                           <asp:TextBox 
                               ID="txtResumenOperacionesDP" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                        <div class="col-xl-3 col-md-6 col-sm-12 mb-2">
                           <label>Estado de cuenta:</label>
                           <asp:TextBox 
                               ID="txtResumenEstadoDP" CssClass="form-control" 
                               runat="server" ReadOnly="True" style="text-align:center">
                           </asp:TextBox>
                        </div>

                    </div>   
                </div>
            </div>
        </div>

        <!-- - - - 3. Cuenta Corriente - - -->

        <div class="container" style="padding-top:25px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                    <b>Cuenta Corriente</b>
                </div>
                
                <div class="card-body">
                    
                    <div class="row" style="padding-left: 10px; padding-right: 10px;">
                        <div class="col-xl-3 col-md-3 col-sm-12 mb-2">
                           <label>Estado de cuenta:</label>
                           <asp:TextBox 
                               ID="txtEstadoCC" CssClass="form-control mb-3" 
                               runat="server" ReadOnly="True" style="text-align:center;">
                           </asp:TextBox>
                        </div>

                       <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                           <asp:GridView 
                               ID="dgvTransaccionesCC" 
                               runat="server" AutoGenerateColumns="False"
                               ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
                               CssClass="table table-bordered table-striped table-scrollbar">
                               <Columns>
                                   <asp:BoundField DataField="codigoTransaccionCC" HeaderText="Código">
                                   <HeaderStyle Width="50px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="tipoTransaccionCC" HeaderText="Tipo">
                                   <HeaderStyle Width="280px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="montoTransaccionCC" HeaderText="Monto">
                                   <HeaderStyle Width="150px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="fechaTransaccionCC" HeaderText="Fecha" >
                                   <HeaderStyle Width="220px" />
                                   </asp:BoundField>
                                </Columns>
                               <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:GridView>    
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <!-- - - - 4. Cuenta de Ahorro - - -->

        <div class="container" style="padding-top:25px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                    <b>Cuenta de Ahorro</b>
                </div>
                
                <div class="card-body">
                    <div class="row" style="padding-left: 10px; padding-right: 10px;">
                        <div class="col-xl-3 col-md-3 col-sm-12 mb-2">
                            
                            <label>Estado de cuenta:</label>
                            <asp:TextBox 
                                ID="txtEstadoCA" CssClass="form-control mb-2" 
                                runat="server" ReadOnly="True" style="text-align:center">
                            </asp:TextBox>

                            <label>Interés mensual:</label>
                            <asp:TextBox 
                                ID="txtInteresCA" CssClass="form-control mb-2" 
                                runat="server" ReadOnly="True" style="text-align:center">
                            </asp:TextBox>
                        </div>

                       <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                           <asp:GridView 
                               ID="dgvTransaccionesCA" runat="server" 
                               AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                               CssClass="table table-bordered table-striped table-scrollbar" 
                               HorizontalAlign="Center">
                               <Columns>
                                   <asp:BoundField DataField="codigoTransaccionCA" HeaderText="Código">
                                   <HeaderStyle Width="50px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="tipoTransaccionCA" HeaderText="Tipo">
                                   <HeaderStyle Width="280px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="montoTransaccionCA" HeaderText="Monto">
                                   <HeaderStyle Width="150px" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="fechaTransaccionCA" HeaderText="Fecha" >
                                   <HeaderStyle Width="200px" />
                                   </asp:BoundField>
                                </Columns>
                               <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:GridView>    
                        </div>
                    </div>   

                </div>
            </div>
        </div>

        <!-- - - - 5. Depósito a Plazo - - -->

        <div class="container" style="padding-top:25px; padding-bottom:20px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                    <b>Depósito a Plazo</b>
                </div>
                
                <div class="card-body">

                    <div class="row" style="padding-left: 10px; padding-right: 10px;">
                        <div class="col-xl-3 col-md-3 col-sm-12 mb-2">
                            <label>Estado de cuenta:</label>
                            <asp:TextBox 
                                ID="txtEstadoDP" CssClass="form-control mb-3" 
                                runat="server" ReadOnly="True" style="text-align:center">
                            </asp:TextBox>
                        </div>

                       <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                           <asp:GridView 
                               ID="dgvTransaccionesDP" runat="server" AutoGenerateColumns="False"
                               ShowHeaderWhenEmpty="True"
                               CssClass="table table-bordered table-striped table-scrollbar" 
                               HorizontalAlign="Center">
                               <Columns>
                                    <asp:BoundField DataField="codigoDeposito" HeaderText="Código">
                                    <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="montoDeposito" HeaderText="Monto">
                                    <HeaderStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="interesDeposito" HeaderText="Interés" >
                                    <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="mesesDeposito" HeaderText="Meses" >
                                    <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaInicioDeposito" HeaderText="Fecha de Transacción" >
                                    <HeaderStyle Width="240px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaFinDeposito" HeaderText="Fecha de Recepción" >
                                    <HeaderStyle Width="220px" />
                                    </asp:BoundField>
                                </Columns>
                               <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:GridView>    
                        </div>

                    </div>   
                </div>
            </div>
        </div>
    </form>
</body>
</html>
