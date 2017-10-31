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
		/// Constructs an initial <see cref="TimeState"/> snapshot.
		/// <para>After this, recreate the <see cref="TimeState"/> each frame using <see cref="Update"/>,
		/// which will provide accurate <see cref="Delta"/>s and target-frametime services.</para>
		/// </summary>
		public TimeState()
		{
			TotalTicks = Core.Time.GetTicks();
			DeltaTicks = 0;
		}

		TimeState(UInt32 totalTicks, UInt32 deltaTicks)
		{
			TotalTicks = totalTicks;
			DeltaTicks = deltaTicks;
		}

		/// <summary>
		/// Creates a new, updated <see cref="TimeState"/> snapshot.
		/// <para>The current thread will optionally be delayed, if the given target frametime is not yet reached.</para>
		/// </summary>
		/// <param name="targetFrametime">If the number of ticks between now and the previous total is less than this,
		/// the current thread will be delayed until the target is reached.
		/// <para>If set to 0, the new <see cref="TimeState"/> will be returned immediately.</para></param>
		/// <returns>A new, updated <see cref="TimeState"/> snapshot.</returns>
		public TimeState Update(UInt32 targetFrametime)
		{
			long newTicks = Core.Time.GetTicks();
			long diff = newTicks - TotalTicks;

			if(diff < targetFrametime)
			{ Core.Time.Delay((UInt32)(targetFrametime - diff)); }

			UInt32 newTotal = Core.Time.GetTicks();
			UInt32 newDelta = newTotal - TotalTicks;

			return new TimeState(newTotal, newDelta);
		}
	};
}