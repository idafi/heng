namespace heng.Input
{
	/// <summary>
	/// Represents a virtual button.
	/// <para>A button returns a boolean, describing whether or not it is currently down.</para>
	/// </summary>
	public interface IButton
	{
		/// <summary>
		/// Reads the current down state of the button.
		/// </summary>
		/// <param name="data">The <see cref="InputData"/> which this <see cref="IButton"/> will read.</param>
		/// <returns>True if the button is currently down; false if not.</returns>
		bool GetValue(InputData data);
	};
}