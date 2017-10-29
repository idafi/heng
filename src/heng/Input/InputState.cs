using System.Collections.Generic;

namespace heng.Input
{
	public class InputState
	{
		public readonly IReadOnlyList<InputDevice> Devices;
		public readonly InputData Data;

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