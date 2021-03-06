﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DevelopmentSupport.Common
{
    /// <summary>
    /// プロパティをバインド可能なクラス
    /// </summary>
    public class BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
