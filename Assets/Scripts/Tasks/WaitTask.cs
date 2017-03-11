using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitTask : Task {

	private static readonly DateTime UnixEpoch = new DateTime(1970,1,1);

	private static double GetTimestamp(){
		return (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
	}

	private readonly double duration;
	private double startTime;

	public WaitTask(double dur){
		duration = dur;
	}

	protected override void Init ()
	{
		startTime = GetTimestamp ();
	}

	internal override void Update ()
	{
		double now = GetTimestamp ();
		bool durationElapsed = (now - startTime) > duration;
		if (durationElapsed) {
			SetStatus (TaskStatus.Success);
		}
	}
}
