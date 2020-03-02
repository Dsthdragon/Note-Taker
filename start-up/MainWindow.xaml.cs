using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace start_up
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private NoteRepository noteRepository = new NoteRepository();


        public MainWindow()
        {
            InitializeComponent();
            this.StartUpPage.mainWindow = this;
        }

        private void Open_About_Dialog(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            this.StartUpPage.SetNote(null);
        }

        private void StackPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Note note = ((StackPanel)sender).DataContext as Note;
            Debug.WriteLine(note);
            this.StartUpPage.SetNote(note);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetNotes();
        }

        public void SetNotes()
        {
            using (var db = new ApplicationDBContext())
            {
                this.SideBar.ItemsSource = db.Notes.ToList();
            }
        }
    }
}
