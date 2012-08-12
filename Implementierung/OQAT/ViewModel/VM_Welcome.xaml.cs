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
        public VM_Welcome()
        {
            InitializeComponent();
            projects = new String[15];
            //memento null referenz exception
          //  this.setMemento(Caretaker.caretaker.getMemento( Directory.GetCurrentDirectory() +"/VM_Welcome.mem"));
            foreach(String s in projects){
                listBox1.Items.Add(s);
            }
        }

        String[] projects;
        private Memento getMemento()
        {
            Memento mem = new Memento("VM_Welcome", this.projects);
            return mem;
        }
        private void setMemento(Memento mem)
        {
            var obj=(String[])mem.state;
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
            Project exPrj = exPrjMem.state as Project;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated,
                new ProjectEventArgs(exPrj));
        }

    }
}

