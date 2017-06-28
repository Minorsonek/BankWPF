using System;
using System.Net;

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
        public int Balance { get; private set; }

        /// <summary>
        /// Account ID
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Account name - login/nickname
        /// </summary>
        public string Name { get; private set; }

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
        public void Deposit(int value)
        {
            // Add value to balance
            Balance += value;

            // Inform user about successful operation
            //MessageBox.Show($"Zdepozytowano { value } zł.");

            // Upload new transaction to the database
            UploadNewTransaction("Deposit", value);

            // Update current balance to the database
            UpdateBalance();
        }

        /// <summary>
        /// Withdraw the specified value
        /// </summary>
        /// <param name="value">The value user wants to withdraw</param>
        public void Withdraw(int value)
        {
            // Check if value is greater than balance, if yes, withdraw the whole cash
            if (value > Balance) value = Balance;

            // Substract value from balance
            Balance -= value;

            // Inform user about successful operation
            //MessageBox.Show($"Wyplacono { value } zł.");

            // Upload new transaction to the database
            UploadNewTransaction("Withdraw", value);

            // Update current balance to database
            UpdateBalance();
        }

        #endregion

        #region Private Helpers

        private void UploadNewTransaction(string paymentway, int value)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/savetohistory/index.php?";
            string myParameters = $"id={ Number }&depOrWit={ paymentway }&value={ value }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") ;
        }

        /// <summary>
        /// Balance in database updater
        /// </summary>
        private void UpdateBalance()
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/uploaddata/index.php?";
            string myParameters = $"id={ Number }&balance={ Balance }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") ;
        }

        #endregion
    }
}
