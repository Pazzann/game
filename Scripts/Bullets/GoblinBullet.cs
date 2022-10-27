using Godot;
using System;
using Game.Main.Scripts.Player;

namespace Game.Main.Scripts.Bullets
{
	public class GoblinBullet : KinematicBody2D
	{
		[Export] private int Speed = 200;

		private Vector2 _velocity = new Vector2();
		private float _bulletSpeed = 1.0f;

		private Movement _player;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_player = GetNode<KinematicBody2D>("../Player") as Movement;
		}

		public void SetVelocity()
		{
			if (_player == null)
				return;
			Vector2 difference = this.Position - _player.Position;
			difference = difference.Normalized();
			_velocity.x -= difference.x * _bulletSpeed; 
			_velocity.y -= difference.y * _bulletSpeed; 
			//_velocity = _velocity.Normalized() * Speed;
		}


		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			var collision = MoveAndCollide(_velocity);
			if (collision != null && ((Node)collision.Collider).Name == "Player")
			{
				QueueFree();
			}
			if (collision != null)
			{
				if (((Node)collision.Collider).Name == "Player")
				{
					_player.Hit(5.0f);
				}
			}

		}
		
		
		
	}
}
