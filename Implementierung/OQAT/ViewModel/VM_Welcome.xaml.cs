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
    using System.Xml;
    using System.Threading;

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
        /// Constructor
        /// </summary>
        /// 
        ArrayList projects;
        String msg = "Projekt nicht gefunden.";

        /// <summary>
        /// Constructor
        /// </summary>
        public VM_Welcome()
        {
            InitializeComponent();
            this.local("VM_Welcome_" + Thread.CurrentThread.CurrentCulture + ".xml");
            projects = new ArrayList();
            this.setMemento(Caretaker.caretaker.getMemento( Directory.GetCurrentDirectory() +"/VM_Welcome.mem"));
            updateListBox();
        }

        /// <summary>
        /// Helper method to set the listbox content to the projects attribute.
        /// </summary>
        private void updateListBox(){
            //btnOpSelPrj.IsEnabled = false;
            listBox1.Items.Clear();
            this.projects.Reverse();
            foreach (String s in projects)
            {
               
                listBox1.Items.Add(s);
            }
            this.projects.Reverse();
        }

        /// <summary>
        /// Returns the current state as a Memento Object
        /// </summary>
        
        private Memento getMemento()
        {
            int listboxLength = 15;
            if (projects.Count > listboxLength)
            {
                this.projects.RemoveRange(0, projects.Count - listboxLength);
            }
            Memento mem = new Memento("VM_Welcome.mem", this.projects, Directory.GetCurrentDirectory() + "/VM_Welcome.mem");
            return mem;
        }

        /// <summary>
        /// Sets mem as current state
        /// </summary>
        private void setMemento(Memento mem)
        {
            if (mem != null)
            {
                var obj = (ArrayList)mem.state;
                if (obj != null)
                {
                    this.projects = obj;
                }
            } 
        }

        /// <summary>
        /// Event to the new Project create Button. Calls the VM_projectopendialog to get the values.
        /// </summary>
        private void newPrjCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            createProject();
        }

        /// <summary>
        /// Opens a dialog for creating a new project
        /// </summary>
        public void createProject()
        {
            var prOpen = new VM_ProjectOpenDialog();
            prOpen.Owner = Window.GetWindow(this);
            Nullable<bool> result = prOpen.ShowDialog();
            if ((result != null) & (bool)result)
            {
                String path = prOpen.pathProject;
                addProjekt(projects.Contains(path), path);
                Caretaker.caretaker.writeMemento(this.getMemento());
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated, new ProjectEventArgs(prOpen.project));
            }
        }

        /// <summary>
        /// Opens a file dialog for opening a project
        /// </summary>
        public void openProject()
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
                addProjekt(projects.Contains(dlg.FileName), dlg.FileName);
            }
        }

        /// <summary>
        /// Event to the browse button to open a dialog to search for existing projects.
        /// </summary>
        private void exPrjOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            openProject();
        }

        /// <summary>
        /// Event to enable the btnOpSelPrj button.
        /// </summary>
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //btnOpSelPrj.IsEnabled = true;
        }

        /// <summary>
        /// Event for clicking the btnOpSelPrj button. Tries to open the selected project.
        /// </summary>
        private void btnOpSelPrj_Click(object sender, RoutedEventArgs e)
        {
            openSelectedProject();
            
        }
        /// <summary>
        /// Helper method to open a project from the listbox.
        /// </summary>
        private void openSelectedProject()
        {
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

        /// <summary>
        /// Helper method to add projects to the memento
        /// </summary>
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

        /// <summary>
        /// Event to enable the btnOpSelPrj button.
        /// </summary>
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //btnOpSelPrj.IsEnabled = true;
        }

        /// <summary>
        /// Localize the language to the xml file with the name from s.
        /// </summary>
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[4];
                String[] t2 = new String[4];
                for (int i = 0; i < 4; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                }
                msg = t2[3];
                newPrjCreate_Button.Content = t2[2];
                //btnOpSelPrj.Content = t2[1];
                btnEx.Content = t2[0];
            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }

        /// <summary>
        /// Event to open projects with a double click
        /// </summary>
        private void listbox1_DoubleKlick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedItem.ToString().Length != 0)
                {
                    openSelectedProject();
                }
            }
        }
    }
}

