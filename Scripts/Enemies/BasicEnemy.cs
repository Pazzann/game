using Godot;
using System;

namespace Game.Main.Scripts.Enemies
{
    public class BasicEnemy : KinematicBody2D
    {
        public byte bossStage = 0;
        public float health = 0.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
           
        }
        
    }

}
