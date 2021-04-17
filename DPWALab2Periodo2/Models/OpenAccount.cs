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
        BankAccount bankAccount;
      
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
            connection = new Connection();
            string query = $@"SELECT open_account_id FROM open_account 
            WHERE bank_account_id = '{bankAccount.GetBankAccountId(accountNumber)}' 
            AND account_type = '{accountType}';";

            sqlDataReader = connection.reader(query);
            sqlDataReader.Read();
            return Convert.ToInt32(sqlDataReader[0].ToString());
        }
    }
}