using System.Collections.Generic;
using System.Net;

namespace BankWPF.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MenuListViewModel"/>
    /// </summary>
    public class MenuListDesignModel : MenuListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MenuListDesignModel Instance => new MenuListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListDesignModel()
        {
            Items = new List<MenuListItemViewModel>
            {
                new MenuListItemViewModel
                {
                    Value = "500",
                    DWLetter = "D",
                    Message = "This chat app is awesome! I bet it will be fast too",
                    ColorStringRGB = "00d405",
                    Date = "2017/11/20 09:06:18"
                },
                new MenuListItemViewModel
                {
                    Value = "-500",
                    DWLetter = "W",
                    Message = "Hey dude, here are the new icons",
                    ColorStringRGB = "fe4503",
                    Date = "2017/11/20 09:06:18"
                },
                new MenuListItemViewModel
                {
                    Value = "300",
                    DWLetter = "D",
                    Message = "The new server is up, got 192.168.1.1",
                    ColorStringRGB = "00d405",
                    Date = "2017/11/20 11:06:18"
                },
            };
        }

        #endregion
    }
}
