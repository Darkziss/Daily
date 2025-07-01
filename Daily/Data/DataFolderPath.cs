using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;

namespace Daily.Data
{
    public static class DataFolderPath
    {
        private static readonly string _path = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!.AbsolutePath;

        public static string Path => _path;
    }
}