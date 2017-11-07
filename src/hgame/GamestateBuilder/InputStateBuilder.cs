using System.Collections.Generic;
using heng;
using heng.Input;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class InputStateBuilder
		{
			readonly List<InputDevice> inputDevices;

			public InputStateBuilder()
			{
				inputDevices = new List<InputDevice>();
			}

			public int AddDevice(InputDevice device)
			{
				Assert.Ref(device);

				inputDevices.Add(device);
				return inputDevices.Count - 1;
			}

			public InputState Build()
			{
				return new InputState(inputDevices);
			}

			public void Clear()
			{
				inputDevices.Clear();
			}
		};
	};
}