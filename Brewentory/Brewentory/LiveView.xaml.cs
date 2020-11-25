using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LiveView : ContentPage
	{
		public LiveView ()
		{
			InitializeComponent ();

            // Toolbar items
            var goToInventory = new ToolbarItem()
            {
                Text = "Inventory"
            };
            goToInventory.Clicked += GoToInventory_Clicked;
            ToolbarItems.Add(goToInventory);

            var goToTimesheet = new ToolbarItem()
            {
                Text = "Timesheet"
            };
            goToTimesheet.Clicked += GoToTimesheet_Clicked;
            ToolbarItems.Add(goToTimesheet);
		}

        private async void GoToTimesheet_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShiftList());
        }

        private async void GoToInventory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryList());
        }
    }
}