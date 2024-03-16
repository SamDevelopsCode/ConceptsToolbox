using Godot;
using System;

public partial class MainControls : HBoxContainer
{
	[ExportCategory("Main App Buttons")]
	[Export] private Button _menuButton;
	[Export] private AnimationPlayer _animPlayer;
	[Export] private PanelContainer _sidebar;
	[Export] private Button _minimizeButton;
	[Export] private Button _maximizeButton;
	[Export] private Button _quitButton;
	
	
	public override void _Ready()
	{
		_menuButton.Pressed += OnMenuButtonPressed;
		_minimizeButton.Pressed += OnMinimizePressed;
		_maximizeButton.Pressed += OnMaximizePressed;
		_quitButton.Pressed += OnQuitPressed;
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
