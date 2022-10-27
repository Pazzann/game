using System;
using Godot;
using Game.Main.Enums;
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

		private CollisionShape2D[] _attackCollision = new CollisionShape2D[8];
		private CollisionShape2D _playerCol;
		private PackedScene _dashGhostScene;

		public override void _Ready()
		{
			_attackCollision[(int)AttackPositions.UpLeft] = GetChild(3) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.Up] = GetChild(4) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.UpRight] = GetChild(5) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.Right] = GetChild(6) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.DownRight] = GetChild(7) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.Down] = GetChild(8) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.DownLeft] = GetChild(9) as CollisionShape2D;
			_attackCollision[(int)AttackPositions.Left] = GetChild(10) as CollisionShape2D;
			foreach (var shape2D in _attackCollision)
			{
				shape2D.Disabled = true;
			}
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

			if (Input.IsActionPressed("sword_attack"))
			{
				var collision = _attackCollision[calculateAttackPosition()];
				collision.Disabled = false;
			}

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
			GD.Print(GetGlobalMousePosition().y);
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
		private int calculateAttackPosition()
		{

			// y = kx + m
			// (-k)x + (-b)y  = 0
			// dis = abs(ax0 + by0) / sqrt(a*a + b*b)			
			
			var mousePos = GetGlobalMousePosition();
			mousePos = mousePos - Position;
			if (0 <= mousePos.x)
			{
				if (0 <= mousePos.y)
				{
					//4
					float dis1 = Math.Abs(mousePos.y);
					float dis2 = Math.Abs((-1.0f) * mousePos.y + (-1.0f) * mousePos.x) / (float)Math.Sqrt(2.0f);
					float dis3 = Math.Abs(mousePos.x);
					if (dis1 <= Math.Min(dis2, dis3))
						return (int)AttackPositions.Right;
					if (dis2 <= Math.Min(dis1, dis3))
						return (int)AttackPositions.DownRight;
					if (dis3 <= Math.Min(dis2, dis1))
						return (int)AttackPositions.Down;
				}
				else
				{
					//1
					float dis1 = Math.Abs(mousePos.x);
					float dis2 = Math.Abs((-1.0f) * mousePos.y + mousePos.x) / (float)Math.Sqrt(2.0f);
					float dis3 = Math.Abs(mousePos.y);
					if (dis1 <= Math.Min(dis2, dis3))
						return (int)AttackPositions.Up;
					if (dis2 <= Math.Min(dis1, dis3))
						return (int)AttackPositions.UpRight;
					if (dis3 <= Math.Min(dis2, dis1))
						return (int)AttackPositions.Right;
				}
			}
			else
			{
				if (0 <= mousePos.y)
				{
					//3
					float dis1 = Math.Abs(mousePos.x);
					float dis2 = Math.Abs((-1.0f) * mousePos.y + mousePos.x) / (float)Math.Sqrt(2.0f);
					float dis3 = Math.Abs(mousePos.y);
					if (dis1 <= Math.Min(dis2, dis3))
						return (int)AttackPositions.Down;
					if (dis2 <= Math.Min(dis1, dis3))
						return (int)AttackPositions.DownLeft;
					if (dis3 <= Math.Min(dis2, dis1))
						return (int)AttackPositions.Left;
				}
				else
				{
					//2
					float dis1 = Math.Abs(mousePos.y);
					float dis2 = Math.Abs((-1.0f) * mousePos.y + (-1.0f) * mousePos.x) / (float)Math.Sqrt(2.0f);
					float dis3 = Math.Abs(mousePos.x);
					if (dis1 <= Math.Min(dis2, dis3))
						return (int)AttackPositions.Left;
					if (dis2 <= Math.Min(dis1, dis3))
						return (int)AttackPositions.UpLeft;
					if (dis3 <= Math.Min(dis2, dis1))
						return (int)AttackPositions.Up;
				}
			}
			return 0;
		}
	}
}
