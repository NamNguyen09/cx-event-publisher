using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace cx.EventsPublisher
{
    public sealed class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            RemoveValue();
        }

        #endregion

        public override object GetValue()
        {
            return HttpContext.Current.Items[this];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(this);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[this] = newValue;
        }
    }
}
