namespace heng.Input
{
	/// <summary>
	/// Codes representing readable buttons on a controller.
	/// <para>These directly mirror SDL2's controller button codes - don't mess with them.</para>
	/// </summary>
	public enum ControllerButtonCode
	{
		Invalid = -1,
		A = 0,
		B = 1,
		X = 2,
		Y = 3,
		Back = 4,
		Guide = 5,
		Start = 6,
		LeftStick = 7,
		RightStick = 8,
		LeftShoulder = 9,
		RightShoulder = 10,
		DPadUp = 11,
		DPadDown = 12,
		DPadLeft = 13,
		DPadRight = 14,

		Count = 15
	};
}