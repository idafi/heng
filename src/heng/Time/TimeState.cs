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
		readonly Core.Time.State coreState;

		/// <summary>
		/// Milliseconds elapsed since the <see cref="Engine"/> was initialized.
		/// </summary>
		public UInt32 TotalTicks => coreState.TotalTicks;

		/// <summary>
		/// Milliseconds elapsed since the last <see cref="TimeState"/> snapshot was constructed.
		/// </summary>
		public UInt32 DeltaTicks => coreState.DeltaTicks;

		/// <summary>
		/// Seconds elapsed since the <see cref="Engine"/> was initialized.
		/// </summary>
		public float Total => (float)(TotalTicks) / 1000;

		/// <summary>
		/// Seconds elapsed since the last <see cref="TimeState"/> snapshot was constructed.
		/// </summary>
		public float Delta => (float)(DeltaTicks) / 1000;

		/// <summary>
		/// How long <see cref="DelayToTarget"/> should halt the current thread, if not yet reached.
		/// <para>If this is 0, <see cref="DelayToTarget"/> will have no effect.</para>
		/// </summary>
		public UInt32 TargetFrametime
		{
			get => coreState.TargetFrametime;
			set => Core.Time.SetTargetFrametime(value);
		}

		/// <summary>
		/// Constructs a new snapshot of the timekeeping system's state.
		/// </summary>
		public TimeState()
		{
			Core.Time.GetSnapshot(out coreState);
		}

		/// <summary>
		/// Halt the current thread for the given number of milliseconds.
		/// </summary>
		/// <param name="ms">The number of milliseconds to wait.</param>
		public void Delay(UInt32 ms)
		{
			Core.Time.Delay(ms);
		}

		/// <summary>
		/// If called before the <see cref="TargetFrametime"/> is reached, halt the current thread until then.
		/// <para>If the <see cref="TargetFrametime"/> is 0, this will have no effect.</para>
		/// </summary>
		public void DelayToTarget()
		{
			Core.Time.DelayToTarget();
		}
	};
}