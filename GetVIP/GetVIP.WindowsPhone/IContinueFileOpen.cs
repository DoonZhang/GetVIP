using Windows.ApplicationModel.Activation;

namespace GetVIP
{
    public interface IContinueFileOpen
    {
        void ContinueFileOpen(FileOpenPickerContinuationEventArgs args);
    }
}