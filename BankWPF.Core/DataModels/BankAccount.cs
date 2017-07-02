using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace BankWPF.Core
{
    /// <summary>
    /// A class that will handle account data
    /// </summary>
    public class BankAccount
    {
        #region Singleton

        /// <summary>
        /// The user's account instance
        /// </summary>
        public static BankAccount UserAccount { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Current balance in our account
        /// </summary>
        public int Balance { get; set; } = 0;

        /// <summary>
        /// Account ID
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Account name - login/nickname
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Private Structs

        /// <summary>
        /// A pair of balance + id to return by methods
        /// </summary>
        private struct BalanceIdPair
        {
            public int balance;
            public int id;

            public BalanceIdPair(int i, int b)
            {
                balance = b;
                id = i;
            }
        }

        #endregion

        #region Constructor

        public BankAccount(int b, int n, string name)
        {
            Balance = b;
            Number = n;
            Name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deposits the specified value
        /// </summary>
        /// <param name="value">The value user wants to deposit</param>
        public void Deposit(int value, string message)
        {
            // Add value to balance
            Balance += value;

            // Inform user about successful operation
            //MessageBox.Show($"Zdepozytowano { value } zł.");

            // Upload new transaction to the database
            UploadNewTransaction("Deposit", value, message);

            // Update current balance to the database
            UpdateBalance(Number, Balance);
        }

        /// <summary>
        /// Withdraw the specified value
        /// </summary>
        /// <param name="value">The value user wants to withdraw</param>
        public void Withdraw(int value, string message)
        {
            // Check if value is greater than balance, if yes, withdraw the whole cash
            if (value > Balance) value = Balance;

            // Substract value from balance
            Balance -= value;

            // Upload new transaction to the database
            UploadNewTransaction("Withdraw", value, message);

            // Update current balance to database
            UpdateBalance(Number, Balance);

            // Inform user about successful operation
            //MessageBox.Show($"Wyplacono { value } zł.");
        }

        /// <summary>
        /// Transfer the specified value
        /// </summary>
        /// <param name="value">The value user wants to transfer</param>
        /// <param name="login">The user to transfer to</param>
        public void Transfer(int value, string login)
        {
            // Check if value is greater than balance, if yes, transfer the whole cash
            if (value > Balance) value = Balance;

            // Substract value from balance
            Balance -= value;

            // Upload new transaction to the database
            TransferMoneyTo(value, login);

            // Update current balance to database
            UpdateBalance(Number, Balance);

            // Inform user about successful operation
            //MessageBox.Show($"Wyplacono { value } zł.");
        }

        #endregion

        #region Private Helpers

        private void UploadNewTransaction(string paymentway, int value, string message)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/savetohistory/index.php?";
            string myParameters = $"id={ Number }&depOrWit={ paymentway }&value={ value }&message={ message }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") Debugger.Break();
        }

        /// <summary>
        /// Balance in database updater
        /// </summary>
        private void UpdateBalance(int id, int balance)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/uploaddata/index.php?";
            string myParameters = $"id={ id }&balance={ balance }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") Debugger.Break();
        }

        /// <summary>
        /// Balance in database updater
        /// </summary>
        private void TransferMoneyTo(int value, string login)
        {
            // Get user balance and id by its login
            var userData = DownloadUserBalance(login);

            // Add value to this
            userData.balance += value;

            // Upload new balance
            UpdateBalance(userData.id, userData.balance);
        }

        private BalanceIdPair DownloadUserBalance(string login)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/downloadbalance/index.php?";
            string myParameters = $"login={ login }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result == "Not found") Debugger.Break();

            // Split the result to balance and id
            string[] resultArray = result.Split('/');

            // Try to convert result balance to integer number
            int balance = 0;
            Int32.TryParse(resultArray[0], out balance);

            // Try to convert result id to integer number
            int id = 0;
            Int32.TryParse(resultArray[1], out id);

            // Make new pair of results
            var pair = new BalanceIdPair(id, balance);

            // Return the pair of balance and id
            return pair;
        }

        #endregion
    }
}
