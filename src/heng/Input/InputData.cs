namespace heng.Input
{
	/// <summary>
	/// An immutable snapshot of all physical input devices' current state.
	/// <para>You can read directly from here using codes such as <see cref="KeyCode"/>, but
	/// it's recommend you use a virtual <see cref="InputDevice"/> as an intepreter.</para>
	/// </summary>
	public class InputData
	{
		readonly Core.Input.State coreState;

		/// <summary>
		/// Checks if the given key is currently down.
		/// </summary>
		/// <param name="code">The <see cref="KeyCode"/> representing the key to check.</param>
		/// <returns>True if the key is currently down; false if not.</returns>
		public bool GetKeyDown(KeyCode code) => coreState.Keyboard[(int)(code)];

		/// <summary>
		/// Checks if the given mouse button is currently down.
		/// </summary>
		/// <param name="code">The <see cref="MouseButtonCode"/> representing the button to check.</param>
		/// <returns>True if the mouse button is currently down; false if not.</returns>
		public bool GetMouseButtonDown(MouseButtonCode code) => coreState.Mouse.Buttons[(int)(code)];

		/// <summary>
		/// Checks if the given controller button is currently down.
		/// </summary>
		/// <param name="id">The device ID of the controller to check.</param>
		/// <param name="code">The <see cref="ControllerButtonCode"/> representing the button to check.</param>
		/// <returns>True if the controller button is currently down; false if not.</returns>
		public bool GetControllerButtonDown(int id, ControllerButtonCode code)
		{
			Assert.Index(id, Core.Input.ControllersMax);

			return coreState.Controllers[id].Buttons[(int)(code)];
		}

		/// <summary>
		/// Reads the current value of a controller axis.
		/// </summary>
		/// <param name="id">The device ID of the controller to read.</param>
		/// <param name="axis">The <see cref="AxisCode"/> representing the axis to read.</param>
		/// <returns>The current position value of the axis.</returns>
		public short GetAxisValue(int id, AxisCode axis)
		{
			Assert.Index(id, Core.Input.ControllersMax);

			return coreState.Controllers[id].Axes[(int)(axis)];
		}

		/// <summary>
		/// Constructs a new, immutable snapshot of the engine's input state.
		/// </summary>
		public InputData()
		{
			Core.Input.GetSnapshot(out coreState);
		}
	};
}