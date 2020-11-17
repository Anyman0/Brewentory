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
	public partial class ModifyShiftsPopupView
	{
        int shiftID;
        string actionName;
        private ObservableCollection<BrewentoryModel> shiftModel;
        public ModifyShiftsPopupView (int id, string action, ObservableCollection<BrewentoryModel> Omodel)
		{
			InitializeComponent ();
            shiftID = id;
            actionName = action;
            shiftModel = Omodel;
		}

        private async void SaveShiftButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();

            try
            {
                if(actionName == "Save")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "CreateShift",
                        ShiftID = shiftID,
                        ShiftName = ShiftNameEntry.Text,
                        ShiftTimes = ShiftTimeEntry.Text
                    };
                    shiftModel.Add(data);
                }
                else if(actionName == "Edit")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "EditShift",
                        ShiftID = shiftID,
                        ShiftName = ShiftNameEntry.Text,
                        ShiftTimes = ShiftTimeEntry.Text
                    };                    
                }

                else if(actionName == "Delete")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = "DeleteShift",
                        ShiftID = shiftID,
                        ShiftName = ShiftNameEntry.Text,
                        ShiftTimes = ShiftTimeEntry.Text
                    };
                }

                // <<<<<< Below method works but may not be the best solution. Return to this? >>>>>>>
                for (int i = 0; i < shiftModel.Count; i++)
                {
                    if (shiftModel[i].ShiftID == shiftID)
                    {
                        if (actionName == "Edit")
                        {
                            shiftModel.Remove(shiftModel[i]);
                            shiftModel.Insert(i, data);
                            break;
                        }
                        else if (actionName == "Delete")
                        {
                            shiftModel.Remove(shiftModel[i]);
                            break;
                        }

                    }
                }

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                HttpResponseMessage msg = await client.PostAsync("api/shiftlist", content);
                string reply = await msg.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);
                
                if(success)
                {
                    await DisplayAlert("Saved!", "Changes saved.", "OK");
                }
                else
                {
                    await DisplayAlert("Save failed!", "Couldn't save changes.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Oops!", "Couldn't get data from DB.", "Ok");
            }

            await Navigation.PopPopupAsync();
        }

        

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (actionName != "Save")
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string json = await client.GetStringAsync("/api/shiftlist?shiftID=" + shiftID);
                BrewentoryModel chosenShiftModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                ShiftNameEntry.Text = chosenShiftModel.ShiftName;
                ShiftTimeEntry.Text = chosenShiftModel.ShiftTimes;             
                if (actionName == "Edit") Header.Text = "Edit Shift";
            }
            else if(actionName == "Save")
            {

            }
            
            if (actionName == "Delete")
            {
                SaveShiftButton.Text = "Delete";
            }
            else if (actionName == "Edit")
            {
                SaveShiftButton.Text = "Save changes";
            }
        }
    }
}