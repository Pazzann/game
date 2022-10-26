using Godot;
using System;
using Game.Main.Scripts.Player;


namespace Game.Main.Scripts
{
    public class HealthBar : ProgressBar
    {
        private Movement _player;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _player = GetNode<KinematicBody2D>("../../Player") as Movement;
            PercentVisible = true;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {

            Value = _player.Health;
        }
    }
}