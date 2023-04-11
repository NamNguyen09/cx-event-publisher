using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace cx.EventsPublisher
{
    public sealed class WindowsServicePerThreadLifetimeManager : LifetimeManager
    {
        private readonly Guid _key = Guid.NewGuid();
        [ThreadStatic]
        private static Dictionary<Guid, object> values;

        private static void EnsureValues()
        {
            if (values == null)
            {
                values = new Dictionary<Guid, object>();
            }
        }

        public override object GetValue()
        {
            object obj2;
            EnsureValues();
            values.TryGetValue(_key, out obj2);
            return obj2;
        }

        public override void RemoveValue()
        {
            EnsureValues();
            values.Remove(_key);
        }

        public override void SetValue(object newValue)
        {
            EnsureValues();
            values[_key] = newValue;
        }
    }
}
