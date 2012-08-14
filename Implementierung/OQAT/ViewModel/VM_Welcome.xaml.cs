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
            this.local("VM_Welcome_default.xml");
            projects = new ArrayList();
            
            this.setMemento(Caretaker.caretaker.getMemento( Directory.GetCurrentDirectory() +"/VM_Welcome.mem"));
            updateListBox();
        }
        private void updateListBox(){
            btnOpSelPrj.IsEnabled = false;
            listBox1.Items.Clear();
            this.projects.Reverse();
            foreach (String s in projects)
            {
               
                listBox1.Items.Add(s);
            }
            this.projects.Reverse();
        }

        
        private Memento getMemento()
        {
            int listboxLenght = 15;
            if (projects.Count > listboxLenght)
            {
                this.projects.RemoveRange(listboxLenght-1, projects.Count - listboxLenght);
            }
            Memento mem = new Memento("VM_Welcome.mem", this.projects, Directory.GetCurrentDirectory() + "/VM_Welcome.mem");
            return mem;
        }
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

        private void newPrjCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var prOpen = new VM_ProjectOpenDialog();
            prOpen.Owner = Window.GetWindow(this);
            Nullable<bool> result = prOpen.ShowDialog();
            if ((result != null) & (bool)result)
            {
                String path = prOpen.pathProject ;
               
                projects.Add(path);
                Caretaker.caretaker.writeMemento(this.getMemento());
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.newProjectCreated, new ProjectEventArgs(prOpen.project));
            }
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
            openSelectedProject();
            
        }

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
                }

                msg = t2[3];
                newPrjCreate_Button.Content = t2[2];
                btnOpSelPrj.Content = t2[1];
                btnEx.Content = t2[0];


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }

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

