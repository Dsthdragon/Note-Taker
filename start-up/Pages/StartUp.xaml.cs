using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
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

namespace start_up.Pages
{
    /// <summary>
    /// Interaction logic for StartUp.xaml
    /// </summary>
    public partial class StartUp : Page
    {
        private NoteRepository noteRepository = new NoteRepository();

        private Note note;

        public MainWindow mainWindow;


        public StartUp()
        {
            InitializeComponent();
        }

        public void SetNote(Note note)
        {
            this.note = note;
            this.noteTextBox.Text = this.note == null ? "" : this.note.Content;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.noteTextBox.Text.Length > 0)
            {
                if (note == null)
                    this.createNote();
                else
                    this.updateNote();
            } else {
                MessageBox.Show("Cannot create Empty Note");
            }
        }

        private void createNote()
        {

            try
            {

            using (var db = new ApplicationDBContext())
            {
                Note item = new Note() { Content = this.noteTextBox.Text };
                db.Notes.Add(item);
                db.SaveChanges();
                this.mainWindow.SetNotes();
            }
            } catch(DbUpdateException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
            
        }

        private async Task updateNote()
        {
            try
            {

                using (var db = new ApplicationDBContext())
                {
                    Note item = new Note() { Content = this.noteTextBox.Text };
                    this.note.Content = this.noteTextBox.Text;
                    db.Notes.Update(this.note);
                    db.SaveChanges();
                    this.mainWindow.SetNotes();
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        //private async Task createNote()
        //{
        //    PostNote note = new PostNote();
        //    note.Content = this.noteTextBox.Text;
        //    this.note = await this.noteRepository.create(note);
        //    MessageBox.Show("Post Created");
        //    note.Content = "";
        //}

        //private async Task updateNote()
        //{
        //    PostNote note = new PostNote();
        //    note.Content = this.noteTextBox.Text;
        //    PostResponse x = await this.noteRepository.update(note,  this.note);
        //    MessageBox.Show("Post Updated");
        //}
    }
}
