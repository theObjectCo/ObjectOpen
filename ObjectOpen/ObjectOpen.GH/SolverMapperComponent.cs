using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.GH
{
    public class SolverMapperComponent : ObjectComponent
    {
        public SolverMapperComponent() : base(
        "SolverMapper",
        "SMapper",
        "Solver mapper",
        "Solvers")
        {
        }

        protected override Bitmap Icon => ObjectOpen.GH.Properties.Resources.icon_small_24x24;

        public Assembly LoadedAssembly { get; set; } = null;
        public Solver CurrentSolver { get; set; } = null;
        public List<Type> AvailableSolvers { get; set; } = new List<Type>();

        public override Guid ComponentGuid => new("{11DD1400-F2AD-4722-A98A-0AC15D734B90}");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (CurrentSolver == null) return;

            SolverInputs inputs = MapInputs(CurrentSolver.GetInputsType(), DA);
            SetInputs(CurrentSolver, inputs);

            var result = CurrentSolver.Solve();

            if (result.Flag != Flag.OK)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Message);

            var outputs = GetOutputs(CurrentSolver);
            MapOutputs(outputs, DA);
        }

        #region Marshalling 
        public SolverInputs MapInputs(Type solverInputType, IGH_DataAccess DA)
        {
            SolverInputs inputs = (SolverInputs)Activator.CreateInstance(solverInputType);

            PropertyInfo[] properties = inputs.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                Type propertyType = prop.PropertyType;

                if (typeof(IList).IsAssignableFrom(propertyType))
                {
                    if (propertyType.IsArray)
                    {
                        List<object> listobject = new List<object>();
                        if (!DA.GetDataList(i, listobject)) continue;

                        var elementType = propertyType.GetElementType();
                        Array mapped = Array.CreateInstance(elementType, listobject.Count);

                        for (int j = 0; j < listobject.Count; j++)
                        {
                            dynamic item = listobject[j];
                            dynamic val = item.Value;
                            mapped.SetValue(val, j);
                        }

                        prop.SetValue(inputs, mapped);
                    }
                    else
                    {
                        List<object> listobject = new List<object>();
                        if (!DA.GetDataList(i, listobject)) continue;

                        var containedType = propertyType.GenericTypeArguments.First();

                        Type listType = typeof(List<>).MakeGenericType(containedType);
                        IList listInstance = (IList)Activator.CreateInstance(listType);

                        foreach (dynamic item in listobject)
                        {
                            dynamic val = item.Value;
                            listInstance.Add(val);
                        }

                        prop.SetValue(inputs, listInstance);
                    }
                }
                else
                {
                    dynamic inputValue = null;
                    if (!DA.GetData(i, ref inputValue)) continue;
                    prop.SetValue(inputs, inputValue.Value);
                }
            }

            return inputs;
        }
        public void MapOutputs(SolverOutputs outputs, IGH_DataAccess DA)
        {
            PropertyInfo[] properties = outputs.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                object data = prop.GetValue(outputs);

                IGH_Param param = this.Params.Output[i];

                if (param.Access == GH_ParamAccess.item)
                {
                    DA.SetData(i, data);

                }
                else if (param.Access == GH_ParamAccess.list)
                {
                    DA.SetDataList(i, (IEnumerable)data);
                }
            }
        }
        public bool SetInputs(Solver solver, SolverInputs inputs)
        {
            try
            {
                foreach (var item in solver.GetType().GetProperties())
                    if (item.PropertyType.IsSubclassOf(typeof(SolverInputs)))
                        item.SetValue(solver, inputs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public SolverOutputs GetOutputs(Solver solver)
        {
            foreach (var propInfo in solver.GetType().GetProperties())
                if (propInfo.PropertyType.IsSubclassOf(typeof(SolverOutputs)))
                    return propInfo.GetValue(solver) as SolverOutputs;

            return null;
        }
        #endregion

        #region Assembly
        public override void AppendAdditionalMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Load Assembly", OnLoadAssembly);

            Menu_AppendSeparator(menu);

            string solverdd = LoadedAssembly == null ? "Solvers" : LoadedAssembly.FullName;
            var solvers = Menu_AppendItem(menu, solverdd);
            foreach (var solver in AvailableSolvers)
                Menu_AppendItem(solvers.DropDown, solver.FullName, OnSolverClick);

            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "Save to JSON", SaveToJSON);
        }

        private void SaveToJSON(object sender, EventArgs e)
        {
            if (CurrentSolver == null) return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
                File.WriteAllText(dialog.FileName, CurrentSolver.ToJSON());
        }

        private void OnSolverClick(object sender, EventArgs e)
        {
            foreach (var item in LoadedAssembly.DefinedTypes)
            {
                ToolStripMenuItem tstrip = sender as ToolStripMenuItem;

                if (item.FullName == tstrip.Text)
                {
                    CurrentSolver = (Solver)Activator.CreateInstance(item);
                    this.Name = item.FullName;
                    this.NickName = item.Name;
                    CreateParameters();
                    break;
                }
            }

        }

        public void OnLoadAssembly(object sender, EventArgs e)
        {
            Rhino.UI.OpenFileDialog dialog = new Rhino.UI.OpenFileDialog();
            if (dialog.ShowOpenDialog())
                LoadAssembly(dialog.FileName);
        }

        public void CreateParameters()
        {
            if (CurrentSolver == null) return;

            Type inputType = CurrentSolver.GetInputsType();
            Type outputType = CurrentSolver.GetOutputsType();

            List<IGH_Param> inputs = new List<IGH_Param>(this.Params.Input);
            foreach (var item in inputs)
                this.Params.UnregisterInputParameter(item);

            List<IGH_Param> outputs = new List<IGH_Param>(this.Params.Output);
            foreach (var item in outputs)
                this.Params.UnregisterOutputParameter(item);

            foreach (var item in GetParameters(inputType))
                this.Params.RegisterInputParam(item);

            foreach (var item in GetParameters(outputType))
                this.Params.RegisterOutputParam(item);

            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
            this.OnPreviewExpired(true);
        }

        private List<IGH_Param> GetParameters(Type mappedType)
        {
            List<IGH_Param> ghparams = new List<IGH_Param>();

            foreach (var property in mappedType.GetProperties())
            {
                IGH_Param param = null;
                param = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                param.Name = property.Name;

                string uppercase = "";
                foreach (char c in property.Name)
                    if (char.IsUpper(c)) uppercase += c;

                param.NickName = string.IsNullOrEmpty(uppercase) ? property.Name.Substring(0, 3) : uppercase;

                if (typeof(IList).IsAssignableFrom(property.PropertyType))
                    param.Access = GH_ParamAccess.list;
                else
                    param.Access = GH_ParamAccess.item;

                ghparams.Add(param);
            }

            return ghparams;
        }

        public override bool Write(GH_IWriter writer)
        {
            if (LoadedAssembly != null)
                writer.SetString("LoadedAssembly", AssemblyPath(LoadedAssembly));
            else
                writer.SetString("LoadedAssembly", "");

            if (CurrentSolver != null)
                writer.SetString("CurrentSolver", CurrentSolver.GetType().FullName);
            else
                writer.SetString("CurrentSolver", "");

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            string assemblyPath = reader.GetString("LoadedAssembly");
            string currentSolver = reader.GetString("CurrentSolver");

            if (!string.IsNullOrEmpty(assemblyPath)) LoadAssembly(assemblyPath);
            if (!string.IsNullOrEmpty(currentSolver)) LoadSolver(currentSolver);

            return base.Read(reader);
        }
        public void LoadAssembly(string fileName)
        {
            try
            {
                LoadedAssembly = Assembly.LoadFrom(fileName);
                AvailableSolvers = SolverReflection.FindSolvers(LoadedAssembly);
            }
            catch (Exception ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
            }
        }
        private void LoadSolver(string fullName)
        {
            CurrentSolver = (Solver)Activator.CreateInstance(LoadedAssembly.GetType(fullName));
            CreateParameters();
        }

        public string AssemblyPath(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return path;
        }

        #endregion
    }
}
