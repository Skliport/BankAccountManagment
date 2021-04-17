<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="DPWALab2Periodo2.User" %>
<link href="Content/bootstrap.css" rel="stylesheet"> 
<link href="Content/bootstrap.min.css" rel="stylesheet"> 
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.3/jquery.min.js"></script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DPWA - User</title>
</head>
<body>
    <form id="form1" runat="server">
        
        <!-- - - - 1. Ingreso a cuenta - - - -->
        <div class="container" style="padding-top:25px;">
            <div class="card border-dark shadow">
                <div class="card-header">
                    <b>Inicio de Sesión en cuenta de Banco</b>
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

                        <div class="col-xl-3 col-md-2 col-sm-12 mb-2">
                           <label>Password:</label>
                           <asp:TextBox 
                               ID="txtPasswordCuenta" CssClass="form-control" 
                               runat="server" MaxLength="4" TextMode="Password" style="text-align:center"> 
                           </asp:TextBox>
                        </div>
                        
                        <div class="col-xl-2 col-md-4 col-sm-12 mb-2"">
                           <label>&#8203</label>
                           <asp:Button ID="btnIngresarCuenta" 
                               CssClass="btn btn-outline-success btn-md btn-block" runat="server" 
                               Text="Iniciar sesión" OnClick="btnIngresarCuenta_Click"/> 
                        </div>
                        
                        <div class="col-xl-12 col-md-6 col-sm-12 mb-2" style="padding-top:14px;">
                            <asp:Label 
                                ID="lblAvisoIngresarCuenta" 
                                runat="server"
                                ForeColor="#EC1354">
                            </asp:Label>
                        </div>
                                     
                    </div>   
                </div>
            </div>
        </div>

        <asp:Panel ID="plContenido" runat="server" Visible="False">
            <!-- - - - 2. Estado de cuenta - - - -->
            <div class="container" style="padding-top:25px;">
                <div class="card border-dark shadow">
                    <div class="card-header">
                       <b>Estado de cuenta</b>
                    </div>
                
                    <div class="card-body">
                        <div class="row" style="padding-left: 10px; padding-right: 10px;">

                            <!-- - - - Estado - Cuenta de banco - - - -->

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                                <label>Número de cuenta:</label>
                                <asp:TextBox 
                                    ID="txtResumenNumero" CssClass="form-control" 
                                    runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Nombre:</label>
                               <asp:TextBox 
                                   ID="txtResumenNombre" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Fecha de apertura:</label>
                               <asp:TextBox 
                                   ID="txtResumenFechaApertura" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Saldo total:</label>
                               <asp:TextBox 
                                   ID="txtResumenSaldoTotal" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <!-- - - - Estado - Cuenta corriente - - - -->
                            <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                            <div class="col-xl-12 mb-4">
                               <asp:TextBox 
                                   ID="txtResumenCC" CssClass="form-control" 
                                   Text="Cuenta Corriente" runat="server" ReadOnly="True" 
                                   style="text-align:center; color:#75a187; font-weight:bold;"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Saldo total (CC):</label>
                               <asp:TextBox 
                                   ID="txtResumenTotalCC" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Número de operaciones:</label>
                               <asp:TextBox 
                                   ID="txtResumenOperacionesCC" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Estado de cuenta:</label>
                               <asp:TextBox 
                                   ID="txtResumenEstadoCC" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                                <label>Gestión de estado:</label>
                                <asp:Button ID="btnSolicitarReactivacionCC" 
                                    runat="server" Text="Solicitar reactivación"
                                    CssClass="btn btn-outline-success btn-md btn-block" 
                                    OnClientClick="return confirm
                                    ('¿Está seguro que desea solicitar reactivar esta cuenta?');" 
                                    Enabled="False" OnClick="btnSolicitarReactivacionCC_Click"/>
                            </div>

                            <div class="col-xl-12 mb-2" style="padding-top:5px;">
                                <asp:Label 
                                    ID="lblAvisoReactivarCC" runat="server" ForeColor="#ff7733">
                                </asp:Label>
                            </div>

                            <!-- - - - Estado - Cuenta de ahorro - - - -->
                            <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                            <div class="col-xl-12 mb-4">
                               <asp:TextBox 
                                   ID="txtCA" CssClass="form-control" 
                                   Text="Cuenta de Ahorro"
                                   runat="server" ReadOnly="True" 
                                   style="text-align:center; color:#75a187; font-weight:bold;"> </asp:TextBox>
                            </div>

                            <div class="col-xl-2 col-md-6 col-sm-12 mb-3">
                               <label>Saldo total (CA):</label>
                               <asp:TextBox 
                                   ID="txtResumenTotalCA" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Número de operaciones:</label>
                               <asp:TextBox 
                                   ID="txtResumenOperacionesCA" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-2 col-md-6 col-sm-12 mb-3">
                               <label>Interés mensual:</label>
                               <asp:TextBox 
                                   ID="txtResumenInteresCA" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-2 col-md-6 col-sm-12 mb-3">
                               <label>Estado de cuenta:</label>
                               <asp:TextBox 
                                   ID="txtResumenEstadoCA" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                                <label>Gestión de estado:</label>
                                <asp:Button ID="btnSolicitarReactivacionCA" 
                                    runat="server" Text="Solicitar reactivación"
                                    CssClass="btn btn-outline-success btn-md btn-block" 
                                    OnClientClick="return confirm
                                    ('¿Está seguro que desea solicitar reactivar esta cuenta?');" 
                                    Enabled="False" OnClick="btnSolicitarReactivacionCA_Click"/>
                            </div>

                            <div class="col-xl-12 mb-2" style="padding-top:5px;">
                                <asp:Label 
                                    ID="lblAvisoReactivarCA" runat="server" ForeColor="#ff7733">
                                </asp:Label>
                            </div>

                            <!-- - - - Estado - Depósito a plazo - - - -->
                            <div class="col-xl-12"><hr style="border: 1px solid #66bc86;"/></div>
                            <div class="col-xl-12 mb-4">
                               <asp:TextBox 
                                   ID="txtDP" CssClass="form-control" 
                                   Text="Depósito a Plazo" runat="server" ReadOnly="True" 
                                   style="text-align:center; color:#75a187; font-weight:bold;"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Saldo total (DP):</label>
                               <asp:TextBox 
                                   ID="txtResumenTotalDP" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Núm. de depósitos activos:</label>
                               <asp:TextBox 
                                   ID="txtResumenDepositosActivosDP" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Núm. de operaciones:</label>
                               <asp:TextBox 
                                   ID="txtResumenOperacionesDP" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
                            </div>

                            <div class="col-xl-3 col-md-6 col-sm-12 mb-3">
                               <label>Estado de cuenta:</label>
                               <asp:TextBox 
                                   ID="txtResumenEstadoDP" CssClass="form-control" 
                                   runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>
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
                                   runat="server" ReadOnly="True" style="text-align:center;"> </asp:TextBox>

                                <!-- - - - Panel - Abrir cuenta corriente - - -->
                                <asp:Button ID="btnAbrirCC" 
                                runat="server" Text="Abrir cuenta"
                                CssClass="btn btn-outline-success btn-md btn-block" 
                                OnClientClick="return confirm
                                ('¿Está seguro que desea abrir una Cuenta Corriente?');" 
                                OnClick="btnAbrirCC_Click"/>

                                <!-- - - - Panel - Registro de transacciones - - -->
                                <asp:Panel ID="plTransaccionesCC" runat="server" Visible="False">
                                    <label>Monto:</label>
                                    <asp:TextBox 
                                        ID="txtMontoCC" CssClass="form-control" 
                                        runat="server" MaxLength="10"
                                        style="text-align:center"> </asp:TextBox>

                                    <div style="padding-top:10px;">
                                        <asp:Button ID="btnAbonarCC" 
                                        CssClass="btn btn-outline-success btn-md btn-block"
                                        runat="server" Text="Realizar abono" 
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea abonar este monto?');" OnClick="btnAbonarCC_Click"/> 
                                
                                        <asp:Button ID="btnRetirarCC" 
                                        runat="server" Text="Realizar retiro"
                                        CssClass="btn btn-outline-danger btn-md btn-block" 
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea retirar este monto?');" OnClick="btnRetirarCC_Click"/>
                                    </div>

                                    <div style="padding-top:5px;">
                                        <asp:Label 
                                            ID="lblAvisoTransaccionCC" 
                                            runat="server" ForeColor="#EC1354">
                                        </asp:Label>
                                    </div> 
                                </asp:Panel>
                            </div>

                           <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                               <asp:GridView 
                                   ID="dgvTransaccionesCC" 
                                   runat="server" AutoGenerateColumns="False"
                                   ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
                                   CssClass="table table-bordered table-striped table-scrollbar">
                                   <Columns>
                                       <asp:BoundField DataField="codigoTransaccionCC" HeaderText="Código">
                                       <HeaderStyle Width="80px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="tipoTransaccionCC" HeaderText="Tipo">
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="montoTransaccionCC" HeaderText="Monto">
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="fechaTransaccionCC" HeaderText="Fecha" >
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
                                    runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>

                                <label>Interés mensual:</label>
                                <asp:TextBox 
                                    ID="txtInteresCA" CssClass="form-control mb-2" 
                                    runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>

                                <!-- - - - Panel - Abrir cuenta de ahorro - - -->
                                <asp:Panel ID="plAbrirCA" runat="server">

                                    <label>Monto de apertura:</label>
                                    <asp:TextBox 
                                        ID="txtMontoAperturaCA" CssClass="form-control mb-3" 
                                        runat="server" Enabled="False" style="text-align:center" 
                                        MaxLength="10">
                                    </asp:TextBox>

                                    <asp:Button ID="btnAbrirCA" 
                                        runat="server" Text="Abrir cuenta"
                                        CssClass="btn btn-outline-success btn-md btn-block mb-2" 
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea abrir una Cuenta de Ahorro?');" 
                                        Enabled="True" OnClick="btnAbrirCA_Click"/>

                                    <asp:Label 
                                        ID="lblAvisoAperturaCA" runat="server" ForeColor="#EC1354">
                                    </asp:Label>
                                </asp:Panel>

                                <!-- - - - Panel - Registro de transacciones - - -->
                                <asp:Panel ID="plTransaccionesCA" runat="server" Visible="False">
                                    <label>Monto:</label>
                                    <asp:TextBox 
                                        ID="txtMontoCA" CssClass="form-control" runat="server" 
                                        MaxLength="10" style="text-align:center"> </asp:TextBox>

                                    <div style="padding-top:10px;">
                                        <asp:Button ID="btnAbonarCA" 
                                        CssClass="btn btn-outline-success btn-md btn-block"
                                        runat="server" Text="Realizar abono" 
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea abonar este monto?');" OnClick="btnAbonarCA_Click"/> 
                                
                                        <asp:Button ID="btnRetirarCA" 
                                        runat="server" Text="Realizar retiro"
                                        CssClass="btn btn-outline-danger btn-md btn-block" 
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea retirar este monto?');" OnClick="btnRetirarCA_Click"/>
                                    </div>

                                    <div style="padding-top:5px;">
                                        <asp:Label 
                                            ID="lblAvisoTransaccionCA" runat="server" ForeColor="#EC1354">
                                        </asp:Label>
                                    </div>
                                </asp:Panel>
                            </div>

                           <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                               <asp:GridView 
                                   ID="dgvTransaccionesCA" runat="server" 
                                   AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                   CssClass="table table-bordered table-striped table-scrollbar" 
                                   HorizontalAlign="Center">
                                   <Columns>
                                       <asp:BoundField DataField="codigoTransaccionCA" HeaderText="Código">
                                       <HeaderStyle Width="80px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="tipoTransaccionCA" HeaderText="Tipo">
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="montoTransaccionCA" HeaderText="Monto">
                                       <HeaderStyle Width="200px" />
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

            <div class="container" style="padding-top:25px; padding-bottom: 20px;">
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
                                    runat="server" ReadOnly="True" style="text-align:center"> </asp:TextBox>

                                <asp:Button ID="btnAbrirDP" 
                                    runat="server" Text="Abrir cuenta"
                                    CssClass="btn btn-outline-success btn-md btn-block mb-2" 
                                    OnClientClick="return confirm
                                    ('¿Está seguro que desea abrir una cuenta para Depósito a Plazo?');" 
                                    Enabled="True" OnClick="btnAbrirDP_Click"/>

                                <!-- - - - Panel - Registro de transacciones - - -->
                                <asp:Panel ID="plTransaccionesDP" runat="server" Visible="False">
                                    <label>Monto:</label>
                                    <asp:TextBox 
                                        ID="txtMontoDP" CssClass="form-control mb-2" 
                                        runat="server" MaxLength="10"
                                        style="text-align:center"> </asp:TextBox>

                                    <label>Interés mensual:</label>
                                    <asp:TextBox 
                                        ID="txtInteresDP" CssClass="form-control mb-2" runat="server"
                                        ReadOnly="True" style="text-align:center"> </asp:TextBox>

                                    <label>Cantidad de meses:</label>
                                    <asp:DropDownList 
                                        ID="ddlMesesDP" CssClass="form-control mb-2" runat="server" 
                                        style="text-align:center">
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>

                                    <label>Fecha de recepción:</label>
                                    <asp:TextBox 
                                        ID="txtFechaFinDP" CssClass="form-control mb-2" 
                                        runat="server" ReadOnly="True"
                                        style="text-align:center"> </asp:TextBox>

                                    <div style="padding-top:10px;">
                                        <asp:Button ID="btnDepositarDP" 
                                        CssClass="btn btn-outline-success btn-md btn-block"
                                        runat="server" Text="Realizar depósito"
                                        OnClientClick="return confirm
                                        ('¿Está seguro que desea depositar este monto?');" OnClick="btnDepositarDP_Click"/> 
                                    </div>

                                    <div style="padding-top:5px;">
                                        <asp:Label 
                                            ID="lblAvisoTransaccionDP" runat="server" ForeColor="#EC1354">
                                        </asp:Label>
                                    </div>   
                                </asp:Panel>
                            </div>

                           <div class="col-xl-9 col-md-9 col-sm-12 mb-2">
                               <asp:GridView 
                                   ID="dgvTransaccionesDP" runat="server" AutoGenerateColumns="False"
                                   ShowHeaderWhenEmpty="True"
                                   CssClass="table table-bordered table-striped table-scrollbar" 
                                   HorizontalAlign="Center">
                                   <Columns>
                                       <asp:BoundField DataField="codigoDeposito" HeaderText="Código">
                                       <HeaderStyle Width="60px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="montoDeposito" HeaderText="Monto">
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="interesDeposito" HeaderText="Interés mensual" >
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="mesesDeposito" HeaderText="Meses" >
                                       <HeaderStyle Width="80px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="fechaInicioDeposito" HeaderText="Inicio" >
                                       <HeaderStyle Width="200px" />
                                       </asp:BoundField>
                                       <asp:BoundField DataField="fechaFinDeposito" HeaderText="Fin" >
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
        </asp:Panel>

    </form>
</body>
</html>
