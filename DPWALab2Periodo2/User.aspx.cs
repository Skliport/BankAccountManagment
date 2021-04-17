using DPWALab2Periodo2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        }

        // 1. Iniciar sesión.
        protected void btnIngresarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                string accountNumber = txtNumeroCuenta.Text; 
                string password = txtPasswordCuenta.Text;

                if (bankAccount.AuthenticateUser(accountNumber, password))
                {
                    plContenido.Visible = true; txtNumeroCuenta.Text = ""; txtPasswordCuenta.Text = "";
                    ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);
                    return;
                }

                lblAvisoIngresarCuenta.Text = "Número de cuenta o contraseña ingresada incorrecta.";
                plContenido.Visible = false; txtNumeroCuenta.Text = ""; txtPasswordCuenta.Text = "";

            } catch (Exception ex) { Debug.WriteLine(ex); }
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
            openAccount.UpdateAccountState(accountNumber, 1, 1); // Activando cuenta.
            ShowBankAccountData(accountNumber); CheckAcountStatus(accountNumber);





            /* REGISTRAR PRIMER ABONO AQUÍ */






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

        }

        // 6. Cuenta Corriente - Retirar.
        protected void btnRetirarCC_Click(object sender, EventArgs e)
        {

        }

        // 7. Cuenta de Ahorro - Abonar.
        protected void btnAbonarCA_Click(object sender, EventArgs e)
        {

        }

        // 8. Cuenta de Ahorro - Retirar.
        protected void btnRetirarCA_Click(object sender, EventArgs e)
        {

        }

        // 9. Cuenta de Depósito - Realizar un depósito.
        protected void btnDepositarDP_Click(object sender, EventArgs e)
        {

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
            try
            {   bankAccount = bankAccount.GetBankAccount(accountNumber);

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
                        txtResumenInteresCA.Text = $"{account.InterestRate * 100} %";
                        txtResumenOperacionesCA.Text = account.Movements.ToString();
                        txtResumenTotalCA.Text = $"$ {account.Balance:0.00}";
                        txtInteresCA.Text = $"{account.InterestRate * 100} %";

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

            } catch (Exception ex) { Debug.WriteLine(ex); }
        }

        // Mostrar transacciones de cuentas abiertas para cuenta bancaria.
        public void ShowTransactions(string accountNumber)
        {
            try
            {   // 0. Cuenta Corriente
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
                        $"{transaction.InterestRate * 100} %",
                        transaction.Months.ToString(),
                        transaction.SubmitDate.ToString(),
                        transaction.ReleaseDate.ToString());
                }
                dgvTransaccionesDP.DataSource = dtTransaccionesDP;
                dgvTransaccionesDP.DataBind();
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
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
        }

    }
}