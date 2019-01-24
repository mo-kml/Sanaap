using Android.Content;
using Android.Views;
using Sanaap.App.Droid.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(SanaapWebViewRenderer))]
namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapWebViewRenderer : WebViewRenderer
    {
        private ViewTreeObserver _viewTreeObserver;
        private Android.Webkit.WebView _webView;
        public SanaapWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            _webView = new Android.Webkit.WebView(Context);

            _viewTreeObserver = _webView.ViewTreeObserver;

            _viewTreeObserver.PreDraw += ResizeWebView;

            SetNativeControl(_webView);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            {
                Control.Touch += (object senderr, TouchEventArgs touch) =>
                {
                    touch.Handled = true;
                };
            }
        }

        private async void ResizeWebView(object sender, EventArgs e)
        {
            if (_webView != null && Element != null)
            {
                Element.HeightRequest = _webView.ContentHeight;
            }
        }
    }

}
