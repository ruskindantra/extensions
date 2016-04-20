using System;
using System.Reflection;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
	public static class EventHandlerExtensions
	{
		public static void Raise(this EventHandler eventHandler, object sender)
		{
			var handler = eventHandler;
			handler?.Invoke(sender, EventArgs.Empty);
		}

		public static void Raise<T>(this EventHandler<T> eventHandler, object sender, [NotNull] T args)
			where T : EventArgs
		{
			if (args == null)
				throw new ArgumentNullException(nameof(args), "Cannot raise event; Arguments are missing");

			var handler = eventHandler;
			handler?.Invoke(sender, args);
		}

		/// <summary>
		/// Removes an event handler from an object if the -= syntax doesn't work
		/// </summary>
		/// <typeparam name="T">The type of the object</typeparam>
		/// <param name="obj">The instance of the object</param>
		/// <param name="eventName">The name of the event</param>
		/// <returns>True if the event exists otherwise false</returns>
		public static bool RemoveEventHandlers<T>(this T obj, [NotNull] string eventName) where T : class
		{
			var type = typeof(T);
			var field = type.GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
			
			if (field == null)
				throw new InvalidOperationException($"Cannot find an event named <{eventName}> in <{type.Name}>");

			EventInfo eventInfo = type.GetEvent(field.Name);
			if (eventInfo != null)
			{
				MulticastDelegate multicastDelegate = field.GetValue(obj) as MulticastDelegate;
				if (multicastDelegate != null)
				{
					foreach (Delegate _delegate in multicastDelegate.GetInvocationList())
					{
						eventInfo.RemoveEventHandler(obj, _delegate);
					}
					return true;
				}
			}
			return false;
		}
	}
}