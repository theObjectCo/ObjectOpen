using Architect.Builder.ViewModels.Base;
using Rhino.PlugIns;
using System.Collections.Generic;
using System.Linq;

namespace VMHirarchy.ViewModels
{
    public class WordsTableVM : ViewModelBase
    {
        private readonly List<PlugInInfo> _loadedPluginsData;
        private int _currentPluginIndex = 0;

        public WordsTableVM()
        {
            _loadedPluginsData = PlugIn.GetInstalledPlugIns()
                .Select(keyVal => PlugIn.GetPlugInInfo(keyVal.Key))
                .ToList();
        }

        public MainVM Parent { get; set; }

        public bool TryGetNextPluginInfo(out PlugInInfo nextInfo)
        {
            nextInfo = null;

            _currentPluginIndex++;

            if (_loadedPluginsData.Count <= _currentPluginIndex)
                return false;

            nextInfo = _loadedPluginsData[_currentPluginIndex];
            return true;
        }
    }
}
