using System;

namespace heng
{
	/// <summary>
	/// Configuration flags for the engine core's event log.
	/// </summary>
	[Flags]
	public enum EventLogMode
	{
		/// <summary>
		/// No events will be logged.
		/// </summary>
		Off			= 0x00,

		/// <summary>
		/// Input events will be logged.
		/// </summary>
		Input		= 0x01,

		/// <summary>
		/// Hardware device connect/disconnect events will be logged.
		/// </summary>
		Device		= 0x02,

		/// <summary>
		/// Window events will be logged.
		/// </summary>
		Window		= 0x04,

		/// <summary>
		/// The Quit event (<see cref="Engine.IsQuitRequested"/>) will be logged.
		/// </summary>
		Quit		= 0x08,

		/// <summary>
		/// Sets all event logging flags to on.
		/// </summary>
		All			= 0x0F,

		/// <summary>
		/// Events will be "played back" from the existing event log.
		/// </summary>
		Simulate	= 0x10
	};
}