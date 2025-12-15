using Godot;
using System;

// Author: Korostelev A.
// Реализация случайной смены управления
public partial class SwapManager : Node2D
{

	private Random Rand;
	private double CountDown = 5.0f;
	private bool SwapShow = false;

	[Export] public Sprite2D _swap;
	[Export] public Player _player;
	[Export] public Player _player2;
	public override void _Ready()
	{
		Rand = new Random();
		_player = GetTree().Root.FindChild("Player", true, false) as Player;
		_player2 = GetTree().Root.FindChild("Player2", true, false) as Player2;
		_swap = GetTree().Root.FindChild("Swap", true, false) as Sprite2D;
	}

	public override void _Process(double delta)
	{
		if (GetTree().Paused)
		{
			if (SwapShow)
			{
				System.Threading.Thread.Sleep(1000);
				_swap.Visible = false;
				GetTree().Paused = false;
				SwapShow = false;
			}
			else
			{
				return;
			}
		}


		if (CountDown <= 0)
		{
			double randomNumber = Rand.NextDouble() * 25;
			CountDown = randomNumber;

			_player.Swap = !_player.Swap;
			_player2.Swap = !_player2.Swap;

			_swap.Visible = true;
			GetTree().Paused = true;
			SwapShow = true;
		}
		else
		{
			CountDown -= delta;
		}


	}
}
