using Architect.Builder.ViewModels.Base;

namespace VMHirarchy.ViewModels
{
    public class WordsTableVM : ViewModelBase
    {
        private readonly string[] _loadedPluginsNames;
        private int _currentPluginIndex = 0;

        public WordsTableVM()
        {
            _loadedPluginsNames = Rhino.PlugIns.PlugIn.GetInstalledPlugInNames();
        }

        public MainVM Parent { get; set; }

        public bool TryGetNextName(out string nextName)
        {
            nextName = string.Empty;

            _currentPluginIndex++;

            if (_loadedPluginsNames.Length >= _currentPluginIndex)
                return false;

            nextName = _loadedPluginsNames[_currentPluginIndex];
            return true;
        }
    }
}
