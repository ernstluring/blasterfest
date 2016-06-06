using UnityEngine;

public class FpsCounter : MonoBehaviour
{
	public int AverageFPS { get; private set; }

	public int HighestFPS { get; private set; }
	public int LowestFPS { get; private set; }

	public int smoothingFrameRange = 60;
	private int[] _fpsBuffer;
	private int _fpsBufferIndex;

	private void Update ()
	{
		if (_fpsBuffer == null || _fpsBuffer.Length != smoothingFrameRange)
		{
			InitializeBuffer ();
		}
		UpdateBuffer ();
		CalculateFPS ();
	}

	private void InitializeBuffer ()
	{
		if (smoothingFrameRange <= 0)
			smoothingFrameRange = 1;
		_fpsBuffer = new int[smoothingFrameRange];
		_fpsBufferIndex = 0;
	}

	private void UpdateBuffer ()
	{
		_fpsBuffer [_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (_fpsBufferIndex >= smoothingFrameRange)
		{
			_fpsBufferIndex = 0;
		}
	}

	private void CalculateFPS ()
	{
		int sum = 0;
		int highest = 0;
		int lowest = int.MaxValue;
		for (int i = 0; i < smoothingFrameRange; i++)
		{
			int fps = _fpsBuffer[i];
			sum += _fpsBuffer [i];
			if (fps > highest) highest = fps;
			if (fps < lowest) lowest = fps;
		}
		AverageFPS = (int)((float)sum / smoothingFrameRange);
		HighestFPS = highest;
		LowestFPS = lowest;
	}
}
