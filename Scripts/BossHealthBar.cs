using Godot;
using System;
using Game.Main.Scripts.Enemies;


namespace Game.Main.Scripts
{
	public class BossHealthBar : ProgressBar
	{
		[Export] 
		public BasicEnemy Boss;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			foreach (Node child in GetParent().GetParent().GetChildren())
			{
				if (child is BasicEnemy enemy)
				{
					MaxValue = enemy.health;
					Boss = enemy;
				}
			}
			PercentVisible = true;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			Value = Boss.health;
		}
	}
}
