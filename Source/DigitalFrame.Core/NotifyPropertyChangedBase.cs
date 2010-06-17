using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DigitalFrame.Core.Extensions;

namespace DigitalFrame.Core
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(Expression<Func<object>> expression)
        {
            PropertyChanged.NotifyPropertyChanged(expression);
        }
    }
}
