using Godot;
using System;

public partial class ContentPage : MarginContainer
{
	[Export] private RichTextLabel _title;
	[Export] public VBoxContainer Vbox;
	
	public override void _Ready()
	{
		if (Name == "DotNet")
		{
			_title.Text = "[center][u].NET[/u][/center]";
		}
		else
		{
			_title.Text = $"[center][u]{Name}[/u][/center]";
		}
	}
	
}
