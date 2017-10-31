using System.Collections.Generic;

namespace heng.Input
{
	/// <summary>
	/// Represents an immutable snapshot of the input system's current state.
	/// <para>Each frame, provided virutal <see cref="InputDevice"/>s read a new <see cref="InputData"/>
	/// snapshot, filtering physical device data through a string-keyed set of virtual button/axis representations.</para>
	/// Be sure to carry over old <see cref="InputDevice"/>s when constructing a new <see cref="InputState"/>, or methods like
	/// <see cref="InputDevice.GetButtonPressed(string)"/> won't be able to interpolate between old and new <see cref="InputData"/>.
	/// </summary>
	public class InputState
	{
		/// <summary>
		/// All currently active virtual <see cref="InputDevice"/>s.
		/// </summary>
		public readonly IReadOnlyList<InputDevice> Devices;

		/// <summary>
		/// The <see cref="InputData"/> snapshot taken when this <see cref="InputState"/> was constructed.
		/// </summary>
		public readonly InputData Data;

		/// <summary>
		/// Constructs a new <see cref="InputState"/> using the given <see cref="InputDevice"/>s.
		/// <para>A new <see cref="InputData"/> snapshot will automatically be taken and assigned to the
		/// <see cref="InputDevice"/>s.</para>
		/// </summary>
		/// <param name="devices">The <see cref="InputDevice"/>s used to construct the new <see cref="InputState"/>.</param>
		public InputState(IEnumerable<InputDevice> devices)
		{
			List<InputDevice> newDevices = new List<InputDevice>();
			Data = new InputData();

			foreach(InputDevice device in devices)
			{
				device.UpdateData(Data);
				newDevices.Add(device);
			}

			Devices = newDevices;
		}
	};
}