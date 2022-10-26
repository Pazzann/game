using Godot;
using System;
using Game.Main.Scripts.Player;

namespace Game.Main.Scripts.Bullets
{
	public class GoblinBullet : KinematicBody2D
	{
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
			Vector2 difference = Position - _player.Position;
			_velocity.x = _bulletSpeed * (difference.x / (difference.x + difference.y));
			_velocity.y = _bulletSpeed * (difference.y / (difference.x + difference.y));
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
