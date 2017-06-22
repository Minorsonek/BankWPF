using System;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankWPF.Core
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The nickname of the user
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// User's account instance
        /// </summary>
        public BankAccount UserAccount { get; private set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await Login(parameter));
            RegisterCommand = new RelayCommand(() => Register());
        }

        #endregion

        #region Procedures

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the user's password</param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {
            await RunCommand(() => this.LoginIsRunning, async () =>
            {
                await Task.Delay(500);
                string accountNickname = this.Nickname;
                string accountPassword = (parameter as IHavePassword).SecurePassword.Unsecure();

                // Try to log in
                bool isAccountFound = LogIntoAccount(accountNickname, accountPassword);

                // If account was found
                if (isAccountFound)
                {
                    // Show main page
                    IoC.Get<ApplicationViewModel>().CurrentPage = ApplicationPage.Main;
                }
                else
                {
                    // Account wasnt found, output error
                }
            });
        }

        /// <summary>
        /// Sends user to the register page
        /// </summary>
        public void Register()
        {
            // Simply send user to account register page
            Process.Start("http://stacjapogody.lo2przemysl.edu.pl/register/");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Try to log in with user's data, if failed returns false
        /// </summary>
        /// <param name="login">Account login</param>
        /// <param name="password">Account password</param>
        /// <returns></returns>
        private bool LogIntoAccount(string login, string password)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bankloggingin/index.php";
            string myParameters = $"login={ login }&password={ password }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If user wasnt found, return false
            if (result == "Not found") return false;
            else
            {
                // We have user's data, lets put this into variables
                string[] accountArray = result.Split('/');
                int id = Int32.Parse(accountArray[0]);
                string name = accountArray[1];
                int balance = Int32.Parse(accountArray[2]);
                UserAccount = new BankAccount(balance, id, name);
                return true;
            }

        }

        #endregion
    }
}
