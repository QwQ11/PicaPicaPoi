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
using System.Windows.Media.Animation;

namespace PicaPicaPoi.Controls
{
    /// <summary>
    /// RecommendItem.xaml 的交互逻辑
    /// </summary>
    public partial class RecommendItem : UserControl
    {
        string IllustId { get; set; }
        string Title
        {
            get
            {
                return title.Text;
            }
            set
            {
                title.Text = value;
            }
        }
        string Description
        {
            get
            {
                return description.Text;
            }
            set
            {
                description.Text = value;
            }
        }
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
        public RecommendItem(string illustId, string imageUrl, string title, string desc)
        {
            InitializeComponent();
            IllustId = illustId;
            Title = title;
            Description = desc;
            
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

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(.2));
            mash.BeginAnimation(Grid.OpacityProperty, da);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(.2));
            mash.BeginAnimation(Grid.OpacityProperty, da);
        }

        private void mash_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.pixiv.net/member_illust.php?mode=medium&illust_id=" + IllustId);
        }
    }
}
