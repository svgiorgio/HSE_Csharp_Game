using Godot;
using System;

// Author: Korostelev A.
public partial class GameRestart : Button
{
	public override void _Ready()
	{
	}

	public override void _Pressed()
	{
		Player.ResetKills();
		Rabbit.ResetSpeed();
		base._Pressed();
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

}
