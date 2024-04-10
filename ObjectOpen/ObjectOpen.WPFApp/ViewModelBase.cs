using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ObjectOpen.WPFApp
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase() {; }

        public string DisplayName { get; set; } = "";
        public bool ThrowOnInvalidPropertyName { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            VerifyPropertyName(propertyName);
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string? propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (propertyName == null || TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                if (ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        public static string? AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }
    }
}
