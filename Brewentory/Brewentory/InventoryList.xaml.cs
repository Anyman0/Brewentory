using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventoryList : ContentPage
	{
		public InventoryList ()
		{
			InitializeComponent ();            
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            testLabel.Text = "WORKS";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            //string json = await client.GetStringAsync("api/Inventory/");
            //string[] inventoryArray = JsonConvert.DeserializeObject<string[]>(json);
            inventoryList.ItemsSource = new string[] {"TEST", "TEST2" };//inventoryArray;
        }
    }
}