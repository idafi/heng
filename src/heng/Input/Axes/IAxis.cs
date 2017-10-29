namespace heng.Input
{
	/// <summary>
	/// Represents a virtual axis.
	/// <para>An axis returns a signed, 1-dimensional value describing its current position.</para>
	/// </summary>
	public interface IAxis
	{
		/// <summary>
		/// Reads the current value of the axis.
		/// </summary>
		/// <param name="state">The <see cref="InputData"/> which this <see cref="IAxis"/> will read.</param>
		/// <returns>The current value of the axis, as a signed 16-bit integer.</returns>
		short GetValue(InputData state);
	};
}