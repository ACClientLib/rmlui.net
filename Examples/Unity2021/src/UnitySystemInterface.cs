using UnityEngine;
using RmlUiNet;
using System;

namespace URmlUi
{
	public class UnitySystemInterface : SystemInterface
	{
		public override double ElapsedTime => Time.realtimeSinceStartup;

		public override bool LogMessage(RmlUiNet.LogType type, string message)
		{
			switch (type)
			{
				case RmlUiNet.LogType.Warning:
					Debug.LogWarning(message);
					break;
				case RmlUiNet.LogType.Error:
					Debug.LogError(message);
					break;
				case RmlUiNet.LogType.Assert:
					Debug.LogAssertion(message);
					break;
				case RmlUiNet.LogType.Always:
				case RmlUiNet.LogType.Info:
				case RmlUiNet.LogType.Debug:
					Debug.Log(message);
					break;
			}

			// don't stop execution as that will block Unity!
			return true;
		}
	}
}
