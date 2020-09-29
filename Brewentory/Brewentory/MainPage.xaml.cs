using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Brewentory
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();


            // Toolbar items
            var GoToInventory = new ToolbarItem()
            {
                Text = "Inventory",
            };
            GoToInventory.Clicked += GoToInventory_Clicked;
            ToolbarItems.Add(GoToInventory);

            var GoToTimeSheet = new ToolbarItem()
            {
                Text = "Shifts"
            };
            GoToTimeSheet.Clicked += GoToTimeSheet_Clicked;
            ToolbarItems.Add(GoToTimeSheet);
		}

        private async void GoToTimeSheet_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShiftList());
        }

        private async void GoToInventory_Clicked(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new InventoryList());
        }        
    }
}
