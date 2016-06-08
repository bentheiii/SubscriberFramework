using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberFramework
{
    public abstract class AdaptedSubscriber<S,N,T> : EventSubscriber<S,N> where N : Event where T:N where S : Notifier<N>
    {
        protected AdaptedSubscriber(EventCatagory catagoryToSubscribe)
        {
            this.catagoryToSubscribe = catagoryToSubscribe;
        }
        public abstract bool isValid(S sender, T e);
        public virtual object Activate(S sender, T e, out N forwardEvent)
        {
            forwardEvent = e;
            return Activate(sender, e);
        }
        public virtual object Activate(S sender, T e)
        {
            throw new NotSupportedException("either this or its overload should be implemented");
        }
        public override object ActivateEvent(S sender, N e, out N forwardEvent)
        {
            var ev = e as T;
            if (ev == null || !isValid(sender, ev))
            {
                forwardEvent = e;
                return null;
            }
            return Activate(sender, ev, out forwardEvent);
        }
        public override EventCatagory catagoryToSubscribe { get; }
    }
}
