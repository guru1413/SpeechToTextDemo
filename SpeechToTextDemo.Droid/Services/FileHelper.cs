using System;
using System.IO;
using Xamarin.Forms;
using SpeechToTextDemo.Interfaces;
using SpeechToTextDemo.Droid.Services;

[assembly: Dependency(typeof(FileHelper))]
namespace SpeechToTextDemo.Droid.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}