using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20_Project_Orion_Lead
{
    /// <summary>
    /// customControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class customControl : UserControl
    {
        public customControl()
        {
            InitializeComponent();
        }
        public ImageSource userImage
        {
            get { return UserImage.Source; }
            set { UserImage.Source = value; }
        }
        public string userName
        {
            get { return UserName.Text; }
            set { UserName.Text = value; }
        }
        public string userTitle
        {
            get { return UserTitle.Text; }
            set { UserTitle.Text = value; }
        }

    }
}
