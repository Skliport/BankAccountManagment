using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DPWALab2Periodo2.Models
{
    public class Transaction
    {
        private int transactionId, openAccountId, months;
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
            DateTime submitDate, DateTime releaseDate)
        {
            this.openAccountId = openAccountId;          // Id de registro en open_account.
            this.transactionId = transactionId;          // Id de registro en transaction.
            this.months = months;                        // Cantidad de meses.
            this.transactionType = transactionType;      // Tipo de transacción. (String).
            this.amount = amount;                        // Monto de transacción.
            this.interestRate = interestRate;            // Interés.
            this.submitDate = submitDate;                // Fecha de transacción. (Fecha inicial).
            this.releaseDate = releaseDate;              // Fecha de liberación. (Fecha final).
        }

        public int OpenAccountId { get => openAccountId; set => openAccountId = value; }
        public int TransactionId { get => transactionId; set => transactionId = value; }
        public int Months { get => months; set => months = value; }
        public string TransactionType { get => transactionType; set => transactionType = value; }
        public double Amount { get => amount; set => amount = value; }
        public double InterestRate { get => interestRate; set => interestRate = value; }
        public DateTime SubmitDate { get => submitDate; set => submitDate = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }

        // 1. Recuperar lista de transacciones para un número de cuenta bancaria.
        // Especifica el tipo de cuenta abierta, account_type: 0 Corriente, 1 Ahorros, 2 Depósito.
        public List<Transaction> GetAccountTransactions(string accountNumber, int accountType)
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
                tn.release_date 
                FROM open_account oa 
                JOIN bank_account ba ON oa.bank_account_id = ba.bank_account_id 
                JOIN [transaction]  tn ON tn.open_account_id = oa.open_account_id 
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

                lTransactions.Add(transaction);
            }
            return lTransactions;
        }

        // 2. Registrar una transacción.
        public void RegisterTransaction(string accountNumber, int accountType, int months, 
            string transactionType, double amount, double interestRate, DateTime releaseDate)
        {
            // Tipos de transacción:
            // "Abono", "Retiro", "Interés por cuenta de ahorros", "Interés por depósito".

            connection = new Connection();

            connection.executeQuery($@"INSERT INTO [transaction] VALUES 
            ('{openAccount.GetOpenAccountId(accountNumber, accountType)}', 
             '{months}', 
             '{transactionType}', 
             '{amount}', 
             '{interestRate}', 
             '{DateTime.Now}', 
             '{releaseDate}');");
        }
    }
}