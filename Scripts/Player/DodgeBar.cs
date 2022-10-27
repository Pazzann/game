using Godot;
using System;



namespace Game.Main.Scripts.Player
{
	public class DodgeBar : ProgressBar
	{
		private Movement _player;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_player = GetNode<KinematicBody2D>("../../Player") as Movement;
			PercentVisible = false;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			Visible = !(_player.DodgeCoolDown < 0);

			Value = 100.0f - _player.DodgeCoolDown * 100;
		}
	}
}
