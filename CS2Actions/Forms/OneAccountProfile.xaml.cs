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
using System.Windows.Shapes;

namespace CS2Actions
{
    /// <summary>
    /// Interaction logic for OneAccountProfile.xaml
    /// </summary>
    public partial class OneAccountProfile : Window
    {
        CS2Profile CSProfile;
        public OneAccountProfile(CS2Profile csProfile)
        {
            InitializeComponent();
            CSProfile = csProfile;
            LoginLabel.Content = csProfile.Login;
            CurrectLevelLabel.Content = "Level: " + csProfile.CS2_Level;
            ProgressLevel.Value = csProfile.CS2_CurrectXP;
            CurrectEXPLabel.Content = csProfile.CS2_CurrectXP + $"[{5000 - csProfile.CS2_CurrectXP}]";
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.GetPosition(this).Y < 30)
                {
                    try
                    {
                        DragMove();
                    }
                    catch (Exception ex)
                    { }
                }
            }

        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
