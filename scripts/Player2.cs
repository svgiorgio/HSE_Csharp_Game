using Godot;
using System;

// Author: Korostelev A.
// Реализация второго игрока. Наследует общее поведение от Player, но изменяет передвижение и способность
public partial class Player2 : Player
{
	[Export] public float _speed = 450.0f;
	private float DashCooldown = 0.0f;
	private const float DashDelay = 3.0f;
	private const float Normal_speed = 450.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction;


		if (Swap) {
			direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		}
		else {
			direction = Input.GetVector("alt_left", "alt_right", "alt_up", "alt_down");
		}

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Y = direction.Y * _speed;
			Bunny.Play("move");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, _speed);
			Bunny.Play("idle");
		}

		if (direction.X < 0)
		{
			Bunny.FlipH = false;
		}
		else
		{
			Bunny.FlipH = true;
		}

		if (DashCooldown > -1)
		{
			DashCooldown -= (float)delta;
		}

		if (DashCooldown <= DashDelay/2)
		{
			_speed = Normal_speed;
		}


		if (Input.IsActionPressed("dash") && (DashCooldown <= 0))
		{
			_speed = Normal_speed * 2;
			DashCooldown = DashDelay;
		}

		Velocity = velocity;
		MoveAndSlide();

	}

}
