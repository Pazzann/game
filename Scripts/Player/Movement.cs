using Godot;
using System;

namespace Game.Main.Scripts.Player{

public class Movement : KinematicBody2D
{
	[Export] public int speed = 200;

	public Vector2 velocity = new Vector2();

	public bool dodgePos = true;

	public float dodgeCoolDown = 0.0f;

	public void GetInput(){
		velocity = new Vector2();

		if (Input.IsActionPressed("right"))
			velocity.x += 1;

		if (Input.IsActionPressed("left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("down"))
			velocity.y += 1;

		if (Input.IsActionPressed("up"))
			velocity.y -= 1;

		velocity = velocity.Normalized() * speed;

		if(dodgeCoolDown < 0)
			dodgePos = true;

		if (dodgePos){
			if (Input.IsActionPressed("dodge")){
				if (Input.IsActionPressed("up") && Input.IsActionPressed("left")){
					velocity.x -= 7142;
					velocity.y -= 7142;
				}

				else if (Input.IsActionPressed("up") && Input.IsActionPressed("right")){
					velocity.x += 7142;
					velocity.y -= 7142;
				}

				else if (Input.IsActionPressed("down") && Input.IsActionPressed("left")){
					velocity.x -= 7142;
					velocity.y += 7142;
				}

				else if (Input.IsActionPressed("down") && Input.IsActionPressed("right")){
					velocity.x += 7142;
					velocity.y += 7142;
				}

				else if (Input.IsActionPressed("right"))
					velocity.x += 10000;

				else if (Input.IsActionPressed("left"))
					velocity.x -= 10000;

				else if (Input.IsActionPressed("down"))
					velocity.y += 10000;

				else if (Input.IsActionPressed("up"))
					velocity.y -= 10000;

				dodgeCoolDown = 1.0f;
				dodgePos = false;
			}
		}
	}

	public override void _PhysicsProcess(float delta){
		GetInput();
		velocity = MoveAndSlide(velocity);
		dodgeCoolDown -= delta;
	}
}

}
