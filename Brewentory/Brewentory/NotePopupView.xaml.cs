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
	public partial class NotePopupView
	{
        private string actionName;
        private int headlineId;
        private Button refreshButton;
        private ObservableCollection<BrewentoryModel> collection;
		public NotePopupView (string action, int id, ObservableCollection<BrewentoryModel> model, Button btn)
		{
			InitializeComponent ();
            actionName = action;
            headlineId = id;
            collection = model;            
            refreshButton = btn;
		}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            BrewentoryModel data = new BrewentoryModel();

            try
            {
                if (actionName == "Edit" || actionName == "Delete")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                        HeadlineID = headlineId,
                        Headline = headlineEntry.Text,
                        Note = noteEntry.Text
                    };
                }
                else if (actionName == "Create")
                {
                    data = new BrewentoryModel()
                    {
                        Operation = actionName,
                        Headline = headlineEntry.Text,
                        Note = noteEntry.Text
                    };
                    refreshButton.Text = "Refresh";
                    refreshButton.BackgroundColor = Color.DarkSeaGreen;
                    Flash();
                    //collection.Add(data);
                }


                if (actionName == "Edit" || actionName == "Delete")
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].HeadlineID == headlineId)
                        {
                            if (actionName == "Edit")
                            {
                                collection.Remove(collection[i]);
                                collection.Insert(i, data);
                                break;
                            }
                            else if (actionName == "Delete")
                            {
                                collection.Remove(collection[i]);
                            }
                        }
                    }
                }

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                string input = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                HttpResponseMessage message = await client.PostAsync("api/noteviewapi", content);
                string reply = await message.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(reply);
                if (success)
                {
                    await DisplayAlert("Saved!", "Changes have been saved.", "Close");
                    await Navigation.PopPopupAsync();
                }
                else
                {
                    await DisplayAlert("Failed!", "Couldn't save changes..", "Close");
                }
            }
            catch(Exception ex)
            {
                string msg = ex.GetType().Name + ": " + ex.Message;
                noteEntry.Text = msg;
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if(actionName == "Edit")
                {
                    saveButton.Text = "Save Changes";
                }
                else if(actionName == "Create")
                {
                    saveButton.Text = "Add New";
                }
                else if(actionName == "Delete")
                {
                    headlineEntry.IsReadOnly = true;
                    noteEntry.IsReadOnly = true;
                    saveButton.Text = "Delete";
                }

                if(actionName != "Create")
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
                    string json = await client.GetStringAsync("api/noteviewapi?headlineID=" + headlineId);
                    BrewentoryModel chosenModel = JsonConvert.DeserializeObject<BrewentoryModel>(json);
                    headlineEntry.Text = chosenModel.Headline;
                    noteEntry.Text = chosenModel.Note;
                }                

            }
            catch(Exception ex)
            {
                string msg = ex.GetType().Name + ": " + ex.Message;
                noteEntry.Text = msg;
            }
        }

        private async void Flash()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(200);
                await refreshButton.FadeTo(0, 250);
                await Task.Delay(200);
                await refreshButton.FadeTo(1, 250);
                if (refreshButton.Text == "Create New") break;                
            }
        }

    }
}