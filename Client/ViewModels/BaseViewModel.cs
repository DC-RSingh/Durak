using System.ComponentModel;
using PropertyChanged;

namespace Client.ViewModels
{
    /// <summary>
    /// A BaseViewModel for classes used in <see cref="Client"/>. Injects the <see cref="PropertyChanged"/> event to all public properties
    /// in classes that inherit it, thanks to the Fody.PropertyChanged package.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Use to notify that a property changed outside of its normal setter.
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
