
using CalculEcolage.Database;
using CalculEcolage.Models;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Essentials;

namespace CalculEcolage
{

    public partial class MainPage : ContentPage
    {

        Color backgroundColour = Color.DeepSkyBlue;
        Color textColor = Color.White;
        double fontSize = 30;
        Thickness margin = new Thickness();
        Thickness padding = new Thickness();
        //Layout
        LayoutOptions horizontalOptions = LayoutOptions.Center;
        LayoutOptions verticalOptions = LayoutOptions.Center;
        //Padding
        int verticalPadding = 10;
        int horizontalPadding = 15;
        //Height
        int Width = 400;
        int Height = 100;

        

        public MainPage()
        {
            InitializeComponent();
            padding.Left = horizontalPadding;
            padding.Right = horizontalPadding;
            padding.Top = verticalPadding;
            padding.Bottom = verticalPadding;
            margin.Bottom = 60;
            //set buttons properties
            SetParametersButton.BackgroundColor = backgroundColour;
            SetParametersButton.TextColor = textColor;
            SetParametersButton.FontSize = fontSize;
            SetParametersButton.Margin = margin;
            SetParametersButton.HorizontalOptions = horizontalOptions;
            SetParametersButton.VerticalOptions = verticalOptions;
            SetParametersButton.HeightRequest = Height;
            SetParametersButton.WidthRequest = Width;
            SetParametersButton.Padding = padding;

            FamilyButton.BackgroundColor = backgroundColour;
            FamilyButton.TextColor = textColor;
            FamilyButton.FontSize = fontSize;
            FamilyButton.Margin = margin;
            FamilyButton.HorizontalOptions = horizontalOptions;
            FamilyButton.VerticalOptions = verticalOptions;
            FamilyButton.HeightRequest = Height;
            FamilyButton.WidthRequest = Width;
            FamilyButton.Padding = padding;
            
        }
        protected override async void OnAppearing()
        {
            //override back button
            if (OnBackButtonPressed())
            {
                await Navigation.PushAsync(new MainPage
                {
                    BindingContext = new MainPage()
                });
            }

            base.OnAppearing();
            InitializeDatabase ini = new InitializeDatabase();
            //use script to fill database if previous database has been deleted
            List<TuitionFees> tuitionFees = await App.Database.GetTuittionFeesListAsync();
            if (tuitionFees.Count() == 0)
            {
                ini.populateDatabase();
            }
        }

            async private void GoToSetParameters(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrUpdate
            {
                BindingContext = new AddOrUpdate()
            });
        }

        async private void GoToFamilyList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FamilyList
            {
                BindingContext = new FamilyList()
            });
        }

        async private void ImportDB(object sender, EventArgs e)
        {
            FileData file = await CrossFilePicker.Current.PickFile();
            if(file != null)
            {
                string filePath = file.FilePath;
                var bytes = File.ReadAllBytes(filePath);
                File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fees20.db3"), bytes); //Replace the current database with the new database
            }
            

        }

        async private void ExportDB(object sender, EventArgs e)
        {
            ExperimentalFeatures.Enable("ShareFileRequest_Experimental");
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fees20.db3");
            //open the Share panel 
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                File = new ShareFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fees20.db3"))
            });
        }
    }


}
