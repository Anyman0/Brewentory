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

            var GoToInventory = new ToolbarItem()
            {
                Text = "Inventory",
            };
            GoToInventory.Clicked += GoToInventory_Clicked;
            ToolbarItems.Add(GoToInventory);
		}

        private void GoToInventory_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new InventoryPage());
        }

        private async void InventoryButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryList());
        }
    }
}
