using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Services
{
    public class DisplayMessageService : IDisplayMessageService
    {
        public async Task DisplayAlertAsync(string title, string message, string buttonText)
        {
            await Shell.Current.DisplayAlert(title, message, buttonText);
        }
    }
}
