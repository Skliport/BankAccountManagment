using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DPWALab2Periodo2.Models
{
    public class OpenAccount
    {
        private int openAccountId, bankAccountId, accountType, status, movements, activeDeposits;
        private double balance, interestRate;
        private Connection connection; SqlDataReader sqlDataReader;
        BankAccount bankAccount; Transaction transaction;
      
        public OpenAccount()
        {         
        }

        // Las cuentas abiertas son 3: Cuenta corriente, Cuenta de ahorro y Cuenta de depósito.
        public OpenAccount(int bankAccountId, int openAccountId, int accountType, int status, int movements, 
            int activeDeposits, double balance, double interestRate)
        {
            this.bankAccountId = bankAccountId;     // Id de registro en transaction.
            this.openAccountId = openAccountId;     // Id de registro en bank_account.
            this.accountType = accountType;         // 0 Corriente, 1 Ahorros, 2 Depósito.
            this.status = status;                   // 0 Inactivo, 1 Activo, 2 Inactivo pendiente de activar.
            this.movements = movements;             // Num. de transacciones/operaciones.
            this.activeDeposits = activeDeposits;   // Depósitos activos.
            this.balance = balance;                 // Saldo total en cuenta abierta.
            this.interestRate = interestRate;       // Interés mensual.
        }

        public int BankAccountId { get => bankAccountId; set => bankAccountId = value; }
        public int OpenAccountId { get => openAccountId; set => openAccountId = value; }
        public int AccountType { get => accountType; set => accountType = value; }
        public int Status { get => status; set => status = value; }
        public int Movements { get => movements; set => movements = value; }
        public int ActiveDeposits { get => activeDeposits; set => activeDeposits = value; }
        public double Balance { get => balance; set => balance = value; }
        public double InterestRate { get => interestRate; set => interestRate = value; }

        // 1. Recuperar lista de cuentas abiertas para un número de cuenta bancaria.
        // account_type: 0 Corriente, 1 Ahorros, 2 Depósito.
        public List<OpenAccount> GetOpenAccountsList (string accountNumber)
        {
            try
            {
                connection = new Connection();

                string query = $@"SELECT 
                oa.open_account_id, 
                oa.bank_account_id, 
                oa.account_type, 
                oa.account_status, 
                oa.movements, 
                oa.active_deposits, 
                oa.balance, 
                oa.interest_rate FROM open_account oa JOIN bank_account ba 
                ON oa.bank_account_id = ba.bank_account_id 
                WHERE ba.account_number = '{accountNumber}';";

                List<OpenAccount> lOpenAccounts = new List<OpenAccount>();

                sqlDataReader = connection.reader(query);
                while (sqlDataReader.Read())
                {
                    OpenAccount openAccount = new OpenAccount();
                    openAccount.openAccountId = Convert.ToInt32(sqlDataReader[0]);
                    openAccount.bankAccountId = Convert.ToInt32(sqlDataReader[1]);
                    openAccount.accountType = Convert.ToInt32(sqlDataReader[2]);
                    openAccount.status = Convert.ToInt32(sqlDataReader[3]);
                    openAccount.movements = Convert.ToInt32(sqlDataReader[4]);
                    openAccount.activeDeposits = Convert.ToInt32(sqlDataReader[5]);
                    openAccount.balance = Convert.ToDouble(sqlDataReader[6]);
                    openAccount.interestRate = Convert.ToDouble(sqlDataReader[7]);

                    lOpenAccounts.Add(openAccount);
                }
                return lOpenAccounts;
            }
            finally
            {
                connection.connection.Close();
            }       
        }

        // 2. Modificar estado de cuenta abierta para un número de cuenta bancaria.
        public void UpdateAccountState(string accountNumber, int accountType, int accountStatus)
        {
            // 0: Inactiva - 1: Activa - 2: Inactiva con solicitud de reactivación.

            connection = new Connection(); bankAccount = new BankAccount(); 

            connection.executeQuery($@"UPDATE open_account 
            SET account_status = '{accountStatus}' 
            WHERE account_type = '{accountType}' 
            AND bank_account_id = '{bankAccount.GetBankAccountId(accountNumber)}';");
        }

        // 3. Recuperar un id de open_account con número de cuenta y tipo de cuenta.
        // Recupera el código de un tipo de cuenta abierta específica.
        public int GetOpenAccountId(string accountNumber, int accountType)
        {
            try
            {
                connection = new Connection(); bankAccount = new BankAccount();
                string query = $@"SELECT open_account_id FROM open_account 
                WHERE bank_account_id = '{bankAccount.GetBankAccountId(accountNumber)}' 
                AND account_type = '{accountType}';";

                sqlDataReader = connection.reader(query);
                sqlDataReader.Read();
                return Convert.ToInt32(sqlDataReader[0].ToString());
            }
            finally
            {
                connection.connection.Close();
            }  
        }

        // 4. Recuperar lista de cuentas abiertas para un tipo de cuenta específica.
        public List<OpenAccount> GetOpenAccountsListByType(int accountType)
        {
            try
            {
                connection = new Connection();

                List<OpenAccount> lOpenAccounts = new List<OpenAccount>();

                sqlDataReader = connection.reader($@"SELECT * FROM open_account 
                WHERE account_type = '{accountType}';");

                while (sqlDataReader.Read())
                {
                    OpenAccount openAccount = new OpenAccount();
                    openAccount.openAccountId = Convert.ToInt32(sqlDataReader[0]);
                    openAccount.bankAccountId = Convert.ToInt32(sqlDataReader[1]);
                    openAccount.accountType = Convert.ToInt32(sqlDataReader[2]);
                    openAccount.status = Convert.ToInt32(sqlDataReader[3]);
                    openAccount.movements = Convert.ToInt32(sqlDataReader[4]);
                    openAccount.activeDeposits = Convert.ToInt32(sqlDataReader[5]);
                    openAccount.balance = Convert.ToDouble(sqlDataReader[6]);
                    openAccount.interestRate = Convert.ToDouble(sqlDataReader[7]);

                    lOpenAccounts.Add(openAccount);
                }
                return lOpenAccounts;
            }
            finally
            {
                connection.connection.Close();
            }
        }

        // 5. Evento - Interés por cuenta de Ahorro cada mes.
        public void CheckSavingsAccountRelease()
        {
            transaction = new Transaction(); bankAccount = new BankAccount();

            if (DateTime.Now.Day == 1)
            {
                if (GetRegisteredSavingAccountInterestToday() == 0)
                {
                    foreach (var account in GetOpenAccountsListByType(1))
                    {
                        if (account.status == 1 && account.balance > 0)
                        {
                            double amount = account.Balance * account.interestRate;

                            transaction.RegisterTransactionCC(bankAccount.GetBankAccountNumber(account.bankAccountId),
                                "Interés por Cuenta de Ahorro", amount);
                        }
                    }
                }           
            }
        }

        // 5.1 Recuperar cantidad de interéses recuperados en cuentas de ahorro este día.
        public int GetRegisteredSavingAccountInterestToday()
        {
            try
            {
                connection = new Connection();
                string query = @"SELECT COUNT(*) FROM [transaction] WHERE 
                transaction_type = 'Interés por Cuenta de Ahorro' 
                AND  CAST(submit_date AS Date) = CAST(GETDATE() AS Date);";

                sqlDataReader = connection.reader(query);
                sqlDataReader.Read();
                return Convert.ToInt32(sqlDataReader[0].ToString());
            }
            finally
            {
                connection.connection.Close();
            }
        }

        // 6. Evento - Interés por depósitos fijos.
        public void CheckDepositAccountRelease(string accountNumber)
        {
            transaction = new Transaction();

            // 1. Update de depósitos activos.
            UpdateActiveDeposits(accountNumber);

            // 2. Registro de interés por depósito.
            foreach (var t in transaction.GetAccountTransactions(accountNumber, 2))
            {
                // Para los depósitos activos, verificar si han expirado.
                if (t.Status == 1)
                {
                    if (DateTime.Compare(DateTime.Now, t.ReleaseDate) > 0)
                    {
                        double amount = t.Amount; double interest = amount * t.InterestRate;

                        // 1. Registrando monto + interés de depósito en cuenta corriente.
                        transaction.RegisterTransactionCC(accountNumber, "Interés por Depósito", amount + interest);

                        // 2. Actualizando cantidad de depósitos activos.
                        transaction.DeactivateDepositTransaction(t.TransactionId);
                    }
                }
            }
        }

        // 7. Update número de depósitos activos actuales.
        public void UpdateActiveDeposits(string accountNumber)
        {
            connection = new Connection();

            connection.executeQuery($@"UPDATE open_account SET active_deposits = (SELECT COUNT(*) 
            FROM [transaction] WHERE [status] = 1 AND open_account_id = '{GetOpenAccountId(accountNumber, 2)}') 
            WHERE open_account_id = '{GetOpenAccountId(accountNumber, 2)}';");
        }
    }
}