namespace heng.Input
{
	/// <summary>
	/// A simple wrapper around a <see cref="KeyCode"/>, representing a single keyboard key.
	/// </summary>
	public class Key : IButton
	{
		readonly KeyCode code;

		/// <summary>
		/// Constructs a new <see cref="Key"/> using the given code.
		/// </summary>
		/// <param name="code">The code for the key represented by this <see cref="Key"/>.</param>
		public Key(KeyCode code)
		{
			this.code = code;
		}

		/// <inheritdoc />
		public bool GetValue(InputData data)
		{
			Assert.Ref(data);

			return data.GetKeyDown(code);
		}
	};
}