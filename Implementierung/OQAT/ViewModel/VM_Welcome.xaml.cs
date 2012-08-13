namespace Oqat.ViewModel
{
    using Oqat.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;
    using System.Windows;
    using System.Collections;
    using Microsoft.Win32;

    /// <summary>
    /// This component is displayed whenever no project is open.
    /// </summary>
    public partial class VM_Welcome : UserControl
    {
        /// <summary>
        /// A list of recently used projects.
        /// </summary>
        public virtual Memento[] lastUsedProjects
        {
            get;
            set;
        }
        /// <summary>
        /// This delegate will be called if the user chooses a project from the "Recently used projects" List and
        /// is responsible for initializing the choosen project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onSelectProject(object sender, EventArgs e) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        ArrayList projects;
        String msg = "Projekt nicht gefunden.";
        public VM_Welcome()
        {
            InitializeComponent();
            projects = new ArrayList();
            //this.setMemento(Caretaker.caretaker.getMemento( Directory.GetCurrentDirectory() +"/VM_Welcome.mem"));
            updateListBox();
        }
        private void updateListBox(){
            foreach (String s in projects)
            {
                btnOpSelPrj.IsEnabled = false;
                listBox1.Items.Clear();
                listBox1.Items.Add(s);
            }
        }

        
        private Memento getMemento()
        {
            int listboxLenght = 15;
            if (projects.Count > listboxLenght)
            {
                this.projects.RemoveRange(listboxLenght-1, projects.Count - listboxLenght);
            }
            Memento mem = new Memento("VM_Welcome.mem", this.projects);
            return mem;
        }
        private void setMemento(Memento mem)
        {
            var obj=(ArrayList)mem.state;
            if (obj != null)
            {
                this.projects = obj;
            }
            
        }

        private void newPrjCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var prOpen = new VM_ProjectOpenDialog();
            prOpen.Owner = Window.GetWindow(this);
            Nullable<bool> result = prOpen.ShowDialog();
            if ((result != null) & (bool)result)
                projects.Add(prOpen.pathProject);
            Caretaker.caretaker.writeMemento(this.getMemento());
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated, new ProjectEventArgs(prOpen.project));
        }

        private void exPrjOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".oqatPrj";
            dlg.Filter = "OQAT projects (.oqatPrj)|*.oqatPrj";

            Nullable<bool> result = dlg.ShowDialog();
            Memento exPrjMem;
            if (result == true)
            {
                exPrjMem = Caretaker.caretaker.getMemento(dlg.FileName);
                Project exPrj = exPrjMem.state as Project;
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated,
                    new ProjectEventArgs(exPrj));
                addProjekt(projects.Contains(dlg.FileName),dlg.FileName);
               
            }
          
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpSelPrj.IsEnabled = true;
        }

        private void btnOpSelPrj_Click(object sender, RoutedEventArgs e)
        {
            //fehlerbehandlung
            String projectToOpnen = this.listBox1.SelectedItem.ToString();
            Memento exPrjMem;
            exPrjMem = Caretaker.caretaker.getMemento(projectToOpnen);
            if (exPrjMem != null)
            {
                Project exPrj = exPrjMem.state as Project;
                addProjekt(projects.Contains(projectToOpnen), projectToOpnen);

                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated,
                    new ProjectEventArgs(exPrj));
            }
            else
            {
                MessageBox.Show(msg);
                
                projects.Remove(projectToOpnen);
                listBox1.Items.Clear();
                updateListBox();
            }
        }


        private void addProjekt(Boolean b, String s)
        {
            if (!b)
            {
                projects.Add(s);
                Caretaker.caretaker.writeMemento(this.getMemento());
            }
            else
            {
                projects.Remove(s);
                projects.Add(s);
                Caretaker.caretaker.writeMemento(this.getMemento());
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpSelPrj.IsEnabled = true;
        }

    }
}

