using Godot;
using System;

public partial class MainControls : HBoxContainer
{
	[ExportCategory("Main App Buttons")]
	[Export] private Button _menuButton;
	[Export] private AnimationPlayer _animPlayer;
	[Export] private Panel _sidebar;
	[Export] private Button _minimizeButton;
	[Export] private Button _maximizeButton;
	[Export] private Button _quitButton;

	[ExportCategory("Concept Buttons")] 
	[Export] private Button _typesButton;

	[ExportCategory("Main Content")]
	[Export] private VBoxContainer _content;


	
	public override void _Ready()
	{
		_menuButton.Pressed += OnMenuButtonPressed;
		_minimizeButton.Pressed += OnMinimizePressed;
		_maximizeButton.Pressed += OnMaximizePressed;
		_quitButton.Pressed += OnQuitPressed;
		_typesButton.Pressed += OnTypesButtonPressed;
	}

	
	private void OnTypesButtonPressed()
	{
		PackedScene infoContainerScene = GD.Load<PackedScene>("res://scenes/info_container.tscn");
		Node infoContainerInstance = infoContainerScene.Instantiate();
		_content.AddChild(infoContainerInstance);
	}


	private void OnQuitPressed()
	{
		GetTree().Quit();
	}

	
	private void OnMaximizePressed()
	{
		if (DisplayServer.WindowGetMode(0) == DisplayServer.WindowMode.ExclusiveFullscreen)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
		}
	}

	
	private void OnMinimizePressed()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Minimized);
	}


	private void OnMenuButtonPressed()
	{
		if (_sidebar.CustomMinimumSize == Vector2.Zero)
		{
			_animPlayer.Play("open_sidebar");
		}
		else
		{
			_animPlayer.PlayBackwards("open_sidebar");
		}
	}
}
