using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Brewentory.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Brewentory
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventoryPopupView
	{        
        private string actionName;
        private int locationID;
        private Button refreshBtn;
        private ObservableCollection<BrewentoryModel> inventoryModel;
        
		public InventoryPopupView (Button btn, string action, int locID, ObservableCollection<BrewentoryModel> inventory)
		{
			InitializeComponent ();            
            inventoryModel = inventory;
            locationID = locID;
            refreshBtn = btn;
            actionName = action;
            
            if(action == "Edit")
            {
                saveButton.Text = "Save Changes";
            }
            else if(action == "Delete")
            {
                saveButton.Text = "Delete";
            }
            else if(action == "Save")
            {
                saveButton.Text = "Save New";                
            }
		}

        private async void Flash()
        {
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(200);
                await refreshBtn.FadeTo(0, 250);
                await Task.Delay(200);
                await refreshBtn.FadeTo(1, 250);
                if (refreshBtn.Text == "Create New") break;
            }            
        }
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();
            try
            {
                if(actionName == "Edit")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                        LocationID = locationID,
                        Location = locationEntry.Text.ToUpper(),
                        Product = productEntry.Text.ToUpper(),
                        Quantity = quantityEntry.Text.ToUpper()
                    };
                }
                else if(actionName == "Save")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "Create",
                        Location = locationEntry.Text.ToUpper(),
                        Product = productEntry.Text.ToUpper(),
                        Quantity = quantityEntry.Text.ToUpper()
                    };
                    refreshBtn.Text = "Refresh";
                    refreshBtn.BackgroundColor = Color.DarkSeaGreen;
                    Flash();
                    //inventoryModel.Add(data);
                }
                else if(actionName == "Delete")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "Delete",
                        LocationID = locationID, 
                        Location = locationEntry.Text,
                        Product = productEntry.Text,
                        Quantity = quantityEntry.Text
                    };
                }

                // <<<<<< Below method works but may not be the best solution. Return to this? >>>>>>>
                if (actionName == "Edit" || actionName == "Delete")
                {
                    for (int i = 0; i < inventoryModel.Count; i++)
                    {
                        if (inventoryModel[i].LocationID == locationID)
                        {
                            if (actionName == "Edit")
                            {
                                inventoryModel.Remove(inventoryModel[i]);
                                inventoryModel.Insert(i, data);
                                break;
                            }
                            else if (actionName == "Delete")
                            {
                                inventoryModel.Remove(inventoryModel[i]);
                                break;
                            }

                        }
                    }
                }


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                var debugText = content.ReadAsStringAsync();

                HttpResponseMessage message = await client.PostAsync("/api/inventory", content);
                string reply = await message.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if(success)
                {
                    await DisplayAlert("Saved!", "Your changes have been saved!", "Close");
                    await Navigation.PopPopupAsync();
                }
                else
                {
                    await DisplayAlert("Failed!", "Could not save changes..", "Close"); 
                }

            }
            catch (Exception ex)
            {
                string errorMsg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errorMsg;
                //await DisplayAlert("Oops..", "Couldn't get data from DB", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if(actionName != "Save")
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                    string json = await client.GetStringAsync("api/inventory?locationID=" + locationID);
                    BrewentoryModel chosenInventoryModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                    invHeadline.Text = actionName;
                    locationEntry.Text = chosenInventoryModel.Location;
                    productEntry.Text = chosenInventoryModel.Product;
                    quantityEntry.Text = chosenInventoryModel.Quantity;
                    locationID = chosenInventoryModel.LocationID;
                }
                else
                {

                }
                
            }
            catch (Exception ex)
            {
                string errorMessage = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errorMessage;
            }
        }
    }
}