﻿using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenRPA.Image
{
    public partial class GetElementDesigner : INotifyPropertyChanged
    {
        public GetElementDesigner()
        {
            InitializeComponent();
            // HighlightImage = Extensions.GetImageSourceFromResource("search.png");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public BitmapFrame HighlightImage { get; set; }
        private async void btn_Select(object sender, RoutedEventArgs e)
        {
            Interfaces.GenericTools.minimize(Interfaces.GenericTools.mainWindow);
            var rect = await getrectangle.GetitAsync();

            var _image = new System.Drawing.Bitmap(rect.Width, rect.Height);
            var graphics = System.Drawing.Graphics.FromImage(_image as System.Drawing.Image);
            graphics.CopyFromScreen(rect.X, rect.Y, 0, 0, _image.Size);
            ModelItem.Properties["Image"].SetValue(Interfaces.Image.Util.Bitmap2Base64(_image));
            NotifyPropertyChanged("Image");
            var element = AutomationHelper.GetFromPoint(rect.X, rect.Y);

            if (element != null)
            {
                var p = System.Diagnostics.Process.GetProcessById(element.ProcessId);
                var Processname = p.ProcessName;
                ModelItem.Properties["Processname"].SetValue(new System.Activities.InArgument<string>(Processname));
            }
            Interfaces.GenericTools.restore();

        }
        private void Highlight_Click(object sender, RoutedEventArgs e)
        {
        }
        public string ImageString
        {
            get
            {
                string result = string.Empty;
                result = ModelItem.GetValue<string>("Image");
                return result;
            }
        }
        public BitmapImage Image
        {
            get
            {
                var base64 = ImageString;
                if (string.IsNullOrEmpty(base64)) return null;
                //if (System.Text.RegularExpressions.Regex.Match(base64, "[a-f0-9]{24}").Success)
                //{
                //    return image.Screenutil.BitmapToImageSource(image.util.loadWorkflowImage(base64), image.Screenutil.ActivityPreviewImageWidth, image.Screenutil.ActivityPreviewImageHeight);
                //}

                // return OpenRPA.Interfaces.Image.Util.BitmapToImageSource
                using (var b = Interfaces.Image.Util.Base642Bitmap(base64))
                {
                    return Interfaces.Image.Util.BitmapToImageSource(b, Interfaces.Image.Util.ActivityPreviewImageWidth, Interfaces.Image.Util.ActivityPreviewImageHeight);
                }
            }
        }
    }
}