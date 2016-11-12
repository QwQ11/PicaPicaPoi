using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.IO;

namespace PicaPicaPoi.Controls
{
    /// <summary>
    /// RecommendItem.xaml 的交互逻辑
    /// </summary>
    public partial class RecommendItem : UserControl
    {
        string IllustId { get; set; }
        ImageSource Source
        {
            get
            {
                return image.Source;
            }
            set
            {
                image.Source = value;
            }
        }
        public RecommendItem(string illustId, string imageUrl)
        {
            InitializeComponent();
            IllustId = illustId;
            
            var wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Referer, "http://spapi.pixiv.net/");
            wc.DownloadDataAsync(new Uri(imageUrl));
            wc.DownloadDataCompleted += (s, e) =>
            {
                var ms = new MemoryStream(e.Result);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                this.image.Source = image;
            };
        }
        private async void like_OnClick(bool isLike)
        {
            if(isLike)
                await ApiManager.Api.LikeIllustAsync(IllustId);
            else
                await ApiManager.Api.UnlikeIllustAsync(IllustId);
        }
    }
}
