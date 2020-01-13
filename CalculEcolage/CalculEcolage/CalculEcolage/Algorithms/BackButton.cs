using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace CalculEcolage.Algorithms
{
    public class BackButton: UIViewController
    {
        public override void ViewWillAppear(bool animated)
        {

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetHidesBackButton(true, false);
        }
    }
}
