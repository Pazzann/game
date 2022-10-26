using Godot;
using System;

namespace Game.Main.Scripts.Player
{
	public class DashGhost : Sprite
	{
		private Tween _tween;
		// Called when the node enters the scene tree for the first time.
		public override async void _Ready()
		{
			_tween = GetChild(0) as Tween;
			if (_tween == null)
				return;
			_tween.InterpolateProperty(this, "self_modulate", new Color(2.0f, 2.0f, 2.0f, 1.0f), new Color(2.0f, 2.0f, 2.0f, 0.0f), 0.5f, Tween.TransitionType.Quart, Tween.EaseType.Out);
			_tween.Start();
			await ToSignal(_tween, "tween_completed");
			QueueFree();
		}
		//
		// public override void _TweenAllCompleted(object obj, NodePath key)
		// {
		//     
		// }
		// // Called every frame. 'delta' is the elapsed time since the previous frame.
		// public override void _Process(float delta)
		// {
		// }
	}
}
