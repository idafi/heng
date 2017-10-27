namespace heng.Input
{
	/// <summary>
	/// A simple wrapper around a controller <see cref="AxisCode"/>, representing
	/// a single stick axis on a specific controller.
	/// </summary>
	public class ControllerAxis : IAxis
	{
		readonly int controller;
		readonly AxisCode code;
		
		/// <summary>
		/// Constructs a new <see cref="ControllerAxis"/> using the given code.
		/// </summary>
		/// <param name="controller">The controller's device ID.</param>
		/// <param name="code">The code for the axis represented by this <see cref="ControllerAxis"/>.</param>
		public ControllerAxis(int controller, AxisCode code)
		{
			Assert.Index(controller, Core.Input.ControllersMax);

			this.controller = controller;
			this.code = code;
		}

		/// <inheritdoc />
		public short GetValue(InputState state)
		{
			Assert.Ref(state);

			return state.GetAxisValue(controller, code);
		}
	};
}