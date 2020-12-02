using Brewentory.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
	public partial class LivePopupView
	{
        private int prodID;
        private string actionName;
        private ObservableCollection<BrewentoryModel> collection;
		public LivePopupView (string action, int productID, ObservableCollection<BrewentoryModel> model)
		{
			InitializeComponent ();
            prodID = productID;
            actionName = action;
            collection = model;
		}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();
            try
            {
                data = new BrewentoryModel()
                {
                    Operation = actionName,
                    ProductID = prodID,
                    ProductLive = productEntry.Text.ToUpper(),
                    Batch = batchEntry.Text.ToUpper(),
                    Pallets = int.Parse(palletsEntry.Text),
                    QuantityLive = int.Parse(quantityEntry.Text),
                    LiveStatus = statusSwitch.IsToggled
                };

                // <<<<<< Below method works but may not be the best solution. Return to this? >>>>>>>
                if (actionName == "Edit")
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].ProductID == prodID)
                        {
                            if (actionName == "Edit")
                            {
                                collection.Remove(collection[i]);
                                collection.Insert(i, data);
                                break;
                            }                            
                        }
                    }
                }

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await client.PostAsync("api/live", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if(success)
                {
                    await DisplayAlert("Saved!", "Your changes have been saved!", "Close");                  
                    await Navigation.PopPopupAsync();                    
                }
                else
                {
                    await DisplayAlert("Failed!", "Couldn't save changes!", "Close");
                }
            }
            catch(Exception ex)
            {
                string errMsg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errMsg;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string json = await client.GetStringAsync("api/live?productID=" + prodID);
                BrewentoryModel chosenModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                productEntry.Text = chosenModel.ProductLive;
                batchEntry.Text = chosenModel.Batch;
                palletsEntry.Text = chosenModel.Pallets.ToString();
                quantityEntry.Text = chosenModel.QuantityLive.ToString();
                statusSwitch.IsToggled = chosenModel.LiveStatus;                
            }
            catch (Exception ex)
            {
                string errMsg = ex.GetType().Name + ": " + ex.Message;
                productEntry.Text = errMsg;
            }
        }
    }
}