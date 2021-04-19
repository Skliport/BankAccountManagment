using DPWALab2Periodo2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DPWALab2Periodo2
{
    public partial class User : System.Web.UI.Page
    {
        DataTable dtTransaccionesCC = new DataTable();
        DataTable dtTransaccionesCA = new DataTable();
        DataTable dtTransaccionesDP = new DataTable();
        BankAccount bankAccount; OpenAccount openAccount; Transaction transaction;

        protected void Page_Load(object sender, EventArgs e)
        {
            bankAccount = new BankAccount();
            openAccount = new OpenAccount();
            transaction = new Transaction();

            // Tabla Transacciones Cuenta Corriente.
            dtTransaccionesCC.Columns.AddRange(
                new DataColumn[4] {
                    new DataColumn("codigoTransaccionCC", typeof(string)),
                    new DataColumn("tipoTransaccionCC", typeof(string)),
                    new DataColumn("montoTransaccionCC", typeof(string)),
                    new DataColumn("fechaTransaccionCC", typeof(string))
                }
            );
            dgvTransaccionesCC.DataSource = dtTransaccionesCC;
            dgvTransaccionesCC.DataBind();

            // Tabla Transacciones Cuenta Ahorro.
            dtTransaccionesCA.Columns.AddRange(
                new DataColumn[4] {
                    new DataColumn("codigoTransaccionCA", typeof(string)),
                    new DataColumn("tipoTransaccionCA", typeof(string)),
                    new DataColumn("montoTransaccionCA", typeof(string)),
                    new DataColumn("fechaTransaccionCA", typeof(string))
                }
            );
            dgvTransaccionesCA.DataSource = dtTransaccionesCA;
            dgvTransaccionesCA.DataBind();

            // Tabla Transacciones Cuenta Depósito.
            dtTransaccionesDP.Columns.AddRange(
                new DataColumn[6] {
                    new DataColumn("codigoDeposito", typeof(string)),
                    new DataColumn("montoDeposito", typeof(string)),
                    new DataColumn("interesDeposito", typeof(string)),
                    new DataColumn("mesesDeposito", typeof(string)),
                    new DataColumn("fechaInicioDeposito", typeof(string)),
                    new DataColumn("fechaFinDeposito", typeof(string))
                }
            );
            dgvTransaccionesDP.DataSource = dtTransaccionesDP;
            dgvTransaccionesDP.DataBind();

            lblAvisoTransaccionCC.Text = ""; lblAvisoTransaccionCA.Text = "";
            lblAvisoTransaccionDP.Text = ""; lblAvisoAperturaCA.Text = "";
            lblAvisoIngresarCuenta.Text = "";
        }

        // 1. Iniciar sesión.
        protected void btnIngresarCuenta_Click(object sender, EventArgs e)
        {
            string accountNumber = txtNumeroCuenta.Text; 
            string password = txtPasswordCuenta.Text;

            if (bankAccount.AuthenticateUser(accountNumber, password))
            {
                plContenido.Visible = true; txtNumeroCuenta.Text = ""; txtPasswordCuenta.Text = "";
                openAccount.CheckDepositAccountRelease(accountNumber);
                openAccount.CheckSavingsAccountRelease();

                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                txtMontoCA.Text = ""; txtMontoCC.Text = ""; txtMontoDP.Text = "";
                txtMontoAperturaCA.Text = "";
                return;
            }

            lblAvisoIngresarCuenta.Text = "Número de cuenta o contraseña ingresada incorrecta.";
            plContenido.Visible = false; txtNumeroCuenta.Text = ""; txtPasswordCuenta.Text = "";
        }

        // 2. Abrir Cuenta Corriente.
        protected void btnAbrirCC_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            openAccount.UpdateAccountState(accountNumber, 0, 1); // Activando cuenta.
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);            
        }

        // 3. Abrir Cuenta de Ahorro.
        protected void btnAbrirCA_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoAperturaCA.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoAperturaCA.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Abono de por lo menos más de 1 USD.
            if (Convert.ToDouble(txtMontoAperturaCA.Text) < 1)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoAperturaCA.Text = "Ingrese un monto superior a 1 USD.";
                return;
            }

            // Activando cuenta.
            openAccount.UpdateAccountState(accountNumber, 1, 1);
            
            // Registrando primer abono de apertura.
            double amount = Convert.ToDouble(txtMontoAperturaCA.Text);
            transaction.RegisterTransactionCA(accountNumber, "Abono", amount);
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
        }

        // 4. Abrir Cuenta de Depósito.
        protected void btnAbrirDP_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            openAccount.UpdateAccountState(accountNumber, 2, 1); // Activando cuenta.
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
        }

        // 5. Cuenta Corriente - Abonar.
        protected void btnAbonarCC_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;

            // INICIO - - - Validaciones - - -

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoCC.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Abono de por lo menos más de 1 USD.
            if (Convert.ToDouble(txtMontoCC.Text) < 1)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "Ingrese un monto superior a 1 USD.";
                return;
            }

            // FIN - - - Validaciones - - -

            double amount = Convert.ToDouble(txtMontoCC.Text);

            transaction.RegisterTransactionCC(accountNumber, "Abono", amount);
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber); 
            txtMontoCC.Text = "";
        }

        // 6. Cuenta Corriente - Retirar.
        protected void btnRetirarCC_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;

            // INICIO - - - Validaciones - - -

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoCC.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Límite máximo de 400 USD por retiro.
            if (Convert.ToDouble(txtMontoCC.Text) > 400)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "Esta operación excede el límite de retiro por transacción (400 USD).";
                return;
            }

            // * Obtener balance actual.
            double currentBalance = 0;
            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                if (account.AccountType == 0) { currentBalance = account.Balance; }
            }

            // 3. El valor de retiro no puede ser mayor al saldo actual.
            if (Convert.ToDouble(txtMontoCC.Text) > currentBalance)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "El monto a retirar excede el saldo disponible.";
                return;
            }

            // 4. Límite máximo de 1000 USD en retiro por día.
            double totalThisDay = 0;

            foreach (var t in transaction.GetAccountTransactions(accountNumber, 0))
            {
                if (t.SubmitDate.Date == DateTime.Today && t.TransactionType == "Retiro") {totalThisDay += t.Amount;}
            }
            
            if ((totalThisDay + Convert.ToDouble(txtMontoCC.Text)) > 1000)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCC.Text = "Esta operación excede el límite de retiro por día (1000 USD)";
                return;
            }

            // 5. Saldo pendiente es inferior a 1 USD - Consultar si desea proceder y desactivar cuenta.
            if ((currentBalance - Convert.ToDouble(txtMontoCC.Text)) < 1)
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Confirm",
                    "<script type='text/javascript'>callConfirmCC();</script>");
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                return;
            }

            // FIN - - - Validaciones - - -

            double amount = Convert.ToDouble(txtMontoCC.Text);

            transaction.RegisterTransactionCC(accountNumber, "Retiro", amount);
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber); 
            txtMontoCC.Text = "";
        }

        // 7. Cuenta de Ahorro - Abonar.
        protected void btnAbonarCA_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;

            // INICIO - - - Validaciones - - -

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoCA.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Abonar por lo menos más de 1 USD.
            if (Convert.ToDouble(txtMontoCA.Text) < 1)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "Ingrese un monto superior a 1 USD.";
                return;
            }

            // FIN - - - Validaciones - - -

            double amount = Convert.ToDouble(txtMontoCA.Text);

            transaction.RegisterTransactionCA(accountNumber, "Abono", amount);
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
            txtMontoCA.Text = "";
        }

        // 8. Cuenta de Ahorro - Retirar.
        protected void btnRetirarCA_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;

            // INICIO - - - Validaciones - - -

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoCA.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Límite máximo de 400 USD por retiro.
            if (Convert.ToDouble(txtMontoCA.Text) > 400)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "Esta operación excede el límite de retiro por transacción (400 USD).";
                return;
            }

            // * Obteniendo balance actual.
            double currentBalance = 0;
            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                if (account.AccountType == 1) { currentBalance = account.Balance; }
            }

            // 3. El valor de retiro no puede ser mayor al saldo actual.
            if (Convert.ToDouble(txtMontoCA.Text) > currentBalance)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "El monto a retirar excede el saldo disponible.";
                return;
            }

            // 4. Límite máximo de 1000 USD en retiro por día.
            double totalThisDay = 0;

            foreach (var t in transaction.GetAccountTransactions(accountNumber, 1))
            {
                if (t.SubmitDate.Date == DateTime.Today && t.TransactionType == "Retiro") {totalThisDay += t.Amount;}
            }

            if ((totalThisDay + Convert.ToDouble(txtMontoCA.Text)) > 1000)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionCA.Text = "Esta operación excede el límite de retiro por día (1000 USD)";
                return;
            }

            // 5. Saldo pendiente es inferior a 1 USD - Consultar si desea proceder y desactivar cuenta.

            if ((currentBalance - Convert.ToDouble(txtMontoCA.Text)) < 1)
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Confirm",
                    "<script type='text/javascript'>callConfirmCA();</script>");
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                return;
            }

            // FIN - - - Validaciones - - -

            double amount = Convert.ToDouble(txtMontoCA.Text);

            transaction.RegisterTransactionCA(accountNumber, "Retiro", amount);
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
            txtMontoCA.Text = "";
        }

        // 9. Cuenta de Depósito - Realizar un depósito.
        protected void btnDepositarDP_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            
            // INICIO - - - Validaciones - - -

            // 1. Decimales o enteros positivos únicamente.
            if (!(Regex.Match(txtMontoDP.Text.ToString(),
                @"^(0*[1-9][0-9]*(\.[0-9]*)?|0*\.[0-9]*[1-9][0-9]*)$").Success))
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionDP.Text = "Ingrese un monto válido.";
                return;
            }

            // 2. Abono de por lo menos más de 10 USD.
            if (Convert.ToDouble(txtMontoDP.Text) < 10)
            {
                ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                lblAvisoTransaccionDP.Text = "Ingrese un monto superior a 10 USD.";
                return;
            }

            // FIN - - - Validaciones - - -

            double amount = Convert.ToDouble(txtMontoDP.Text);
            double interestRate = Convert.ToDouble(ddlMesesDP.SelectedItem.Value.Substring(0,4))/ 100;
            int months = Convert.ToInt32(ddlMesesDP.SelectedItem.Text);

            transaction.RegisterTransactionCD(accountNumber, months, amount, interestRate);
            openAccount.CheckDepositAccountRelease(accountNumber);

            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);      
            this.ddlMesesDP.SelectedIndex = 0; txtMontoDP.Text = "";
        }

        // 10. Solicitar reactivar Cuenta Corriente.
        protected void btnSolicitarReactivacionCC_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            openAccount.UpdateAccountState(accountNumber, 0, 2);
            btnSolicitarReactivacionCC.Enabled = false;
            lblAvisoReactivarCC.Text = "Una solicitud de reactivación se encuentra pendiente.";
        }

        // 11. Solicitar reactivar Cuenta de Ahorro.
        protected void btnSolicitarReactivacionCA_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            openAccount.UpdateAccountState(accountNumber, 1, 2);
            btnSolicitarReactivacionCA.Enabled = false;
            lblAvisoReactivarCA.Text = "Una solicitud de reactivación se encuentra pendiente.";
        }

        // Mostrar estado de cuenta de banco.
        public void ShowBankAccountData(string accountNumber)
        {
            bankAccount = bankAccount.GetBankAccount(accountNumber);
            openAccount.CheckDepositAccountRelease(accountNumber);

            txtResumenNombre.Text = bankAccount.Name;
            txtResumenNumero.Text = bankAccount.Number;
            txtResumenSaldoTotal.Text = $"$ {bankAccount.Balance:0.00}";
            txtResumenFechaApertura.Text = bankAccount.OpeningDate.ToString();

            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                string accountStatus = account.Status == 1 ? "Activa" : "Inactiva";

                // 0. Cuenta Corriente.
                if (account.AccountType == 0)
                {
                    txtResumenEstadoCC.Text = accountStatus;
                    txtEstadoCC.Text = accountStatus;
                    txtResumenOperacionesCC.Text = account.Movements.ToString();
                    txtResumenTotalCC.Text = $"$ {account.Balance:0.00}";

                    if (account.Status == 1)
                    {
                        txtResumenEstadoCC.ForeColor = Color.MediumSeaGreen;
                        txtEstadoCC.ForeColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        txtResumenEstadoCC.ForeColor = Color.Crimson;
                        txtEstadoCC.ForeColor = Color.Crimson;
                    }
                }

                // 1. Cuenta de Ahorro.
                if (account.AccountType == 1)
                {
                    txtEstadoCA.Text = accountStatus;
                    txtResumenEstadoCA.Text = accountStatus;
                    txtResumenInteresCA.Text = $"{account.InterestRate * 100:0.00} %";
                    txtResumenOperacionesCA.Text = account.Movements.ToString();
                    txtResumenTotalCA.Text = $"$ {account.Balance:0.00}";
                    txtInteresCA.Text = $"{account.InterestRate * 100:0.00} %";

                    if (account.Status == 1)
                    {
                        txtResumenEstadoCA.ForeColor = Color.MediumSeaGreen;
                        txtEstadoCA.ForeColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        txtResumenEstadoCA.ForeColor = Color.Crimson;
                        txtEstadoCA.ForeColor = Color.Crimson;
                    }
                }

                // 2. Cuenta de depósitos.
                if (account.AccountType == 2)
                {
                    txtEstadoDP.Text = accountStatus;
                    txtResumenDepositosActivosDP.Text = account.ActiveDeposits.ToString();
                    txtResumenEstadoDP.Text = accountStatus;
                    txtResumenOperacionesDP.Text = account.Movements.ToString();
                    txtResumenTotalDP.Text = $"$ {account.Balance:0.00}";

                    if (account.Status == 1)
                    {
                        txtResumenEstadoDP.ForeColor = Color.MediumSeaGreen;
                        txtEstadoDP.ForeColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        txtResumenEstadoDP.ForeColor = Color.Crimson;
                        txtEstadoDP.ForeColor = Color.Crimson;
                    }
                }
            }
            ShowTransactions(accountNumber); lblAvisoIngresarCuenta.Text = "";
        }

        // Mostrar transacciones de cuentas abiertas para cuenta bancaria.
        public void ShowTransactions(string accountNumber)
        {
            // 0. Cuenta Corriente
            dtTransaccionesCC.Clear();
            foreach (var transaction in transaction.GetAccountTransactions(accountNumber, 0))
            {
                dtTransaccionesCC.Rows.Add(
                    transaction.TransactionId.ToString(),
                    transaction.TransactionType,
                    $"{transaction.Amount:0.00} $",
                    transaction.SubmitDate.ToString());
            }
            dgvTransaccionesCC.DataSource = dtTransaccionesCC;
            dgvTransaccionesCC.DataBind();

            // 1. Cuenta de Ahorro.

            dtTransaccionesCA.Clear();
            foreach (var transaction in transaction.GetAccountTransactions(accountNumber, 1))
            {
                dtTransaccionesCA.Rows.Add(
                    transaction.TransactionId.ToString(),
                    transaction.TransactionType,
                    $"$ {transaction.Amount:0.00}",
                    transaction.SubmitDate.ToString());
            }
            dgvTransaccionesCA.DataSource = dtTransaccionesCA;
            dgvTransaccionesCA.DataBind();

            // 2. Cuenta de Depósito

            dtTransaccionesDP.Clear();
            foreach (var transaction in transaction.GetAccountTransactions(accountNumber, 2))
            {
                dtTransaccionesDP.Rows.Add(
                    transaction.TransactionId.ToString(),
                    $"$ {transaction.Amount:0.00}",
                    $"{transaction.InterestRate * 100:0.00} %",
                    transaction.Months.ToString(),
                    transaction.SubmitDate.ToString(),
                    transaction.ReleaseDate.ToShortDateString());
            }
            dgvTransaccionesDP.DataSource = dtTransaccionesDP;
            dgvTransaccionesDP.DataBind();
        }

        // Revisar estado de cuentas abiertas.
        public void CheckAcountStatus(string accountNumber)
        {
            plTransaccionesCC.Visible = true; btnAbrirCC.Visible = false;
            plTransaccionesCA.Visible = true; plAbrirCA.Visible = false;
            plTransaccionesDP.Visible = true; btnAbrirDP.Visible = false;
            txtMontoAperturaCA.Enabled = false;

            // 1. Revisar si las cuentas se encuentran abiertas o requieren apertura.
            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                if (account.Status == 0 && account.Movements == 0)
                {
                    // Cuenta Corriente requiere de apertura.
                    if (account.AccountType == 0)
                    {
                        plTransaccionesCC.Visible = false; btnAbrirCC.Visible = true;
                    }

                    // Cuenta de Ahorro requiere de apertura.
                    if (account.AccountType == 1)
                    {
                        plTransaccionesCA.Visible = false; plAbrirCA.Visible = true;
                        txtMontoAperturaCA.Enabled = true;
                    }

                    // Cuenta de Depósito requiere de apertura.
                    if (account.AccountType == 2)
                    {
                        plTransaccionesDP.Visible = false; btnAbrirDP.Visible = true;
                    }
                }
            }

            // 2. Revisar si las cuentas aceptan solicitudes de reactivación.
            btnSolicitarReactivacionCC.Enabled = false; btnSolicitarReactivacionCA.Enabled = false;
            lblAvisoReactivarCC.Text = ""; lblAvisoReactivarCA.Text = "";
   
            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                // Verificar si existen solicitudes de reactivación pendientes.
                if (account.Status == 2)
                {
                    if (account.AccountType == 0)
                    { lblAvisoReactivarCC.Text = "Una solicitud de reactivación se encuentra pendiente."; }

                    else if (account.AccountType == 1)
                    { lblAvisoReactivarCA.Text = "Una solicitud de reactivación se encuentra pendiente."; }
                }

                // Verificar si es posible habilitar solicitudes de reactivación.
                else if (account.Status == 0 && account.Movements > 0)
                {
                    // Habilitar reactivación de Cuenta Corriente.
                    if (account.AccountType == 0)
                    { btnSolicitarReactivacionCC.Enabled = true; }

                    // Habilitar reactivación de Cuenta de Ahorro.
                    else if (account.AccountType == 1)
                    { btnSolicitarReactivacionCA.Enabled = true; }
                }
            }

            // 3. Verificar si las cuentas están activas o inactivas.
            btnAbonarCA.Enabled = true; btnRetirarCA.Enabled = true;
            txtMontoCA.Enabled = true; btnAbonarCC.Enabled = true; 
            btnRetirarCC.Enabled = true; txtMontoCC.Enabled = true;

            foreach (var account in openAccount.GetOpenAccountsList(accountNumber))
            {
                if (account.Status == 0 || account.Status == 2)
                {
                    if (account.AccountType == 0)
                    { btnAbonarCC.Enabled = false; btnRetirarCC.Enabled = false; txtMontoCC.Enabled = false;}

                    if (account.AccountType == 1)
                    { btnAbonarCA.Enabled = false; btnRetirarCA.Enabled = false; txtMontoCA.Enabled = false;}
                }
            }
        }

        // Cuenta corriente - Saldo pendiente inferior a 1USD - Desactivar cuenta.
        protected void btnRetirarYDesactivarCC_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            double amount = Convert.ToDouble(txtMontoCC.Text);

            transaction.RegisterTransactionCC(accountNumber, "Retiro", amount);
            openAccount.UpdateAccountState(accountNumber, 0, 0); // Desactivar cuenta.

            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber); txtMontoCC.Text = "";
        }

        // Cuenta de ahorro - Saldo pendiente inferior a 1USD - Desactivar cuenta.
        protected void btnRetirarYDesactivarCA_Click(object sender, EventArgs e)
        {
            string accountNumber = txtResumenNumero.Text;
            double amount = Convert.ToDouble(txtMontoCA.Text);

            transaction.RegisterTransactionCA(accountNumber, "Retiro", amount);
            openAccount.UpdateAccountState(accountNumber, 1, 0);  // Desactivar cuenta.
            
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber); txtMontoCA.Text = "";
        }
    }
}