namespace heng.Input
{
	/// <summary>
	/// A simple wrapper around a <see cref="MouseButtonCode"/>, representing a single mouse button.
	/// </summary>
	public class MouseButton : IButton
	{
		readonly MouseButtonCode code;
		
		/// <summary>
		/// Constructs a new <see cref="MouseButton"/> using the given code.
		/// </summary>
		/// <param name="code">The code for the mouse button represented by this <see cref="MouseButton"/>.</param>
		public MouseButton(MouseButtonCode code)
		{
			this.code = code;
		}

		/// <inheritdoc />
		public bool GetValue(InputData data)
		{
			Assert.Ref(data);

			return data.GetMouseButtonDown(code);
		}
	};
}