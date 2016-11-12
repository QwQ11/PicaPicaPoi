using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using static PicaPicaPoi.ThemeHelper;

namespace PicaPicaPoi
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static List<Swatch> SwatchList { get; } = new SwatchesProvider().Swatches.ToList();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ChangeTheme();
        }
        public static void ChangeTheme()
        {
            ApplyPrimary((from item in SwatchList where item.Name == "blue" select item).FirstOrDefault());
        }
    }
}
