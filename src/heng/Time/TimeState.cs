using System;

namespace heng.Time
{
	/// <summary>
	/// Represents an immutable snapshot of the engine's timekeeping system.
	/// <para>The snapshot's <see cref="Delta"/> values represent the time since the last snapshot was taken.</para>
	/// Take a new snapshot at the start of every frame to ensure accurate delta-time values for your other systems.
	/// </summary>
	public class TimeState
	{
		/// <summary>
		/// Milliseconds elapsed since the <see cref="Engine"/> was initialized.
		/// </summary>
		public readonly UInt32 TotalTicks;

		/// <summary>
		/// Milliseconds elapsed since the last <see cref="TimeState"/> was constructed.
		/// </summary>
		public readonly UInt32 DeltaTicks;

		/// <summary>
		/// Seconds elapsed since the <see cref="Engine"/> was initialized.
		/// </summary>
		public float Total => (float)(TotalTicks) / 1000;

		/// <summary>
		/// Seconds elapsed since the last <see cref="TimeState"/> was constructed.
		/// </summary>
		public float Delta => (float)(DeltaTicks) / 1000;


		/// <summary>
		/// Constructs a new <see cref="TimeState"/>.
		/// <para>The current thread will optionally be delayed, if the given target frametime is not yet reached.</para>
		/// </summary>
		/// <param name="previousTotal">The <see cref="TotalTicks"/> from the previous <see cref="TimeState"/>.
		/// <para>Set this to 0 if this is the first <see cref="TimeState"/> to be constructed.</para></param>
		/// <param name="targetFrametime">If the number of ticks between now and the previous total is less than this,
		/// the current thread will be delayed until the target is reached.
		/// <para>If set to 0, the new <see cref="TimeState"/> will always be constructed immediately.</para></param>
		public TimeState(UInt32 previousTotal, UInt32 targetFrametime)
		{
			long newTicks = Core.Time.GetTicks();
			long diff = newTicks - previousTotal;

			if(diff < targetFrametime)
			{ Core.Time.Delay((UInt32)(targetFrametime - diff)); }

			TotalTicks = Core.Time.GetTicks();
			DeltaTicks = TotalTicks - previousTotal;
		}
	};
}