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
using MaterialDesignThemes.Wpf;
using PixivApi;

namespace PicaPicaPoi.Controls
{
    /// <summary>
    /// IllustList.xaml 的交互逻辑
    /// </summary>
    public partial class IllustList : UserControl
    {
        public event EventHandler OnScrolledToBottom;

        List<RecommendItem> t = new List<RecommendItem>();
        public IllustList()
        {
            InitializeComponent();
        }
        public void Add(string illustId, string imageUrl, string title, string desc)
        {
            RecommendItem item = new RecommendItem(illustId, imageUrl, title, desc);
            item.Height = 150;
            item.Width = 150;
            t.Add(item);
            item.Margin = new Thickness(2);

            if(t.Count % 2 == 0)
                content2.Children.Add(item);
            else
                content1.Children.Add(item);
        }
        public void Add(Illust illust)
        {
            this.Add(illust.Id, illust.ImageUrl, illust.Title, illust.Description);
        }
        
        private bool IsVerticalScrollBarAtButtom
        {
            get
            {
                bool isAtButtom = false;
                double dVer = sView.VerticalOffset;
                double dViewport = sView.ViewportHeight;
                double dExtent = sView.ExtentHeight;
                if (dVer != 0)
                {
                    if (dVer + dViewport == dExtent)
                    {
                        isAtButtom = true;
                    } else
                    {
                        isAtButtom = false;
                    }
                } else
                {
                    isAtButtom = false;
                }
                if (sView.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled
                    || sView.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
                {
                    isAtButtom = true;
                }

                return isAtButtom;
            }
        }

        private void sView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (IsVerticalScrollBarAtButtom)
            {
                OnScrolledToBottom?.Invoke(this, null);
            }
        }
    }
}
