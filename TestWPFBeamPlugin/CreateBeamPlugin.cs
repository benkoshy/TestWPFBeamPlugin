using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Dialog;
using Tekla.Structures.Plugins;

using Tekla.Structures.Model.UI;
using Tekla.Structures.Model;
using FlatBuffers;
using static Tekla.Structures.Filtering.Categories.PartFilterExpressions;
using System.Windows;
using RenderData;

using Point = Tekla.Structures.Geometry3d.Point;
using Tekla.Structures.Model.Operations;

namespace TestWPFBeamPlugin
{
    [Plugin("BKBeam")]
    [PluginUserInterface("TestWPFBeamPlugin.MainWindow")]
    public class CreateBeamPlugin : PluginBase
    {        
        private PluginData pluginData;
        public PluginData PluginData
        {
            get { return pluginData; }
            set { pluginData = value; }
        }

        private Model model;

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        
        private string _partName = string.Empty;
        private string _profile = string.Empty;
        private string _material = string.Empty;
        private double _offset = 0.0;
        private int _lengthFactor = 0;


        public CreateBeamPlugin(PluginData pluginData) 
        {
            this.PluginData = pluginData;
            this.Model = new Model();            
        }   

        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> PointList = new List<InputDefinition>();
            Picker Picker = new Picker();
            ArrayList PickedPoints = Picker.PickPoints(Picker.PickPointEnum.PICK_TWO_POINTS);

            Operation.DisplayPrompt("Ben - we can now debug without restarting Tekla");

            PointList.Add(new InputDefinition(PickedPoints));

            return PointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                GetValuesFromDialog();

                ArrayList Points = (ArrayList)Input[0].GetInput();
                Point StartPoint = Points[0] as Point;
                Point EndPoint = Points[1] as Point;

                Point LengthVector = new Point(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y, EndPoint.Z - StartPoint.Z);

                if (_lengthFactor > 0)
                {
                    EndPoint.X = _lengthFactor * LengthVector.X + StartPoint.X;
                    EndPoint.Y = _lengthFactor * LengthVector.Y + StartPoint.Y;
                    EndPoint.Z = _lengthFactor * LengthVector.Z + StartPoint.Z;
                }

                Beam beam = new Beam(StartPoint, EndPoint);
                beam.Position.PlaneOffset = _offset;
                beam.Name = _partName;
                beam.Profile.ProfileString = _profile;
                beam.Material.MaterialString = _material;
                beam.Insert();

                Operation.DisplayPrompt("Selected component " + pluginData.componentName + " : " + pluginData.componentNumber.ToString());

            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.ToString());
            }

            return true;
        }

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            _partName = pluginData.partName;
            _profile = pluginData.profile;
            _material = pluginData.material;
            _offset = pluginData.offset;
            _lengthFactor = pluginData.lengthFactor + 1;

            if (IsDefaultValue(_partName))
                _partName = "TEST";
            if (IsDefaultValue(_profile))
                _profile = "HEA200";
            if (IsDefaultValue(_material))
                _material = "STEEL_UNDEFINED";
            if (IsDefaultValue(_offset))
                _offset = 0;
            if (IsDefaultValue(_lengthFactor) || _lengthFactor == 0)
                _offset = 1;
        }
        #endregion
    }

    public class PluginData
    {
        [StructuresField("name")]
        public string partName;

        [StructuresField("profile")]
        public string profile;

        [StructuresField("offset")]
        public double offset;

        [StructuresField("material")]
        public string material;

        [StructuresField("componentname")]
        public string componentName;

        [StructuresField("componentnumber")]
        public int componentNumber;

        [StructuresField("lengthfactor")]
        public int lengthFactor;
    }


    /// MVVM 
    ///   The UI binds to MVVM
    ///   The MVVM ----> saves the data to Tekla
    ///   
    ///   Tekla then passes in data to the plugin (via the StructuresField). i.e. the structuresField should map to the StructuresDialog
    ///   
    ///   And then plugin, in the examples noted, seem to be storing a local copy of the pluginData.
    ///   


}
