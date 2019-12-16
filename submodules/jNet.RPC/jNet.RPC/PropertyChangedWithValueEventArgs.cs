﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;

namespace jNet.RPC
{
    [JsonObject(IsReference = false)]
    internal class PropertyChangedWithDataEventArgs : PropertyChangedEventArgs
    {
        protected PropertyChangedWithDataEventArgs(string propertyName): base(propertyName) { }

        public static PropertyChangedEventArgs Create(string propertyName, object value)
        {
            if (value is IEnumerable)
                return new PropertyChangedWithArrayEventArgs(propertyName) {Value = value};
            return new PropertyChangedWithValueEventArgs(propertyName) {Value = value};
        }

        public virtual object Value
        {
            get => (this as PropertyChangedWithValueEventArgs)?.Value ??
                   (this as PropertyChangedWithArrayEventArgs)?.Value;
            internal set => throw new NotImplementedException();
        }
    }

    [DebuggerDisplay("{PropertyName} = {Value}")]
    internal class PropertyChangedWithValueEventArgs : PropertyChangedWithDataEventArgs
    {
        public PropertyChangedWithValueEventArgs(string propertyName) : base(propertyName) { }


        [JsonProperty]
        public override object Value { get; internal set; }
    }

    [DebuggerDisplay("{PropertyName} = {Value}")]
    internal class PropertyChangedWithArrayEventArgs : PropertyChangedWithDataEventArgs
    {
        public PropertyChangedWithArrayEventArgs(string propertyName) : base(propertyName) { }

        [JsonProperty(ItemIsReference = true, TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)]
        public override object Value { get; internal set; }
    }


    internal class PropertyChangedValueReader
    {
        public PropertyChangedValueReader(string propertyName, Func<object> valueFunc)
        {
            PropertyName = propertyName;
            ValueFunc = valueFunc;
        }
        public string PropertyName { get; }
        public Func<object> ValueFunc { get; }
    }

}
