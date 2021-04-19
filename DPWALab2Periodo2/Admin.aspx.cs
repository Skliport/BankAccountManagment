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
    public partial class Admin : System.Web.UI.Page
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

            // Tabla Transacciones Cuenta de Ahorro.
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

            // Tabla Transacciones Cuenta de Depósitos.
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

            if (!IsPostBack){ShowAccountNumbers();}
        }

        // 1. Registrar una cuenta de banco.
        protected void btnRegistrarCuenta_Click(object sender, EventArgs e)
        {
            // Número de cuenta - Formato.
            if (!(Regex.Match(txtNumeroCuenta.Text, @"^(([0-9]{4})-){3}([0-9]{4})$").Success))
            {lblAvisoRegistrarCuenta.Text = "Ingrese un formato de número de cuenta válido."; return;}

            // Número de cuenta - Unique.
            foreach (var account in bankAccount.GetBankAccountsList())
            {
                if (txtNumeroCuenta.Text == account.Number)
                {
                    lblAvisoRegistrarCuenta.Text = "Este número de cuenta se encuentra registrado.";
                    txtNombreCuenta.Text = ""; txtPasswordCuenta.Text = "";
                    return;
                }
            }

            // Nombre - Formato.
            if (!(Regex.Match(txtNombreCuenta.Text, @"^([A-z]\s?)+$").Success))
            {lblAvisoRegistrarCuenta.Text = "Ingrese un nombre válido."; return; }

            // Password - Formato.
            else if (!(Regex.Match(txtPasswordCuenta.Text, @"^\d{4}$").Success))
            {lblAvisoRegistrarCuenta.Text = "Ingrese una contraseña válida."; return; }

            bankAccount.RegisterBankAccount(txtNumeroCuenta.Text,
            txtNombreCuenta.Text, txtPasswordCuenta.Text);
            Response.Redirect(Request.RawUrl);       
        }

        // 2. Reactivar una Cuenta Corriente con solicitud.
        protected void btnActivarCC_Click(object sender, EventArgs e)
        {
            lblAvisoActivacionCC.Text = ""; lblAvisoRegistrarCuenta.Text = ""; 
            btnActivarCC.Enabled = false;
            openAccount.UpdateAccountState(ddlCuentaDeBanco.Text.Substring(0, 19), 0, 1);
            btnSeleccionCuenta_Click(null, null);
        }

        // 3. Reactivar una Cuenta de Ahorro con solicitud.
        protected void btnActivarCA_Click(object sender, EventArgs e)
        {
            lblAvisoActivacionCA.Text = ""; lblAvisoRegistrarCuenta.Text = "";
            btnActivarCA.Enabled = false;
            openAccount.UpdateAccountState(ddlCuentaDeBanco.Text.Substring(0, 19), 1, 1);
            btnSeleccionCuenta_Click(null, null);
        }

        // 4. Seleccionar una cuenta y mostrar su contenido.
        protected void btnSeleccionCuenta_Click(object sender, EventArgs e)
        {
            string accountNumber = ddlCuentaDeBanco.Text.Substring(0, 19);
            openAccount.CheckSavingsAccountRelease();

            openAccount.CheckDepositAccountRelease(accountNumber);
            bankAccount = bankAccount.GetBankAccount(accountNumber);
            
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
                    {   txtResumenEstadoCC.ForeColor = Color.MediumSeaGreen;
                        txtEstadoCC.ForeColor = Color.MediumSeaGreen; }
                    else
                    {   txtResumenEstadoCC.ForeColor = Color.Crimson;
                        txtEstadoCC.ForeColor = Color.Crimson; }

                    // Habilitando reactivación para cuenta corriente.
                    if (account.Status == 2)
                    {
                        lblAvisoActivacionCC.Text = "AVISO: Esta cuenta dispone una " +
                            "solicitud de reactivación.";
                        btnActivarCC.Enabled = true;
                    }
                    else {lblAvisoActivacionCC.Text = ""; btnActivarCC.Enabled = false;}
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
                    {   txtResumenEstadoCA.ForeColor = Color.MediumSeaGreen;
                        txtEstadoCA.ForeColor = Color.MediumSeaGreen; }
                    else
                    {   txtResumenEstadoCA.ForeColor = Color.Crimson;
                        txtEstadoCA.ForeColor = Color.Crimson; }

                    // Habilitando reactivación para cuenta de ahorro.
                    if (account.Status == 2)
                    {
                        lblAvisoActivacionCA.Text = "AVISO: Esta cuenta dispone una " +
                            "solicitud de reactivación.";
                        btnActivarCA.Enabled = true;
                    }
                    else {lblAvisoActivacionCA.Text = ""; btnActivarCA.Enabled = false;}
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
                    {   txtResumenEstadoDP.ForeColor = Color.MediumSeaGreen;
                        txtEstadoDP.ForeColor = Color.MediumSeaGreen; }
                    else
                    {   txtResumenEstadoDP.ForeColor = Color.Crimson;
                        txtEstadoDP.ForeColor = Color.Crimson; }
                }
            }
            ShowTransactions(accountNumber); lblAvisoRegistrarCuenta.Text = "";
        }

        // Mostrar transacciones para CC, CA, CD.
        public void ShowTransactions(string accountNumber)
        {
            // 0. Cuenta Corriente
            dtTransaccionesCC.Clear();
            foreach (var transaction in transaction.GetAccountTransactions(accountNumber, 0))
            {
                dtTransaccionesCC.Rows.Add(
                    transaction.TransactionId.ToString(),
                    transaction.TransactionType.ToString(),
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
                    transaction.TransactionType.ToString(),
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

        // Mostrar números de cuenta.
        public void ShowAccountNumbers()
        {
            try
            {
                ddlCuentaDeBanco.Items.Clear();
                foreach (var account in bankAccount.GetBankAccountsList())
                {
                    ddlCuentaDeBanco.Items.Add($"{account.Number} ({account.Name})");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
    }
}