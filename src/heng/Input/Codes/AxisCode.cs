namespace heng.Input
{
	/// <summary>
	/// Codes representing readable axes on a controller.
	/// <para>These directly mirror SDL2's axis codes - don't mess with them.</para>
	/// </summary>
	public enum AxisCode
	{
		Invalid = -1,
		LeftX = 0,
		LeftY = 1,
		RightX = 2,
		RightY = 3,
		TriggerLeft = 4,
		TriggerRight = 5,

		Count = 6
	};
}