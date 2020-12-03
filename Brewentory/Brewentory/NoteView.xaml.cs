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
	public partial class NoteView : ContentPage
	{
        private string[] noteArray;
        private string actionName;
        private int headId;
        private Color color;

        private ObservableCollection<BrewentoryModel> noteCollection;
		public NoteView ()
		{
			InitializeComponent ();
		}

        private void NoteList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/noteviewapi");
            noteArray = JsonConvert.DeserializeObject<string[]>(json);

            noteCollection = new ObservableCollection<BrewentoryModel>();

            for(int i = 0; i < noteArray.Count(); i++)
            {
                string[] data = noteArray[i].Split(",");
                noteCollection.Add(new BrewentoryModel { HeadlineID = int.Parse(data[0]), Headline = data[1], Note = data[2] });
            }

            noteList.ItemsSource = noteCollection;

        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = noteList.SelectedItem as BrewentoryModel;
                actionName = "Edit";
                headId = item.HeadlineID;
                await Navigation.PushPopupAsync(new NotePopupView(actionName, headId, noteCollection, CreateButton));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item first.", "OK");
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = noteList.SelectedItem as BrewentoryModel;
                actionName = "Delete";
                headId = item.HeadlineID;
                await Navigation.PushPopupAsync(new NotePopupView(actionName, headId, noteCollection, CreateButton));
            }
            catch
            {
                await DisplayAlert("Oops!", "Choose an item first.", "OK");
            }
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            if(CreateButton.Text == "Create New")
            {
                color = CreateButton.BackgroundColor;
                await Navigation.PushPopupAsync(new NotePopupView("Create", 0, noteCollection, CreateButton));
            }
            else if(CreateButton.Text == "Refresh")
            {
                RefreshView();
            }           
        }

        private async void RefreshView()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://brewentory.azurewebsites.net");
            string json = await client.GetStringAsync("api/noteviewapi");
            noteArray = JsonConvert.DeserializeObject<string[]>(json);

            noteCollection = new ObservableCollection<BrewentoryModel>();

            for (int i = 0; i < noteArray.Count(); i++)
            {
                string[] data = noteArray[i].Split(",");
                noteCollection.Add(new BrewentoryModel { HeadlineID = int.Parse(data[0]), Headline = data[1], Note = data[2] });
            }

            noteList.ItemsSource = noteCollection;

            CreateButton.Text = "Create New";
            CreateButton.BackgroundColor = color;
        }
    }
}