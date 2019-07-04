﻿using OpenRPA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Image
{
    public class ImageElement : IElement
    {
        public string Name { get; set; }
        public string Processname { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public System.Drawing.Bitmap element { get; set; }
        public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IElement.RawElement { get => element; set => element = value as System.Drawing.Bitmap; }
        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return new System.Drawing.Rectangle(X, Y, Width, Height);
            }
        }
        public ImageElement(System.Drawing.Rectangle Rectangle)
        {
            X = Rectangle.X;
            Y = Rectangle.Y;
            Width = Rectangle.Width;
            Height = Rectangle.Height;
        }
        public void Click(bool VirtualClick, Input.MouseButton Button, int OffsetX, int OffsetY)
        {
            //Task.Run(() =>
            //{
            //});
            //Log.Debug("MouseMove to " + Rectangle.X + "," + Rectangle.Y + " and click");
            //Input.InputDriver.Instance.MouseMove(Rectangle.X + OffsetX, Rectangle.Y + OffsetY);
            //Input.InputDriver.DoMouseClick();
            // var point = new FlaUI.Core.Shapes.Point(Rectangle.X + OffsetX, Rectangle.Y + OffsetY);
            //FlaUI.Core.Input.Mouse.MoveTo(point);

            OpenRPA.Input.InputDriver.SetCursorPos(Rectangle.X + OffsetX, Rectangle.Y + OffsetY);
            //FlaUI.Core.Input.MouseButton flabuttun = FlaUI.Core.Input.MouseButton.Left;
            //if (Button == Input.MouseButton.Middle) flabuttun = FlaUI.Core.Input.MouseButton.Middle;
            //if (Button == Input.MouseButton.Right) flabuttun = FlaUI.Core.Input.MouseButton.Right;

            OpenRPA.Input.InputDriver.Click(Button);
            // FlaUI.Core.Input.Mouse.Click(flabuttun);
            // FlaUI.Core.Input.Mouse.Click(flabuttun, point);
            //Log.Debug("Click done");
            return;
        }
        public void Focus()
        {
            throw new NotImplementedException();
        }
        public Task Highlight(bool Blocking, System.Drawing.Color Color, TimeSpan Duration)
        {
            if (!Blocking)
            {
                Task.Run(() => _Highlight(Color, Duration));
                return Task.CompletedTask;
            }
            return _Highlight(Color, Duration);
        }
        public Task _Highlight(System.Drawing.Color Color, TimeSpan Duration)
        {
            using (Interfaces.Overlay.OverlayWindow _overlayWindow = new Interfaces.Overlay.OverlayWindow())
            {
                _overlayWindow.Visible = true;
                _overlayWindow.SetTimeout(Duration);
                _overlayWindow.Bounds = Rectangle;
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                do
                {
                    System.Threading.Thread.Sleep(10);
                    _overlayWindow.TopMost = true;
                } while (_overlayWindow.Visible && sw.Elapsed < Duration);
                return Task.CompletedTask;
            }
        }
        public string ImageString()
        {
            var AddedWidth = 10;
            var AddedHeight = 10;
            var ScreenImageWidth = Rectangle.Width + AddedWidth;
            var ScreenImageHeight = Rectangle.Height + AddedHeight;
            var ScreenImagex = Rectangle.X - (AddedWidth / 2);
            var ScreenImagey = Rectangle.Y - (AddedHeight / 2);
            if (ScreenImagex < 0) ScreenImagex = 0; if (ScreenImagey < 0) ScreenImagey = 0;
            using (var image = Interfaces.Image.Util.Screenshot(ScreenImagex, ScreenImagey, ScreenImageWidth, ScreenImageHeight, Interfaces.Image.Util.ActivityPreviewImageWidth, Interfaces.Image.Util.ActivityPreviewImageHeight))
            {
                // Interfaces.Image.Util.SaveImageStamped(image, System.IO.Directory.GetCurrentDirectory(), "ImageElement");
                return Interfaces.Image.Util.Bitmap2Base64(image);
            }
        }

    }
}
