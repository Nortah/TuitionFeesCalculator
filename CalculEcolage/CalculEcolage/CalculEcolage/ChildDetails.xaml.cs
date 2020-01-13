﻿using CalculEcolage.Models;
using CalculEcolage.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Label = Xamarin.Forms.Label;
using Entry = Xamarin.Forms.Entry;
using XAMLtoPDF;
using System.Windows.Controls;

namespace CalculEcolage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildDetails : ContentPage
    {
        GetNumberSep n = new GetNumberSep();
        //modifyable datas
        string devise = " CHF";
        //User interface formating
        int contentLabelSize = 18;
        int contentLabelMargin = 11;

        int StackLayoutMargin = 15;
        string year;
        Child child;



        public ChildDetails()
        {  
            InitializeComponent();
            //set styles
            var LabelStyle = new Style(typeof(Label))
            {
                Setters =
            {
                new Setter {Property = Label.MarginProperty, Value = contentLabelMargin},
                new Setter {Property = Label.FontSizeProperty, Value = contentLabelSize},
                new Setter {Property = Label.TextColorProperty, Value = Xamarin.Forms.Color.DeepSkyBlue}
            }
            };

            var EntryStyle = new Style(typeof(Entry))
            {
                Setters =
            {
                new Setter {Property = Entry.SelectionLengthProperty, Value = contentLabelMargin}
            }
            };

            MandatoryEntryBlock.Margin = StackLayoutMargin;
            OptionnalEntryBlock.Margin = StackLayoutMargin;

            labelMandatoryColumn.Margin = StackLayoutMargin;

            


        }

        protected override async void OnAppearing()
        {

            child = (Child)BindingContext;
            ChildNameValue.Text = child.childName;
            //Retrieve lists 
            List<TuitionFees> tuitions = await App.Database.GetTuittionFeesListAsync();
            List<AssistanceDiscount> assistanceDiscounts = await App.Database.GetAssistanceDiscountListAsync();
            List<TermDiscount> termDiscounts = await App.Database.GetTermDiscountListAsync();
            List<SchoolTransport> schoolTransports = await App.Database.GetSchoolTransportListAsync();
            List<Supervision> supervisions = await App.Database.GetSupervisionListAsync();
            List<Support> supports = await App.Database.GetSupportListAsync();

            // set tuition picker list
            Picker yearPicker = YearPicker as Picker;
            List<string> years = new List<string>();

            string loadedYear = child.yearName;

            tuitions = tuitions.OrderBy(i => i.yearName).ToList();
            foreach (TuitionFees tuition in tuitions)
            {
                years.Add(tuition.yearName);
            }
            years.Add("<Empty>");
            yearPicker.ItemsSource = years;
            //revover last value
            yearPicker.SelectedItem = loadedYear;

            // set assistance Discount list
            Picker assistanceDiscountPicker = AssistanceDiscountPicker as Picker;
            List<string> childrenNr = new List<string>();

            string loadedAssistanceDiscount = child.assistanceDiscountName;

            assistanceDiscounts = assistanceDiscounts.OrderBy(i => i.name).ToList();
            foreach (AssistanceDiscount assistanceDiscount in assistanceDiscounts)
            {
                childrenNr.Add(assistanceDiscount.name);
            }
            childrenNr.Add("<Empty>");
            assistanceDiscountPicker.ItemsSource = childrenNr;
            //recover last value
            assistanceDiscountPicker.SelectedItem = loadedAssistanceDiscount;

            // set Term Discount list
            Picker termDiscountPicker = TermDiscountPicker as Picker;
            List<string> paymentPlan = new List<string>();

            string loadedTermDiscount = child.termDiscountName;

            termDiscounts = termDiscounts.OrderBy(i => i.name).ToList();
            foreach (TermDiscount termDiscount in termDiscounts)
            {
                paymentPlan.Add(termDiscount.name);
            }
            paymentPlan.Add("<Empty>");
            termDiscountPicker.ItemsSource = paymentPlan;
            //recover last value
            termDiscountPicker.SelectedItem = loadedTermDiscount;


            // set zone picker list
            Picker zonePicker = ZonePicker as Picker;
            List<string> zones = new List<string>();

            schoolTransports = schoolTransports.OrderBy(i => i.zone).ToList();
            foreach (SchoolTransport schoolTransport in schoolTransports)
            {
                zones.Add(schoolTransport.zone);
            }
            zones.Add("<Empty>");
            zonePicker.ItemsSource = zones;
            //revover last value
            zonePicker.SelectedItem = child.zoneName;

            // set supervision picker list
            Picker supervisionPicker = SupervisionPicker as Picker;
            List<string> supervisionNames = new List<string>();

            supervisions = supervisions.OrderBy(i => i.name).ToList();
            foreach (Supervision supervision in supervisions)
            {
                supervisionNames.Add(supervision.name);
            }

            supervisionNames.Add("<Empty>");
            supervisionPicker.ItemsSource = supervisionNames;
            //revover last value
            supervisionPicker.SelectedItem = child.supervisionName;

            // set support picker list
            Picker supportPicker = SupportPicker as Picker;
            List<string> supportNames = new List<string>();

            supports = supports.OrderBy(i => i.name).ToList();
            foreach (Support support in supports)
            {
                supportNames.Add(support.name);
            }
            supportNames.Add("<Empty>");
            supportPicker.ItemsSource = supportNames;
            //revover last value
            supportPicker.SelectedItem = child.supportName;
            try
            {

                //Get all corresponding cost

                enrolementFeeValue.Text = n.ToNumberFormat(child.enrolementFee) + devise;
                tuitionFeesValue.Text = n.ToNumberFormat(child.tuitionFees) + devise;
                fixedChargesValue.Text = n.ToNumberFormat(child.fixedCharges) + devise;
                externalExaminationFeesValue.Text = n.ToNumberFormat(child.externalExaminationFees) + devise;
                assistanceDiscountValue.Text = n.ToNumberFormat(child.assistanceDiscount * child.tuitionFees / 100) + devise;
                termDiscountValue.Text = n.ToNumberFormat(child.termDiscount * child.tuitionFees / 100) + devise;
                schoolMealsValue.Text = n.ToNumberFormat(child.schoolMeals) + devise;
                wednesdayMealsValue.Text = n.ToNumberFormat(child.wednesdayMeals) + devise;
                schoolTransportValue.Text = n.ToNumberFormat(child.schoolTransport) + devise;
                supervisionValue.Text = n.ToNumberFormat(child.supervision) + devise;
                supportValue.Text = n.ToNumberFormat(child.support) + devise;
                

                //set checkboxes
                if(child.assistanceDiscount != 0)
                {
                    assistanceDiscountCheckBox.IsChecked = true;
                }
                if (child.termDiscount != 0)
                {
                    termDiscountCheckBox.IsChecked = true;
                }
                if (child.schoolMeals != 0)
                {
                    schoolMealsCheckBox.IsChecked = true;
                }
                if (child.wednesdayMeals != 0)
                {
                    wednesdayMealsCheckBox.IsChecked = true;
                }
                if (child.schoolTransport != 0)
                {
                    schoolTransportCheckBox.IsChecked = true;
                }
                if (child.supervision != 0)
                {
                    supervisionCheckBox.IsChecked = true;
                }
                if (child.support != 0)
                {
                    supportCheckBox.IsChecked = true;
                }

            }
            catch (Exception) { }

            //Display result
            ResultLabel.Text = n.ToNumberFormat(child.cost) + devise;
        }

        //edit the name of the child
        async void EditName(object sender, EventArgs e)
        {
            Child child = (Child)BindingContext;

            child.childName = ChildNameValue.Text;
        }
        

        //Calculate the total and display errors
        async void Button_Calculate(object sender, EventArgs e)
        {
            //set all pickers
            Picker yearPicker = YearPicker as Picker;
            Picker assistanceDiscountPicker = AssistanceDiscountPicker as Picker;
            Picker termDiscountPicker = TermDiscountPicker as Picker;
            Picker zonePicker = ZonePicker as Picker;
            Picker supervisionPicker = SupervisionPicker as Picker;
            Picker supportPicker = SupportPicker as Picker;
            //set all picked parameters
            string year;
            string zone;
            string assistanceDicountName;
            string termDiscountName;
            string supervisionName;
            string supportName;

            child = (Child)BindingContext;

            try
            {
                year = yearPicker.SelectedItem.ToString();

                if (year == "<Empty>")
                {
                    child.yearName = null;
                    child.tuitionFees = 0;
                    child.fixedCharges = 0;
                    child.externalExaminationFees = 0;
                    child.schoolMeals = 0;
                    child.wednesdayMeals = 0;
                }
                //Get all corresponding cost

                EnrolementFees enrolementFee = await App.Database.GetEnrolementFeesAsync(0);
                TuitionFees tuitionFees = await App.Database.GetTuitionFeesAsync(year);
                FixedCharges fixedCharges = await App.Database.GetFixedChargesAsync(year);
                ExternalExaminationFees examinationFees = await App.Database.GetExternalExaminationFeesAsync(year);

                //set mandatory fees Child with entries provided by user
                child.yearName = year;
                child.enrolementFee = enrolementFee.cost;
                child.tuitionFees = tuitionFees.cost;
                child.recalculatedTuitionFees = tuitionFees.cost;
                child.fixedCharges = fixedCharges.cost;
                child.externalExaminationFees = examinationFees.cost;

                // enrolementFeeValue.Text = n.ToNumberFormat() + devise;
                enrolementFeeValue.Text = n.ToNumberFormat(child.enrolementFee) + devise;
                tuitionFeesValue.Text = n.ToNumberFormat(child.tuitionFees) + devise;
                fixedChargesValue.Text = n.ToNumberFormat(child.fixedCharges) + devise;
                externalExaminationFeesValue.Text = n.ToNumberFormat(child.externalExaminationFees) + devise;

            }
            catch (Exception) { }

            // set Optionnal assistance discount
            try
            {
                assistanceDicountName = assistanceDiscountPicker.SelectedItem.ToString();
                if (assistanceDicountName == "<Empty>")
                {
                    child.assistanceDiscountName = null;
                    child.assistanceDiscount = 0;
                }
                else
                {
                    AssistanceDiscount assistanceDiscount = await App.Database.GetAssistanceDiscountAsync(assistanceDicountName);
                    child.assistanceDiscount = assistanceDiscount.discountPercentage;
                    child.assistanceDiscountName = assistanceDicountName;
                }
                assistanceDiscountValue.Text = n.ToNumberFormat(child.assistanceDiscount * child.tuitionFees / 100) + devise;
                child.recalculatedTuitionFees = child.tuitionFees * (100 + child.assistanceDiscount) / 100;


            }
            catch (Exception) { }

            // set Optionnal term discount
            try
            {
                termDiscountName = termDiscountPicker.SelectedItem.ToString();
                if (termDiscountName == "<Empty>")
                {
                    child.termDiscountName = null;
                    child.termDiscount = 0;
                }
                else
                {
                    TermDiscount termDiscount = await App.Database.GetTermDiscountAsync(termDiscountName);
                    child.termDiscount = termDiscount.discountPercentage;
                    child.termDiscountName = termDiscountName;
                }
                termDiscountValue.Text = n.ToNumberFormat(child.termDiscount * child.tuitionFees / 100) + devise;
                child.recalculatedTuitionFees = child.recalculatedTuitionFees * (100 + child.termDiscount) / 100;


            }
            catch (Exception) { }

            // set Optionnal school meal
            try
            {
                if (schoolMealsCheckBox.IsChecked)
                {
                    year = yearPicker.SelectedItem.ToString();
                    SchoolMeals schoolMeals = await App.Database.GetSchoolMealsAsync(year);
                    child.schoolMeals = schoolMeals.cost;

                }
                else { child.schoolMeals = 0; }
                schoolMealsValue.Text = n.ToNumberFormat(child.schoolMeals) + devise;

            }
            catch (Exception) { }

            // set Optionnal wednesday meal
            try
            {
                if (wednesdayMealsCheckBox.IsChecked)
                {
                    year = yearPicker.SelectedItem.ToString();
                    WednesdayMeals wednesdayMeals = await App.Database.GetWednesdayMealsAsync(year);
                    child.wednesdayMeals = wednesdayMeals.cost;

                }
                else { child.wednesdayMeals = 0; }
                wednesdayMealsValue.Text = n.ToNumberFormat(child.wednesdayMeals) + devise;

            }
            catch (Exception) { }

            // set Optionnal school transport
            try
            {
                zone = zonePicker.SelectedItem.ToString();
                if (zone == "<Empty>")
                {
                    child.zoneName = null;
                    child.schoolTransport = 0;
                }
                else
                {
                    SchoolTransport schoolTransport = await App.Database.GetSchoolTransportAsync(zone);
                    child.schoolTransport = schoolTransport.cost;
                    child.zoneName = zone;
                }
                schoolTransportValue.Text = n.ToNumberFormat(child.schoolTransport) + devise;

            }
            catch (Exception) { }

            // set Optionnal supervision 
            try
            {
                supervisionName = supervisionPicker.SelectedItem.ToString();
                if (supervisionName == "<Empty>")
                {
                    child.supervisionName = null;
                    child.supervision = 0;
                }
                else
                {
                    Supervision supervision = await App.Database.GetSupervisionAsync(supervisionName);
                    child.supervision = supervision.cost;
                    child.supervisionName = supervisionName;
                }
                supervisionValue.Text = n.ToNumberFormat(child.supervision) + devise;

            }
            catch (Exception) { }
            // set Optionnal support transport
            try
            {
                supportName = supportPicker.SelectedItem.ToString();
                if (supportName == "<Empty>")
                {
                    child.supportName = null;
                    child.support = 0;
                }
                else
                {
                        Support support = await App.Database.GetSupportAsync(supportName);
                        child.support = support.cost;
                        child.supportName = supportName;
                }
                supportValue.Text = n.ToNumberFormat(child.support) + devise;
            }
            catch (Exception) { }
            child.childName = ChildNameValue.Text; //set the child name if it has been modified
            //set total child cost
            child.cost = (child.fixedCharges + child.recalculatedTuitionFees + child.enrolementFee + child.externalExaminationFees + child.schoolMeals + child.schoolTransport + child.supervision + child.support);
            Family family = await App.Database.GetFamilyByIdAsync(child.familyId);
            family.totalCost = family.totalCost + child.cost;
            await App.Database.SaveFamilyAsync(family);
            //save child in database
            await App.Database.SaveChildAsync(child);

            //Display result
            ResultLabel.Text = n.ToNumberFormat(child.cost) + devise;

           
        }

        async void Button_Delete(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete Child ?", "Do you really want to delete this child?", "Yes", "No");
            if(answer == true)
            {
                child = (Child)BindingContext;
                Family family = await App.Database.GetFamilyByIdAsync(child.familyId);
                family.totalCost = family.totalCost - child.cost;
                await App.Database.SaveFamilyAsync(family);
                await App.Database.DeleteChildAsync(child);
                //return to child List
                await Navigation.PushAsync(new ChildList
                {
                    BindingContext = family
                });
            }
            

        }
        //on back clicked
        async void OnBackClicked(object sender, EventArgs e)
        {
            child = (Child)BindingContext;
            child.childName = ChildNameValue.Text; //set the child name if it has been modified
            //save child in database
            await App.Database.SaveChildAsync(child);
            Family family = await App.Database.GetFamilyByIdAsync(child.familyId);
            await Navigation.PushAsync(new ChildList
            {
                BindingContext = family
            });
        }
        //Transfer image
        async void OnTransferImageClicked(object sender, EventArgs e)
        {
            //scroll to the top of the page
            await ChildScrollView.ScrollToAsync(0, 0, false).ConfigureAwait(false);

            child = (Child)BindingContext;
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add page to the PDF document
            PdfPage page = document.Pages.Add();
            //Create graphics instance
            PdfGraphics graphics = page.Graphics;
            Stream imageStream = null;

            //Captures the XAML page as image and returns the image in memory stream
            imageStream = new MemoryStream(DependencyService.Get<IScreenshotService>().Capture());

            //Load the image in PdfBitmap
            PdfBitmap image = new PdfBitmap(imageStream);

            
            //scroll to the second page
            await ChildScrollView.ScrollToAsync(0, 940, false).ConfigureAwait(true);


            //Add page to the PDF document
            PdfPage page2 = document.Pages.Add();
            //Create graphics instance
            PdfGraphics graphics2 = page2.Graphics;
            imageStream = new MemoryStream(DependencyService.Get<IScreenshotService>().Capture());
            PdfBitmap image2 = new PdfBitmap(imageStream);


            //Set layout property to make the element break across the pages

            PdfLayoutFormat format = new PdfLayoutFormat();

            format.Break = PdfLayoutBreakType.FitPage;

            format.Layout = PdfLayoutType.Paginate;

            //Draw the image to the page
            graphics.DrawImage(image, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);
            graphics2.DrawImage(image2, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);


            //Save the document into memory stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            //Save the stream as a file in the device and invoke it for viewing
            DependencyService.Get<ISave>().Save("TuitionFees.pdf", "application/pdf", stream);
        }

        //Check if the student is primary or secondary and remove the uneeded fields in supervision picker
        private async void CheckClass(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Picker yearPicker = YearPicker as Picker;
            Picker supervisionPicker = SupervisionPicker as Picker;
            List<Supervision> supervisions = await App.Database.GetSupervisionListAsync();
            List<string> supervisionNames = new List<string>();

            supervisions = supervisions.OrderBy(i => i.name).ToList();
            foreach (Supervision supervision in supervisions)
            {
                supervisionNames.Add(supervision.name);
            }

            try
            {

                year = yearPicker.SelectedItem.ToString();
                if (year == "MYP3" || year == "MYP4" || year == "MYP5" || year == "DP1" || year == "DP2")
                {
                    //Remove the followin activities for each secondary year
                    supervisionNames.Remove(supervisionNames[6]);
                    supervisionNames.Remove(supervisionNames[5]);

                }

                supervisionPicker.ItemsSource = supervisionNames;

                if(supervisionPicker.SelectedItem is null)
                {
                    //revover last value
                    supervisionPicker.SelectedItem = child.supervisionName;
                }

            }
            catch (Exception) { }
            
        }
    }
    
}