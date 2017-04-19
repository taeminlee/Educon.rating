using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GoogleDocsCrawler
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Doc> docs = new ObservableCollection<Doc>();

        public MainWindow()
        {
            InitializeComponent();
            this.dgDocs.ItemsSource = docs;
        }

        private void dgDocs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Doc;
            if (item != null)
            {
                /*if (item.FileId != "" && item.FileId != null && item.IsDownloaded == false)
                {
                    item.Download();
                }*/
                if(item.IsDownloaded == true)
                {
                    txt.Text = item.Txt;
                }
            }
        }

        private void dgDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var item = e.AddedItems[0] as Doc;
                if (item != null)
                {
                    if (item.IsDownloaded == true)
                    {
                        txt.Text = item.Txt;
                    }
                }
            }
        }
    }
}
