using Godot;
using System;


namespace Game.Main.Scripts
{
	public class Camera : Camera2D
	{
		private KinematicBody2D _Player;
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this._Player = GetNode<KinematicBody2D>("../Player");
			this.Current = true;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			this.Position = _Player.Position;
		}
	}
}
