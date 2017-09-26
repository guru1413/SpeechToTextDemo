using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SpeechToTextDemo.Helpers;
using SpeechToTextDemo.Interfaces;
using SpeechToTextDemo.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SpeechToTextDemo.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        IDependencyService _dependencyService;
        IBingSpeechService bingSpeechService;
        bool isRecording = false;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string buttonText = "Start";
        public string ButtonText
        {
            get { return buttonText; }
            set { SetProperty(ref buttonText, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private DelegateCommand speechCommand;
        public DelegateCommand SpeechCommand =>
            speechCommand ?? (speechCommand = new DelegateCommand(speech, () => !IsBusy));

        private DelegateCommand nativeSpeechCommand;
        public DelegateCommand NativeSpeechCommand =>
            nativeSpeechCommand ?? (nativeSpeechCommand = new DelegateCommand(nativeSpeech, () => !IsBusy));

        private async void nativeSpeech()
        {
            IsBusy = true;
            Title = await _dependencyService.Get<ISpeechToText>().SpeechToTextAsync();
            IsBusy = false;
        }

        private async void speech()
        {
            try
            {
                IsBusy = true;
                var audioRecordingService = _dependencyService.Get<IAudioRecorderService>();
                if (!isRecording)
                {
                    audioRecordingService.StartRecording();
                    ButtonText = "Stop";
                }
                else
                {
                    ButtonText = "Start";
                    audioRecordingService.StopRecording();
                }

                isRecording = !isRecording;
                if (!isRecording)
                {
                    var speechResult = await bingSpeechService.RecognizeSpeechAsync(Constants.AudioFilename);
                    Debug.WriteLine("Name: " + speechResult.Name);
                    Debug.WriteLine("Confidence: " + speechResult.Confidence);
                    Title = speechResult.Name;

                    //if (!string.IsNullOrWhiteSpace(speechResult.Name))
                    //{
                    //    TodoItem.Name = char.ToUpper(speechResult.Name[0]) + speechResult.Name.Substring(1);
                    //    OnPropertyChanged("TodoItem");
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public MainPageViewModel(IDependencyService dependencyService)
        {
            _dependencyService = dependencyService;
            bingSpeechService = new BingSpeechService(new AuthenticationService(Constants.BingSpeechApiKey), Device.OS.ToString());
        }


        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //if (parameters.ContainsKey("title"))
            //    Title = (string)parameters["title"] + " and Prism";
        }
    }
}
