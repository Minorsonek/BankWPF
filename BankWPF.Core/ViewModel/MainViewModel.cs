using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankWPF.Core
{
    /// <summary>
    /// The View Model for a main page
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user's input value
        /// </summary>
        public string InputValue { get; set; }

        /// <summary>
        /// Current user's balance converted to string to display
        /// </summary>
        public string BalanceString => BankAccount.UserAccount.Balance.ToString();

        /// <summary>
        /// A flag indicating if the deposit command is running
        /// </summary>
        public bool DepositIsRunning { get; set; }

        /// <summary>
        /// A flag indicating if the withdraw command is running
        /// </summary>
        public bool WithdrawIsRunning { get; set; }

        /// <summary>
        /// A flag indicating if the not integer number error should be shown
        /// </summary>
        public bool ErrorNotInteger { get; set; }

        /// <summary>
        /// A flag indicating if the not enough money error should be shown
        /// </summary>
        public bool ErrorNotEnoughMoney { get; set; }

        /// <summary>
        /// A flag indicating if the wrong value error should be shown
        /// </summary>
        public bool ErrorWrongValue { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to deposit the value
        /// </summary>
        public ICommand DepositCommand { get; set; }

        /// <summary>
        /// The command to withdraw the value
        /// </summary>
        public ICommand WithdrawCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            // Create commands
            DepositCommand = new RelayParameterizedCommand(async (parameter) => await DepositAsync(parameter));
            WithdrawCommand = new RelayParameterizedCommand(async (parameter) => await WithdrawAsync(parameter));
        }

        #endregion

        #region Procedures

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task DepositAsync(object parameter)
        {
            // This function will run only if LoginIsRunning is false
            await RunCommandAsync(() => DepositIsRunning, async () =>
            {
                await Task.Delay(500);

                int valueToDeposit = 0;

                // Try to parse user's input from textbox to integer
                if (Int32.TryParse(this.InputValue, out valueToDeposit))
                {
                    // If its greater than 0, deposit
                    if (valueToDeposit > 0)
                    {
                        // Deposit value
                        BankAccount.UserAccount.Deposit(valueToDeposit);
                    }
                    else
                    {
                        // Can't deposit negative or null value, output error
                        ErrorNotInteger = false;
                        ErrorNotEnoughMoney = false;
                        ErrorWrongValue = true;
                    }
                }
                else
                {
                    // If failed, output error
                    ErrorNotInteger = true;
                    ErrorNotEnoughMoney = false;
                    ErrorWrongValue = false;
                }

            });
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        /// <returns></returns>
        public async Task WithdrawAsync(object parameter)
        {
            // This function will run only if WithdrawIsRunning is false
            await RunCommandAsync(() => WithdrawIsRunning, async () =>
            {
                await Task.Delay(500);

                int valueToWithdraw = 0;

                // Try to parse user's input from textbox to integer
                if (Int32.TryParse(this.InputValue, out valueToWithdraw))
                {
                    // If its greater than 0, withdraw
                    if (valueToWithdraw > 0)
                    {
                        // Withdraw value
                        BankAccount.UserAccount.Withdraw(valueToWithdraw);
                    }
                    else
                    {
                        // Can't withdraw negative or null value, output error
                        ErrorNotInteger = false;
                        ErrorNotEnoughMoney = false;
                        ErrorWrongValue = true;
                    }
                }
                else
                {
                    // If failed, output error
                    ErrorNotInteger = true;
                    ErrorNotEnoughMoney = false;
                    ErrorWrongValue = false;
                }
            });
        }

        #endregion
    }
}
