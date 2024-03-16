using Godot;
using System;
using System.Collections.Generic;

public partial class ConceptInfoLoader : VBoxContainer
{
	[Export] private PackedScene _typesInfoScene;
	[Export] private PackedScene _dotnetInfoScene;
	
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _content;

	

	public override void _Ready()
	{
		foreach (Button child in GetChildren())
		{
			child.Text = child.Name;
			child.Pressed += () => OnButtonPressed(child);
		}
	}

	
	private async void OnButtonPressed(Button button)
	{
		if (_content.GetChildCount() > 0)
		{
			var currentInfoContainer = _content.GetChild(0);
			if (currentInfoContainer.Name == button.Name)
			{
				return;
			}

			var animPlayer = currentInfoContainer.GetNode<AnimationPlayer>("AnimPlayer");
			
			animPlayer.PlayBackwards("expand");
			await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
			currentInfoContainer.QueueFree();
		}
		
		Node infoContainerInstance;
		switch (button.Name)
		{ 
			case "Types":
				infoContainerInstance = _typesInfoScene.Instantiate();
				_content.AddChild(infoContainerInstance);
				break;
			case "DotNet":
				infoContainerInstance = _dotnetInfoScene.Instantiate();
				_content.AddChild(infoContainerInstance);
				break;
		};
	}
	
	
}
