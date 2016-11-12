using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PicaPicaPoi
{
    static class AnimationHelper
    {
        static public void NavigatePage(Grid oldGrid, Grid newGrid)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.To = new Thickness(oldGrid.Margin.Left,
                -oldGrid.ActualHeight,
                oldGrid.Margin.Right,
                oldGrid.ActualHeight);
            ta.AccelerationRatio = 1;
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            ta.Completed += (s, e) =>
            {
                MainWindow m = (MainWindow)oldGrid.Parent;
                ThicknessAnimation tan = new ThicknessAnimation();
                tan.To = new Thickness(0, 0, 0, 0);
                tan.DecelerationRatio = 1;
                tan.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                m.Content = newGrid;
                newGrid.Margin = new Thickness(0, oldGrid.ActualHeight, 0, -oldGrid.ActualHeight);
                newGrid.BeginAnimation(Grid.MarginProperty, tan);
            };
            oldGrid.BeginAnimation(Grid.MarginProperty, ta);
        }
    }
}
