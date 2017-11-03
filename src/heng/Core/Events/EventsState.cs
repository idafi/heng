namespace heng
{
	/// <summary>
	/// Represents an immutable snapshot of the event system's current state.
	/// <para>Constructing a new EventsState will pump the engine core's event queue.</para>
	/// If you don't do this at the start of every frame, most engine systems (input, audio, video) can't function.
	/// </summary>
	public class EventsState
	{
		/// <summary>
		/// Whether or not the engine core received a quit request from the user.
		/// <para>By default, this is set to true when all open windows are closed.</para>
		/// </summary>
		public readonly bool IsQuitRequested;
		
		/// <summary>
		/// Constructs a new snapshot of the event system's state, pumping the engine core's event queue.
		/// <para>Most engine systems rely on the event queue to function, so be sure to build one of
		/// these at the start of every frame.</para>
		/// </summary>
		public EventsState()
		{
			Core.Events.Pump();
			IsQuitRequested = Core.Events.IsQuitRequested();
		}
	};
}