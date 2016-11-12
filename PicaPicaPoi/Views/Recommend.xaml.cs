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
using PixivApi;

namespace PicaPicaPoi.Views
{
    /// <summary>
    /// Recommend.xaml 的交互逻辑
    /// </summary>
    public partial class Recommend : Grid
    {
        public Recommend()
        {
            InitializeComponent();
            illustList.OnScrolledToBottom += (s, e) => LoadNew();
            LoadNew();
        }
        internal async void LoadNew()
        {
            var illusts = await ApiManager.Api.GetRecommendAsync();
            foreach(Illust i in illusts)
            {
                illustList.Add(i);
            }
        }
    }
}
