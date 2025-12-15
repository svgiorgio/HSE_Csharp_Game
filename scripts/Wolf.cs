using Godot;
using Godot.NativeInterop;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

public partial class Wolf : CharacterBody2D
{


	public const float Speed = 50.0f;
	public const float ExploisonRadius = 150.0f;
	public const int ExploisonDamage = 20;

	public int WolfHealth = 20;

	private bool chaseState = false;

	private bool isExploiding = false;

	private CollisionShape2D _collisionShape;

	[Export] public AnimatedSprite2D WolfAnim;
	[Export] public Player _player;


	public override void _Ready()
	{
		AddToGroup("enemies");
		Area2D _detect = GetNode<Area2D>($"./DetectorW");
		_detect.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBodyEntered));

		Area2D _detect_boom = GetNode<Area2D>($"./Detector_BOOMW");
		_detect_boom.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));

		//Area2D _detect_player = GetNode<Area2D>($"../Player");
		//_detect_player.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));

		_player = GetTree().Root.FindChild("Player", true, false) as Player;

		_collisionShape = GetNode<CollisionShape2D>($"./CollisionShape2DW");
		WolfAnim.Play("run");
	}
	public override void _PhysicsProcess(double delta)
	{
		if (isExploiding) { return; }

		CharacterBody2D _player = GetNode<CharacterBody2D>($"../Player");

		Vector2 velocity = Velocity;

		Vector2 direction;
		direction.X = Math.Sign(_player.Position.X - this.Position.X);
		direction.Y = Math.Sign(_player.Position.Y - this.Position.Y);
		if ((direction != Vector2.Zero) && (chaseState))
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}

		if ((direction.X < 0) || (Math.Abs(_player.Position.X - this.Position.X) < 10))
		{
			WolfAnim.FlipH = true;
		}
		else
		{
			WolfAnim.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();

	}
	private void OnBodyEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body is Player)
		{
			chaseState = true;
		}
	}
	private async void OnBoomEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body is Player)
		{

			((Player)body).BunnyTakeDamage(20);
			WolfAnim.Play("bite");
			await ToSignal(WolfAnim, "animation_finished");
			isExploiding = true;
			Death();
		}
	}
	public void WolfTakeDamage()
	{
		WolfHealth = WolfHealth - 10;
		if (WolfHealth == 0)
		{
			Death();
		}
	}

	public async void Death()
	{

		WolfAnim.Play("death");
		//GetNode<AudioStreamPlayer2D>("ShotSound").Play();
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		await ToSignal(WolfAnim, "animation_finished");
		QueueFree();
	}

	public void VarDeath(Vector2 coord, float radius)
	{
		if (coord.DistanceTo(GlobalPosition) < radius)
		{
			this.WolfTakeDamage();
		}
	}

}
