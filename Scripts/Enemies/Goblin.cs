using Godot;
using System;
using Game.Main.Scripts.Bullets;

namespace Game.Main.Scripts.Enemies
{
	public class Goblin : BasicEnemy
	{
		private PackedScene _goblinBulletScene;
		private float timer = 1.0f;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			health = 1000.0f;
			_goblinBulletScene = GD.Load<PackedScene>("res://Prefabs/GoblinBullet.tscn");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			timer -= delta;
			if (timer <= 0.0f)
			{
				GoblinBullet bullet = _goblinBulletScene.Instance() as GoblinBullet;
				bullet.Position = Position;
				GetParent().AddChild(bullet);
				bullet.SetVelocity();
				timer = 1.0f;
			}
		}
	}
}
