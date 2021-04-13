using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private object _content;

        public object Content
        {
            get => _content;
            set
            {
                if (value == null || _content == value) return;

                _content = value;
            }
        }
    }
}
