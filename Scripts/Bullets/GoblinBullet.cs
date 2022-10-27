using Godot;
using System;
using Game.Main.Scripts.Player;

namespace Game.Main.Scripts.Bullets
{
	public class GoblinBullet : BasicBullet
	{

		private Vector2 _velocity = new Vector2();
		[Export] 
		public float BulletSpeed = 2.0f;

		private Movement _player;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Damage = 5.0f;
			_player = GetNode<KinematicBody2D>("../Player") as Movement;
		}

		public void SetVelocity()
		{
			if (_player == null)
				return;
			Vector2 difference = this.Position - _player.Position;
			difference = difference.Normalized();
			_velocity.x -= difference.x * BulletSpeed; 
			_velocity.y -= difference.y * BulletSpeed;
		}

		public void SetVelocity2(Vector2 velocity)
		{
			_velocity = velocity;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			var collision = MoveAndCollide(_velocity);
			if (collision != null && ((Node)collision.Collider).GetType() == typeof(Movement))
			{
				_player.Hit(Damage);
				QueueFree();
			}
		}
	}
}
