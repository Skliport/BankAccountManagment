using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DPWALab2Periodo2.Models
{
    public class BankAccount
    {
        private int bankAccountId;
        private string number, name, password;
        private double balance;
        private DateTime openingDate;
        private Connection connection; SqlDataReader sqlDataReader;

        public BankAccount()
        {
        }

        public BankAccount(string number, string name, string password, int bankAccountId, 
            double balance, DateTime openingDate)
        {
            this.bankAccountId = bankAccountId;     // Id de registro en bank_account.
            this.number = number;                   // Número de cuenta.
            this.name = name;                       // Nombre de cuenta.
            this.password = password;               // Password de cuenta.
            this.balance = balance;                 // Saldo total en cuenta (CC+CA+DP).
            this.openingDate = openingDate;         // Fecha de apertura de cuenta.
        }

        public int BankAccountId { get => bankAccountId; set => bankAccountId = value; }
        public string Number { get => number; set => number = value; }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public double Balance { get => balance; set => balance = value; }
        public DateTime OpeningDate { get => openingDate; set => openingDate = value; }

        // 1. Recuperar una Cuenta de Banco específica por número de cuenta.
        public BankAccount GetBankAccount(string accountNumber)
        {
            try
            {
                connection = new Connection();
                string query = $"SELECT * FROM bank_account WHERE account_number = '{accountNumber}';";

                sqlDataReader = connection.reader(query);
                if (sqlDataReader.Read())
                {
                    BankAccount bankAccount = new BankAccount();
                    bankAccount.bankAccountId = Convert.ToInt32(sqlDataReader[0]);
                    bankAccount.number = sqlDataReader[1].ToString();
                    bankAccount.name = sqlDataReader[2].ToString();
                    bankAccount.password = sqlDataReader[3].ToString();
                    bankAccount.balance = Convert.ToDouble(sqlDataReader[4].ToString());
                    bankAccount.openingDate = Convert.ToDateTime(sqlDataReader[5].ToString());

                    return bankAccount;
                }
                else return null;
            }
            finally
            {
                connection.connection.Close();
            }       
        }

        // 2. Recuperar lista de Cuentas de Banco.
        public List<BankAccount> GetBankAccountsList()
        {
            try
            {
                connection = new Connection();
                List<BankAccount> lBankAccounts = new List<BankAccount>();

                sqlDataReader = connection.reader("SELECT * FROM bank_account");
                while (sqlDataReader.Read())
                {
                    BankAccount bankAccount = new BankAccount();
                    bankAccount.bankAccountId = Convert.ToInt32(sqlDataReader[0]);
                    bankAccount.number = sqlDataReader[1].ToString();
                    bankAccount.name = sqlDataReader[2].ToString();
                    bankAccount.password = sqlDataReader[3].ToString();
                    bankAccount.balance = Convert.ToDouble(sqlDataReader[4].ToString());
                    bankAccount.openingDate = Convert.ToDateTime(sqlDataReader[5].ToString());

                    lBankAccounts.Add(bankAccount);
                }
                return lBankAccounts;
            }
            finally
            {
                connection.connection.Close();
            }      
        }

        // 3. Recuperar un id de registro específico en bank_account con número de cuenta.
        public int GetBankAccountId(string accountNumber)
        {
            try
            {
                connection = new Connection();
                string query = $"SELECT bank_account_id FROM bank_account WHERE account_number = '{accountNumber}';";
                sqlDataReader = connection.reader(query);
                sqlDataReader.Read();
                return Convert.ToInt32(sqlDataReader[0].ToString());
            }
            finally
            {
                connection.connection.Close();
            }     
        }

        // 4. Recuperar un numero de cuenta de registro específico por bank_account_id.
        public string GetBankAccountNumber(int bankAccountId)
        {
            try
            {
                connection = new Connection();
                string query = $"SELECT account_number FROM bank_account WHERE bank_account_id = '{bankAccountId}';";
                sqlDataReader = connection.reader(query);
                sqlDataReader.Read();
                return sqlDataReader[0].ToString();
            }
            finally
            {
                connection.connection.Close();
            }
        }

        // 5. Registra una cuenta de banco con un número, nombre y password.
        public void RegisterBankAccount(string number, string name, string password)
        {
            connection = new Connection();
            connection.executeQuery($@"INSERT INTO bank_account 
            VALUES('{number}', '{name}', '{password}', '0', '{DateTime.Now}')");

            // 0 - Cuenta corriente; 1 - Cuenta de ahorro; 2 - Cuenta de depósito.
            for (int accountType = 0; accountType < 3; accountType++)
            {
                connection.executeQuery($@"INSERT INTO open_account VALUES 
                ({GetBankAccountId(number)}, {accountType}, '0', '0', '0', '0','0');");
            }
        }

        // 6. Inicio de sesión.
        public bool AuthenticateUser(string accountNumber, string password)
        {
            try
            {
                connection = new Connection();
                sqlDataReader = connection.reader($@"SELECT COUNT(*) FROM bank_account 
                WHERE account_number = '{accountNumber}' AND account_password = '{password}';");

                sqlDataReader.Read();
                if (Convert.ToInt32(sqlDataReader[0]) == 1) { return true; } else return false;
            }
            finally
            {
                connection.connection.Close();
            }          
        }
    }
}