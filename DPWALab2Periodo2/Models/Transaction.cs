using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DPWALab2Periodo2.Models
{
    public class Transaction
    {
        private int transactionId, openAccountId, months, status;
        private string transactionType;
        private double amount, interestRate;
        private DateTime submitDate, releaseDate;
        private Connection connection; SqlDataReader sqlDataReader;
        OpenAccount openAccount;

        public Transaction()
        {
        }

        public Transaction(int openAccountId, int transactionId, int months, 
            string transactionType, double amount, double interestRate, 
            DateTime submitDate, DateTime releaseDate, int status)
        {
            this.openAccountId = openAccountId;          // Id de registro en open_account.
            this.transactionId = transactionId;          // Id de registro en transaction.
            this.months = months;                        // Cantidad de meses.
            this.transactionType = transactionType;      // Tipo de transacción. (String).
            this.amount = amount;                        // Monto de transacción.
            this.interestRate = interestRate;            // Interés.
            this.submitDate = submitDate;                // Fecha de transacción. (Fecha inicial).
            this.releaseDate = releaseDate;              // Fecha de liberación. (Fecha final).
            this.status = status;
        }

        public int OpenAccountId { get => openAccountId; set => openAccountId = value; }
        public int TransactionId { get => transactionId; set => transactionId = value; }
        public int Months { get => months; set => months = value; }
        public string TransactionType { get => transactionType; set => transactionType = value; }
        public double Amount { get => amount; set => amount = value; }
        public double InterestRate { get => interestRate; set => interestRate = value; }
        public DateTime SubmitDate { get => submitDate; set => submitDate = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public int Status { get => status; set => status = value; }

        // 1. Recuperar lista de transacciones para un número de cuenta bancaria.
        // Especifica el tipo de cuenta abierta, account_type: 0 Corriente, 1 Ahorros, 2 Depósito.
        public List<Transaction> GetAccountTransactions(string accountNumber, int accountType)
        {
            try
            {
                connection = new Connection();
                string query = $@"SELECT 
                tn.transaction_id, 
                tn.open_account_id, 
                tn.months, 
                tn.transaction_type, 
                tn.amount, 
                tn.interest_rate, 
                tn.submit_date, 
                tn.release_date, 
                tn.[status] 
                FROM open_account oa 
                JOIN bank_account ba ON oa.bank_account_id = ba.bank_account_id 
                JOIN [transaction] tn ON tn.open_account_id = oa.open_account_id 
                WHERE ba.account_number = '{accountNumber}' AND oa.account_type = {accountType};";

                List<Transaction> lTransactions = new List<Transaction>();

                sqlDataReader = connection.reader(query);
                while (sqlDataReader.Read())
                {
                    Transaction transaction = new Transaction();

                    transaction.transactionId = Convert.ToInt32(sqlDataReader[0]);
                    transaction.openAccountId = Convert.ToInt32(sqlDataReader[1]);
                    transaction.months = Convert.ToInt32(sqlDataReader[2]);
                    transaction.transactionType = sqlDataReader[3].ToString();
                    transaction.amount = Convert.ToDouble(sqlDataReader[4]);
                    transaction.interestRate = Convert.ToDouble(sqlDataReader[5]);
                    transaction.submitDate = Convert.ToDateTime(sqlDataReader[6]);
                    transaction.releaseDate = Convert.ToDateTime(sqlDataReader[7]);
                    transaction.status = Convert.ToInt32(sqlDataReader[8]);

                    lTransactions.Add(transaction);
                }
                return lTransactions;
            }
            finally
            {
                connection.connection.Close();
            }     
        }

        // Tipos de transacción:
        // "Abono", "Retiro", "Interés por cuenta de ahorros", "Interés por depósito".

        // 2. Registrar transacción - Cuenta Corriente (CC)
        public void RegisterTransactionCC(string accountNumber, string transactionType, double amount)
        {    
            connection = new Connection(); openAccount = new OpenAccount();

            connection.executeQuery($@"INSERT INTO [transaction] VALUES 
            ('{openAccount.GetOpenAccountId(accountNumber, 0)}', '0', '{transactionType}', '{amount}', 
            '0', '{DateTime.Now}', '{DateTime.Now}', '1');");

            UpdateAccountMovements(accountNumber, 0);
            UpdateCurrentAccountValue(accountNumber, amount, transactionType);
        }

        // 3. Registrar transacción - Cuenta de Ahorros (CA)
        public void RegisterTransactionCA(string accountNumber, string transactionType, double amount)
        {
            connection = new Connection(); openAccount = new OpenAccount();

            connection.executeQuery($@"INSERT INTO [transaction] VALUES 
            ('{openAccount.GetOpenAccountId(accountNumber, 1)}', '0', '{transactionType}', '{amount}', 
            '0', '{DateTime.Now}', '{DateTime.Now}', '1');");

              UpdateAccountMovements(accountNumber, 1);
              UpdateSavingsAccountValue(accountNumber, amount, transactionType);
              UpdateSavingsAccountInterest(accountNumber);
        }

        // 4. Registrar transacción - Cuenta de Depósito (CD)
        public void RegisterTransactionCD(string accountNumber, int months, double amount, double interestRate)
        {
            connection = new Connection(); openAccount = new OpenAccount();

            connection.executeQuery($@"INSERT INTO [transaction] VALUES 
            ('{openAccount.GetOpenAccountId(accountNumber, 2)}', '{months}', 'Deposito', 
            '{amount}', '{interestRate}', '{DateTime.Now}', '{DateTime.Now.AddMonths(months)}', '1');");

            UpdateAccountMovements(accountNumber, 2);
            UpdateDepositAccountValue(accountNumber);
        }

        // 5. Update en cant. de movimientos de cuenta.
        public void UpdateAccountMovements(string accountNumber, int accountType)
        {
            connection = new Connection(); openAccount = new OpenAccount();

            connection.executeQuery($@"UPDATE open_account SET movements = (SELECT movements FROM open_account  
            WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, accountType)}') + 1 
            WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, accountType)}';");
        }

        // 6. Update Balance Cuenta de Corriente.
        public void UpdateCurrentAccountValue(string accountNumber, double amount, string transactionType)
        {
            connection = new Connection(); openAccount = new OpenAccount();

            if (transactionType == "Retiro")
            {
                connection.executeQuery($@"UPDATE open_account SET balance = (SELECT balance FROM open_account 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 0)}') - '{amount}' 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 0)}';");
            }
            else
            {
                connection.executeQuery($@"UPDATE open_account SET balance = (SELECT balance FROM open_account 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 0)}') + '{amount}' 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 0)}';");
            }
        }

        // 7. Update Balance Cuenta de Ahorros.
        public void UpdateSavingsAccountValue(string accountNumber, double amount, string transactionType)
        {
            connection = new Connection(); openAccount = new OpenAccount();

            if (transactionType == "Retiro")
            {
                connection.executeQuery($@"UPDATE open_account SET balance = (SELECT balance FROM open_account 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 1)}') - '{amount}' 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 1)}';");
            }
            else
            {
                connection.executeQuery($@"UPDATE open_account SET balance = (SELECT balance FROM open_account 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 1)}') + '{amount}' 
                WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 1)}';");
            }           
        }

        // 8. Update interés de cuenta de ahorro.
        public void UpdateSavingsAccountInterest(string accountNumber)
        {
            connection = new Connection(); openAccount = new OpenAccount();
            connection.executeQuery($@"EXEC update_savings_account_interest  
            @open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 1)}';");
        }

        // 9. Update Balance de cuenta de depósito
        public void UpdateDepositAccountValue(string accountNumber)
        {
            connection = new Connection(); openAccount = new OpenAccount();
            connection.executeQuery($@"UPDATE open_account SET balance = (SELECT SUM(amount) FROM [transaction] 
            WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 2)}' AND status = '1') 
            WHERE open_account_id = '{openAccount.GetOpenAccountId(accountNumber, 2)}'");
        }

        // 10. Update estado de transacción en cuenta depósito - Pasar a inactivo.
        public void DeactivateDepositTransaction(int transactionId)
        {
            connection = new Connection(); openAccount = new OpenAccount();
            connection.executeQuery($@"UPDATE [transaction] SET [status] = '0' 
            WHERE transaction_id = '{transactionId}';");
        }

    }
}