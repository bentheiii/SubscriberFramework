using System.Collections.Generic;
using System.Linq;
using Edge.Arrays;
using Edge.Looping;

namespace SubscriberFramework
{
    public class Notifier<E> where E : Event
    {
        protected readonly IDictionary<EventCatagory, ISet<IEventSubscriber<E>>> Subscribers = new Dictionary<EventCatagory, ISet<IEventSubscriber<E>>>();
        public void Subscribe(IEventSubscriber<E> subscriber)
        {
            var type = subscriber.catagoryToSubscribe;
            this.Subscribers.EnsureDefinition(type, new HashSet<IEventSubscriber<E>>());
            this.Subscribers[type].Add(subscriber);
            subscriber.OnSubscribe(this);
        }
        public void UnSubscribe(IEventSubscriber<E> subscriber)
        {
            this.Subscribers[subscriber.catagoryToSubscribe].Remove(subscriber);
        }
        private static IEnumerable<T> CascadeEvent<T>(IEnumerable<IEventSubscriber<E>> subs, Notifier<E> c, E e)
        {
            foreach (var sub in subs)
            {
                if (e == null)
                    yield break;
                var ret = sub.ActivateEvent(c, e, out e);
                if (ret is T)
                    yield return (T)ret;
            }
        }
        public T[] Notify<T>(E e)
        {
            return CascadeEvent<T>(this.Subscribers[e.catagory],this, e).ToArray();
        }
        public void Notify(E e)
        {
            CascadeEvent<object>(this.Subscribers[e.catagory], this, e).Do();
        }
    }
}
