using Godot;
using System;
using Game.Main.Scripts.Bullets;

namespace Game.Main.Scripts.Player
{
	public class Movement : KinematicBody2D
	{
		[Export] private int Speed = 200;

		private Vector2 _velocity = new Vector2();
		public float Health = 100.0f;

		private bool _dodgePos = true;
		private int _dodgeTimes = 0;
		public float DodgeCoolDown = 0.0f;
		private bool _dodging = false;

		private CollisionShape2D _playerCol;

		private PackedScene _dashGhostScene;

		public override void _Ready()
		{
			_playerCol = GetChild(0) as CollisionShape2D;
			_dashGhostScene = GD.Load<PackedScene>("res://Prefabs/DashGhost.tscn");
		}

		public void GetInput()
		{
			_velocity = new Vector2();

			if (Input.IsActionPressed("right"))
				_velocity.x += 1;

			if (Input.IsActionPressed("left"))
				_velocity.x -= 1;

			if (Input.IsActionPressed("down"))
				_velocity.y += 1;

			if (Input.IsActionPressed("up"))
				_velocity.y -= 1;

			_velocity = _velocity.Normalized() * Speed;

			if (DodgeCoolDown < 0)
				_dodgePos = true;

			if (Input.IsActionPressed("dodge") && _dodgePos)
				_dodging = true;

			if (_dodging)
			{
				if (Input.IsActionPressed("up") && Input.IsActionPressed("left"))
				{
					_velocity.x -= 714.2f;
					_velocity.y -= 714.2f;
				}

				else if (Input.IsActionPressed("up") && Input.IsActionPressed("right"))
				{
					_velocity.x += 714.2f;
					_velocity.y -= 714.2f;
				}

				else if (Input.IsActionPressed("down") && Input.IsActionPressed("left"))
				{
					_velocity.x -= 714.2f;
					_velocity.y += 714.2f;
				}

				else if (Input.IsActionPressed("down") && Input.IsActionPressed("right"))
				{
					_velocity.x += 714.2f;
					_velocity.y += 714.2f;
				}

				else if (Input.IsActionPressed("right"))
					_velocity.x += 1000;

				else if (Input.IsActionPressed("left"))
					_velocity.x -= 1000;

				else if (Input.IsActionPressed("down"))
					_velocity.y += 1000;

				else if (Input.IsActionPressed("up"))
					_velocity.y -= 1000;
			}
		}

		public void Hit(float damage)
		{
			Health -= damage;
		}
		public override void _PhysicsProcess(float delta)
		{
			GetInput();
			_playerCol.Disabled = _dodging;
			_velocity = MoveAndSlide(_velocity);
			
			for (int i = 0; i < GetSlideCount(); i++)
			{
				var collision = GetSlideCollision(i);
				if (((Node)collision.Collider) is BasicBullet bullet)
				{
					bullet.QueueFree();
					Hit(bullet.Damage);
				}
			}
			
			DodgeCoolDown -= delta;
			if (_dodging)
			{
				_dodgeTimes++;
				if (_dodgeTimes % 3 == 1)
				{
					Sprite dashGhost = _dashGhostScene.Instance() as Sprite;
					dashGhost.Position = new Vector2(Position.x - 16.0f, Position.y - 16.0f);
					GetParent().AddChild(dashGhost);
				}
				

			}

			if (_dodgeTimes >= 10)
			{
				_dodging = false;
				_dodgeTimes = 0;
				DodgeCoolDown = 1.0f;
				_dodgePos = false;
			}
		}
	}
}
