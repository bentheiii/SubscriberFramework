using System;
using System.Collections.Generic;
using Edge.SystemExtensions;

namespace SubscriberFramework
{
    public interface IEventSubscriber<E> where E : Event
    {
        object ActivateEvent(Notifier<E> sender, E e, out E forwardEvent);
        EventCatagory catagoryToSubscribe { get; }
        void OnSubscribe(Notifier<E> notifier);
    }
    public abstract class EventSubscriber<S,E> : IEventSubscriber<E> where S : Notifier<E> where E : Event
    {
        public abstract object ActivateEvent(S sender, E e, out E forwardEvent);
        public object ActivateEvent(Notifier<E> sender, E e, out E forwardEvent)
        {
            var s = sender as S;
            if (s != null)
                return ActivateEvent(s, e, out forwardEvent);
            throw new Exception("sender is not of type S");
        }
        public abstract EventCatagory catagoryToSubscribe { get; }
        public virtual void OnSubscribe(S notifier)
        {
            
        }
        public void OnSubscribe(Notifier<E> notifier)
        {
            var s = notifier as S;
            if (s != null)
                OnSubscribe(s);
            else
                throw new Exception("sender is not of type S");
        }
        public override int GetHashCode()
        {
            return IdDistributer.getId(this).GetHashCode();
        }
    }
    public class EventCatagory
    {
        public override int GetHashCode()
        {
            return IdDistributer.getId(this).GetHashCode();
        }
    }
    public abstract class Event
    {
        public abstract EventCatagory catagory { get; }
    }
}
