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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tekla.Structures.Dialog;

namespace TestWPFBeamPlugin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PluginWindowBase
    {
        public ViewModel DataModel { get; }        
        public MainWindow(ViewModel dataModel)
        {
            InitializeComponent();
            DataModel = dataModel;
        }

        private void WPFOkApplyModifyGetOnOffCancel_ApplyClicked(object sender, EventArgs e)
        {
            this.Apply();
        }

        private void WPFOkApplyModifyGetOnOffCancel_CancelClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_GetClicked(object sender, EventArgs e)
        {
            this.Get();
        }

        private void WPFOkApplyModifyGetOnOffCancel_ModifyClicked(object sender, EventArgs e)
        {
            this.Modify();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OkClicked(object sender, EventArgs e)
        {
            this.Apply();
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OnOffClicked(object sender, EventArgs e)
        {
            this.ToggleSelection();
        }

        private void WPFMaterialCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.materialCatalog.SelectedMaterial = this.DataModel.Material;
        }

        private void WPFMaterialCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.DataModel.Material = this.materialCatalog.SelectedMaterial;
        }

        private void profileCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.profileCatalog.SelectedProfile = this.DataModel.Profilename;
        }

        private void profileCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.DataModel.Profilename = this.profileCatalog.SelectedProfile;
        }
        private void componentCatalog_SelectClicked(object sender, EventArgs e)
        {
            this.componentCatalog.SelectedName = this.DataModel.ComponentName;
            this.componentCatalog.SelectedNumber = this.DataModel.ComponentNumber;
        }

        private void componentCatalog_SelectionDone(object sender, EventArgs e)
        {
            this.DataModel.ComponentName = this.componentCatalog.SelectedName;
            this.DataModel.ComponentNumber = this.componentCatalog.SelectedNumber;
        }
    }
}
