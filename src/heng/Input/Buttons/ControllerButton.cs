namespace heng.Input
{
	/// <summary>
	/// A simple wrapper around a <see cref="ControllerButtonCode"/>, representing a
	/// single button on a specific controller.
	/// </summary>
	public class ControllerButton : IButton
	{
		readonly int controller;
		readonly ControllerButtonCode code;

		/// <summary>
		/// Constructs a new <see cref="ControllerButton"/> using the given code.
		/// </summary>
		/// <param name="controller">The controller's device ID.</param>
		/// <param name="code">The code for the button represented by this <see cref="ControllerButton"/>.</param>
		public ControllerButton(int controller, ControllerButtonCode code)
		{
			Assert.Index(controller, Core.Input.ControllersMax);

			this.controller = controller;
			this.code = code;
		}

		/// <inheritdoc />
		public bool GetValue(InputData data)
		{
			Assert.Ref(data);

			return data.GetControllerButtonDown(controller, code);
		}
	};
}