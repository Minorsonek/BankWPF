using System.Collections.Generic;

namespace BankWPF.Core
{
    /// <summary>
    /// A view model for the overview menu list
    /// </summary>
    public class MenuListViewModel : BaseViewModel
    {
        /// <summary>
        /// The menu list items for the list
        /// </summary>
        public static List<MenuListItemViewModel> Items { get; set; }  
    }
}
