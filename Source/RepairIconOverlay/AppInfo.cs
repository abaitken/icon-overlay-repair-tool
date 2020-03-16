using System.Reflection;

namespace RepairIconOverlay
{
    class AppInfo
    {
        private readonly Assembly _assembly;

        public AppInfo()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        public string Title => _assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public string Description => _assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public string Copyright => _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string Trademark => _assembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark;
        public string Version => _assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        public string Company => _assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
    }
}