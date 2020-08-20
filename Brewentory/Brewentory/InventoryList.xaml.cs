using Brewentory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<BrewentoryModel> inventory { get; set; } 
        public InventoryList ()
		{
			InitializeComponent ();
                       
            inventoryList.ItemSelected += inventoryList_ItemSelected;
		}

        private void inventoryList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/inventory/");
            string[] inventoryArray = JsonConvert.DeserializeObject<string[]>(json);
            inventoryList.ItemsSource = inventoryArray;
            
            inventory = new ObservableCollection<BrewentoryModel>();
            

            for(int i = 0; i < inventoryArray.Count(); i++)
            {
                string[] data = inventoryArray[i].Split(",");               
                inventory.Add(new BrewentoryModel { Location = data[0], Product = data[1], Quantity = data[2] });
            }

            lstView.ItemsSource = inventory;
            string[] inventoryHeaders = new string[] {"Location", "Product", "Quantity" };
           
        }
        
        // GET: Inventory
        /*public BrewentoryModel GetInventoryModel()
        {
            
        }*/
    }
}