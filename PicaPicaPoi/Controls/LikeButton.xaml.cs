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
using System.Windows.Media.Animation;

namespace PicaPicaPoi.Controls
{
    /// <summary>
    /// LikeButton.xaml 的交互逻辑
    /// </summary>
    public partial class LikeButton : UserControl
    {
        public LikeButton()
        {
            InitializeComponent();
        }

        public event ClickHeander OnClick;
        public delegate void ClickHeander(bool isLike);
        public bool IsClicked { get; internal set; }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var targetColor = (Color)ColorConverter.ConvertFromString("#F44336");
            if (IsClicked) {
                targetColor = Colors.Gray;
            }

            IsClicked = !IsClicked;
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(5);
            ta.Duration = new Duration(TimeSpan.FromSeconds(.1));
            ta.AccelerationRatio = 1;
            ta.Completed += Ta_Completed;

            ColorAnimation ca = new ColorAnimation();
            ca.To = targetColor;
            ca.Duration = new Duration(TimeSpan.FromSeconds(.1));
            
            SolidColorBrush tmp = new SolidColorBrush(Colors.Gray);
            path.Fill = tmp;

            vBox.BeginAnimation(Grid.MarginProperty, ta);
            tmp.BeginAnimation(SolidColorBrush.ColorProperty, ca);

            OnClick?.Invoke(IsClicked);
        }

        private void Ta_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(7);
            ta.Duration = new Duration(TimeSpan.FromSeconds(.1));
            ta.DecelerationRatio = 1;
            vBox.BeginAnimation(Grid.MarginProperty, ta);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsClicked) return;
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(10);
            ta.Duration = new Duration(TimeSpan.FromSeconds(.1));
            ta.AccelerationRatio = 1;
            vBox.BeginAnimation(Grid.MarginProperty, ta);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(7);
            ta.Duration = new Duration(TimeSpan.FromSeconds(.1));
            ta.AccelerationRatio = 1;
            vBox.BeginAnimation(Grid.MarginProperty, ta);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(8);
            ta.Duration = new Duration(TimeSpan.FromSeconds(.1));
            ta.AccelerationRatio = 1;
            vBox.BeginAnimation(Grid.MarginProperty, ta);
        }
    }
}
